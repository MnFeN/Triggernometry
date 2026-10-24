using System.Collections.Generic;
using System.Linq;
using Triggernometry.Core.Variables;

namespace Triggernometry.Core
{
    public partial class RealPlugin
    {
        /// <summary>
        /// Registers global definitions once when a folder subtree is added, regardless of its enabled state.
        /// </summary>
        internal void RegisterFolderVariables(Folder folder)
        {
            foreach (RegisteredVariable variable in folder.RegisteredVariables.Where(value => !value.LoadWhenActive))
            {
                RegisterVariable(folder.LogName, variable);
            }
            foreach (Folder child in folder.Folders)
            {
                RegisterFolderVariables(child);
            }
        }

        /// <summary>
        /// Removes the keys currently declared by a folder subtree when the subtree is removed.
        /// </summary>
        internal void UnregisterFolderVariables(Folder folder)
        {
            lock (RegisteredVariables)
            {
                foreach (RegisteredVariable variable in folder.RegisteredVariables.Where(value => value.Enabled && !value.IsComment))
                {
                    RegisteredVariables.Remove(variable.Name);
                }
                folder.isRegisteredVariableScopeActive = false;
                foreach (Folder child in folder.Folders)
                {
                    UnregisterFolderVariables(child);
                }
            }
        }

        /// <summary>
        /// Collects effective activation changes for a folder and its descendants.
        /// </summary>
        private void CollectFolderVariableChanges(Folder folder, bool isActive, string zone, List<Folder> disabled, List<Folder> enabled)
        {
            isActive = isActive && folder.Enabled && folder.PassesZoneRestriction(zone);
            if (folder.isRegisteredVariableScopeActive != isActive)
            {
                (isActive ? enabled : disabled).Add(folder);
                folder.isRegisteredVariableScopeActive = isActive;
            }
            foreach (Folder child in folder.Folders)
            {
                CollectFolderVariableChanges(child, isActive, zone, disabled, enabled);
            }
        }

        /// <summary>
        /// Deletes all deactivated scoped keys before loading any newly active folder's scoped definitions.
        /// </summary>
        internal void UpdateActiveFolderVariables(string zone)
        {
            lock (RegisteredVariables)
            {
                var disabled = new List<Folder>();
                var enabled = new List<Folder>();
                CollectFolderVariableChanges(cfg.Root, true, zone, disabled, enabled);
                foreach (Repository repo in cfg.RepositoryRoot.Repositories)
                {
                    CollectFolderVariableChanges(repo.Root, cfg.RepositoryRoot.Enabled && repo.Enabled, zone, disabled, enabled);
                }
                // Finish removals first so a departing folder cannot delete a newly loaded value.
                foreach (Folder folder in disabled)
                {
                    foreach (RegisteredVariable variable in folder.RegisteredVariables.Where(value => value.Enabled && !value.IsComment && value.LoadWhenActive))
                    {
                        RegisteredVariables.Remove(variable.Name);
                    }
                }
                foreach (Folder folder in enabled)
                {
                    foreach (RegisteredVariable variable in folder.RegisteredVariables.Where(value => value.LoadWhenActive))
                    {
                        RegisterVariable(folder.LogName, variable);
                    }
                }
            }
        }
    }
}
