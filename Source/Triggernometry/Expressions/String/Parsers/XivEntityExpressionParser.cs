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
    internal static class XivEntityExpressionParser
    {

        public static string EvaluateEntity(Entity entity, string rawMethodExpressions)
        {
            var evaluator = XivEntityEvaluator.BuildEvaluator(rawMethodExpressions);
            return string.Join(", ", evaluator(entity));
        }

        internal static string Parse(IndexMethodExpression expr, Context ctx)
        {
            var conditionExpr = expr.Index;
            var isParty = expr.Name.EndsWith("party");
            var rawMethodExprs = expr.Method.RawExpression;

            var entity = GetEntityFromUserInput(conditionExpr, isParty);

            if (!entity.Exist && !rawMethodExprs.Equals("exist", StringComparison.OrdinalIgnoreCase))
            {
                RealPlugin.Instance.UnfilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Warning,
                    I18n.Translate("internal/Context/noEntity", "The queried entity does not exist: {0}. Trigger: ({1})",
                        expr.RawExpression, ctx?.Trigger?.FullPath ?? "null"),
                    ctx?.Trigger);
            }

            var evaluator = XivEntityEvaluator.BuildEvaluator(rawMethodExprs);
            return string.Join(", ", evaluator(entity));
        }

        private static readonly Regex entityNameGuess = new Regex("^[^<>()=&|!,]+$", RegexOptions.Compiled);

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
                if (entity != null && (!isParty || entity.InParty))
                    yield return entity;
                yield break;
            }

            inputCondition = inputCondition.TrimEx();

            // 3. single name
            if (entityNameGuess.IsMatch(inputCondition))
            {
                var entitiesByName = Entity.GetEntities(e => e.Name == inputCondition && (!isParty || e.InParty))
                    .ToList();

                foreach (var e in entitiesByName)
                    yield return e;

                if (entitiesByName.Count > 0) // found by name => skip 4. expression parsing
                    yield break;
            }

            // 4. expression (x > 100 && y < 100 && type = 1)
            var filter = XivEntityFilterEvaluator.CreateFilter(inputCondition);
            foreach (var e in Entity.GetEntities(e => filter(e) && (!isParty || e.InParty)))
                yield return e;
        }

    }
}
