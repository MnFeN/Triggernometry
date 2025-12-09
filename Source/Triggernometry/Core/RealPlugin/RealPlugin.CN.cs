using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {

        public void FixConfigurationCN() => FixConfigurationOnStartCN();

        private void FixConfigurationOnStartCN()
        {
            cfg.ShowWelcome = false;
            cfg.TestLiveByDefault = true;
            cfg.TestIgnoreConditionsByDefault = true;
            cfg.TtsMethod = Configuration.AudioRoutingMethodEnum.ACT;
            cfg.AutosaveEnabled = true;
            cfg.UpdateNotifications = Configuration.UpdateNotificationsEnum.Yes;
            cfg.UpdateCheckMethod = Configuration.UpdateCheckMethodEnum.External;
            cfg.UpdateExternalChannelUrl = "https://vip.123pan.cn/1824544011/Triggernometry_Release_CN/UpdateManifest.xml";
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

    }
}
