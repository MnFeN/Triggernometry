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
        public bool BoolParam { get; set; } = false;

        [XmlAttribute("BoolParam")]
        public string Xml_BoolParam
        {
            get => XmlAttr.Bool(BoolParam, false);
            set => BoolParam = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        [Action(order: 3)]
        public string StringParam { get; set; } = "";

        [XmlAttribute("StringParam")]
        public string Xml_StringParam
        {
            get => XmlAttr.String(StringParam);
            set => StringParam = value;
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.SetCombatState:
                    return BoolParam == false 
                        ? I18n.Translate("internal/Action/descactcombatend", "end ACT encounter")
                        : I18n.Translate("internal/Action/descactcombatstart", "start ACT encounter");                    
                case OperationEnum.LogAllNetwork:
                    return I18n.Translate("internal/Action/descactlogallnetwork", "{0} option: Log all network data", I18n.TranslateEnable(BoolParam));
                case OperationEnum.UseDeucalion:
                    return I18n.Translate("internal/Action/descactusedeucalion", "{0} option: Use Deucalion (injection)", I18n.TranslateEnable(BoolParam));
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }
        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;
            switch (Operation)
            {
                case OperationEnum.SetCombatState:
                    plug.SetCombatStateHook(BoolParam);
                    break;
                case OperationEnum.LogAllNetwork:
                    PluginBridges.BridgeFFXIV.LogAllNetwork(BoolParam);
                    break;
                case OperationEnum.UseDeucalion:
                    PluginBridges.BridgeFFXIV.UseDeucalion(BoolParam);
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion Implementation

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionActInteraction(ActionOld oldAction)
        {
            var action = new ActionActInteraction();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._ActOpType;
            action.BoolParam = oldAction._ActOpBoolParam;
            action.StringParam = oldAction._ActOpStringParam;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionActInteraction action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.ActInteraction;
            oldAction._ActOpType = (ActionOld.ActInteractionTypeEnum)(int)action.Operation;
            oldAction._ActOpBoolParam = action.BoolParam;
            oldAction._ActOpStringParam = action.StringParam;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
