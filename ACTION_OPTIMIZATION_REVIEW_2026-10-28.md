# Action 及相关代码优化审查记录（1–52）

记录日期：2026-10-28。

## 范围与状态

- 本文件保存本轮聊天已经输出的 **1–52 条**，编号固定，不混入此前其他轮次的编号，也不加入尚未输出的线索。
- **全部为代码阅读结论；本轮没有修改实现，没有运行构建、功能测试或性能测试。** 以下“收益”是机制分析或复杂度判断，不是实测结果。
- 全部条目目前统一标记为待评估、未实施、未测试；部分涉及正确性、资源生命周期或行为兼容，不能作为纯性能替换直接合并。
- 对照的是本地 `new` 分支当前工作区，含未提交的 Action 重构及编辑器修改。记录时 HEAD：`684ba61a89a9561d8057103397e3d3975256f73f`。**HEAD 本身不包含全部审查对象，后续应以用户最新源码为准。**
- 位置同时记录文件、行号和方法/代码标记；行号为记录时位置，代码移动后优先按方法名或标记重新定位。
- 本文件仅记录，本轮不自动实施建议。后续可按编号核对、讨论、实施及更新状态。

## 分类索引

| 类别 | 编号 |
| --- | --- |
| 排队与执行 | [1](#item-1)、[2](#item-2)、[3](#item-3)、[6](#item-6)、[17](#item-17)、[20](#item-20)、[31](#item-31)、[41](#item-41)、[42](#item-42)、[43](#item-43) |
| 变量、排序与文件 | [4](#item-4)、[5](#item-5)、[9](#item-9)、[18](#item-18)、[21](#item-21)、[32](#item-32)、[33](#item-33)、[35](#item-35)、[38](#item-38)、[52](#item-52) |
| 编辑器与配置 | [7](#item-7)、[8](#item-8)、[10](#item-10)、[11](#item-11)、[12](#item-12)、[14](#item-14)、[15](#item-15)、[16](#item-16)、[19](#item-19)、[27](#item-27)、[34](#item-34)、[36](#item-36)、[37](#item-37)、[39](#item-39)、[44](#item-44)、[45](#item-45)、[48](#item-48)、[49](#item-49) |
| 原生调用与声音 | [13](#item-13)、[40](#item-40)、[50](#item-50)、[51](#item-51) |
| 悬浮窗运行时 | [22](#item-22)、[23](#item-23)、[24](#item-24)、[25](#item-25)、[26](#item-26)、[28](#item-28)、[29](#item-29)、[30](#item-30)、[46](#item-46)、[47](#item-47) |

## 后续约束

- 保持实现简洁，优先复用现有逻辑；不为小收益引入新的缓存或管理框架。
- 实体相关优化、普通 JSON 检查及此前明确暂缓的项目不因本清单而恢复。
- 第 1、3、6、20、33、41–43、46–47 等涉及并发、顺序或生命周期的建议，实施时须核对原有语义。
- 第 14、39、49 条涉及相近的批量编辑入口，可以合并实施，但应分别记录完成情况；其他相关条目也保留原编号。

<a id="item-1"></a>

## 1. 批量取消动作逐项 Remove，存在平方级删除开销

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:57](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:57) — `CancelQueuedActions`。
- **现状：** 先 Where(...).ToList() 收集目标，再逐项调用 ActionQueue.Remove；每次删除都可能重新查找并搬移后续元素。
- **建议：** 改为统一扫描、批量删除，保留被删除实例以完成现有互斥锁释放或交接。
- **收益与边界：** 删除部分可由最坏 O(N²) 降为 O(N)；不能省略互斥锁处理，也应确认过滤器异常时的行为。

<a id="item-2"></a>

## 2. 入队序号存在重复加锁

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:159](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:159) — `QueueAction(ActionInstance, Trigger)：curOrdinal++`。
- **现状：** 外层已经持有 ActionQueue 锁，增加 curOrdinal 时又 lock(this)；当前代码中序号写入集中在这里。
- **建议：** 直接由现有队列锁保护序号，移除内部插件实例锁。
- **收益与边界：** 性能收益很小，主要减少锁层次；修改前再次搜索所有 curOrdinal 写入。

<a id="item-3"></a>

## 3. 执行集合重复登记，额外获取队列锁

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:232](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:232) — `ExecuteNextQueuedAction`。
  - [Source/Triggernometry/Core/ActionBase.cs:440](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:440) — `Execute(ActionInstance, Context)`。
- **现状：** 调度线程取出动作时已将实例加入 ExecutingActions；Execute 随后再次获取队列锁，向同一个 HashSet 添加同一实例。
- **建议：** 保留出队与执行登记的原子性，让正常调度路径只登记一次；单独考虑公开 Execute 的直接调用。
- **收益与边界：** 减少每个排队动作的一次锁操作及重复 HashSet 查找；不能制造出队后暂未登记的取消窗口。

<a id="item-4"></a>

## 4. 按列排序会为每一行重新排序一次

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableTable.cs:921](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableTable.cs:921) — `ExecuteImplementation：SortLine 的列排序分支`。
  - [Source/Triggernometry/Core/ActionBase.cs:565](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:565) — `ApplySorting`。
- **现状：** ApplySorting 返回延迟执行的 IOrderedEnumerable；每一行枚举 sortedIndices 时，都会再次执行相同排序。
- **建议：** 先将排序后的列索引物化一次，再让所有行复用同一份顺序。
- **收益与边界：** 排序工作从 H 次 W log W 降为一次 W log W；单元格搬移仍为 H×W。保持稳定排序及比较规则。

<a id="item-5"></a>

## 5. 数值排序键经历数字到文本再到数字的转换

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:703](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:703) — `SortByKeys：生成排序键`。
  - [Source/Triggernometry/Core/Actions/ActionVariableTable.cs:913](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableTable.cs:913) — `SortLine：生成排序键`。
  - [Source/Triggernometry/Core/ActionBase.cs:565](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:565) — `ApplySorting / ParseSortKeyFunctions`。
