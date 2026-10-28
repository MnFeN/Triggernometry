using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Loop
    /// </summary>
    [Action("Loop", ActionAttribute.CategoryType.Programming)]
    [XmlRoot(ElementName = "ActionLoop")]
    public class ActionLoop : ActionBase
    {

        #region Properties

        private bool ShouldSerializeLoopCondition() => LoopCondition.Children.Count > 0;

        /// <summary>
        /// Condition that is checked before every iterator whether loop should continue or not
        /// </summary>
        [EditorField(4, Label = "Loop condition (must be true to iterate)")]
        public ConditionGroup LoopCondition = new ConditionGroup();

        /// <summary>
        /// Actions within the loop
        /// </summary>
        [XmlIgnore]
        [EditorField(5, Label = "Loop actions")]
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
        [ActionSpecificProperty]
        [ExpressionEditor(3, EditorExpressionType.Numeric, Label = "Loop iteration delay")]
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
        [ActionSpecificProperty]
        [ExpressionEditor(1, EditorExpressionType.Numeric, Label = "Initial iterator")]
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
        [ActionSpecificProperty]
        [ExpressionEditor(2, EditorExpressionType.Numeric, Label = "Increment")]
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

        internal override void ExecuteImplementation(Context ctx)
        {
            // To-do: Loop iterations are now handled by the base scheduling path.
        }

        /// <summary>Queues the next iteration using this invocation's iterator and return path.</summary>
        internal bool QueueIteration(ActionInstance ai)
        {
            if (!LoopCondition.Enabled) return false;

            Context loopContext = ai.LoopContext;
            if (loopContext == null)
            {
                loopContext = ai.Context.Duplicate();
                loopContext.loopActionId = Id;
                loopContext.loopIterator = (int)ai.Context.EvaluateNumericExpression(ActionContextLogger, ai.Context, InitExpression);
            }
            else
            {
                loopContext.loopIterator += (int)loopContext.EvaluateNumericExpression(ActionContextLogger, loopContext, IncrExpression);
            }
            // A failed initial test must not replace the caller's context.
            if (!LoopCondition.CheckCondition(loopContext, ActionContextLogger, loopContext)) return false;
            ai.LoopContext = loopContext;

            return loopContext.Plugin.QueueActions(loopContext, DateTime.Now, LoopActions,
                loopContext.Trigger.Sequential, ai.Mutex, ActionContextLogger, ai) != null;
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
            action.LoopActions = oldAction.LoopActions?.OrderBy(x => x.OrderNumber).Select(x => x.ConvertToNewAction()).ToList() ?? new List<ActionBase>();
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
