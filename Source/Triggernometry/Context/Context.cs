using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Triggernometry.FFXIV;
using Triggernometry.PluginBridges;
using Triggernometry.Utilities;
using Triggernometry.Variables;
/*
namespace Triggernometry.NewContext
{
    public partial class NewContext
    {
        public string ExpandVariables(LoggerDelegate logger, object o, bool numeric, string expr)
        {
            Match m, mx;
            string newexpr = expr;
            newexpr = ReplaceLineBreak(newexpr); // replace back after parsed

            int i = 1;
            while (true)
            {
                m = rex.Match(newexpr);
                if (m.Success == false)
                {
                    m = rexNum.Match(newexpr);
                    if (m.Success == false)
                    {
                        break;
                    }
                }
                string x = m.Groups["id"].Value;
                string val = "";
                bool found = false;
                if (testByPlaceholder == true)
                {
                    if (x == "_since")
                    {
                        val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalSeconds)).ToString();
                    }
                    else if (x == "_sincems")
                    {
                        val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalMilliseconds)).ToString();
                    }
                    else
                    {
                        val = (numeric == true ? "1" : "test");
                    }
                    found = true;
                }
                else
                {
                    int gn;
                    if (Int32.TryParse(x, out gn) == true)
                    {
                        if (gn >= 0 && gn < numgroups.Count)
                        {
                            val = numgroups[gn];
                            if (plug != null)
                            {
                                val = plug.cfg.PerformSubstitution(val, Configuration.Substitution.SubstitutionScopeEnum.CaptureGroup);
                            }
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        if (namedgroups.ContainsKey(x) == true)
                        {
                            val = namedgroups[x];
                            if (plug != null)
                            {
                                val = plug.cfg.PerformSubstitution(val, Configuration.Substitution.SubstitutionScopeEnum.CaptureGroup);
                            }
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        Match matchExistVar;
                        if (x == "_since")
                        {
                            val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalSeconds)).ToString();
                            found = true;
                        }
                        else if (x == "_sincems")
                        {
                            val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalMilliseconds)).ToString();
                            found = true;
                        }
                        else if (x == "_systemtime")
                        {
                            val = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
                            found = true;
                        }
                        else if (x == "_systemtimems")
                        {
                            val = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
                            found = true;
                        }
                        else if (x == "_ffxivplayer" || x == "_me")
                        {
                            VariableDictionary vc = PluginBridges.BridgeFFXIV.GetMyself();
                            if (vc != null)
                            {
                                val = vc.GetValue("name").ToString();
                            }
                            found = true;
                        }
                        else if (x == "_ffxivzoneid")
                        {
                            if (zoneIdOverride != null)
                            {
                                val = zoneIdOverride;
                            }
                            else
                            {
                                val = PluginBridges.BridgeFFXIV.ZoneID.ToString();
                            }
                            found = true;
                        }
                        else if (x == "_ffxivpartyorder")
                        {
                            val = plug.cfg.FfxivPartyOrdering + " " + plug.cfg.FfxivCustomPartyOrder;
                            found = true;
                        }
                        else if (x == "_ffxivprocid")
                        {
                            val = PluginBridges.BridgeFFXIV.GetProcessId().ToString();
                            found = true;
                        }
                        else if (x == "_ffxivprocname")
                        {
                            val = PluginBridges.BridgeFFXIV.GetProcessName();
                            found = true;
                        }
                        else if (x == "_ffxivversion")
                        {
                            val = PluginBridges.BridgeFFXIV.GetGameVersion();
                            found = true;
                        }
                        else if (x == "_ffxivlanguage")
                        {
                            val = GameLanguage.Language.ToString();
                            found = true;
                        }
                        else if (x == "_ffxivisglobal")
                        {
                            val = (byte)GameLanguage.Language <= 3 ? "1" : "0";
                            found = true;
                        }
                        else if (x == "_ffxivincombat") // game status
                        {
                            val = ModuleInCombat.GetInCombat() ? "1" : "0";
                            found = true;
                        }
                        else if (x == "_incombat") // ACT status
                        {
                            val = plug != null && plug.InCombatHook() ? "1" : "0";
                            found = true;
                        }
                        else if (x == "_duration")
                        {
                            try
                            {
                                if (plug != null && plug.InCombatHook() == true)
                                {
                                    val = ((int)Math.Floor(plug.EncounterDurationHook())).ToString();
                                }
                                else
                                {
                                    val = "0";
                                }
                            }
                            catch (Exception)
                            {
                                val = "0";
                            }
                            found = true;
                        }
                        else if (x == "_response")
                        {
                            val = contextResponse;
                            found = true;
                        }
                        else if (x == "_responsecode")
                        {
                            val = contextResponseCode.ToString();
                            found = true;
                        }
                        else if (x == "_triggername")
                        {
                            val = trig?.Name ?? "(null)";
                            found = true;
                        }
                        else if (x == "_triggerid")
                        {
                            val = trig != null ? trig.Id.ToString() : "(null)";
                            found = true;
                        }
                        else if (x == "_triggerpath")
                        {
                            val = trig?.FullPath ?? "";
                            found = true;
                        }
                        else if (x == "_loopiterator" || x == "_i")
                        {
                            val = loopiterator.ToString();
                            found = true;
                        }
                        else if (x == "_idx")
                        {
                            val = listIndex.ToString();
                            found = true;
                        }
                        else if (x == "_col")
                        {
                            val = tableColIndex.ToString();
                            found = true;
                        }
                        else if (x == "_row")
                        {
                            val = tableRowIndex.ToString();
                            found = true;
                        }
                        else if (x.StartsWith("_col["))
                        {
                            string rowIndex = x.Substring(5, x.Length - 6);
                            val = $"${{{varName}[{tableColIndex}][{rowIndex}]}}";
                            found = true;
                        }
                        else if (x.StartsWith("_row["))
                        {
                            string colIndex = x.Substring(5, x.Length - 6);
                            val = $"${{{varName}[{colIndex}][{tableRowIndex}]}}";
                            found = true;
                        }
                        else if (x.StartsWith("_colrl["))
                        {
                            int colonIndex = varName.IndexOf(":");
                            string prefix = varName.Substring(0, colonIndex) + "dl" + varName.Substring(colonIndex);
                            string colHeader = $"${{{varName}[{tableColIndex}][1]}}";
                            string rowHeader = x.Substring(7, x.Length - 8);
                            val = $"${{{prefix}[{colHeader}][{rowHeader}]}}";
                            found = true;
                        }
                        else if (x.StartsWith("_rowcl["))
                        {
                            int colonIndex = varName.IndexOf(":");
                            string prefix = varName.Substring(0, colonIndex) + "dl" + varName.Substring(colonIndex);
                            string colHeader = x.Substring(7, x.Length - 8);
                            string rowHeader = $"${{{varName}[1][{tableRowIndex}]}}";
                            val = $"${{{prefix}[{colHeader}][{rowHeader}]}}";
                            found = true;
                        }
                        else if (x == "_key")
                        {
                            val = dictKey;
                            found = true;
                        }
                        else if (x == "_val")
                        {
                            val = dictValue;
                            found = true;
                        }
                        else if (x == "_this")
                        {
                            if (varName.StartsWith("tvar:") || varName.StartsWith("ptvar:"))
                            {
                                val = $"${{{varName}[{tableColIndex}][{tableRowIndex}]}}";
                            }
                            else
                            {
                                val = $"${{{varName}[{listIndex}]}}";
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_actionhistory"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = mx.Groups["index"].Value;
                                if (idx == "previous")
                                {
                                    val = PeekActionResult(true, 0).ToString();
                                }
                                else
                                {
                                    val = PeekActionResult(false, Int32.Parse(idx)).ToString();
                                }
                                found = true;
                            }
                        }
                        else if (x == "_configpath")
                        {
                            val = RealPlugin.plug.path;
                            found = true;
                        }
                        else if (x == "_pluginpath")
                        {
                            val = RealPlugin.plug.pluginPath;
                            found = true;
                        }
                        else if (x == "_pluginversion")
                        {
                            val = RealPlugin.plug.cfg.PluginVersion;
                            found = true;
                        }
                        else if (x.StartsWith("_env"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = mx.Groups["index"].Value;
                                val = System.Environment.GetEnvironmentVariable(idx);
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_offset["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string key = Trim(mx.Groups["index"].Value).ToLower();
                                if (key == "1b")
                                    val = I18n.ThingToString(Memory.Offset1B);
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_targetmarker2id[") || x.StartsWith("_tm2id["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string key = Trim(mx.Groups["index"].Value);
                                uint id = Memory.EntityIdByTargetMarker(key) ?? throw ParseTypeError(I18n.TranslateWord("string"), key, "targetmarker", x);
                                val = id.ToString("X8");
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_wm[") || x.StartsWith("_waymark["))
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string rawType = Trim(mx.Groups["index"].Value);
                                string rawProp = Trim(mx.Groups["prop"].Value);
                                val = Memory.Waymark.QueryWaymark(rawType, rawProp);
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_storage["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string key = Trim(mx.Groups["index"].Value);
                                Dictionary<string, object> storage = plug.scriptingStorage;

                                if (!storage.ContainsKey(key))
                                    val = "";
                                else
                                {
                                    object item = storage[key];
                                    Type type = item.GetType();
                                    if (type.IsPrimitive && type != typeof(char) || type == typeof(decimal) || type == typeof(string))
                                        val = Convert.ChangeType(item, type, InvClt).ToString();
                                    else
                                        val = item.ToString();
                                }
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_job[")) // ${_job[jobid].prop} or ${_job[Name].prop}
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string rawJob = Trim(mx.Groups["index"].Value);
                                string prop = mx.Groups["prop"].Value;
                                val = Job.GetJob(rawJob).QueryProperty(prop);
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_const"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = Trim(mx.Groups["index"].Value);
                                lock (plug.cfg.Constants)
                                {
                                    if (plug.cfg.Constants.ContainsKey(idx))
                                    {
                                        val = plug.cfg.Constants[idx].Value;
                                    }
                                    else
                                    {
                                        val = "";
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_config["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = Trim(mx.Groups["index"].Value);
                                switch (idx)
                                {
                                    case "DebugLevel": val = ((int)plug.cfg.DebugLevel).ToString(InvClt); break;
                                    case "UseACTForSound": val = plug.cfg.SoundMethod == Configuration.AudioRoutingMethodEnum.ACT ? "1" : "0"; break;
                                    case "UseACTForTTS": val = plug.cfg.TtsMethod == Configuration.AudioRoutingMethodEnum.ACT ? "1" : "0"; break;
                                    case "FfxivLogNetwork": val = plug.cfg.FfxivLogNetwork ? "1" : "0"; break;
                                    case "UseOsClipboard": val = "1"; break;
                                    case "DeveloperMode": val = plug.cfg.DeveloperMode ? "1" : "0"; break;
                                    case "AutoComplete": val = plug.cfg.AutoComplete ? "1" : "0"; break;
                                    case "Autosave": val = plug.cfg.AutosaveEnabled ? plug.cfg.AutosaveInterval.ToString(InvClt) : "0"; break;
                                    case "Language": val = plug.cfg.Language; break;
                                    case "UnsafeUsage": val = ((int)plug.cfg.UnsafeUsage).ToString(); break;
                                    default:
                                        try
                                        {
                                            foreach (var api in plug.cfg.GetAPIUsages())
                                            {
                                                if (api.Name == idx)
                                                    val = ((api.AllowLocal ? 1 : 0) + (api.AllowRemote ? 2 : 0) + (api.AllowAdmin ? 4 : 0)).ToString();
                                            }
                                        }
                                        catch { throw InvalidValueError("_config", I18n.TranslateWord("key"), idx, x); }
                                        break;
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_jsonresponse"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string pathspec = mx.Groups["index"].Value;
                                if (contextJsonParsed == false)
                                {
                                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                                    contextJsonResponse = jsonSerializer.Deserialize<dynamic>(contextResponse);
                                    contextJsonParsed = true;
                                }
                                string[] path = pathspec.Split("/".ToCharArray());
                                dynamic curdir = contextJsonResponse;
                                foreach (string px in path)
                                {
                                    curdir = curdir[px];
                                }
                                val = curdir.ToString();
                                found = true;
                            }
                        }
                        // check if variable exists (combined the logic for all types of variable expressions)
                        // evar  = ev, epvar  = epv;    elvar = el, eplvar = epl;
                        // etvar = et, eptvar = ept;    edvar = ed, epdvar = epd;
                        // etext, eimage for Auras;     ecallback for named callbacks;   estorage for script storage
                        else if ((matchExistVar = rexExistVar.Match(x)).Success)
                        {
                            bool persist = matchExistVar.Groups["persist"].Value == "p";
                            string type = matchExistVar.Groups["type"].Value;
                            string varname = matchExistVar.Groups["name"].Value;
                            VariableStore store = persist ? plug.cfg.PersistentVariables : plug.sessionvars;
                            dynamic source = null;
                            switch (type)
                            {
                                case "v": source = store.Scalar; break;
                                case "l": source = store.List; break;
                                case "t": source = store.Table; break;
                                case "d": source = store.Dict; break;
                                case "text":
                                    if (plug.sc != null)
                                        source = plug.sc.textitems;
                                    else
                                        source = plug.textauras;
                                    break;
                                case "image":
                                    if (plug.sc != null)
                                        source = plug.sc.imageitems;
                                    else
                                        source = plug.imageauras;
                                    break;
                                case "callback": source = plug.callbacksByName; break;
                                case "storage": source = plug.scriptingStorage; break;
                            }
                            lock (source)
                            {
                                val = (source == null) ? "" : source.ContainsKey(varname) ? "1" : "0";
                            }
                            found = true;
                        }
                        // retrieve scalar variable value
                        else if (x.StartsWith("var:") || x.StartsWith("pvar:") || x.StartsWith("v:") || x.StartsWith("pv:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            lock (store.Scalar) // verified
                            {
                                VariableScalar vs = GetScalarVariable(store, varname, false);
                                val = vs.Value;
                            }
                            found = true;
                        }
                        // retrieve list variable value
                        else if (x.StartsWith("lvar:") || x.StartsWith("l:")
                              || x.StartsWith("plvar:") || x.StartsWith("pl:")
                              || x.StartsWith("?lvar:") || x.StartsWith("?l:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables
                                                : x.StartsWith("l") ? plug.sessionvars : new VariableStore();
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexMethod.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gprop = mx.Groups["prop"].Value;
                                string garg = mx.Groups["arg"].Value;
                                string[] args = SplitArguments(garg);
                                int argc = args.Length;
                                if (x.StartsWith("?"))
                                {   // build temp var
                                    store.List["?lvar"] = VariableList.BuildTemp(gname);
                                    gname = "?lvar";
                                }
                                switch (gprop)
                                {
                                    case "size":
                                    case "length":
                                        lock (store.List)
                                        {
                                            VariableList vl = GetListVariable(store, gname, false);
                                            val = vl.Size.ToString();
                                        }
                                        found = true;
                                        break;
                                    case "indexof":
                                    case "i":
                                    case "lastindexof":
                                        if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                        lock (store.List)
                                        {
                                            VariableList vl = GetListVariable(store, gname, false);
                                            val = (gprop.StartsWith("i")) ? vl.IndexOf(args[0]).ToString() : vl.LastIndexOf(args[0]).ToString();
                                        }
                                        found = true;
                                        break;
                                    case "indicesof":
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(gprop, "1-3", argc, x); }
                                            string joiner = GetArgument(args, 1, defaultValue: ",");
                                            string slicesStr = GetArgument(args, 2, defaultValue: ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = vl.IndicesOf(args[0], joiner, indices);
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "sum":  // lvar:list.sum(slices = ":")
                                        {
                                            if (argc > 1) { throw ArgCountError(gprop, "0-1", argc, x); }
                                            string slicesStr = GetArgument(args, 0, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = I18n.ThingToString(vl.Sum(indices));
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "min":  // lvar:list.min(type = "n", slices = ":")  num = "n" / str = "s" / hex = "h"
                                    case "max":
                                        {
                                            if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                            ExtremumInit(args, gprop, x, out string type, out bool isMin);
                                            string slicesStr = GetArgument(args, 1, ":");
                                            List<string> strings;
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                strings = vl.Values.Where((_, idx) => indices.Contains(idx)).Select(var => var.ToString()).ToList();
                                            }
                                            val = ExtremumGetResult(strings, type, isMin, gname, gprop, x);
                                            found = true;
                                        }
                                        break;
                                    case "join":        // lvar:list.join(joiner = ",", slices = ":")
                                    case "randjoin":    // lvar:list.randjoin(joiner = ",", slices = ":")
                                        {
                                            if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                            string joiner = GetArgument(args, 0, ",");
                                            string slicesStr = GetArgument(args, 1, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                List<int> indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                if (gprop == "randjoin")
                                                {
                                                    indices = indices.OrderBy(_ => rng.Next()).ToList();
                                                }
                                                val = vl.Join(joiner, indices);
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "count": // count(targetStr, slices = ":")
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(gprop, "1-2", argc, x); }
                                            var slicesStr = GetArgument(args, 1, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = vl.Count(args[0], indices).ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "contain":
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(gprop, "1-2", argc, x); }
                                            string slicesStr = GetArgument(args, 1, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                List<int> indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = (indices.Any(idx => vl.Values[idx].ToString() == args[0])) ? "1" : "0";
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "ifcontain":
                                        if (argc != 3) { throw ArgCountError(gprop, "3", argc, x); }
                                        lock (store.List)
                                        {
                                            VariableList vl = GetListVariable(store, gname, false);
                                            val = (vl.Values.Any(v => v.ToString() == args[0])) ? args[1] : args[2];
                                            found = true;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                mx = rexListIdx.Match(varname);
                                if (mx.Success)
                                {
                                    string gname = mx.Groups["name"].Value;
                                    string gindex = mx.Groups["index"].Value;
                                    gindex = (gindex == "last") ? "-1" : gindex;

                                    if (x.StartsWith("?"))
                                    {   // build temp var
                                        store.List["?lvar"] = VariableList.BuildTemp(gname);
                                        gname = "?lvar";
                                    }
                                    if (!int.TryParse(gindex, NSFloat, InvClt, out int iindex))
                                    {
                                        throw ParseTypeError(I18n.TranslateWord("index"), gindex, I18n.TranslateWord("int"), x);
                                    }
                                    lock (store.List)
                                    {
                                        VariableList vl = GetListVariable(store, gname, false);
                                        val = vl.Peek(iindex).ToString();
                                    }
                                    found = true;
                                }
                            }
                        }
                        // retrieve dict variable value
                        else if (x.StartsWith("dvar:") || x.StartsWith("d:")
                              || x.StartsWith("pdvar:") || x.StartsWith("pd:")
                              || x.StartsWith("?dvar:") || x.StartsWith("?d:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables
                                                : x.StartsWith("d") ? plug.sessionvars : new VariableStore();
                            string varname = x.Substring(x.IndexOf(":") + 1);

                            mx = rexMethod.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gprop = mx.Groups["prop"].Value;
                                string garg = mx.Groups["arg"].Value;
                                string[] args = SplitArguments(garg);
                                int argc = args.Length;
                                if (x.StartsWith("?"))
                                {   // build temp var
                                    store.Dict["?dvar"] = VariableDictionary.BuildTemp(gname);
                                    gname = "?dvar";
                                }
                                switch (gprop)
                                {
                                    case "size":
                                    case "length":
                                        {
                                            lock (store.Dict)
                                            {
                                                VariableDictionary vd = GetDictVariable(store, gname, false);
                                                val = vd.Size.ToString();
                                            }
                                        }
                                        break;
                                    case "ekey":
                                    case "evalue":
                                        if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            bool exist = (gprop == "ekey") ? vd.ContainsKey(queryStr) : vd.ContainsValue(queryStr);
                                            val = exist ? "1" : "0";
                                        }
                                        break;
                                    case "ifekey":
                                    case "ifevalue":
                                        if (argc != 3) { throw ArgCountError(gprop, "3", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            bool exist = (gprop == "ekey") ? vd.ContainsKey(queryStr) : vd.ContainsValue(queryStr);
                                            val = exist ? args[1] : args[2];
                                        }
                                        break;
                                    case "count": // count(value)
                                        {
                                            if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                            lock (store.Dict)
                                            {
                                                VariableDictionary vd = GetDictVariable(store, gname, false);
                                                val = vd.Count(args[0]).ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "keyof":
                                        if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            val = vd.KeyOf(queryStr);
                                        }
                                        break;
                                    case "keysof":
                                        if (argc != 1 && argc != 2) { throw ArgCountError(gprop, "1-2", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            string joiner = GetArgument(args, 1, ",");
                                            val = vd.KeysOf(queryStr, joiner);
                                        }
                                        break;
                                    case "joinkeys":
                                    case "joinvalues":
                                        if (argc > 1) { throw ArgCountError(gprop, "0-1", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string joiner = GetArgument(args, 0, ",");
                                            val = (gprop == "joinkeys") ? vd.JoinKeys(joiner) : vd.JoinValues(joiner);
                                        }
                                        break;
                                    case "joinall":
                                        if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string kvjoiner = GetArgument(args, 0, "=");
                                            string pairjoiner = GetArgument(args, 1, ",");
                                            val = vd.JoinAll(kvjoiner, pairjoiner);
                                        }
                                        break;
                                    case "sumkeys":
                                    case "sum": // sum values
                                        if (argc > 0) { throw ArgCountError(gprop, "0", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            double sum = (gprop == "sumkeys") ? vd.SumKeys() : vd.Sum();
                                            val = I18n.ThingToString(sum);
                                        }
                                        break;
                                    case "minkey":
                                    case "maxkey":
                                    case "min":  // dvar:list.min(type = "n")  num = "n" / str = "s" / hex = "h"
                                    case "max":
                                        {
                                            if (argc > 1) { throw ArgCountError(gprop, "0-1", argc, x); }
                                            ExtremumInit(args, gprop, x, out string type, out bool isMin);
                                            List<string> strings;
                                            lock (store.Dict)
                                            {
                                                VariableDictionary vd = GetDictVariable(store, gname, false);
                                                strings = (gprop.EndsWith("key")) ? vd.Values.Keys.ToList()
                                                                                  : vd.Values.Values.Select(var => var.ToString()).ToList();
                                            }
                                            val = ExtremumGetResult(strings, type, isMin, gname, gprop, x);
                                            found = true;
                                        }
                                        break;
                                }
                                found = true;
                            }
                            else
                            {
                                mx = rexListIdx.Match(varname);
                                if (mx.Success)
                                {
                                    string gname = mx.Groups["name"].Value;
                                    string gindex = mx.Groups["index"].Value;
                                    if (x.StartsWith("?"))
                                    {
                                        var vd = VariableDictionary.BuildTemp(gname);
                                        val = vd.GetValue(gindex).ToString();
                                    }
                                    else
                                    {
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            val = vd.GetValue(gindex).ToString();
                                        }
                                    }
                                    found = true;
                                }
                            }
                        }
                        // retrieve table variable value
                        else if (x.StartsWith("tvar:") || x.StartsWith("t:")
                              || x.StartsWith("ptvar:") || x.StartsWith("pt:")
                              || x.StartsWith("?tvar:") || x.StartsWith("?t:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables
                                                : x.StartsWith("t") ? plug.sessionvars : new VariableStore();
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexMethod.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gprop = mx.Groups["prop"].Value;
                                string[] args = SplitArguments(mx.Groups["arg"].Value);
                                int argc = args.Length;
                                if (x.StartsWith("?"))
                                {   // build temp var
                                    store.Table["?tvar"] = VariableTable.BuildTemp(gname);
                                    gname = "?tvar";
                                }
                                switch (gprop)
                                {
                                    case "w":
                                    case "width":
                                        {
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                val = vt.Width.ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "h":
                                    case "height":
                                        {
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                val = vt.Height.ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "hjoin": // .hjoin(joiner1 = ",", joiner2 = LINEBREAK_PLACEHOLDER, colSlices = ":", rowSlices = ":")
                                    case "vjoin": // .vjoin(joiner1 = ",", joiner2 = LINEBREAK_PLACEHOLDER, colslices = ":", rowslices = ":")
                                        if (argc > 4) { throw ArgCountError(gprop, "0-4", argc, x); }
                                        lock (store.Table)
                                        {
                                            VariableTable vt = GetTableVariable(store, gname, false);
                                            string joiner1 = GetArgument(args, 0, ",", false);
                                            string joiner2 = GetArgument(args, 1, LINEBREAK_PLACEHOLDER.ToString(), false);
                                            string colSlicesStr = GetArgument(args, 2, ":");
                                            string rowSlicesStr = GetArgument(args, 3, ":");
                                            var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                            var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                            if (gprop.StartsWith("hj"))
                                            {
                                                val = vt.HJoin(joiner1, joiner2, colIndices, rowIndices);
                                            }
                                            else
                                            {
                                                val = vt.VJoin(joiner1, joiner2, colIndices, rowIndices);
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "hl":
                                    case "hlookup": // .hlookup(targetStr, rowIndex, colslices = ":") => colIndex
                                    case "vl":
                                    case "vlookup": // .vlookup(targetStr, colIndex, rowslices = ":") => rowIndex
                                        if (argc != 2 && argc != 3) { throw ArgCountError(gprop, "2-3", argc, x); }
                                        lock (store.Table)
                                        {
                                            VariableTable vt = GetTableVariable(store, gname, false);
                                            string targetStr = args[0];
                                            if (!int.TryParse(args[1], NSFloat, InvClt, out int rawIndex))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), args[1], I18n.TranslateWord("int"), x);
                                            }
                                            int maxLength = (gprop.StartsWith("hl")) ? vt.Height : vt.Width;
                                            int index = (rawIndex < 0) ? (rawIndex + maxLength) : (rawIndex - 1);
                                            string slicesStr = GetArgument(args, 2, ":");

                                            List<int> indices;
                                            if (gprop.StartsWith("hl"))
                                            {
                                                indices = GetSliceIndices(slicesStr, vt.Width, x, startIndex: 1);
                                                val = vt.HLookup(targetStr, index, indices).ToString();
                                            }
                                            else
                                            {
                                                indices = GetSliceIndices(slicesStr, vt.Height, x, startIndex: 1);
                                                val = vt.VLookup(targetStr, index, indices).ToString();
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "count": // count(targetStr, colslices = ":", rowslices = ":")
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(gprop, "1-3", argc, x); }
                                            var colSlicesStr = GetArgument(args, 1, ":");
                                            var rowSlicesStr = GetArgument(args, 2, ":");
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                val = vt.Count(args[0], colIndices, rowIndices).ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "sum":
                                        {
                                            if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                            var colSlicesStr = GetArgument(args, 0, ":");
                                            var rowSlicesStr = GetArgument(args, 1, ":");
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                val = I18n.ThingToString(vt.Sum(colIndices, rowIndices));
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "min":  // tvar:table.min(type = "n", colSlices = ":", rowSlices = ":")  num = "n" / str = "s" / hex = "h"
                                    case "max":
                                        {
                                            if (argc > 3) { throw ArgCountError(gprop, "0-3", argc, x); }
                                            ExtremumInit(args, gprop, x, out string type, out bool isMin);
                                            string colSlicesStr = GetArgument(args, 1, ":");
                                            string rowSlicesStr = GetArgument(args, 2, ":");
                                            List<string> strings;
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                strings = rowIndices.SelectMany(
                                                    row => colIndices.Select(col => vt.Rows[row].Values[col].ToString())
                                                    ).ToList();
                                            }
                                            val = ExtremumGetResult(strings, type, isMin, gname, gprop, x);
                                            found = true;
                                        }
                                        break;
                                    case "contain":
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(gprop, "1-3", argc, x); }
                                            string colSlicesStr = GetArgument(args, 1, ":");
                                            string rowSlicesStr = GetArgument(args, 2, ":");
                                            lock (store.Dict)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                List<int> colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                List<int> rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                val = (colIndices.Any(col => rowIndices.Any(
                                                    row => vt.Rows[row].Values[col].ToString() == args[0])
                                                )) ? "1" : "0";
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "ifcontain":
                                        if (argc != 3) { throw ArgCountError(gprop, "3", argc, x); }
                                        lock (store.Table)
                                        {
                                            VariableTable vt = GetTableVariable(store, gname, false);
                                            val = vt.Rows.SelectMany(row => row.Values).Any(cell => cell.ToString() == args[0])
                                                ? args[1] : args[2];
                                            found = true;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                mx = rexTableIdx.Match(varname);
                                if (mx.Success)
                                {
                                    string gname = mx.Groups["name"].Value;
                                    string gcol = mx.Groups["column"].Value;
                                    string grow = mx.Groups["row"].Value;
                                    gcol = (gcol == "last") ? "-1" : gcol;
                                    grow = (grow == "last") ? "-1" : grow;

                                    if (x.StartsWith("?"))
                                    {   // build temp var
                                        store.Table["?tvar"] = VariableTable.BuildTemp(gname);
                                        gname = "?tvar";
                                    }
                                    if (!Int32.TryParse(gcol, NSFloat, InvClt, out int xindex))
                                    {
                                        throw ParseTypeError(I18n.TranslateWord("index"), gcol, I18n.TranslateWord("int"), x);
                                    }
                                    if (!Int32.TryParse(grow, NSFloat, InvClt, out int yindex))
                                    {
                                        throw ParseTypeError(I18n.TranslateWord("index"), grow, I18n.TranslateWord("int"), x);
                                    }
                                    lock (store.Table)
                                    {
                                        VariableTable vt = GetTableVariable(store, gname, false);
                                        val = vt.Peek(xindex, yindex).ToString();
                                    }
                                    found = true;
                                }
                            }
                        }
                        // row-based table lookup
                        else if (x.StartsWith("tvarrl:") || x.StartsWith("ptvarrl:") || x.StartsWith("trl:") || x.StartsWith("ptrl:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexTableIdx.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gheader = mx.Groups["column"].Value;
                                string gindex = mx.Groups["row"].Value;
                                gindex = (gindex == "last") ? "-1" : gindex;

                                if (!Int32.TryParse(gindex, NSFloat, InvClt, out int xindex))
                                {
                                    throw ParseTypeError(I18n.TranslateWord("index"), gindex, I18n.TranslateWord("int"), x);
                                }
                                lock (store.Table)
                                {
                                    VariableTable vt = GetTableVariable(store, gname, false);
                                    int yindex = vt.SeekRow(gheader);
                                    if (yindex > 0)
                                    {
                                        val = vt.Peek(xindex + (xindex >= 0 ? 1 : 0), yindex).ToString();
                                    }
                                }
                                found = true;
                            }
                        }
                        // column-based table lookup
                        else if (x.StartsWith("tvarcl:") || x.StartsWith("ptvarcl:") || x.StartsWith("tcl:") || x.StartsWith("ptcl:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexTableIdx.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gheader = mx.Groups["column"].Value;
                                string gindex = mx.Groups["row"].Value;
                                gindex = (gindex == "last") ? "-1" : gindex;
                                if (!Int32.TryParse(gindex, NSFloat, InvClt, out int yindex))
                                {
                                    throw ParseTypeError(I18n.TranslateWord("index"), gindex, I18n.TranslateWord("int"), x);
                                }
                                lock (store.Table)
                                {
                                    VariableTable vt = GetTableVariable(store, gname, false);
                                    // index starts from 1; 0 if not found
                                    int xindex = vt.SeekColumn(gheader);
                                    if (xindex > 0)
                                    {
                                        val = vt.Peek(xindex, yindex + (yindex >= 0 ? 1 : 0)).ToString();
                                    }
                                }
                                found = true;
                            }
                        }
                        // double-based lookup based on col/row names
                        else if (x.StartsWith("tvardl:") || x.StartsWith("ptvardl:") || x.StartsWith("tdl:") || x.StartsWith("ptdl:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexTableIdx.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string colHeader = mx.Groups["column"].Value;
                                string rowHeader = mx.Groups["row"].Value;

                                lock (store.Table)
                                {
                                    VariableTable vt = GetTableVariable(store, gname, false);

                                    // index starts from 1; 0 if not found
                                    int xindex = vt.SeekColumn(colHeader);
                                    int yindex = vt.SeekRow(rowHeader);

                                    if (xindex > 0 && yindex > 0)
                                    {
                                        val = vt.Peek(xindex, yindex).ToString();
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("env:"))
                        {
                            string envVarName = x.Substring(4);
                            Folder f = trig?.Parent;
                            string envVarValue = "";
                            while (f != null)
                            {
                                if (f.EnvironmentVariables.TryGetValue(envVarName, out envVarValue))
                                {
                                    break;
                                }
                                f = f.Parent;
                            }
                            val = envVarValue;
                            found = true;
                        }
                        else if (x.StartsWith("numeric:") || x.StartsWith("n:"))
                        {
                            string numexpr = (x.StartsWith("numeric:")) ? x.Substring(8) : x.Substring(2);
                            val = I18n.ThingToString(EvaluateNumericExpression(logger, o, numexpr));
                            found = true;
                        }
                        else if (x.StartsWith("string:") || x.StartsWith("s:"))
                        {
                            string strexpr = (x.StartsWith("string:")) ? x.Substring(7) : x.Substring(2);
                            val = EvaluateStringExpression(logger, o, strexpr);
                            found = true;
                        }
                        else if (x.StartsWith("if:"))
                        {
                            string ternaryExpr = x.Substring(3);
                            ParseTernaryExpression(ternaryExpr, out string condExpr, out string trueStr, out string falseStr);
                            if (trueStr == null || falseStr == null)
                            {
                                throw new Exception(I18n.Translate("internal/Context/ternaryexpressionerror",
                                    "Ternary expression ({0}) could not be parsed: \r\nCondition: ({1}); \r\nTrueExpr: ({2}); \r\nFalseExpr: ({3})",
                                    ternaryExpr, condExpr, trueStr ?? "null", falseStr ?? "null"));
                            }
                            bool cond = !MathParser.IsZero(MathParser.Parse(condExpr));
                            val = cond ? trueStr : falseStr;
                            found = true;
                        }
                        else if (x.StartsWith("func:") || x.StartsWith("f:"))
                        {
                            val = "";
                            found = true;
                            string funcexpr = (x.StartsWith("func:")) ? x.Substring(5) : x.Substring(2);
                            Match rxm = rexFunc.Match(funcexpr);
                            if (rxm.Success)
                            {
                                string funcname = Trim(rxm.Groups["name"].Value.ToLower());
                                string funcarg = rxm.Groups["arg"].Value;
                                string funcval = rxm.Groups["val"].Value;
                                string[] args = SplitArguments(funcarg);
                                int argc = args.Count();
                                switch (funcname)
                                {
                                    case "toupper": val = funcval.ToUpper(); break;
                                    case "tolower": val = funcval.ToLower(); break;
                                    case "tofullwidth": val = ToFullWidth(funcval); break;
                                    case "tohalfwidth": val = ToHalfWidth(funcval); break;
                                    case "toxivchar": // old name
                                    case "toblackchar":
                                        {
                                            if (!bool.TryParse(GetArgument(args, 0, "false"), out bool combineDigits))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), args[0], I18n.TranslateWord("bool"), x);
                                            }
                                            val = ToXivBlackChar(funcval, combineDigits);
                                        }
                                        break;
                                    case "towhitechar": val = ToXivWhiteChar(funcval); break;
                                    case "length": val = funcval.Length.ToString(); break;
                                    case "hex2dec":    // hex2dec()
                                    case "hex2float":  // hex2float()
                                    case "hex2double": // hex2double()
                                        {
                                            funcval = Trim(funcval);
                                            if (!new Regex("^[0-9A-Fa-f]+$").IsMatch(funcval))
                                            {
                                                throw InvalidValueError(funcname, "funcval", funcval, x);
                                            }
                                            switch (funcname)
                                            {
                                                case "hex2dec":
                                                    val = "" + Int64.Parse(funcval, NumberStyles.HexNumber, InvClt);
                                                    break;
                                                case "hex2float":
                                                    Int32 bytesArrayFloat = Int32.Parse(funcval, NumberStyles.HexNumber, InvClt);
                                                    val = "" + BitConverter.ToSingle(BitConverter.GetBytes(bytesArrayFloat), 0);
                                                    break;
                                                case "hex2double":
                                                    Int64 bytesArrayDouble = Int64.Parse(funcval, NumberStyles.HexNumber, InvClt);
                                                    val = "" + BitConverter.ToDouble(BitConverter.GetBytes(bytesArrayDouble), 0);
                                                    break;
                                            }
                                        }
                                        break;
                                    case "parsedmg": // parse the hex damage in ACT loglines to dec value
                                        {
                                            funcval = Trim(funcval);
                                            if (!reHex8.IsMatch(funcval))
                                            {
                                                throw InvalidValueError(funcname, "funcval", funcval, x);
                                            }
                                            val = MathParser.ParseDamage(funcval).ToString(InvClt);
                                        }
                                        break;
                                    case "float2hex":
                                        {
                                            if (!float.TryParse(funcval, NSFloat, InvClt, out float floatValue))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("float"), x);
                                            }
                                            byte[] bytesArray = BitConverter.GetBytes(floatValue);
                                            Array.Reverse(bytesArray, 0, bytesArray.Length);
                                            val = BitConverter.ToString(bytesArray).Replace("-", "");
                                        }
                                        break;
                                    case "double2hex":
                                        {
                                            if (!double.TryParse(funcval, NSFloat, InvClt, out double doubleValue))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("double"), x);
                                            }
                                            Int64 bytesArray = BitConverter.DoubleToInt64Bits(doubleValue);
                                            val = bytesArray.ToString("X");
                                        }
                                        break;
                                    case "dec2hex": // dec2hex()
                                    case "dec2hex2": // dec2hex2()
                                    case "dec2hex4": // dec2hex4()
                                    case "dec2hex8": // dec2hex8()
                                        {
                                            if (!Int64.TryParse(funcval, NSFloat, InvClt, out Int64 intValue))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("int"), x);
                                            }
                                            string format = funcname.Substring(6).ToUpper(); // "X" "X2" "X4" "X8"
                                            val = intValue.ToString(format);
                                        }
                                        break;
                                    case "ord": // chars => charcodes separated by separator
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            string separator = GetArgument(args, 0, ",");
                                            List<int> charcodes = new List<int>();
                                            for (int idx = 0; idx < funcval.Length; idx++)
                                            {
                                                if (char.IsHighSurrogate(funcval[idx]) && idx + 1 < funcval.Length && char.IsLowSurrogate(funcval[idx + 1]))
                                                {
                                                    charcodes.Add(char.ConvertToUtf32(funcval[idx++], funcval[idx]));
                                                }
                                                else
                                                {
                                                    charcodes.Add(funcval[idx]);
                                                }
                                            }
                                            val = string.Join(separator, charcodes);
                                        }
                                        break;
                                    case "chr": // charcodes separated by separator => chars
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            string separator = GetArgument(args, 0, ",");
                                            string[] rawCharcodes = SplitArguments(funcval, separator: separator);
                                            List<string> chars = new List<string>();
                                            for (int idx = 0; idx < rawCharcodes.Length; idx++)
                                            {
                                                if (int.TryParse(rawCharcodes[idx], out int charcode))
                                                {
                                                    chars.Add(char.ConvertFromUtf32(charcode));
                                                }
                                                else
                                                {
                                                    throw ParseTypeError($"#{idx}" + I18n.TranslateWord("string"), rawCharcodes[idx], I18n.TranslateWord("int"), x);
                                                }
                                            }
                                            val = string.Join("", chars);
                                        }
                                        break;
                                    case "padleft":
                                    case "padright":
                                        {
                                            if (argc != 2) { throw ArgCountError(funcname, "2", argc, x); }
                                            char paddingChar = (args[0].Length == 1) ? args[0][0] : GetReplacedChar(Int32.Parse(args[0], InvClt));
                                            int length = Int32.Parse(args[1], InvClt);

                                            if (funcname == "padleft")
                                                val = funcval.PadLeft(length, paddingChar);
                                            else
                                                val = funcval.PadRight(length, paddingChar);
                                        }
                                        break;
                                    case "repeat": // repeat(times, joiner = "")
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                            if (!Int32.TryParse(args[0], NSFloat, InvClt, out int times))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("times"), args[0], I18n.TranslateWord("int"), x);
                                            }
                                            string joiner = GetArgument(args, 1, "");
                                            if (times == 0)
                                            {
                                                val = "";
                                            }
                                            else
                                            {
                                                if (times < 0)
                                                {
                                                    times = -times;
                                                    funcval = new string(funcval.Reverse().ToArray());
                                                }
                                                StringBuilder sb = new StringBuilder(funcval);
                                                string repeatedUnit = joiner + funcval;
                                                for (int repeatCount = 1; repeatCount < times; repeatCount++)
                                                {
                                                    sb.Append(repeatedUnit);
                                                }
                                                val = $"{sb}";
                                            }
                                        }
                                        break;
                                    case "replace": // replace(oldStr, newStr = "", isLooped = false)
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(funcname, "1-3", argc, x); }

                                            string oldStr = args[0];
                                            if (oldStr == "") { throw InvalidValueError(funcname, "oldString", oldStr, x); }

                                            string newStr = GetArgument(args, 1, "");
                                            if (newStr == oldStr) { break; }

                                            string isLoopedStr = GetArgument(args, 2, "false");
                                            if (!bool.TryParse(isLoopedStr, out bool isLooped))
                                            {
                                                throw ParseTypeError("isLooped", isLoopedStr, I18n.TranslateWord("bool"), x);
                                            }
                                            if (newStr.Contains(oldStr) && isLooped)
                                            {
                                                throw InfiniteRepeatError(newStr, oldStr, x);
                                            }

                                            val = funcval.Replace(oldStr, newStr);
                                            while (val.Contains(oldStr) && isLooped)
                                            {
                                                val = val.Replace(oldStr, newStr);
                                            }
                                        }
                                        break;
                                    case "substring": // substring(startindex, length) or substring(startindex)
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                            if (!int.TryParse(args[0], NSFloat, InvClt, out int startIndex))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("startindex"), args[0], I18n.TranslateWord("int"), x);
                                            }
                                            if (startIndex < 0)
                                            {
                                                startIndex += funcval.Length;
                                            }
                                            switch (argc)
                                            {
                                                case 1:
                                                    val = funcval.Substring(startIndex);
                                                    break;
                                                case 2:
                                                    if (!int.TryParse(args[1], NSFloat, InvClt, out int length))
                                                    {
                                                        throw ParseTypeError(I18n.TranslateWord("length"), args[1], I18n.TranslateWord("int"), x);
                                                    }
                                                    val = funcval.Substring(startIndex, length);
                                                    break;
                                            }
                                            break;
                                        }
                                    case "slice":  // slice(slices = ":") 
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            string slicesStr = GetArgument(args, 0, ":");
                                            var indices = GetSliceIndices(slicesStr, funcval.Length, x, startIndex: 0);
                                            StringBuilder sb = new StringBuilder();
                                            foreach (int index in indices)
                                            {
                                                sb.Append(funcval[index]);
                                            }
                                            val = $"{sb}";
                                            break;
                                        }
                                    case "pick": // pick(index, splitter = ",")
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                            string separator = GetArgument(args, 1, ",");
                                            string[] strArray = SplitArguments(funcval, separator: separator);
                                            if (!int.TryParse(args[0], NSFloat, InvClt, out int index))
                                            { throw ParseTypeError(I18n.TranslateWord("index"), args[0], I18n.TranslateWord("int"), x); }

                                            int normIndex = (index < 0) ? (index + strArray.Length) : index;
                                            val = (normIndex >= 0 && normIndex < strArray.Length)
                                                ? strArray[normIndex] : "";
                                        }
                                        break;
                                    case "args": // for testing the argument splitting: ${f:args(...):}
                                        val = "(" + string.Join(")\n(", args) + ")";
                                        break;
                                    case "i":
                                    case "indexof":      // indexof(stringtosearch)
                                    case "lastindexof":  // lastindexof(stringtosearch)
                                        {
                                            if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                            int index = (funcname.StartsWith("i")) ? funcval.IndexOf(args[0]) : funcval.LastIndexOf(args[0]);
                                            val = I18n.ThingToString(index);
                                        }
                                        break;
                                    case "indicesof":
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(funcname, "1-3", argc, x); }
                                            string targetStr = args[0];
                                            int subLength = targetStr.Length;
                                            int totalLength = funcval.Length;
                                            string joiner = GetArgument(args, 1, defaultValue: ",");
                                            string slicesStr = GetArgument(args, 2, defaultValue: ":");
                                            List<int> indices = GetSliceIndices(slicesStr, totalLength - subLength + 1, x, startIndex: 0);
                                            StringBuilder sb = new StringBuilder();
                                            foreach (int idx in indices)
                                            {
                                                if (funcval.Substring(idx, subLength) == targetStr)
                                                {
                                                    if (sb.Length > 0)
                                                        sb.Append(joiner);
                                                    sb.Append(idx);
                                                }
                                            }
                                            val = $"{sb}";
                                            break;
                                        }
                                    case "compare": // compare(stringtocompare) or compare(stringtocompare, ignorecase)
                                        if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                        string ignoreCaseStr = GetArgument(args, 1, "true");
                                        if (!bool.TryParse(ignoreCaseStr, out bool ignoreCase))
                                        {
                                            throw ParseTypeError("ignoreCase", ignoreCaseStr, I18n.TranslateWord("bool"), x);
                                        }
                                        val = "" + String.Compare(funcval, args[0], ignoreCase);
                                        break;
                                    case "versioncompare": // ${f:versioncompare(1.2.0.0):1.1.8.0} = -1
                                        if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                        Version srcVersion = Version.TryParse(funcval, out Version v)
                                            ? v : throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("version"), x);
                                        Version tgtVersion = Version.TryParse(args[0], out v)
                                            ? v : throw ParseTypeError(I18n.TranslateWord("string"), args[0], I18n.TranslateWord("version"), x);
                                        val = I18n.ThingToString(srcVersion.CompareTo(tgtVersion));
                                        break;
                                    case "contain":
                                    case "startwith":
                                    case "endwith":
                                    case "equal":
                                        {
                                            if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                            switch (funcname)
                                            {
                                                case "contain": val = (funcval.Contains(args[0])) ? "1" : "0"; break;
                                                case "startwith": val = (funcval.StartsWith(args[0])) ? "1" : "0"; break;
                                                case "endwith": val = (funcval.EndsWith(args[0])) ? "1" : "0"; break;
                                                case "equal": val = (args[0] == funcval) ? "1" : "0"; break;
                                            }
                                        }
                                        break;
                                    case "ifcontain":
                                    case "ifstartwith":
                                    case "ifendwith":
                                    case "ifequal":
                                        {
                                            if (argc != 3) { throw ArgCountError(funcname, "3", argc, x); }
                                            switch (funcname)
                                            {
                                                case "ifcontain": val = (funcval.Contains(args[0])) ? args[1] : args[2]; break;
                                                case "ifstartwith": val = (funcval.StartsWith(args[0])) ? args[1] : args[2]; break;
                                                case "ifendwith": val = (funcval.EndsWith(args[0])) ? args[1] : args[2]; break;
                                                case "ifequal": val = (args[0] == funcval) ? args[1] : args[2]; break;
                                            }
                                        }
                                        break;
                                    case "match": // func:match(str):regex
                                        {
                                            if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                            Match match = new Regex(UnescapeCustomExpr(funcval)).Match(args[0]);
                                            val = (match.Success) ? "1" : "0";
                                        }
                                        break;
                                    case "capture": // func:capture(str, group):regex
                                        {
                                            if (argc != 2) { throw ArgCountError(funcname, "2", argc, x); }
                                            Match match = new Regex(UnescapeCustomExpr(funcval)).Match(args[0]);
                                            if (int.TryParse(args[1], NSFloat, InvClt, out int groupNumber))
                                            {
                                                if (groupNumber >= 0 && groupNumber < match.Groups.Count)
                                                {
                                                    val = (match.Success) ? match.Groups[groupNumber].Value : "";
                                                    break;
                                                }
                                            }
                                            val = (match.Success) ? match.Groups[args[1]].Value : "";
                                        }
                                        break;
                                    case "ifmatch": // func:ifmatch(str, successStr, failStr):regex
                                        {
                                            if (argc != 3) { throw ArgCountError(funcname, "3", argc, x); }
                                            Match match = new Regex(UnescapeCustomExpr(funcval)).Match(args[0]);
                                            val = (match.Success) ? args[1] : args[2];
                                        }
                                        break;
                                    case "trim":        // trim() or trim(charcode/char, charcode/char, ...)
                                    case "trimleft":    // trimleft() or trimleft(charcode/char, charcode/char, ...)
                                    case "trimright":   // trimright() or trimright(charcode/char, charcode/char, ...)
                                        string trimChars = "";
                                        if (argc > 0)
                                        {
                                            foreach (string arg in args)
                                            {
                                                // length == 1: char    length != 1: charcode
                                                if (arg.Length == 1)
                                                {
                                                    trimChars += arg;
                                                }
                                                else if (arg.Length == 0)
                                                {
                                                    throw InvalidValueError(funcname, I18n.TranslateWord("char") + "/" + I18n.TranslateWord("charcode"), arg, x);
                                                }
                                                else if (arg.Length > 1)
                                                {
                                                    if (!int.TryParse(arg, NSFloat, InvClt, out int charcode))
                                                    {
                                                        throw ParseTypeError(I18n.TranslateWord("charcode"), arg, I18n.TranslateWord("int"), x);
                                                    }
                                                    trimChars += GetReplacedChar(charcode).ToString();
                                                }
                                            }
                                        }
                                        char[] trimCharsArray = trimChars.ToCharArray();

                                        switch (funcname)
                                        {
                                            case "trim":
                                                val = argc == 0 ? Trim(funcval) : funcval.Trim(trimCharsArray);
                                                break;
                                            case "trimleft":
                                                val = argc == 0 ? TrimL(funcval) : funcval.TrimStart(trimCharsArray);
                                                break;
                                            case "trimright":
                                                val = argc == 0 ? TrimR(funcval) : funcval.TrimEnd(trimCharsArray);
                                                break;
                                        }
                                        break;
                                    case "format": // format(type,formatstring)
                                        if (argc != 2) { throw ArgCountError(funcname, "2", argc, x); }
                                        else
                                        {
                                            Type type = Type.GetType(args[0]);
                                            object converted = Convert.ChangeType(funcval, type, InvClt);
                                            val = String.Format("{0:" + args[1] + "}", converted);
                                        }
                                        break;
                                    case "utctime": // utctime(formatstring)
                                    case "localtime": // localtime(formatstring)
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            Int64 ts = Int64.Parse(funcval, InvClt);
                                            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                            dt = dt.AddSeconds(ts);
                                            if (funcname == "localtime")
                                            {
                                                dt = dt.ToLocalTime();
                                            }
                                            string format = GetArgument(args, 0, "");
                                            val = dt.ToString(format);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (x.StartsWith("_ffxivparty[") || x.StartsWith("_party[") ||
                                 x.StartsWith("_ffxiventity[") || x.StartsWith("_entity["))
                        {
                            mx = rexListProp.Match(x);
                            if (mx.Success)
                            {
                                bool isParty = x.StartsWith("_ffxivparty[") || x.StartsWith("_party[");
                                string key = Trim(mx.Groups["index"].Value);
                                string prop = Trim(mx.Groups["prop"].Value);
                                FFXIV.Entity entity;

                                if (isParty && int.TryParse(key, out int partyIdx) && partyIdx >= 1 && partyIdx <= 8)
                                {   // ffxivparty[n]
                                    string hexID = PluginBridges.BridgeFFXIV.GetPartyMember(partyIdx).GetValue("id").ToString();
                                    entity = hexID == "" ? BridgeFFXIV.XivEntity.NullEntity() : FFXIV.Entity.GetEntityByID(hexID);
                                    RealPlugin.plug.UnfilteredAddToLog(
                                        RealPlugin.DebugLevelEnum.Warning,
                                        partyIdx == 1 ? $"请使用 ${{_me.{prop}}} 代替 ${{_ffxivparty[1].{prop}}}。触发器：{trig.FullPath}"
                                                      : $"不推荐使用 ${{{x}}}，ACT 无法按顺序获取小队列表，请使用其他方式访问小队。触发器：{trig.FullPath}",
                                        this.trig);
                                }
                                else
                                {
                                    var entities = FFXIV.Entity.GetFilteredEntities(key);
                                    if (isParty)
                                    {
                                        entities = entities.Where(e => e.InParty);
                                    }
                                    entity = entities.FirstOrDefault();
                                }
                                if (entity == null)
                                {
                                    RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, I18n.Translate(
                                        "internal/Context/noEntity",
                                        "The queried entity does not exist: {0}. Trigger: ({1})",
                                        x, trig?.FullPath ?? "null"), trig);
                                }
                                else val = string.Join(", ", entity.QueryProperties(prop));
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_me.")) // ${_me.prop}
                        {
                            string prop = Trim(x.Substring(4));
                            if (prop.ToLower() == "id")
                            {
                                val = BridgeFFXIV.PlayerHexId;
                            }
                            else
                            {
                                val = string.Join(", ", FFXIV.Entity.GetMyself().QueryProperties(prop));
                            }
                        }
                        else if (x.StartsWith("_tgt.")) // ${_tgt.prop}
                        {   // just for simplifying the expression ${_entity[${_me.targetid}].prop}
                            string prop = Trim(x.Substring(5));
                            var targetID = FFXIV.Entity.GetMyself().TargetID;
                            FFXIV.Entity tgt = FFXIV.Entity.GetEntityByID(targetID)
                                ?? FFXIV.Entity.NullEntity();
                            val = string.Join(", ", tgt.QueryProperties(prop));
                        }
                        else if (x == "_clipboard")
                        {
                            val = Action.ClipboardGetText();
                            if (val.Contains("${_clipboard}"))
                            {
                                throw InfiniteClipboardError();
                            }
                            found = true;
                        }
                        else if (x == "_ffxivtime" || x == "_ET")
                        {
                            TimeSpan ez = GetEorzeanTime();
                            int mins = (int)Math.Floor(ez.TotalMinutes);
                            val = mins.ToString();
                            found = true;
                        }
                        else if (x == "_ETprecise")
                        {
                            TimeSpan ez = GetEorzeanTime();
                            val = ez.TotalMinutes.ToString();
                            found = true;
                        }
                        else if (x == "_lastencounter")
                        {
                            val = plug != null ? plug.LastEncounterHook() : "";
                            found = true;
                        }
                        else if (x == "_activeencounter")
                        {
                            val = plug != null ? plug.ActiveEncounterHook() : "";
                            found = true;
                        }
                        else if (x.StartsWith("_textaura"))
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string gindex = mx.Groups["index"].Value;
                                string gprop = mx.Groups["prop"].Value.ToLower();
                                val = "";
                                if (plug.sc != null)
                                {
                                    lock (plug.sc.textitems)
                                    {
                                        Scarborough.ScarboroughText item = plug.sc.GetText(gindex);
                                        if (item != null)
                                        {
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(item.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(item.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(item.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(item.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(item.Opacity);
                                                    break;
                                                case "text":
                                                    val = item.Text;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lock (plug.textauras)
                                    {
                                        if (plug.textauras.ContainsKey(gindex))
                                        {
                                            Forms.AuraContainerForm acf = plug.textauras[gindex];
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(acf.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(acf.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(acf.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(acf.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(acf.PresentableOpacity);
                                                    break;
                                                case "text":
                                                    val = acf.CurrentText;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_imageaura"))
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string gindex = mx.Groups["index"].Value;
                                string gprop = mx.Groups["prop"].Value.ToLower();
                                val = "";
                                if (plug.sc != null)
                                {
                                    lock (plug.sc.imageitems)
                                    {
                                        Scarborough.ScarboroughImage item = plug.sc.GetImage(gindex);
                                        if (item != null)
                                        {
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(item.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(item.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(item.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(item.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(item.Opacity);
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lock (plug.imageauras)
                                    {
                                        if (plug.imageauras.ContainsKey(gindex))
                                        {
                                            Forms.AuraContainerForm acf = plug.imageauras[gindex];
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(acf.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(acf.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(acf.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(acf.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(acf.PresentableOpacity);
                                                    break;
                                            }
                                        }
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x == "_screenwidth")
                        {
                            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.PrimaryScreen;
                            val = I18n.ThingToString(scr.WorkingArea.Width);
                            found = true;
                        }
                        else if (x == "_screenheight")
                        {
                            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.PrimaryScreen;
                            val = I18n.ThingToString(scr.WorkingArea.Height);
                            found = true;
                        }
                        else if (x == "_screenminx")
                        {
                            val = I18n.ThingToString(plug.MinX);
                            found = true;
                        }
                        else if (x == "_screenminy")
                        {
                            val = I18n.ThingToString(plug.MinY);
                            found = true;
                        }
                        else if (x == "_screenmaxx")
                        {
                            val = I18n.ThingToString(plug.MaxX);
                            found = true;
                        }
                        else if (x == "_screenmaxy")
                        {
                            val = I18n.ThingToString(plug.MaxY);
                            found = true;
                        }
                    }
                }
                newexpr = newexpr.Substring(0, m.Index) + val + newexpr.Substring(m.Index + m.Length);
                i++;
            };
            newexpr = ReplacePlaceholderBrackets(newexpr);

            // replace back linebreaks
            newexpr = newexpr.Replace(LINEBREAK_PLACEHOLDER.ToString(), Environment.NewLine);

            if (trig != null)
            {
                if (trig._DebugLevel == RealPlugin.DebugLevelEnum.Inherit)
                {
                    if (plug != null)
                    {
                        if (plug.cfg.DebugLevel < RealPlugin.DebugLevelEnum.Verbose)
                        {
                            return newexpr;
                        }
                    }
                }
                else
                {
                    if (trig._DebugLevel < RealPlugin.DebugLevelEnum.Verbose)
                    {
                        return newexpr;
                    }
                }
            }
            if (plug != null)
            {
                if (plug.cfg.LogVariableExpansions == false)
                {
                    return newexpr;
                }
            }
            if (newexpr.CompareTo(expr) != 0)
            {
                if (logger != null)
                {
                    logger(o, I18n.Translate("internal/Context/expansion", "Variable expansion from '{0}' to '{1}'", expr, newexpr));
                }
                else if (plug != null)
                {
                    plug.FilteredAddToLog(
                        RealPlugin.DebugLevelEnum.Verbose,
                        I18n.Translate("internal/Context/expansion", "Variable expansion from '{0}' to '{1}'", expr, newexpr),
                        this.trig);
                }
            }
            return newexpr;
        }


        private string ExpandBrackets(string expression)
        {
            Match m, mx;
            string newexpr = expression;
            newexpr = ReplaceLineBreak(newexpr); // replace back after parsed

            int i = 1;
            while (true)
            {
                m = rex.Match(newexpr);
                if (m.Success == false)
                {
                    m = rexNum.Match(newexpr);
                    if (m.Success == false)
                    {
                        break;
                    }
                }
                string x = m.Groups["id"].Value;
                string val = "";
                bool found = false;
                if (testByPlaceholder == true)
                {
                    if (x == "_since")
                    {
                        val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalSeconds)).ToString();
                    }
                    else if (x == "_sincems")
                    {
                        val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalMilliseconds)).ToString();
                    }
                    else
                    {
                        val = (numeric == true ? "1" : "test");
                    }
                    found = true;
                }
                else
                {
                    int gn;
                    if (Int32.TryParse(x, out gn) == true)
                    {
                        if (gn >= 0 && gn < numgroups.Count)
                        {
                            val = numgroups[gn];
                            if (plug != null)
                            {
                                val = plug.cfg.PerformSubstitution(val, Configuration.Substitution.SubstitutionScopeEnum.CaptureGroup);
                            }
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        if (namedgroups.ContainsKey(x) == true)
                        {
                            val = namedgroups[x];
                            if (plug != null)
                            {
                                val = plug.cfg.PerformSubstitution(val, Configuration.Substitution.SubstitutionScopeEnum.CaptureGroup);
                            }
                            found = true;
                        }
                    }
                    if (found == false)
                    {
                        Match matchExistVar;
                        if (x == "_since")
                        {
                            val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalSeconds)).ToString();
                            found = true;
                        }
                        else if (x == "_sincems")
                        {
                            val = ((int)Math.Floor((DateTime.UtcNow - triggered).TotalMilliseconds)).ToString();
                            found = true;
                        }
                        else if (x == "_systemtime")
                        {
                            val = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
                            found = true;
                        }
                        else if (x == "_systemtimems")
                        {
                            val = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
                            found = true;
                        }
                        else if (x == "_ffxivplayer" || x == "_me")
                        {
                            VariableDictionary vc = PluginBridges.BridgeFFXIV.GetMyself();
                            if (vc != null)
                            {
                                val = vc.GetValue("name").ToString();
                            }
                            found = true;
                        }
                        else if (x == "_ffxivzoneid")
                        {
                            if (zoneIdOverride != null)
                            {
                                val = zoneIdOverride;
                            }
                            else
                            {
                                val = PluginBridges.BridgeFFXIV.ZoneID.ToString();
                            }
                            found = true;
                        }
                        else if (x == "_ffxivpartyorder")
                        {
                            val = plug.cfg.FfxivPartyOrdering + " " + plug.cfg.FfxivCustomPartyOrder;
                            found = true;
                        }
                        else if (x == "_ffxivprocid")
                        {
                            val = PluginBridges.BridgeFFXIV.GetProcessId().ToString();
                            found = true;
                        }
                        else if (x == "_ffxivprocname")
                        {
                            val = PluginBridges.BridgeFFXIV.GetProcessName();
                            found = true;
                        }
                        else if (x == "_ffxivversion")
                        {
                            val = PluginBridges.BridgeFFXIV.GetGameVersion();
                            found = true;
                        }
                        else if (x == "_ffxivlanguage")
                        {
                            val = GameLanguage.Language.ToString();
                            found = true;
                        }
                        else if (x == "_ffxivisglobal")
                        {
                            val = (byte)GameLanguage.Language <= 3 ? "1" : "0";
                            found = true;
                        }
                        else if (x == "_ffxivincombat") // game status
                        {
                            val = ModuleInCombat.GetInCombat() ? "1" : "0";
                            found = true;
                        }
                        else if (x == "_incombat") // ACT status
                        {
                            val = plug != null && plug.InCombatHook() ? "1" : "0";
                            found = true;
                        }
                        else if (x == "_duration")
                        {
                            try
                            {
                                if (plug != null && plug.InCombatHook() == true)
                                {
                                    val = ((int)Math.Floor(plug.EncounterDurationHook())).ToString();
                                }
                                else
                                {
                                    val = "0";
                                }
                            }
                            catch (Exception)
                            {
                                val = "0";
                            }
                            found = true;
                        }
                        else if (x == "_response")
                        {
                            val = contextResponse;
                            found = true;
                        }
                        else if (x == "_responsecode")
                        {
                            val = contextResponseCode.ToString();
                            found = true;
                        }
                        else if (x == "_triggername")
                        {
                            val = trig?.Name ?? "(null)";
                            found = true;
                        }
                        else if (x == "_triggerid")
                        {
                            val = trig != null ? trig.Id.ToString() : "(null)";
                            found = true;
                        }
                        else if (x == "_triggerpath")
                        {
                            val = trig?.FullPath ?? "";
                            found = true;
                        }
                        else if (x == "_loopiterator" || x == "_i")
                        {
                            val = loopiterator.ToString();
                            found = true;
                        }
                        else if (x == "_idx")
                        {
                            val = listIndex.ToString();
                            found = true;
                        }
                        else if (x == "_col")
                        {
                            val = tableColIndex.ToString();
                            found = true;
                        }
                        else if (x == "_row")
                        {
                            val = tableRowIndex.ToString();
                            found = true;
                        }
                        else if (x.StartsWith("_col["))
                        {
                            string rowIndex = x.Substring(5, x.Length - 6);
                            val = $"${{{varName}[{tableColIndex}][{rowIndex}]}}";
                            found = true;
                        }
                        else if (x.StartsWith("_row["))
                        {
                            string colIndex = x.Substring(5, x.Length - 6);
                            val = $"${{{varName}[{colIndex}][{tableRowIndex}]}}";
                            found = true;
                        }
                        else if (x.StartsWith("_colrl["))
                        {
                            int colonIndex = varName.IndexOf(":");
                            string prefix = varName.Substring(0, colonIndex) + "dl" + varName.Substring(colonIndex);
                            string colHeader = $"${{{varName}[{tableColIndex}][1]}}";
                            string rowHeader = x.Substring(7, x.Length - 8);
                            val = $"${{{prefix}[{colHeader}][{rowHeader}]}}";
                            found = true;
                        }
                        else if (x.StartsWith("_rowcl["))
                        {
                            int colonIndex = varName.IndexOf(":");
                            string prefix = varName.Substring(0, colonIndex) + "dl" + varName.Substring(colonIndex);
                            string colHeader = x.Substring(7, x.Length - 8);
                            string rowHeader = $"${{{varName}[1][{tableRowIndex}]}}";
                            val = $"${{{prefix}[{colHeader}][{rowHeader}]}}";
                            found = true;
                        }
                        else if (x == "_key")
                        {
                            val = dictKey;
                            found = true;
                        }
                        else if (x == "_val")
                        {
                            val = dictValue;
                            found = true;
                        }
                        else if (x == "_this")
                        {
                            if (varName.StartsWith("tvar:") || varName.StartsWith("ptvar:"))
                            {
                                val = $"${{{varName}[{tableColIndex}][{tableRowIndex}]}}";
                            }
                            else
                            {
                                val = $"${{{varName}[{listIndex}]}}";
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_actionhistory"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = mx.Groups["index"].Value;
                                if (idx == "previous")
                                {
                                    val = PeekActionResult(true, 0).ToString();
                                }
                                else
                                {
                                    val = PeekActionResult(false, Int32.Parse(idx)).ToString();
                                }
                                found = true;
                            }
                        }
                        else if (x == "_configpath")
                        {
                            val = RealPlugin.plug.path;
                            found = true;
                        }
                        else if (x == "_pluginpath")
                        {
                            val = RealPlugin.plug.pluginPath;
                            found = true;
                        }
                        else if (x == "_pluginversion")
                        {
                            val = RealPlugin.plug.cfg.PluginVersion;
                            found = true;
                        }
                        else if (x.StartsWith("_env"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = mx.Groups["index"].Value;
                                val = System.Environment.GetEnvironmentVariable(idx);
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_offset["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string key = Trim(mx.Groups["index"].Value).ToLower();
                                if (key == "1b")
                                    val = I18n.ThingToString(Memory.Offset1B);
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_targetmarker2id[") || x.StartsWith("_tm2id["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string key = Trim(mx.Groups["index"].Value);
                                uint id = Memory.EntityIdByTargetMarker(key) ?? throw ParseTypeError(I18n.TranslateWord("string"), key, "targetmarker", x);
                                val = id.ToString("X8");
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_wm[") || x.StartsWith("_waymark["))
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string rawType = Trim(mx.Groups["index"].Value);
                                string rawProp = Trim(mx.Groups["prop"].Value);
                                val = Memory.Waymark.QueryWaymark(rawType, rawProp);
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_storage["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string key = Trim(mx.Groups["index"].Value);
                                Dictionary<string, object> storage = plug.scriptingStorage;

                                if (!storage.ContainsKey(key))
                                    val = "";
                                else
                                {
                                    object item = storage[key];
                                    Type type = item.GetType();
                                    if (type.IsPrimitive && type != typeof(char) || type == typeof(decimal) || type == typeof(string))
                                        val = Convert.ChangeType(item, type, InvClt).ToString();
                                    else
                                        val = item.ToString();
                                }
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_job[")) // ${_job[jobid].prop} or ${_job[Name].prop}
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string rawJob = Trim(mx.Groups["index"].Value);
                                string prop = mx.Groups["prop"].Value;
                                val = Job.GetJob(rawJob).QueryProperty(prop);
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_const"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = Trim(mx.Groups["index"].Value);
                                lock (plug.cfg.Constants)
                                {
                                    if (plug.cfg.Constants.ContainsKey(idx))
                                    {
                                        val = plug.cfg.Constants[idx].Value;
                                    }
                                    else
                                    {
                                        val = "";
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_config["))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string idx = Trim(mx.Groups["index"].Value);
                                switch (idx)
                                {
                                    case "DebugLevel": val = ((int)plug.cfg.DebugLevel).ToString(InvClt); break;
                                    case "UseACTForSound": val = plug.cfg.SoundMethod == Configuration.AudioRoutingMethodEnum.ACT ? "1" : "0"; break;
                                    case "UseACTForTTS": val = plug.cfg.TtsMethod == Configuration.AudioRoutingMethodEnum.ACT ? "1" : "0"; break;
                                    case "FfxivLogNetwork": val = plug.cfg.FfxivLogNetwork ? "1" : "0"; break;
                                    case "UseOsClipboard": val = "1"; break;
                                    case "DeveloperMode": val = plug.cfg.DeveloperMode ? "1" : "0"; break;
                                    case "AutoComplete": val = plug.cfg.AutoComplete ? "1" : "0"; break;
                                    case "Autosave": val = plug.cfg.AutosaveEnabled ? plug.cfg.AutosaveInterval.ToString(InvClt) : "0"; break;
                                    case "Language": val = plug.cfg.Language; break;
                                    case "UnsafeUsage": val = ((int)plug.cfg.UnsafeUsage).ToString(); break;
                                    default:
                                        try
                                        {
                                            foreach (var api in plug.cfg.GetAPIUsages())
                                            {
                                                if (api.Name == idx)
                                                    val = ((api.AllowLocal ? 1 : 0) + (api.AllowRemote ? 2 : 0) + (api.AllowAdmin ? 4 : 0)).ToString();
                                            }
                                        }
                                        catch { throw InvalidValueError("_config", I18n.TranslateWord("key"), idx, x); }
                                        break;
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_jsonresponse"))
                        {
                            mx = rexListIdx.Match(x);
                            if (mx.Success)
                            {
                                string pathspec = mx.Groups["index"].Value;
                                if (contextJsonParsed == false)
                                {
                                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                                    contextJsonResponse = jsonSerializer.Deserialize<dynamic>(contextResponse);
                                    contextJsonParsed = true;
                                }
                                string[] path = pathspec.Split("/".ToCharArray());
                                dynamic curdir = contextJsonResponse;
                                foreach (string px in path)
                                {
                                    curdir = curdir[px];
                                }
                                val = curdir.ToString();
                                found = true;
                            }
                        }
                        // check if variable exists (combined the logic for all types of variable expressions)
                        // evar  = ev, epvar  = epv;    elvar = el, eplvar = epl;
                        // etvar = et, eptvar = ept;    edvar = ed, epdvar = epd;
                        // etext, eimage for Auras;     ecallback for named callbacks;   estorage for script storage
                        else if ((matchExistVar = rexExistVar.Match(x)).Success)
                        {
                            bool persist = matchExistVar.Groups["persist"].Value == "p";
                            string type = matchExistVar.Groups["type"].Value;
                            string varname = matchExistVar.Groups["name"].Value;
                            VariableStore store = persist ? plug.cfg.PersistentVariables : plug.sessionvars;
                            dynamic source = null;
                            switch (type)
                            {
                                case "v": source = store.Scalar; break;
                                case "l": source = store.List; break;
                                case "t": source = store.Table; break;
                                case "d": source = store.Dict; break;
                                case "text":
                                    if (plug.sc != null)
                                        source = plug.sc.textitems;
                                    else
                                        source = plug.textauras;
                                    break;
                                case "image":
                                    if (plug.sc != null)
                                        source = plug.sc.imageitems;
                                    else
                                        source = plug.imageauras;
                                    break;
                                case "callback": source = plug.callbacksByName; break;
                                case "storage": source = plug.scriptingStorage; break;
                            }
                            lock (source)
                            {
                                val = (source == null) ? "" : source.ContainsKey(varname) ? "1" : "0";
                            }
                            found = true;
                        }
                        // retrieve scalar variable value
                        else if (x.StartsWith("var:") || x.StartsWith("pvar:") || x.StartsWith("v:") || x.StartsWith("pv:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            lock (store.Scalar) // verified
                            {
                                VariableScalar vs = GetScalarVariable(store, varname, false);
                                val = vs.Value;
                            }
                            found = true;
                        }
                        // retrieve list variable value
                        else if (x.StartsWith("lvar:") || x.StartsWith("l:")
                              || x.StartsWith("plvar:") || x.StartsWith("pl:")
                              || x.StartsWith("?lvar:") || x.StartsWith("?l:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables
                                                : x.StartsWith("l") ? plug.sessionvars : new VariableStore();
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexMethod.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gprop = mx.Groups["prop"].Value;
                                string garg = mx.Groups["arg"].Value;
                                string[] args = SplitArguments(garg);
                                int argc = args.Length;
                                if (x.StartsWith("?"))
                                {   // build temp var
                                    store.List["?lvar"] = VariableList.BuildTemp(gname);
                                    gname = "?lvar";
                                }
                                switch (gprop)
                                {
                                    case "size":
                                    case "length":
                                        lock (store.List)
                                        {
                                            VariableList vl = GetListVariable(store, gname, false);
                                            val = vl.Size.ToString();
                                        }
                                        found = true;
                                        break;
                                    case "indexof":
                                    case "i":
                                    case "lastindexof":
                                        if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                        lock (store.List)
                                        {
                                            VariableList vl = GetListVariable(store, gname, false);
                                            val = (gprop.StartsWith("i")) ? vl.IndexOf(args[0]).ToString() : vl.LastIndexOf(args[0]).ToString();
                                        }
                                        found = true;
                                        break;
                                    case "indicesof":
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(gprop, "1-3", argc, x); }
                                            string joiner = GetArgument(args, 1, defaultValue: ",");
                                            string slicesStr = GetArgument(args, 2, defaultValue: ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = vl.IndicesOf(args[0], joiner, indices);
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "sum":  // lvar:list.sum(slices = ":")
                                        {
                                            if (argc > 1) { throw ArgCountError(gprop, "0-1", argc, x); }
                                            string slicesStr = GetArgument(args, 0, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = I18n.ThingToString(vl.Sum(indices));
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "min":  // lvar:list.min(type = "n", slices = ":")  num = "n" / str = "s" / hex = "h"
                                    case "max":
                                        {
                                            if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                            ExtremumInit(args, gprop, x, out string type, out bool isMin);
                                            string slicesStr = GetArgument(args, 1, ":");
                                            List<string> strings;
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                strings = vl.Values.Where((_, idx) => indices.Contains(idx)).Select(var => var.ToString()).ToList();
                                            }
                                            val = ExtremumGetResult(strings, type, isMin, gname, gprop, x);
                                            found = true;
                                        }
                                        break;
                                    case "join":        // lvar:list.join(joiner = ",", slices = ":")
                                    case "randjoin":    // lvar:list.randjoin(joiner = ",", slices = ":")
                                        {
                                            if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                            string joiner = GetArgument(args, 0, ",");
                                            string slicesStr = GetArgument(args, 1, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                List<int> indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                if (gprop == "randjoin")
                                                {
                                                    indices = indices.OrderBy(_ => rng.Next()).ToList();
                                                }
                                                val = vl.Join(joiner, indices);
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "count": // count(targetStr, slices = ":")
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(gprop, "1-2", argc, x); }
                                            var slicesStr = GetArgument(args, 1, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                var indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = vl.Count(args[0], indices).ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "contain":
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(gprop, "1-2", argc, x); }
                                            string slicesStr = GetArgument(args, 1, ":");
                                            lock (store.List)
                                            {
                                                VariableList vl = GetListVariable(store, gname, false);
                                                List<int> indices = GetSliceIndices(slicesStr, vl.Size, x, startIndex: 1);
                                                val = (indices.Any(idx => vl.Values[idx].ToString() == args[0])) ? "1" : "0";
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "ifcontain":
                                        if (argc != 3) { throw ArgCountError(gprop, "3", argc, x); }
                                        lock (store.List)
                                        {
                                            VariableList vl = GetListVariable(store, gname, false);
                                            val = (vl.Values.Any(v => v.ToString() == args[0])) ? args[1] : args[2];
                                            found = true;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                mx = rexListIdx.Match(varname);
                                if (mx.Success)
                                {
                                    string gname = mx.Groups["name"].Value;
                                    string gindex = mx.Groups["index"].Value;
                                    gindex = (gindex == "last") ? "-1" : gindex;

                                    if (x.StartsWith("?"))
                                    {   // build temp var
                                        store.List["?lvar"] = VariableList.BuildTemp(gname);
                                        gname = "?lvar";
                                    }
                                    if (!int.TryParse(gindex, NSFloat, InvClt, out int iindex))
                                    {
                                        throw ParseTypeError(I18n.TranslateWord("index"), gindex, I18n.TranslateWord("int"), x);
                                    }
                                    lock (store.List)
                                    {
                                        VariableList vl = GetListVariable(store, gname, false);
                                        val = vl.Peek(iindex).ToString();
                                    }
                                    found = true;
                                }
                            }
                        }
                        // retrieve dict variable value
                        else if (x.StartsWith("dvar:") || x.StartsWith("d:")
                              || x.StartsWith("pdvar:") || x.StartsWith("pd:")
                              || x.StartsWith("?dvar:") || x.StartsWith("?d:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables
                                                : x.StartsWith("d") ? plug.sessionvars : new VariableStore();
                            string varname = x.Substring(x.IndexOf(":") + 1);

                            mx = rexMethod.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gprop = mx.Groups["prop"].Value;
                                string garg = mx.Groups["arg"].Value;
                                string[] args = SplitArguments(garg);
                                int argc = args.Length;
                                if (x.StartsWith("?"))
                                {   // build temp var
                                    store.Dict["?dvar"] = VariableDictionary.BuildTemp(gname);
                                    gname = "?dvar";
                                }
                                switch (gprop)
                                {
                                    case "size":
                                    case "length":
                                        {
                                            lock (store.Dict)
                                            {
                                                VariableDictionary vd = GetDictVariable(store, gname, false);
                                                val = vd.Size.ToString();
                                            }
                                        }
                                        break;
                                    case "ekey":
                                    case "evalue":
                                        if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            bool exist = (gprop == "ekey") ? vd.ContainsKey(queryStr) : vd.ContainsValue(queryStr);
                                            val = exist ? "1" : "0";
                                        }
                                        break;
                                    case "ifekey":
                                    case "ifevalue":
                                        if (argc != 3) { throw ArgCountError(gprop, "3", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            bool exist = (gprop == "ekey") ? vd.ContainsKey(queryStr) : vd.ContainsValue(queryStr);
                                            val = exist ? args[1] : args[2];
                                        }
                                        break;
                                    case "count": // count(value)
                                        {
                                            if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                            lock (store.Dict)
                                            {
                                                VariableDictionary vd = GetDictVariable(store, gname, false);
                                                val = vd.Count(args[0]).ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "keyof":
                                        if (argc != 1) { throw ArgCountError(gprop, "1", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            val = vd.KeyOf(queryStr);
                                        }
                                        break;
                                    case "keysof":
                                        if (argc != 1 && argc != 2) { throw ArgCountError(gprop, "1-2", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string queryStr = args[0];
                                            string joiner = GetArgument(args, 1, ",");
                                            val = vd.KeysOf(queryStr, joiner);
                                        }
                                        break;
                                    case "joinkeys":
                                    case "joinvalues":
                                        if (argc > 1) { throw ArgCountError(gprop, "0-1", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string joiner = GetArgument(args, 0, ",");
                                            val = (gprop == "joinkeys") ? vd.JoinKeys(joiner) : vd.JoinValues(joiner);
                                        }
                                        break;
                                    case "joinall":
                                        if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            string kvjoiner = GetArgument(args, 0, "=");
                                            string pairjoiner = GetArgument(args, 1, ",");
                                            val = vd.JoinAll(kvjoiner, pairjoiner);
                                        }
                                        break;
                                    case "sumkeys":
                                    case "sum": // sum values
                                        if (argc > 0) { throw ArgCountError(gprop, "0", argc, x); }
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            double sum = (gprop == "sumkeys") ? vd.SumKeys() : vd.Sum();
                                            val = I18n.ThingToString(sum);
                                        }
                                        break;
                                    case "minkey":
                                    case "maxkey":
                                    case "min":  // dvar:list.min(type = "n")  num = "n" / str = "s" / hex = "h"
                                    case "max":
                                        {
                                            if (argc > 1) { throw ArgCountError(gprop, "0-1", argc, x); }
                                            ExtremumInit(args, gprop, x, out string type, out bool isMin);
                                            List<string> strings;
                                            lock (store.Dict)
                                            {
                                                VariableDictionary vd = GetDictVariable(store, gname, false);
                                                strings = (gprop.EndsWith("key")) ? vd.Values.Keys.ToList()
                                                                                  : vd.Values.Values.Select(var => var.ToString()).ToList();
                                            }
                                            val = ExtremumGetResult(strings, type, isMin, gname, gprop, x);
                                            found = true;
                                        }
                                        break;
                                }
                                found = true;
                            }
                            else
                            {
                                mx = rexListIdx.Match(varname);
                                if (mx.Success)
                                {
                                    string gname = mx.Groups["name"].Value;
                                    string gindex = mx.Groups["index"].Value;
                                    if (x.StartsWith("?"))
                                    {
                                        var vd = VariableDictionary.BuildTemp(gname);
                                        val = vd.GetValue(gindex).ToString();
                                    }
                                    else
                                    {
                                        lock (store.Dict)
                                        {
                                            VariableDictionary vd = GetDictVariable(store, gname, false);
                                            val = vd.GetValue(gindex).ToString();
                                        }
                                    }
                                    found = true;
                                }
                            }
                        }
                        // retrieve table variable value
                        else if (x.StartsWith("tvar:") || x.StartsWith("t:")
                              || x.StartsWith("ptvar:") || x.StartsWith("pt:")
                              || x.StartsWith("?tvar:") || x.StartsWith("?t:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables
                                                : x.StartsWith("t") ? plug.sessionvars : new VariableStore();
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexMethod.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gprop = mx.Groups["prop"].Value;
                                string[] args = SplitArguments(mx.Groups["arg"].Value);
                                int argc = args.Length;
                                if (x.StartsWith("?"))
                                {   // build temp var
                                    store.Table["?tvar"] = VariableTable.BuildTemp(gname);
                                    gname = "?tvar";
                                }
                                switch (gprop)
                                {
                                    case "w":
                                    case "width":
                                        {
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                val = vt.Width.ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "h":
                                    case "height":
                                        {
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                val = vt.Height.ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "hjoin": // .hjoin(joiner1 = ",", joiner2 = LINEBREAK_PLACEHOLDER, colSlices = ":", rowSlices = ":")
                                    case "vjoin": // .vjoin(joiner1 = ",", joiner2 = LINEBREAK_PLACEHOLDER, colslices = ":", rowslices = ":")
                                        if (argc > 4) { throw ArgCountError(gprop, "0-4", argc, x); }
                                        lock (store.Table)
                                        {
                                            VariableTable vt = GetTableVariable(store, gname, false);
                                            string joiner1 = GetArgument(args, 0, ",", false);
                                            string joiner2 = GetArgument(args, 1, LINEBREAK_PLACEHOLDER.ToString(), false);
                                            string colSlicesStr = GetArgument(args, 2, ":");
                                            string rowSlicesStr = GetArgument(args, 3, ":");
                                            var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                            var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                            if (gprop.StartsWith("hj"))
                                            {
                                                val = vt.HJoin(joiner1, joiner2, colIndices, rowIndices);
                                            }
                                            else
                                            {
                                                val = vt.VJoin(joiner1, joiner2, colIndices, rowIndices);
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "hl":
                                    case "hlookup": // .hlookup(targetStr, rowIndex, colslices = ":") => colIndex
                                    case "vl":
                                    case "vlookup": // .vlookup(targetStr, colIndex, rowslices = ":") => rowIndex
                                        if (argc != 2 && argc != 3) { throw ArgCountError(gprop, "2-3", argc, x); }
                                        lock (store.Table)
                                        {
                                            VariableTable vt = GetTableVariable(store, gname, false);
                                            string targetStr = args[0];
                                            if (!int.TryParse(args[1], NSFloat, InvClt, out int rawIndex))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), args[1], I18n.TranslateWord("int"), x);
                                            }
                                            int maxLength = (gprop.StartsWith("hl")) ? vt.Height : vt.Width;
                                            int index = (rawIndex < 0) ? (rawIndex + maxLength) : (rawIndex - 1);
                                            string slicesStr = GetArgument(args, 2, ":");

                                            List<int> indices;
                                            if (gprop.StartsWith("hl"))
                                            {
                                                indices = GetSliceIndices(slicesStr, vt.Width, x, startIndex: 1);
                                                val = vt.HLookup(targetStr, index, indices).ToString();
                                            }
                                            else
                                            {
                                                indices = GetSliceIndices(slicesStr, vt.Height, x, startIndex: 1);
                                                val = vt.VLookup(targetStr, index, indices).ToString();
                                            }
                                            found = true;
                                        }
                                        break;
                                    case "count": // count(targetStr, colslices = ":", rowslices = ":")
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(gprop, "1-3", argc, x); }
                                            var colSlicesStr = GetArgument(args, 1, ":");
                                            var rowSlicesStr = GetArgument(args, 2, ":");
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                val = vt.Count(args[0], colIndices, rowIndices).ToString();
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "sum":
                                        {
                                            if (argc > 2) { throw ArgCountError(gprop, "0-2", argc, x); }
                                            var colSlicesStr = GetArgument(args, 0, ":");
                                            var rowSlicesStr = GetArgument(args, 1, ":");
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                val = I18n.ThingToString(vt.Sum(colIndices, rowIndices));
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "min":  // tvar:table.min(type = "n", colSlices = ":", rowSlices = ":")  num = "n" / str = "s" / hex = "h"
                                    case "max":
                                        {
                                            if (argc > 3) { throw ArgCountError(gprop, "0-3", argc, x); }
                                            ExtremumInit(args, gprop, x, out string type, out bool isMin);
                                            string colSlicesStr = GetArgument(args, 1, ":");
                                            string rowSlicesStr = GetArgument(args, 2, ":");
                                            List<string> strings;
                                            lock (store.Table)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                var colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                var rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                strings = rowIndices.SelectMany(
                                                    row => colIndices.Select(col => vt.Rows[row].Values[col].ToString())
                                                    ).ToList();
                                            }
                                            val = ExtremumGetResult(strings, type, isMin, gname, gprop, x);
                                            found = true;
                                        }
                                        break;
                                    case "contain":
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(gprop, "1-3", argc, x); }
                                            string colSlicesStr = GetArgument(args, 1, ":");
                                            string rowSlicesStr = GetArgument(args, 2, ":");
                                            lock (store.Dict)
                                            {
                                                VariableTable vt = GetTableVariable(store, gname, false);
                                                List<int> colIndices = GetSliceIndices(colSlicesStr, vt.Width, x, startIndex: 1);
                                                List<int> rowIndices = GetSliceIndices(rowSlicesStr, vt.Height, x, startIndex: 1);
                                                val = (colIndices.Any(col => rowIndices.Any(
                                                    row => vt.Rows[row].Values[col].ToString() == args[0])
                                                )) ? "1" : "0";
                                                found = true;
                                            }
                                        }
                                        break;
                                    case "ifcontain":
                                        if (argc != 3) { throw ArgCountError(gprop, "3", argc, x); }
                                        lock (store.Table)
                                        {
                                            VariableTable vt = GetTableVariable(store, gname, false);
                                            val = vt.Rows.SelectMany(row => row.Values).Any(cell => cell.ToString() == args[0])
                                                ? args[1] : args[2];
                                            found = true;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                mx = rexTableIdx.Match(varname);
                                if (mx.Success)
                                {
                                    string gname = mx.Groups["name"].Value;
                                    string gcol = mx.Groups["column"].Value;
                                    string grow = mx.Groups["row"].Value;
                                    gcol = (gcol == "last") ? "-1" : gcol;
                                    grow = (grow == "last") ? "-1" : grow;

                                    if (x.StartsWith("?"))
                                    {   // build temp var
                                        store.Table["?tvar"] = VariableTable.BuildTemp(gname);
                                        gname = "?tvar";
                                    }
                                    if (!Int32.TryParse(gcol, NSFloat, InvClt, out int xindex))
                                    {
                                        throw ParseTypeError(I18n.TranslateWord("index"), gcol, I18n.TranslateWord("int"), x);
                                    }
                                    if (!Int32.TryParse(grow, NSFloat, InvClt, out int yindex))
                                    {
                                        throw ParseTypeError(I18n.TranslateWord("index"), grow, I18n.TranslateWord("int"), x);
                                    }
                                    lock (store.Table)
                                    {
                                        VariableTable vt = GetTableVariable(store, gname, false);
                                        val = vt.Peek(xindex, yindex).ToString();
                                    }
                                    found = true;
                                }
                            }
                        }
                        // row-based table lookup
                        else if (x.StartsWith("tvarrl:") || x.StartsWith("ptvarrl:") || x.StartsWith("trl:") || x.StartsWith("ptrl:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexTableIdx.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gheader = mx.Groups["column"].Value;
                                string gindex = mx.Groups["row"].Value;
                                gindex = (gindex == "last") ? "-1" : gindex;

                                if (!Int32.TryParse(gindex, NSFloat, InvClt, out int xindex))
                                {
                                    throw ParseTypeError(I18n.TranslateWord("index"), gindex, I18n.TranslateWord("int"), x);
                                }
                                lock (store.Table)
                                {
                                    VariableTable vt = GetTableVariable(store, gname, false);
                                    int yindex = vt.SeekRow(gheader);
                                    if (yindex > 0)
                                    {
                                        val = vt.Peek(xindex + (xindex >= 0 ? 1 : 0), yindex).ToString();
                                    }
                                }
                                found = true;
                            }
                        }
                        // column-based table lookup
                        else if (x.StartsWith("tvarcl:") || x.StartsWith("ptvarcl:") || x.StartsWith("tcl:") || x.StartsWith("ptcl:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexTableIdx.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string gheader = mx.Groups["column"].Value;
                                string gindex = mx.Groups["row"].Value;
                                gindex = (gindex == "last") ? "-1" : gindex;
                                if (!Int32.TryParse(gindex, NSFloat, InvClt, out int yindex))
                                {
                                    throw ParseTypeError(I18n.TranslateWord("index"), gindex, I18n.TranslateWord("int"), x);
                                }
                                lock (store.Table)
                                {
                                    VariableTable vt = GetTableVariable(store, gname, false);
                                    // index starts from 1; 0 if not found
                                    int xindex = vt.SeekColumn(gheader);
                                    if (xindex > 0)
                                    {
                                        val = vt.Peek(xindex, yindex + (yindex >= 0 ? 1 : 0)).ToString();
                                    }
                                }
                                found = true;
                            }
                        }
                        // double-based lookup based on col/row names
                        else if (x.StartsWith("tvardl:") || x.StartsWith("ptvardl:") || x.StartsWith("tdl:") || x.StartsWith("ptdl:"))
                        {
                            VariableStore store = x.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                            string varname = x.Substring(x.IndexOf(":") + 1);
                            mx = rexTableIdx.Match(varname);
                            if (mx.Success)
                            {
                                string gname = mx.Groups["name"].Value;
                                string colHeader = mx.Groups["column"].Value;
                                string rowHeader = mx.Groups["row"].Value;

                                lock (store.Table)
                                {
                                    VariableTable vt = GetTableVariable(store, gname, false);

                                    // index starts from 1; 0 if not found
                                    int xindex = vt.SeekColumn(colHeader);
                                    int yindex = vt.SeekRow(rowHeader);

                                    if (xindex > 0 && yindex > 0)
                                    {
                                        val = vt.Peek(xindex, yindex).ToString();
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("env:"))
                        {
                            string envVarName = x.Substring(4);
                            Folder f = trig?.Parent;
                            string envVarValue = "";
                            while (f != null)
                            {
                                if (f.EnvironmentVariables.TryGetValue(envVarName, out envVarValue))
                                {
                                    break;
                                }
                                f = f.Parent;
                            }
                            val = envVarValue;
                            found = true;
                        }
                        else if (x.StartsWith("numeric:") || x.StartsWith("n:"))
                        {
                            string numexpr = (x.StartsWith("numeric:")) ? x.Substring(8) : x.Substring(2);
                            val = I18n.ThingToString(EvaluateNumericExpression(logger, o, numexpr));
                            found = true;
                        }
                        else if (x.StartsWith("string:") || x.StartsWith("s:"))
                        {
                            string strexpr = (x.StartsWith("string:")) ? x.Substring(7) : x.Substring(2);
                            val = EvaluateStringExpression(logger, o, strexpr);
                            found = true;
                        }
                        else if (x.StartsWith("if:"))
                        {
                            string ternaryExpr = x.Substring(3);
                            ParseTernaryExpression(ternaryExpr, out string condExpr, out string trueStr, out string falseStr);
                            if (trueStr == null || falseStr == null)
                            {
                                throw new Exception(I18n.Translate("internal/Context/ternaryexpressionerror",
                                    "Ternary expression ({0}) could not be parsed: \r\nCondition: ({1}); \r\nTrueExpr: ({2}); \r\nFalseExpr: ({3})",
                                    ternaryExpr, condExpr, trueStr ?? "null", falseStr ?? "null"));
                            }
                            bool cond = !MathParser.IsZero(MathParser.Parse(condExpr));
                            val = cond ? trueStr : falseStr;
                            found = true;
                        }
                        else if (x.StartsWith("func:") || x.StartsWith("f:"))
                        {
                            val = "";
                            found = true;
                            string funcexpr = (x.StartsWith("func:")) ? x.Substring(5) : x.Substring(2);
                            Match rxm = rexFunc.Match(funcexpr);
                            if (rxm.Success)
                            {
                                string funcname = Trim(rxm.Groups["name"].Value.ToLower());
                                string funcarg = rxm.Groups["arg"].Value;
                                string funcval = rxm.Groups["val"].Value;
                                string[] args = SplitArguments(funcarg);
                                int argc = args.Count();
                                switch (funcname)
                                {
                                    case "toupper": val = funcval.ToUpper(); break;
                                    case "tolower": val = funcval.ToLower(); break;
                                    case "tofullwidth": val = ToFullWidth(funcval); break;
                                    case "tohalfwidth": val = ToHalfWidth(funcval); break;
                                    case "toxivchar": // old name
                                    case "toblackchar":
                                        {
                                            if (!bool.TryParse(GetArgument(args, 0, "false"), out bool combineDigits))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), args[0], I18n.TranslateWord("bool"), x);
                                            }
                                            val = ToXivBlackChar(funcval, combineDigits);
                                        }
                                        break;
                                    case "towhitechar": val = ToXivWhiteChar(funcval); break;
                                    case "length": val = funcval.Length.ToString(); break;
                                    case "hex2dec":    // hex2dec()
                                    case "hex2float":  // hex2float()
                                    case "hex2double": // hex2double()
                                        {
                                            funcval = Trim(funcval);
                                            if (!new Regex("^[0-9A-Fa-f]+$").IsMatch(funcval))
                                            {
                                                throw InvalidValueError(funcname, "funcval", funcval, x);
                                            }
                                            switch (funcname)
                                            {
                                                case "hex2dec":
                                                    val = "" + Int64.Parse(funcval, NumberStyles.HexNumber, InvClt);
                                                    break;
                                                case "hex2float":
                                                    Int32 bytesArrayFloat = Int32.Parse(funcval, NumberStyles.HexNumber, InvClt);
                                                    val = "" + BitConverter.ToSingle(BitConverter.GetBytes(bytesArrayFloat), 0);
                                                    break;
                                                case "hex2double":
                                                    Int64 bytesArrayDouble = Int64.Parse(funcval, NumberStyles.HexNumber, InvClt);
                                                    val = "" + BitConverter.ToDouble(BitConverter.GetBytes(bytesArrayDouble), 0);
                                                    break;
                                            }
                                        }
                                        break;
                                    case "parsedmg": // parse the hex damage in ACT loglines to dec value
                                        {
                                            funcval = Trim(funcval);
                                            if (!reHex8.IsMatch(funcval))
                                            {
                                                throw InvalidValueError(funcname, "funcval", funcval, x);
                                            }
                                            val = MathParser.ParseDamage(funcval).ToString(InvClt);
                                        }
                                        break;
                                    case "float2hex":
                                        {
                                            if (!float.TryParse(funcval, NSFloat, InvClt, out float floatValue))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("float"), x);
                                            }
                                            byte[] bytesArray = BitConverter.GetBytes(floatValue);
                                            Array.Reverse(bytesArray, 0, bytesArray.Length);
                                            val = BitConverter.ToString(bytesArray).Replace("-", "");
                                        }
                                        break;
                                    case "double2hex":
                                        {
                                            if (!double.TryParse(funcval, NSFloat, InvClt, out double doubleValue))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("double"), x);
                                            }
                                            Int64 bytesArray = BitConverter.DoubleToInt64Bits(doubleValue);
                                            val = bytesArray.ToString("X");
                                        }
                                        break;
                                    case "dec2hex": // dec2hex()
                                    case "dec2hex2": // dec2hex2()
                                    case "dec2hex4": // dec2hex4()
                                    case "dec2hex8": // dec2hex8()
                                        {
                                            if (!Int64.TryParse(funcval, NSFloat, InvClt, out Int64 intValue))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("int"), x);
                                            }
                                            string format = funcname.Substring(6).ToUpper(); // "X" "X2" "X4" "X8"
                                            val = intValue.ToString(format);
                                        }
                                        break;
                                    case "ord": // chars => charcodes separated by separator
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            string separator = GetArgument(args, 0, ",");
                                            List<int> charcodes = new List<int>();
                                            for (int idx = 0; idx < funcval.Length; idx++)
                                            {
                                                if (char.IsHighSurrogate(funcval[idx]) && idx + 1 < funcval.Length && char.IsLowSurrogate(funcval[idx + 1]))
                                                {
                                                    charcodes.Add(char.ConvertToUtf32(funcval[idx++], funcval[idx]));
                                                }
                                                else
                                                {
                                                    charcodes.Add(funcval[idx]);
                                                }
                                            }
                                            val = string.Join(separator, charcodes);
                                        }
                                        break;
                                    case "chr": // charcodes separated by separator => chars
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            string separator = GetArgument(args, 0, ",");
                                            string[] rawCharcodes = SplitArguments(funcval, separator: separator);
                                            List<string> chars = new List<string>();
                                            for (int idx = 0; idx < rawCharcodes.Length; idx++)
                                            {
                                                if (int.TryParse(rawCharcodes[idx], out int charcode))
                                                {
                                                    chars.Add(char.ConvertFromUtf32(charcode));
                                                }
                                                else
                                                {
                                                    throw ParseTypeError($"#{idx}" + I18n.TranslateWord("string"), rawCharcodes[idx], I18n.TranslateWord("int"), x);
                                                }
                                            }
                                            val = string.Join("", chars);
                                        }
                                        break;
                                    case "padleft":
                                    case "padright":
                                        {
                                            if (argc != 2) { throw ArgCountError(funcname, "2", argc, x); }
                                            char paddingChar = (args[0].Length == 1) ? args[0][0] : GetReplacedChar(Int32.Parse(args[0], InvClt));
                                            int length = Int32.Parse(args[1], InvClt);

                                            if (funcname == "padleft")
                                                val = funcval.PadLeft(length, paddingChar);
                                            else
                                                val = funcval.PadRight(length, paddingChar);
                                        }
                                        break;
                                    case "repeat": // repeat(times, joiner = "")
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                            if (!Int32.TryParse(args[0], NSFloat, InvClt, out int times))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("times"), args[0], I18n.TranslateWord("int"), x);
                                            }
                                            string joiner = GetArgument(args, 1, "");
                                            if (times == 0)
                                            {
                                                val = "";
                                            }
                                            else
                                            {
                                                if (times < 0)
                                                {
                                                    times = -times;
                                                    funcval = new string(funcval.Reverse().ToArray());
                                                }
                                                StringBuilder sb = new StringBuilder(funcval);
                                                string repeatedUnit = joiner + funcval;
                                                for (int repeatCount = 1; repeatCount < times; repeatCount++)
                                                {
                                                    sb.Append(repeatedUnit);
                                                }
                                                val = $"{sb}";
                                            }
                                        }
                                        break;
                                    case "replace": // replace(oldStr, newStr = "", isLooped = false)
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(funcname, "1-3", argc, x); }

                                            string oldStr = args[0];
                                            if (oldStr == "") { throw InvalidValueError(funcname, "oldString", oldStr, x); }

                                            string newStr = GetArgument(args, 1, "");
                                            if (newStr == oldStr) { break; }

                                            string isLoopedStr = GetArgument(args, 2, "false");
                                            if (!bool.TryParse(isLoopedStr, out bool isLooped))
                                            {
                                                throw ParseTypeError("isLooped", isLoopedStr, I18n.TranslateWord("bool"), x);
                                            }
                                            if (newStr.Contains(oldStr) && isLooped)
                                            {
                                                throw InfiniteRepeatError(newStr, oldStr, x);
                                            }

                                            val = funcval.Replace(oldStr, newStr);
                                            while (val.Contains(oldStr) && isLooped)
                                            {
                                                val = val.Replace(oldStr, newStr);
                                            }
                                        }
                                        break;
                                    case "substring": // substring(startindex, length) or substring(startindex)
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                            if (!int.TryParse(args[0], NSFloat, InvClt, out int startIndex))
                                            {
                                                throw ParseTypeError(I18n.TranslateWord("startindex"), args[0], I18n.TranslateWord("int"), x);
                                            }
                                            if (startIndex < 0)
                                            {
                                                startIndex += funcval.Length;
                                            }
                                            switch (argc)
                                            {
                                                case 1:
                                                    val = funcval.Substring(startIndex);
                                                    break;
                                                case 2:
                                                    if (!int.TryParse(args[1], NSFloat, InvClt, out int length))
                                                    {
                                                        throw ParseTypeError(I18n.TranslateWord("length"), args[1], I18n.TranslateWord("int"), x);
                                                    }
                                                    val = funcval.Substring(startIndex, length);
                                                    break;
                                            }
                                            break;
                                        }
                                    case "slice":  // slice(slices = ":") 
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            string slicesStr = GetArgument(args, 0, ":");
                                            var indices = GetSliceIndices(slicesStr, funcval.Length, x, startIndex: 0);
                                            StringBuilder sb = new StringBuilder();
                                            foreach (int index in indices)
                                            {
                                                sb.Append(funcval[index]);
                                            }
                                            val = $"{sb}";
                                            break;
                                        }
                                    case "pick": // pick(index, splitter = ",")
                                        {
                                            if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                            string separator = GetArgument(args, 1, ",");
                                            string[] strArray = SplitArguments(funcval, separator: separator);
                                            if (!int.TryParse(args[0], NSFloat, InvClt, out int index))
                                            { throw ParseTypeError(I18n.TranslateWord("index"), args[0], I18n.TranslateWord("int"), x); }

                                            int normIndex = (index < 0) ? (index + strArray.Length) : index;
                                            val = (normIndex >= 0 && normIndex < strArray.Length)
                                                ? strArray[normIndex] : "";
                                        }
                                        break;
                                    case "args": // for testing the argument splitting: ${f:args(...):}
                                        val = "(" + string.Join(")\n(", args) + ")";
                                        break;
                                    case "i":
                                    case "indexof":      // indexof(stringtosearch)
                                    case "lastindexof":  // lastindexof(stringtosearch)
                                        {
                                            if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                            int index = (funcname.StartsWith("i")) ? funcval.IndexOf(args[0]) : funcval.LastIndexOf(args[0]);
                                            val = I18n.ThingToString(index);
                                        }
                                        break;
                                    case "indicesof":
                                        {
                                            if (argc == 0 || argc > 3) { throw ArgCountError(funcname, "1-3", argc, x); }
                                            string targetStr = args[0];
                                            int subLength = targetStr.Length;
                                            int totalLength = funcval.Length;
                                            string joiner = GetArgument(args, 1, defaultValue: ",");
                                            string slicesStr = GetArgument(args, 2, defaultValue: ":");
                                            List<int> indices = GetSliceIndices(slicesStr, totalLength - subLength + 1, x, startIndex: 0);
                                            StringBuilder sb = new StringBuilder();
                                            foreach (int idx in indices)
                                            {
                                                if (funcval.Substring(idx, subLength) == targetStr)
                                                {
                                                    if (sb.Length > 0)
                                                        sb.Append(joiner);
                                                    sb.Append(idx);
                                                }
                                            }
                                            val = $"{sb}";
                                            break;
                                        }
                                    case "compare": // compare(stringtocompare) or compare(stringtocompare, ignorecase)
                                        if (argc != 1 && argc != 2) { throw ArgCountError(funcname, "1-2", argc, x); }
                                        string ignoreCaseStr = GetArgument(args, 1, "true");
                                        if (!bool.TryParse(ignoreCaseStr, out bool ignoreCase))
                                        {
                                            throw ParseTypeError("ignoreCase", ignoreCaseStr, I18n.TranslateWord("bool"), x);
                                        }
                                        val = "" + String.Compare(funcval, args[0], ignoreCase);
                                        break;
                                    case "versioncompare": // ${f:versioncompare(1.2.0.0):1.1.8.0} = -1
                                        if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                        Version srcVersion = Version.TryParse(funcval, out Version v)
                                            ? v : throw ParseTypeError(I18n.TranslateWord("string"), funcval, I18n.TranslateWord("version"), x);
                                        Version tgtVersion = Version.TryParse(args[0], out v)
                                            ? v : throw ParseTypeError(I18n.TranslateWord("string"), args[0], I18n.TranslateWord("version"), x);
                                        val = I18n.ThingToString(srcVersion.CompareTo(tgtVersion));
                                        break;
                                    case "contain":
                                    case "startwith":
                                    case "endwith":
                                    case "equal":
                                        {
                                            if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                            switch (funcname)
                                            {
                                                case "contain": val = (funcval.Contains(args[0])) ? "1" : "0"; break;
                                                case "startwith": val = (funcval.StartsWith(args[0])) ? "1" : "0"; break;
                                                case "endwith": val = (funcval.EndsWith(args[0])) ? "1" : "0"; break;
                                                case "equal": val = (args[0] == funcval) ? "1" : "0"; break;
                                            }
                                        }
                                        break;
                                    case "ifcontain":
                                    case "ifstartwith":
                                    case "ifendwith":
                                    case "ifequal":
                                        {
                                            if (argc != 3) { throw ArgCountError(funcname, "3", argc, x); }
                                            switch (funcname)
                                            {
                                                case "ifcontain": val = (funcval.Contains(args[0])) ? args[1] : args[2]; break;
                                                case "ifstartwith": val = (funcval.StartsWith(args[0])) ? args[1] : args[2]; break;
                                                case "ifendwith": val = (funcval.EndsWith(args[0])) ? args[1] : args[2]; break;
                                                case "ifequal": val = (args[0] == funcval) ? args[1] : args[2]; break;
                                            }
                                        }
                                        break;
                                    case "match": // func:match(str):regex
                                        {
                                            if (argc != 1) { throw ArgCountError(funcname, "1", argc, x); }
                                            Match match = new Regex(UnescapeCustomExpr(funcval)).Match(args[0]);
                                            val = (match.Success) ? "1" : "0";
                                        }
                                        break;
                                    case "capture": // func:capture(str, group):regex
                                        {
                                            if (argc != 2) { throw ArgCountError(funcname, "2", argc, x); }
                                            Match match = new Regex(UnescapeCustomExpr(funcval)).Match(args[0]);
                                            if (int.TryParse(args[1], NSFloat, InvClt, out int groupNumber))
                                            {
                                                if (groupNumber >= 0 && groupNumber < match.Groups.Count)
                                                {
                                                    val = (match.Success) ? match.Groups[groupNumber].Value : "";
                                                    break;
                                                }
                                            }
                                            val = (match.Success) ? match.Groups[args[1]].Value : "";
                                        }
                                        break;
                                    case "ifmatch": // func:ifmatch(str, successStr, failStr):regex
                                        {
                                            if (argc != 3) { throw ArgCountError(funcname, "3", argc, x); }
                                            Match match = new Regex(UnescapeCustomExpr(funcval)).Match(args[0]);
                                            val = (match.Success) ? args[1] : args[2];
                                        }
                                        break;
                                    case "trim":        // trim() or trim(charcode/char, charcode/char, ...)
                                    case "trimleft":    // trimleft() or trimleft(charcode/char, charcode/char, ...)
                                    case "trimright":   // trimright() or trimright(charcode/char, charcode/char, ...)
                                        string trimChars = "";
                                        if (argc > 0)
                                        {
                                            foreach (string arg in args)
                                            {
                                                // length == 1: char    length != 1: charcode
                                                if (arg.Length == 1)
                                                {
                                                    trimChars += arg;
                                                }
                                                else if (arg.Length == 0)
                                                {
                                                    throw InvalidValueError(funcname, I18n.TranslateWord("char") + "/" + I18n.TranslateWord("charcode"), arg, x);
                                                }
                                                else if (arg.Length > 1)
                                                {
                                                    if (!int.TryParse(arg, NSFloat, InvClt, out int charcode))
                                                    {
                                                        throw ParseTypeError(I18n.TranslateWord("charcode"), arg, I18n.TranslateWord("int"), x);
                                                    }
                                                    trimChars += GetReplacedChar(charcode).ToString();
                                                }
                                            }
                                        }
                                        char[] trimCharsArray = trimChars.ToCharArray();

                                        switch (funcname)
                                        {
                                            case "trim":
                                                val = argc == 0 ? Trim(funcval) : funcval.Trim(trimCharsArray);
                                                break;
                                            case "trimleft":
                                                val = argc == 0 ? TrimL(funcval) : funcval.TrimStart(trimCharsArray);
                                                break;
                                            case "trimright":
                                                val = argc == 0 ? TrimR(funcval) : funcval.TrimEnd(trimCharsArray);
                                                break;
                                        }
                                        break;
                                    case "format": // format(type,formatstring)
                                        if (argc != 2) { throw ArgCountError(funcname, "2", argc, x); }
                                        else
                                        {
                                            Type type = Type.GetType(args[0]);
                                            object converted = Convert.ChangeType(funcval, type, InvClt);
                                            val = String.Format("{0:" + args[1] + "}", converted);
                                        }
                                        break;
                                    case "utctime": // utctime(formatstring)
                                    case "localtime": // localtime(formatstring)
                                        {
                                            if (argc > 1) { throw ArgCountError(funcname, "0-1", argc, x); }
                                            Int64 ts = Int64.Parse(funcval, InvClt);
                                            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                            dt = dt.AddSeconds(ts);
                                            if (funcname == "localtime")
                                            {
                                                dt = dt.ToLocalTime();
                                            }
                                            string format = GetArgument(args, 0, "");
                                            val = dt.ToString(format);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (x.StartsWith("_ffxivparty[") || x.StartsWith("_party[") ||
                                 x.StartsWith("_ffxiventity[") || x.StartsWith("_entity["))
                        {
                            mx = rexListProp.Match(x);
                            if (mx.Success)
                            {
                                bool isParty = x.StartsWith("_ffxivparty[") || x.StartsWith("_party[");
                                string key = Trim(mx.Groups["index"].Value);
                                string prop = Trim(mx.Groups["prop"].Value);
                                FFXIV.Entity entity;

                                if (isParty && int.TryParse(key, out int partyIdx) && partyIdx >= 1 && partyIdx <= 8)
                                {   // ffxivparty[n]
                                    string hexID = PluginBridges.BridgeFFXIV.GetPartyMember(partyIdx).GetValue("id").ToString();
                                    entity = hexID == "" ? BridgeFFXIV.XivEntity.NullEntity() : FFXIV.Entity.GetEntityByID(hexID);
                                    RealPlugin.plug.UnfilteredAddToLog(
                                        RealPlugin.DebugLevelEnum.Warning,
                                        partyIdx == 1 ? $"请使用 ${{_me.{prop}}} 代替 ${{_ffxivparty[1].{prop}}}。触发器：{trig.FullPath}"
                                                      : $"不推荐使用 ${{{x}}}，ACT 无法按顺序获取小队列表，请使用其他方式访问小队。触发器：{trig.FullPath}",
                                        this.trig);
                                }
                                else
                                {
                                    var entities = FFXIV.Entity.GetFilteredEntities(key);
                                    if (isParty)
                                    {
                                        entities = entities.Where(e => e.InParty);
                                    }
                                    entity = entities.FirstOrDefault();
                                }
                                if (entity == null)
                                {
                                    RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, I18n.Translate(
                                        "internal/Context/noEntity",
                                        "The queried entity does not exist: {0}. Trigger: ({1})",
                                        x, trig?.FullPath ?? "null"), trig);
                                }
                                else val = string.Join(", ", entity.QueryProperties(prop));
                            }
                            found = true;
                        }
                        else if (x.StartsWith("_me.")) // ${_me.prop}
                        {
                            string prop = Trim(x.Substring(4));
                            if (prop.ToLower() == "id")
                            {
                                val = BridgeFFXIV.PlayerHexId;
                            }
                            else
                            {
                                val = string.Join(", ", FFXIV.Entity.GetMyself().QueryProperties(prop));
                            }
                        }
                        else if (x.StartsWith("_tgt.")) // ${_tgt.prop}
                        {   // just for simplifying the expression ${_entity[${_me.targetid}].prop}
                            string prop = Trim(x.Substring(5));
                            var targetID = FFXIV.Entity.GetMyself().TargetID;
                            FFXIV.Entity tgt = FFXIV.Entity.GetEntityByID(targetID)
                                ?? FFXIV.Entity.NullEntity();
                            val = string.Join(", ", tgt.QueryProperties(prop));
                        }
                        else if (x == "_clipboard")
                        {
                            val = Action.ClipboardGetText();
                            if (val.Contains("${_clipboard}"))
                            {
                                throw InfiniteClipboardError();
                            }
                            found = true;
                        }
                        else if (x == "_ffxivtime" || x == "_ET")
                        {
                            TimeSpan ez = GetEorzeanTime();
                            int mins = (int)Math.Floor(ez.TotalMinutes);
                            val = mins.ToString();
                            found = true;
                        }
                        else if (x == "_ETprecise")
                        {
                            TimeSpan ez = GetEorzeanTime();
                            val = ez.TotalMinutes.ToString();
                            found = true;
                        }
                        else if (x == "_lastencounter")
                        {
                            val = plug != null ? plug.LastEncounterHook() : "";
                            found = true;
                        }
                        else if (x == "_activeencounter")
                        {
                            val = plug != null ? plug.ActiveEncounterHook() : "";
                            found = true;
                        }
                        else if (x.StartsWith("_textaura"))
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string gindex = mx.Groups["index"].Value;
                                string gprop = mx.Groups["prop"].Value.ToLower();
                                val = "";
                                if (plug.sc != null)
                                {
                                    lock (plug.sc.textitems)
                                    {
                                        Scarborough.ScarboroughText item = plug.sc.GetText(gindex);
                                        if (item != null)
                                        {
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(item.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(item.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(item.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(item.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(item.Opacity);
                                                    break;
                                                case "text":
                                                    val = item.Text;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lock (plug.textauras)
                                    {
                                        if (plug.textauras.ContainsKey(gindex))
                                        {
                                            Forms.AuraContainerForm acf = plug.textauras[gindex];
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(acf.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(acf.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(acf.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(acf.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(acf.PresentableOpacity);
                                                    break;
                                                case "text":
                                                    val = acf.CurrentText;
                                                    break;
                                            }
                                        }
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x.StartsWith("_imageaura"))
                        {
                            mx = rexListMethod.Match(x);
                            if (mx.Success)
                            {
                                string gindex = mx.Groups["index"].Value;
                                string gprop = mx.Groups["prop"].Value.ToLower();
                                val = "";
                                if (plug.sc != null)
                                {
                                    lock (plug.sc.imageitems)
                                    {
                                        Scarborough.ScarboroughImage item = plug.sc.GetImage(gindex);
                                        if (item != null)
                                        {
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(item.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(item.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(item.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(item.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(item.Opacity);
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lock (plug.imageauras)
                                    {
                                        if (plug.imageauras.ContainsKey(gindex))
                                        {
                                            Forms.AuraContainerForm acf = plug.imageauras[gindex];
                                            switch (gprop)
                                            {
                                                case "x":
                                                    val = I18n.ThingToString(acf.Left);
                                                    break;
                                                case "y":
                                                    val = I18n.ThingToString(acf.Top);
                                                    break;
                                                case "w":
                                                case "width":
                                                    val = I18n.ThingToString(acf.Width);
                                                    break;
                                                case "h":
                                                case "height":
                                                    val = I18n.ThingToString(acf.Height);
                                                    break;
                                                case "opacity":
                                                    val = I18n.ThingToString(acf.PresentableOpacity);
                                                    break;
                                            }
                                        }
                                    }
                                }
                                found = true;
                            }
                        }
                        else if (x == "_screenwidth")
                        {
                            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.PrimaryScreen;
                            val = I18n.ThingToString(scr.WorkingArea.Width);
                            found = true;
                        }
                        else if (x == "_screenheight")
                        {
                            System.Windows.Forms.Screen scr = System.Windows.Forms.Screen.PrimaryScreen;
                            val = I18n.ThingToString(scr.WorkingArea.Height);
                            found = true;
                        }
                        else if (x == "_screenminx")
                        {
                            val = I18n.ThingToString(plug.MinX);
                            found = true;
                        }
                        else if (x == "_screenminy")
                        {
                            val = I18n.ThingToString(plug.MinY);
                            found = true;
                        }
                        else if (x == "_screenmaxx")
                        {
                            val = I18n.ThingToString(plug.MaxX);
                            found = true;
                        }
                        else if (x == "_screenmaxy")
                        {
                            val = I18n.ThingToString(plug.MaxY);
                            found = true;
                        }
                    }
                }
                newexpr = newexpr.Substring(0, m.Index) + val + newexpr.Substring(m.Index + m.Length);
                i++;
            };
        }

        private string ReplacePlaceholderBrackets(string expression)
        {
            return expression.Replace("¤{", "${");
        }

    }
}
*/