- **现状：** 数值表达式先得到数字，又经 ThingToString 存成字符串，排序阶段再转换为数字；类型、方向、表达式和值还分散在四份平行列表中。
- **建议：** 数值键保留数值类型，文本键保留字符串；以一份排序键描述承载相关设置，避免平行列表不同步。
- **收益与边界：** 减少转换和字符串分配，主要影响大列表、多列、多关键字排序；须保持原有数值范围、字符串比较与稳定性。

<a id="item-6"></a>

## 6. 鼠标点击自行 Task.Run，绕过通用同步执行语义

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionMouse.cs:177](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionMouse.cs:177) — `ExecuteImplementation：三个 Click 分支`。
  - [Source/Triggernometry/Core/ActionBase.cs:440](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:440) — `Execute`。
- **现状：** 点击分支各自 Task.Run，并在内部移动、按下、等待和抬起；即使公共异步选项关闭，动作实现也会提前返回。
- **建议：** 由 ActionBase 统一决定异步与否；点击过程在实现内部顺序执行，并复用仅按钮标志不同的三个分支。
- **收益与边界：** 减少重复任务调度并修正完成时机；涉及同步语义，后续必须单独验证顺序动作，不能视作纯性能改动。

<a id="item-7"></a>

## 7. 两处测试动作逻辑重复，忽略条件选项行为不一致

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:539](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:539) — `ctxTest_Click / ctxTestLiveIgnoreCnd`。
  - [Source/Triggernometry/UI/Forms/ActionForm.cs:291](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/ActionForm.cs:291) — `TestAction`。
- **现状：** ActionViewer 的显式“真实值、忽略条件”仍依赖全局 TestIgnoreConditionsByDefault；新 ActionForm 的同名显式选项会直接忽略条件。
- **建议：** 共用测试动作的上下文与执行入口，明确区分默认测试选项和用户显式选择。
- **收益与边界：** 主要改善正确性和复用；保留真实值、占位值、声音/TTS hook 及按键测试延迟等差异。

<a id="item-8"></a>

## 8. 修改一个字体预览字段会重新计算整份预览

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.Font.cs:68](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.Font.cs:68) — `UpdateOverlayFontPreview`。
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.cs:212](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.cs:212) — `字体相关 ExpressionTextBox.TextChanged`。
- **现状：** 颜色文本每次改变，都会重新计算字号、三种颜色、字体摘要及预览尺寸；不相关字段之间存在重复工作。
- **建议：** 拆开字体相关刷新和单个颜色刷新，仅在首次绑定或完整重载时刷新全部预览。
- **收益与边界：** 减少输入颜色表达式时的重复解析和布局，不缓存运行时表达式的长期结果。

<a id="item-9"></a>

## 9. 列表复制动作没有复用现有深复制实现

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:727](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:727) — `ExecuteImplementation：Copy`。
  - [Source/Triggernometry/Core/Variables/VariableList.cs:80](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Variables/VariableList.cs:80) — `Duplicate / Push`。
- **现状：** 复制动作逐个 Duplicate 后调用 Push；每个元素都会重复更新目标容器的修改时间和来源。
- **建议：** 复用已经直接添加元素的 VariableList.Duplicate，再一次性设置目标容器的来源与时间。
- **收益与边界：** 保持逐项深复制，减少重复元数据更新；空列表与嵌套值的元数据语义需核对。

<a id="item-10"></a>

## 10. 帮助内容未变化也重新加载全文并着色

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.cs:38](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.cs:38) — `configurationEditor.ValueChanged`。
  - [Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:55](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:55) — `RichTextBoxHelper.UpdateText`。
- **现状：** 持久变量开关、表达式类型等改变时也调用帮助 UpdateText；它会重新读取文本、整段替换并重新着色。
- **建议：** 仅在帮助键、语言或展开状态变化时更新正文；布局变化只更新高度。
- **收益与边界：** 减少无效资源读取、文本处理和原生控件操作；不同动作子类型的帮助切换必须保留。

<a id="item-11"></a>

## 11. 帮助文本逐字符着色成本高，并有未释放字体

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:121](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:121) — `RichTextBoxHelper.SetStyles`。
  - [Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:151](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:151) — `monospaceFont 创建`。
