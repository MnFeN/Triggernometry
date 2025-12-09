using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triggernometry.FFXIV
{
    internal static class EorzeanTime
    {
        private const double EORZEA_MULTIPLIER = 3600.0 / 175;

        internal static TimeSpan Now => GetEorzeanTime(DateTime.UtcNow);

        internal static TimeSpan GetEorzeanTime(DateTime time)
        {
            long epochTicks = time.Ticks - (new DateTime(1970, 1, 1).Ticks);
            long eorzeaTicks = (long)Math.Round(epochTicks * EORZEA_MULTIPLIER);
            return new DateTime(eorzeaTicks).TimeOfDay;
        }

    }
}
