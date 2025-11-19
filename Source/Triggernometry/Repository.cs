using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Triggernometry.Utilities;
using static Triggernometry.RealPlugin;

namespace Triggernometry
{

    public class Repository
    {

        public class RepositoryItem
        {

            [XmlAttribute]
            public Guid Id;

            [XmlAttribute]
            public bool Enabled;

        }

        public enum NewBehaviorEnum
        {
            AsDefined,
            AlwaysEnable,
            AlwaysDisable
        }

        public enum UpdatePolicyEnum
        {
            Startup,
            Manual
        }

        public enum AudioOutputEnum
        {
            AsDefined,
            AlwaysOverride,
            NeverOverride
        }

        [XmlAttribute]
        public bool Enabled
        {
            get
            {
                return Root.Enabled;
            }
            set
            {
                Root.Enabled = value;
            }
        }

        [XmlAttribute]
        public string Name
        {
            get
            {
                return Root.Name;
            }
            set
            {
                Root.Name = value;
            }
        }

        [XmlAttribute]
        public Guid Id { get; set; } = Guid.NewGuid();

        internal RepositoryFolder Parent { get; set; }

        internal List<Trigger> ReadmeTriggers = new List<Trigger>();

        [XmlAttribute]
        public string Address { get; set; }

        /// <summary>Whether to save a local backup of the repository XML.</summary>
        [XmlAttribute]
        public bool KeepLocalBackup { get; set; } = true;

        /// <summary>
        /// Currently loaded repository content length (raw bytes). <br />
        /// </summary>
        internal long CurrentContentLength { get; set; } = 0;

        /// <summary>
        /// The last known modification time of the downloaded remote repository backup. <br />
        /// Note: this is not related with the backup file, but the remote metadata.
        /// </summary>
        [XmlAttribute]
        public DateTime LocalLastModified { get; set; } = DateTime.MinValue;