- **现状：** 多次扫描 Text、逐字符 Substring/Select/SelectionColor，产生大量字符串或原生控件调用；创建的 Consolas 字体没有明确释放。
- **建议：** 先读取本地字符串，计算连续样式区间，再按区间设置样式；明确字体的持有与释放。
- **收益与边界：** 长帮助文本更有价值；保持表达式、引号、等宽标记和分隔符之间的着色优先关系。

<a id="item-12"></a>

## 12. 编辑器和 XML 使用不同的动作类型清单

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Forms/ActionForm.cs:15](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/ActionForm.cs:15) — `静态 actionTypes`。
  - [Source/Triggernometry/Core/Serialization/ActionXmlCollection.cs:16](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Serialization/ActionXmlCollection.cs:16) — `serializers / actionTypes`。
  - [Source/Triggernometry/Core/ActionBase.cs:27](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:27) — `XmlInclude 声明`。
- **现状：** 编辑器扫描程序集里的带属性子类，XML 则读取 XmlInclude；新增动作可能只出现在其中一边。
- **建议：** 复用现有 XmlInclude 声明作为共享类型来源，再分别建立 UI 与 XML 所需映射。
- **收益与边界：** 主要减少维护遗漏，初始化收益较小；保留类型顺序、分类及元素名。

<a id="item-13"></a>

## 13. 启动程序动作没有及时释放 Process

- **类别 / 状态：** 原生调用与声音；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionLaunchProcess.cs:126](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionLaunchProcess.cs:126) — `ExecuteImplementation`。
- **现状：** new Process 后没有 Dispose；同步 WaitForExit 完成后也没有释放对象。
- **建议：** 用 using 包住现有启动及等待逻辑。
- **收益与边界：** 明确释放进程句柄等资源；Dispose 不会终止已启动的外部程序，保留当前同步/异步等待行为。

<a id="item-14"></a>

## 14. 撤销快照保存时机和覆盖范围不一致

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:283](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:283) — `StoreActions`。
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:414](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:414) — `PasteActions`。
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:578](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:578) — `批量条件及属性修改`。
- **现状：** 粘贴在解析前就深复制所有动作，失败也会覆盖上次撤销；批量修改条件、异步状态等又没有保存快照。
- **建议：** 在确定要修改数据后进入统一的“快照、修改、刷新通知”流程；粘贴先解析成功再保存快照。
- **收益与边界：** 避免失败粘贴的无效深复制，并使撤销一致；不能让取消、复制等非修改操作覆盖撤销。

<a id="item-15"></a>

## 15. 整行样式在每个单元格中重复计算

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:309](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:309) — `dgvActions_CellFormatting`。
- **现状：** 复选框列和描述列分别计算相同延迟警告色、背景表达式及文本颜色表达式。
- **建议：** 按行计算样式；空颜色表达式直接采用默认色。
- **收益与边界：** 减少滚动或刷新时的重复解析；保持动态颜色刷新，避免永久缓存结果。

<a id="item-16"></a>

## 16. 引用选择树先全量克隆再删除无关节点

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.cs:315](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.cs:315) — `CreateReferenceEditor`。
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.cs:329](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.cs:329) — `PrepareNode / TreeNode.Clone`。
- **现状：** 分组选择器先克隆所有触发器节点，再删除触发器；仓库选择也存在类似工作。
- **建议：** 复制过程中直接筛选需要的节点，保留 Tag、图标、层次和引用索引。
- **收益与边界：** 大配置可减少节点分配与遍历；缺失 GUID 的保留逻辑、仓库来源和可选节点限制不能丢失。

<a id="item-17"></a>

## 17. 动作上存在未接入执行流程的统计状态

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/ActionBase.cs:418](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:418) — `LastExecutionResult / LastExecutionTime / ExecutionCount`。
- **现状：** 当前仓库只找到这些成员的定义；没有实际读写入口，统计值不会更新。
- **建议：** 确认不存在外部反射依赖后删除无效状态。
- **收益与边界：** 减少对象体积和认知负担，不宣称明显执行速度收益；不擅自新增一套统计功能。

<a id="item-18"></a>

## 18. 变量修改来源文本重复生成逻辑可以复用

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableScalar.cs:264](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableScalar.cs:264) — `ExecuteImplementation：changer`。
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:431](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:431) — `changer`。
  - [Source/Triggernometry/Core/Actions/ActionVariableTable.cs:490](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableTable.cs:490) — `vtchanger`。
  - [Source/Triggernometry/Core/Actions/ActionVariableDict.cs:372](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableDict.cs:372) — `changer`。
  - [Source/Triggernometry/Core/Actions/ActionDiskOperation.cs:225](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionDiskOperation.cs:225) — `文件读入来源`。
- **现状：** 多种动作重复判断 ctx.Trigger 并生成同样的触发器/测试模式来源文本。
- **建议：** 在 ActionBase 中提供简短的来源生成方法，复用现有翻译键和文本格式。
- **收益与边界：** 主要减少重复维护；来源参与 StateForm 定位，不能当作可省略的普通调试日志。

<a id="item-19"></a>

