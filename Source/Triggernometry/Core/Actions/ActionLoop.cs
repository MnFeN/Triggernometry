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
        public string LoopDelay { get; set; } = "";

        [XmlAttribute("LoopDelay")]
        public string Xml_LoopDelay
        {
            get => XmlAttr.String(LoopDelay, "", omitWhitespace: false);
            set => LoopDelay = value;
        }

        /// <summary>
        /// Expression for the initial value of the loop iterator
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, EditorExpressionType.Numeric, Label = "Initial iterator")]
        public string Start { get; set; } = "0";

        [XmlAttribute("Start")]
        public string Xml_Start
        {
            get => XmlAttr.String(Start, "0", omitWhitespace: false);
            set => Start = value;
        }

        /// <summary>
        /// Expression for the addition to the loop iterator after every iteration
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, EditorExpressionType.Numeric, Label = "Increment")]
        public string Step { get; set; } = "1";

        [XmlAttribute("Step")]
        public string Xml_Step
        {
            get => XmlAttr.String(Step, "1", omitWhitespace: false);
            set => Step = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate(
                "internal/Action/descloop", "Loop with {0} actions at ({1}) ms intervals",
                LoopActions?.Count(action => action.Enabled) ?? 0,
                string.IsNullOrWhiteSpace(LoopDelay) ? "0" : LoopDelay
            );
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            // 直接调用只负责启动循环；后续迭代和子动作仍由队列推进。
            var iteration = new ActionInstance(DateTime.Now, 0, null, this, ctx, false);
            QueueIteration(iteration);
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
                loopContext.loopIterator = (int)ai.Context.EvaluateNumericExpression(ActionContextLogger, ai.Context, Start);
            }
            else
            {
                loopContext.loopIterator += (int)loopContext.EvaluateNumericExpression(ActionContextLogger, loopContext, Step);
            }
            // A failed initial test must not replace the caller's context.
            if (!LoopCondition.Check(loopContext, ActionContextLogger, loopContext)) return false;
            ai.LoopContext = loopContext;

            return loopContext.Plugin.QueueActions(loopContext, DateTime.Now, LoopActions,
                loopContext.Trigger?.Sequential ?? true, ai.Mutex, ActionContextLogger, ai) != null;
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
            action.LoopDelay = oldAction._LoopDelayExpression;
            action.Start = oldAction._LoopInitExpression;
            action.Step = oldAction._LoopIncrExpression;
            action.LoopCondition = (ConditionGroup)oldAction.LoopCondition?.Duplicate() ?? new ConditionGroup();

            action.LoopActions = oldAction.LoopActions?.Select(child => child.Copy()).ToList() ?? new List<ActionBase>();
            return action;
        }

        #endregion Old Action Converter

    }

}
