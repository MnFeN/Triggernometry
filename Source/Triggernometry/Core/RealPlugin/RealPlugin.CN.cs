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
        internal const string UpdateRemotePathCN = "https://1824544011.v.123pan.cn/1824544011/Triggernometry_Release_CN/";
        private void FixConfigurationOnStartCN()
        {
            cfg.ShowWelcome = false;
            cfg.TestLiveByDefault = true;
            cfg.TestIgnoreConditionsByDefault = true;
            cfg.TtsMethod = Configuration.AudioRoutingMethodEnum.ACT;
            cfg.AutosaveEnabled = true;
            cfg.UpdateNotifications = Configuration.UpdateNotificationsEnum.Yes;
            cfg.UpdateCheckMethod = Configuration.UpdateCheckMethodEnum.External;
            cfg.UpdateExternalChannelUrl = UpdateRemotePathCN + "UpdateManifest.xml";
            var apis = (List<Configuration.APIUsage>)cfg?.GetType()?.GetProperty("_APIUsages", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(cfg);
            var utilities = apis?.FirstOrDefault(a => a.Name == "Triggernometry.Utilities");
            if (utilities != null)
            { 
                utilities.AllowLocal = true;
                utilities.AllowRemote = true;
                utilities.AllowAdmin = true;
            }
            // 删除 CafeStore 中旧版插件信息（如果存在）
            /*
            try
            {
                var result = PluginBridges.BridgeCafe.AutoRemoveTriggernometryFromCafeStore();
                Instance.UnfilteredAddToLog(DebugLevelEnum.Info, "尝试从 CafeStore 移除旧版 Triggernometry 信息：" + result);
            }
            catch (Exception ex)
            {
                Instance.UnfilteredAddToLog(DebugLevelEnum.Warning, "处理 CafeStore 旧版 Triggernometry 信息时出错：" + ex.Message);
            }
            */
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


    }
}
