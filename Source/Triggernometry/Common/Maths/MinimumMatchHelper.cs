using Triggernometry.Localization;
using System;
using System.Numerics;

namespace Triggernometry.Common.Maths
{
    /// <summary>
    /// Provides minimum-cost one-to-one matching helpers for small equal-sized sets.<br />
    /// Returns source indexes in target order, so result.SrcIndexes[j] = i means tgt[j] is matched to src[i].<br />
    /// Uses state-compressed DP, suitable for small input sizes.
    /// </summary>
    public static class MinimumMatchHelper
    {
        /// <summary>
        /// Matches two value arrays by minimum total absolute difference.<br />
        /// Returns source indexes in target order, together with the minimum total cost.
        /// </summary>
        public static (int[] SrcIndexes, double MinCost) MatchValue(double[] srcPos, double[] tgtPos)
        {
            return Match(srcPos, tgtPos, (s, t) => Math.Abs(s - t));
        }

        /// <summary>
        /// Matches two Vector2 arrays by minimum total Euclidean distance.<br />
        /// Returns source indexes in target order, together with the minimum total cost.
        /// </summary>
        public static (int[] SrcIndexes, double MinCost) MatchVector2(Vector2[] srcPos, Vector2[] tgtPos)
        {
            return Match(srcPos, tgtPos, (s, t) => Vector2.Distance(s, t));
        }

        /// <summary>
        /// Matches two Vector3 arrays by minimum total Euclidean distance.<br />
        /// Returns source indexes in target order, together with the minimum total cost.
        /// </summary>
        public static (int[] SrcIndexes, double MinCost) MatchVector3(Vector3[] srcPos, Vector3[] tgtPos)
        {
            return Match(srcPos, tgtPos, (s, t) => Vector3.Distance(s, t));
        }

        /// <summary>
        /// Finds the minimum-cost one-to-one matching between two equal-sized arrays.<br />
        /// Returns source indexes in target order, together with the minimum total cost.
        /// </summary>
        public static (int[] SrcIndexes, double MinCost) Match<TSrc, TTgt>(TSrc[] src, TTgt[] tgt, Func<TSrc, TTgt, double> costSelector)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (tgt == null) throw new ArgumentNullException(nameof(tgt));
            if (costSelector == null) throw new ArgumentNullException(nameof(costSelector));
            if (src.Length != tgt.Length) throw new ArgumentException(I18n.Translate("internal/MinimumMatchHelper/lengthmismatch", "src and tgt must have the same length."));

            int n = src.Length;
            if (n == 0) return (new int[0], 0);
            if (n > 30) throw new ArgumentOutOfRangeException(nameof(src), I18n.Translate("internal/MinimumMatchHelper/toomanyelements", "Too many elements; at most 30 are supported."));

            double[,] cost = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double value = costSelector(src[i], tgt[j]);
                    if (double.IsNaN(value) || double.IsInfinity(value))
                        throw new ArgumentException(I18n.Translate("internal/MinimumMatchHelper/invalidcost", "Pair ({0}, {1}) returned an invalid value: {2}.", i, j, value));

                    cost[i, j] = value;
                }
            }

            int stateCount = 1 << n;
            double[] dp = new double[stateCount];
            int[] prevMask = new int[stateCount];
            int[] prevChoice = new int[stateCount];

            for (int mask = 0; mask < stateCount; mask++)
            {
                dp[mask] = double.PositiveInfinity;
                prevMask[mask] = -1;
                prevChoice[mask] = -1;
            }

            dp[0] = 0;

            for (int mask = 0; mask < stateCount; mask++)
            {
                if (double.IsPositiveInfinity(dp[mask]))
                    continue;

                int tgtIndex = 0;
                int x = mask;
                while (x != 0)
                {
                    x &= x - 1;
                    tgtIndex++;
                }

                if (tgtIndex >= n)
                    continue;

                for (int srcIndex = 0; srcIndex < n; srcIndex++)
                {
                    if ((mask & (1 << srcIndex)) != 0)
                        continue;

                    int nextMask = mask | (1 << srcIndex);
                    double newCost = dp[mask] + cost[srcIndex, tgtIndex];
                    if (newCost < dp[nextMask])
                    {
                        dp[nextMask] = newCost;
                        prevMask[nextMask] = mask;
                        prevChoice[nextMask] = srcIndex;
                    }
                }
            }

            int[] matchByTgt = new int[n];
            int curMask = stateCount - 1;

            for (int tgtIndex = n - 1; tgtIndex >= 0; tgtIndex--)
            {
                int srcIndex = prevChoice[curMask];
                if (srcIndex < 0)
                    throw new InvalidOperationException(I18n.Translate("internal/MinimumMatchHelper/reconstructionfailed", "Could not reconstruct the complete matching result."));

                matchByTgt[tgtIndex] = srcIndex;
                curMask = prevMask[curMask];
            }

            return (matchByTgt, dp[stateCount - 1]);
        }
    }
}