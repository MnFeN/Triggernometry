# TODO

## 变量动作的锁与并发语义

- [ ] 基于原有代码，检查并消除持有变量字典锁时求值造成的交叉死锁。

当前代码尚未修复此问题。此前试验的全局共用锁方案已撤销，不作为当前实现。

### 原代码的问题

列表、表格等动作会在持有变量字典锁时计算表达式。例如两个并发的 `Set` 动作：

1. A 锁住列表，然后通过表达式读取表格。
2. B 锁住表格，然后通过表达式读取列表。
3. 双方等待对方释放锁，形成死锁；后续访问同一存储区列表、表格的操作也可能阻塞。

此问题已用实际变量动作在独立测试进程中复现，与触发器配置的 mutex 无关。

### 待评估的修改方向

考虑参考原有标量 `Set`，采用“锁外求值、锁内写入”，避免持有目标变量锁时读取其他类型变量。

逐项检查赋值、批量转换、筛选、排序等路径，避免用覆盖所有变量动作的全局锁牺牲并发性能。

需要明确并发语义：

- 原有标量 `Set` 不保证“读取旧值、计算、写回”的原子性。`Set a = ${var:a} - 1` 可能覆盖期间其他动作对 `a` 的写入。
- 列表、表格的部分操作目前在持锁期间完成求值和修改。改为锁外求值，会放宽这些操作原有的保护，不应宣称并发行为完全不变。
- 是否统一采用标量 `Set` 的语义，尚待决定。需要原子更新的场景可使用专门操作或显式 mutex；原有标量 `Increment` 已在锁内完成读、改、写。
- 保留 `_this`、`_idx`、`_row`、`_col`、`_key`、`_val` 的上下文和单线程计算结果；另外处理集合遍历期间的并发修改，避免枚举异常。

## 条件表达式的逻辑和界面待优化：数值直接解析

---

索引处理小数报错
不存在的var方法报错
临时集合处理小数点的逻辑
min/max重构
trim name
trim 冒号后面的
context == null 时统一修改报错
递归报错
保证tryxxx不会返回null
新建触发器收不到环境变量提示

删掉：

        private static Exception InfiniteClipboardError()
        {
            return new ArgumentException(I18n.Translate("internal/Context/infiniteClipboardError",
                "The current clipboard contains the expression ${{_clipboard}}, which would cause infinite loop"));
        }

表单内 parenttrigger疑似没有继承
Regex Captured Group out of range: #1 没有提示名字
远程禁止复制？
循环动作计数没把placeholder排除
鲶鱼精入口函数加个提示：这可能是上次act强制退出导致的，需重启游戏

[鲶鱼精邮差扩展] 模块 VfxModule 初始化失败：Error while reading memory! 仅完成部分的 ReadProcessMemory 或 WriteProcessMemory 请求。, at addr: 7FF74EC2EF17, Size: 8

[鲶鱼精邮差扩展] 模块 VfxModule 初始化失败：Error while reading memory! 仅完成部分的 ReadProcessMemory 或 WriteProcessMemory 请求。, at addr: 7FF6F216F217, Size: 8

