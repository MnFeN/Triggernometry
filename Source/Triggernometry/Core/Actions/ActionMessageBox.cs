using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Message box
    /// </summary>
    [Action("Show message box", ActionAttribute.CategoryType.Display)]
    [XmlRoot(ElementName = "ActionMessageBox")]
    public class ActionMessageBox : ActionBase
    {

        #region Properties

        #region Properties

        /// <summary>
        /// Message box icon types
        /// </summary>
        public enum MessageBoxIconEnum
        {
            None = 0,
            Error = 16,
            Question = 32,
            Warning = 48,
            Information = 64
        }

        /// <summary>
        /// Icon to display on message box
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Message box icon",
            ChoiceLabels = new[]
        {
            "None",
            "Error",
            "Question",
            "Warning",
            "Information",
        }, ChoiceValues = new[] { 0, 16, 32, 48, 64 })]
        public MessageBoxIconEnum Icon { get; set; } = MessageBoxIconEnum.None;

        [XmlAttribute("Icon")]
        public string Xml_Icon
        {
            get => XmlAttr.Enum(Icon, MessageBoxIconEnum.None);
            set => Icon = XmlAttr.Enum<MessageBoxIconEnum>(value);
        }

        /// <summary>
        /// Text to display on message box
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Text to display")]
        public string Text { get; set; } = "";

        [XmlAttribute("Text")]
        public string Xml_Text
        {
            get => XmlAttr.String(Text, "", omitWhitespace: false);
            set => Text = value;
        }

        #endregion


        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate($"internal/Action/descmsgbox{Icon}", "show a message box saying ({0}) with icon (" + Icon.ToString() + ")", Text);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            Form activeForm = Form.ActiveForm;
            if (activeForm != null)
            {
                MessageBox.Show(activeForm, ctx.EvaluateStringExpression(ActionContextLogger, ctx, Text), "", MessageBoxButtons.OK, (MessageBoxIcon)Icon);
            }
            else
            {
                MessageBox.Show(ctx.EvaluateStringExpression(ActionContextLogger, ctx, Text), "", MessageBoxButtons.OK, (MessageBoxIcon)Icon);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionMessageBox(ActionOld oldAction)
        {
            var action = new ActionMessageBox();
            oldAction.CopyCommonPropertiesTo(action);
            action.Icon = (MessageBoxIconEnum)(int)oldAction._MessageBoxIconType;
            action.Text = oldAction._MessageBoxText;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionMessageBox action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.MessageBox;
            oldAction._MessageBoxIconType = (ActionOld.MessageBoxIconTypeEnum)(int)action.Icon;
            oldAction._MessageBoxText = action.Text;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
