using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;
using static Triggernometry.UI.CustomControls.UserInterface;

namespace Triggernometry.Core
{
    public partial class RealPlugin
    {

        private const string DefaultRepoManifestUrl =
            "https://1824544011.cdn.123clouddisk.com/1824544011/Triggernometry_Release_CN/RepositoryManifest.xml";

        private static RepositoryManifest LoadRepositoryManifest(string url)
        {
            byte[] raw = HttpHelper.GetBytesAsync(url).GetAwaiter().GetResult();
            string xml = System.Text.Encoding.UTF8.GetString(raw);

            XmlSerializer serializer = new XmlSerializer(typeof(RepositoryManifest));
            using (StringReader reader = new StringReader(xml))
            {
                return (RepositoryManifest)serializer.Deserialize(reader);
            }
        }


        private static readonly List<string> _legalRepoPrefixes = new List<string> {
            "https://github.com/paissaheavyindustries/Triggernometry",
            "https://vip.123pan.cn/1824544011/",
            "https://1824544011.v.123pan.cn/",
            "https://1824544011.cdn.123clouddisk.com/",
        };

        public void AddRepositoryManifestItem(RepositoryManifestItem item, bool shouldUpdate)
        {
            if (ui.InvokeRequired)
            {
                ui.Invoke(new Action(() => AddRepositoryManifestItem(item, shouldUpdate)));
                return;
            }
            if (!_legalRepoPrefixes.Any(prefix => item.Address.StartsWith(prefix)))
            {
                UnfilteredAddToLog(DebugLevelEnum.Error,
                    I18n.IsChineseEnvironment
                    ? $"正在尝试添加的远程仓库地址 {item.Address} 未在信任列表内，你需要手动添加此远程仓库。"
                    : $"The repository address {item.Address} you are trying to add is not a trusted address and needs to be added manually."
                );
                return;
            }

            RepositoryFolder rfo = (RepositoryFolder)ui.treeView1.Nodes[1].Tag;
            TreeNode tn = rfo.Repositories
                .Where(r => r.Address == item.Address)
                .Select(r => ui.treeView1.Nodes[1].Nodes.Cast<TreeNode>().FirstOrDefault(node => node.Tag == r))
                .FirstOrDefault();

            Repository repo;
            if (tn != null)
            {
                repo = (Repository)tn.Tag;
                // do not change enable state
            }
            else
            {
                repo = new Repository { Enabled = item.Enabled };
            }
            repo.Address = item.Address;
            repo.Name = item.Name;
            repo.AllowProcessLaunch = item.AllowProcessLaunch;
            repo.AllowScriptExecution = item.AllowScriptExecution;
            repo.AllowDiskOperations = item.AllowDiskOperations;
            repo.AllowWindowMessages = item.AllowWindowMessages;
            repo.AllowObsControl = item.AllowObsControl;
            repo.KeepLocalBackup = item.KeepLocalBackup;
            repo.NewBehavior = item.NewBehavior;
            repo.UpdatePolicy = item.UpdatePolicy;
            repo.AudioOutput = item.AudioOutput;
            repo.AutoUpdate = item.AutoUpdate;
            repo.UpdateInterval = item.UpdateInterval;

            if (tn == null)
            {
                tn = new TreeNode
                {
                    Tag = repo,
                };
                rfo.Repositories.Add(repo);
                repo.Parent = rfo;
                ui.treeView1.Nodes[1].Nodes.Add(tn);
                ui.treeView1.Nodes[1].Expand();
            }
            tn.Text = repo.Name;
            tn.Checked = repo.Enabled;
            tn.ImageIndex = (int)ImageIndices.RemoteRepoUnavailable;
            tn.SelectedImageIndex = tn.ImageIndex;

            ui.RecolorStartingFromNode(tn.Parent, tn.Parent.Checked, true);
            ui.treeView1.Sort();
            if (shouldUpdate)
            {
                ui.ForceUpdateRepository(tn);
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

        public void LoadDefaultRepoCN(bool shouldUpdate = false)
        {
            try
            {
                RepositoryManifest repoManifest = LoadRepositoryManifest(DefaultRepoManifestUrl);
                repoManifest.Remove.ForEach(partialUrl => RemoveRepo(partialUrl));
                repoManifest.Add.ForEach(item => AddRepositoryManifestItem(item, shouldUpdate));
            }
            catch (Exception ex)
            {
                FilteredAddToLog(DebugLevelEnum.Error, "无法添加默认远程仓库：" + ex.Message);
            }
        }

        [XmlRoot("RepositoryManifest")]
        public class RepositoryManifest
        {
            [XmlArray("Add")]
            [XmlArrayItem("Repo")]
            public List<RepositoryManifestItem> Add { get; set; } = new List<RepositoryManifestItem>();

            [XmlArray("Remove")]
            [XmlArrayItem("Item")]
            public List<string> Remove { get; set; } = new List<string>();
        }

        public class RepositoryManifestItem
        {
            [XmlAttribute]
            public string Address { get; set; } = "";

            [XmlAttribute]
            public string Name { get; set; } = "";

            [XmlAttribute]
            public int UpdateInterval { get; set; } = 60;

            [XmlAttribute]
            public bool Enabled { get; set; } = true;

            [XmlAttribute]
            public bool AllowProcessLaunch { get; set; } = true;

            [XmlAttribute]
            public bool AllowScriptExecution { get; set; } = true;

            [XmlAttribute]
            public bool AllowDiskOperations { get; set; } = false;

            [XmlAttribute]
            public bool AllowWindowMessages { get; set; } = false;

            [XmlAttribute]
            public bool AllowObsControl { get; set; } = false;

            [XmlAttribute]
            public bool KeepLocalBackup { get; set; } = true;

            [XmlAttribute]
            public bool AutoUpdate { get; set; } = true;

            [XmlAttribute]
            public Repository.NewBehaviorEnum NewBehavior { get; set; } = Repository.NewBehaviorEnum.AsDefined;

            [XmlAttribute]
            public Repository.UpdatePolicyEnum UpdatePolicy { get; set; } = Repository.UpdatePolicyEnum.Startup;

            [XmlAttribute]
            public Repository.AudioOutputEnum AudioOutput { get; set; } = Repository.AudioOutputEnum.NeverOverride;
        }
    }
}
