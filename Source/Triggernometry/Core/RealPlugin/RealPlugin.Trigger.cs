using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Triggernometry.Localization;

namespace Triggernometry.Core
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

        internal void TestTrigger(Trigger trigger, LogEvent logEvent, ActionOld.TriggerForceTypeEnum forceType)
        {
            lock (trigger)
            {
                Match match = null;
                if ((forceType & ActionOld.TriggerForceTypeEnum.SkipRegexp) == 0)
                {
                    match = trigger.CheckMatch(logEvent.Text);
                    if (match == null)
                    {
                        return;
                    }
                    else
                    {
                        trigger.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigmatches", "Trigger '{0}' matches log line '{1}'", trigger.LogName, logEvent.Text));
                    }
                }
                if ((forceType & ActionOld.TriggerForceTypeEnum.SkipActive) == 0)
                {
                    if (trigger.Enabled == false)
                    {
                        trigger.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trignotactive", "Trigger '{0}' is not active for firing", trigger.LogName));
                        return;
                    }
                }
                if ((forceType & ActionOld.TriggerForceTypeEnum.SkipParent) == 0)
                {
                    Folder.FilterFailReason reason = trigger.Parent.PassesFilter(logEvent);
                    if (reason != Folder.FilterFailReason.Passed)
                    {
                        if (reason != Folder.FilterFailReason.NotEnabled)
                        {
                            trigger.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigparentfail", "Trigger '{0}' doesn't pass parent folder '{1}' filter(s): {2}", trigger.LogName, trigger.Parent.Name, reason.ToString()));
                        }
                        return;
                    }
                }
                if ((forceType & ActionOld.TriggerForceTypeEnum.SkipRefire) == 0)
                {
                    if (trigger.PeriodRefire == Trigger.RefireEnum.Deny && DateTime.Now < trigger.RefireDelayedUntil)
                    {
                        trigger.AddToLog(DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/trigrefirefail", "Trigger '{0}' refire delayed until {1}", trigger.LogName, FormatDateTime(trigger.RefireDelayedUntil)));
                        return;
                    }
                }
                trigger.AddToLog(DebugLevelEnum.Info, I18n.Translate("internal/Plugin/trigfiring", "Firing trigger '{0}'", trigger.LogName));
                Context ctx = new Context(trigger);
                ctx.soundhook = SoundPlaybackSmart;
                ctx.ttshook = TtsPlaybackSmart;

                if ((forceType & ActionOld.TriggerForceTypeEnum.SkipRegexp) == 0)
                {
                    ctx.RecordCaptureGroups(trigger, match);
                }

                ctx.zoneName = logEvent.ZoneName;
                ctx.triggeredText = logEvent.Text;
                if (logEvent.TestMode == true && logEvent.ZoneId != "")
                {
                    ctx.zoneIdOverride = logEvent.ZoneId;
                }
                ctx.force = forceType;
                trigger.Fire(this, ctx, null);
            }
        }

    }
}
