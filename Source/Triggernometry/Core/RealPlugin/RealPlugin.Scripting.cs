using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Triggernometry.Core.Scripting;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {
        internal Interpreter scripting;
        internal bool scriptingInited = false;
        internal Dictionary<string, object> scriptingStorage = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        private List<Configuration.APIUsage> DefaultAPIUsages = ScriptSecurity.SecurityAPIs.Select(name => new Configuration.APIUsage
        {
            Name = name,
            AllowLocal = false,
            AllowRemote = false,
            AllowAdmin = false
        }).ToList();

        private void InitScripting()
        {
            try
            {
                scripting = new Interpreter();
            }
            catch (Exception ex)
            {
                FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/iniscripterror", 
                    "Error when initializing scripting - try changing plugin load order: {0}", ex.FullMessage()));
            }
        }

        internal List<Configuration.APIUsage> GetDefaultAPIUsages()
        {
            List<Configuration.APIUsage> l = new List<Configuration.APIUsage>();
            foreach (Configuration.APIUsage a in DefaultAPIUsages)
            {
                l.Add(new Configuration.APIUsage() { Name = a.Name, AllowLocal = a.AllowLocal, AllowRemote = a.AllowRemote, AllowAdmin = a.AllowAdmin });
            }
            return l;
        }

        private void SetupDefaultSecurity()
        {
            MethodInfo setter = Config.GetType().GetMethod("AddAPIUsage", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (Configuration.APIUsage a in DefaultAPIUsages)
            {
                setter.Invoke(Config, new object[] { a, false });
            }
        }

        public object InvokeStorageCallback(string name, IEnumerable<string> args)
        {
            if (!Instance.scriptingStorage.TryGetValue(name, out var obj))
                throw new ArgumentException(I18n.Translate("internal/Scripting/callbacknotfound",
                    "Callback function '{0}' was not found.", name));

            if (!(obj is Delegate callback))
                throw new ArgumentException(I18n.Translate("internal/Scripting/storedobjectnotdelegate",
                    "Stored object '{0}' is not a delegate.", name));

            return callback.RawInvoke(args?.ToArray());
        }

    }

}
