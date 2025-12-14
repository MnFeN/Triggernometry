using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Placeholder (noop)
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "Placeholder")]
    public class ActionPlaceholder : ActionBase
    {

        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descplaceholder", "Placeholder");
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            // nothing to execute            
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionPlaceholder(ActionOld oldAction)
        {
            var action = new ActionPlaceholder();
            oldAction.CopyCommonPropertiesTo(action);
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionPlaceholder action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Placeholder;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