错误	动作异常：索引超出了数组界限。  
动作：[条] 调用具名回调 Omen，参数：'${?l:${f:replace(" ", ","):${vfxname}}[1]}, ${n: ${?l:${f:replace(" ", ","):${vfxname}}[2]} ?? 3}, ${_me.pos, h}, ${n: ${?l:${f:replace(" ", ","):${vfxname}}[3]} ?? 5}, ${n: ${?l:${f:replace(" ", ","):${vfxname}}[4]} ?? 5}, ${n: ${?l:${f:replace(" ", ","):${vfxname}}[5]} ?? 5}...  
触发器：[工具] 运行支持库（必需）\[工具] 运行支持库 Utils.xml\鲶鱼精邮差扩展 v4.3.0\core\接收文本指令\Omen
IndexOutOfRangeException: 索引超出了数组界限。
   在 System.Collections.Generic.Dictionary`2.Insert(TKey key, TValue value, Boolean add)
   在 GreyMagic.ExternalProcessMemory.CallInjected64[T](IntPtr address, Object[] args)
   在 CallSite.Target(Closure , CallSite , Object , IntPtr , Object[] )
   在 Triggernometry.PluginBridges.BridgeNamazu.GreyMagicExternalProcessMemory.<>c__DisplayClass14_0`1.<CallInjected64>b__0()
   在 Triggernometry.PluginBridges.BridgeNamazu.GreyMagicExternalProcessMemory.WrapInjectedCall[T](Func`1 func)
   在 Triggernometry.PluginBridges.BridgeNamazu.Modules.VfxModule.ProcessStaticVfx(String rawArgs, String nameFormatTemplate)
--- 引发异常的上一位置中堆栈跟踪的末尾 ---
   在 System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   在 Triggernometry.PluginBridges.BridgeNamazu.Modules.ModuleBase.<>c__DisplayClass21_0.<RegisterAnnotatedCallbackMethods>b__0(Object _, String data)

多了个句号
动作异常：解析文本表达式时出错:文本模板 ${y}无法识别为有效表达式，且不存在此名称的正则捕获组。。完整表达式:

双击无法定位
动作异常：解析文本表达式时出错：Variable 'V7a1_移动_dir4' does not exist.。
完整表达式：π - DirToRad(${!v:V7a1_移动_dir4}, 4)
触发器：7.2 指方向 (9ce64ace-01ea-4c7e-aaaa-1866bfb1136c)  
动作：标量 (V7a1_指令1_指路_dθ) 赋值为数值表达式 (π - DirToRad(${!v:V7a1_移动_dir4}, 4))  
触发器：Local triggers\异闻迷宫宝宝椅\V7a 商客奇谭　　　v0.3.0\1 人鱼达莉娅\B. 迷人的指令 1\7.2 指方向
Exception: 解析文本表达式时出错：Variable 'V7a1_移动_dir4' does not exist.。
完整表达式：π - DirToRad(${!v:V7a1_移动_dir4}, 4)
触发器：7.2 指方向 (9ce64ace-01ea-4c7e-aaaa-1866bfb1136c)
   在 Triggernometry.Expressions.String.StringParser.Parse(String expr, Context ctx, Boolean isTestModeNumeric)
   在 Triggernometry.Core.Context.ExpandVariables(LoggerDelegate logger, Object o, Boolean isNumeric, String expr)
   在 Triggernometry.Core.Context.EvaluateNumericExpression(LoggerDelegate logger, Object o, String expr)
   在 Triggernometry.Core.Actions.ActionVariableScalar.ExecuteImplementation(ActionInstance ai)
   在 Triggernometry.Core.ActionOld.ExecutionCore(QueuedAction qa, Context ctx)
   在 Triggernometry.Core.ActionOld.ExecutionImplementation(QueuedAction qa, Context ctx)
---------
Inner: Exception: Variable 'V7a1_移动_dir4' does not exist.
   在 Triggernometry.Expressions.String.Parsers.ColonParser.GetVariableWithCondition[T](VariableStore store, Dictionary`2 variables, String varName, Boolean mustExist)
   在 Triggernometry.Expressions.String.Parsers.ColonParser.TryParse(String template, Context ctx)
   在 Triggernometry.Expressions.String.Parsers.TemplateParser.ParseSingleTemplate(String templateBody, Context ctx, Boolean isTestModeNumeric)
   在 Triggernometry.Expressions.String.Parsers.TemplateParser.ParseTemplateMatch(TemplateMatch m, Context ctx, Boolean isNumeric)
   在 Triggernometry.Expressions.String.Parsers.TemplateParser.ReplaceTemplates(String text, List`1 templates, Context ctx, Boolean isNumeric)
   在 Triggernometry.Expressions.String.StringParser.Parse(String expr, Context ctx, Boolean isTestModeNumeric)

