using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Named callback invocation
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.RemoteControl)]
    [XmlRoot(ElementName = "NamedCallback")]
    internal class ActionNamedCallback : ActionBase
    {

        #region Properties

        /// <summary>
        /// Name of the callback to invoke
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name);
            set => Name = value;
        }

        /// <summary>
        /// Parameter value to pass to the callback
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Parameter { get; set; } = "";

        [XmlAttribute("Parameter")]
        public string Xml_Parameter
        {
            get => XmlAttr.String(Parameter);
            set => Parameter = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            return I18n.Translate("internal/Action/descnamedcallback", "Invoke named callback ({0}) with parameter ({1})", Name, Parameter);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            string cbname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            string cbparm = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Parameter);
            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/callbackinvoke", "Invoking named callback ({0}) with parameter ({1})", cbname, cbparm));
            ctx.Plugin.InvokeNamedCallback(cbname, cbparm);
        }

        #endregion

    }

}
