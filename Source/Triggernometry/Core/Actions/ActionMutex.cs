using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Mutex operations
    /// </summary>
    [Action("Mutex operation", ActionAttribute.CategoryType.Programming)]
    [XmlRoot(ElementName = "ActionMutex")]
    public class ActionMutex : ActionBase
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
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation type",
            ChoiceLabels = new[]
        {
            "Release mutex",
            "Acquire mutex",
        })]
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
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Mutex name")]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name, "", omitWhitespace: false);
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

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string mutexName = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            switch (Operation)
            {
                case OperationEnum.Acquire:
                    {
                        RealPlugin.MutexInfo mi = plug.GetMutex(mutexName);
                        mi.Acquire(ctx);
                    }
                    break;
                case OperationEnum.Release:
                    {
                        RealPlugin.MutexInfo mi = plug.GetMutex(mutexName);
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

        #endregion Old Action Converter

    }

}