        /// <summary>The time of the most recent update check, no matter updated or not.</summary>
        [XmlAttribute]
        public DateTime UpdateLastChecked { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Enables periodic background checks for updates on this remote repository. <br />
        /// When enabled, a timer will regularly scan for updates.
        /// </summary>
        [XmlAttribute]
        public bool AutoUpdate { get; set; } = false;

        /// <summary>
        /// The interval, in minutes, at which automatic update checks are performed. <br />
        /// This value is used by the update timer when <see cref="AutoUpdate"/> is enabled.
        /// </summary>
        [XmlAttribute]
        public int UpdateInterval { get; set; } = 5;

        [XmlAttribute]
        public bool AllowScriptExecution { get; set; } = false;
        [XmlAttribute]
        public bool AllowProcessLaunch { get; set; } = false;
        [XmlAttribute]
        public bool AllowWindowMessages { get; set; } = false;
        [XmlAttribute]
        public bool AllowObsControl { get; set; } = false;
        [XmlAttribute]
        public bool AllowDiskOperations { get; set; } = false;

        /// <summary>Determines how newly imported folders or triggers should be enabled.</summary>
        [XmlAttribute]
        public NewBehaviorEnum NewBehavior { get; set; }

        /// <summary>Determines when remote update checks should be performed.</summary>
        [XmlAttribute]
        public UpdatePolicyEnum UpdatePolicy { get; set; }

        /// <summary>Determines whether repository settings override the audio output method.</summary>
        [XmlAttribute]
        public AudioOutputEnum AudioOutput { get; set; }

        /// <summary>
        /// Stores the states of all folders in the repository. <br />
        /// Currently only for enabled/disabled. <br />
        /// </summary>
        public List<RepositoryItem> FolderStates { get; set; } = new List<RepositoryItem>();

        /// <summary>
        /// Stores the states of all triggers in the repository. 
        /// Currently only for enabled/disabled.
        /// </summary>
        public List<RepositoryItem> TriggerStates { get; set; } = new List<RepositoryItem>();

        internal Folder Root { get; set; } = new Folder();

        private readonly object _logLock = new object();
        private List<string> UpdateLog { get; set; } = new List<string>();
        public string[] UpdateLogSnapshot()
        {
            lock (_logLock)
            {
                return UpdateLog.ToArray();
            }
        }

        public Repository()
        {
        }

        /// <summary>
        /// Add a message to the repository's log. <br />
        /// If a debug level is provided, also forward the message to <see cref="RealPlugin.FilteredAddToLog" />.
        /// </summary>
        public void AddToLog(DebugLevelEnum? level, string log)
        {
            string line = $"[{FormatDateTime(DateTime.Now)}] {log}";
            lock (_logLock)
            {
                UpdateLog.Add(line);
            }
            if (level.HasValue)
            { 
                plug.FilteredAddToLog(level.Value, log);
            }
        }

        /// <summary> Clear all log line of the repository. </summary>
        public void ClearLog()
        {
            lock (_logLock)
            {
                UpdateLog.Clear();
            }
        }

        /// <summary>
        /// Completely remove all existing repository content: <br/>
        /// - Unregister triggers from the plugin. <br />
        /// - Clear all folders and triggers <br />
        /// - Clear readme triggers <br />
        /// - Clear UI tree node <br />
        /// Ensures the repository starts with a clean state for the next import.
        /// </summary>
        public void ClearContent()
        {
            plug.RemoveTriggersFromFolder(Root);
            Root.Folders.Clear();
            Root.Triggers.Clear();
            ReadmeTriggers.Clear();
            plug.ui.ClearRepositoryInTree(this);
        }

        #region Restrictions

        [Flags]
        public enum RestrictionEnum
        {
            None = 0,
            LaunchProcess = 1 << 0,
            ExecuteScript = 1 << 1,
            WindowMessage = 1 << 2,
            ObsControl    = 1 << 3,
            DiskOperation = 1 << 4,
        }

        /// <summary>
        /// Returns all <see cref="RestrictionEnum" /> flags violated by the actions and stores them in <see cref="Trigger.RepoRestrictions" />.
        /// </summary>
        internal RestrictionEnum GetAndSetTriggerRestrictions(Trigger t)
        {
            var restrictions = t.Actions?.Select(GetActionRestrictions).Aggregate(RestrictionEnum.None, (acc, cur) => acc | cur) ?? RestrictionEnum.None;
            t.RepoRestrictions = restrictions;
            return restrictions;
        }

        /// <summary>
        /// Returns the <see cref="RestrictionEnum" /> flags violated by this action.
        /// </summary>
        private RestrictionEnum GetActionRestrictions(Action a)
        {
            if (!a._Enabled)
                return RestrictionEnum.None;
            switch (a._ActionType)
            {
                case Action.ActionTypeEnum.ExecuteScript:
                    return AllowScriptExecution ? RestrictionEnum.None : RestrictionEnum.ExecuteScript;
                case Action.ActionTypeEnum.LaunchProcess:
                    return AllowProcessLaunch ? RestrictionEnum.None : RestrictionEnum.LaunchProcess;
                case Action.ActionTypeEnum.WindowMessage:
                    return AllowWindowMessages ? RestrictionEnum.None : RestrictionEnum.WindowMessage;
                case Action.ActionTypeEnum.DiskFile:
                    return AllowDiskOperations ? RestrictionEnum.None : RestrictionEnum.DiskOperation;
                case Action.ActionTypeEnum.ObsControl:
                    return AllowObsControl ? RestrictionEnum.None : RestrictionEnum.ObsControl;
                case Action.ActionTypeEnum.Loop:
                    return a.LoopActions?.Select(GetActionRestrictions).Aggregate(RestrictionEnum.None, (acc, cur) => acc | cur) ?? RestrictionEnum.None;
            }
            return RestrictionEnum.None;
        }

        /// <summary>
        /// Applies repository-level settings to the specified folder: <br />
        /// 1) Retrieve previously saved Enabled state (if any). <br />
        /// 2) Recursively apply settings to subfolders and triggers. <br />
        /// </summary>
        /// <returns>
        /// A <see cref="RestrictionEnum" /> flag set describing which action types are forbidden. <br />
        /// Returns <see cref="RestrictionEnum.None" /> if fully allowed.
        /// </returns>
        internal RestrictionEnum ApplySettingsOnFolder(Folder f)
        {
            f.Repo = this;

            // Retrieve previously saved Enabled state (if any)
            var savedFolder = FolderStates.FirstOrDefault(saved => saved.Id == f.Id);
            if (savedFolder == null) // new
            {
                switch (NewBehavior)
                {
                    case NewBehaviorEnum.AlwaysEnable:
                        f.Enabled = true;
                        break;
                    case NewBehaviorEnum.AlwaysDisable:
                        f.Enabled = false;
                        break;
                }
                FolderStates.Add(new RepositoryItem() { Id = f.Id, Enabled = f.Enabled });
            }
            else // saved
            {
                f.Enabled = savedFolder.Enabled;
            }

            // Recursively apply settings to subfolders and triggers, and collect restrictions
            var restrictions = RestrictionEnum.None;
            foreach (var subFolder in f.Folders)
            {
                restrictions |= ApplySettingsOnFolder(subFolder);
            }
            foreach (var trigger in f.Triggers)
            {
                restrictions |= ApplySettingsOnTrigger(trigger);
            }
            return restrictions;
        }

        /// <summary>
        /// Applies repository-level settings to the specified trigger: <br />
        /// 1) Restoring or initializing the trigger's Enabled state using saved TriggerStates. <br />
        /// 2) Applying repository-level audio routing policies. <br />
        /// 3) Collect and store any violated restriction flags. <br />
        /// </summary>
        /// <returns>
        /// A <see cref="RestrictionEnum" /> flag set describing which action types are forbidden. <br />
        /// Returns <see cref="RestrictionEnum.None" /> if no violated restrictions were found.
        /// </returns>
        internal RestrictionEnum ApplySettingsOnTrigger(Trigger t)
        {
            t.Repo = this;

            // Retrieve previously saved Enabled state (if any)
            var savedTrigger = TriggerStates.FirstOrDefault(saved => saved.Id == t.Id);
            if (savedTrigger == null) // new
            {
                switch (NewBehavior)
                {
                    case NewBehaviorEnum.AlwaysEnable:
                        t.Enabled = true;
                        break;
                    case NewBehaviorEnum.AlwaysDisable:
                        t.Enabled = false;
                        break;
                }
                TriggerStates.Add(new RepositoryItem() { Id = t.Id, Enabled = t.Enabled });
            }
            else // saved
            {
                t.Enabled = savedTrigger.Enabled;
            }

            // Apply audio routing policy
            switch (AudioOutput)
            {
                case AudioOutputEnum.AlwaysOverride:
                    {
                        foreach (Action a in t.Actions)
                        {
                            a._SoundRouting = Configuration.AudioRoutingMethodEnum.Triggernometry;
                            a._TTSRouting = Configuration.AudioRoutingMethodEnum.Triggernometry;
                        }
                    }
                    break;
                case AudioOutputEnum.NeverOverride:
                    {
                        // ?
                    }
                    break;
            }

            // Collect and store any violated restriction flags
            return GetAndSetTriggerRestrictions(t);
        }

        #endregion Restrictions

        #region Update and Import

        /// <summary>
        /// Imports exported folders and triggers from <see cref="TriggernometryExport"> into this repository and applies all repo settings.
        /// </summary>
        internal void AddContentFromExport(TriggernometryExport exp)
        {
            ClearContent();

            var restrictions = RestrictionEnum.None;

            if (exp.ExportedFolder != null)
            {
                restrictions |= ApplySettingsOnFolder(exp.ExportedFolder);
                Root.Folders.Add(exp.ExportedFolder);
                RegisterFolder(exp.ExportedFolder, this.Enabled && plug.ui.treeView1.Nodes[1].Checked);
            }

            if (exp.ExportedTrigger != null)
            {
                restrictions |= ApplySettingsOnTrigger(exp.ExportedTrigger);
                Root.Triggers.Add(exp.ExportedTrigger);
                exp.ExportedTrigger.Parent = Root;

                RegisterTrigger(exp.ExportedTrigger, Enabled);
            }

            if (restrictions != RestrictionEnum.None)
            {
                var restrictionNames = Enum.GetValues(typeof(RestrictionEnum))
                    .Cast<RestrictionEnum>()
                    .Where(flag => flag != RestrictionEnum.None && restrictions.HasFlag(flag))
                    .Select(flag => I18n.Translate($"internal/Repository/restrictionenum/{flag}", flag.ToString()))
                    .ToList();
                AddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Repository/restricted",
                    "Remote repository {0} has the following permissions disabled: [{1}]. Some triggers could not work correctly.",
                    Name, string.Join(", ", restrictionNames)));
            }

