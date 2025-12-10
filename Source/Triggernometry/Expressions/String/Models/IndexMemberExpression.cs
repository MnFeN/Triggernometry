using System;
using System.Collections.Generic;
using System.Text;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.Expressions.String.Models
{
    /// <summary>
    /// Represents a name with optional indexes followed by optional dot member expression. <br />
    /// Name is always present; Indexes/Member may be present: <br />
    /// · <c>Name</c> <br />
    /// · <c>Name.XXX</c> <br />
    /// · <c>Name.XXX()</c> <br />
    /// · <c>Name.XXX(args)</c> <br />
    /// · <c>Name[index]</c> <br />
    /// · <c>Name[index][index2][index3]</c> <br />
    /// · <c>Name[index].Member</c> <br />
    /// · <c>Name[index].Member(MemberArgs)</c> <br />
    /// The part after '.' is treated as a <see cref="MemberExpression"/>.
    /// </summary>
    public readonly struct IndexMemberExpression
    {
        /// <summary> 
        /// The part before indexes or member. 
        /// Trimmed. 
        /// Should not be <see langword="null"/>. 
        /// </summary>
        public readonly string Name;

        private readonly string[] _indexes;
        private readonly MemberExpression? _member;
        private readonly string _rawExpression;

        /// <summary>
        /// The parsed index array with each index expression trimmed. <br />
        /// Should not be <see langword="null"/>; Empty array when absent.
        /// </summary>
        public string[] Indexes => _indexes ?? Array.Empty<string>();

        /// <summary>
        /// The parsed member expression. <br />
        /// <see cref="MemberExpression.Empty"/> when no member expression is given.
        /// </summary>
        public MemberExpression Member => _member ?? MemberExpression.Empty;

        /// <summary>
        /// The original expression text.
        /// Reconstructed if the raw expression was not provided.
        /// </summary>
        public string RawExpression => _rawExpression ?? BuildRawExpression();

        /// <summary>
        /// Creates an instance directly from already-parsed components.
        /// </summary>
        public IndexMemberExpression(
            string name,
            string[] indexes,
            MemberExpression member,
            string rawExpression = null)
        {
            Name = name ?? throw new ArgumentNullException("name");
            _indexes = indexes ?? Array.Empty<string>();
            _member = member;
            _rawExpression = rawExpression;
        }

        /// <summary>
        /// Parses an index-member expression from the given string.
        /// </summary>
        public IndexMemberExpression(string expr)
        {
            _rawExpression = expr ?? throw new ArgumentNullException(nameof(expr));
            _indexes = null;
            _member = null;
            Name = expr;

            // Scan the string to find the first '[' or '.'.
            // Whichever appears first determines the parsing path.
            string rawPossibleName = ScanPossibleName(expr, out int lBracketPos, out int validDotPos);

            // Pure name: no index and no member access
            if (rawPossibleName == null) return;

            // Parse index list if '[' is present
            if (lBracketPos >= 0)
            {
                var indexList = ScanIndexList(expr, startIndex: lBracketPos + 1, out validDotPos);

                // '[' present but no valid closing ']' found:
                // fall back to treating the entire expression as Name
                if (indexList.Count == 0) return;

                _indexes = indexList.ToArray();
            }

            Name = rawPossibleName.TrimEx();

            // Parse member access or invocation if a valid '.' is found
            if (validDotPos != -1)
                _member = new MemberExpression(expr, validDotPos + 1);
        }

        #region Index helpers

        /// <summary>Alias for the first index expression.</summary>
        public string Index => GetIndex(0);

        /// <summary>Alias for the first index expression.</summary>
        public string Index1 => GetIndex(0);

        /// <summary>Alias for the second index expression.</summary>
        public string Index2 => GetIndex(1);

        /// <summary>Alias for the first index expression, commonly used as table column.</summary>
        public string Col => GetIndex(0);

        /// <summary>Alias for the second index expression, commonly used as table row.</summary>
        public string Row => GetIndex(1);

        /// <summary>
        /// Get the index value at the specified position.
        /// Throw if the index does not exist.
        /// </summary>
        public string GetIndex(int indexOfIndex) => TryGetIndex(indexOfIndex)
            ?? throw new ArgumentOutOfRangeException(
                nameof(indexOfIndex),
                $"Expression {RawExpression} has only {Indexes.Length} index(es).");

        /// <summary>
        /// Attempt to get the index value at the specified position.
        /// Return <see langword="null" /> if the index does not exist.
        /// </summary>
        public string TryGetIndex(int indexOfIndex)
        {
            if (indexOfIndex >= 0 && indexOfIndex < Indexes.Length)
                return Indexes[indexOfIndex];
            else
                return null;
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Builds the normalized expression text from parsed components.
        /// </summary>
        private string BuildRawExpression()
        {
            var sb = new StringBuilder(Name);

            if (Indexes.Length > 0)
                sb.Append($"[{string.Join("][", Indexes)}]");

            if (Member.HasValue)
                sb.Append($".{Member.RawExpression}");

            return sb.ToString();
        }

        /// <summary>
        /// Scans the expression to find the possible base name,
        /// along with the positions of the first '[' and the first valid '.'.
        /// </summary>
        private static string ScanPossibleName(
            string expr,
            out int lBracketPos,
            out int validDotPos)
        {
            validDotPos = -1;
            lBracketPos = -1;

            for (int i = 0; i < expr.Length; i++)
            {
                char c = expr[i];

                // '[' is straightforward: take the substring before it
                if (c == '[' && lBracketPos == -1)
                {
                    lBracketPos = i;
                    return expr.Substring(0, i);
                }

                // '.' may appear inside expressions and must be validated
                // e.g. ${?d: a=1.5, b=2.5 .size}
                else if (c == '.' && validDotPos == -1)
                {
                    bool isValidDot = true;
                    for (int k = i + 1; k < expr.Length; k++)
                    {
                        var c2 = expr[k];

                        // Another '[' or '.' invalidates this dot
                        if (c2 == '[' || c2 == '.')
                        {
                            i = k - 1; // Skip ahead
                            isValidDot = false;
                            break;
                        }
                        // '(' indicates a method call and is acceptable
                        else if (c2 == '(')
                        {
                            break;
                        }
                    }

                    if (isValidDot)
                    {
                        validDotPos = i;
                        return expr.Substring(0, i);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Scans and parses a sequence of index expressions starting at the given position.
        /// </summary>
        private static List<string> ScanIndexList(
            string expr,
            int startIndex,
            out int validDotPos)
        {
            validDotPos = -1;
            var indexList = new List<string>();

            for (int i = startIndex; i < expr.Length; i++)
            {
                char c = expr[i];
                if (c != ']') continue;

                // After ']', scan ahead for the next non-whitespace character
                int k = i + 1;
                while (k < expr.Length && expr[k].IsWhiteSpaceEx())
                {
                    k++;
                }

                bool isValidClose = k >= expr.Length || expr[k] == '[' || expr[k] == '.';
                if (!isValidClose)
                {
                    i = k - 1; // Skip scanned whitespace
                    continue;
                }

                // Valid index closure: extract index content
                var indexString = expr.Substring(startIndex, i - startIndex).TrimEx();
                indexList.Add(indexString);

                if (k >= expr.Length)
                {
                    break;
                }
                else if (expr[k] == '[')
                {
                    startIndex = k + 1;
                    i = k;
                    continue;
                }
                else if (expr[k] == '.')
                {
                    validDotPos = k;
                    break;
                }
            }

            return indexList;
        }

        #endregion
    }
}
