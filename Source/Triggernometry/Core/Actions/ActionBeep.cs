using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Common.Audio;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Beep
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Audio)]
    [XmlRoot(ElementName = "Beep")]
    internal class ActionBeep : ActionBase
    {

        #region Properties

        /// <summary>
        /// Frequency of the beep
        /// </summary>
        [XmlIgnore]
        [Action(order: 1, typehint: typeof(float))]
        public string Frequency { get; set; } = "1046.5"; // freq(C6)

        [XmlAttribute("Frequency")]
        public string Xml_Frequency
        {
            get => XmlAttr.String(Frequency, "1046.5");
            set => Frequency = value;
        }

        /// <summary>
        /// Duration of the beep
        /// </summary>
        [XmlIgnore]
        [Action(order: 2, typehint: typeof(int))]
        public string Duration { get; set; } = "100";

        [XmlAttribute("Duration")]
        public string Xml_Duration
        {
            get => XmlAttr.String(Duration, "100");
            set => Duration = value;
        }


        #endregion

        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descbeep", "Beep at ({0}) hz for ({1}) ms", Frequency, Duration);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;
            bool useSimulatedBeep = plug.cfg.UseSimulatedBeep;

            double beepLength = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Duration);
            if (beepLength < 0.0)
            {
                beepLength = 0.0;
                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/beeplengthlo", "Beep length below limit, capping to {0}", beepLength));
            }

            double frequency = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Frequency);
            if (!useSimulatedBeep)
            {
                if (frequency < 37.0)
                {
                    frequency = 37.0;
                    AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/beepfreqlo", "Beep frequency below limit, capping to {0}", frequency));
                }
                if (frequency > 32767.0)
                {
                    frequency = 32767.0;
                    AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/beepfreqhi", "Beep frequency above limit, capping to {0} ", frequency));
                }
            }

            double volume = plug.cfg.SimulatedBeepVolume / 100.0;
            Beep(frequency, beepLength, volume, useSimulatedBeep);
        }

        public static void Beep(double frequency, double beepLength, double volume, bool useSimulatedBeep)
        {
            if (useSimulatedBeep)
            {
                WaveGenerator.PlaySyncBeep((int)Math.Ceiling(frequency), (int)Math.Ceiling(beepLength), volume);
            }
            else // Console.Beep
            {
                Console.Beep((int)Math.Ceiling(frequency), (int)Math.Ceiling(beepLength));
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionBeep(ActionOld oldAction)
        {
            var action = new ActionBeep();
            oldAction.CopyCommonPropertiesTo(action);
            action.Frequency = oldAction._SystemBeepFreqExpression;
            action.Duration = oldAction._SystemBeepLengthExpression;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionBeep action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.SystemBeep;
            oldAction._SystemBeepFreqExpression = action.Frequency;
            oldAction._SystemBeepLengthExpression = action.Duration;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
