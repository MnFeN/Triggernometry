using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.Core
{

    public class Folder
    {

        internal Folder Parent { get; set; }

        internal Repository Repo { get; set; } = null;

        internal string FullPath => Parent == null ? Name : $@"{Parent.FullPath}\{Name}"; // recursive



        [XmlAttribute("Id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; } = true;

        [XmlArray("Folders")]
        [XmlArrayItem("Folder")]
        public List<Folder> Folders { get; set; } = new List<Folder>();
        public bool ShouldSerializeFolders() => Folders != null && Folders.Count > 0;
        

        [XmlArray("Triggers")]
        [XmlArrayItem("Trigger")]
        public List<Trigger> Triggers { get; set; } = new List<Trigger>();
        public bool ShouldSerializeTriggers() => Triggers != null && Triggers.Count > 0;


        [XmlAttribute("Name")]
        public string Name { get; set; }

        // ============ ACT Zone Name ============

        [XmlIgnore]
        internal bool ZoneFilterEnabled { get; set; } = false;

        [XmlAttribute("ZoneFilterEnabled")]
        public string Xml_ZoneFilterEnabled
        {
            get => XmlAttr.Bool(ZoneFilterEnabled, false);
            set => ZoneFilterEnabled = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        private string _zoneRegex = "";

        [XmlIgnore]
        private Regex _regexCacheZone;

        [XmlIgnore]
        public string ZoneRegex
        {
            get => _zoneRegex;
            set => SetRegex(value, ref _zoneRegex, ref _regexCacheZone);
        }

        [XmlAttribute("ZoneFilterRegularExpression")] // old name, kept for now
        public string Xml_ZoneRegex
        {
            get => XmlAttr.String(ZoneRegex);
            set => ZoneRegex = value;
        }



        // ============ FFXIV Zone ID ============

        [XmlIgnore]
        internal bool FFXIVZoneFilterEnabled { get; set; } = false;

        [XmlAttribute("FFXIVZoneFilterEnabled")]
        public string Xml_FFXIVZoneFilterEnabled
        {
            get => XmlAttr.Bool(FFXIVZoneFilterEnabled, false);
            set => FFXIVZoneFilterEnabled = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        private string _ffxivZoneIdRegex = "";

        [XmlIgnore]
        private Regex _regexCacheFfxivZoneId;

        [XmlIgnore]
        public string FfxivZoneIdRegex
        {
            get => _ffxivZoneIdRegex;
            set => SetRegex(value, ref _ffxivZoneIdRegex, ref _regexCacheFfxivZoneId);
        }

        [XmlAttribute("FfxivZoneFilterRegularExpression")] // old name, kept for now
        public string Xml_FfxivZoneIdRegex
        {
            get => XmlAttr.String(FfxivZoneIdRegex);
            set => FfxivZoneIdRegex = value;
        }



        // ============ Event Text ============

        [XmlIgnore]
        internal bool EventFilterEnabled { get; set; } = false;

        [XmlAttribute("EventFilterEnabled")]
        public string Xml_EventFilterEnabled
        {
            get => XmlAttr.Bool(EventFilterEnabled, false);
            set => EventFilterEnabled = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        private string _eventRegex = "";

        [XmlIgnore]
        private Regex _regexCacheEvent;

        [XmlIgnore]
        public string EventRegex
        {
            get => _eventRegex;
            set => SetRegex(value, ref _eventRegex, ref _regexCacheEvent);
        }

        [XmlAttribute("EventFilterRegularExpression")] // old name, kept for now
        public string Xml_EventRegex
        {
            get => XmlAttr.String(EventRegex);
            set => EventRegex = value;
        }



        // ============ FFXIV Job ============

        [XmlIgnore]
        internal bool FFXIVJobFilterEnabled { get; set; } = false;

        [XmlAttribute("FFXIVJobFilterEnabled")]
        public string Xml_FFXIVJobFilterEnabled
        {
            get => XmlAttr.Bool(FFXIVJobFilterEnabled, false);
            set => FFXIVJobFilterEnabled = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        internal long FFXIVJobFilter { get; set; } = 0;

        [XmlAttribute("FFXIVJobFilter")]
        public string Xml_FFXIVJobFilter
        {
            get => XmlAttr.Long(FFXIVJobFilter, 0);
            set => FFXIVJobFilter = XmlAttr.Long(value);
        }



        // ============ Environment Variables ============

        [XmlIgnore]
        private string _rawEnvironmentVariables;

        [XmlIgnore]
        public Dictionary<string, string> EnvironmentVariables { get; private set; }
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [XmlIgnore]
        public string RawEnvironmentVariables
        {
            get => _rawEnvironmentVariables;
            set
            {
                _rawEnvironmentVariables = value;
                ParseRawEnvironmentVariables();
            }
        }

        [XmlAttribute("RawEnvironmentVariables")]
        public string Xml_RawEnvironmentVariables
        {
            get => XmlAttr.String(RawEnvironmentVariables);
            set => RawEnvironmentVariables = value;
        }

        internal void ParseRawEnvironmentVariables()
        {
            EnvironmentVariables.Clear();

            if (string.IsNullOrWhiteSpace(RawEnvironmentVariables)) return;
            // separate each lines
            var kvps = ArgHelper.SplitArguments(
                ParserCommon.ReplaceLineBreak(RawEnvironmentVariables),
                allowEmptyList: false,
                separator: ParserCommon.LINEBREAK_STR
            );
            foreach (var rawkvp in kvps)
            {
                if (rawkvp.StartsWith("//") || string.IsNullOrWhiteSpace(rawkvp)) continue;
                var kvp = ArgHelper.SplitArguments(rawkvp, allowEmptyList: false, separator: "=");
                if (kvp.Length >= 2)
                {
                    EnvironmentVariables[kvp[0]] = kvp[1];
                }
                if (kvp.Length > 2)
                {
                    RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Folder/envvariablekvptoolong",
                        "The raw environment variable key-value pair expression contains more than 2 parts. \n Folder: {0}; \n Expression: {1}",
                        FullPath, string.Join(" = ", kvp)));
                }
            }
        }



        // ============ Misc ============

        [XmlIgnore]
        internal bool DescendingSort { get; set; } = false;

        [XmlAttribute("DescendingSort")]
        public string Xml_DescendingSort
        {
            get => XmlAttr.Bool(DescendingSort, false);
            set => DescendingSort = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        internal bool ReadOnly { get; set; } = false;

        [XmlAttribute("ReadOnly")]
        public string Xml_ReadOnly
        {
            get => XmlAttr.Bool(ReadOnly, false);
            set => ReadOnly = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        internal bool DisableRemoteExpand { get; set; } = false;

        [XmlAttribute("DisableRemoteExpand")]
        public string Xml_DisableRemoteExpand
        {
            get => XmlAttr.Bool(DisableRemoteExpand, false);
            set => DisableRemoteExpand = XmlAttr.Bool(value);
        }

        [XmlIgnore]
        internal bool DisableRemoteToggle { get; set; } = false;

        [XmlAttribute("DisableRemoteToggle")]
        public string Xml_DisableRemoteToggle
        {
            get => XmlAttr.Bool(DisableRemoteToggle, false);
            set => DisableRemoteToggle = XmlAttr.Bool(value);
        }

        private void SetRegex(string value, ref string rawRegexField, ref Regex regexCacheField)
        {
            value = value ?? "";

            if (rawRegexField == value) return;

            rawRegexField = value;
            if (string.IsNullOrWhiteSpace(rawRegexField))
            {
                regexCacheField = null;
                return;
            }

            try
            {
                regexCacheField = new Regex(rawRegexField);
            }
            catch
            {
                regexCacheField = null;
            }
        }


        public Folder()
        {
        }

        public enum FilterFailReason
        {
            Passed,
            Failed,
            NotEnabled,
            Exception
        }

        public bool ParentsEnabled()
        {
            Folder f = this;
            while (f != null)
            {
                if (f.Enabled == false)
                {
                    return false;
                }
                f = f.Parent;
            }
            return true;
        }

        public bool IsLimited() => ZoneFilterEnabled || EventFilterEnabled || FFXIVJobFilterEnabled || FFXIVZoneFilterEnabled;

        public bool PassesZoneRestriction(string zone)
        {
            if (zone == null)
            {
                return false;
            }
            bool ret = true;
            Folder f = this;
            while (f != null && ret == true)
            {
                if (ret == true && f.ZoneFilterEnabled == true)
                {
                    ret = f._regexCacheZone != null && f._regexCacheZone.IsMatch(zone);
                }
                if (ret == true && f.FFXIVZoneFilterEnabled == true)
                {
                    ret = f._regexCacheFfxivZoneId != null && f._regexCacheFfxivZoneId.IsMatch(PluginBridges.BridgeFFXIV.ZoneID.ToString());
                }
                f = f.Parent;
            }
            return ret;
        }
		
		internal FilterFailReason PassesFilter(LogEvent le)
		{
			bool ret = true;
			Folder f = this;
            while (f != null && ret == true)
			{
                if (f.Enabled == false)
                {
                    return FilterFailReason.NotEnabled;
                }
                if (ret == true && f.ZoneFilterEnabled == true)
				{
					ret = f._regexCacheZone != null && f._regexCacheZone.IsMatch(le.ZoneName);
				}		
				if (ret == true && f.EventFilterEnabled == true)
				{
					ret = f._regexCacheEvent != null && f._regexCacheEvent.IsMatch(le.Text);
				}
                if (ret == true && f.FFXIVZoneFilterEnabled == true)
                {
                    string zId = le.ZoneId ?? PluginBridges.BridgeFFXIV.ZoneID.ToString();
                    ret = f._regexCacheFfxivZoneId != null && f._regexCacheFfxivZoneId.IsMatch(zId);
                }
                if (ret == true && f.FFXIVJobFilterEnabled == true)
                {
                    VariableDictionary vc = PluginBridges.BridgeFFXIV.GetMyself();
                    if (vc != null)
                    {
                        long.TryParse(vc.GetValue("jobid").ToString(), out long currentJob);
                        long shifted = (long)1 << (int)currentJob - 1;
                        ret = (f.FFXIVJobFilter & shifted) != 0;
                    }
                    else
                    {
                        ret = false;
                    }
                }
                f = f.Parent;
			}
			return ret == true ? FilterFailReason.Passed : FilterFailReason.Failed;
		}

        public Dictionary<string, string> RecursiveGetEnvironmentVariables()
        {
            var dict = new Dictionary<string, string>(EnvironmentVariables, StringComparer.OrdinalIgnoreCase);
            for (var parent = Parent; parent != null; parent = parent.Parent)
            {
                foreach (var kv in parent.EnvironmentVariables)
                {
                    if (!dict.ContainsKey(kv.Key)) dict[kv.Key] = kv.Value;
                }
            }
            return dict;
        }

        /// <summary>
        /// Recursively enumerates all <see cref="Trigger"/> instances contained in this folder and its subfolders. <br/>
        /// Each trigger in the current folder is yielded first, followed by triggers from all nested folders.
        /// </summary>
        public IEnumerable<Trigger> RecursiveGetTriggers()
        {
            foreach (var t in Triggers)
                yield return t;

            foreach (var sub in Folders)
            {
                foreach (var t in sub.RecursiveGetTriggers())
                    yield return t;
            }
        }

    }

}
