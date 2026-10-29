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
        internal const string UpdateRemotePathCN = "https://1824544011.cdn.123clouddisk.com/1824544011/Triggernometry_Release_CN/";
        private void FixConfigurationOnStartCN()
        {
            Config.ShowWelcome = false;
            Config.TestLiveByDefault = true;
            Config.TestIgnoreConditionsByDefault = true;
            Config.TtsMethod = Configuration.AudioRoutingMethodEnum.ACT;
            Config.AutosaveEnabled = true;
            Config.UpdateNotifications = Configuration.UpdateNotificationsEnum.Yes;
            Config.UpdateCheckMethod = Configuration.UpdateCheckMethodEnum.External;
            Config.UpdateExternalChannelUrl = UpdateRemotePathCN + "UpdateManifest.xml";
            var apis = (List<Configuration.APIUsage>)Config?.GetType()?.GetProperty("_APIUsages", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(Config);
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

        // Custom-root serializers need explicit reuse; defer creation until the first export.
        private static readonly Lazy<XmlSerializer> missingTranslationsSerializer = new Lazy<XmlSerializer>(() =>
            new XmlSerializer(typeof(List<Language.TranslationEntry>), new XmlRootAttribute("Translations")));

        public static void CopyMissingTranslations()
        {
            var entries = I18n.CurrentLanguage.ExportMissingTranslations();

            using (StringWriter writer = new StringWriter())
            {
                missingTranslationsSerializer.Value.Serialize(writer, entries);
                string serializedEntries = writer.ToString();
                RealPlugin.ClipboardSetText(serializedEntries);
                MessageBox.Show(I18n.Translate("internal/Plugin/missingtranslationscopied", "Missing translations copied to clipboard."));
            }
        }


    }
}
