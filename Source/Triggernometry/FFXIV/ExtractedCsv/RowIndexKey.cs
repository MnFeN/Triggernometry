using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    /// <summary>
    /// Represents a row identifier composed of a primary index and an optional sub-index. <br />
    /// Some data sheets use row keys in the form of "Main.Sub" (e.g., "1.0", "5.7", "65599.255") instead of a single integer.
    /// </summary>
    public readonly struct RowIndexKey : IEquatable<RowIndexKey>
    {
        public readonly int Main;
        public readonly int Sub;

        public RowIndexKey(int main, int sub)
        {
            Main = main;
            Sub = sub;
        }

        public static RowIndexKey Parse(string s)
        {
            int dot = s.IndexOf('.');
            if (dot < 0)
                return new RowIndexKey(int.Parse(s), 0);

            return new RowIndexKey(
                int.Parse(s.Substring(0, dot)),
                int.Parse(s.Substring(dot + 1))
            );
        }

        public bool Equals(RowIndexKey other)
            => Main == other.Main && Sub == other.Sub;

        public override int GetHashCode()
            => (Main * 397) ^ Sub;

        public override string ToString()
            => Sub == 0 ? Main.ToString() : Main + "." + Sub;

        public static explicit operator int(RowIndexKey key)
        {
            return key.Main;
        }

        public static implicit operator RowIndexKey(int main)
        {
            return new RowIndexKey(main, 0);
        }

    }
}
