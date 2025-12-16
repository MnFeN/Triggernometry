using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Message logging
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "LogMessage")]
    internal class ActionLogMessage : ActionBase
    {

        #region Properties

        /// <summary>
        /// Log levels
        /// </summary>
        public enum LogLevelEnum
        {
            Error,
            Warning,
            Custom,
            Custom2,
            Info,
            Verbose,
        }

        /// <summary>
        /// Target stream for the log event to be inserted into
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public LogEvent.SourceEnum Target { get; set; } = LogEvent.SourceEnum.Log;

        [XmlAttribute("Target")]
        public string Xml_Target
        {
            get => XmlAttr.Enum(Target, LogEvent.SourceEnum.Log);
            set => Target = XmlAttr.Enum<LogEvent.SourceEnum>(value);
        }

        [XmlIgnore]
        [Action(order: 2)]
        public string Message { get; set; } = "";

        [XmlAttribute("Message")]
        public string Xml_Message
        {
            get => XmlAttr.String(Message);
            set => Message = value;
        }

        /// <summary>
        /// If set, the logged message will be processed as if it came to Triggernometry from the specified target
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public bool ProcessAsLogline { get; set; } = false;

        [XmlAttribute("ProcessAsLogline")]
        public string Xml_ProcessAsLogline
        {
            get => XmlAttr.Bool(ProcessAsLogline, false);
            set => ProcessAsLogline = XmlAttr.Bool(value);
        }

        /// <summary>
        /// If set, the logged message will be inserted into ACT's encounter log
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public bool AddToACTEncounter { get; set; } = false;

        [XmlAttribute("AddToACTEncounter")]
        public string Xml_AddToACTEncounter
        {
            get => XmlAttr.Bool(AddToACTEncounter, false);
            set => AddToACTEncounter = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Level for the logged message
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public LogLevelEnum Level { get; set; } = LogLevelEnum.Error;

        [XmlAttribute("Level")]
        public string Xml_Level
        {
            get => XmlAttr.Enum(Level, LogLevelEnum.Error);
            set => Level = XmlAttr.Enum<LogLevelEnum>(value); // 原本有 if _Level == -1 => Error 的防护逻辑，但似乎当前 cbx 设计不可能产生 selectedindex = -1
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
            if (ProcessAsLogline == true)
            {
                string srcType = "";
                switch (Target)
                {
                    case LogEvent.SourceEnum.ACT: srcType = "ACT event"; break;
                    case LogEvent.SourceEnum.NetworkFFXIV: srcType = "FFXIV network event"; break;
                    case LogEvent.SourceEnum.Log: srcType = "Normal log line"; break;
                    default: return NotImplementedEnumMessage(Target);
                }
                srcType = I18n.Translate($"ActionForm/cbxLogMessageTarget[{srcType}]", srcType);
                return I18n.Translate(
                    "internal/Action/descprocessmessage",
                    "process message ({0}) as {1}", Message, srcType
                );
            }
            string level;
            switch (Level)
            {
                case LogLevelEnum.Error: level = "Error"; break;
                case LogLevelEnum.Info: level = "Info"; break;
                case LogLevelEnum.Verbose: level = "Verbose"; break;
                case LogLevelEnum.Warning: level = "Warning"; break;
                case LogLevelEnum.Custom: level = "Custom"; break;
                case LogLevelEnum.Custom2: level = "Custom 2"; break;
                default: return NotImplementedEnumMessage(Level);
            }
            level = I18n.Translate($"ActionForm/cbxLogMessageLevel[{level}]", level);
            return I18n.Translate(
                "internal/Action/desclogmessage",
                "log message ({0}) with {1} level", Message, level
            );
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string message = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Message);

            if (ProcessAsLogline)
            {
                string zone = ctx.EvaluateStringExpression(ActionContextLogger, ctx, ctx.Plugin.currentZone);
                ctx.Plugin.LogLineQueuer(message, zone, Target);
            }
            else
            {
                RealPlugin.DebugLevelEnum debugLevel = RealPlugin.DebugLevelEnum.Error;
                switch (Level)
                {
                    case LogLevelEnum.Custom: debugLevel = RealPlugin.DebugLevelEnum.Custom; break;
                    case LogLevelEnum.Custom2: debugLevel = RealPlugin.DebugLevelEnum.Custom2; break;
                    case LogLevelEnum.Error: debugLevel = RealPlugin.DebugLevelEnum.Error; break;
                    case LogLevelEnum.Info: debugLevel = RealPlugin.DebugLevelEnum.Info; break;
                    case LogLevelEnum.Verbose: debugLevel = RealPlugin.DebugLevelEnum.Verbose; break;
                    case LogLevelEnum.Warning: debugLevel = RealPlugin.DebugLevelEnum.Warning; break;
                    default: throw NotImplementedEnumException(Level);
                }
                AddToLog(ctx, debugLevel, message);
            }
            if (AddToACTEncounter)
            {
                ctx.Plugin.ACTEncounterLogHook(message);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionLogMessage(ActionOld oldAction)
        {
            var action = new ActionLogMessage();
            oldAction.CopyCommonPropertiesTo(action);
            action.Target = (LogEvent.SourceEnum)(int)oldAction._LogMessageTarget;
            action.Message = oldAction._LogMessageText;
            action.ProcessAsLogline = oldAction._LogProcess;
            action.AddToACTEncounter = oldAction._LogProcessACT;
            action.Level = (LogLevelEnum)(int)oldAction._LogLevel;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionLogMessage action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.LogMessage;
            oldAction._LogMessageTarget = (LogEvent.SourceEnum)(int)action.Target;
            oldAction._LogMessageText = action.Message;
            oldAction._LogProcess = action.ProcessAsLogline;
            oldAction._LogProcessACT = action.AddToACTEncounter;
            oldAction._LogLevel = (ActionOld.LogMessageEnum)(int)action.Level;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
