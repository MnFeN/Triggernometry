using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Send window message
    /// </summary>
    [Action("Send window message", ActionAttribute.CategoryType.SystemInteraction)]
    [XmlRoot(ElementName = "ActionWindowMessage")]
    public class ActionWindowMessage : ActionBase
    {

        #region Properties

        /// <summary>
        /// Process ID to send window message to.
        /// If -1, message is sent to all windows in all windows with a matching title in all processes.
        /// If 0, message is sent to first window with a matching title in all processes.
        /// If >0, message is sent to all windows in all processes with matching title in the given process.
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, EditorExpressionType.Numeric, Label = "Process ID")]
        public string ProcessId { get; set; } = "";

        [XmlAttribute("ProcessId")]
        public string Xml_ProcessId
        {
            get => XmlAttr.String(ProcessId, "", omitWhitespace: false);
            set => ProcessId = value;
        }

        /// <summary>
        /// Window title to send window message to
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, EditorExpressionType.Regex, Label = "Window title")]
        public string WinTitleRegex { get; set; } = "";

        [XmlAttribute("WinTitleRegex")]
        public string Xml_WinTitleRegex
        {
            get => XmlAttr.String(WinTitleRegex, "", omitWhitespace: false);
            set => WinTitleRegex = value;
        }

        /// <summary>
        /// Id of the window message
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(3, EditorExpressionType.Numeric, Label = "Message code")]
        public string MessageId { get; set; } = "";

        [XmlAttribute("MessageId")]
        public string Xml_MessageId
        {
            get => XmlAttr.String(MessageId, "", omitWhitespace: false);
            set => MessageId = value;
        }

        /// <summary>
        /// Wparam of the window message
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(4, EditorExpressionType.Numeric, Label = "WParam")]
        public string Wparam { get; set; } = "";

        [XmlAttribute("Wparam")]
        public string Xml_Wparam
        {
            get => XmlAttr.String(Wparam, "", omitWhitespace: false);
            set => Wparam = value;
        }

        /// <summary>
        /// Lparam of the window message
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, EditorExpressionType.Numeric, Label = "LParam")]
        public string Lparam { get; set; } = "";

        [XmlAttribute("Lparam")]
        public string Xml_Lparam
        {
            get => XmlAttr.String(Lparam, "", omitWhitespace: false);
            set => Lparam = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string target;
            ProcessId = ProcessId.Trim();
            if (WinTitleRegex.Trim().Length == 0)
            {
                // the same condition check as in WindowsUtils.FindWindowsByTitleRegex
                target = I18n.Translate("internal/Action/descwindowtargetnone", "(unspecified window name)");
            }
            if (ProcessId == "" || ProcessId == "0")
            {
                target = I18n.Translate("internal/Action/descwindowtargetsingle", "the first window whose title match ({0})", WinTitleRegex);
            }
            else if (ProcessId == "-1")
            {
                target = I18n.Translate("internal/Action/descwindowtargetall", "all windows whose titles match ({0})", WinTitleRegex);
            }
            else
            {
                target = I18n.Translate("internal/Action/descwindowtargetid", "windows in the process id ({0}) whose titles match ({1})", ProcessId, WinTitleRegex);
            }
            return I18n.Translate("internal/Action/descwmsg", "Send message ({0}) wparam ({1}) lparam ({2}) to {3}", MessageId, Wparam, Lparam, target);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ProcessId);
            string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, WinTitleRegex);
            int code = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, MessageId);
            IntPtr wparam = (IntPtr)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Wparam);
            IntPtr lparam = (IntPtr)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Lparam);
            WindowsUtils.SendMessageToWindow(procid, window, (ushort)code, wparam, lparam);
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionWindowMessage(ActionOld oldAction)
        {
            var action = new ActionWindowMessage();
            oldAction.CopyCommonPropertiesTo(action);
            action.ProcessId = oldAction._WmsgProcId;
            action.WinTitleRegex = oldAction._WmsgTitle;
            action.MessageId = oldAction._WmsgCode;
            action.Wparam = oldAction._WmsgWparam;
            action.Lparam = oldAction._WmsgLparam;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionWindowMessage action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.WindowMessage;
            oldAction._WmsgProcId = action.ProcessId;
            oldAction._WmsgTitle = action.WinTitleRegex;
            oldAction._WmsgCode = action.MessageId;
            oldAction._WmsgWparam = action.Wparam;
            oldAction._WmsgLparam = action.Lparam;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
