using System;
using System.Xml.Serialization;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{
    /// <summary>Stores legacy action XML settings and converts them to concrete runtime actions.</summary>
    [XmlType("Action")]
    public partial class ActionOld
    {

        #region General properties



        /// <summary>
        /// for queue controlling
        /// </summary>
        internal Guid Id { get; set; } = Guid.NewGuid();

        [XmlIgnore]
        public bool Enabled { get; set; } = true;

        [XmlAttribute("Enabled")]
        public string Xml_Enabled
        {
            get => XmlAttr.Bool(Enabled, true);
            set => Enabled = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public Trigger ParentTrigger { get; set; } = null;



        [XmlIgnore]
        public ActionTypeEnum ActionType = ActionTypeEnum.SystemBeep;

        [XmlAttribute("ActionType")]
        public string Xml_ActionType
        {
            get => XmlAttr.Enum(ActionType, ActionTypeEnum.SystemBeep);
            set
            {
                if (XmlAttr.TryEnum(value, out ActionType)) return;

                if (value == "EndEncounter") // convert old actions
                {
                    ActionType = ActionTypeEnum.ActInteraction;
                    _ActOpBoolParam = false;
                    return;
                }

                throw InvalidEnumException("ActionTypeEnum", value);
            }
        }



        [XmlIgnore]
        public string ExecutionDelayExpression { get; set; } = "0";

        [XmlAttribute("ExecutionDelayExpression")]
        public string Xml_ExecutionDelayExpression
        {
            get => XmlAttr.String(ExecutionDelayExpression, "0");
            set => ExecutionDelayExpression = value;
        }



        [XmlIgnore]
        public bool Asynchronous { get; set; } = true;

        [XmlAttribute("Asynchronous")]
        public string Xml_Asynchronous
        {
            get => XmlAttr.Bool(Asynchronous, true);
            set => Asynchronous = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public DebugLevelEnum DebugLevel { get; set; } = DebugLevelEnum.Inherit;

        [XmlAttribute("DebugLevel")]
        public string Xml_DebugLevel
        {
            get => XmlAttr.Enum(DebugLevel, DebugLevelEnum.Inherit);
            set => DebugLevel = XmlAttr.Enum<DebugLevelEnum>(value);
        }



        [XmlIgnore]
        public bool RefireInterrupt { get; set; } = false;

        [XmlAttribute("RefireInterrupt")]
        public string Xml_RefireInterrupt
        {
            get => XmlAttr.Bool(RefireInterrupt, false);
            set => RefireInterrupt = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public bool RefireRequeue { get; set; } = true;

        [XmlAttribute("RefireRequeue")]
        public string Xml_RefireRequeue
        {
            get => XmlAttr.Bool(RefireRequeue, true);
            set => RefireRequeue = XmlAttr.Bool(value);
        }



        [XmlAttribute]
        public int OrderNumber;



        [XmlIgnore]
        public string Description { get; set; } = "";

        [XmlAttribute("Description")]
        public string Xml_Description
        {
            get => XmlAttr.String(Description);
            set => Description = value;
        }



        [XmlIgnore]
        public string DescBgColor { get; set; } = "";

        [XmlAttribute("DescBgColor")]
        public string Xml_DescBgColor
        {
            get => XmlAttr.String(DescBgColor);
            set => DescBgColor = value;
        }



        [XmlIgnore]
        public string DescTextColor { get; set; } = "";

        [XmlAttribute("DescTextColor")]
        public string Xml_DescTextColor
        {
            get => XmlAttr.String(DescTextColor);
            set => DescTextColor = value;
        }



        [XmlIgnore]
        public bool DescriptionOverride { get; set; } = false;

        [XmlAttribute("DescriptionOverride")]
        public string Xml_DescriptionOverride
        {
            get => XmlAttr.Bool(DescriptionOverride, false);
            set => DescriptionOverride = XmlAttr.Bool(value);
        }



        /// <summary>A string tag used in trigger/folder actions to interrupt specific actions.</summary>
        [XmlIgnore]
        public string Tag { get; set; }

        [XmlAttribute("Tag")]
        public string Xml_Tag
        {
            get => XmlAttr.String(Tag);
            set => Tag = value;
        }



        [XmlIgnore]
        public ConditionGroup Condition = new ConditionGroup { Enabled = false };

        [XmlElement("Condition")]
        public ConditionGroup Xml_Condition
        {
            get => Condition.Children.Count == 0 && !Condition.Enabled ? null : Condition;
            set => Condition = value ?? new ConditionGroup { Enabled = false };
        }

        #endregion

        public static ArgumentException InvalidEnumException(string enumName, string enumValue)
        { 
            return new ArgumentException(
                I18n.Translate(
                    "internal/Action/invalidEnumType",
                    "{0} = {1} is not a known enum type.\n\n" +
                    "This may be because your Triggernometry plugin is not up to date, or the data you are trying to import is corrupted.",
                    enumName, enumValue)
                );
        }

        internal void CopyCommonPropertiesTo(ActionBase action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action.Enabled = Enabled;
            action.Id = Id;
            action.ParentTrigger = ParentTrigger;
            action.Condition = (ConditionGroup)Condition?.Duplicate();
            action.Tag = Tag;
            action.RefireInterrupt = RefireInterrupt;
            action.RefireRequeue = RefireRequeue;
            action.ExecutionDelayExpression = ExecutionDelayExpression;
            action.Asynchronous = Asynchronous;
            action.DebugLevel = DebugLevel;
            action.Description = Description;
            action.DescBgColor = DescBgColor;
            action.DescTextColor = DescTextColor;
            action.DescriptionOverride = DescriptionOverride;
        }

        internal ActionBase ConvertToNewAction()
        {
            switch (ActionType)
            {
                case ActionTypeEnum.ActInteraction:
                    return (ActionActInteraction)this;
                case ActionTypeEnum.SystemBeep:
                    return (ActionBeep)this;
                case ActionTypeEnum.DiscordWebhook:
                    return (ActionDiscordWebhook)this;
                case ActionTypeEnum.DiskFile:
                    return (ActionDiskOperation)this;
                case ActionTypeEnum.ExecuteScript:
                    return (ActionExecuteScript)this;
                case ActionTypeEnum.Folder:
                    return (ActionFolderOperation)this;
                case ActionTypeEnum.GenericJson:
                    return (ActionJsonRequest)this;
                case ActionTypeEnum.KeyPress:
                    return (ActionKeypress)this;
                case ActionTypeEnum.LaunchProcess:
                    return (ActionLaunchProcess)this;
                case ActionTypeEnum.LiveSplitControl:
                    return (ActionLiveSplitControl)this;
                case ActionTypeEnum.LogMessage:
                    return (ActionLogMessage)this;
                case ActionTypeEnum.Loop:
                    return (ActionLoop)this;
                case ActionTypeEnum.MessageBox:
                    return (ActionMessageBox)this;
                case ActionTypeEnum.Mouse:
                    return (ActionMouse)this;
                case ActionTypeEnum.Mutex:
                    return (ActionMutex)this;
                case ActionTypeEnum.NamedCallback:
                    return (ActionNamedCallback)this;
                case ActionTypeEnum.ObsControl:
                    return (ActionObsControl)this;
                case ActionTypeEnum.Aura:
                    return (ActionOverlayImage)this;
                case ActionTypeEnum.TextAura:
                    return (ActionOverlayText)this;
                case ActionTypeEnum.Placeholder:
                    return (ActionPlaceholder)this;
                case ActionTypeEnum.PlaySound:
                    return (ActionPlaySound)this;
                case ActionTypeEnum.UseTTS:
                    return (ActionPlaySpeech)this;
                case ActionTypeEnum.Repository:
                    return (ActionRepository)this;
                case ActionTypeEnum.Trigger:
                    return (ActionTriggerOperation)this;
                case ActionTypeEnum.DictVariable:
                    return (ActionVariableDict)this;
                case ActionTypeEnum.ListVariable:
                    return (ActionVariableList)this;
                case ActionTypeEnum.Variable:
                    return (ActionVariableScalar)this;
                case ActionTypeEnum.TableVariable:
                    return (ActionVariableTable)this;
                case ActionTypeEnum.WindowMessage:
                    return (ActionWindowMessage)this;

                default:
                    throw new NotSupportedException(I18n.Translate("internal/Action/unknowntype", "Unknown ActionType: {0}", ActionType));
            }
        }

    }

}
