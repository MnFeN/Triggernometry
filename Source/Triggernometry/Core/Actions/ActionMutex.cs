using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Mutex operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "Mutex")]
    internal class ActionMutex : ActionBase
    {

        #region Properties

        /// <summary>
        /// Mutex operations
        /// </summary>
        public enum OperationEnum
        {
            Release,
            Acquire
        }

        /// <summary>
        /// Type of the mutex operation
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Release;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Release);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Name of the mutex
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name);
            set => Name = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.Release:
                    return I18n.Translate("internal/Action/mutexrelease", "release mutex ({0})", Name);
                case OperationEnum.Acquire:
                    return I18n.Translate("internal/Action/mutexacquire", "acquire mutex ({0})", Name);
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string mn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            switch (Operation)
            {
                case OperationEnum.Acquire:
                    {
                        RealPlugin.MutexInformation mi = plug.GetMutex(mn);
                        mi.Acquire(ctx);
                    }
                    break;
                case OperationEnum.Release:
                    {
                        RealPlugin.MutexInformation mi = plug.GetMutex(mn);
                        mi.Release(ctx);
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionMutex(ActionOld oldAction)
        {
            var action = new ActionMutex();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._MutexOpType;
            action.Name = oldAction._MutexName;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionMutex action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Mutex;
            oldAction._MutexOpType = (ActionOld.MutexOpEnum)(int)action.Operation;
            oldAction._MutexName = action.Name;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
