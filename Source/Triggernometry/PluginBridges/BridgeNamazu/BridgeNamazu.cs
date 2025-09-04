using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Triggernometry.Forms;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;

namespace Triggernometry.PluginBridges.BridgeNamazu
{
    public static class BridgeNamazu
    {
        public const string PluginName = "PostNamzu.dll";
        public const string PluginType = "PostNamazu.PostNamazu";

        public static RealPlugin.PluginWrapper WrappedPlugin
            => _wrappedPlugin ?? (_wrappedPlugin = RealPlugin.InstanceHook(PluginName, PluginType));
        private static RealPlugin.PluginWrapper _wrappedPlugin;
        
        public static NamazuPlugin NamazuPlugin
            => _namazuPlugin ?? (_namazuPlugin = new NamazuPlugin(WrappedPlugin.pluginObj));
        private static NamazuPlugin _namazuPlugin;
        
        public static IReadOnlyDictionary<Type, ModuleBase> Modules => _modules;
        private static readonly Dictionary<Type, ModuleBase> _modules = new Dictionary<Type, ModuleBase>();
        
        public static IReadOnlyDictionary<Type, ModuleBase> SideloadModules => _sideloadModules;
        private static readonly Dictionary<Type, ModuleBase> _sideloadModules = new Dictionary<Type, ModuleBase>();

        static BridgeNamazu()
        {
            if (!RealPlugin.IsAdmin())
            {
                RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                    "[鲶鱼精邮差扩展] 警告：ACT 未以管理员权限运行。如果遇到游戏崩溃，请尝试右键 ACT 程序 - 属性 - 兼容性，开启管理员身份运行。");
            }
        }

        internal static void AddSideloadModule(ModuleBase module)
        {
            lock (_sideloadModules)
            {
                _sideloadModules[module.GetType()] = module;
            }
        }

        private static IEnumerable<Type> GetAllModuleTypes()
        {
            var baseType = typeof(ModuleBase);
            return baseType.Assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t));
        }

        /// <summary>
        /// 接收到鲶鱼精邮差注入游戏的日志后，需要从脚本调用此方法以便初始化所有模块。
        /// 可传入 sideload 方法，改写模块的 ScanMethod 或其他字段等。
        /// </summary>
        public static void InitializeModules(System.Action sideload = null)
        {
            // 重新生成所有模块实例
            lock (_modules)
            {
                foreach (var type in GetAllModuleTypes())
                {
                    try
                    {
                        var instance = (ModuleBase)Activator.CreateInstance(type);
                        _modules[type] = instance;
                    }
                    catch (Exception ex)
                    {
                        RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                            $"[鲶鱼精邮差扩展] 模块 {type.Name} 创建失败：{ex.Message}");
                    }
                }
            }
            // 执行 sideload 方法
            sideload?.Invoke();
            // 扫描所有模块
            foreach (var type in GetAllModuleTypes())
            {
                try
                {
                    var module = GetModule(type);
                    module.Scan();
                    RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Custom,
                        $"[鲶鱼精邮差扩展] 已初始化模块 {module.GetType().Name}。");
                }
                catch (Exception ex)
                { 
                    RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                        $"[鲶鱼精邮差扩展] 模块 {type.Name} 初始化失败：{ex.Message}");
                }
            }
            // 生成日志 以供后续脚本添加回调
            RealPlugin.plug.LogLineQueuer("PNE_ModulesInited", "", LogEvent.SourceEnum.Log);
        }

        /// <summary>
        /// InitializeModules 方法调用结束后，调用此方法注册所有模块的具名回调 / 脚本 storage 方法。
        /// 不传入参数时，注册所有模块中无 tag 的方法；否则注册指定 tag 的方法。
        /// </summary>
        public static void RegisterAnnotatedMethods(params string[] methodTags)
        {
            foreach (var type in GetAllModuleTypes())
            {
                var module = GetModule(type);
                module.RegisterAnnotatedMethods(methodTags);
            }
            if (methodTags.Length == 0)
                methodTags = new string[] { "Basic" };
            foreach (var tag in methodTags)
            {
                // 生成日志
                RealPlugin.plug.LogLineQueuer($"PNE_ModulesRegistered:{tag}", RealPlugin.plug.currentZone, LogEvent.SourceEnum.Log);
            }
        }

        public static ModuleBase GetModule(Type type)
        {
            if (!_modules.TryGetValue(type, out var module))
            {
                throw new Exception($"[鲶鱼精邮差扩展] 模块 {type.Name} 名称错误或未初始化。");
            }
            return module;
        }

        public static T GetModule<T>() where T : ModuleBase
            => (T)GetModule(typeof(T));

        public static void Log(string msg) => ((dynamic)NamazuPlugin.PluginUI).Log(msg);

        [STAThread]
        public static void ShowConfig()
        {
            try
            {
                Application.OpenForms.OfType<GameConfigForm>().ToList().ForEach(f => f.Close());
                Thread staThread = new Thread(new ThreadStart(NamazuConfig.TryRunConfigForm));
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[鲶鱼精邮差扩展] 配置表单运行错误：\n" + ex, NamazuConfig.Info.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
