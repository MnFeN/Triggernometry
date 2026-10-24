using System.Xml.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Conditions
{

    /// <summary> Stores a note in condition order without participating in evaluation. </summary>
    public sealed class ConditionComment : ConditionComponent
    {

        private string _Text = "";
        [XmlAttribute]
        public string Text
        {
            get => _Text;
            set
            {
                if (_Text == value) return;
                _Text = value;
                TriggerOnPropertyChange();
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Text)
                ? I18n.Translate("internal/ConditionComment/empty", "Comment")
                : Text.Replace("\r\n", " ").Replace('\r', ' ').Replace('\n', ' ');
        }

        internal override bool CheckCondition(Context ctx, Context.LoggerDelegate logger, object o)
        {
            // ConditionGroup skips comments before evaluating its children.
            return false;
        }

        internal override ConditionComponent Duplicate()
        {
            return new ConditionComment { Enabled = Enabled, Text = Text };
        }

    }

}
