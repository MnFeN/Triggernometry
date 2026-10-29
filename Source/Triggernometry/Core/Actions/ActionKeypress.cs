using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Expressions.Maths;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Keypress operations
    /// </summary>
    [Action("Send keypresses to active window", ActionAttribute.CategoryType.SystemInteraction)]
    [XmlRoot(ElementName = "ActionKeypress")]
    public class ActionKeypress : ActionBase
    {

        #region Properties

        /// <summary>
        /// Keypress operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Send by using the SendKeys API
            /// </summary>
            SendKeys,
            /// <summary>
            /// Send as a window message
            /// </summary>
            WindowMessage,
            /// <summary>
            /// Send as a series of window messages
            /// </summary>
            WindowMessageCombo
        }

        /// <summary>
        /// Type of the keypress operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Keypress send method",
            ChoiceLabels = new[]
        {
            "SendKeys",
            "Window message",
            "Multiple window messages",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.SendKeys;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.SendKeys);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Keypress expression (relevant only for SendKeys)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [KeypressEditor(3, Label = "Keypresses to send")]
        public string Keypress { get; set; } = "";

        [XmlAttribute("Keypress")]
        public string Xml_Keypress
        {
            get => XmlAttr.String(Keypress, "", omitWhitespace: false);
            set => Keypress = value;
        }

        /// <summary>
        /// Keycode (relevant only for window message modes)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [KeypressEditor(5, Label = "Keycode to send", Keycode = true)]
        public string Keycode { get; set; } = "";

        [XmlAttribute("Keycode")]
        public string Xml_Keycode
        {
            get => XmlAttr.String(Keycode, "", omitWhitespace: false);
            set => Keycode = value;
        }

        /// <summary>
        /// Window title to send keypress to (relevant only for window message modes)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(4, EditorExpressionType.Regex, Label = "Window title")]
        public string WinTitleRegex { get; set; } = "";

        [XmlAttribute("WinTitleRegex")]
        public string Xml_WinTitleRegex
        {
            get => XmlAttr.String(WinTitleRegex, "", omitWhitespace: false);
            set => WinTitleRegex = value;
        }

        /// <summary>
        /// Process ID to send keypress to (relevant only for window message modes)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, EditorExpressionType.Numeric, Label = "Process ID")]
        public string ProcessId { get; set; } = "";

        [XmlAttribute("ProcessId")]
        public string Xml_ProcessId
        {
            get => XmlAttr.String(ProcessId, "", omitWhitespace: false);
            set => ProcessId = value;
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var keys = editor.GetEditor(nameof(Keypress));
            var code = editor.GetEditor(nameof(Keycode));
            var window = editor.GetEditor(nameof(WinTitleRegex));
            var process = editor.GetEditor(nameof(ProcessId));
            return () =>
            {
                var op = (OperationEnum)operation.GetValue();
                if (op == OperationEnum.SendKeys) editor.EnableOnly(operation, keys);
                else editor.EnableOnly(operation, code, window, process);
                editor.SetExpressionType(op == OperationEnum.WindowMessageCombo ? ExpressionType.String : ExpressionType.Numeric, code);
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.SendKeys:
                    return I18n.Translate("internal/Action/desckeypresses", "send keypresses ({0}) to the active window", Keypress);                    
                case OperationEnum.WindowMessage:
                case OperationEnum.WindowMessageCombo:
                    string target;
                    ProcessId = ProcessId.Trim();
                    string title = string.IsNullOrWhiteSpace(WinTitleRegex) ? ".*" : WinTitleRegex;
                    int parsedProcId = 1;
                    try { parsedProcId = (int)MathParser.Parse(ProcessId); }
                    catch { }
                    if (parsedProcId == 0)
                    {
                        target = I18n.Translate("internal/Action/descwindowtargetsingle", "the first window whose title match ({0})", title);
                    }
                    else if (parsedProcId < 0)
                    {
                        target = I18n.Translate("internal/Action/descwindowtargetall", "all windows whose titles match ({0})", title);
                    }
                    else
                    {
                        target = I18n.Translate("internal/Action/descwindowtargetid", "windows in the process with id ({0}) whose titles match ({1})", ProcessId, title);
                    }
                    if (Operation == OperationEnum.WindowMessage)
                    {
                        return I18n.Translate("internal/Action/desckeypress", "send keycode ({0}) to {1}", Keycode, target);
                    }
                    return I18n.Translate("internal/Action/desckeypresscombo", "send keycodes ({0}) to {1}", Keycode, target);
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            switch (Operation)
            {
                case OperationEnum.SendKeys:
                    {
                        string ks = EvalString(Keypress, ctx);
                        SendKeys.SendWait(ks);
                    }
                    break;
                case OperationEnum.WindowMessage:
                    {
                        int procid = (int)EvalNumeric(ProcessId, ctx);
                        string window = EvalString(WinTitleRegex, ctx);
                        int keycode = (int)EvalNumeric(Keycode, ctx);
                        WindowsUtils.SendKeycode(procid, window, keycode);
                    }
                    break;
                case OperationEnum.WindowMessageCombo:
                    {
                        int procid = (int)EvalNumeric(ProcessId, ctx);
                        string window = EvalString(WinTitleRegex, ctx);
                        int[] keycodes = EvalString(Keycode, ctx)
                            .Split(',').Select(kx => Convert.ToInt32(kx.Trim())).ToArray();
                        WindowsUtils.SendKeycodes(procid, window, keycodes);
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionKeypress(ActionOld oldAction)
        {
            var action = new ActionKeypress();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._KeypressType;
            action.Keypress = oldAction._KeyPressExpression;
            action.Keycode = oldAction._KeyPressCode;
            action.WinTitleRegex = oldAction._KeyPressWindow;
            action.ProcessId = oldAction._KeyPressProcId;
            return action;
        }

        #endregion Old Action Converter

    }

}
