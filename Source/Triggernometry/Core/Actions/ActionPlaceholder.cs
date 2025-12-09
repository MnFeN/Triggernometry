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

        internal override string DescribeImplementation(Context ctx)
        {
            return I18n.Translate("internal/Action/descplaceholder", "Placeholder");
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            // nothing to execute            
        }

        #endregion

    }

}
