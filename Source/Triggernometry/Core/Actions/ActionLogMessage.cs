using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Message logging
    /// </summary>
    [Action("Log message", ActionAttribute.CategoryType.Programming)]
    [XmlRoot(ElementName = "ActionLogMessage")]
    public class ActionLogMessage : ActionBase
    {

        #region Properties

        /// <summary>
        /// Log levels
        /// </summary>
        public enum LogLevel
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
        [ActionSpecificProperty]
        [EnumChoiceEditor(5, Label = "Target event source",
            ChoiceLabels = new[]
        {
            "Normal log line",
            "FFXIV network event",
            "ACT event",
        })]
        public LogEvent.SourceEnum LogSource { get; set; } = LogEvent.SourceEnum.Log;

        [XmlAttribute("LogSource")]
        public string Xml_LogSource
        {
            get => XmlAttr.Enum(LogSource, LogEvent.SourceEnum.Log);
            set => LogSource = XmlAttr.Enum<LogEvent.SourceEnum>(value);
        }

        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Message to log")]
        public string Message { get; set; } = "";

        [XmlAttribute("Message")]
        public string Xml_Message
        {
            get => XmlAttr.String(Message, "", omitWhitespace: false);
            set => Message = value;
        }

        /// <summary>
        /// If set, the logged message will be processed as if it came to Triggernometry from the specified target
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [CheckBoxEditor(2, Label = "Process message as log line")]
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
        [ActionSpecificProperty]
        [CheckBoxEditor(3, Label = "Add the log message to ACT combat log")]
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
        [ActionSpecificProperty]
        [EnumChoiceEditor(4, Label = "Level to log message on",
            ChoiceLabels = new[]
        {
            "Error",
            "Warning",
            "Custom",
            "Custom 2",
            "Info",
            "Verbose",
        })]
        public LogLevel Level { get; set; } = LogLevel.Error;

        [XmlAttribute("Level")]
        public string Xml_Level
        {
            get => XmlAttr.Enum(Level, LogLevel.Error);
            set => Level = XmlAttr.Enum<LogLevel>(value); // 原本有 if _Level == -1 => Error 的防护逻辑，但似乎当前 cbx 设计不可能产生 selectedindex = -1
        }

        #endregion

        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var process = editor.GetEditor(nameof(ProcessAsLogline));
            var target = editor.GetEditor(nameof(LogSource));
            var encounter = editor.GetEditor(nameof(AddToACTEncounter));
            var level = editor.GetEditor(nameof(Level));
            return () =>
            {
                bool isLogline = (bool)process.GetValue();
                editor.SetEnabled(isLogline, target, encounter);
                editor.SetEnabled(!isLogline, level);
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            if (ProcessAsLogline == true)
            {
                string srcType = "";
                switch (LogSource)
                {
                    case LogEvent.SourceEnum.ACT:
                        srcType = "ACT event"; break;
                    case LogEvent.SourceEnum.NetworkFFXIV:
                        srcType = "FFXIV network event"; break;
                    case LogEvent.SourceEnum.Log:
                        srcType = "Normal log line"; break;
                    default:
                        return NotImplementedEnumMessage(LogSource);
                }
                srcType = I18n.Translate($"internal/EditorFields/{nameof(ActionLogMessage)}/{nameof(LogSource)}[{srcType}]", srcType);
                return I18n.Translate(
                    "internal/Action/descprocessmessage",
                    "process message ({0}) as {1}", Message, srcType
                );
            }
            string level;
            switch (Level)
            {
                case LogLevel.Error:
                    level = "Error"; break;
                case LogLevel.Info:
                    level = "Info"; break;
                case LogLevel.Verbose:
                    level = "Verbose"; break;
                case LogLevel.Warning:
                    level = "Warning"; break;
                case LogLevel.Custom:
                    level = "Custom"; break;
                case LogLevel.Custom2:
                    level = "Custom 2"; break;
                default:
                    return NotImplementedEnumMessage(Level);
            }
            level = I18n.Translate($"internal/EditorFields/{nameof(ActionLogMessage)}/{nameof(Level)}[{level}]", level);
            return I18n.Translate(
                "internal/Action/desclogmessage",
                "log message ({0}) with {1} level", Message, level
            );
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string message = EvalString(Message, ctx);

            if (ProcessAsLogline)
            {
                string zone = EvalString(plug.currentZone, ctx);
                plug.LogLineQueuer(message, zone, LogSource);
            }
            else
            {
                RealPlugin.DebugLevelEnum debugLevel = RealPlugin.DebugLevelEnum.Error;
                switch (Level)
                {
                    case LogLevel.Custom:
                        debugLevel = RealPlugin.DebugLevelEnum.Custom; break;
                    case LogLevel.Custom2:
                        debugLevel = RealPlugin.DebugLevelEnum.Custom2; break;
                    case LogLevel.Error:
                        debugLevel = RealPlugin.DebugLevelEnum.Error; break;
                    case LogLevel.Info:
                        debugLevel = RealPlugin.DebugLevelEnum.Info; break;
                    case LogLevel.Verbose:
                        debugLevel = RealPlugin.DebugLevelEnum.Verbose; break;
                    case LogLevel.Warning:
                        debugLevel = RealPlugin.DebugLevelEnum.Warning; break;
                    default:
                        throw NotImplementedEnumException(Level);
                }
                AddToLog(ctx, debugLevel, message);
            }
            if (AddToACTEncounter)
            {
                plug.ACTEncounterLogHook(message);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionLogMessage(ActionOld oldAction)
        {
            var action = new ActionLogMessage();
            oldAction.CopyCommonPropertiesTo(action);
            action.LogSource = (LogEvent.SourceEnum)(int)oldAction._LogMessageTarget;
            action.Message = oldAction._LogMessageText;
            action.ProcessAsLogline = oldAction._LogProcess;
            action.AddToACTEncounter = oldAction._LogProcessACT;
            action.Level = (LogLevel)(int)oldAction._LogLevel;
            return action;
        }

        #endregion Old Action Converter

    }

}
