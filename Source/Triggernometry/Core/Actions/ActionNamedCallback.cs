using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Named callback invocation
    /// </summary>
    [Action("Named callback operation", ActionAttribute.CategoryType.Programming)]
    [XmlRoot(ElementName = "ActionNamedCallback")]
    public class ActionNamedCallback : ActionBase
    {

        #region Properties

        /// <summary>
        /// Name of the callback to invoke
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Callback name", Autofill = EditorAutofillType.Callback)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name, "", omitWhitespace: false);
            set => Name = value;
        }

        /// <summary>
        /// Parameter value to pass to the callback
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Callback parameter")]
        public string Parameter { get; set; } = "";

        [XmlAttribute("Parameter")]
        public string Xml_Parameter
        {
            get => XmlAttr.String(Parameter, "", omitWhitespace: false);
            set => Parameter = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descnamedcallback", "Invoke named callback ({0}) with parameter ({1})", Name, Parameter);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string cbname = EvalString(Name, ctx);
            string cbparm = EvalString(Parameter, ctx);
            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/callbackinvoke", "Invoking named callback ({0}) with parameter ({1})", cbname, cbparm));
            plug.InvokeNamedCallback(cbname, cbparm);
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionNamedCallback(ActionOld oldAction)
        {
            var action = new ActionNamedCallback();
            oldAction.CopyCommonPropertiesTo(action);
            action.Name = oldAction._NamedCallbackName;
            action.Parameter = oldAction._NamedCallbackParam;
            return action;
        }

        #endregion Old Action Converter

    }

}
