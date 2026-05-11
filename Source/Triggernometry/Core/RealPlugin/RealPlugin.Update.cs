using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.UI.Forms;
using Triggernometry.Utilities;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {
        private static readonly HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        #region Plugin Update

        internal DateTime UpdateLastChecked = DateTime.MinValue;

        internal void CheckForUpdates(bool isManual = false)
        {
            UpdateLastChecked = DateTime.Now;
            switch (cfg.UpdateCheckMethod)
            {
                case Configuration.UpdateCheckMethodEnum.ACT:
                    CheckForUpdatesACT();
                    break;
                case Configuration.UpdateCheckMethodEnum.Builtin:
                    CheckForUpdatesBuiltin(alwaysNotify: isManual);
                    break;
                case Configuration.UpdateCheckMethodEnum.External:
                    CheckForUpdatesExternal(cfg.UpdateExternalChannelUrl, notifyIfLatest: isManual);
                    break;
            }
        }

        internal void CheckForUpdatesACT()
        {
            CheckUpdateHook();
        }

        private string _builtInUpdateDownloadUrl;

        /// <summary>
        /// Checks for new versions of Triggernometry by querying GitHub Releases.
        /// This is the built-in update check (legacy).
        /// </summary>
        internal void CheckForUpdatesBuiltin(bool alwaysNotify = false)
        {
            Task.Run(async () =>
            {
                // Current running plugin version
                Version localVer = Assembly.GetExecutingAssembly().GetName().Version;
                Version latestVer = localVer;
                // The URL of the downloadable asset of the latest version
                string latestAssetUrl = "";

                try
                {
                    // Fetch GitHub release list (JSON array) and parse it
                    string json = await HttpHelper.GetStringAsync("https://api.github.com/repos/paissaheavyindustries/Triggernometry/releases");
                    dynamic releases = new JavaScriptSerializer().DeserializeObject(json);

                    // Iterate through releases and find the highest valid version
                    dynamic latestRelease = null;
                    foreach (dynamic release in releases)
                    {
                        string tag = (string)release["tag_name"]; // e.g. v1.2.3.4
                        if (tag.StartsWith("v"))
                            tag = tag.Substring(1);
                        if (!Version.TryParse(tag, out Version remoteVer))
                            continue;
                        if (remoteVer > latestVer)
                        {
                            latestVer = remoteVer;
                            latestRelease = release;
                        }
                    }
                    // Extract asset URL
                    if (latestRelease?["assets"] is object[] assets && assets.Length > 0 &&
                        assets[0] is Dictionary<string, object> asset && asset.TryGetValue("browser_download_url", out object url))
                    {
                        latestAssetUrl = url.ToString();
                    }
                }
                catch (Exception ex)
                {
                    FilteredAddToLog(DebugLevelEnum.Error,
                        I18n.Translate("internal/Plugin/vercheckfail",
                        "Version update check failed: {0}", ex.ToString()));
                    return;
                }

                if (latestVer > localVer)
                {
                    _builtInUpdateDownloadUrl = latestAssetUrl;

                    FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/verchecknew",
                        "Version check: A new version {0} is available to replace current version {1}", latestVer, localVer));

                    var t = new Toast
                    {
                        ToastText = I18n.Translate("internal/Plugin/downloadnewver",
                            "A new version ({0}) is available. Would you like to open the download page?", latestVer),
                        ToastType = Toast.ToastTypeEnum.YesNo
                    };
                    t.OnYes += (_, __) => System.Diagnostics.Process.Start(_builtInUpdateDownloadUrl);
                    ui.QueueToast(t);
                }
                else
                {
                    var info = I18n.Translate("internal/Plugin/verchecksame",
                        "Version check: Newest version {0} is the same or older than current version {1}", latestVer, localVer);
                    FilteredAddToLog(DebugLevelEnum.Info, info);
                    if (alwaysNotify)
                    {
                        ui.QueueToast(new Toast { ToastText = info, ToastType = Toast.ToastTypeEnum.OK });
                    }
                }
            });
        }

        #endregion Plugin Update

        #region Plugin Update (External)

        /// <summary>
        /// Represents an external update manifest used for Triggernometry auto-updates. <br />
        /// The manifest provides version information, download URLs, and an optional update message displayed to the user.
        /// </summary>
        public class UpdateManifest
        {
            /// <summary> 
            /// The version of the remote plugin file. 
            /// </summary>
            [XmlIgnore]
            public Version Version { get; set; }

            [XmlAttribute("Version")]
            public string Xml_Version
            { 
                get => Version?.ToString();
                set => Version = Version.Parse(value);
            }

            /// <summary>
            /// (Optional) The minimum version, required to continue running without warnings. <br />
            /// If the current version is lower, an urgent restart prompt may be shown.
            /// </summary>
            [XmlIgnore]
            public Version LowestAllowedVersion { get; set; }

            [XmlAttribute("LowestAllowedVersion")]
            public string Xml_LowestAllowedVersion
            {
                get => LowestAllowedVersion?.ToString();
                set => LowestAllowedVersion = Version.Parse(value);
            }

            /// <summary>
            /// The URL to download the remote plugin DLL.
            /// </summary>
            [XmlAttribute]
            public string Url { get; set; }

            /// <summary>
            /// (Optional) The URL to download the updated translation file.
            /// </summary>
            [XmlAttribute]
            public string TranslationUrl { get; set; }

            /// <summary>
            /// (Optional) The message template shown to the user when an update is available. <br />
            /// May contain placeholders {0} (local version) and {1} (remote version). <br />
            /// A default message is used if not provided.
            /// </summary>
            [XmlAttribute]
            public string Message { get; set; }

        }

        public void CheckForUpdatesExternal(string manifestUrl = null, bool notifyIfLatest = false, bool forceAutoUpdate = false)
        {
            manifestUrl = manifestUrl ?? cfg.UpdateExternalChannelUrl;

            Task.Run(async () =>
            {
                try
                {
                    Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;

                    string manifestData = await client.GetStringAsync(manifestUrl);
                    UpdateManifest um;
                    using (var sr = new StringReader(manifestData))
                    {
                        um = (UpdateManifest)new XmlSerializer(typeof(UpdateManifest)).Deserialize(sr);
                    }

                    Version remoteVersion = um.Version;
                    if (remoteVersion <= localVersion)
                    {
                        string info = I18n.Translate("internal/Plugin/verchecksame",
                            "Version check: Newest version {0} is the same or older than current version {1}", remoteVersion, localVersion);
                        Instance.FilteredAddToLog(DebugLevelEnum.Info, info);
                        if (notifyIfLatest)
                        {
                            ui.QueueToast(new Toast { ToastText = info, ToastType = Toast.ToastTypeEnum.OK });
                        }
                        return;
                    }

                    FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/verchecknew",
                        "Version check: A new version {0} is available to replace current version {1}", remoteVersion, localVersion));

                    if (forceAutoUpdate || cfg.UpdateNotifications == Configuration.UpdateNotificationsEnum.Yes)
                    {
                        UpdatePluginExternal(um, localVersion);
                        return;
                    }

                    var msg = um.Message?.Replace("{0}", $"{localVersion}").Replace("{1}", $"{remoteVersion}") // in case the message template is broken
                        ?? I18n.Translate("internal/Plugin/verchecknew", "Version check: A new version {0} is available to replace current version {1}", remoteVersion, localVersion);

                    new TraySlider(2, msg, "Triggernometry Update", TraySliderLevel.Info, 600000)
                    {
                        OnClick1 = () => UpdatePluginExternal(um, localVersion),
                    }.Show();
                }
                catch (Exception ex)
                {
                    Instance.FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/extplugincheckfailed", 
                        "Couldn't process update manifest from {0}, error: {1}", manifestUrl, ex.Message));
                    return;
                }
            });
        }

        internal bool hasAutoUpdated = false;

        private void UpdatePluginExternal(UpdateManifest um, Version localVersion)
        {
            var filePath = Path.Combine(pluginPath, $"{pluginName}.dll");

            Task.Run(async () =>
            {
                try
                {
                    await HttpHelper.DownloadAndReplaceAsync(um.Url, filePath, $"{filePath}.{localVersion}.backup");

                    if (!string.IsNullOrWhiteSpace(um.TranslationUrl))
                    {
                        UpdateTranslationExternal(um);
                    }
                    
                    hasAutoUpdated = true;

                    var isUrgent = um.LowestAllowedVersion != null && um.LowestAllowedVersion > localVersion;

                    string msg;
                    TraySliderLevel level;

                    if (isUrgent) // recommend restart immediately
                    {
                        msg = I18n.Translate("internal/Plugin/extpluginupdatedurgenttrue",
                                "The plugin is updated from {0} to {1}, including an urgent update. It is recommended to restart immediately.\r\n\r\nWould you like to view the changelog?",
                                localVersion, um.Version);
                        level = TraySliderLevel.Warning;
                    }
                    else
                    {
                        msg = I18n.Translate("internal/Plugin/extpluginupdatedurgentfalse",
                            "The plugin is updated from {0} to {1}. This is a non-urgent update. You can restart ACT at your convenience.\r\n\r\nWould you like to view the changelog?",
                            localVersion, um.Version);
                        level = TraySliderLevel.Info;
                    }

                    var title = I18n.Translate("internal/Plugin/triggernometryupdate", "Triggernometry Update");
                    new TraySlider(2, msg, title, level, 600000)
                    {
                        OnClick1 = () => ShowChangeLog(),
                    }.Show();

                    cfg.PreviousNotifiedPluginVersion = um.Version.ToString();
                }
                catch (Exception ex)
                {
                    var err = I18n.Translate("internal/Plugin/extpluginupdatefailed",
                        "Couldn't update plugin file from {0}: {1}", um.Url, ex.Message);
                    Instance.FilteredAddToLog(DebugLevelEnum.Error, err);
                }
            });
        }

        public async Task UpdateTranslationExternal()
        {
            string manifestData = await client.GetStringAsync(cfg.UpdateExternalChannelUrl);
            UpdateManifest um;
            using (var sr = new StringReader(manifestData))
            {
                um = (UpdateManifest)new XmlSerializer(typeof(UpdateManifest)).Deserialize(sr);
            }
            UpdateTranslationExternal(um);
        }

        private void UpdateTranslationExternal(UpdateManifest um)
        {
            string fileName = Path.GetFileName(um.TranslationUrl);
            string localPath = Path.Combine(pluginPath, fileName);

            Task.Run(async () =>
            {
                try
                {
                    await HttpHelper.DownloadAndReplaceAsync(um.TranslationUrl, localPath);
                    var info = I18n.Translate("internal/Plugin/exttranslationupdated",
                        "Translation file updated, restart ACT for changes to take effect.");
                    Instance.FilteredAddToLog(DebugLevelEnum.Info, info);
                    // ui.QueueToast(new Toast { ToastText = info, ToastType = Toast.ToastTypeEnum.OK });
                    // to-do: check version, notify if language file is for different version than plugin
                }
                catch (Exception ex)
                {
                    string err = I18n.Translate("internal/Plugin/exttranslationupdatefail",
                        "Couldn't update translation file from {0}: {1}", um.TranslationUrl, ex.Message);
                    Instance.FilteredAddToLog(DebugLevelEnum.Error, err);
                    ui.QueueToast(new Toast
                    {
                        ToastText = err,
                        ToastType = Toast.ToastTypeEnum.OK
                    });
                }
            });
        }

        public void UpdatePostNamazu(string remoteVersion)
        {
            var plug = InstanceHook(null, "PostNamazu.PostNamazu");
            var localVersion = plug.FileVersion;
            if (localVersion == null)
            {
                ui.QueueToast(new Toast
                {
                    ToastText = $"未找到鲶鱼精邮差插件。",
                    ToastType = Toast.ToastTypeEnum.OK
                });
                return;
            }
            if (new Version(localVersion) >= new Version(remoteVersion))
            {
                ui.QueueToast(new Toast
                {
                    ToastText = $"鲶鱼精邮差插件本地版本 {localVersion} 不低于远程版本 {remoteVersion}，无需更新。",
                    ToastType = Toast.ToastTypeEnum.OK
                });
                return;
            }
            Task.Run(async () =>
            {
                try
                {
                    var filePath = plug.PluginFile.FullName;
                    string tmpPath = filePath + ".tmp";
                    using (HttpClient client = new HttpClient())
                    {
                        byte[] fileBytes = await client.GetByteArrayAsync(UpdateRemotePathCN + "PostNamazu.dll");
                        File.WriteAllBytes(tmpPath, fileBytes);
                    }
                    if (File.Exists(filePath))
                    {
                        string backupPath = $"{filePath}.{localVersion}.backup";
                        if (File.Exists(backupPath))
                            File.Delete(backupPath);
                        File.Move(filePath, backupPath);
                    }
                    File.Move(tmpPath, filePath);
                    ui.QueueToast(new Toast
                    {
                        ToastText = $"鲶鱼精邮差插件已从 {localVersion} 更新至 {remoteVersion}，重启后生效。",
                        ToastType = Toast.ToastTypeEnum.OK
                    });
                }
                catch (Exception ex)
                {
                    FilteredAddToLog(DebugLevelEnum.Error, $"鲶鱼精邮差插件插件更新失败：{ex.Message}");
                }
            });
        }

        public void ShowChangeLog()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://docs.qq.com/doc/DTFZFZFF0dGh2eWhm",
                UseShellExecute = true
            });
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/MnFeN/Triggernometry/wiki/%E6%9B%B4%E6%96%B0%E6%97%A5%E5%BF%97",
                UseShellExecute = true
            });
        }

        #endregion Plugin Update (External)

        #region Repo Update

        public async Task UpdateAllRepositoriesAsync(bool isStartup)
        {
            await UpdateRepositoriesAsync(cfg.RepositoryRoot.Repositories.Where(r => r.Enabled), isStartup);
        }

        internal async Task UpdateSingleRepositoryAsync(Repository r)
        {
            string info = I18n.Translate("internal/Plugin/repoupdate", "Going to update {0} repository(s)", 1);
            FilteredAddToLog(DebugLevelEnum.Info, info);
            ShowProgress(-1, info);

            await r.CheckAndUpdateAsync();
            _ = CompleteRepositoryUpdate();
        }

        internal async Task UpdateRepositoriesAsync(IEnumerable<Repository> repos, bool isStartup)
        {
            if (!repos.Any())
            {
                return;
            }
            string trans = I18n.Translate("internal/Plugin/repoupdate", "Going to update {0} repository(s)", repos.Count());
            FilteredAddToLog(DebugLevelEnum.Info, trans);
            ShowProgress(-1, trans);
            int total = repos.Count();
            int completed = 0;
            object progressLock = new object();
            var tasks = repos.Select(async r => 
            {
                if (ExitEvent.WaitOne(0)) return;
                await r.CheckAndUpdateAsync(isStartup);
                int percent; string info;
                lock (progressLock)
                {
                    completed++;
                    info = I18n.Translate("internal/Plugin/repoupdatedcount",
                        "[{0}/{1}] Updated repository {2} at {3}",
                        completed, total, r.Name, r.Address);
                    r.AddToLog(DebugLevelEnum.Info, info);
                    percent = (int)(100.0 * completed / total);
                }
                ShowProgress(percent, info);
            });
            await Task.WhenAll(tasks);
            _ = CompleteRepositoryUpdate();
        }

        private async Task CompleteRepositoryUpdate()
        {
            string info = I18n.Translate("internal/Plugin/repoupdatecomplete", "Repository update complete");
            Instance.UnfilteredAddToLog(DebugLevelEnum.Info, info);
            await ShowProgressWhenComplete(info);
        }

        #endregion
    }
}
