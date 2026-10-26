using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Triggernometry.Expressions.String.Utils
{
    /// <summary>
    /// Stores zero-based slice ranges for repeatable enumeration without expanding individual indices.
    /// Range order and duplicate indices are preserved.
    /// </summary>
    internal struct SliceIndices : IEnumerable<int>
    {
        // Store the first range inline; allocate a list only for additional ranges.
        private Range first;
        private List<Range> remaining;
        private bool hasFirst;

        /// <summary>
        /// An inclusive start, exclusive end, and nonzero step in either direction.
        /// </summary>
        private readonly struct Range
        {
            internal readonly int Start;
            internal readonly int End;
            internal readonly int Step;

            internal Range(int start, int end, int step)
            {
                Start = start;
                End = end;
                Step = step;
            }
        }

        /// <summary>
        /// Appends a normalized range, ignoring ranges that are empty in the specified direction.
        /// </summary>
        internal void AddRange(int start, int end, int step)
        {
            int sign = Math.Sign(step);
            if (!(sign * start < sign * end)) return;

            var range = new Range(start, end, step);
            if (!hasFirst)
            {
                first = range;
                hasFirst = true;
            }
            else
            {
                if (remaining == null) remaining = new List<Range>();
                remaining.Add(range);
            }
        }

        /// <summary>
        /// Returns a repeatable sequence, using a framework iterator for empty or contiguous selections.
        /// Complete the selection before enumerating it.
        /// </summary>
        internal IEnumerable<int> AsEnumerable()
        {
            if (!hasFirst) return Enumerable.Empty<int>();
            if (remaining == null && first.Step == 1)
                return Enumerable.Range(first.Start, first.End - first.Start);
            return this;
        }

        /// <summary>
        /// Creates an independent cursor over the stored ranges.
        /// </summary>
        public Enumerator GetEnumerator() => new Enumerator(this);
        IEnumerator<int> IEnumerable<int>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Enumerates parsed ranges in their original order, retaining duplicate indices.
        /// </summary>
        public struct Enumerator : IEnumerator<int>
        {
            private readonly List<Range> remaining;
            private Range range;
            private int rangeIndex; // Next range in the additional-range list.
            private int next;

            internal Enumerator(SliceIndices slices)
            {
                remaining = slices.remaining;
                range = slices.hasFirst ? slices.first : default;
                rangeIndex = 0;
                next = range.Start;
                Current = 0;
            }

            public int Current { get; private set; }
            object IEnumerator.Current => Current;

            /// <summary>
            /// Advances within the current range, then moves to the next range when exhausted.
            /// </summary>
            public bool MoveNext()
            {
                // A zero step marks an empty or exhausted cursor.
                while (range.Step != 0)
                {
                    bool inRange = range.Step > 0
                        ? next >= range.Start && next < range.End
                        : unchecked(-next) >= unchecked(-range.Start) && unchecked(-next) < unchecked(-range.End);
                    if (inRange)
                    {
                        Current = next;
                        next = unchecked(next + range.Step);
                        return true;
                    }
                    if (remaining == null || rangeIndex >= remaining.Count)
                    {
                        range = default;
                        break;
                    }
                    range = remaining[rangeIndex++];
                    next = range.Start;
                }
                return false;
            }

            public void Reset() => throw new NotSupportedException();
            public void Dispose() { }
        }
    }
}
