using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {

        private Configuration _config;
        internal Configuration Config => _config;

        // 配置类型固定，读写时复用同一份反射元数据。
        private static readonly PropertyInfo configSecurityLockProperty = typeof(Configuration)
            .GetProperty("SecuritySettingsLocked", BindingFlags.NonPublic | BindingFlags.Instance);

        internal bool configBroken = false;
        internal DateTime lastConfigSave = DateTime.Now;

        /// <summary>补齐缺失的内置常量，并更新当前插件版本。</summary>
        private void AutofixConfiguration()
        {
            if (Config == null)
                return;

            if (!Config._ShowWelcomeHasBeenSet && (Config.Root.Folders.Count > 0 || Config.Root.Triggers.Count > 0))
                Config.ShowWelcome = false;

            var defaults = new Configuration();
            foreach (var pair in defaults.Constants)
            {
                if (!Config.Constants.ContainsKey(pair.Key))
                    Config.Constants[pair.Key] = (VariableScalar)pair.Value.Duplicate();
            }
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Config.Constants["TriggernometryVersionMajor"] = new VariableScalar(version.Major.ToString());
            Config.Constants["TriggernometryVersionMinor"] = new VariableScalar(version.Minor.ToString());
            Config.Constants["TriggernometryVersionBuild"] = new VariableScalar(version.Build.ToString());
            Config.Constants["TriggernometryVersionRevision"] = new VariableScalar(version.Revision.ToString());
        }

        /// <summary>版本变化时提示更新内容、备份配置并记录新版本。</summary>
        public void HandleVersionUpdate()
        {
            string currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string prevVersion = Config.PluginVersion ?? "pre1.0.4.4";
            if (prevVersion == currentVersion) return;
            // Inform the user to view the changelog after a version change.
            // If this version was already notified after an auto-update, do not show it again.

            AskChangeLogOnStart(prevVersion, currentVersion);
            BackupConfiguration(prevVersion, currentVersion);
            Config.PluginVersion = currentVersion;
        }

        /// <summary>每个新版本仅提示一次，点击提示可查看更新日志。</summary>
        private void AskChangeLogOnStart(string prevVersion, string currentVersion)
        {
            if (Config.PreviousNotifiedPluginVersion == currentVersion) return;

            Config.PreviousNotifiedPluginVersion = currentVersion;
            var title = I18n.Translate("internal/Plugin/triggernometryupdate", "Triggernometry Update");
            var msg = I18n.Translate("internal/Plugin/triggernometryupdated",
                "The plugin had been updated from {0} to {1}. \r\nWould you like to view the changelog?",
                prevVersion, currentVersion);
            var tray = TraySlider.Info(2, msg, title);
            tray.OnClick1 = () => ShowChangeLog();
            tray.Show();
        }

        /// <summary>为旧版本创建独立备份；已有同版本备份时保留原文件。</summary>
        private void BackupConfiguration(string prevVersion, string currentVersion)
        {
            string configFilename = Path.Combine(ConfigPath, pluginName + ".config.xml");
            if (!File.Exists(configFilename)) return;

            string backupFilename = Path.Combine(ConfigPath, pluginName + "." + prevVersion + ".config.xml");
            if (File.Exists(backupFilename))
            {
                FilteredAddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Plugin/cfgbackupdatewarn", 
                    "Plugin updated from {0} to {1}, but a backup configuration file already exists, not overwriting",
                    prevVersion, currentVersion));
                return;
            }

            FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/cfgbackupupdate", 
                "Plugin updated from {0} to {1}, backing up configuration as {2}",
                prevVersion, currentVersion, backupFilename));
            File.Copy(configFilename, backupFilename, false);
        }

        /// <summary>记录本次保存时间，并保存当前配置及上一份备份。</summary>
        public void SaveCurrentConfig()
        {
            lastConfigSave = DateTime.Now;
            SaveConfigToFile(Config, Path.Combine(ConfigPath, pluginName + ".config.xml"), true);
        }

        /// <summary> Checks the closing tag from the end without reading the whole configuration. </summary>
        private static bool HasConfigurationEndTag(string filename)
        {
            const string endTag = "</" + nameof(Configuration) + ">";
            using (StreamReader reader = new StreamReader(filename))
            {
                reader.Peek(); // Detect a possible BOM before seeking past it.
                long position = reader.BaseStream.Length;
                string tail;
                do
                {
                    position = Math.Max(0, position - 4096);
                    reader.DiscardBufferedData();
                    reader.BaseStream.Position = position;
                    tail = reader.ReadToEnd().TrimEnd();
                }
                while (position > 0 && tail.Length < endTag.Length);
                return tail.EndsWith(endTag, StringComparison.Ordinal);
            }
        }

        /// <summary>选择配置加载路径；加载失败时禁止覆盖原配置。</summary>
        private Configuration LoadConfigFromFile(string filename)
        {
            try
            {
                FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/cfgload", "Loading configuration from '{0}'", filename));
                var fileInfo = new FileInfo(filename);
                if (!fileInfo.Exists)
                    return CreateDefaultConfig(filename);
                if (!HasConfigurationEndTag(filename))
                    return RecoverConfigFromBackup(filename);

                Configuration config = ReadConfigFile(filename);
                config.lastWrite = fileInfo.LastWriteTimeUtc;
                return config;
            }
            catch (Exception ex)
            {
                FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/Plugin/cfgloadfail",
                    "Loading configuration file failed, make sure you are running the latest version - overwriting configuration suppressed"));
                configBroken = true;
                GenericExceptionHandler(I18n.Translate("internal/Plugin/cfgloadex",
                    "Loading the configuration file '{0}' failed due to an exception", filename), ex);
                return null;
            }
        }

        /// <summary>配置文件不存在时创建默认配置，并锁定安全设置。</summary>
        private Configuration CreateDefaultConfig(string filename)
        {
            FilteredAddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Plugin/cfgnew", "Configuration file '{0}' does not exist, creating a new configuration", filename));
            var config = new Configuration();
            configSecurityLockProperty.SetValue(config, true);
            return config;
        }

        /// <summary>提示配置损坏，尝试加载上一份备份，并将成功恢复的配置写回原路径。</summary>
        private Configuration RecoverConfigFromBackup(string filename)
        {
            string previousFilename = filename + ".previous";
            var previousFile = new FileInfo(previousFilename);
            // to-do: translation file is loaded after this, so the I18n won't work
            string recoveryMessage = I18n.Translate("internal/Plugin/cfgcorrupted",
                "Configuration file has been corrupted: \n" +
                "'{0}' \n\n" +
                "Loading previous configuration file: \n" +
                "'{1}'",
                filename, previousFilename);
            MessageBox.Show(recoveryMessage, "Triggernometry", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // 没有备份时仍尝试读取原文件；仅在备份加载成功后写回，保留 .previous。
            bool hasPrevious = previousFile.Exists;
            Configuration config = ReadConfigFile(hasPrevious ? previousFilename : filename);
            config.lastWrite = previousFile.LastWriteTimeUtc;
            if (hasPrevious)
            {
                config.corruptRecoveryError = recoveryMessage;
                SaveConfigToFile(config, filename, false);
            }
            return config;
        }

        /// <summary>读取配置 XML，并设置已加载状态及配置锁。</summary>
        private static Configuration ReadConfigFile(string filename)
        {
            var serializer = new XmlSerializer(typeof(Configuration));
            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                var config = (Configuration)serializer.Deserialize(stream);
                configSecurityLockProperty.SetValue(config, true);
                config.isNew = false;
                return config;
            }
        }

        /// <summary>生成并校验配置 XML，通过临时文件保存，按需保留上一份配置。</summary>
        private void SaveConfigToFile(Configuration config, string filename, bool backupPrevious)
        {
            try
            {
                FilteredAddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/cfgsave", "Saving configuration to '{0}'", filename));
                var serializer = new XmlSerializer(typeof(Configuration));
                string xml = SerializeConfigXml(config, serializer);
                ValidateConfigXml(xml, serializer);

                string tempFilename = filename + ".temp";
                WriteConfigTempFile(tempFilename, xml);
                ReplaceConfigFile(tempFilename, filename, backupPrevious);
            }
            catch (Exception ex)
            {
                GenericExceptionHandler(I18n.Translate("internal/Plugin/cfgsaveex", 
                    "Saving the configuration file '{0}' failed due to an exception", filename), ex);
            }
        }

        /// <summary>序列化完整配置，并转换 XML 不支持的控制字符。</summary>
        private static string SerializeConfigXml(Configuration config, XmlSerializer serializer)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            using (var stream = new MemoryStream())
            {
                configSecurityLockProperty.SetValue(config, false);
                try
                {
                    serializer.Serialize(stream, config, namespaces);
                }
                finally
                {
                    // 序列化失败时也重新锁定，避免临时解锁状态残留。
                    configSecurityLockProperty.SetValue(config, true);
                }
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                    return SerializeInvalidXmlCharacters(reader.ReadToEnd());
            }
        }

        /// <summary>确认生成的 XML 能反序列化为配置；校验结果不替换当前配置。</summary>
        private static void ValidateConfigXml(string xml, XmlSerializer serializer)
        {
            using (var reader = new StringReader(xml))
                serializer.Deserialize(reader);
        }

        /// <summary>写入临时配置文件，并确认文件结束标记完整。</summary>
        private static void WriteConfigTempFile(string tempFilename, string xml)
        {
            using (var stream = File.Open(tempFilename, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
                writer.Write(xml);

            if (!HasConfigurationEndTag(tempFilename))
                throw new Exception(I18n.Translate("internal/Plugin/cfgsaveincomplete", "The saving process was interrupted.") + "\n");
        }

        /// <summary>用临时文件替换正式配置，按需将旧配置保存为 .previous。</summary>
        private static void ReplaceConfigFile(string tempFilename, string filename, bool shouldBackup)
        {
            // 备份完成前保留正式文件，备份或替换失败时仍可使用当前配置。
            if (shouldBackup && File.Exists(filename))
                File.Copy(filename, filename + ".previous", true);
            Utilities.WindowsUtils.ReplaceFile(tempFilename, filename);
        }

    }
}