            plug.ui.BuildTreeForRepository(exp, this);
        }

        internal void RegisterFolder(Folder f, bool parentEnabled)
        {
            if (f.Enabled == false)
            {
                parentEnabled = false;
            }

            foreach (Folder subFolder in f.Folders)
            {
                subFolder.Parent = f;
                RegisterFolder(subFolder, parentEnabled);
            }

            foreach (Trigger t in f.Triggers)
            {
                t.Parent = f;
                RegisterTrigger(t, parentEnabled);
            }
        }

        internal void RegisterTrigger(Trigger t, bool parentEnabled)
        {
            // Always register the trigger, but enable/disable depends on parentEnabled
            plug.AddTrigger(t, parentEnabled);

            if (t.IsReadme == true && t.Enabled == true)
            {
                ReadmeTriggers.Add(t);
            }
        }

        /// <summary>
        /// Downloads the repository XML, loads it and imports content. <br />
        /// Does NOT perform any validation, metadata checks, cache checks or decision logic. <br />
        /// </summary>
        private async Task<bool> UpdateAsync()
        {
            try
            {
                // Download
                AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/updating",
                    "Downloading repository {0} from {1}", Name, Address));

                // use bytes to keep Content-Length, instead of read it as a string directly
                byte[] raw = await HttpHelper.GetBytesAsync(Address);