## 19. 测试动作时控件值被写回两次

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Forms/ActionForm.cs:291](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/ActionForm.cs:291) — `TestAction`。
  - [Source/Triggernometry/UI/Forms/ActionForm.cs:228](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/ActionForm.cs:228) — `SettingsToAction / WriteValues`。
- **现状：** TestAction 先 TryWriteValues，随后 SettingsToAction 又 WriteValues。
- **建议：** 验证和写回只执行一次，再取得独立的测试动作副本。
- **收益与边界：** 减少重复转换与反射赋值；保留验证失败的提示和测试副本隔离。

<a id="item-20"></a>

## 20. 启用与禁用动作复制了整套 UI 更新逻辑

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionFolderOperation.cs:106](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionFolderOperation.cs:106) — `ExecuteImplementation：Disable / Enable`。
  - [Source/Triggernometry/Core/Actions/ActionTriggerOperation.cs:479](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionTriggerOperation.cs:479) — `EnableTrigger / DisableTrigger`。
- **现状：** 设置状态、Invoke、查找树节点、改变勾选及输出日志的分支成对重复。
- **建议：** 先计算 enabled，共用节点查找与状态更新；日志仍使用原有的启用/禁用翻译键。
- **收益与边界：** 主要改善可读性；保留当前线程切换、事件顺序以及分组变量与触发器启停的关联。

<a id="item-21"></a>

## 21. 远程资源缓存下载逻辑重复四份

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionDiskOperation.cs:175](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionDiskOperation.cs:175) — `远程文件读取`。
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Audio.cs:274](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Audio.cs:274) — `SoundPlaybackSmart`。
  - [Source/Triggernometry/UI/Forms/AuraContainerForm.cs:180](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/AuraContainerForm.cs:180) — `LoadImageData`。
  - [Source/Triggernometry/UI/Scarborough/ScarboroughImage.cs:216](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/ScarboroughImage.cs:216) — `LoadImageData`。
- **现状：** URL、哈希文件名、缓存检查、下载落盘的代码分散在文件、声音及两套图片悬浮窗中。
- **建议：** 抽出返回本地文件路径的小工具方法，由调用方传入目录、有效期、缓存策略和 User-Agent。
- **收益与边界：** 主要统一维护；不得悄悄改变各入口已有缓存策略，也不宣称下载网络本身会变快。

<a id="item-22"></a>

## 22. 悬浮窗预处理总会生成五组数字并替换占位符

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/ScarboroughItem.cs:157](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/ScarboroughItem.cs:157) — `PreprocessExpression`。
  - [Source/Triggernometry/UI/Forms/AuraContainerForm.cs:293](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/AuraContainerForm.cs:293) — `PreprocessExpression`。
- **现状：** 即使表达式没有位置等占位符，也生成数字字符串并执行多次 Replace。
- **建议：** 先跳过不含相关标记的表达式，命中时才转换相应数值；保留两种后端各自的当前值/待更新值语义。
- **收益与边界：** 位于持续更新路径，减少重复扫描和小分配；无需长期缓存表达式求值结果。

<a id="item-23"></a>

## 23. 重复激活文本悬浮窗时无条件重建字体

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:365](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:365) — `ActivateText`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:391](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:391) — `existing.NeedFont = true`。
  - [Source/Triggernometry/UI/Scarborough/ScarboroughText.cs:189](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/ScarboroughText.cs:189) — `LoadFontOnDemand`。
- **现状：** 更新已有悬浮窗后总设 NeedFont；下一次绘制释放并重建字体和画刷，哪怕只改文字或位置。
- **建议：** 赋值前比较字体名称、字号和样式，仅实际改变时标记字体重建。
- **收益与边界：** 减少频繁更新文本时的原生资源创建；新悬浮窗、字体变化和绘图资源重建仍需正确初始化。

<a id="item-24"></a>

## 24. 文本悬浮窗每次重绘都重新排版

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/ScarboroughText.cs:293](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/ScarboroughText.cs:293) — `Render：CreateTextLayout`。
  - [Source/Triggernometry/UI/Scarborough/Drawing/Graphics.cs:1766](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Drawing/Graphics.cs:1766) — `CreateTextLayout`。
- **现状：** 仅位置、颜色或透明度改变，也会创建并释放相同文字的 TextLayout。
- **建议：** 每个悬浮窗复用自己的排版对象；文字、字体、尺寸、对齐或相关资源改变时失效并重建。
- **收益与边界：** 长文本移动、淡入淡出更有价值，静止文本收益有限；必须管理排版对象的释放与失效条件。

<a id="item-25"></a>

## 25. 新版文本悬浮窗保留了不必要的同步 UI Invoke

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:448](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:448) — `SbTextAuraManagement`。
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:226](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:226) — `SbImageAuraManagement`。
- **现状：** Scarborough 文本管理先同步进入主 UI，再解析参数并向渲染队列提交；同后端图片路径没有这层切换。
- **建议：** 核对这条路径的线程依赖后移除多余 Invoke，让 Scarborough 继续在自己的渲染线程管理窗口。
- **收益与边界：** 减少动作线程等待 UI 及 UI 负担；旧 WinForms 后端保留必要的线程切换。

