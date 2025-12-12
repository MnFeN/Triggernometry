using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Core.Variables;

namespace Triggernometry.Core.Scripting
{
    /// <summary>
    /// Provides context-bound helper APIs for scripts. <br />
    /// · Tied to the current <see cref="Context"/>. <br />
    /// · Intended to be exposed via <see cref="ScriptGlobs.TriggernometryHelpers"/> inside scripts. <br />
    /// · Cannot be used in static classes/methods in scripts. <br />
    /// </summary>
    public class ScriptContextHelper // the previous "Interpreter.Helper"
    {
        public Dictionary<string, object> Storage
            => (CurrentContext?.Plugin ?? RealPlugin.Instance).scriptingStorage;

        public RealPlugin Plugin
            => CurrentContext?.Plugin ?? RealPlugin.Instance;

        public Context CurrentContext { get; set; }

        public void Log(int level, string message)
        {
            Log((RealPlugin.DebugLevelEnum)level, message);
        }

        public void Log(RealPlugin.DebugLevelEnum level, string message)
        {
            if (CurrentContext?.Trigger != null)
                CurrentContext.Trigger.AddToLog(level, message);
            else
                Plugin.FilteredAddToLog(level, message);
        }

        public void PlayTTS(string text)
            => ScriptHelper.PlayTTS(text, CurrentContext);

        public void PlaySound(string uri)
            => ScriptHelper.PlayTTS(uri, CurrentContext);

        public string EvaluateStringExpression(string expr)
            => ScriptHelper.EvaluateStringExpression(expr, CurrentContext);

        public double EvaluateNumericExpression(string expr)
            => ScriptHelper.EvaluateNumericExpression(expr, CurrentContext);

        public string GetScalarVariable(bool persistent, string varname)
            => ScriptHelper.GetScalarVariable(persistent, varname);
        public VariableList GetListVariable(bool persistent, string varname)
            => ScriptHelper.GetListVariable(persistent, varname);
        public VariableTable GetTableVariable(bool persistent, string varname)
            => ScriptHelper.GetTableVariable(persistent, varname);
        public VariableDictionary GetDictVariable(bool persistent, string varname)
            => ScriptHelper.GetDictVariable(persistent, varname);

        public void SetScalarVariable(bool persistent, string varname, object data)
            => ScriptHelper.SetScalarVariable(persistent, varname, data);
        public void SetListVariable(bool persistent, string varname, VariableList data)
            => ScriptHelper.SetListVariable(persistent, varname, data);
        public void SetTableVariable(bool persistent, string varname, VariableTable data)
            => ScriptHelper.SetTableVariable(persistent, varname, data);
        public void SetDictVariable(bool persistent, string varname, VariableDictionary data)
            => ScriptHelper.SetDictVariable(persistent, varname, data);

        public string GetRegexMatch(int idx, string defValue = "")
            => CurrentContext.GetNumGroup(idx) ?? defValue;

        public string GetRegexMatch(string name, string defValue = "")
            => CurrentContext.GetNamedGroup(name) ?? defValue;

    }
}
