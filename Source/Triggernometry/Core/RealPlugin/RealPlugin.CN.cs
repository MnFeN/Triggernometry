using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Localization;
using static Triggernometry.UI.CustomControls.UserInterface;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {

        private void FixConfigurationOnStartCN()
        {
            cfg.ShowWelcome = false;
            cfg.TestLiveByDefault = true;
            cfg.TestIgnoreConditionsByDefault = true;
            cfg.TtsMethod = Configuration.AudioRoutingMethodEnum.ACT;
            cfg.AutosaveEnabled = true;
            cfg.UpdateNotifications = Configuration.UpdateNotificationsEnum.Yes;
            cfg.UpdateCheckMethod = Configuration.UpdateCheckMethodEnum.External;
            cfg.UpdateExternalChannelUrl = "https://1824544011.v.123pan.cn/1824544011/Triggernometry_Release_CN/UpdateManifest.xml";
            cfg.AutoUpdate = true;
            var apis = (List<Configuration.APIUsage>)cfg?.GetType()?.GetProperty("_APIUsages", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg);
            var utilities = apis?.FirstOrDefault(a => a.Name == "Triggernometry.Utilities");
            if (utilities != null)
            { 
                utilities.AllowLocal = true;
                utilities.AllowRemote = true;
                utilities.AllowAdmin = true;
            }
        }

        public static void CopyMissingTranslations()
        {
            var entries = I18n.CurrentLanguage.ExportMissingTranslations();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Language.TranslationEntry>), new XmlRootAttribute("Translations"));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, entries);
                string serializedEntries = writer.ToString();
                ActionOld.ClipboardSetText(serializedEntries);
                MessageBox.Show("Missing translations copied to clipboard.");
            }
        }

        private static readonly List<string> _legalRepoPrefixes = new List<string> {
            "https://github.com/paissaheavyindustries/Triggernometry",
            "https://vip.123pan.cn/1824544011/",
            "https://1824544011.v.123pan.cn/",
        };

        public void AddRepo(Repository r, bool shouldUpdate)
        {
            if (ui.InvokeRequired)
            {
                ui.Invoke(new Action(() => AddRepo(r, shouldUpdate)));
                return;
            }
            if (!_legalRepoPrefixes.Any(prefix => r.Address.StartsWith(prefix)))
            {
                UnfilteredAddToLog(DebugLevelEnum.Error,
                    I18n.IsChineseEnvironment
                    ? $"正在尝试添加的远程仓库地址 {r.Address} 未在信任列表内，你需要手动添加此远程仓库。"
                    : $"The repository address {r.Address} you are trying to add is not a trusted address and needs to be added manually."
                );
                return;
            }

            RepositoryFolder rfo = (RepositoryFolder)ui.treeView1.Nodes[1].Tag;
            TreeNode tn = rfo.Repositories
                .Where(repo => repo.Address == r.Address)
                .Select(repo => ui.treeView1.Nodes[1].Nodes.Cast<TreeNode>().FirstOrDefault(node => node.Tag == repo))
                .FirstOrDefault();

            if (tn != null)
            {
                Repository existingRepo = (Repository)tn.Tag;
                existingRepo.Name = r.Name;
                // do not change enable state
                existingRepo.AllowProcessLaunch = r.AllowProcessLaunch;
                existingRepo.AllowScriptExecution = r.AllowScriptExecution;
                existingRepo.AllowDiskOperations = r.AllowDiskOperations;
                existingRepo.AllowWindowMessages = r.AllowWindowMessages;
                existingRepo.AllowObsControl = r.AllowObsControl;
                existingRepo.KeepLocalBackup = r.KeepLocalBackup;
                existingRepo.UpdatePolicy = r.UpdatePolicy;
                existingRepo.AudioOutput = r.AudioOutput;
                existingRepo.AutoUpdate = r.AutoUpdate;
                existingRepo.UpdateInterval = r.UpdateInterval;

                tn.Text = existingRepo.Name;
                tn.Checked = existingRepo.Enabled;
                tn.ImageIndex = (int)ImageIndices.RemoteRepoUnavailable;
                tn.SelectedImageIndex = tn.ImageIndex;
            }
            else
            {
                tn = new TreeNode
                {
                    Text = r.Name,
                    Tag = r,
                    Checked = r.Enabled,
                    ImageIndex = (int)ImageIndices.RemoteRepoUnavailable
                };
                tn.SelectedImageIndex = tn.ImageIndex;
                rfo.Repositories.Add(r);
                r.Parent = rfo;
                ui.treeView1.Nodes[1].Nodes.Add(tn);
                ui.treeView1.Nodes[1].Expand();
            }
            ui.RecolorStartingFromNode(tn.Parent, tn.Parent.Checked, true);
            ui.treeView1.Sort();
            if (shouldUpdate)
            {
                ui.ForceUpdateRepository(tn);
            }
        }

        public void AddRepos(IEnumerable<Repository> repos, bool shouldUpdate)
        {
            foreach (Repository r in repos)
            {
                AddRepo(r, shouldUpdate);
            }
        }

        public void RemoveRepo(string partialUrl)
        {
            if (ui.InvokeRequired)
            {
                ui.Invoke(new Action(() => RemoveRepo(partialUrl)));
                return;
            }
            partialUrl = partialUrl.Trim();
            RepositoryFolder rfo = (RepositoryFolder)ui.treeView1.Nodes[1].Tag;
            var nodes = rfo.Repositories
                .Where(repo => repo.Address.IndexOf(partialUrl, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(repo => ui.treeView1.Nodes[1].Nodes.Cast<TreeNode>().FirstOrDefault(node => node.Tag == repo))
                .ToList();

            for (int i = 0; i < nodes.Count; i++)
            {
                var tn = nodes[i];
                if (tn == null) continue;
                Repository r = (Repository)tn.Tag;
                r.Enabled = false;
                rfo.Repositories.Remove(r);
                ui.treeView1.Nodes[1].Nodes.Remove(tn);
            }
        }

        public Repository DefaultRepoCN(string address, string name, int updateIntervalMinutes) => new Repository
        {
            Enabled = true,
            Address = address,
            AllowProcessLaunch = true,
            AllowScriptExecution = true,
            KeepLocalBackup = true,
            Name = name,
            NewBehavior = Repository.NewBehaviorEnum.AsDefined,
            UpdatePolicy = Repository.UpdatePolicyEnum.Startup,
            AudioOutput = Repository.AudioOutputEnum.NeverOverride,
            AutoUpdate = true,
            UpdateInterval = updateIntervalMinutes
        };

        public void AddDefaultRepoCN(bool shouldUpdate = false)
        {
            var now = DateTime.Now;
            var isVCPeriod = new DateTime(2026, 3, 3) < now && now < new DateTime(2026, 3, 17);
            var repos = new List<Repository>
            {
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/SelfTest.xml",
                    "[工具] 问题自检工具箱 + 使用教程　　有问题请自行在此解决", 60 * 6),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/Utils.xml",
                    "[工具] 运行支持库（必需）", 60 * 6),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/S7a.xml",
                    "7.0 M1-4 阿卡狄亚轻量级", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/S7b.xml",
                    "7.2 M5-8 阿卡狄亚中量级", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/S7c.xml",
                    "7.4 M9-12 阿卡狄亚重量级", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/Ex7.xml",
                    "7.X 极神", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/temp.xml",
                    "临时推送", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/U7a.xml",
                    "7.1 绝伊甸", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/field.xml",
                    "特殊场景探索", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/dungeon.xml",
                    "深宫", 60 * 24),
                DefaultRepoCN("https://1824544011.v.123pan.cn/1824544011/Remote_Triggers/vc.xml",
                    "异闻迷宫", isVCPeriod ? 60 : 60 * 24),
            };
            RemoveRepo("vip.123pan.cn/1824544011"); // old
            AddRepos(repos, shouldUpdate);
        }

    }
}
