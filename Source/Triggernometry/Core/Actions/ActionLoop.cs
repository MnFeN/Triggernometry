using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Loop
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Programming)]
    [XmlRoot(ElementName = "Loop")]
    internal class ActionLoop : ActionBase
    {

        #region Properties

        // todo probably needs a custom property editor

        private bool ShouldSerializeLoopCondition()
        {
            return LoopCondition.Children.Count > 0;
        }

        private bool ShouldSerializeLoopActions()
        {
            return LoopActions.Count > 0;
        }

        /// <summary>
        /// Condition that is checked before every iterator whether loop should continue or not
        /// </summary>
        public ConditionGroup LoopCondition = new ConditionGroup();

        /// <summary>
        /// Actions within the loop
        /// </summary>
        public List<ActionBase> LoopActions = new List<ActionBase>();

        /// <summary>
        /// Delay between every loop iteration
        /// </summary>        
        [XmlIgnore]
        public string DelayExpression { get; set; } = "";

        [XmlAttribute("DelayExpression")]
        public string Xml_DelayExpression
        {
            get => XmlAttr.String(DelayExpression);
            set => DelayExpression = value;
        }

        /// <summary>
        /// Expression for the initial value of the loop iterator
        /// </summary>
        [XmlIgnore]
        public string InitExpression { get; set; } = "0";

        [XmlAttribute("InitExpression")]
        public string Xml_InitExpression
        {
            get => XmlAttr.String(InitExpression, "0");
            set => InitExpression = value;
        }

        /// <summary>
        /// Expression for the addition to the loop iterator after every iteration
        /// </summary>
        [XmlIgnore]
        public string IncrExpression { get; set; } = "1";

        [XmlAttribute("IncrExpression")]
        public string Xml_IncrExpression
        {
            get => XmlAttr.String(IncrExpression, "1");
            set => IncrExpression = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate(
                "internal/Action/descloop", "Loop with {0} actions at ({1}) ms intervals",
                LoopActions?.Count(action => action.Enabled) ?? 0,
                string.IsNullOrWhiteSpace(DelayExpression) ? "0" : DelayExpression
            );
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            throw new NotImplementedException("Loop action should be handled specially for now.");
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            if (ctx.loopActionId == Id)
            {
                ctx.loopIterator += (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, IncrExpression);
            }
            if (LoopCondition.Enabled == true && LoopCondition.CheckCondition(ctx, ActionContextLogger, ctx) == true)
            {
                bool continuing = false;
                if (ctx.loopActionId != Id)
                {
                    continuing = ctx.loopActionId == Guid.Empty;
                    ctx = ctx.Duplicate();
                    if (ctx.loopActionId != Guid.Empty && ctx.loopActionId != Id)
                    {
                        ctx.id = Guid.NewGuid();
                    }
                    ctx.loopActionId = Id;
                    ctx.loopIterator = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, InitExpression);
                }
                else
                {
                    continuing = true;
                }
                DateTime curTime = DateTime.Now;
                ActionOld lastAction = plug.QueueActions(ctx, curTime, null /* todo Actions proper type */, ctx.Trigger.Sequential, ai?.mutex, ActionContextLogger);
                lastAction.LoopAction = null; // todo supposed to be a reference to this action
                if (continuing == true)
                {
                    return;
                }
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionLoop(ActionOld oldAction)
        {
            var action = new ActionLoop();
            oldAction.CopyCommonPropertiesTo(action);
            action.DelayExpression = oldAction._LoopDelayExpression;
            action.InitExpression = oldAction._LoopInitExpression;
            action.IncrExpression = oldAction._LoopIncrExpression;
            action.LoopCondition = (ConditionGroup)oldAction.LoopCondition?.Duplicate() ?? new ConditionGroup();
            // todo Actions proper type
            action.LoopActions = oldAction.LoopActions?.Select(x => (ActionBase)new ActionPlaceholder()).ToList() ?? new List<ActionBase>();
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionLoop action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Loop;
            oldAction._LoopDelayExpression = action.DelayExpression;
            oldAction._LoopInitExpression = action.InitExpression;
            oldAction._LoopIncrExpression = action.IncrExpression;
            oldAction.LoopCondition = (ConditionGroup)action.LoopCondition?.Duplicate() ?? new ConditionGroup();
            // todo 
            // oldAction.LoopActions = action.LoopActions?.Select(x => x.Copy()).ToList() ?? new List<ActionOld>();
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
