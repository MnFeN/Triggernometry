using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Triggernometry
{

    public partial class RealPlugin
    {

        internal List<Trigger> Triggers = new List<Trigger>();

        internal List<Trigger> ActiveTextTriggers = new List<Trigger>();
        internal List<Trigger> ActiveFFXIVNetworkTriggers = new List<Trigger>();
        internal List<Trigger> ActiveACTTriggers = new List<Trigger>();
        internal List<Trigger> ActiveEndpointTriggers = new List<Trigger>();

        private List<Trigger> GetActiveTriggers(Trigger.TriggerSourceEnum src)
        {
            switch (src)
            {
                        case Trigger.TriggerSourceEnum.Log:
                    return ActiveTextTriggers;

                        case Trigger.TriggerSourceEnum.FFXIVNetwork:
                    return ActiveFFXIVNetworkTriggers;

                        case Trigger.TriggerSourceEnum.ACT:
                    return ActiveACTTriggers;

                        case Trigger.TriggerSourceEnum.Endpoint:
                    return ActiveEndpointTriggers;

                        case Trigger.TriggerSourceEnum.None:
                    return null;

                default:
                    throw new ArgumentOutOfRangeException(nameof(src), src, "Unknown trigger source");
                    }
                }

        internal void AddTrigger(Trigger t, bool parentEnabled)
        {
            List<Trigger> activeTriggers = GetActiveTriggers(t.Source);

            lock (Triggers)
            {
                Triggers.Add(t);
                if (t.Enabled == true && parentEnabled == true && activeTriggers != null)
                {
                    lock (activeTriggers)
                        {
                        activeTriggers.Add(t);
                        }
                        }
                        }
                        }

        internal void SourceChange(Trigger t, Trigger.TriggerSourceEnum oldSource, Trigger.TriggerSourceEnum newSource)
                {
            if (oldSource == newSource)
                return;

            if (t.Enabled == false || t.Parent?.ParentsEnabled() != true)
                return;

            var oldList = GetActiveTriggers(oldSource);
            if (oldList != null)
                        {
                lock (oldList)
                        {
                    oldList.Remove(t);
                        }
                        }

            var newList = GetActiveTriggers(newSource);
            if (newList != null)
                        {
                lock (newList)
                {
                    if (!newList.Contains(t))
                    {
                        newList.Add(t);
                        }
                }
            }
        }

        internal void RemoveTriggersFromFolder(Folder f)
        {
            foreach (var trigger in f.RecursiveGetTriggers())
            {
                RemoveTrigger(trigger);
            }
        }

        internal void RemoveTrigger(Trigger t)
        {
            List<Trigger> activeTriggers = GetActiveTriggers(t.Source);
            lock (Triggers)
            {
                if (activeTriggers != null)
                {
                    lock (activeTriggers)
                        {
                        activeTriggers.Remove(t);
                            }
                        }
                Triggers.Remove(t);
            }
        }

        internal void TriggerEnabled(Trigger t)
        {
            List<Trigger> activeTriggers = GetActiveTriggers(t.Source);

            if (activeTriggers == null)
                            return;

            lock (activeTriggers)
                    {
                if (!activeTriggers.Contains(t))
                        {
                    FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigaddbook",
                        "Trigger '{0}' added to bookkeeping", t.LogName));
                    activeTriggers.Add(t);
                    }
            }
        }

        internal void TriggerDisabled(Trigger t)
        {
            List<Trigger> activeTriggers = GetActiveTriggers(t.Source);

            if (activeTriggers != null)
            {
                lock (activeTriggers)
                    {
                    if (activeTriggers.Contains(t))
                        {
                        FilteredAddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigrembook",
                            "Trigger '{0}' removed from bookkeeping", t.LogName));
                        activeTriggers.Remove(t);
                        }
                    }
                        }

                    RemoveAurasFromTrigger(t);
        }

        internal void TestTrigger(Trigger t, LogEvent le, Action.TriggerForceTypeEnum force)
        {
            lock (t)
            {
                Match m = null;
                if ((force & Action.TriggerForceTypeEnum.SkipRegexp) == 0)
                {
                    m = t.CheckMatch(le.Text);
                    if (m == null)
                    {
                        return;
                    }
                    else
                    {
                        t.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigmatches", "Trigger '{0}' matches log line '{1}'", t.LogName, le.Text));
                    }
                }
                if ((force & Action.TriggerForceTypeEnum.SkipActive) == 0)
                {
                    if (t.Enabled == false)
                    {
                        t.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trignotactive", "Trigger '{0}' is not active for firing", t.LogName));
                        return;
                    }
                }
                if ((force & Action.TriggerForceTypeEnum.SkipParent) == 0)
                {
                    Folder.FilterFailReason reason = t.Parent.PassesFilter(le);
                    if (reason != Folder.FilterFailReason.Passed)
                    {
                        if (reason != Folder.FilterFailReason.NotEnabled)
                        {
                            t.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigparentfail", "Trigger '{0}' doesn't pass parent folder '{1}' filter(s): {2}", t.LogName, t.Parent.Name, reason.ToString()));
                        }
                        return;
                    }
                }
                if ((force & Action.TriggerForceTypeEnum.SkipRefire) == 0)
                {
                    if (t.PeriodRefire == Trigger.RefireEnum.Deny && DateTime.Now < t.RefireDelayedUntil)
                    {
                        t.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigrefirefail", "Trigger '{0}' refire delayed until {1}", t.LogName, FormatDateTime(t.RefireDelayedUntil)));
                        return;
                    }
                }
                t.AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/trigfiring", "Firing trigger '{0}'", t.LogName));
                Context ctx = new Context();
                ctx.plug = this;
                ctx.trig = t;
                ctx.soundhook = SoundPlaybackSmart;
                ctx.ttshook = TtsPlaybackSmart;
                if ((force & Action.TriggerForceTypeEnum.SkipRegexp) == 0)
                {
                    foreach (int idx in t.regexCache.GetGroupNumbers())
                    {
                        ctx.numgroups.Add(m.Groups[idx].Value);
                        t.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/debugnumgroup", "Trigger '{0}' numbered group {1}: {2}", t.LogName, idx, m.Groups[idx].Value));
                    }
                    foreach (string sdx in t.regexCache.GetGroupNames())
                    {
                        ctx.namedgroups[sdx] = m.Groups[sdx].Value;
                        t.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/debugnamedgroup", "Trigger '{0}' named group '{1}': {2}", t.LogName, sdx, m.Groups[sdx].Value));
                    }
                }
                ctx.namedgroups["_zone"] = le.ZoneName;
                ctx.namedgroups["_event"] = le.Text;
                if (le.TestMode == true && le.ZoneId != "")
                {
                    ctx.zoneIdOverride = le.ZoneId;
                }
                ctx.triggered = DateTime.UtcNow;
                ctx.namedgroups["_timestamp"] = "" + (long)(ctx.triggered - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
                ctx.namedgroups["_timestampms"] = "" + (long)(ctx.triggered - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
                ctx.force = force;
                t.Fire(this, ctx, null);
            }
        }

    }
}