<a id="item-26"></a>

## 26. 正常悬浮窗刷新也创建空的异常/删除列表

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:467](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:467) — `UpdateImages`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:551](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:551) — `UpdateText`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:651](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:651) — `Render(RenderCollection)`。
- **现状：** 每轮创建待删除和待输出消息列表，大多数时候两者都为空。
- **建议：** 首次确有删除项或消息时才创建临时列表。
- **收益与边界：** 减少持续更新中的小对象分配，不增加长期缓存；正常更新和渲染项收集仍照常进行。

<a id="item-27"></a>

## 27. 标签列每次 Layout 都重新测量全部标签

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ConfigurationEditor.cs:127](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConfigurationEditor.cs:127) — `table.Layout 事件`。
  - [Source/Triggernometry/UI/CustomControls/ConfigurationEditor.cs:131](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConfigurationEditor.cs:131) — `UpdateLabelColumnWidth`。
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.Rules.cs:163](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.Rules.cs:163) — `动态标签文字`。
- **现状：** 即使只改变控件高度或展开表达式框，也逐个调用标签 GetPreferredSize。
- **建议：** 在创建、文字或字体变化时更新标签所需宽度；缩放时只结合既有 2/3 上限计算实际宽度。
- **收益与边界：** 减少重复测量；动作子类型导致的标签改名，以及字体/DPI 变化必须触发重新测量。

<a id="item-28"></a>

## 28. 图片和文本悬浮窗更新循环基本重复

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:467](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:467) — `UpdateImages`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:551](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:551) — `UpdateText`。
- **现状：** 两套循环重复更新、标记重绘、异常处理、收集删除、释放及消息输出。
- **建议：** 利用已有 ScarboroughItem 基类，提取一个简单的泛型更新实现。
- **收益与边界：** 主要减少重复维护；不引入新的管理框架，保留各项更新顺序与错误上下文。

<a id="item-29"></a>

## 29. 显示层次不变时仍反复重建并排序渲染列表

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:769](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:769) — `Render 逻辑 tick`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:778](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:778) — `rc.items.Sort`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:317](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:317) — `GetNextOrdinal / ActivateImage / ActivateText`。
- **现状：** 排序已移出高频绘制部分，但每个逻辑 tick 仍重建并排序；Ordinal 主要在激活时改变。
- **建议：** 复用现有渲染顺序，只在新增、删除或重新激活改变层次时更新。
- **收益与边界：** 悬浮窗多时更有价值，数量少时有限；不能漏掉删除和重新激活带来的顺序变化。

<a id="item-30"></a>

## 30. 字体样式的四段位标志转换重复多处

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.cs:170](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.cs:170) — `CreateFontFromDefinition`。
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:477](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:477) — `Scarborough 字体样式转换`。
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:593](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:593) — `旧文本悬浮窗字体比较与创建`。
  - [Source/Triggernometry/UI/CustomControls/ActionEditor.Font.cs:77](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionEditor.Font.cs:77) — `现有位掩码写法`。
- **现状：** 粗体、斜体、下划线、删除线分别用 if 拼装 FontStyle，并在多个入口重复。
- **建议：** 复用简短的位掩码转换方法；四个值与 FontStyle 对应，明确屏蔽 Outline 等非字体样式位。
- **收益与边界：** 主要改善写法与复用，性能变化小；保持描边独立。

<a id="item-31"></a>

## 31. 高频日志先生成文本，再判断是否过滤

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:149](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:149) — `QueueAction 的入队/去重日志`。
  - [Source/Triggernometry/Core/ActionBase.cs:476](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:476) — `ExecutionImplementation 的执行日志`。
  - [Source/Triggernometry/Core/ActionBase.cs:546](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:546) — `AddToLog`。
- **现状：** 即使 Info/Verbose 不输出，也先执行 Describe、翻译与格式化，随后才被 AddToLog 丢弃。
- **建议：** 在高频入口先取得有效日志级别，仅实际需要输出时生成相应文本。
- **收益与边界：** 轻量动作大量执行时更值得处理；保留动作/触发器级别继承，不能省掉变量来源等必要描述。

<a id="item-32"></a>

## 32. 字典按长度生成时 ElementAt 导致平方级枚举

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableDict.cs:541](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableDict.cs:541) — `SetAll 的指定长度分支`。
  - [Source/Triggernometry/Core/Actions/ActionVariableDict.cs:561](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableDict.cs:561) — `两次 vd.Values.ElementAt(i)`。
- **现状：** 循环内分别从头枚举取得第 i 个键和值，处理 N 个旧条目时读取部分为 O(N²)。
- **建议：** 使用一个枚举器按原有顺序读取同一对键和值，超过旧字典长度后继续使用空值。
- **收益与边界：** 读取部分可降为 O(N)，不增加索引缓存；保留生成表达式的求值次数、顺序和索引起点。

<a id="item-33"></a>

## 33. 跨变量存储区插入列表时深复制两遍

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:749](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:749) — `ExecuteImplementation：InsertList`。
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:765](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:765) — `源列表 Duplicate`。
  - [Source/Triggernometry/Core/Variables/VariableList.cs:142](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Variables/VariableList.cs:142) — `InsertList 内逐项 Duplicate`。
