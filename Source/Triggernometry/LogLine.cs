using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triggernometry
{

    public sealed class LogEvent
    {

        public enum SourceEnum
        {
            /// <summary> Parsed ACT Log </summary>
            Log,
            /// <summary> Network Log </summary>
            NetworkFFXIV,
            /// <summary> ACT event </summary>
            ACT,
            /// <summary> Endpoint </summary>
            Endpoint
        }

        public string Text { get; set; }
        public string ZoneName { get; set; }
        public SourceEnum Source { get; set; }
        public DateTime Timestamp { get; set; }
        public bool TestMode { get; set; } = false;
        public string ZoneId { get; set; }

    }

}
