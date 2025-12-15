using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Script execution
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "ExecuteScript")]
    internal class ActionExecuteScript : ActionBase
    {

        #region Properties

        /// <summary>
        /// Comma-separated list of referenced assemblies
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public string Assemblies { get; set; } = "";

        [XmlAttribute("Assemblies")]
        public string Xml_Assemblies
        {
            get => XmlAttr.String(Assemblies);
            set => Assemblies = value;
        }

        /// <summary>
        /// Script code expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Script { get; set; } = "";

        [XmlAttribute("Script")]
        public string Xml_Script
        {
            get => XmlAttr.String(Script);
            set => Script = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descexecscript", "execute C# script");
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string scp = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Script);
            string assy = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Assemblies);
            while (ctx.Plugin.scriptingInited == false)
            {
                Thread.Sleep(10);
            }
            if (ctx.Plugin?.scripting.Ready == true)
            {
                ctx.Plugin.scripting.Evaluate(scp, assy, ctx);
            }
            else
            {
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/scriptinifailed", "Action #{0} on trigger '{1}' not fired, scripting not available", OrderNumber, ctx.Trigger?.LogName ?? "(null)"));
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

        // (ActionOld)this
        public static explicit operator ActionOld(ActionExecuteScript action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.ExecuteScript;
            oldAction._ExecScriptAssembliesExpression = action.Assemblies;
            oldAction._ExecScriptExpression = action.Script;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
