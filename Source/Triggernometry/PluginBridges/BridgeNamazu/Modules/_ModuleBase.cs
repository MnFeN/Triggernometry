using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Triggernometry.Utilities;
using Triggernometry.Variables;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public abstract class ModuleBase
    {
        public static NamazuPlugin Plugin => BridgeNamazu.NamazuPlugin;
        public static GreyMagicExternalProcessMemory Memory => Plugin?.Memory;
        public static NamazuScanner Scanner => Plugin?.SigScanner;

        public System.Action ScanMethod;

        public void Scan()
        { 
            _ = ScanMethod ?? throw new Exception($"[鲶鱼精邮差扩展] {GetType().Name} 扫描方法 ScanMethod 未设置。");
            ScanMethod?.Invoke();
        }

        public void CheckBeforeExecution(string command)
        {
            if (!Plugin.IsReady)
                throw new Exception("[鲶鱼精邮差扩展] 没有对应的 FFXIV 进程。");
        }

        public void CheckIfAnyZeroPtr(params IntPtr[] ptrs)
        {
            if (ptrs.Any(p => p == IntPtr.Zero))
                throw new Exception($"[鲶鱼精邮差扩展] {GetType().Name} 指令执行所需的 IntPtr 未初始化，无法执行指令。");
        } 

        public void NamazuLog(string msg) => BridgeNamazu.Log(msg);
        public void TriggerLog(RealPlugin.DebugLevelEnum level, string msg) => RealPlugin.plug.UnfilteredAddToLog(level, msg);
        public void InfoLog(string msg) => RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Info, msg);
        public void CustomLog(string msg) => RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Custom, msg);
        public void Custom2Log(string msg) => RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Custom2, msg);
        public void WarningLog(string msg) => RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, msg);
        public void ErrorLog(string msg) => RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error, msg);

        public void Sideload(params string[] methodTags)
        {
            BridgeNamazu.AddSideloadModule(this);
            Scan();
            CustomLog($"[鲶鱼精邮差扩展] 已初始化模块 {GetType().Name}。");
        }

        public void RegisterCallback(string callBackName, Action<string> callBackAction)
        {
            Triggernometry.RealPlugin.plug.RegisterNamedCallback(
                callBackName,
                new Action<object, string>((_, cmd) => callBackAction(cmd)),
                null,
                registrant: $"[鲶鱼精邮差扩展] {GetType().Name}"
            );
        }

        public void RegisterAnnotatedMethods(params string[] tags)
        {
            var tagsSet = (tags.Length == 0) ? new HashSet<string> { null } : new HashSet<string>(tags);
            RegisterAnnotatedCallbackMethods(tagsSet);
            RegisterAnnotatedScriptingMethods(tagsSet);
        }

        private IEnumerable<(MethodInfo Method, TAttr Attribute)> GetAnnotatedMethods<TAttr>(HashSet<string> tagsSet)
            where TAttr : MethodRegistrationAttribute
        {
            return this.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .SelectMany(m => m.GetCustomAttributes(typeof(TAttr), false).Cast<TAttr>()
                    .Where(attr => string.IsNullOrEmpty(attr.Tag) || tagsSet.Contains(attr.Tag))
                    .Select(attr => (m, attr)));
        }

        private void RegisterAnnotatedCallbackMethods(HashSet<string> tagsSet)
        {
            foreach (var (method, attr) in GetAnnotatedMethods<CallbackMethodAttribute>(tagsSet))
            {
                if (method.IsStatic)
                {
                    ErrorLog($"[鲶鱼精邮差扩展] 回调方法 {method.Name} 必须为实例方法。");
                    continue;
                }

                var parameters = method.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(string))
                {
                    ErrorLog($"[鲶鱼精邮差扩展] 回调方法 {method.Name} 参数签名必须是 (string)。");
                    continue;
                }

                var callback = new Action<object, string>((_, data) =>
                {
                    try
                    {
                        method.Invoke(this, new object[] { data });
                    }
                    catch (TargetInvocationException ex) when (ex.InnerException != null)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                });
                RealPlugin.plug.RegisterNamedCallback(attr.Name, callback, null, false, $"[鲶鱼精邮差扩展] {GetType().Name}");
            }
        }

        private void RegisterAnnotatedScriptingMethods(HashSet<string> tagsSet)
        {
            foreach (var (method, attr) in GetAnnotatedMethods<ScriptingMethodAttribute>(tagsSet))
            {
                if (method.IsStatic)
                {
                    ErrorLog($"[鲶鱼精邮差扩展] 脚本方法 {method.Name} 必须为实例方法。");
                    continue;
                }

                var storage = RealPlugin.plug.scriptingStorage;
                lock (storage)
                {
                    try
                    {
                        Delegate del = Delegate.CreateDelegate(GetDelegateType(method), this, method);
                        storage[attr.Name] = del;
                    }
                    catch (TargetInvocationException ex) when (ex.InnerException != null)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                    catch (Exception ex)
                    {
                        ErrorLog($"[鲶鱼精邮差扩展] 方法 {method.Name} 无法生成 Delegate：{ex.Message}");
                    }
                }
            }
        }

        private static Type GetDelegateType(MethodInfo method)
        {
            var paramTypes = method.GetParameters().Select(p => p.ParameterType).ToList();
            if (paramTypes.Count >= 17)
                throw new NotSupportedException("不支持 17 个或以上参数的方法");
            if (method.ReturnType == typeof(void))
            {
                return Expression.GetActionType(paramTypes.ToArray());
            }
            else
            {
                paramTypes.Add(method.ReturnType);
                return Expression.GetFuncType(paramTypes.ToArray());
            }
        }
        
        public static T GetConfigOrSetDefault<T>(string key, T defaultValue)
        {
            var cfg = GetConfigDict();
            if (cfg.Values.TryGetValue(key, out var rawValue))
            {
                return rawValue.ToString().FromDataString<T>();
            }
            cfg.SetValue(key, defaultValue.ToDataString());
            return defaultValue;
        }

        public static T? GetConfig<T>(string key) where T : struct
        {
            var cfg = GetConfigDict();
            if (cfg.Values.TryGetValue(key, out var rawValue))
            {
                return rawValue.ToString().FromDataString<T>();
            }
            return null;
        }

        public static void SetConfig<T>(string key, T data) where T : struct
        {
            GetConfigDict().SetValue(key, data.ToDataString());
        }

        public static void RemoveConfig(string key)
        {
            GetConfigDict().Values.Remove(key);
        }

        public static VariableDictionary GetConfigDict()
        {
            var cfg = Triggernometry.Interpreter.StaticHelpers.GetDictVariable(true, "PNE_cfg");
            if (cfg == null)
            {
                cfg = new VariableDictionary();
                Triggernometry.Interpreter.StaticHelpers.SetDictVariable(true, "PNE_cfg", cfg);
            }
            return cfg;
        }

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class MethodRegistrationAttribute : Attribute
    {
        /// <summary>
        /// 脚本系统的标签，用于分组或筛选
        /// </summary>
        public string Tag { get; }
        public string Name { get; }

        public MethodRegistrationAttribute(string name, string tag = null)
        {
            Name = name;
            Tag = tag;
        }

    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CallbackMethodAttribute : MethodRegistrationAttribute
    {
        public CallbackMethodAttribute(string name, string tag = null) : base(name, tag)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ScriptingMethodAttribute : MethodRegistrationAttribute
    {
        public ScriptingMethodAttribute(string name, string tag = null) : base(name, tag)
        {         
        }
    }
}