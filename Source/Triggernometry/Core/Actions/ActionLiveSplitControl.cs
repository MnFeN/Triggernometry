using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.PluginBridges.ExternalTools;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// LiveSplit remote control operations
    /// </summary>
    [Action("LiveSplit remote control operation", ActionAttribute.CategoryType.ExternalInteraction)]
    [XmlRoot(ElementName = "ActionLiveSplitControl")]
    public class ActionLiveSplitControl : ActionBase
    {

        #region Properties

        /// <summary>
        /// LiveSplit remote control operations
        /// </summary>
        public enum OperationEnum
        {
            StartOrSplit,
            Start,
            Split,
            UndoSplit,
            SkipSplit,
            Reset,
            Pause,
            Resume,
            CustomPayload
        }

        /// <summary>
        /// Type of the LiveSplit control operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation type",
            ChoiceLabels = new[]
        {
            "Start run or split",
            "Start run",
            "Split",
            "Undo split",
            "Skip split",
            "Reset run",
            "Pause run",
            "Resume run",
            "Custom payload",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.StartOrSplit;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.StartOrSplit);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Custom payload to send to LiveSplit
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Custom payload")]
        public string CustomPayload { get; set; } = "";

        [XmlAttribute("CustomPayload")]
        public string Xml_CustomPayload
        {
            get => XmlAttr.String(CustomPayload, "", omitWhitespace: false);
            set => CustomPayload = value;
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var payload = editor.GetEditor(nameof(CustomPayload));
            return () => editor.SetEnabled((OperationEnum)operation.GetValue() == OperationEnum.CustomPayload, payload);
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.StartOrSplit:
                    return I18n.Translate("internal/Action/desclsstartorsplit", "Start run or split on LiveSplit");
                case OperationEnum.Start:
                    return I18n.Translate("internal/Action/desclsstart", "Start run on LiveSplit");
                case OperationEnum.Split:
                    return I18n.Translate("internal/Action/desclssplit", "Split on LiveSplit");
                case OperationEnum.UndoSplit:
                    return I18n.Translate("internal/Action/desclsundosplit", "Undo split on LiveSplit");
                case OperationEnum.SkipSplit:
                    return I18n.Translate("internal/Action/desclsskipsplit", "Skip split on LiveSplit");
                case OperationEnum.Reset:
                    return I18n.Translate("internal/Action/desclsreset", "Reset run on LiveSplit");
                case OperationEnum.Pause:
                    return I18n.Translate("internal/Action/desclspause", "Pause run on LiveSplit");
                case OperationEnum.Resume:
                    return I18n.Translate("internal/Action/desclsresume", "Resume run on LiveSplit");
                case OperationEnum.CustomPayload:
                    return I18n.Translate("internal/Action/desclscustompayload", "Send custom payload to LiveSplit");
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            LiveSplitController livesplitController = plug._livesplit;
            if (livesplitController == null) return;

            lock (livesplitController)
            {
                if (LiveSplitConnector(ctx) != true)
                {
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/lscontrolerror", "Can't execute LiveSplit control action due to error"));
                    return;
                }
                try
                {
                    switch (Operation)
                    {
                        case OperationEnum.StartOrSplit:
                            livesplitController.StartOrSplit();
                            break;
                        case OperationEnum.Start:
                            livesplitController.Start();
                            break;
                        case OperationEnum.Split:
                            livesplitController.Split();
                            break;
                        case OperationEnum.UndoSplit:
                            livesplitController.UndoSplit();
                            break;
                        case OperationEnum.SkipSplit:
                            livesplitController.SkipSplit();
                            break;
                        case OperationEnum.Reset:
                            livesplitController.Reset();
                            break;
                        case OperationEnum.Pause:
                            livesplitController.Pause();
                            break;
                        case OperationEnum.Resume:
                            livesplitController.Resume();
                            break;
                        case OperationEnum.CustomPayload:
                            string lscommand = EvalString(CustomPayload, ctx);
                            livesplitController.SendCommand(lscommand);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/lscontrolexception", "Can't execute LiveSplit control action due to exception: " + ex.Message));
                }
            }
        }

        internal bool LiveSplitConnector(Context ctx)
        {
            var liveSplitController = ctx.Plugin._livesplit;
            lock (liveSplitController)
            {
                if (liveSplitController.IsConnected == true)
                {
                    return true;
                }
                try
                {
                    liveSplitController.Connect();
                    AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Action/lsconnectok", "LiveSplit connected successfully"));
                    return true;
                }
                catch (Exception ex)
                {
                    AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/lsconnecterror", "Error connecting to LiveSplit: {0}", ex.Message));
                }
            }
            return false;
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionLiveSplitControl(ActionOld oldAction)
        {
            var action = new ActionLiveSplitControl();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._LSControlType;
            action.CustomPayload = oldAction._LSCustomPayload;
            return action;
        }

        #endregion Old Action Converter

    }

}
