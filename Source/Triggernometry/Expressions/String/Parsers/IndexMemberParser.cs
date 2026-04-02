using Scarborough;
using System;
using System.Linq;
using System.Web.Script.Serialization;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Evaluators;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.FFXIV;
using Triggernometry.Localization;
using Triggernometry.PluginBridges;
using Triggernometry.Utilities;
using static Triggernometry.Core.Configuration;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class IndexMemberParser
    {
        internal static string TryParse(string template, Context ctx)
        {
            ctx = ctx ?? Context.Unbound;
            var plug = ctx.Plugin; // can be null

            var expr = new IndexMemberExpression(template);
            if (expr.Member.Name == null && expr.Indexes.Length == 0)
            {
                return null;
            }

            switch (expr.Name)
            {
                // ===== FFXIV =====

                case "_me": // ${_me.prop}
                    if (expr.Member.Name.ToLowerInvariant() == "id" && !string.IsNullOrWhiteSpace(BridgeFFXIV.PlayerHexId))
                    {
                        return BridgeFFXIV.PlayerHexId;
                    }
                    else
                    {
                        var entity = Entity.GetMyself();
                        var evaluator = XivEntityEvaluator.BuildEvaluator(expr);
                        return string.Join(", ", evaluator(entity));
                    }

                case "_tgt": // ${_tgt.prop}
                    {
                        var targetID = Entity.GetMyself().TargetID;
                        var entity = Entity.GetEntityByID(targetID) ?? Entity.NullEntity();
                        var evaluator = XivEntityEvaluator.BuildEvaluator(expr);
                        return string.Join(", ", evaluator(entity));
                    }

                case "_ffxivparty":
                case "_party":
                case "_ffxiventity":
                case "_entity":
                    {
                        var entity = XivEntityParser.GetEntity(expr, ctx);
                        var evaluator = XivEntityEvaluator.BuildEvaluator(expr);
                        return string.Join(", ", evaluator(entity));
                    }

                case "_job": // ${_job[jobid].prop} or ${_job[Name].prop}
                    return Job.GetJob(expr.Index).QueryProperty(expr.Member.Name);

                case "_targetmarker2id":
                case "_tm2id":
                    return Memory.EntityIdByTargetMarker(expr.Index)?.ToString("X8")
                        ?? throw ErrorHelper.ParseTypeError(I18n.TranslateWord("string"), expr.Index, "targetmarker");

                case "_waymark":
                case "_wm":
                    return Memory.Waymark.QueryWaymark(expr.Index, expr.Member.Name) ?? "";

                // ===== Dynamic expressions with index =====

                case "_col": return $"${{{ctx.varName}[{ctx.tableColIndex}][{expr.Index}]}}";
                case "_row": return $"${{{ctx.varName}[{expr.Index}][{ctx.tableRowIndex}]}}";
                case "_colrl":
                    {
                        int colonIndex = ctx.varName.IndexOf(":");
                        string prefix = ctx.varName.Substring(0, colonIndex) + "dl" + ctx.varName.Substring(colonIndex);
                        string colHeader = $"${{{ctx.varName}[{ctx.tableColIndex}][1]}}";
                        string rowHeader = expr.Index;
                        return $"${{{prefix}[{colHeader}][{rowHeader}]}}";
                    }
                case "_rowcl":
                    {
                        int colonIndex = ctx.varName.IndexOf(":");
                        string prefix = ctx.varName.Substring(0, colonIndex) + "dl" + ctx.varName.Substring(colonIndex);
                        string colHeader = expr.Index;
                        string rowHeader = $"${{{ctx.varName}[1][{ctx.tableRowIndex}]}}";
                        return $"${{{prefix}[{colHeader}][{rowHeader}]}}";
                    }

                // ===== Plugin state =====

                case "_env":
                    return Environment.GetEnvironmentVariable(expr.Index) ?? "";

                case "_storage":
                    _ = plug.scriptingStorage.TryGetValue(expr.Index, out var scriptingObj);
                    return scriptingObj?.ToDataString() ?? "";

                case "_const":
                    lock (plug.cfg.Constants)
                    {
                        _ = plug.cfg.Constants.TryGetValue(expr.Index, out var constant);
                        return constant?.Value ?? "";
                    }

                case "_config":
                    switch (expr.Index)
                    {
                        case "DebugLevel": return ((int)plug.cfg.DebugLevel).ToString();
                        case "UseACTForSound": return plug.cfg.SoundMethod == AudioRoutingMethodEnum.ACT ? "1" : "0";
                        case "UseACTForTTS": return plug.cfg.TtsMethod == AudioRoutingMethodEnum.ACT ? "1" : "0";
                        case "FfxivLogNetwork": return plug.cfg.FfxivLogNetwork ? "1" : "0";
                        case "UseOsClipboard": return "1";
                        case "DeveloperMode": return plug.cfg.DeveloperMode ? "1" : "0";
                        case "AutoComplete": return plug.cfg.AutoComplete ? "1" : "0";
                        case "Autosave": return plug.cfg.AutosaveEnabled ? plug.cfg.AutosaveInterval.ToString() : "0";
                        case "Language": return plug.cfg.Language;
                        case "UnsafeUsage": return ((int)plug.cfg.UnsafeUsage).ToString();
                        case "DynamicUsage": return ((int)plug.cfg.DynamicUsage).ToString();
                        default:
                            var api = plug.cfg.GetAPIUsages().FirstOrDefault(a => a.Name == expr.Index) ?? new APIUsage();
                            return ((api.AllowLocal ? 1 : 0) + (api.AllowRemote ? 2 : 0) + (api.AllowAdmin ? 4 : 0)).ToString();
                    }

                case "_jsonresponse":
                    if (ctx.isContextJsonParsed == false)
                    {
                        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                        ctx.contextJsonResponse = jsonSerializer.Deserialize<dynamic>(ctx.contextResponse);
                        ctx.isContextJsonParsed = true;
                    }
                    string jsonPath = expr.Index;
                    dynamic node = ctx.contextJsonResponse;
                    foreach (string nodeName in jsonPath.Split("/".ToCharArray()))
                    {
                        node = node[nodeName];
                    }
                    return node?.ToString() ?? "";

                case "_actionhistory":
                    return expr.Index == "previous"
                        ? ctx.PeekActionResult(true, 0).ToString()
                        : ctx.PeekActionResult(false, int.Parse(expr.Index)).ToString();

                case "_textaura":
                    {
                        if (plug.sc != null)
                        {
                            lock (plug.sc.textitems)
                            {
                                ScarboroughText item = plug.sc.GetText(expr.Index);
                                if (item == null) return "";
                                switch (expr.Member.Name.ToLowerInvariant())
                                {
                                    case "x":
                                        return I18n.ThingToString(item.Left);
                                    case "y":
                                        return I18n.ThingToString(item.Top);
                                    case "w":
                                    case "width":
                                        return I18n.ThingToString(item.Width);
                                    case "h":
                                    case "height":
                                        return I18n.ThingToString(item.Height);
                                    case "opacity":
                                        return I18n.ThingToString(item.Opacity);
                                    case "text":
                                        return item.Text;
                                }
                            }
                        }
                        else
                        {
                            lock (plug.textauras)
                            {
                                if (!plug.textauras.TryGetValue(expr.Index, out UI.Forms.AuraContainerForm acf))
                                    return "";

                                switch (expr.Member.Name.ToLowerInvariant())
                                {
                                    case "x":
                                        return I18n.ThingToString(acf.Left);
                                    case "y":
                                        return I18n.ThingToString(acf.Top);
                                    case "w":
                                    case "width":
                                        return I18n.ThingToString(acf.Width);
                                    case "h":
                                    case "height":
                                        return I18n.ThingToString(acf.Height);
                                    case "opacity":
                                        return I18n.ThingToString(acf.PresentableOpacity);
                                    case "text":
                                        return acf.CurrentText;
                                }
                            }
                        }
                        return "";
                    }
                case "_imageaura":
                    {
                        if (plug.sc != null)
                        {
                            lock (plug.sc.imageitems)
                            {
                                ScarboroughImage item = plug.sc.GetImage(expr.Index);
                                if (item == null) return "";
                                switch (expr.Member.Name.ToLowerInvariant())
                                {
                                    case "x":
                                        return I18n.ThingToString(item.Left);
                                    case "y":
                                        return I18n.ThingToString(item.Top);
                                    case "w":
                                    case "width":
                                        return I18n.ThingToString(item.Width);
                                    case "h":
                                    case "height":
                                        return I18n.ThingToString(item.Height);
                                    case "opacity":
                                        return I18n.ThingToString(item.Opacity);
                                }
                            }
                        }
                        else
                        {
                            lock (plug.imageauras)
                            {
                                if (!plug.imageauras.TryGetValue(expr.Index, out UI.Forms.AuraContainerForm acf))
                                    return "";
                                switch (expr.Member.Name.ToLowerInvariant())
                                {
                                    case "x":
                                        return I18n.ThingToString(acf.Left);
                                    case "y":
                                        return I18n.ThingToString(acf.Top);
                                    case "w":
                                    case "width":
                                        return I18n.ThingToString(acf.Width);
                                    case "h":
                                    case "height":
                                        return I18n.ThingToString(acf.Height);
                                    case "opacity":
                                        return I18n.ThingToString(acf.PresentableOpacity);
                                }
                            }
                        }
                        return "";
                    }
            }

            return null;
        }
    }
}
