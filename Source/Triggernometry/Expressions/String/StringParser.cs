using System;
using System.Collections.Generic;
using System.Text;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Parsers;
using static Triggernometry.Expressions.String.Utils.ParserCommon;

namespace Triggernometry.Expressions.String
{
    /// <summary>
    /// Entry point for Triggernometry string expression parsing. <br /><br />
    /// · This is the only public API for evaluating string expressions such as <c>${...}</c> templates.<br />
    /// </summary>
    public static class StringParser
    {
        /// <summary>
        /// Evaluate a single template body expression "..." (the inside of "<c>${...}</c>").<br />
        /// · <paramref name="expr" />: Expression inside the template, such as "<c>lvar:a[1]</c>".<br />
        /// · <paramref name="ctx" />: Evaluation context; if null, <see cref="Context.Unbound"/> is used.<br />
        /// · Returns the evaluated result as a string.
        /// </summary>
        public static string ParseSingleTemplate(string expr, Context ctx = null)
            => TemplateParser.ParseSingleTemplate(expr, ctx, false);

        /// <summary>
        /// Parse a full string that may contain templates such as <c>$n</c>, <c>${...}</c>, <c>¤{...}</c>.<br />
        /// · <paramref name="expr" />: Full input text to parse and expand.<br />
        /// · <paramref name="ctx" />: Evaluation context; if null, <see cref="Context.Unbound"/> is used.<br />
        /// · Returns the text with all templates evaluated.
        /// </summary>
        public static string Parse(string expr, Context ctx = null)
            => Parse(expr, ctx, false);

        /// <summary>
        /// Core implementation for parsing template expressions in a string.<br />
        /// · <paramref name="expr" />: Full input text that may contain templates such as <c>$n</c>, <c>${...}</c>, <c>¤{...}</c>.<br />
        /// · <paramref name="ctx" />: Evaluation context used during template evaluation; if null, <see cref="Context.Unbound"/> is used.<br />
        /// · <paramref name="isTestModeNumeric" />: Whether the expression is numeric. Only used when testing with placeholders.<br />
        /// </summary>
        internal static string Parse(string expr, Context ctx, bool isTestModeNumeric)
        {
            ctx = ctx ?? Context.Unbound;

            // '\r' '\n' => '⏎'
            string newExpr = ReplaceLineBreak(expr); 

            int depth;
            for (depth = 0; depth < MAX_DEPTH; depth++)
            {
                var templates = TemplateParser.FindNonNestedTemplates(newExpr);
                if (templates.Count == 0)
                    break;

                newExpr = TemplateParser.ReplaceTemplates(newExpr, templates, ctx, isTestModeNumeric);
            }

            if (depth == MAX_DEPTH)
            {
                throw new InvalidOperationException($"当前表达式 {expr} 解析疑似无限循环。");
            }

            // ¤1 ¤{...} => $1 ${...}
            newExpr = TemplateParser.ReplacePlaceholderTemplates(newExpr);

            // '⏎' => '\r' '\n' 
            newExpr = newExpr.Replace(LINEBREAK.ToString(), Environment.NewLine);
            return newExpr;
        }

    }
}
