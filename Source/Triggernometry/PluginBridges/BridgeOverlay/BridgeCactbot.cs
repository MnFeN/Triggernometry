using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Triggernometry.Core;

namespace Triggernometry.PluginBridges
{
    public static class BridgeCactbot
    {

        public static JObject LoadCactbotOptions()
        {
            try
            {
                return ModuleEvents.CallOverlayHandler(new JObject
                {
                    ["call"] = "cactbotLoadData",
                    ["overlay"] = "options"
                }) as JObject;
            }
            catch
            {
                return null;
            }
        }

        public static void SaveCactbotOptions(JObject config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            ModuleEvents.CallOverlayHandler(new JObject
            {
                ["call"] = "cactbotSaveData",
                ["overlay"] = "options",
                ["data"] = config["data"]
            });

            ModuleEvents.CallOverlayHandler(new JObject
            {
                ["call"] = "cactbotReloadOverlays"
            });
        }

        public static void DisableTriggerSetsTtsCallback(object _, string triggerSetNames)
        {
            if (string.IsNullOrWhiteSpace(triggerSetNames)) return;

            var config = LoadCactbotOptions();
            if (config == null)
            { 
                RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, "获取 Cactbot 配置失败.");
                return;
            }

            var names = triggerSetNames
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(name => name.Trim())
                .Where(name => !string.IsNullOrEmpty(name));

            var changed = false;
            foreach (var name in names)
            {
                changed |= DisableTriggerSetTts(config, name);
            }
            if (changed)
            {
                SaveCactbotOptions(config);
            }
        }

        public static bool DisableTriggerSetTts(JObject config, string triggerSetName)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (string.IsNullOrWhiteSpace(triggerSetName))
                return false;

            triggerSetName = triggerSetName.Trim();

            var triggerSet = GetRaidTriggerSet(config, triggerSetName);

            var current = triggerSet.Value<string>("Output");
            var newOption = GetDisabledTtsOption(current);
            if (newOption == null)
                return false;

            triggerSet["Output"] = newOption;
            RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Custom,
                $"已更改 Cactbot 分组 {triggerSetName} 的 TTS 设置： {current ?? "(null)"} -> {newOption}");
            return true;
        }

        private static JObject GetRaidTriggerSet(JObject config, string triggerSetName)
        {
            return config
                .GetOrCreate("data")
                .GetOrCreate("raidboss")
                .GetOrCreate("triggerSets")
                .GetOrCreate(triggerSetName); // e.g. FuturesRewrittenUltimate
        }

        private static string GetDisabledTtsOption(string current)
        {
            switch (current)
            {
                case "ttsOnly":
                    return "disabled";

                case null:
                case "default":
                case "ttsAndText":
                    return "textOnly";

                default:
                    return null;
            }
        }

        public static JObject GetOrCreate(this JObject parent, string property)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            if (parent[property] is JObject child)
                return child;
            child = new JObject();
            parent[property] = child;
            return child;
        }
    }
}