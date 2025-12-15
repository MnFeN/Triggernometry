using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Expressions.Maths;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Keypress operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Input)]
    [XmlRoot(ElementName = "Keypress")]
    internal class ActionKeypress : ActionBase
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
        [Action(order: 1)]
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
        [Action(order: 2, specialtype: ActionAttribute.SpecialTypeEnum.KeypressRecorder)]
        public string Keypress { get; set; } = "";

        [XmlAttribute("Keypress")]
        public string Xml_Keypress
        {
            get => XmlAttr.String(Keypress);
            set => Keypress = value;
        }

        /// <summary>
        /// Keycode (relevant only for window message modes)
        /// </summary>
        [XmlIgnore]
        [Action(order: 3, specialtype: ActionAttribute.SpecialTypeEnum.KeypressRecorder)]
        public string Keycode { get; set; } = "";

        [XmlAttribute("Keycode")]
        public string Xml_Keycode
        {
            get => XmlAttr.String(Keycode);
            set => Keycode = value;
        }

        /// <summary>
        /// Window title to send keypress to (relevant only for window message modes)
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string WindowTitle { get; set; } = "";

        [XmlAttribute("WindowTitle")]
        public string Xml_WindowTitle
        {
            get => XmlAttr.String(WindowTitle);
            set => WindowTitle = value;
        }

        /// <summary>
        /// Process ID to send keypress to (relevant only for window message modes)
        /// </summary>
        [XmlIgnore]
        [Action(order: 5, typehint: typeof(int))]
        public string ProcessId { get; set; } = "";

        [XmlAttribute("ProcessId")]
        public string Xml_ProcessId
        {
            get => XmlAttr.String(ProcessId);
            set => ProcessId = value;
        }

        #endregion


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
                    string title = string.IsNullOrWhiteSpace(WindowTitle) ? ".*" : WindowTitle;
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
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            switch (Operation)
            {
                case OperationEnum.SendKeys:
                    {
                        string ks = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Keypress);
                        SendKeys.SendWait(ks);
                    }
                    break;
                case OperationEnum.WindowMessage:
                    {
                        int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ProcessId);
                        string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, WindowTitle);
                        int keycode = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Keycode);
                        WindowsUtils.SendKeycode(procid, window, keycode);
                    }
                    break;
                case OperationEnum.WindowMessageCombo:
                    {
                        int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ProcessId);
                        string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, WindowTitle);
                        int[] keycodes = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Keycode)
                            .Split(',').Select(kx => Convert.ToInt32(kx.Trim())).ToArray();
                        WindowsUtils.SendKeycodes(procid, window, keycodes);
                    }
                    break;
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
            action.WindowTitle = oldAction._KeyPressWindow;
            action.ProcessId = oldAction._KeyPressProcId;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionKeypress action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.KeyPress;
            oldAction._KeypressType = (ActionOld.KeypressTypeEnum)(int)action.Operation;
            oldAction._KeyPressExpression = action.Keypress;
            oldAction._KeyPressCode = action.Keycode;
            oldAction._KeyPressWindow = action.WindowTitle;
            oldAction._KeyPressProcId = action.ProcessId;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
