using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Triggernometry.Localization
{

    public sealed class Language
    {

        public enum MissingHandlingEnum
        {
            DefaultLanguage,
            DefaultString,
            OutputKey
        }

        [XmlAttribute]
        public string LanguageName { get; set; }
        [XmlAttribute]
        public MissingHandlingEnum MissingKeyHandling { get; set; }

        // 内建文本会在翻译调用中按需收集，需允许多个线程同时查询和记录。
        internal ConcurrentDictionary<string, string> TranslationsLookup { get; } = new ConcurrentDictionary<string, string>();

        internal bool IsDefault { get; set; }

        public class TranslationEntry
        {

            [XmlAttribute]
            public string Key { get; set; }
            [XmlAttribute]
            public string Translation { get; set; }

            public TranslationEntry()
            {
            }

        }

        public List<TranslationEntry> Translations = new List<TranslationEntry>();

        public Language()
        {
            IsDefault = true;
            LanguageName = "(undefined)";
            MissingKeyHandling = MissingHandlingEnum.DefaultLanguage;
        }

        internal void BuildLookup()
        {
            foreach (TranslationEntry te in Translations)
            {
                TranslationsLookup[te.Key] = te.Translation;
            }
            Translations.Clear();
        }

        internal void BuildList()
        {
            Translations.Clear();
            foreach (KeyValuePair<string, string> kp in TranslationsLookup.ToArray())
            {
                Translations.Add(new TranslationEntry() { Key = kp.Key, Translation = kp.Value });
            }
            Translations.Sort((a, b) => { return a.Key.CompareTo(b.Key); });
        }

        public string Lookup(string key)
        {
            return TranslationsLookup.TryGetValue(key, out string text) ? text : null;
        }

        private readonly ConcurrentDictionary<string, string> _missingTranslations = new ConcurrentDictionary<string, string>();

        public string Translate(string key, string text, params object[] args)
        {
            if (TranslationsLookup.TryGetValue(key, out string data))
            {
                return I18n.FormatTranslation(data, args);
            }
            if (IsDefault)
            {
                data = TranslationsLookup.GetOrAdd(key, text);
                return I18n.FormatTranslation(data, args);
            }
            if (!string.IsNullOrEmpty(text) && !_missingTranslations.ContainsKey(key))
                _missingTranslations.TryAdd(key, text);
            switch (MissingKeyHandling)
            {
                case MissingHandlingEnum.DefaultString:
                    {
                        if (TranslationsLookup.TryGetValue("internal/default", out string fallback))
                        {
                            return string.Format(fallback, key);
                        }
                        else
                        {
                            return I18n.FormatTranslation(text, args);
                        }
                    }
                case MissingHandlingEnum.OutputKey:
                    return key;
            }
            return I18n.FormatTranslation(text, args);
        }

        internal List<TranslationEntry> ExportMissingTranslations()
        {
            var entries = _missingTranslations.ToArray();
            Array.Sort(entries, (a, b) => a.Key.CompareTo(b.Key));
            return entries.Select(entry => new TranslationEntry {
                Key = entry.Key,
                Translation = entry.Value
            }).ToList();
        }

    }

}
