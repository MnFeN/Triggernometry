using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Triggernometry.Localization;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {

        /// <summary> Set text to the clipboard using the UI thread. If text is empty, clear the clipboard. </summary>
        public static void ClipboardSetText(string text)
        {
            Instance.ui.Invoke(new System.Action(() =>
            {
                if (string.IsNullOrEmpty(text))
                    Clipboard.Clear();
                else
                    Clipboard.SetText(text);
            }));
        }

        /// <summary> Get the clipboard text using the UI thread. </summary>
        public static string ClipboardGetText()
            => (string)Instance.ui.Invoke(new Func<string>(Clipboard.GetText));

        internal static string FormatDateTime(DateTime dt)
        {
            return dt.ToString("MM-dd HH:mm:ss.fff");
        }

        internal static string GenerateHash(string addy)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(addy);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }

        // 按完整缓存路径协调，同一路径始终使用同一个锁，不同资源的下载互不阻塞。
        private static readonly ConcurrentDictionary<string, object> resourceCacheLocks =
            new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>返回完整的本地缓存文件；有效期为空时沿用已有文件，禁用缓存时始终重新下载。</summary>
        internal string GetCachedResource(Uri uri, string cacheFolder, int? expiryMinutes, string userAgent, bool useCache = true)
        {
            string directory = Path.GetFullPath(Path.Combine(ConfigPath, cacheFolder));
            string filename = Path.Combine(directory, GenerateHash(uri.AbsoluteUri) + Path.GetExtension(uri.LocalPath));
            lock (resourceCacheLocks.GetOrAdd(filename, _ => new object()))
            {
                // 等待其他下载完成后再检查缓存，复用刚写入的文件。
                if (useCache && File.Exists(filename) && (!expiryMinutes.HasValue ||
                    new FileInfo(filename).LastWriteTime > DateTime.Now.AddMinutes(-expiryMinutes.Value)))
                    return filename;

                byte[] data;
                using (var client = new WebClient())
                {
                    client.Headers["User-Agent"] = userAgent;
                    data = client.DownloadData(uri.AbsoluteUri);
                }

                // 同卷临时文件写完后再发布；独立子目录避开只清理正式缓存的 GetFiles()。
                string tempDirectory = Path.Combine(directory, ".downloads");
                Directory.CreateDirectory(tempDirectory);
                string tempFilename = Path.Combine(tempDirectory, Guid.NewGuid().ToString("N") + ".tmp");
                try
                {
                    File.WriteAllBytes(tempFilename, data);
                    Utilities.WindowsUtils.ReplaceFile(tempFilename, filename);
                }
                finally
                {
                    File.Delete(tempFilename);
                }
                return filename;
            }
        }

        public bool CheckIfAdministrator(bool warnIfNotAdmin)
        {
            bool ret;
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                ret = principal.IsInRole(WindowsBuiltInRole.Administrator);
                if (ret == false && warnIfNotAdmin == true)
                {
                    var t = new UI.CustomControls.Toast();
                    t.ToastText = I18n.Translate("internal/Plugin/notadministrator", "You are not running ACT as an administrator - this might prevent some triggers from working.");
                    t.ToastType = UI.CustomControls.Toast.ToastTypeEnum.OK;
                    ui.QueueToast(t);
                }
            }
            return ret;
        }

        // to-do: These chars should not be kept in Regex textboxes.
        // Better warn the user when they paste these invalid chars, or delete them directly.
        internal static string SerializeInvalidXmlCharacters(string ex)
        {
            ex = ex.Replace("&#x0;", "␀");
            ex = ex.Replace("&#x1;", "␁");
            ex = ex.Replace("&#x2;", "␂");
            ex = ex.Replace("&#x3;", "␃");
            ex = ex.Replace("&#x4;", "␄");
            ex = ex.Replace("&#x5;", "␅");
            ex = ex.Replace("&#x6;", "␆");
            ex = ex.Replace("&#x7;", "␇");
            ex = ex.Replace("&#x8;", "␈");
            ex = ex.Replace("&#x9;", "␉");
            ex = ex.Replace("&#xa;", "␊");
            ex = ex.Replace("&#xb;", "␋");
            ex = ex.Replace("&#xc;", "␌");
            ex = ex.Replace("&#xd;", "␍");
            ex = ex.Replace("&#xe;", "␎");
            ex = ex.Replace("&#xf;", "␏");
            ex = ex.Replace("&#x10;", "␐");
            ex = ex.Replace("&#x11;", "␑");
            ex = ex.Replace("&#x12;", "␒");
            ex = ex.Replace("&#x13;", "␓");
            ex = ex.Replace("&#x14;", "␔");
            ex = ex.Replace("&#x15;", "␕");
            ex = ex.Replace("&#x16;", "␖");
            ex = ex.Replace("&#x17;", "␗");
            ex = ex.Replace("&#x18;", "␘");
            ex = ex.Replace("&#x19;", "␙");
            ex = ex.Replace("&#x1a;", "␚");
            ex = ex.Replace("&#x1b;", "␛");
            ex = ex.Replace("&#x1c;", "␜");
            ex = ex.Replace("&#x1d;", "␝");
            ex = ex.Replace("&#x1e;", "␞");
            ex = ex.Replace("&#x1f;", "␟");
            ex = ex.Replace("&#x7f;", "␡");
            return ex;
        }

        internal static string UnserializeInvalidXmlCharacters(string ex)
        {
            char exx = '\x01';
            string hex = ex;
            ex = ex.Replace("␀", "\x00");
            ex = ex.Replace("␁", "" + exx);
            ex = ex.Replace("␂", "\x02");
            ex = ex.Replace("␃", "\x03");
            ex = ex.Replace("␄", "\x04");
            ex = ex.Replace("␅", "\x05");
            ex = ex.Replace("␆", "\x06");
            ex = ex.Replace("␇", "\x07");
            ex = ex.Replace("␈", "\x08");
            ex = ex.Replace("␉", "\x09");
            ex = ex.Replace("␊", "\x0a");
            ex = ex.Replace("␋", "\x0b");
            ex = ex.Replace("␌", "\x0c");
            ex = ex.Replace("␍", "\x0d");
            ex = ex.Replace("␎", "\x0e");
            ex = ex.Replace("␏", "\x0f");
            ex = ex.Replace("␐", "\x10");
            ex = ex.Replace("␑", "\x11");
            ex = ex.Replace("␒", "\x12");
            ex = ex.Replace("␓", "\x13");
            ex = ex.Replace("␔", "\x14");
            ex = ex.Replace("␕", "\x15");
            ex = ex.Replace("␖", "\x16");
            ex = ex.Replace("␗", "\x17");
            ex = ex.Replace("␘", "\x18");
            ex = ex.Replace("␙", "\x19");
            ex = ex.Replace("␚", "\x1a");
            ex = ex.Replace("␛", "\x1b");
            ex = ex.Replace("␜", "\x1c");
            ex = ex.Replace("␝", "\x1d");
            ex = ex.Replace("␞", "\x1e");
            ex = ex.Replace("␟", "\x1f");
            ex = ex.Replace("␡", "\x7f");
            return ex;
        }

        internal Repository GetRepositoryById(Guid id)
        {
            return (from ix in Config.RepositoryRoot.Repositories where ix.Id == id select ix).FirstOrDefault();
        }

        internal Trigger GetTriggerById(Guid id, Repository repo)
        {
            if (id == Guid.Empty) return null;
            lock (Triggers)
            {
                // Preserve the first registered match, including duplicate IDs in the same repository.
                if (triggersById == null)
                {
                    triggersById = new Dictionary<(Guid Id, Repository Repo), Trigger>();
                    foreach (Trigger trigger in Triggers)
                    {
                        var key = (trigger.Id, trigger.Repo);
                        if (!triggersById.ContainsKey(key)) triggersById.Add(key, trigger);
                    }
                }
                return triggersById.TryGetValue((id, repo), out Trigger result) ? result : null;
            }
        }

        internal Folder GetFolderById(Guid id, Repository repo)
        {
            if (id == Guid.Empty) return null;
            if (repo != null)
            {
                return RecursiveFolderSearch(repo.Root, id, repo);
            }
            else
            {
                return RecursiveFolderSearch(Config.Root, id, repo);
            }
        }

        internal Folder RecursiveFolderSearch(Folder f, Guid id, Repository repo)
        {
            if (f.Id == id && f.Repo == repo)
            {
                return f;
            }
            foreach (Folder c in f.Folders)
            {
                Folder ex = RecursiveFolderSearch(c, id, repo);
                if (ex != null)
                {
                    return ex;
                }
            }
            return null;
        }

        internal TreeNode LocateNodeHostingTrigger(TreeNode tn, Trigger t)
        {
            if (tn.Tag == t)
            {
                return tn;
            }
            foreach (TreeNode tc in tn.Nodes)
            {
                TreeNode tp = LocateNodeHostingTrigger(tc, t);
                if (tp != null)
                {
                    return tp;
                }
            }
            return null;
        }

        internal TreeNode LocateNodeHostingRepository(TreeNode tn, Repository r)
        {
            foreach (TreeNode tc in tn.Nodes)
            {
                if (tc.Tag == r)
                {
                    return tc;
                }
            }
            return null;
        }

        internal TreeNode LocateNodeHostingFolder(TreeNode tn, Folder f)
        {
            if (tn.Tag == f)
            {
                return tn;
            }
            foreach (TreeNode tc in tn.Nodes)
            {
                TreeNode tp = LocateNodeHostingFolder(tc, f);
                if (tp != null)
                {
                    return tp;
                }
            }
            return null;
        }

        internal TreeNode LocateNodeHostingTriggerId(TreeNode tn, Guid id, Repository repo)
        {
            Trigger t = GetTriggerById(id, repo);
            if (t == null)
            {
                return null;
            }
            return LocateNodeHostingTrigger(tn, t);
        }

        internal TreeNode LocateNodeHostingRepositoryId(TreeNode tn, Guid id)
        {
            Repository r = GetRepositoryById(id);
            if (r == null)
            {
                return null;
            }
            return LocateNodeHostingRepository(tn, r);
        }

        internal TreeNode LocateNodeHostingFolderId(TreeNode tn, Guid id, Repository repo)
        {
            Folder f = GetFolderById(id, repo);
            if (f == null)
            {
                return null;
            }
            return LocateNodeHostingFolder(tn, f);
        }

        public static bool IsAdmin()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

    }

    public static class Extensions
    {
        public static string FullMessage(this Exception ex)
        {
            if (ex == null)
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"{ex.GetType().Name}: {ex.Message}");
            if (!string.IsNullOrEmpty(ex.StackTrace))
                sb.AppendLine(ex.StackTrace);

            var inner = ex.InnerException;
            while (inner != null)
            {
                sb.AppendLine("---------");
                sb.AppendLine($"Inner: {inner.GetType().Name}: {inner.Message}");
                if (!string.IsNullOrEmpty(inner.StackTrace))
                    sb.AppendLine(inner.StackTrace);
                inner = inner.InnerException;
            }
            return sb.ToString();
        }
    }
}
