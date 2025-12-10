using System;
using System.Linq;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.FFXIV;
using Triggernometry.Localization;

namespace Triggernometry.Expressions.String.Evaluators
{
    internal static class XivEntityEvaluator
    {
        
        /// <summary>
        /// · "X, Y, HasStatus(0x1A)"
        /// </summary>
        internal static Func<Entity, string[]> BuildEvaluator(string rawMemberExpressions)
        {
            var memberExpressions = ArgHelper.SplitArguments(rawMemberExpressions);
            return BuildEvaluator(memberExpressions, $"Entity.{rawMemberExpressions}");
        }

        internal static Func<Entity, string[]> BuildEvaluator(IndexMemberExpression expr)
        {
            if (!expr.Member.HasValue) 
                throw new ArgumentException($"实体表达式 {expr.RawExpression} 未包含属性", nameof(expr.Member));

            // Entity expressions allow multiple members (properties/methods) separated with comma,
            // so we need to split the raw expression of the member part.
            var memberExpressions = ArgHelper.SplitArguments(expr.Member.RawExpression);
            return BuildEvaluator(memberExpressions, expr.RawExpression);
        }

        internal static Func<Entity, string[]> BuildEvaluator(string[] memberExpressions, string rawExprForErrorOverride = null)
        {
            var rawExpr = rawExprForErrorOverride ?? $"Entity.{string.Join(", ", memberExpressions)}";
            var accessors = memberExpressions
                .Select(raw => new MemberExpression(raw))
                .Select(member => GetSingleAccessor(member)); // GetSingleAccessor would throw error if not found

            return entity => accessors.Select(a => {
                try
                {
                    return a(entity).ToDataString();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(I18n.Translate("internal/FFXIV/Entity/？？？？？？？？",
                        "Failed to evaluate entity property/method expression '{0}': {1}",
                        rawExpr, ex.Message), ex);

                }
            }).ToArray();
        }

        internal static Func<Entity, object> GetSingleAccessor(MemberExpression expr)
            => TryGetSingleAccessor(expr) ?? throw new ArgumentException("Invalid entity property/method name: " + expr.Name, nameof(expr));

        internal static Func<Entity, object> TryGetSingleAccessor(MemberExpression expr)
        {
            var accessor = TryGetSingleMethodAccessor(expr.Name, expr.Args)
                        ?? TryGetSinglePropAccessor(expr.Name);
            if (accessor != null) 
                return accessor;

            // Job-related: Job, JobID, JobEN, Role, etc.
            if (Job.TryGetAccessor(expr.Name, out var jobAccessor)) 
                return entity => jobAccessor(entity.Job);

            return null;
        }

        private static Func<Entity, object> TryGetSinglePropAccessor(string propName)
        {
            if (Entity._propAccessors.TryGetValue(propName, out var propAccessor))
            {
                return entity => propAccessor(entity);
            }
            else return null;
        }

        private static Func<Entity, object> TryGetSingleMethodAccessor(string methodName, string[] args)
        {
            if (Entity._methodAccessors.TryGetValue(methodName, out var methodAccessor))
            {
                return entity => methodAccessor(entity, args);
            }
            else return null;
        }

    }
}
