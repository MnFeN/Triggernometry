using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Script execution
    /// </summary>
    [Action("Execute script", ActionAttribute.CategoryType.Programming)]
    [XmlRoot(ElementName = "ActionExecuteScript")]
    public class ActionExecuteScript : ActionBase
    {

        #region Properties

        /// <summary>
        /// Comma-separated list of referenced assemblies
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "List of assemblies to reference")]
        public string Assemblies { get; set; } = "";

        [XmlAttribute("Assemblies")]
        public string Xml_Assemblies
        {
            get => XmlAttr.String(Assemblies, "", omitWhitespace: false);
            set => Assemblies = value;
        }

        /// <summary>
        /// Script code expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Script code")]
        public string Script { get; set; } = "";

        [XmlAttribute("Script")]
        public string Xml_Script
        {
            get => XmlAttr.String(Script, "", omitWhitespace: false);
            set => Script = value;
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descexecscript", "execute C# script");
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string scp = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Script);
            string assy = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Assemblies);
            while (plug.scriptingInited == false)
            {
                Thread.Sleep(10);
            }
            if (plug?.scripting.Ready == true)
            {
                plug.scripting.Evaluate(scp, assy, ctx);
            }
            else
            {
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/scriptinifailed", "Action #{0} on trigger '{1}' not fired, scripting not available", IndexInTrigger, ctx.Trigger?.LogName ?? "(null)"));
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionExecuteScript(ActionOld oldAction)
        {
            var action = new ActionExecuteScript();
            oldAction.CopyCommonPropertiesTo(action);
            action.Assemblies = oldAction._ExecScriptAssembliesExpression;
            action.Script = oldAction._ExecScriptExpression;
            return action;
        }

        #endregion Old Action Converter

    }

}
