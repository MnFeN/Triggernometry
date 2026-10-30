using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{
    /// <summary>解析文本表达式并显示现有的滑动弹窗。</summary>
    [Action("Show TraySlider", ActionAttribute.CategoryType.Display)]
    [XmlRoot(ElementName = "ActionTraySlider")]
    public class ActionTraySlider : ActionBase
    {
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Text")]
        public string Text { get; set; } = "";

        [XmlAttribute("Text")]
        public string Xml_Text
        {
            get => XmlAttr.String(Text, "", omitWhitespace: false);
            set => Text = value;
        }

        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(2, Label = "Message level", ChoiceLabels = new[] { "Information", "Warning", "Error" })]
        public TraySliderLevel Level { get; set; } = TraySliderLevel.Info;

        [XmlAttribute("Level")]
        public string Xml_Level
        {
            get => XmlAttr.Enum(Level, TraySliderLevel.Info);
            set => Level = XmlAttr.Enum<TraySliderLevel>(value);
        }

        /// <summary>显示文本表达式和消息等级。</summary>
        internal override string DescribeImplementation()
            => I18n.Translate("internal/Action/desctrayslider" + Level,
                "show a tray notification ({0}), level (" + Level + ")", Text);

        /// <summary>沿用具名回调的标题拆分、确认按钮和线程安全显示逻辑。</summary>
        internal override void ExecuteImplementation(Context ctx)
        {
            var (title, message) = TraySlider.ParseTitleAndMessage(EvalString(Text, ctx));
            new TraySlider(1, message, title, Level).Show();
        }
    }
}
