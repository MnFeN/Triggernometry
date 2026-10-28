using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Launch process
    /// </summary>
    [Action("Launch process", ActionAttribute.CategoryType.SystemInteraction)]
    [XmlRoot(ElementName = "ActionLaunchProcess")]
    public class ActionLaunchProcess : ActionBase
    {

        #region Properties

        /// <summary>
        /// Window style to launch the process with
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(4, Label = "Process window",
            ChoiceLabels = new[]
        {
            "Normal",
            "Hidden from view",
            "Minimized to taskbar",
            "Maximized to fullscreen",
        })]
        public ProcessWindowStyle WindowStyle { get; set; } = ProcessWindowStyle.Normal;

        [XmlAttribute("WindowStyle")]
        public string Xml_WindowStyle
        {
            get => XmlAttr.Enum(WindowStyle, ProcessWindowStyle.Normal);
            set => WindowStyle = XmlAttr.Enum<ProcessWindowStyle>(value);
        }

        /// <summary>
        /// Path to the process to launch
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [FileEditor(1, EditorFileType.Executable, Label = "Process to launch")]
        public string Path { get; set; } = "";

        [XmlAttribute("Path")]
        public string Xml_Path
        {
            get => XmlAttr.String(Path, "", omitWhitespace: false);
            set => Path = value;
        }

        /// <summary>
        /// Command line arguments to pass to the process
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Command line parameters")]
        public string Arguments { get; set; } = "";

        [XmlAttribute("Arguments")]
        public string Xml_Arguments
        {
            get => XmlAttr.String(Arguments, "", omitWhitespace: false);
            set => Arguments = value;
        }

        /// <summary>
        /// Working directory
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [FileEditor(3, EditorFileType.Directory, Label = "Working directory")]
        public string WorkingDirectory { get; set; } = "";

        [XmlAttribute("WorkingDirectory")]
        public string Xml_WorkingDirectory
        {
            get => XmlAttr.String(WorkingDirectory, "", omitWhitespace: false);
            set => WorkingDirectory = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string tempt = "";
            switch (WindowStyle)
            {
                case ProcessWindowStyle.Hidden:
                    tempt = I18n.Lookup($"internal/EditorFields/{nameof(ActionLaunchProcess)}/{nameof(WindowStyle)}[Hidden from view]", WindowStyle.ToString());
                    break;
                case ProcessWindowStyle.Maximized:
                    tempt = I18n.Lookup($"internal/EditorFields/{nameof(ActionLaunchProcess)}/{nameof(WindowStyle)}[Maximized to fullscreen]", WindowStyle.ToString());
                    break;
                case ProcessWindowStyle.Minimized:
                    tempt = I18n.Lookup($"internal/EditorFields/{nameof(ActionLaunchProcess)}/{nameof(WindowStyle)}[Minimized to taskbar]", WindowStyle.ToString());
                    break;
                case ProcessWindowStyle.Normal:
                    tempt = I18n.Lookup($"internal/EditorFields/{nameof(ActionLaunchProcess)}/{nameof(WindowStyle)}[Normal]", WindowStyle.ToString());
                    break;
                default:
                    return NotImplementedEnumMessage(WindowStyle);
            }
            return I18n.Translate("internal/Action/desclaunchprocess", "launch process ({0}) as ({1}) using command line parameters ({2})",
                Xml_Path,
                tempt,
                Xml_Arguments
            );
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            Process p = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Arguments);
            psi.WindowStyle = WindowStyle;
            psi.WorkingDirectory = ctx.EvaluateStringExpression(ActionContextLogger, ctx, WorkingDirectory);
            psi.FileName = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Path);
            p.StartInfo = psi;
            p.Start();
            if (Async == false)
            {
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/waitingprocexit", "Waiting for process to exit"));
                p.WaitForExit();
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionLaunchProcess(ActionOld oldAction)
        {
            var action = new ActionLaunchProcess();
            oldAction.CopyCommonPropertiesTo(action);
            action.WindowStyle = oldAction._LaunchProcessWindowStyle;
            action.Path = oldAction._LaunchProcessPathExpression;
            action.Arguments = oldAction._LaunchProcessCmdlineExpression;
            action.WorkingDirectory = oldAction._LaunchProcessWorkingDirExpression;
            return action;
        }

        #endregion Old Action Converter

    }

}
