using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Triggernometry.Localization;

namespace Triggernometry.Core.Scripting
{

    internal class Interpreter
    {
        private readonly ScriptOptions _scriptOptions;

        internal bool Ready = false;
        internal Interpreter()
        {
            _scriptOptions = ScriptOptions.Default.AddImports("System");
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                _scriptOptions = _scriptOptions.AddMetadataReferenceFromAssembly(asm);
            }
            Task.Run(() => Initialize()); // takes ~3 s, do it async
        }

        private void Initialize()
        {
            try
            {
                Evaluate("int whee;", null, Context.Unbound);
                Ready = true;
            }
            catch (Exception ex)
            {
                RealPlugin.Instance.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                    I18n.Translate(
                        "internal/Plugin/iniscripterror",
                        "Error when initializing scripting - try changing plugin load order: {0}",
                        ex.Message));
            }
            RealPlugin.Instance.scriptingInited = true;
        }

        public void Evaluate(string rawScript, string extraAssembliesInput, Context ctx)
        {
            ScriptGlobs globs = new ScriptGlobs(ctx);
            ScriptOptions scriptOptions = BuildScriptOptions(extraAssembliesInput, ctx);

            string[] restrictedApis = ScriptSecurity.GetRestrictedApisFromConfig(ctx);
            bool allowDynamic = ScriptSecurity.IsFeatureAllowedByConfig(ctx.Plugin.cfg.DynamicUsage, ctx);

            // nothing restricted: run directly
            if (restrictedApis.Length == 0 && allowDynamic)
            {
                Task<object> scriptTask = CSharpScript.EvaluateAsync(rawScript, scriptOptions, globs, typeof(ScriptGlobs));
                ExecuteScriptTask(scriptTask, globs);
                return;
            }
            else // check and run
            {
                Script<object> script = CSharpScript.Create(rawScript, scriptOptions, typeof(ScriptGlobs));

                if (!PassedRestrictedApiCheck(script, restrictedApis, globs) ||
                    !PassedDynamicUsageCheck(script, allowDynamic, globs))
                    return;

                Task<ScriptState<object>> scriptTask = script.RunAsync(globs);
                ExecuteScriptTask(scriptTask, globs);
            }
        }

        /// <summary>
        /// · <paramref name="extraAssembliesInput" />: User-input assemblies, separated with comma
        /// </summary>
        private ScriptOptions BuildScriptOptions(string extraAssembliesInput, Context ctx)
        {
            var allowUnsafe = ScriptSecurity.IsFeatureAllowedByConfig(ctx.Plugin.cfg.UnsafeUsage, ctx);
            ScriptOptions scriptOptions = _scriptOptions.WithAllowUnsafe(allowUnsafe);

            if (string.IsNullOrEmpty(extraAssembliesInput))
                return scriptOptions;

            var currentAsms = AppDomain.CurrentDomain.GetAssemblies();
            var extraAsmNames = extraAssembliesInput.Split(',')
                .Select(s => s.Trim())
                .Where(name => !string.IsNullOrEmpty(name));

            foreach (var extraAsmName in extraAsmNames)
            {
                Assembly found = currentAsms.FirstOrDefault(a => a.GetName().Name.Equals(extraAsmName, StringComparison.OrdinalIgnoreCase));

                // try to first load from current assemblies
                // including the assemblies loaded into memory by ACT which were not detected during InitPlugin
                scriptOptions = found != null
                    ? scriptOptions.AddMetadataReferenceFromAssembly(found)
                    : scriptOptions.AddReferences(extraAsmName);
            }

            return scriptOptions;
        }

        private void ExecuteScriptTask(Task task, ScriptGlobs g)
        {
            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                List<Exception> exList = ex is AggregateException aex
                    ? aex.Flatten().InnerExceptions.ToList()
                    : new List<Exception> { ex };

                foreach (var e in exList)
                {
                    g.TriggernometryHelpers.Log(RealPlugin.DebugLevelEnum.Error,
                        I18n.Translate(
                            "internal/Interpreter/scriptExecutionError",
                            "Error occurred during script execution: \n{0}", e.FullMessage()
                        )
                    );
                }
            }
        }

        private static bool PassedRestrictedApiCheck(Script<object> script, string[] restrictedApis, ScriptGlobs globs)
        {
            if (restrictedApis == null || restrictedApis.Length == 0)
                return true;

            if (!ScriptSecurity.TryGetViolatingApi(script, out string violatingApi, restrictedApis))
                return true;

            LogBlockedError(globs, violatingApi);
            return false;
        }

        private static bool PassedDynamicUsageCheck(Script<object> script, bool allowDynamic, ScriptGlobs globs)
        {
            if (allowDynamic) return true;

            if (!ScriptSecurity.ContainsDynamic(script)) return true;

            LogBlockedError(globs, "dynamic");
            return false;
        }

        private static void LogBlockedError(ScriptGlobs globs, string reason)
        {
            var ctx = globs.TriggernometryHelpers.CurrentContext;
            string triggerName = ctx?.Trigger?.LogName ?? "(null)";
            globs.TriggernometryHelpers.Log(
                RealPlugin.DebugLevelEnum.Error, 
                I18n.Translate(
                    "internal/Interpreter/scriptblocked",
                    "Script execution on trigger {0} blocked due to restricted API/feature: {1}",
                    triggerName, reason));
        }
    }

    public static class ScriptOptionsExtensions
    {
        public static ScriptOptions AddMetadataReferenceFromAssembly(this ScriptOptions options, Assembly asm)
        {
            try
            {
                options = options.AddReferences(asm);
            }
            catch // fallback
            {
                if (TryCreateReferenceFromRawBytes(asm, out var reference))
                    options = options.AddReferences(reference);
            }
            return options;
        }

        private static bool TryCreateReferenceFromRawBytes(Assembly asm, out PortableExecutableReference reference)
        {
            try
            {
                MethodInfo getRawBytesMethod = asm.GetType().GetMethod("GetRawBytes", BindingFlags.Instance | BindingFlags.NonPublic);
                byte[] assemblyBytes = (byte[])getRawBytesMethod.Invoke(asm, null);
                if (assemblyBytes != null && assemblyBytes.Length > 0)
                {
                    reference = MetadataReference.CreateFromImage(assemblyBytes);
                    return true;
                }
            }
            catch { }
            reference = null;
            return false;
        }
    }

}