- **现状：** 临时/永久存储区不同时先复制源列表，插入方法又把复制后的元素再复制一遍。
- **建议：** 保持现有锁方式，在源锁内复制一次；插入已独立拥有的元素时共用补位、插入与元数据更新逻辑。
- **收益与边界：** 大列表和嵌套值减少第二遍深复制；不得让目标与源共享可编辑元素，也不借此重做变量锁。

<a id="item-34"></a>

## 34. 切换动作类型时重新创建相同的通用设置控件

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Forms/ActionForm.cs:153](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/ActionForm.cs:153) — `BindEditors`。
  - [Source/Triggernometry/UI/Forms/ActionForm.cs:165](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/ActionForm.cs:165) — `generalEditor.Bind`。
  - [Source/Triggernometry/UI/CustomControls/ConfigurationEditor.cs:164](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConfigurationEditor.cs:164) — `Bind`。
- **现状：** 访问通用设置页后，每次切换动作类型都会销毁并生成相同的通用字段控件。
- **建议：** 提供轻量重新绑定入口，替换模型并重新加载值，保留这批通用控件。
- **收益与边界：** 减少原生控件创建、销毁和布局；保留只读状态、共同参数及事件加载抑制。

<a id="item-35"></a>

## 35. 表格追加先生成空单元格，再覆盖它们

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Variables/VariableTable.cs:220](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Variables/VariableTable.cs:220) — `AppendVertical`。
  - [Source/Triggernometry/Core/Variables/VariableTable.cs:241](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Variables/VariableTable.cs:241) — `AppendHorizontal`。
  - [Source/Triggernometry/Core/Actions/ActionVariableTable.cs:569](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableTable.cs:569) — `追加前的源表深复制`。
- **现状：** Resize 为新增区域构造空标量，后续马上用源单元格覆盖其中大部分。
- **建议：** 直接追加已有行或单元格，仅对尺寸不齐的区域补空；沿用调用方已经完成的深复制。
- **收益与边界：** 减少大表追加的无效对象创建；保持横纵追加尺寸、单元格独立性和修改元数据。

<a id="item-36"></a>

## 36. 条件模型事件未退订，可能保留已关闭编辑器

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:330](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:330) — `AttachConditionComponentToNode / OnPropertyChange 订阅`。
  - [Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:84](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:84) — `ClearReverseLookup / RemoveReverseLookup`。
  - [Source/Triggernometry/UI/CustomControls/ConditionViewer.Designer.cs:14](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConditionViewer.Designer.cs:14) — `Dispose`。
  - [Source/Triggernometry/UI/Forms/TriggerForm.cs:210](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/TriggerForm.cs:210) — `保存 ConditionToEdit 到触发器`。
- **现状：** 条件订阅编辑器的事件后未对应解绑；保存时模型进入配置，可能通过事件委托保留已关闭的控件/窗体。
- **建议：** 统一管理订阅，在换绑、移除和 Dispose 时解绑。
- **收益与边界：** 资源与生命周期问题，优先级高于一般小分配；需要后续验证模型不再保留关闭的编辑器。

<a id="item-37"></a>

## 37. ACT 交互的 StringParam 是未使用配置

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionActInteraction.cs:83](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionActInteraction.cs:83) — `StringParam / Xml_StringParam`。
  - [Source/Triggernometry/Core/Actions/ActionActInteraction.cs:115](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionActInteraction.cs:115) — `ExecuteImplementation`。
- **现状：** 现有三个操作的执行及描述只使用 BoolParam，字符串仍参与生成输入框、复制和保存。
- **建议：** 停止生成无效编辑控件；如需读取旧字段，留在兼容边界处理。
- **收益与边界：** 减少无效 UI 和维护成本，避免用户填写不起作用的参数；先确认没有外部使用约定。

<a id="item-38"></a>

## 38. 列表排序的升降序分支成对重复

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:611](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:611) — `SortAlphaAsc / SortAlphaDesc`。
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:635](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:635) — `SortNumericAsc / SortNumericDesc`。
  - [Source/Triggernometry/Core/Actions/ActionVariableList.cs:659](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionVariableList.cs:659) — `SortFfxivPartyAsc / SortFfxivPartyDesc`。
- **现状：** 三类排序分别复制两份取变量、加锁、执行和日志代码。
- **建议：** 每对先计算方向，共用主体，仅调用处区分升序与降序。
- **收益与边界：** 主要改善可读性，不宣称明显速度提升；保留已有方法、枚举值及排序语义，不扩展实体检查。

<a id="item-39"></a>

## 39. 快捷输入对话框没有被调用方释放

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:643](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:643) — `延迟/颜色快捷编辑`。
  - [Source/Triggernometry/UI/Forms/SimpleInputForm.cs:81](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/SimpleInputForm.cs:81) — `GetInput`。
  - [Source/Triggernometry/UI/Forms/SimpleInputForm.cs:67](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/SimpleInputForm.cs:67) — `Disposed 时释放 fontOverride`。
