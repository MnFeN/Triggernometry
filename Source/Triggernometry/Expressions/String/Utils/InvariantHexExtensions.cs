using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Triggernometry.Expressions.String.Utils
{
    public static class InvarianHexExtensions
    {
    
        /// <summary> Convert a hexadecimal <see cref="string"/> to an <see cref="uint"/>. </summary>
        /// <remarks> Uses <see cref="NumberStyles.HexNumber"/> and <see cref="CultureInfo.InvariantCulture"/>.</remarks>
        public static uint ParseHexUInt(this string s)
            => uint.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        /// <summary> Attempt to convert a hexadecimal <see cref="string"/> to an <see cref="uint"/>. </summary>
        /// <remarks> Uses <see cref="NumberStyles.HexNumber"/> and <see cref="CultureInfo.InvariantCulture"/>.</remarks>
        public static bool TryParseHexUInt(this string s, out uint result)
            => uint.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);


        /// <summary> Convert a hexadecimal <see cref="string"/> to an <see cref="uint"/>. </summary>
        /// <remarks> Uses <see cref="NumberStyles.HexNumber"/> and <see cref="CultureInfo.InvariantCulture"/>. Allow leading "0x".</remarks>
        public static int ParseHexInt(this string s)
            => int.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        /// <summary> Attempt to convert a hexadecimal <see cref="string"/> to an <see cref="int"/>. </summary>
        /// <remarks> Uses <see cref="NumberStyles.HexNumber"/> and <see cref="CultureInfo.InvariantCulture"/>. Allow leading "0x".</remarks>
        public static bool TryParseHexInt(this string s, out int result)
            => int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
      

    }
}
