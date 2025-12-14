using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Evaluators;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.FFXIV;
using Triggernometry.Localization;
using Triggernometry.PluginBridges;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class XivEntityParser
    {

        public static string EvaluateEntityMember(Entity entity, string rawMemberExpression)
        {
            var accessor = XivEntityEvaluator.GetSingleAccessor(new MemberExpression(rawMemberExpression));
            return accessor(entity).ToDataString();
        }

        public static string EvaluateEntityMembers(Entity entity, string rawMemberExpressions)
        {
            var evaluator = XivEntityEvaluator.BuildEvaluator(rawMemberExpressions);
            return string.Join(", ", evaluator(entity));
        }

        internal static string Parse(IndexMemberExpression expr, Context ctx)
        {
            var conditionExpr = expr.Index;
            var isParty = expr.Name.EndsWith("party");
            var rawMemberExprs = expr.Member.RawExpression;

            var entity = GetEntityFromUserInput(conditionExpr, isParty);

            if (isParty && (expr.Index == "1" || expr.Index == Entity.MyName))
            {
                RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning,
                    $"检测到已弃用的旧版本表达式：{expr.RawExpression}。\n" +
                    $"请使用 ${{_me.属性}} 代替 ${{_ffxivparty[1].属性}}、${{_ffxivparty[${{_ffxivplayer}}].属性}} 等表达式查询自身属性。\n" +
                    $"触发器：{ctx?.Trigger?.FullPath ?? "null"}", 
                    ctx?.Trigger);
            }

            if (!entity.Exist && !rawMemberExprs.Equals("exist", StringComparison.OrdinalIgnoreCase))
            {
                RealPlugin.Instance.UnfilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Warning,
                    I18n.Translate("internal/Context/noEntity", "The queried entity does not exist: {0}. Trigger: ({1})",
                        expr.RawExpression, ctx?.Trigger?.FullPath ?? "null"),
                    ctx?.Trigger);
            }

            var evaluator = XivEntityEvaluator.BuildEvaluator(rawMemberExprs);
            return string.Join(", ", evaluator(entity));
        }

        internal static Entity GetEntityFromUserInput(string inputCondition, bool isParty = false)
            => GetEntitiesFromUserInput(inputCondition, isParty).FirstOrDefault() ?? Entity.NullEntity();

        internal static IEnumerable<Entity> GetEntitiesFromUserInput(string inputCondition, bool isParty = false)
        {
            // 1. party index: ffxivparty[n]
            if (isParty && int.TryParse(inputCondition, out int partyIdx) && partyIdx >= 1 && partyIdx <= 8)
            {
                string hexID = BridgeFFXIV.GetPartyMember(partyIdx).GetValue("id").ToString();
                if (!string.IsNullOrEmpty(hexID))
                {
                    var entity = Entity.GetEntityByID(hexID);
                    if (entity.Exist) yield return entity;
                }
                yield break;
            }

            // 2. single id (10123456)
            const int MinID = 0x1000_0000;
            if (uint.TryParse(inputCondition, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint id) && id >= MinID)
            {
                var entity = Entity.GetEntityByID(id);
                if (entity != null && (!isParty || entity.InParty || entity.ID == BridgeFFXIV.PlayerId))
                    yield return entity;
                yield break;
            }

            inputCondition = inputCondition.TrimEx();

            // 3. single name
            if (ShouldTreatAsName(inputCondition))
            {
                var entitiesByName = Entity.GetEntities(e => e.Name == inputCondition && (!isParty || e.InParty))
                    .ToList();

                foreach (var e in entitiesByName)
                    yield return e;
                // prevent further processing, we do not want to treat a name as a math expression
            }

            // 4. expression (x > 100 && y < 100 && type = 1)
            // Simple bools (like IsTH) should be written as IsTH = 1, to avoid ambiguity with names (like someone named "Isth"). 
            else
            {
                var filter = XivEntityFilterEvaluator.CreateFilter(inputCondition);
                foreach (var e in Entity.GetEntities(e => filter(e) && (!isParty || e.InParty)))
                    yield return e;
            }
        }

        private static bool ShouldTreatAsName(string input)
        { 
            var prevChar = '\0';
            foreach (char c in input)
            {
                // X > 0: false
                if (c == '=' | c == '<' | c == '>' | c == '&' | c == '|') return false; 
                // HasStatus(1): false
                // 724P flight unit (A-lpha): true
                // (Pure String): true
                if (c == '(' && prevChar != ' ' && prevChar != '\0') return false; 
                prevChar = c;
            }
            return true;
        }

    }
}
