using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Remote repository operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "Repository")]
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
        [Action(order: 1)]
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
        [Action(order: 2, specialtype: ActionAttribute.SpecialTypeEnum.RepoReference)]
        public Guid RepositoryId { get; set; } = Guid.Empty;

        [XmlAttribute("RepositoryId")]
        public string Xml_RepositoryId
        {
            get => XmlAttr.Guid(RepositoryId, Guid.Empty);
            set => RepositoryId = XmlAttr.Guid(value);
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            switch (Operation)
            {
                case OperationEnum.UpdateSelf:
                    return I18n.Translate("internal/Action/repoupdateself", "Update containing repository");                    
                case OperationEnum.UpdateRepo:
                    Repository r = ctx.Plugin.GetRepositoryById(RepositoryId);
                    if (r != null)
                    {
                        return I18n.Translate("internal/Action/repoupdatespecific", "Update repository ({0})", r.Name);
                    }
                    return I18n.Translate("internal/Action/descrepoinvalidref", "repository action with an invalid repository reference ({0})", RepositoryId);
                case OperationEnum.UpdateAll:
                    return I18n.Translate("internal/Action/repoupdateall", "Update all repositories");                    
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            Repository r = null;
            switch (Operation)
            {
                case OperationEnum.UpdateSelf:
                    r = ctx.Trigger?.Repo;
                    break;
                case OperationEnum.UpdateRepo:
                    r = ctx.Plugin.GetRepositoryById(RepositoryId);
                    break;
                case OperationEnum.UpdateAll:
                    _ = ctx.Plugin.UpdateAllRepositoriesAsync(false);
                    break;
            }
            if (r != null)
            {
                _ = ctx.Plugin.UpdateSingleRepositoryAsync(r);
            }
        }

        #endregion

    }

}