- **现状：** new SimpleInputForm(...).GetInput() 仅 ShowDialog，模态窗体关闭后没有相应 Dispose。
- **建议：** 调用方使用 using；需要时复用一个负责创建、显示、取值和释放的简短入口。
- **收益与边界：** 明确窗口及自建字体资源释放；保持仍由调用方持有窗口的用法。

<a id="item-40"></a>

## 40. 声音停止事件通过遍历查找对应播放器

- **类别 / 状态：** 原生调用与声音；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionPlaySound.cs:94](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionPlaySound.cs:94) — `Mywmp_PlayStateChange`。
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Audio.cs:151](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Audio.cs:151) — `专用播放器事件注册`。
- **现状：** 停止事件反复扫描播放器列表并读取 COM playState，找出停止项后删除，再次从头查找。
- **建议：** 注册时捕获当前播放器，向清理方法传入对应实例，直接处理此播放器，并统一错误回调的清理。
- **收益与边界：** 重叠播放多时减少遍历与 COM 调用；保留声音持续播放语义，仔细管理事件和播放器生命周期。

<a id="item-41"></a>

## 41. 同步执行路径承担异步捕获结构的分配

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/ActionBase.cs:440](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/ActionBase.cs:440) — `Execute(ActionInstance, Context) / 本地 ExecuteCore`。
- **现状：** 本地函数捕获 ai、ctx、ct、this，并转换为 Task.Run 使用的委托；当前结构使同步路径也涉及捕获对象。
- **建议：** 公共执行主体改为带参数的实例方法，只有专门的异步启动入口创建闭包。
- **收益与边界：** 候选分配优化，尚未检查发布 IL 或实测；需确认编译结果与收益，不复制两份执行/清理逻辑。

<a id="item-42"></a>

## 42. 仅判断禁止重触发时仍完整计数队列

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Trigger.cs:710](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Trigger.cs:710) — `ApplyPrevActionsPolicy`。
  - [Source/Triggernometry/Core/Trigger.cs:740](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Trigger.cs:740) — `CountSelfQueuedActions`。
- **现状：** 拦截决定只需要知道是否存在旧动作，但总是 Count；数量用于随后可能被过滤的 Info 日志。
- **建议：** 只判断存在性时用 Any；实际需要准确数量日志时再完整计数。
- **收益与边界：** 长队列且频繁重触发时减少扫描；保留准确日志和现有队列锁，不改变重触发策略。

<a id="item-43"></a>

## 43. 动作中断去重先 Any，再 RemoveAll 扫描两遍

- **类别 / 状态：** 排队与执行；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:140](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.ActionQueue.cs:140) — `QueueAction：PreventDuplicateQueue / RefireInterrupt`。
- **现状：** 中断模式先 Any 检查重复，随后 RemoveAll 再次查找并删除。
- **建议：** 中断模式直接用 RemoveAll 返回数量判断旧实例是否存在；仅禁止重复时使用 Any。
- **收益与边界：** 减少一次扫描；两选项同时开启时保留当前“移除旧实例后拒绝新入队”的行为，维持取消及互斥锁语义。

<a id="item-44"></a>

## 44. 选择条件时，程序加载触发了用户修改通知

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:358](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:358) — `UpdateEditorToConditionComponent`。
  - [Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:671](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:671) — `表达式类型下拉框事件`。
  - [Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:942](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ConditionViewer.cs:942) — `操作下拉框事件`。
- **现状：** 设置文本框和下拉框会进入写回、类型切换和 ConditionsUpdated；用户只是选中条件也会产生重复处理。
- **建议：** 使用加载标记抑制用户编辑事件，加载完成后统一更新表达式类型与界面。
- **收益与边界：** 减少多余通知和中间状态处理；真实编辑仍触发通知，不能再次引入单击条件被误判修改的问题。

<a id="item-45"></a>

## 45. 派生控件通过反射设置自己可直接访问的属性

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:259](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ReadonlyRichTextBox.cs:259) — `构造函数：DoubleBuffered 反射赋值`。
- **现状：** 在派生类内部通过 GetProperty/SetValue 设置继承的受保护 DoubleBuffered。
- **建议：** 直接赋值 DoubleBuffered = true。
- **收益与边界：** 主要简化写法，初始化性能变化很小；保持当前控件行为。

<a id="item-46"></a>

## 46. 悬浮窗执行整批操作时持有生产者队列锁

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:88](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:88) — `ExecuteActions`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:111](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:111) — `ExecuteAction`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:285](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:285) — `Activate 等提交入口`。
- **现状：** 取队列项后仍持锁执行参数解析、窗口更新和资源释放，生产者提交新操作需要一起等待。
- **建议：** 缩短锁到队列取项或取当前批次，之后由同一个渲染线程顺序执行。
- **收益与边界：** 减少提交线程等待；保留顺序、Completed 通知和有限批次，避免持续入队使渲染饥饿；无需增加线程。

<a id="item-47"></a>

## 47. Scarborough 退出没有统一释放所有活动项

