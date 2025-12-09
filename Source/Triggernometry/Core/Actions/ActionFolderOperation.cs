using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Folder operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "FolderOperation")]
    internal class ActionFolderOperation : ActionBase
    {

        #region Properties

        /// <summary>
        /// Folder operations
        /// </summary>
        private enum OperationEnum
        {
            Enable,
            Disable,
            CancelTriggers,
        }

        /// <summary>
        /// Type of the folder operation
        /// </summary>
        [Action(ordernum: 1)]
        private OperationEnum _Operation { get; set; } = OperationEnum.Enable;
        [XmlAttribute]
        public string Operation
        {
            get
            {
                if (_Operation != OperationEnum.Enable)
                {
                    return _Operation.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _Operation = (OperationEnum)Enum.Parse(typeof(OperationEnum), value);
            }
        }

        /// <summary>
        /// Reference to the folder
        /// </summary>
        [Action(ordernum: 2, specialtype: ActionAttribute.SpecialTypeEnum.FolderReference)]
        private Guid _FolderId { get; set; } = Guid.Empty;
        [XmlAttribute]
        public string FolderId
        {
            get
            {
                if (_FolderId != Guid.Empty)
                {
                    return _FolderId.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _FolderId = Guid.Parse(value);
            }
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            Folder f = ctx.Plugin.GetFolderById(_FolderId, ctx.Trigger?.Repo);
            if (f != null)
            {
                switch (_Operation)
                {
                    case OperationEnum.Disable:
                        return I18n.Translate("internal/Action/descdisablefolder", "disable folder ({0})", f.Name);
                    case OperationEnum.Enable:
                        return I18n.Translate("internal/Action/descenablefolder", "enable folder ({0})", f.Name);
                    case OperationEnum.CancelTriggers:
                        return I18n.Translate("internal/Action/desccancelfolder", "cancel all actions from folder ({0})", f.Name);
                }
                return "";
            }
            return I18n.Translate("internal/Action/descinvalidfolderref", "folder action with an invalid folder reference ({0})", _FolderId);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            Folder f = ctx.Plugin.GetFolderById(_FolderId, ctx.Trigger?.Repo);
            if (f != null)
            {
                switch (_Operation)
                {
                    case OperationEnum.Disable:
                        {
                            f.Enabled = false;

                            ctx.Plugin.ui.Invoke((System.Action)(() =>
                            {
                                bool isLocal = ctx.Trigger?.Repo == null;
                                TreeNode tn = ctx.Plugin.LocateNodeHostingFolder(ctx.Plugin.ui.treeView1.Nodes[isLocal ? 0 : 1], f);

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

                            ctx.Plugin.ui.Invoke((System.Action)(() =>
                            {
                                bool isLocal = ctx.Trigger?.Repo == null;
                                TreeNode tn = ctx.Plugin.LocateNodeHostingFolder(ctx.Plugin.ui.treeView1.Nodes[isLocal ? 0 : 1], f);

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
                }
            }
            else
            {
                AddToLog(ctx, RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Action/nofolderwithid",
                    "Folder operation failed: In trigger ({1}), the specified folder id ({0}) does not exist.", _FolderId, ParentTrigger?.FullPath ?? "null"));
            }
        }

        #endregion

    }

}
