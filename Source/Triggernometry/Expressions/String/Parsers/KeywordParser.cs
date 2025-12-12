using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.FFXIV;
using Triggernometry.Localization;
using Triggernometry.PluginBridges;
using static Triggernometry.Core.Configuration;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class KeywordParser
    {
        internal static string TryParse(string rawExpr, Context ctx, bool isTestModeNumeric)
        {
            ctx = ctx ?? Context.Unbound;
            var plug = ctx.Plugin; // can be null

            // cases related with the triggered event (allowed to get actual values when testing by placeholder)
            switch (rawExpr)
            {
                case "_since":
                    return ctx.SinceTriggered().ToString();

                case "_sincems":
                    return ctx.SinceTriggeredMs().ToString();

                case "_timestamp":
                    return ctx.TimestampTriggered().ToString();

                case "_timestampms":
                    return ctx.TimestampTriggeredMs().ToString();

                case "_zone":
                    return ctx.zoneName;

                case "_event":
                    return ctx.triggeredText;

                default:
                    if (ctx.testByPlaceholder)
                        return isTestModeNumeric ? "1" : "test";
                    break;
            }

            // other cases
            switch (rawExpr)
            {
                // ===== Time =====
                case "_systemtime":
                    return ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString();

                case "_systemtimems":
                    return ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();

                case "_ffxivtime":
                case "_ET":
                    return ((int)Math.Floor(EorzeanTime.Now.TotalMinutes)).ToString();

                case "_ETprecise":
                    return EorzeanTime.Now.TotalMinutes.ToString();


                // ===== FFXIV info =====
                case "_ffxivplayer":
                case "_me":
                    return BridgeFFXIV.GetMyself().GetValue("name").ToString();

                case "_ffxivzoneid":
                    return ctx.zoneIdOverride ?? BridgeFFXIV.ZoneID.ToString();

                case "_ffxivpartyorder":
                    return plug.cfg.FfxivPartyOrdering + " " + plug.cfg.FfxivCustomPartyOrder;

                case "_ffxivprocid":
                    return BridgeFFXIV.GetProcessId().ToString();

                case "_ffxivprocname":
                    return BridgeFFXIV.GetProcessName();

                case "_ffxivversion":
                    return BridgeFFXIV.GetGameVersion();

                case "_ffxivlanguage":
                    return GameLanguage.Language.ToString();

                case "_ffxivlanguageid":
                    return I18n.ThingToString((int)GameLanguage.Language);

                case "_ffxivisglobal":
                    return (byte)GameLanguage.Language <= 3 ? "1" : "0";


                // ===== Combat status =====
                case "_ffxivincombat": // game state
                    return ModuleInCombat.GetInCombat() ? "1" : "0";

                case "_incombat": // ACT state
                    return plug != null && plug.InCombatHook() ? "1" : "0";

                case "_duration":
                    if (plug?.InCombatHook?.Invoke() == true)
                        return Math.Floor(plug.EncounterDurationHook()).ToString("0.###", CultureInfo.InvariantCulture);
                    else
                        return "0";

                case "_lastencounter":
                    return plug?.LastEncounterHook() ?? "";

                case "_activeencounter":
                    return plug?.ActiveEncounterHook() ?? "";


                // ===== HTTP response =====
                case "_response":
                    return ctx?.contextResponse ?? "";

                case "_responsecode":
                    return ctx.contextResponseCode.ToString();


                // ===== Trigger info =====
                case "_triggername":
                    return ctx.Trigger?.Name ?? "(null)";

                case "_triggerid":
                    return ctx.Trigger?.Id.ToString() ?? "(null)";

                case "_triggerpath":
                    return ctx.Trigger?.FullPath ?? "";

                // ===== Dynamic expressions =====
                case "_loopiterator":
                case "_i":
                    return ctx.loopIterator.ToString();

                case "_idx":
                    return ctx.listIndex.ToString();

                case "_col":
                    return ctx.tableColIndex.ToString();

                case "_row":
                    return ctx.tableRowIndex.ToString();

                case "_key":
                    return ctx?.dictKey ?? "";

                case "_val":
                    return ctx?.dictValue ?? "";

                case "_this": // 【需要优化】
                    if (ctx.varName.StartsWith("tvar:") || ctx.varName.StartsWith("ptvar:"))
                        return $"${{{ctx.varName}[{ctx.tableColIndex}][{ctx.tableRowIndex}]}}";
                    else
                        return $"${{{ctx.varName}[{ctx.listIndex}]}}";


                // ===== Triggernometry info =====
                case "_configpath":
                    return plug.ConfigPath;

                case "_pluginpath":
                    return plug.pluginPath;

                case "_pluginversion":
                    return plug.cfg.PluginVersion;


                // ===== Screen =====
                case "_screenwidth":
                    return I18n.ThingToString(Screen.PrimaryScreen.WorkingArea.Width);

                case "_screenheight":
                    return I18n.ThingToString(Screen.PrimaryScreen.WorkingArea.Height);

                case "_screenminx":
                    return I18n.ThingToString(plug.MinX);

                case "_screenminy":
                    return I18n.ThingToString(plug.MinY);

                case "_screenmaxx":
                    return I18n.ThingToString(plug.MaxX);

                case "_screenmaxy":
                    return I18n.ThingToString(plug.MaxY);

                // ===== Misc =====
                case "_clipboard":
                    return Core.ActionOld.ClipboardGetText();

                default: 
                    break;
            }

            // ${1} ${2}
            if (int.TryParse(rawExpr, out int groupIdx))
            {
                var result = ctx.GetNumGroup(groupIdx);
                if (plug != null)
                {
                    result = plug.cfg.PerformSubstitution(result, Substitution.SubstitutionScopeEnum.CaptureGroup);
                }
                return result;
            }

            // ${capturedGroupName}
            if (ctx.namedGroups.TryGetValue(rawExpr, out var namedResult))
            {
                if (plug != null)
                {
                    namedResult = plug.cfg.PerformSubstitution(namedResult, Substitution.SubstitutionScopeEnum.CaptureGroup);
                }
                return namedResult;
            }

            return null; // not found
        }
    }
}