- **类别 / 状态：** 悬浮窗运行时；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:67](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:67) — `Dispose`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:76](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:76) — `Shutdown`。
  - [Source/Triggernometry/UI/Scarborough/Scarborough.cs:749](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/Scarborough.cs:749) — `Render 线程退出路径`。
  - [Source/Triggernometry/UI/Scarborough/ScarboroughItem.cs:142](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Scarborough/ScarboroughItem.cs:142) — `已有 Dispose`。
  - [Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:40](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/RealPlugin/RealPlugin.Aura.cs:40) — `DeInitAura`。
- **现状：** Shutdown 停线程，Dispose 只释放退出事件，仍活动的项目未统一调用现成资源清理。
- **建议：** 在渲染线程退出的 finally 中逐项释放并清空集合，再结束线程。
- **收益与边界：** 明确插件卸载/重新加载时图片、字体、画刷和绘图对象的释放；保留资源所属线程与停止顺序。

<a id="item-48"></a>

## 48. 触发器摘要刷新重复遍历动作并解析延迟

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/Forms/TriggerForm.cs:345](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/TriggerForm.cs:345) — `actionViewer1_ActionsUpdated`。
  - [Source/Triggernometry/UI/Forms/TriggerForm.cs:352](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/TriggerForm.cs:352) — `cndCondition_ConditionsUpdated`。
  - [Source/Triggernometry/UI/Forms/TriggerForm.cs:379](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/Forms/TriggerForm.cs:379) — `SetTriggerDescription`。
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:468](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:468) — `GetActionTotalDelay`。
- **现状：** 事件先计算延迟或条件信息，SetTriggerDescription 又计算一次；冷却和中断信息也有重复。
- **建议：** 统一计算入口，或事件只更新对应数据，摘要方法只负责格式化。
- **收益与边界：** 动作多时省掉整轮遍历和表达式解析；初次加载及相关参数变化时仍完整刷新。

<a id="item-49"></a>

## 49. 取消批量编辑仍刷新列表并通知摘要更新

- **类别 / 状态：** 编辑器与配置；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:641](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:641) — `ctxEditPropDelay_Click`。
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:658](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:658) — `ctxEditPropBgColor_Click`。
  - [Source/Triggernometry/UI/CustomControls/ActionViewer.cs:675](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/UI/CustomControls/ActionViewer.cs:675) — `ctxEditPropTextColor_Click`。
- **现状：** 输入窗口返回 null 时只跳过赋值，仍 Refresh 和 OnActionsUpdated。
- **建议：** 取消时直接返回。
- **收益与边界：** 避免无修改时的整表绘制及父窗体摘要重算；可随第 14、39 条一并整理，但编号独立保留。

<a id="item-50"></a>

## 50. 发送单个窗口按键时查找窗口两遍

- **类别 / 状态：** 原生调用与声音；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Utilities/WindowsUtils.cs:170](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Utilities/WindowsUtils.cs:170) — `SendKeycode`。
  - [Source/Triggernometry/Utilities/WindowsUtils.cs:190](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Utilities/WindowsUtils.cs:190) — `SendKeycodes`。
- **现状：** SendKeycode 中 FindWindows 的 wins 未使用；SendKeycodes 随后又执行一次 FindWindows。
- **建议：** 删除第一次未使用的窗口查找。
- **收益与边界：** 省一次窗口枚举、标题读取及正则匹配；发送目标仍来自原先实际使用的第二次查找。

<a id="item-51"></a>

## 51. 窗口查找的筛选顺序及首项结束可以优化

- **类别 / 状态：** 原生调用与声音；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Utilities/WindowsUtils.cs:128](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Utilities/WindowsUtils.cs:128) — `FindWindows(int, Regex)`。
  - [Source/Triggernometry/Utilities/WindowsUtils.cs:119](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Utilities/WindowsUtils.cs:119) — `FindWindows(int, string)`。
- **现状：** 先对所有窗口读取标题并匹配，完成后才按进程筛选；只需要首个窗口也枚举到底。
- **建议：** 指定进程时先筛进程；首个匹配即可返回的模式提前结束；只需成功与否时用 IsMatch。
- **收益与边界：** 减少原生 API 与匹配工作，不缓存窗口句柄；保留 procid 的负数/零/正数含义、枚举顺序和异常策略。

<a id="item-52"></a>

## 52. 文件追加到列表时逐行重复更新容器元数据

- **类别 / 状态：** 变量、排序与文件；待评估，未实施、未测试。
- **代码位置：**
  - [Source/Triggernometry/Core/Actions/ActionDiskOperation.cs:246](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Actions/ActionDiskOperation.cs:246) — `ReadIntoListVariable`。
  - [Source/Triggernometry/Core/Variables/VariableList.cs:98](C:/Users/Ferrum/source/repos/MnFeN/Triggernometry/Source/Triggernometry/Core/Variables/VariableList.cs:98) — `Push`。
- **现状：** 每行 Push 更新时间和来源，导入末尾又统一覆盖这些字段。
- **建议：** 直接批量添加单元格，末尾只更新一次容器元数据。
- **收益与边界：** 大文件减少重复时间读取与赋值；保留现有追加行为及子元素状态，不擅自改成覆盖。


