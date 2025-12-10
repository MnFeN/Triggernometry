using System;
using System.Collections.Generic;
using System.Linq;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.FFXIV;

namespace Triggernometry.Expressions.String.Evaluators
{
    internal static class XivEntityFilterEvaluator // to-do: sortings
    {
        internal static Func<Entity, bool> CreateFilter(string rawFilterExpression)
        { 
            var funcTokens = BuildEntityTokenPipeline(rawFilterExpression);
            return entity =>
            {
                var tokens = funcTokens.Select(func => func(entity)).ToList();
                var result = MathParser.MathParserLogic(tokens);
                return !MathParser.IsZero(result);
            };
        }

        private static List<Func<Entity, string>> BuildEntityTokenPipeline(string rawFilterExpression)
        {
            // "X=0 && HasStatus(0x32)"
            var rawTokenList = MathParser.Lexer(rawFilterExpression);
            // "X"   "="   "0"   "&&"   "HasStatus"   " *"   "("   "0x32"   ")"
            var funcTokens = new List<Func<Entity, string>>();
            for (int i = 0; i < rawTokenList.Count; i++)
            {
                var token = rawTokenList[i];
                bool followedByParentheses = // the fake token " *" added by the lexer, need to rewrite when the lexer is improved
                    i + 2 < rawTokenList.Count && rawTokenList[i + 1] == " *" && rawTokenList[i + 2] == "("; 

                // single keyword token (prop; jobProp; method without args) or other normal numeric tokens
                if (!followedByParentheses)
                {
                    var possibleMemberExpr = new MemberExpression(token, null, token);
                    var accessor = XivEntityEvaluator.TryGetSingleAccessor(possibleMemberExpr);
                    if (accessor != null) // found
                        funcTokens.Add(e => accessor(e).ToDataString().Replace(" ", "")); // why replace?
                    else
                        funcTokens.Add(e => token); // normal numeric token
                    continue;
                }

                // token followed by parentheses
                if (!Entity.ValidEntityMethodNames.Contains(token))   // normal numeric tokens
                {
                    funcTokens.Add(e => token);
                    continue;
                }

                // entity method with args: search for the matching ")"
                int depth = 1;
                bool paired = false;
                for (int j = i + 3; j < rawTokenList.Count; j++) // start from the token after "("
                {
                    if (rawTokenList[j] == "(")
                        depth++;
                    else if (rawTokenList[j] == ")")
                    {
                        depth--;
                        if (depth == 0) // closed
                        {
                            var methodArgs = rawTokenList.GetRange(i + 3, j - i - 3).ToArray(); // between (...)
                            var methodExpr = new MemberExpression(token, methodArgs);
                            var accessor = XivEntityEvaluator.GetSingleAccessor(methodExpr);
                            funcTokens.Add(e => accessor(e).ToDataString().Replace(" ", "")); // to-do: spaces in numeric expressions
                            i = j;
                            paired = true;
                            break;
                        }
                    }
                }
                if (!paired) funcTokens.Add(e => token);

            }
            return funcTokens;
        }


    }
}
