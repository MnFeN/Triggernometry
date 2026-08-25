using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Folder operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "FolderOperation")]
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
        }

        /// <summary>
        /// Type of the folder operation
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
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
        [Action(order: 2, specialtype: ActionAttribute.SpecialTypeEnum.FolderReference)]
        public Guid FolderId { get; set; } = Guid.Empty;

        [XmlAttribute("FolderId")]
        public string Xml_FolderId
        {
            get => XmlAttr.Guid(FolderId, Guid.Empty);
            set => FolderId = XmlAttr.Guid(value);
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
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

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

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
                    {
                        f.Enabled = false;

                        plug.ui.Invoke((System.Action)(() =>
                        {
                            bool isLocal = ctx.Trigger?.Repo == null;
                            TreeNode tn = plug.LocateNodeHostingFolder(plug.ui.treeView1.Nodes[isLocal ? 0 : 1], f);

                            if (tn != null)
                            {
                                tn.Checked = false;
                            }
                            else
                            {
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodefolderwithid", "Didn't find a tree node for folder ({0}) with id ({1})", f.Name, f.Id));
                            }
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/disabledfolderwithid", "Disabled folder ({0}) with id ({1})", f.Name, f.Id));
                        }));
                    }
                    break;
                case OperationEnum.Enable:
                    {
                        f.Enabled = true;

                        plug.ui.Invoke((System.Action)(() =>
                        {
                            bool isLocal = ctx.Trigger?.Repo == null;
                            TreeNode tn = plug.LocateNodeHostingFolder(plug.ui.treeView1.Nodes[isLocal ? 0 : 1], f);

                            if (tn != null)
                            {
                                tn.Checked = true;
                            }
                            else
                            {
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodefolderwithid", "Didn't find a tree node for folder ({0}) with id ({1})", f.Name, f.Id));
                            }
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/enabledfolderwithid", "Enabled folder ({0}) with id ({1})", f.Name, f.Id));
                        }));
                    }
                    break;
                case OperationEnum.CancelTriggers:
                    {
                        var triggersInFolder = new HashSet<Trigger>(f.RecursiveGetTriggers());
                        int removed = RealPlugin.Instance.CancelQueuedActions(
                            _qa => _qa?.ctx?.Trigger != null && triggersInFolder.Contains(_qa.ctx.Trigger)
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

        // (ActionOld)this
        public static explicit operator ActionOld(ActionFolderOperation action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Folder;
            oldAction._FolderOp = (ActionOld.FolderOpEnum)(int)action.Operation;
            oldAction._FolderId = action.FolderId;
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
