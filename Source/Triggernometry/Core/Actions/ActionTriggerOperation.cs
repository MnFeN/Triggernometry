using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Trigger operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "TriggerOperation")]
    public class ActionTriggerOperation : ActionBase
    {

        #region Properties

        /// <summary>
        /// Trigger operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Fire trigger
            /// </summary>
            FireTrigger,
            /// <summary>
            /// Cancel all queued actions from trigger
            /// </summary>
            CancelTrigger,
            /// <summary>
            /// Enable trigger
            /// </summary>
            EnableTrigger,
            /// <summary>
            /// Disable trigger
            /// </summary>
            DisableTrigger,
            /// <summary>
            /// Cancel all queued actions from all triggers
            /// </summary>
            CancelAllTrigger
        }

        /// <summary>
        /// Trigger force firing flags
        /// </summary>
        [Flags]
        public enum ForceEnum
        {
            /// <summary>
            /// Don't skip anything, all restrictions in effect
            /// </summary>
            NoSkip = 0,
            /// <summary>
            /// Skip checking regexp
            /// </summary>
            SkipRegexp = 1,
            /// <summary>
            /// Skip checking conditions
            /// </summary>
            SkipConditions = 2,
            /// <summary>
            /// Skip checking refire restrictions
            /// </summary>
            SkipRefire = 4,
            /// <summary>
            /// Skip any checks from parent folder(s)
            /// </summary>
            SkipParent = 8,
            /// <summary>
            /// Skip checking whether trigger is active or not
            /// </summary>
            SkipActive = 16,
            /// <summary>
            /// Skip all checks except condition checks
            /// </summary>
            SkipExceptConditions = SkipRegexp | SkipRefire | SkipParent | SkipActive,
            /// <summary>
            /// Skip all checks
            /// </summary>
            SkipAll = SkipRegexp | SkipConditions | SkipRefire | SkipParent | SkipActive
        }

        /// <summary>
        /// Type of the zone information
        /// </summary>
        public enum ZoneTypeEnum
        {
            /// <summary>
            /// Zone information is a zone name
            /// </summary>
            ZoneName,
            /// <summary>
            /// Zone information is a FFXIV zone ID
            /// </summary>
            ZoneIdFFXIV
        }

        /// <summary>
        /// Type of the zone information provided
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public ZoneTypeEnum ZoneType { get; set; } = ZoneTypeEnum.ZoneName;

        [XmlAttribute("ZoneType")]
        public string Xml_ZoneType
        {
            get => XmlAttr.Enum(ZoneType, ZoneTypeEnum.ZoneName);
            set => ZoneType = XmlAttr.Enum<ZoneTypeEnum>(value);
        }

        /// <summary>
        /// Type of the trigger operation
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public OperationEnum Operation { get; set; } = OperationEnum.FireTrigger;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.FireTrigger);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Reference to the trigger
        /// </summary>
        [XmlIgnore]
        [Action(order: 3, specialtype: ActionAttribute.SpecialTypeEnum.TriggerReference)]
        public Guid TriggerId { get; set; } = Guid.Empty;

        [XmlAttribute("TriggerId")]
        public string Xml_TriggerId
        {
            get => XmlAttr.Guid(TriggerId, Guid.Empty);
            set => TriggerId = XmlAttr.Guid(value);
        }

        /// <summary>
        /// Event text the trigger is fired with
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string Text { get; set; } = "";

        [XmlAttribute("Text")]
        public string Xml_Text
        {
            get => XmlAttr.String(Text);
            set => Text = value;
        }

        /// <summary>
        /// Zone information the trigger is fired with
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public string Zone { get; set; } = "";

        [XmlAttribute("Zone")]
        public string Xml_Zone
        {
            get => XmlAttr.String(Zone);
            set => Zone = value;
        }

        /// <summary>
        /// Action tag (to interrupt actions)
        /// </summary>
        [XmlIgnore]
        [Action(order: 6)]
        public string TagRegex { get; set; } = "";

        [XmlAttribute("TagRegex")]
        public string Xml_TagRegex
        {
            get => XmlAttr.String(TagRegex);
            set => TagRegex = value;
        }

        /// <summary>
        /// Trigger force firing flags
        /// </summary>
        [XmlIgnore]
        [Action(order: 7)]
        public ForceEnum Force { get; set; } = ForceEnum.NoSkip;

        [XmlAttribute("Force")]
        public string Xml_Force
        {
            get
            {
                List<string> ex = new List<string>();
                if (Force == ForceEnum.SkipAll)
                {
                    ex.Add("true");
                }
                else
                {
                    if ((Force & ForceEnum.SkipRegexp) != 0)
                    {
                        ex.Add("regexp");
                    }
                    if ((Force & ForceEnum.SkipConditions) != 0)
                    {
                        ex.Add("conditions");
                    }
                    if ((Force & ForceEnum.SkipRefire) != 0)
                    {
                        ex.Add("refire");
                    }
                    if ((Force & ForceEnum.SkipParent) != 0)
                    {
                        ex.Add("parent");
                    }
                    if ((Force & ForceEnum.SkipActive) != 0)
                    {
                        ex.Add("active");
                    }
                }
                string temp = string.Join(",", ex.ToArray());
                return temp.Length > 0 ? temp : null;
            }
            set
            {
                string[] exx = value != null ? value.Split(",".ToCharArray()) : new string[] { "" };
                ForceEnum newval = ForceEnum.NoSkip;
                foreach (string ex in exx)
                {
                    if (string.Compare(ex, "true", true) == 0)
                    {
                        newval = ForceEnum.SkipAll;
                        break;
                    }
                    else if (string.Compare(ex, "false", true) == 0)
                    {
                        newval = ForceEnum.NoSkip;
                        break;
                    }
                    if (string.Compare(ex, "regexp", true) == 0)
                    {
                        newval |= ForceEnum.SkipRegexp;
                    }
                    if (string.Compare(ex, "conditions", true) == 0)
                    {
                        newval |= ForceEnum.SkipConditions;
                    }
                    if (string.Compare(ex, "refire", true) == 0)
                    {
                        newval |= ForceEnum.SkipRefire;
                    }
                    if (string.Compare(ex, "parent", true) == 0)
                    {
                        newval |= ForceEnum.SkipParent;
                    }
                    if (string.Compare(ex, "active", true) == 0)
                    {
                        newval |= ForceEnum.SkipActive;
                    }
                }
                Force = newval;
            }
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            Trigger t = ctx.Plugin.GetTriggerById(TriggerId, ctx.Trigger?.Repo);
            if (t == null && Operation != OperationEnum.CancelAllTrigger)
            {
                return I18n.Translate("internal/Action/desctriginvalidref", "trigger action with an invalid trigger reference ({0})", TriggerId);
            }
            switch (Operation)
            {
                case OperationEnum.CancelTrigger:
                    if (string.IsNullOrWhiteSpace(TagRegex))
                    {
                        return I18n.Translate("internal/Action/desctrigcanceltrig",
                            "cancel all actions queued from trigger ({0})",
                            t?.Name ?? "null");
                    }
                    else
                    {
                        return I18n.Translate("internal/Action/desctrigcanceltrigtag",
                            "cancel all actions queued from trigger ({0}) with tags matching regex ({1})",
                            t?.Name ?? "null", TagRegex);
                    }
                case OperationEnum.CancelAllTrigger:
                    if (string.IsNullOrWhiteSpace(TagRegex))
                    {
                        return I18n.Translate("internal/Action/desctrigcancelall",
                            "cancel all actions queued from all triggers");
                    }
                    else
                    {
                        return I18n.Translate("internal/Action/desctrigcanceltag",
                            "cancel all actions queued from all triggers with tags matching regex ({0})",
                            TagRegex);
                    }
                case OperationEnum.FireTrigger:
                    string temp = I18n.Translate("internal/Action/desctrigfire", "fire trigger ({0})", t?.Name ?? "null");
                    List<string> ex = new List<string>();
                    if (Force == ForceEnum.SkipAll)
                    {
                        ex.Add(I18n.Translate("internal/Action/desctrigignoreall", "all restrictions"));
                    }
                    else
                    {
                        if ((Force & ForceEnum.SkipRegexp) != 0)
                        {
                            ex.Add(I18n.Translate("internal/Action/desctrigignoreregex", "regular expression"));
                        }
                        else
                        {
                            temp += " " + I18n.Translate("internal/Action/desctrigfireusing", "with event text ({0}) and zone ({1})", Text, Zone);
                        }
                        if ((Force & ForceEnum.SkipConditions) != 0)
                        {
                            ex.Add(I18n.Translate("internal/Action/desctrigignoreconditions", "conditions"));
                        }
                        if ((Force & ForceEnum.SkipRefire) != 0)
                        {
                            ex.Add(I18n.Translate("internal/Action/desctrigignorerefire", "refire delay"));
                        }
                        if ((Force & ForceEnum.SkipParent) != 0)
                        {
                            ex.Add(I18n.Translate("internal/Action/desctrigignoreparent", "parent folder settings"));
                        }
                        if ((Force & ForceEnum.SkipActive) != 0)
                        {
                            ex.Add(I18n.Translate("internal/Action/desctrigignorestate", "enabled/disabled status"));
                        }
                    }
                    if (ex.Count > 1)
                    {
                        ex[ex.Count - 1] = I18n.Translate("internal/Action/desctrigignoreand", "and") + " " + ex[ex.Count - 1];
                    }
                    if (ex.Count > 0)
                    {
                        temp += ", " + I18n.Translate("internal/Action/desctrigignoring", "ignoring") + " " + string.Join(", ", ex);
                    }
                    return temp;
                case OperationEnum.DisableTrigger:
                    return I18n.Translate("internal/Action/desctrigdisable", "disable trigger ({0})", t?.Name ?? "null");
                case OperationEnum.EnableTrigger:
                    return I18n.Translate("internal/Action/desctrigenable", "enable trigger ({0})", t?.Name ?? "null");
                default:
                    throw new NotImplementedException(Operation.ToString());
            }
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            Trigger t = ctx.Plugin.GetTriggerById(TriggerId, ctx.Trigger?.Repo);
            if (t == null && Operation != OperationEnum.CancelAllTrigger)
            {
                AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/notriggerwithid",
                    "Trigger operation failed: In trigger ({1}), the specified trigger id ({0}) does not exist.", TriggerId, ParentTrigger?.FullPath ?? "null"));
                return;
            }
            switch (Operation)
            {
                case OperationEnum.CancelAllTrigger:
                    {
                        // Specified Tag Regex
                        if (!string.IsNullOrWhiteSpace(TagRegex))
                        {
                            var tag = ctx.EvaluateStringExpression(ActionContextLogger, null, TagRegex);
                            var regex = new Regex(tag);
                            var removedCount = Instance.CancelQueuedActions(
                                _qa => regex.IsMatch(_qa?.ParsedTag ?? "")
                            );
                            Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                "internal/Action/trigcanceltag",
                                "{0} queued action(s) with tags matching '{1}' cancelled",
                                removedCount, tag));
                        }
                        else
                        {
                            var removedCount = Instance.CancelQueuedActions();
                            Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                "internal/Action/trigcancelall",
                                "All {0} queued action(s) cancelled",
                                removedCount));
                        }
                    }
                    break;
                case OperationEnum.CancelTrigger:
                    {
                        bool trigFilter(QueuedAction _qa) => _qa?.ctx?.Trigger == t;

                        if (!string.IsNullOrWhiteSpace(TagRegex))
                        {
                            var tag = ctx.EvaluateStringExpression(ActionContextLogger, null, TagRegex);
                            var regex = new Regex(tag);
                            var removedCount = Instance.CancelQueuedActions(
                                _qa => trigFilter(_qa) && regex.IsMatch(_qa?.ParsedTag ?? "")
                            );
                            Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                "internal/Action/trigcanceltrigtag",
                                "{0} queued action(s) from trigger '{1}' with tags matching '{2}' cancelled",
                                removedCount, t.LogName, tag));
                        }
                        else
                        {
                            var removedCount = Instance.CancelQueuedActions(queuedAction => trigFilter(queuedAction));
                            Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                "internal/Action/trigcanceltrig",
                                "{0} queued action(s) from trigger '{1}' cancelled",
                                removedCount, t.LogName));
                        }
                    }
                    break;
                case OperationEnum.FireTrigger:
                    {
                        LogEvent le = new LogEvent();
                        le.Text = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Text);
                        le.ZoneName = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Zone);
                        if (ZoneType == ZoneTypeEnum.ZoneIdFFXIV && le.ZoneName.Trim().Length > 0)
                        {
                            le.ZoneId = le.ZoneName;
                        }
                        le.Timestamp = DateTime.Now;
                        if (ctx.zoneIdOverride != null)
                        {
                            le.TestMode = true;
                            le.ZoneId = ctx.zoneIdOverride;
                        }
                        //ctx.plug.TestTrigger(t, le, _Force); todo
                    }
                    break;
                case OperationEnum.EnableTrigger:
                    {
                        t.Enabled = true;
                        ctx.Plugin.ui.Invoke((System.Action)(() =>
                        {
                            bool isLocal = ctx.Trigger == null || ctx.Trigger.Repo == null;
                            TreeNode tn = ctx.Plugin.LocateNodeHostingTrigger(ctx.Plugin.ui.treeView1.Nodes[isLocal ? 0 : 1], t);

                            if (tn != null)
                            {
                                AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/trigenable", "Trigger '{0}' enabled", t.LogName));
                                tn.Checked = true;
                            }
                            else
                            {
                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodetrigenable", "Could not find tree node to modify for enabling trigger {0}", t.LogName));
                            }
                        }));
                    }
                    break;
                case OperationEnum.DisableTrigger:
                    {
                        t.Enabled = false;
                        ctx.Plugin.ui.Invoke((System.Action)(() =>
                        {
                            bool isLocal = ctx.Trigger == null || ctx.Trigger.Repo == null;
                            TreeNode tn = ctx.Plugin.LocateNodeHostingTrigger(ctx.Plugin.ui.treeView1.Nodes[isLocal ? 0 : 1], t);

                            if (tn != null)
                            {
                                AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/trigdisable", "Trigger '{0}' disabled", t.LogName));
                                tn.Checked = false;
                            }
                            else
                            {
                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodetrigdisable", "Could not find tree node to modify for disabling trigger {0}", t.LogName));
                            }
                        }));
                    }
                    break;
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionTriggerOperation(ActionOld oldAction)
        {
            var action = new ActionTriggerOperation();
            oldAction.CopyCommonPropertiesTo(action);
            action.ZoneType = (ZoneTypeEnum)(int)oldAction._TriggerZoneType;
            action.Operation = (OperationEnum)(int)oldAction._TriggerOp;
            action.TriggerId = oldAction._TriggerId;
            action.Text = oldAction._TriggerText;
            action.Zone = oldAction._TriggerZone;
            action.TagRegex = oldAction._TriggerTagRegex;
            action.Force = (ForceEnum)(int)oldAction._TriggerForceType;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionTriggerOperation action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Trigger;
            oldAction._TriggerZoneType = (ActionOld.TriggerZoneTypeEnum)(int)action.ZoneType;
            oldAction._TriggerOp = (ActionOld.TriggerOpEnum)(int)action.Operation;
            oldAction._TriggerId = action.TriggerId;
            oldAction._TriggerText = action.Text;
            oldAction._TriggerZone = action.Zone;
            oldAction._TriggerTagRegex = action.TagRegex;
            oldAction._TriggerForceType = (ActionOld.TriggerForceTypeEnum)(int)action.Force;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
