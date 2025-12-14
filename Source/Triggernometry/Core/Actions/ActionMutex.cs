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

        internal override string DescribeImplementation(Context ctx)
        {
            switch (Operation)
            {
                case OperationEnum.Release:
                    return I18n.Translate("internal/Action/mutexrelease", "release mutex ({0})", Name);
                case OperationEnum.Acquire:
                    return I18n.Translate("internal/Action/mutexacquire", "acquire mutex ({0})", Name);
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            string mn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            switch (Operation)
            {
                case OperationEnum.Acquire:
                    {
                        RealPlugin.MutexInformation mi = ctx.Plugin.GetMutex(mn);
                        mi.Acquire(ctx);
                    }
                    break;
                case OperationEnum.Release:
                    {
                        RealPlugin.MutexInformation mi = ctx.Plugin.GetMutex(mn);
                        mi.Release(ctx);
                    }
                    break;
            }
        }

        #endregion

    }

}
