using System;
using System.Globalization;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    /// <summary>
    /// Represents a row identifier composed of a primary index and an optional sub-index. <br />
    /// If Sub = null, it means no sub-index (e.g., "1"). <br />
    /// If Sub = 0, it's explicitly "1.0". These are NOT equal.
    /// </summary>
    public readonly struct RowIndexKey : IEquatable<RowIndexKey>
    {
        public readonly int Main;
        public readonly int? Sub;

        public RowIndexKey(int main, int? sub = null)
        {
            Main = main;
            Sub = sub;
        }

        public static RowIndexKey Parse(string s)
        {
            int dot = s.IndexOf('.');

            if (dot < 0)
            {
                return new RowIndexKey(int.Parse(s));
            }

            return new RowIndexKey(
                int.Parse(s.Substring(0, dot)),
                int.Parse(s.Substring(dot + 1))
            );
        }

        public bool Equals(RowIndexKey other)
            => Main == other.Main && Sub == other.Sub;

        public override int GetHashCode()
        {
            unchecked
            {
                return (Main * 397) ^ (Sub ?? 0x7FFFFFFF);
            }
        }

        public override string ToString()
        {
            if (Sub == null)
            {
                return Main.ToString(CultureInfo.InvariantCulture);
            }
            else
            { 
                return Main.ToString(CultureInfo.InvariantCulture) +  "." + Sub.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static explicit operator int(RowIndexKey key)
        {
            return key.Main;
        }

        public static implicit operator RowIndexKey(int main)
        {
            return new RowIndexKey(main);
        }

        public static implicit operator RowIndexKey((int main, int? sub) pair)
        {
            return new RowIndexKey(pair.main, pair.sub);
        }
    }
}
