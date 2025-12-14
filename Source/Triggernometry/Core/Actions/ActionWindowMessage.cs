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
    [ActionCategory(ActionCategory.CategoryTypeEnum.RemoteControl)]
    [XmlRoot(ElementName = "WindowMessage")]
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
        [Action(order: 1, typehint: typeof(uint))] // -1 uint? 
        public string ProcessId { get; set; } = "";

        [XmlAttribute("ProcessId")]
        public string Xml_ProcessId
        {
            get => XmlAttr.String(ProcessId);
            set => ProcessId = value;
        }

        /// <summary>
        /// Window title to send window message to
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string WindowTitle { get; set; } = "";

        [XmlAttribute("WindowTitle")]
        public string Xml_WindowTitle
        {
            get => XmlAttr.String(WindowTitle);
            set => WindowTitle = value;
        }

        /// <summary>
        /// Id of the window message
        /// </summary>
        [XmlIgnore]
        [Action(order: 3, typehint: typeof(uint))]
        public string MessageId { get; set; } = "";

        [XmlAttribute("MessageId")]
        public string Xml_MessageId
        {
            get => XmlAttr.String(MessageId);
            set => MessageId = value;
        }

        /// <summary>
        /// Wparam of the window message
        /// </summary>
        [XmlIgnore]
        [Action(order: 4, typehint: typeof(int))]
        public string Wparam { get; set; } = "";

        [XmlAttribute("Wparam")]
        public string Xml_Wparam
        {
            get => XmlAttr.String(Wparam);
            set => Wparam = value;
        }

        /// <summary>
        /// Lparam of the window message
        /// </summary>
        [XmlIgnore]
        [Action(order: 5, typehint: typeof(int))]
        public string Lparam { get; set; } = "";

        [XmlAttribute("Lparam")]
        public string Xml_Lparam
        {
            get => XmlAttr.String(Lparam);
            set => Lparam = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            string target;
            ProcessId = ProcessId.Trim();
            if (WindowTitle.Trim().Length == 0)
            {
                // the same condition check as in WindowsUtils.FindWindowsByTitleRegex
                target = I18n.Translate("internal/Action/descwindowtargetnone", "(unspecified window name)");
            }
            if (ProcessId == "" || ProcessId == "0")
            {
                target = I18n.Translate("internal/Action/descwindowtargetsingle", "the first window whose title match ({0})", WindowTitle);
            }
            else if (ProcessId == "-1")
            {
                target = I18n.Translate("internal/Action/descwindowtargetall", "all windows whose titles match ({0})", WindowTitle);
            }
            else
            {
                target = I18n.Translate("internal/Action/descwindowtargetid", "windows in the process id ({0}) whose titles match ({1})", ProcessId, WindowTitle);
            }
            return I18n.Translate("internal/Action/descwmsg", "Send message ({0}) wparam ({1}) lparam ({2}) to {3}", MessageId, Wparam, Lparam, target);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ProcessId);
            string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, WindowTitle);
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
            action.WindowTitle = oldAction._WmsgTitle;
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
            oldAction._WmsgTitle = action.WindowTitle;
            oldAction._WmsgCode = action.MessageId;
            oldAction._WmsgWparam = action.Wparam;
            oldAction._WmsgLparam = action.Lparam;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
