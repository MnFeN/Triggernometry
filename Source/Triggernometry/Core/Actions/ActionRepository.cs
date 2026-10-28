using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Remote repository operations
    /// </summary>
    [Action("Repository operation", ActionAttribute.CategoryType.TriggerManagement)]
    [XmlRoot(ElementName = "ActionRepository")]
    public class ActionRepository : ActionBase
    {

        #region Properties

        /// <summary>
        /// Repository operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Update remote repository containing trigger
            /// </summary>
            UpdateSelf,
            /// <summary>
            /// Update specified remote repository
            /// </summary>
            UpdateRepo,
            /// <summary>
            /// Update all remote repositories
            /// </summary>
            UpdateAll
        }

        /// <summary>
        /// Type of the repository operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation",
            ChoiceLabels = new[]
        {
            "Update containing repository",
            "Update selected repository",
            "Update all enabled repositories",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.UpdateSelf;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.UpdateSelf);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Reference to remote respository
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ReferenceEditor(2, EditorReferenceType.Repository, Label = "Repository")]
        public Guid RepositoryId { get; set; } = Guid.Empty;

        [XmlAttribute("RepositoryId")]
        public string Xml_RepositoryId
        {
            get => XmlAttr.Guid(RepositoryId, Guid.Empty);
            set => RepositoryId = XmlAttr.Guid(value);
        }

        #endregion

        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var repository = editor.GetEditor(nameof(RepositoryId));
            return () => editor.SetEnabled((OperationEnum)operation.GetValue() == OperationEnum.UpdateRepo, repository);
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.UpdateSelf:
                    return I18n.Translate("internal/Action/repoupdateself", "Update containing repository");                    
                case OperationEnum.UpdateRepo:
                    Repository r = RealPlugin.Instance.GetRepositoryById(RepositoryId);
                    if (r != null)
                    {
                        return I18n.Translate("internal/Action/repoupdatespecific", "Update repository ({0})", r.Name);
                    }
                    return I18n.Translate("internal/Action/descrepoinvalidref", "repository action with an invalid repository reference ({0})", RepositoryId);
                case OperationEnum.UpdateAll:
                    return I18n.Translate("internal/Action/repoupdateall", "Update all repositories");
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            Repository r = null;
            switch (Operation)
            {
                case OperationEnum.UpdateSelf:
                    r = ctx.Trigger?.Repo;
                    break;
                case OperationEnum.UpdateRepo:
                    r = plug.GetRepositoryById(RepositoryId);
                    break;
                case OperationEnum.UpdateAll:
                    _ = plug.UpdateAllRepositoriesAsync(false);
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
            if (r != null)
            {
                _ = plug.UpdateSingleRepositoryAsync(r);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionRepository(ActionOld oldAction)
        {
            var action = new ActionRepository();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._RepositoryOp;
            action.RepositoryId = oldAction._RepositoryId;
            return action;
        }

        #endregion Old Action Converter
    }

}
