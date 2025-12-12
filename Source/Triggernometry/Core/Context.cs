using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String;
using Triggernometry.Localization;
using static Triggernometry.Core.Configuration;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{
    /// <summary>
    /// Context for running triggers and evaluating string expressions.<br />
    /// Holds trigger, captured groups, timing info and plugin hooks used during evaluation.
    /// </summary>
    public partial class Context
    {
        internal Guid id = Guid.NewGuid();
        internal bool testByPlaceholder;
        private readonly RealPlugin _plugOverride;
        internal RealPlugin Plugin => _plugOverride ?? RealPlugin.Instance;
        public readonly Trigger Trigger;
        internal ActionOld.TriggerForceTypeEnum force;

        internal RealPlugin.ActionExecutionHook soundhook;
        internal RealPlugin.ActionExecutionHook ttshook;

        private Dictionary<string, string> _namedRegexGroups = null;
        private List<string> _numRegexGroups = null;
        internal DateTime triggeredTime;
        internal string zoneName = "";
        internal string regexPattern; // todo
        internal string triggeredText = "";
        internal string zoneIdOverride = null;

        internal string contextResponse = "";
        internal int contextResponseCode = 0;
        internal dynamic contextJsonResponse;
        internal bool isContextJsonParsed = false;

        internal List<int> ActionResults = new List<int>();
        internal Dictionary<Mutex, int> heldmutices = new Dictionary<Mutex, int>();

        // to-do: refactor these dynamic expressions
        internal int loopIterator = 0;
        internal Guid loopActionId = Guid.Empty;
        internal string varName = "";         // for ${_this} ${_row[i]} ${_col[i]}
        internal int listIndex = 0;           // for ${_idx}
        internal int tableColIndex = 0;       // for ${_col}
        internal int tableRowIndex = 0;       // for ${_row}
        internal string dictKey = "";         // for ${_key}
        internal string dictValue = "";       // for ${_val}

        /// <summary>
        /// Shared context not bound to any trigger, used for evaluations not related with any triggers.<br />
        /// ， Has <see cref="Trigger"/> <c> == null</c> and uses the global <see cref="RealPlugin.Instance"/> instance.
        /// </summary>
        public static Context Unbound { get; } = new Context(null);

        /// <summary>
        /// Create a new evaluation context bound to a trigger.<br />
        /// ， <paramref name="trigger" />: Trigger associated with this context; may be null.
        /// </summary>
        public Context(Trigger trigger)
        {
            Trigger = trigger;
            if (Trigger != null)
            {
                triggeredTime = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Create a new evaluation context with an explicit plugin instance.<br />
        /// ， <paramref name="trigger" />: Trigger associated with this context; may be null.<br />
        /// ， <paramref name="plugOverride" />: Plugin instance to use instead of <see cref="RealPlugin.Instance"/>.
        /// </summary>
        public Context(Trigger trigger, RealPlugin plugOverride)
        {
            Trigger = trigger;
            _plugOverride = plugOverride;
        }

        public override string ToString()
        {
            return id.ToString() + " for " + (Trigger != null ? Trigger.LogName : "(no trigger)") + " at " + triggeredTime.ToString();
        }

        internal Context Duplicate()
        {
            Context ctx = (Context)MemberwiseClone();
            return ctx;
        }

        internal void PushActionResult(int i)
        {
            lock (ActionResults)
            {
                ActionResults.Add(i);
            }
        }

        internal int PeekActionResult(bool previous, int i)
        {
            lock (ActionResults)
            {
                if (previous == true)
                {
                    if (ActionResults.Count > 0)
                    {
                        return ActionResults[ActionResults.Count - 1];
                    }
                    return 0;
                }
                else
                {
                    if (i < 1 || i > ActionResults.Count)
                    {
                        return 0;
                    }
                    return ActionResults[i - 1];
                }
            }
        }

        internal void RecordCaptureGroups(Trigger trigger, Match match)
        {
            _numRegexGroups = new List<string>();
            foreach (int idx in trigger.regexCache.GetGroupNumbers())
            {
                string capturedValue = match?.Groups[idx].Value ?? "";
                _numRegexGroups.Add(capturedValue);
                Plugin?.FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/debugnumgroup",
                    "Trigger '{0}' numbered group {1}: {2}", trigger.LogName, idx, capturedValue));
            }

            _namedRegexGroups = new Dictionary<string, string>();
            foreach (string name in trigger.regexCache.GetGroupNames())
            {
                string capturedValue = match?.Groups[name].Value ?? "";
                _namedRegexGroups[name] = capturedValue;
                Plugin?.FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/debugnamedgroup",
                    "Trigger '{0}' named group '{1}': {2}", trigger.LogName, name, capturedValue));
            }
        }

        internal string GetNumGroup(int groupIdx)
        {
            string result = null;

            if (_numRegexGroups != null)
            {
                if (groupIdx >= 0 && groupIdx < _numRegexGroups.Count)
                    result = _numRegexGroups[groupIdx];
            }
            else // never recorded: testing / skipped regex
            {
                if (Trigger?.regexCache?.GetGroupNumbers().Contains(groupIdx) == true)
                    result = "";
            }

            if (result != null && Plugin != null)
            {
                result = Plugin.cfg.PerformSubstitution(result, Substitution.SubstitutionScopeEnum.CaptureGroup);
            }
            return result;
        }

        internal string GetNamedGroup(string groupName)
        {
            string result = null;

            if (_namedRegexGroups != null)
            {
                if (_namedRegexGroups.TryGetValue(groupName, out var value))
                    result = value;
            }
            else // never recorded: testing / skipped regex
            {
                if (Trigger?.regexCache?.GetGroupNames().Contains(groupName) == true)
                    result = "";
            }

            if (result != null && Plugin != null)
            {
                result = Plugin.cfg.PerformSubstitution(result, Substitution.SubstitutionScopeEnum.CaptureGroup);
            }
            return result;
        }

        /// <summary>Return the elapsed time in seconds since this context was triggered.</summary>
        internal long SinceTriggered()
        {
            if (triggeredTime == default) return 0;
            return (long)(DateTime.UtcNow - triggeredTime).TotalSeconds;
        }

        /// <summary>Return the elapsed time in seconds since this context was triggered.</summary>
        internal long SinceTriggeredMs()
        {
            if (triggeredTime == default) return 0;
            return (long)(DateTime.UtcNow - triggeredTime).TotalMilliseconds;
        }

        /// <summary>Return the timestamp when this context was triggered in seconds.</summary>
        internal long TimestampTriggered()
        {
            if (triggeredTime == default) return 0;
            return (long)(triggeredTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>Return the timestamp when this context was triggered in milliseconds.</summary>
        internal long TimestampTriggeredMs()
        {
            if (triggeredTime == default) return 0;
            return (long)(triggeredTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public delegate void LoggerCallback(object o, string message);

        public double EvaluateNumericExpression(LoggerDelegate logger, object o, string expr)
        {
            string exp = ExpandVariables(logger, o, true, expr ?? "");
            if (Plugin != null)
            {
                exp = Plugin.cfg.PerformSubstitution(exp, Configuration.Substitution.SubstitutionScopeEnum.NumericExpression);
            }
            return MathParser.Parse(exp);
        }

        public string EvaluateStringExpression(LoggerDelegate logger, object o, string expr)
        {
            string exp = ExpandVariables(logger, o, false, expr ?? "");
            if (Plugin != null)
            {
                exp = Plugin.cfg.PerformSubstitution(exp, Configuration.Substitution.SubstitutionScopeEnum.StringExpression);
            }
            return exp;
        }

        public delegate void LoggerDelegate(object o, string msg);


        public string ExpandVariables(LoggerDelegate logger, object o, bool isNumeric, string expr)
        {
            var result = StringParser.Parse(expr, this, isNumeric);
            // log expansions: ${...} => ...
            if (Plugin?.cfg?.LogVariableExpansions == true &&
                result != expr &&
                Trigger?.GetDebugLevel(Plugin) >= RealPlugin.DebugLevelEnum.Verbose) // should not be DebugLevelEnum.Inherit here
            {
                var log = I18n.Translate("internal/Context/expansion", "Variable expansion from '{0}' to '{1}'", expr, result);
                if (logger != null)
                {
                    logger(o, log);
                }
                else
                {
                    Plugin?.FilteredAddToLog(RealPlugin.DebugLevelEnum.Verbose, log, Trigger);
                }
            }
            return result;
        }


    }

}
