using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;
using Triggernometry.UI.CustomControls;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Folder operations
    /// </summary>
    [Action("Folder operation", ActionAttribute.CategoryType.TriggerManagement)]
    [XmlRoot(ElementName = "ActionFolderOperation")]
    public class ActionFolderOperation : ActionBase
    {

        #region Properties

        /// <summary>
        /// Folder operations
        /// </summary>
        public enum OperationEnum
        {
            Enable,
            Disable,
            CancelTriggers,
            CancelTriggersByNameRegex,
        }

        /// <summary>
        /// Type of the folder operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation",
            ChoiceLabels = new[]
        {
            "Enable the specified folder",
            "Disable the specified folder",
            "Cancel all actions in the specified folder",
            "Cancel all actions in folders matching a name regex",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.Enable;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Enable);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Reference to the folder
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ReferenceEditor(2, EditorReferenceType.Folder, Label = "Folder")]
        public Guid FolderId { get; set; } = Guid.Empty;

        [XmlAttribute("FolderId")]
        public string Xml_FolderId
        {
            get => XmlAttr.Guid(FolderId, Guid.Empty);
            set => FolderId = XmlAttr.Guid(value);
        }

        /// <summary>用于匹配分组名称的正则表达式；执行前展开文本表达式。</summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(3, EditorExpressionType.Regex, Label = "Name regex")]
        public string NameRegex { get; set; } = "";

        [XmlAttribute("NameRegex")]
        public string Xml_NameRegex
        {
            get => XmlAttr.String(NameRegex, "", omitWhitespace: false);
            set => NameRegex = value;
        }

        #endregion

        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var folder = editor.GetEditor(nameof(FolderId));
            var nameRegex = editor.GetEditor(nameof(NameRegex));
            return () =>
            {
                bool isRegexOperation = (OperationEnum)operation.GetValue() == OperationEnum.CancelTriggersByNameRegex;
                editor.SetEnabled(!isRegexOperation, folder);
                editor.SetEnabled(isRegexOperation, nameRegex);
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            if (Operation == OperationEnum.CancelTriggersByNameRegex)
                return I18n.Translate("internal/Action/desccancelfoldersregex",
                    "cancel all actions from folders with names matching ({0})", NameRegex);
            Folder f = RealPlugin.Instance.GetFolderById(FolderId, ParentTrigger?.Repo);
            if (f != null)
            {
                switch (Operation)
                {
                    case OperationEnum.Disable:
                        return I18n.Translate("internal/Action/descdisablefolder", "disable folder ({0})", f.Name);
                    case OperationEnum.Enable:
                        return I18n.Translate("internal/Action/descenablefolder", "enable folder ({0})", f.Name);
                    case OperationEnum.CancelTriggers:
                        return I18n.Translate("internal/Action/desccancelfolder", "cancel all actions from folder ({0})", f.Name);
                    default:
                        return NotImplementedEnumMessage(Operation);
                }
            }
            return I18n.Translate("internal/Action/descinvalidfolderref", "folder action with an invalid folder reference ({0})", FolderId);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            if (Operation == OperationEnum.CancelTriggersByNameRegex)
            {
                var pattern = EvalString(NameRegex, ctx);
                var regex = new Regex(pattern);
                var triggersInFolders = new HashSet<Trigger>();
                void CollectTriggers(Folder folder)
                {
                    if (regex.IsMatch(folder.Name ?? ""))
                        triggersInFolders.UnionWith(folder.RecursiveGetTriggers());
                    else
                        foreach (Folder child in folder.Folders)
                            CollectTriggers(child);
                }
                CollectTriggers(ctx.Trigger?.Repo?.Root ?? plug.Config.Root);
                int removed = plug.CancelQueuedActions(
                    action => action.Context?.Trigger != null && triggersInFolders.Contains(action.Context.Trigger));
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/cancelfoldersregex",
                    "Cancelled {1} queued action(s) from {2} triggers in folders with names matching ({0})",
                    pattern, removed, triggersInFolders.Count));
                return;
            }

            Folder f = plug.GetFolderById(FolderId, ctx.Trigger?.Repo);
            if (f == null)
            {
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/nofolderwithid",
                    "Folder operation failed: In trigger ({1}), the specified folder id ({0}) does not exist.", FolderId, ParentTrigger?.FullPath ?? "null"));
                return;
            }
            switch (Operation)
            {
                case OperationEnum.Disable:
                case OperationEnum.Enable:
                    {
                        bool isEnable = Operation == OperationEnum.Enable;
                        f.Enabled = isEnable;
                        plug.ui.Invoke((System.Action)(() =>
                        {
                            bool isLocal = ctx.Trigger?.Repo == null;
                            TreeNode tn = plug.LocateNodeHostingFolder(plug.ui.treeView1.Nodes[isLocal ? 0 : 1], f);
                            if (tn != null) tn.Checked = isEnable;
                            else AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodefolderwithid", "Didn't find a tree node for folder ({0}) with id ({1})", f.Name, f.Id));
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, isEnable
                                ? I18n.Translate("internal/Action/enabledfolderwithid", "Enabled folder ({0}) with id ({1})", f.Name, f.Id)
                                : I18n.Translate("internal/Action/disabledfolderwithid", "Disabled folder ({0}) with id ({1})", f.Name, f.Id));
                        }));
                    }
                    break;
                case OperationEnum.CancelTriggers:
                    {
                        var triggersInFolder = new HashSet<Trigger>(f.RecursiveGetTriggers());
                        int removed = plug.CancelQueuedActions(
                            _qa => _qa?.Context?.Trigger != null && triggersInFolder.Contains(_qa.Context.Trigger)
                        );
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/cancelfolder",
                            "Cancelled {1} queued action(s) from {2} triggers in folder ({0})",
                            f.Name, removed, triggersInFolder.Count));
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionFolderOperation(ActionOld oldAction)
        {
            var action = new ActionFolderOperation();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._FolderOp;
            action.FolderId = oldAction._FolderId;
            return action;
        }

        #endregion Old Action Converter

    }

}
