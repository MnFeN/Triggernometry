using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.PluginBridges.ExternalTools;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// LiveSplit remote control operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.RemoteControl)]
    [XmlRoot(ElementName = "LiveSplitControl")]
    internal class ActionLiveSplitControl : ActionBase
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
        [Action(order: 1)]
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
        [Action(order: 2)]
        public string CustomPayload { get; set; } = "";

        [XmlAttribute("CustomPayload")]
        public string Xml_CustomPayload
        {
            get => /*_Operation != OperationEnum.CustomPayload ? null :*/ XmlAttr.String(CustomPayload);
            set => CustomPayload = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
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
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            LiveSplitController livesplitController = ctx.Plugin._livesplit;
            if (livesplitController != null)
            {
                lock (livesplitController)
                {
                    if (LiveSplitConnector(ctx) == true)
                    {
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
                                    string lscommand = ctx.EvaluateStringExpression(ActionContextLogger, ctx, CustomPayload);
                                    livesplitController.SendCommand(lscommand);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/lscontrolexception", "Can't execute LiveSplit control action due to exception: " + ex.Message));
                        }
                    }
                    else
                    {
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/lscontrolerror", "Can't execute LiveSplit control action due to error"));
                    }
                }
            }
        }

        #endregion

    }

}
