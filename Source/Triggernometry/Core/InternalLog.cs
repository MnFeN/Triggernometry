using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Localization;

namespace Triggernometry.Core
{

    public sealed class InternalLog
    {

        public DateTime Timestamp { get; set; }
        public RealPlugin.DebugLevelEnum Level { get; set; }
        public string Message { get; set; }
        public Trigger SourceTrigger { get; set; }
        public ActionOld SourceAction { get; set; }
        internal static ActionOld RecordedAction { get; set; }

        public override string ToString()
        {
            return RealPlugin.FormatDateTime(Timestamp) + " - " + I18n.Translate($"LogForm/chk{Level}", $"{Level}") + " - " + Message;
        }

    }

}
