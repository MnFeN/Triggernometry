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
    [XmlRoot(ElementName = "ActionLoop")]
    public class ActionLoop : ActionBase
    {

        #region Properties

        // todo probably needs a custom property editor

        private bool ShouldSerializeLoopCondition()
        {
            return LoopCondition.Children.Count > 0;
        }

        /// <summary>
        /// Condition that is checked before every iterator whether loop should continue or not
        /// </summary>
        public ConditionGroup LoopCondition = new ConditionGroup();

        /// <summary>
        /// Actions within the loop
        /// </summary>
        [XmlIgnore]
        public List<ActionBase> LoopActions = new List<ActionBase>();

        [XmlElement("LoopActions")]
        public ActionXmlCollection Xml_LoopActions
        {
            get => new ActionXmlCollection(LoopActions);
            set => LoopActions = value.Actions;
        }

        public bool ShouldSerializeXml_LoopActions() => LoopActions.Count > 0;

        /// <summary>
        /// Delay between every loop iteration
        /// </summary>        
        [XmlIgnore]
        [Action(order: 1, typehint: typeof(float))]
        public string DelayExpression { get; set; } = "";

        [XmlAttribute("DelayExpression")]
        public string Xml_DelayExpression
        {
            get => XmlAttr.String(DelayExpression, "", omitWhitespace: false);
            set => DelayExpression = value;
        }

        /// <summary>
        /// Expression for the initial value of the loop iterator
        /// </summary>
        [XmlIgnore]
        [Action(order: 2, typehint: typeof(int))]
        public string InitExpression { get; set; } = "0";

        [XmlAttribute("InitExpression")]
        public string Xml_InitExpression
        {
            get => XmlAttr.String(InitExpression, "0", omitWhitespace: false);
            set => InitExpression = value;
        }

        /// <summary>
        /// Expression for the addition to the loop iterator after every iteration
        /// </summary>
        [XmlIgnore]
        [Action(order: 3, typehint: typeof(int))]
        public string IncrExpression { get; set; } = "1";

        [XmlAttribute("IncrExpression")]
        public string Xml_IncrExpression
        {
            get => XmlAttr.String(IncrExpression, "1", omitWhitespace: false);
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
            Context ctx = ai.ctx;
            QueueIteration(ref ctx, ai);
        }

        /// <summary>Queues a loop iteration and returns whether the outer continuation must wait.</summary>
        internal bool QueueIteration(ref Context ctx, ActionInstance qa)
        {
            var loopActionIdOld = ctx.loopActionId;

            if (loopActionIdOld == Id)
                ctx.loopIterator += (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, IncrExpression);

            if (!LoopCondition.Enabled)
                return false;

            bool continuing = false;
            Context loopContext = ctx;
            if (loopActionIdOld != Id)
            {
                continuing = loopActionIdOld == Guid.Empty;
                loopContext = ctx.Duplicate();
                if (loopActionIdOld != Guid.Empty && loopActionIdOld != Id)
                {
                    loopContext.id = Guid.NewGuid();
                }
                loopContext.loopActionId = Id;
                loopContext.loopIterator = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, InitExpression);
            }
            else
            {
                continuing = true;
            }
            // Test the initial value before entering, without replacing the caller's context on failure.
            if (LoopCondition.CheckCondition(loopContext, ActionContextLogger, loopContext) != true)
                return false;
            ctx = loopContext;

            DateTime curTime = DateTime.Now;
            ActionBase lastAction = ctx.Plugin.QueueActions(ctx, curTime, LoopActions, ctx.Trigger.Sequential, qa?.mutex, ActionContextLogger);
            lastAction.LoopAction = this;

            return continuing;
        }

        /// <summary>Copies loop conditions and child actions without sharing editable objects.</summary>
        internal override void CopySpecificPropertiesTo(ActionBase clone)
        {
            base.CopySpecificPropertiesTo(clone);
            var loop = (ActionLoop)clone;
            loop.LoopCondition = (ConditionGroup)LoopCondition?.Duplicate();
            loop.LoopActions = LoopActions.Select(action => action.Copy()).ToList();
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
            action.LoopActions = oldAction.LoopActions?.Select(x => x.ConvertToNewAction()).ToList() ?? new List<ActionBase>();
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
            oldAction.LoopActions = action.LoopActions?.Select(x => x.ConvertToOldAction()).ToList() ?? new List<ActionOld>();
            return oldAction;
        }

        #endregion Old Action Converter

    }

}