                // Encode and Deserialize
                string data = Encoding.UTF8.GetString(raw);
                TriggernometryExport exp = TriggernometryExport.Unserialize(data);
                if (exp.Corrupted)
                {
                    AddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Repository/updatecorrupted",
                        "Data for repository {0} at {1} could not be unserialized. Make sure you are running the latest version of Triggernometry, and the repository is up-to-date.", 
                        Name, Address));
                    return false;
                }

                // Apply content
                CurrentContentLength = raw.Length;
                AddContentFromExport(exp);

                // Save backup
                if (KeepLocalBackup == true)
                {
                    SaveLocalBackup(raw);
                }

                return true;
            }
            catch (Exception ex)
            {
                AddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Repository/updateex",
                    "Couldn't update repository {0} due to exception: {1}", Name, ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Performs a safe repository update process. <br />
        /// Decides whether the remote version should be fetched or the local backup should be used,  <br />
        /// and falls back gracefully if remote update fails. <br /><br />
        /// Should NOT throw any exceptions.
        /// </summary>
        public async Task CheckAndUpdateAsync(bool isStartup = false)
        {
            // this function should not throw any uncaught exceptions

            UpdateLastChecked = DateTime.Now;
            ClearContent();
            AddToLog(null, new string('=', 50)); // separator before each update attempt

            // Whether we should check the remote metadata at all (depends on startup mode and update policy).
            bool shouldCheckUpdate = !isStartup || UpdatePolicy == Repository.UpdatePolicyEnum.Startup;

            // Whether the remote repo should be downloaded (based on metadata comparison and backup validity).
            bool shouldUpdate = false;

            // Whether the local backup has already been tried to load, to avoid loading it twice.
            bool triedLoadBackup = false;

            // check if update is needed (by metadata)
            long remoteLength;
            DateTime remoteLastModified = default;
            if (shouldCheckUpdate)
            {
                var metadata = await HttpHelper.GetMetadataAsync(Address);
                remoteLength = metadata.contentLength ?? -1;
                remoteLastModified = metadata.lastModified ?? LocalLastModified; // GitHub does not provide last-modified, bypass this check
                shouldUpdate = ShouldUpdate(remoteLength, remoteLastModified); 
            }

            // if an update is not needed, try to load local backup
            if (!shouldUpdate && KeepLocalBackup)
            {
                if (TryLoadLocalBackup() == false)
                {
                    // but if the local backup fails to load, still need to update
                    shouldUpdate = true;
                }
                triedLoadBackup = true;
            }

            // update from remote
            if (shouldUpdate)
            {
                bool updated = await UpdateAsync();

                if (updated)
                { 
                    LocalLastModified = remoteLastModified;
                }

                // if update failed, try to load local backup
                if (!updated && KeepLocalBackup && !triedLoadBackup)
                {
                    _ = TryLoadLocalBackup();
                }
            }
        }

        /// <summary>
        /// Checks whether the remote repository needs to be updated: <br />
        /// 1) Compare with local backup file timestamp + size <br />
        /// 2) Consider cache expiry <br />
        /// </summary>
        /// <returns>
        /// true  = Should update (remote changed / cache expired / missing backup) <br />
        /// false = No update needed (remote matches local backup)
        /// </returns>
        private bool ShouldUpdate(long remoteLength, DateTime remoteLastModified)
        {
            // --- Step 1: check remote metadata ---
            AddToLog(null, I18n.Translate("internal/Repository/probeinforemote",
                "[Info] Remote metadata: Length={0}, Time={1}", remoteLength, remoteLastModified));
            if (remoteLength < 0)
            {
                AddToLog(null, I18n.Translate("internal/Repository/probeinforemotefail",
                    "[Info] Remote metadata size not available, forcing update."));
                return true;
            }

            // --- Step 2: load local backup info ---

            CurrentContentLength = GetBackupFileLength();
            if (CurrentContentLength < 0)
            {
                AddToLog(null, I18n.Translate("internal/Repository/probeinfolocalmissing",
                    "[Info] Local backup does not exist, update required."));
                return true;
            }
            AddToLog(null, I18n.Translate("internal/Repository/probeinfolocal", 
                "[Info] Local backup: Size={0}, Time={1}", CurrentContentLength, LocalLastModified));

            // --- Step 3: cache expiry check ---
            DateTime cacheLimit;
            try
            {
                cacheLimit = LocalLastModified.AddMinutes(plug.cfg.CacheRepoExpiry);
            }
            catch
            {
                cacheLimit = DateTime.MinValue;
            }
            bool cacheExpired = cacheLimit < DateTime.Now;
            AddToLog(null, I18n.Translate("internal/Repository/probeinfocache", "[Info] Cache expired: {0}", cacheExpired));

            // --- Step 4: compare remote & local ---
            bool shouldUpdate = remoteLength != CurrentContentLength || remoteLastModified != LocalLastModified || cacheExpired;

            if (shouldUpdate)
                AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/proberesulttrue",
                    "Repository {0} has changed, fetching new version. Size: {1} → {2}, Time: ({3}) → ({4})",
                    Name, CurrentContentLength, remoteLength, LocalLastModified, remoteLastModified));
            else
                AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/proberesultfalse",
                    "Repository {0} hasn't changed, update is not needed. Size: {1}, Time: ({2})",
                    Name, CurrentContentLength, LocalLastModified));

            return shouldUpdate;
        }

        #endregion Update and Import


        #region Backup

        /// <summary>Get the folder path where repository backups are stored.</summary>
        internal static string BackupFolderPath => Path.Combine(plug.ConfigPath, "TriggernometryRepoBackups");

        /// <summary>Get the full path of the backup file for this repository.</summary>
        internal string GetBackupFileName()
            => Path.Combine(BackupFolderPath, GenerateHash(Address) + ".xml");

        private long GetBackupFileLength()
        {
            var fi = new FileInfo(GetBackupFileName());
            if (!fi.Exists)
            {
                return -1;
            }
            return fi.Length;
        }

        /// <summary>
        /// Attempt to load the repository from its local backup file. <br />
        /// Make sure <see cref="ClearContent()" /> is used before this function.
        /// </summary>
        private bool TryLoadLocalBackup()
        {
            try
            {
                string path = GetBackupFileName();
                AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Repository/localloading",
                    "Loading local backup of repository {0} in {1}", Name, path));

                if (!File.Exists(path))
                {
                    AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/localloadnotfound",
                        "No local backup found for repository {0} at {1}", Name, path));
                    return false;
                }

                byte[] raw = File.ReadAllBytes(path);
                string data = Encoding.UTF8.GetString(raw);
                TriggernometryExport exp = TriggernometryExport.Unserialize(data);
                if (exp.Corrupted)
                {
                    AddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Repository/localloadcorrupted",
                        "Backup data for repository {0} could not be unserialized.", Name));
                    return false;
                }

                CurrentContentLength = raw.Length;
                AddContentFromExport(exp);
                AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/localloaded",
                    "Loaded local backup of repository {0} from {1}", Name, path));
                return true;
            }
            catch (Exception ex)
            {
                AddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Repository/loadlocalex",
                    "Couldn't load local backup of repository {0} due to exception: {1}", Name, ex.Message));
            }
            return false;
        }

        /// <summary>Saves the repository data into a local backup file.</summary>
        private void SaveLocalBackup(byte[] raw)
        {
            try
            {
                string path = GetBackupFileName();
                Directory.CreateDirectory(BackupFolderPath);

                AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Repository/localsaving",
                    "Saving local backup of repository {0} in {1}", Name, path));

                File.WriteAllBytes(path, raw);
                AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/localsaved",
                    "Saved local backup of repository {0} in {1}", Name, path));
            }
            catch (Exception ex)
            {
                AddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Repository/localsaveex",
                    "Couldn't save local backup of repository {0} due to exception: {1}", Name, ex.Message));
            }
        }

        /// <summary>Deletes the local backup file for this repository if it exists.</summary>
        internal void ClearLocalBackup()
        {
            try
            {
                string path = GetBackupFileName();
                if (File.Exists(path))
                {
                    File.Delete(path);
                    AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/localcleared",
                        "Cleared local backup of repository {0} at {1}", Name, path));
                }
                else
                {
                    AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Repository/localclearnotfound",
                        "No local backup found for repository {0} at {1}", Name, path));
                }
            }
            catch (Exception ex)
            {
                AddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Repository/localclearex",
                    "Couldn't clear local backup of repository {0} due to exception: {1}", Name, ex.Message));
            }
        }

        #endregion Backup

    }

}
