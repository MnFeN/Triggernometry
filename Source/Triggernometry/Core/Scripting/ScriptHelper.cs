using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Triggernometry.Core.Variables;

namespace Triggernometry.Core.Scripting
{
    /// <summary>
    /// Provides static helper APIs for scripts and internal code. <br />
    /// · Uses the singleton <see cref="RealPlugin.Instance"/> and optional context when needed. <br />
    /// · Can be used in static classes/methods in scripts.
    /// </summary>
    public static class ScriptHelper // the previous "Interpreter.StaticHelper"
    {
        public static Dictionary<string, object> Storage => RealPlugin.Instance.scriptingStorage;

        public static void Log(int level, string message)
            => RealPlugin.Instance.FilteredAddToLog((RealPlugin.DebugLevelEnum)level, message);

        public static void Log(RealPlugin.DebugLevelEnum level, string message)
            => RealPlugin.Instance.FilteredAddToLog(level, message);

        public static void ErrorLog(string message) => Log(RealPlugin.DebugLevelEnum.Error, message);
        public static void WarningLog(string message) => Log(RealPlugin.DebugLevelEnum.Warning, message);
        public static void CustomLog(string message) => Log(RealPlugin.DebugLevelEnum.Custom, message);
        public static void Custom2Log(string message) => Log(RealPlugin.DebugLevelEnum.Custom2, message);
        public static void InfoLog(string message) => Log(RealPlugin.DebugLevelEnum.Info, message);
        public static void VerboseLog(string message) => Log(RealPlugin.DebugLevelEnum.Verbose, message);

        public static void PlayTTS(string text)
            => RealPlugin.Instance.TtsPlaybackHook?.Invoke(text);

        public static void PlayTTS(string text, Context ctx)
        {
            ActionOld a = new ActionOld();
            a._UseTTSTextExpression = text;
            ctx.ttshook(ctx, a);
        }

        public static void PlaySound(string uri, Context ctx)
        {
            ActionOld a = new ActionOld();
            a._PlaySoundFileExpression = uri;
            ctx.soundhook(ctx, a);
        }

        public static string EvaluateStringExpression(string expr, Context ctx = null)
        {
            ctx = ctx ?? Context.Unbound;
            return ctx.EvaluateStringExpression(null, ctx, expr);
        }

        public static double EvaluateNumericExpression(string expr, Context ctx = null)
        {
            ctx = ctx ?? Context.Unbound;
            return ctx.EvaluateNumericExpression(null, ctx, expr);
        }

        public static string GetScalarVariable(bool isPersistent, string varname)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.Scalar)
            {
                return vs.Scalar.TryGetValue(varname, out var variable) ? variable.Value : null;
            }
        }

        public static VariableList GetListVariable(bool isPersistent, string varname)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.List)
            {
                return vs.List.TryGetValue(varname, out var variable) ? variable : null;
            }
        }

        public static VariableTable GetTableVariable(bool isPersistent, string varname)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.Table)
            {
                return vs.Table.TryGetValue(varname, out var variable) ? variable : null;
            }
        }

        public static VariableDictionary GetDictVariable(bool isPersistent, string varname)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.Dict)
            {
                return vs.Dict.TryGetValue(varname, out var variable) ? variable : null;
            }
        }

        public static void SetScalarVariable(bool isPersistent, string varname, object data)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.Scalar)
            {
                if (data == null)
                    vs.Scalar.Remove(varname);
                else if (data is VariableScalar variable)
                    vs.Scalar[varname] = variable;
                else
                    vs.Scalar[varname] = new VariableScalar { Value = data.ToString() };
            }
        }

        public static void SetListVariable(bool isPersistent, string varname, VariableList data)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.List)
            {
                if (data == null)
                    vs.List.Remove(varname);
                else
                    vs.List[varname] = data;
            }
        }

        public static void SetTableVariable(bool isPersistent, string varname, VariableTable data)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.Table)
            {
                if (data == null)
                    vs.Table.Remove(varname);
                else
                    vs.Table[varname] = data;
            }
        }

        public static void SetDictVariable(bool isPersistent, string varname, VariableDictionary data)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            lock (vs.Dict)
            {
                if (data == null)
                    vs.Dict.Remove(varname);
                else
                    vs.Dict[varname] = data;
            }
        }

        public static string Serialize(object o, bool indent = true)
            => JsonSerializer.Serialize(o, new JsonSerializerOptions { WriteIndented = indent });
        public static T Deserialize<T>(string s) => JsonSerializer.Deserialize<T>(s);

        public static Process XivProcess => Utilities.Memory.XivProc;
        public static void RegisterXivProcessUpdatedAction(string key, Action action)
            => Utilities.Memory.RegisterXivProcUpdatedAction(key, action);
    }
}
