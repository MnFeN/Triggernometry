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

