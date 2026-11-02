using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using Triggernometry.Core.Serialization;

namespace Triggernometry.Core
{

    public class TriggernometryExport
    {
        private Version _pluginVersion;
        [XmlAttribute]
        public string PluginVersion
        { 
            get => _pluginVersion?.ToString();
            set 
            {
                _pluginVersion = string.IsNullOrEmpty(value) ? null : new Version(value);
            }
        }

        public string PluginVersionDescription => string.IsNullOrWhiteSpace(PluginVersion) ? "< 1.2.0.1" : PluginVersion;

        [XmlIgnore]
        public bool Corrupted = false;

        public Folder ExportedFolder;
        public Trigger ExportedTrigger;

        public TriggernometryExport()
        {
            _pluginVersion = null;
            ExportedFolder = null;
            ExportedTrigger = null;
        }

        public string Serialize()
        {
            XmlSerializer xs = new XmlSerializer(typeof(TriggernometryExport));
            return XmlSerialization.Serialize(xs, this);
        }

        public static TriggernometryExport Unserialize(string src)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(TriggernometryExport));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(src)))
                {
                    var result = (TriggernometryExport)xs.Deserialize(ms);
                    result.ExportedFolder?.RecursiveGetTriggers()?.ToList().ForEach(t => t.SetActionsParent());
                    result.ExportedTrigger?.SetActionsParent();
                    return result;
                }
            }
            catch (Exception)
            {
                Regex rexVersion = new Regex(@"TriggernometryExport[^>]+PluginVersion *= *(?<version>\d+\.\d+\.\d+\.\d+)");
                string version = rexVersion.Match(src.Length > 100 ? src.Substring(0, 100) : src).Groups["version"].Value;
                return new TriggernometryExport() { PluginVersion = version, Corrupted = true };
            }
        }

    }

}
