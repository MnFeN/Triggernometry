using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triggernometry
{

    public sealed class InternalLog
    {

        public DateTime Timestamp { get; set; }
        public RealPlugin.DebugLevelEnum Level { get; set; }
        public string Message { get; set; }
        public Trigger SourceTrigger { get; set; }
        public Action SourceAction { get; set; }
        internal static Action RecordedAction { get; set; }

        public override string ToString()
        {
            return RealPlugin.FormatDateTime(Timestamp) + " - " + I18n.Translate($"LogForm/chk{Level}", $"{Level}") + " - " + Message;
        }

    }

}
