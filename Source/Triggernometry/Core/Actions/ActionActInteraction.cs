using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// ACT interaction operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.RemoteControl)]
    [XmlRoot(ElementName = "ActInteraction")]
    public class ActionActInteraction : ActionBase
    {

        #region Properties

        /// <summary>
        /// ACT interaction operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Set ACT combat state
            /// </summary>
            SetCombatState,
            /// <summary>
            /// Toggle all network logging
            /// </summary>
            LogAllNetwork,
            /// <summary>
            /// Toggle Deucalion usage
            /// </summary>
            UseDeucalion,
        }

        /// <summary>
        /// Type of ACT interaction
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public OperationEnum Operation { get; set; } = OperationEnum.SetCombatState;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.SetCombatState);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Value to set
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value);
            set => Value = value;
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            switch (Operation)
            {
                case OperationEnum.SetCombatState:
                    return bool.Parse(Value) == false ? 
                        I18n.Translate("internal/Action/descactcombatend", "end ACT encounter")
                        :
                        I18n.Translate("internal/Action/descactcombatstart", "start ACT encounter");                    
                case OperationEnum.LogAllNetwork:
                    return I18n.Translate("internal/Action/descactlogallnetwork", "{0} option: Log all network data", I18n.TranslateEnable(bool.Parse(Value)));
                case OperationEnum.UseDeucalion:
                    return I18n.Translate("internal/Action/descactusedeucalion", "{0} option: Use Deucalion (injection)", I18n.TranslateEnable(bool.Parse(Value)));
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            RealPlugin plug = ai.ctx.Plugin;
            switch (Operation)
            {
                case OperationEnum.SetCombatState:
                    plug.SetCombatStateHook(bool.Parse(Value));
                    break;
                case OperationEnum.LogAllNetwork:
                    PluginBridges.BridgeFFXIV.LogAllNetwork(bool.Parse(Value));
                    break;
                case OperationEnum.UseDeucalion:
                    PluginBridges.BridgeFFXIV.UseDeucalion(bool.Parse(Value));
                    break;
            }
        }

        #endregion

    }

}
