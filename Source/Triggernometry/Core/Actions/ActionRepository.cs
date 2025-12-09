using System;
using System.Windows.Forms;
using System.Xml.Serialization;
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
        private enum OperationEnum
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
        [Action(ordernum: 1)]
        private OperationEnum _Operation { get; set; } = OperationEnum.UpdateSelf;
        [XmlAttribute]
        public string Operation
        {
            get
            {
                if (_Operation != OperationEnum.UpdateSelf)
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
        /// Reference to remote respository
        /// </summary>
        [Action(ordernum: 2, specialtype: ActionAttribute.SpecialTypeEnum.RepoReference)]
        private Guid _RepositoryId { get; set; } = Guid.Empty;
        [XmlAttribute]
        public string RepositoryId
        {
            get
            {
                if (_RepositoryId != Guid.Empty)
                {
                    return _RepositoryId.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _RepositoryId = Guid.Parse(value);
            }
        }

        #endregion

        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            switch (_Operation)
            {
                case OperationEnum.UpdateSelf:
                    return I18n.Translate("internal/Action/repoupdateself", "Update containing repository");                    
                case OperationEnum.UpdateRepo:
                    Repository r = ctx.Plugin.GetRepositoryById(_RepositoryId);
                    if (r != null)
                    {
                        return I18n.Translate("internal/Action/repoupdatespecific", "Update repository ({0})", r.Name);
                    }
                    return I18n.Translate("internal/Action/descrepoinvalidref", "repository action with an invalid repository reference ({0})", _RepositoryId);
                case OperationEnum.UpdateAll:
                    return I18n.Translate("internal/Action/repoupdateall", "Update all repositories");                    
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            Repository r = null;
            switch (_Operation)
            {
                case OperationEnum.UpdateSelf:
                    r = ctx.Trigger?.Repo;
                    break;
                case OperationEnum.UpdateRepo:
                    r = ctx.Plugin.GetRepositoryById(_RepositoryId);
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
