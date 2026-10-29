using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Globalization;
using Triggernometry.UI.Forms;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Core.Variables;
using Triggernometry.Core;

namespace Triggernometry.UI.CustomControls
{
    public partial class ExpressionTextBox
    {
        #region Autofill Text

        public static List<string> math = new List<string>()
        {
            // numeric const
            "pi", "π", "pi2", "pi05", "pi025", "pi0125", "pitorad", "piofrad",
            "phi", "major", "minor", "ETmin2sec", "semitone", "cent",

            // numeric func: basic
            "sqrt(x)", "pow(x, y)", "root(x, y)", "exp(x)", "log(x, base=e)",
            "abs(x)", "sign(x)", "rem(x, y)", "mod(x, y)", "mod(x, y, min)", "random(start, end)",
            "truncate(x)", "floor(x)", "ceiling(x)", "round(x, digits=0)",
            "max(...)", "min(...)", "or(...)", "and(...)", "if(condition, trueVal, falseVal)",

            // numeric func: trigonometric 
            "sin(x)", "cos(x)", "tan(x)", "cot(x)", "cotan(x)", "sec(x)", "csc(x)", "cosec(x)",
            "arcsin(x)", "arccos(x)", "arctan(x)", "atan2(x, y)", "arctan2(x, y)",
            "sinh(x)", "cosh(x)", "tanh(x)",

            // numeric func: distance
            "Distance(x0, y0, x1, y1)", "d(x0, y0, x1, y1)", 
            "Distance(x0, y0, z0..., x1, y1, z1...)", "d(x0, y0, z0..., x1, y1, z1...)",
            "ManhattanDistance(x0, y0, z0..., x1, y1, z1...)", "L1d(x0, y0, z0..., x1, y1, z1...)",
            "ChebyshevDistance(x0, y0, z0..., x1, y1, z1...)", "L∞d(x0, y0, z0..., x1, y1, z1...)",
            "ProjD(srcX, srcY, θ, tgtX, tgtY)", "ProjectDistance(srcX, srcY, θ, tgtX, tgtY)",
            "ProjH(srcX, srcY, θ, tgtX, tgtY)", "ProjectHeight(srcX, srcY, θ, tgtX, tgtY)",
            "IsPointInRay(srcX, srcY, θ, width, tgtX, tgtY)",

            // numeric func: angle
            "RadToDeg(rad)", "DegToRad(deg)",
            "Angle(srcX, srcY, tgtX, tgtY)", "θ(srcX, srcY, tgtX, tgtY)", 
            "RelAngle(srcθ, tgtθ)", "Relθ(srcθ, tgtθ)",
            "RadToDir(θ, ±segments, digits=0)",
            "VecToDir(dx, dy, ±segments, digits=0)",
            "DirToRad(dir, ±segments)",
            "IsAngleBetween(θ, minθ, maxθ)", "IsθBetween(θ, minθ, maxθ)",

            // numeric func: Lerp
            "Lerp(start, end, ratio)",
            "LerpAngle(start, end, ratio)",
            "LerpAngleCW(start, end, ratio)",
            "LerpAngleCCW(start, end, ratio)",
            "LerpDir(start, end, ±segments, ratio)",
            "LerpDirCW(start, end, ±segments, ratio)",
            "LerpDirCCW(start, end, ±segments, ratio)",

            // numeric string func
            "hex2dec(hex)", "hex2float(hex)", "hex2double(hex)", "X8float(hex)", "ParseDmg(hex)", 
            "len(alphanumstr)",
            "Freq(note, semitones=0)", "NextETms(XX:XX)", "NextETms(ETmin)",
        };

        public static List<string> prefixes = new List<string>() // right after "${"
        {
            "numeric:", "n:", "func:", "f:", "sfunc:", "if:",
            "var:", "pvar:", "evar:", "epvar:", "v:", "pv:", "ev:", "epv:", "!var:", "!v:", "!pvar:", "!pv:",
            "lvar:", "plvar:", "elvar:", "eplvar:", "l:", "pl:", "el:", "epl:", "!lvar:", "!l:", "!plvar:", "!pl:",
            "tvar:", "ptvar:", "etvar:", "eptvar:", "t:", "pt:", "et:", "ept:", "!tvar:", "!t:", "!ptvar:", "!pt:",
            "dvar:", "pdvar:", "edvar:", "epdvar:", "d:", "pd:", "ed:", "epd:", "!dvar:", "!d:", "!pdvar:", "!pd:",
            "tvarcl:", "tcl:", "ptvarcl:", "ptcl:", "!tvarcl:", "!tcl:", "!ptvarcl:", "!ptcl:", 
            "tvarrl:", "trl:", "ptvarrl:", "ptrl:", "!tvarrl:", "!trl:", "!ptvarrl:", "!ptrl:",
            "tvardl:", "tdl:", "ptvardl:", "ptdl:", "!tvardl:", "!tdl:", "!ptvardl:", "!ptdl:",
            "?l:", "?lvar:", "?t:", "?tvar:", "?d:", "?dvar:",
            "etext:", "eimage:", "ecallback:", "estorage:",
            "env:", "reg:", "ereg:", "!reg:",

            // special variables
            "_incombat", "_lastencounter", "_activeencounter", "_configpath", "_pluginpath", "_pluginversion",
            "_duration", "_event", "_since", "_sincems", "_triggerid", "_triggername", "_triggerpath", "_zone",
            "_response", "_responsecode", "_jsonresponse[x]",
            "_timestamp", "_timestampms", "_systemtime", "_systemtimems", "_clipboard",
            "_screenwidth", "_screenheight", "_textaura[x]", "_imageaura[x]",
            "_x", "_y", "_w", "_width", "_h", "_height", "_opacity",
            "_ffxivparty[x]", "_party[x]", "_ffxiventity[x]", "_entity[x]", "_ffxivplayer", "_me", "_me.id",
            "_job[jobid/jobName/jobAbbrev]", "_tm2id[markType]", "_targetmarker2id[markType]", "_wm[markType]", "_waymark[markType]",
            "_ffxivtime", "_ET", "_ETprecise", "_ffxivprocid", "_ffxivprocname", "_ffxivzoneid",
            "_ffxivversion", "_ffxivincombat", "_ffxivisglobal", "_ffxivlanguage", "_ffxivlanguageid",
            "_env[x]", "_const[x]", "_config[x]", "_storage[x]", "_actionhistory[i/previous]",
            "_this", "_idx", "_col", "_row", "_col[i]", "_row[i]", "_colrl[...]", "_rowcl[...]", "_key", "_val",
            "_loopiterator", "_i",
        };

        public static List<string> funcs = new List<string>()
        {
            "toupper", "tolower", "tofullwidth", "tohalfwidth", "toblackchar(combineDigits=false)", "towhitechar", "tosimpcn", "totradcn",
            "length", "dec2hex", "dec2hex2", "dec2hex4", "dec2hex8", "float2hex", "double2hex",
            "hex2dec", "hex2float", "hex2double", "parsedmg",
            "substring(index)", "substring(index, len)", "slice(slices)", "pick(index, separator=',')",
            "indexof(str)", "lastindexof(str)", "i(str)", "indicesof(str, joiner=',', slices='::')",
            "padleft(char, len)", "padright(char, len)", "chr(separator=',')", "ord(joiner=',')",
            "trim()", "trim(char, char, ...)", "trimleft()", "trimleft(char, char, ...)", "trimright()", "trimright(char, char, ...)",
            "repeat(times, joiner=',')", "replace(oldStr, newStr='', isLooped=false)", "dictreplace(old1 = new1, old2 = new2, ...)",
            "format(type, format)", "compare(str, ignorecase=true)", "versioncompare(tgtVersion)",
            "contain(str)", "ifcontain(str, t, f)", "equal(str)", "ifequal(str, t, f)",
            "startwith(str)", "ifstartwith(str, t, f)", "endwith(str)", "ifendwith(str, t, f)",
            "match(str)", "ifmatch(str, t, f)", "capture(str, groupName)", "capture(str, groupIndex)",
            "utctime(format)", "localtime(format)"
        };

        public static List<string> lvarProps = new List<string>()
        {
            "size", "length", "indexof(str)", "i(str)", "lastindexof(str)",
            "get(idx, default)", 
            "indicesof(str, joiner=',', slices='::')",
            "sum(slices='::')", "count(str, slices='::')",
            "join(joiner=',', slices='::')",
            "randjoin(joiner=',', slices='::')",
            "contain(str, slices='::')", "ifcontain(str, t, f)",
            "max(type='n', slices='::')", "min(type='n', slices='::')",
        };

        public static List<string> tvarProps = new List<string>()
        {
            "w", "width", "h", "height",
            "get(colIndex, rowIIndex, default)",
            "hjoin()", "hjoin(joiner1=',', joiner2='⏎', colSlices='::', rowSlices='::')",
            "vjoin()", "vjoin(joiner1=',', joiner2='⏎', colSlices='::', rowSlices='::')",
            "hlookup(str, rowIndex, colSlices='::')",
            "vlookup(str, colIndex, rowSlices='::')",
            "hl(str, rowIndex, colSlices='::')",
            "vl(str, colIndex, rowSlices='::')",
            "count(str, colSlices='::', rowSlices='::')",
            "sum(colSlices='::', rowSlices='::')",
            "max()", "max(type='n', colSlices='::', rowSlices='::')",
            "min()", "min(type='n', colSlices='::', rowSlices='::')",
            "contain(str, colSlices='::', rowSlices='::')", "ifcontain(str, t, f)",
        };

        public static List<string> dvarProps = new List<string>()
        {
            "size", "length", "ekey(key)", "evalue(value)", "ifekey(key, t, f)", "ifevalue(value, t, f)",
            "get(key, default)",
            "keyof(value)", "keyof(value, default)", "keysof(value, joiner=',')",
            "joinall(kvjoiner='=', pairjoiner=',')", "joinall(kvjoiner='=', pairjoiner=',', selectedKeys...)",
            "joinkeys(joiner=',')", "joinvalues(joiner=',')", "joinvalues(joiner=',', selectedKeys...)",
            "sumkeys", "sum", "count(value)",
            "max(type='n')", "min(type='n')", "maxkey(type='n')", "minkey(type='n')",
        };

        public static List<string> textAuraProps = new List<string>()
        {
            "x", "y", "w", "h", "opacity", "text",
        };

        public static List<string> imageAuraProps = new List<string>()
        {
            "x", "y", "w", "h", "opacity"
        };

        // Name, X, Job, Role, etc.
        public static List<string> XivEntityProps = new List<string> { 
            "HasStatus(statusId)",
            "HasAnyStatus(statusIds...)",
            "HasAllStatus(statusIds...)", 
            "HasTankStance",
            "StatusTimer(statusId, default=-1)", 
            "StatusStack(statusId, default=-1)",
            "PercentHP(digits=-1)", "PercentMP(digits=-1)", 
            "PercentCP(digits=-1)", "PercentGP(digits=-1)",
            "DistanceTo(x, y)", "DistanceTo(x, y, z)",
            "AngleFrom(x, y)", "AngleTo(x, y)", "LocalAngleTo(x, y)",
            "DirFrom(x, y, ±segments, digits=0)",
            "DirTo(x, y, ±segments, digits=0)",
            "LocalDirTo(x, y, ±segments, digits=0)",
            "LocalToWorld(dx, dy)", "LocalToWorld(dx, dy, dz)",
            "WorldToLocal(dx, dy)", "WorldToLocal(dx, dy, dz)",
        }.Concat(FFXIV.Entity.ValidEntityPropNames).Concat(FFXIV.Job.LegalJobPropNames).ToList();
        
        // Job, Role, etc.
        public static List<string> XivJobProps = FFXIV.Job.LegalJobPropNames.ToList();

        public static List<string> configurations = new List<string>()
        {
            "DebugLevel", "UseACTForSound", "UseACTForTTS", "FfxivLogNetwork", "UseOsClipboard", "DeveloperMode", "Autosave", "Language", 
            "UnsafeUsage", "DynamicUsage",
            "Microsoft.CodeAnalysis", "Microsoft.Win32", "System.CodeDom.Compiler", "System.Diagnostics", "Triggernometry.Utilities",
            "System.IO", "System.Net", "System.Reflection", "System.Runtime", "System.Security", "System.Web",
        };

        // sfunc:FuncName(type argName, ...) : returnType
        static readonly Dictionary<Type, string> typeAliases = new Dictionary<Type, string>
        {
            { typeof(void), "void" },
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(object), "object" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(string), "string" }
        };

        static string GetFriendlyTypeName(Type type) // ignore types like List<> that could not be invoked by string expressions
        {
            // Nullable
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var innerType = type.GetGenericArguments()[0];
                return GetFriendlyTypeName(innerType) + "?";
            }

            return typeAliases.TryGetValue(type, out string typeName) ? typeName : type.Name;
        }

        public static List<string> GetStorageFuncDescs()
        {
            var result = new List<string>();
            var delegates = RealPlugin.Instance.scriptingStorage.Where(p => p.Value is Delegate);

            foreach (var kv in delegates)
            {
                var name = kv.Key;
                var del = (Delegate)kv.Value;
                var method = del.Method;
                var parameters = method.GetParameters();

                var paramStrs = parameters.Select(p =>
                {
                    // type
                    string param;
                    if (p.GetCustomAttributes(typeof(ParamArrayAttribute), false).Any())
                    {
                        param = "params " + GetFriendlyTypeName(p.ParameterType.GetElementType()) + "[]";
                    }
                    else
                    {
                        param = GetFriendlyTypeName(p.ParameterType);
                    }
                    // name
                    param += $" {p.Name}";

                    // default value
                    if (p.HasDefaultValue)
                    {
                        param += $"= {p.DefaultValue.ToDataString()}";
                    }

                    return param;
                }).ToArray();

                string paramList = string.Join(", ", paramStrs);
                string returnType = GetFriendlyTypeName(method.ReturnType);

                string sig = $"{name}({paramList}) : {returnType}";
                result.Add(sig);
            }

            return result;
        }
        #endregion


        // record all the capture groups and combine with prefixes
        // when entering a trigger or editing the trigger regex

        private static string _currentTriggerRegexStr = "";
        internal static string CurrentTriggerRegexStr
        {
            get => _currentTriggerRegexStr;
            set
            {   
                _currentTriggerRegexStr = value;
                _currentRegexGroupsAndPrefixes = null;
            }
        }

        private static HashSet<string> _currentRegexGroupsAndPrefixes = null;
        private static HashSet<string> CurrentRegexGroupsAndPrefixes
        {
            get // lazy loading
            {
                if (_currentRegexGroupsAndPrefixes == null)
                {
                    List<string> groups;
                    try
                    {
                        Regex regex = new Regex(_currentTriggerRegexStr);
                        groups = regex.GetGroupNames().ToList();
                        groups.AddRange(prefixes);
                    }
                    catch
                    {
                        _currentTriggerRegexStr = "";
                        groups = prefixes;
                    }
                    _currentRegexGroupsAndPrefixes = new HashSet<string>(groups);
                }
                return _currentRegexGroupsAndPrefixes;
            }
        }


        public static readonly Regex rexPrefix
            = new Regex(@"[$¤]\{(?<prefix>[^$¤{}\[:.]*)$", RegexOptions.Compiled);
        public static readonly Regex rexFunc
            = new Regex(@"[$¤]\{f(?:unc)?:(?<funcid>[^$¤{}(:]*)$", RegexOptions.Compiled);
        public static readonly Regex rexStorageFunc
            = new Regex(@"[$¤]\{sfunc:(?<funcid>[^$¤{}(:]*)$", RegexOptions.Compiled);
        public static readonly Regex rexEnvironment
            = new Regex(@"[$¤]\{env:(?<key>[^$¤{}]*)$", RegexOptions.Compiled);
        public static readonly Regex rexVarName
            = new Regex(@"[$¤]\{(?<e>e?)!?(?:(?<type>reg):(?<name>[^$¤{}]*)|(?<persist>p?)(?<type>[vltd]|text|image|callback|storage)(?:v?ar)?(?:[cdr]l)?:(?<name>[^$¤.[{}]*))$", RegexOptions.Compiled);
        public static readonly Regex rexColHeader
            = new Regex(@"[$¤]\{!?(?<persist>p?)t(?:var)?[cd]l:(?<name>[^$¤[]+)\[(?<key>[^$¤\]]*)$", RegexOptions.Compiled);
        public static readonly Regex rexRowHeader
            = new Regex(@"[$¤]\{!?(?<persist>p?)t(?:var)?(?:rl:(?<name1>[^$¤[]+)|dl:(?<name2>[^$¤[]+)\[.*\])\[(?<key>[^$¤\]]*)$", RegexOptions.Compiled);
        public static readonly Regex rexDictKey
            = new Regex(@"[$¤]\{!?(?<persist>p?)d(?:var)?:(?<name>[^$¤[]+)\[(?<key>[^$¤\]]*)$", RegexOptions.Compiled);
        public static readonly Regex rexStructKey
            = new Regex(@"[$¤]\{_(?<struct>const|textaura|imageaura|config|storage)\[(?<key>[^$¤\]]*)$", RegexOptions.Compiled);
        // The regexes "rex...Prop" and "rexMath" are matched after looking for the previous unclosed '{'
        public static readonly Regex rexVarProp
            = new Regex(@"^!?[p?]?(?<type>[ltd])(?:var)?:.*\.(?<prop>[^.(]*)$", RegexOptions.Compiled);
        public static readonly Regex rexMeProp
            = new Regex(@"^_me\.(?<prop>.*)$", RegexOptions.Compiled);
        public static readonly Regex rexStructProp
            = new Regex(@"_(?<struct>[^[]+)\[.*\]\.(?<prop>[^.]*)$", RegexOptions.Compiled);
        public static readonly Regex rexMath
            = new Regex(@"(?<![[$¤.])\b[\p{L}\w]+$", RegexOptions.Compiled);

        private static readonly Regex rexDynamicNames // capture the names in the expressions and store into lists for autofill
            = new Regex(@"[$¤]\{e?!?(?:(?<type>reg):(?<name>[^$¤{}\n]*)|(?<persist>p?)(?<type>[vltd]|text|image)(?:v?ar)?(?:[cdr]l)?:(?<name>[^$¤.[{}\n]*)[^\${}]*)\}", RegexOptions.Compiled);

        private string CurrentMatch;
        private bool isNameAutocomplete;
        private readonly Timer acfDebounceTimer = new Timer();

        public Forms.AutoCompleteForm acf = null;


        private void ExpressionTextBox_LostFocus(object sender, EventArgs e)
        {
            HideAutocomplete();
        }

        private void ExpressionTextBox_Leave(object sender, EventArgs e)
        {
            HideAutocomplete();
        }

        private void ExpressionTextBox_Disposed(object sender, EventArgs e)
        {
            HideAutocomplete();
            acfDebounceTimer.Dispose();
        }

        private void RefreshAutocomplete(IEnumerable<string> strs)
        {
            lock (this)
            {
                if (acf == null)
                {
                    return;
                }
                acf.BuildList(strs);
            }
        }

        [DllImport("user32.dll")]
        static extern bool GetCaretPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        private void ShowAutocomplete(IEnumerable<string> strs)
        {
            if (AutocompleteAvailable == false)
            {
                return;
            }
            lock (this)
            {
                bool refresh = acf != null;
                if (!refresh)
                {
                    acf = new Forms.AutoCompleteForm();
                }

                // show acf beneath the start of the matched string
                GetCaretPos(out POINT cursorPoint);
                Size size = TextRenderer.MeasureText(CurrentMatch, textBox1.Font);
                Point parent = Parent.PointToScreen(Location);
                double lineHeight = textBox1.Font.Height * 4 / 3;
                acf.Left = parent.X + panel1.Width + cursorPoint.X - size.Width + 4;
                acf.Top = parent.Y + cursorPoint.Y + (int)lineHeight;

                if (refresh)
                {
                    RefreshAutocomplete(strs);
                    return;
                }

                acf.GotFocus += Acf_GotFocus;
                acf.listBox1.MouseDown += ListBox1_MouseDown;
                acf.listBox1.DoubleClick += ListBox1_DoubleClick;
                RefreshAutocomplete(strs);
                acf.Show(this);
                AcceptButton = ParentForm.AcceptButton;
                CancelButton = ParentForm.CancelButton;
                ParentForm.AcceptButton = null;
                ParentForm.CancelButton = null;
            }
        }

        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (AutocompleteActive() == true)
            {
                ApplyAutoComplete();
            }
        }

        private void ListBox1_MouseDown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void Acf_GotFocus(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void HideAutocomplete()
        {
            acfDebounceTimer.Stop();
            lock (this)
            {
                if (acf != null)
                {
                    acf.Dispose();
                    acf = null;
                    if (ParentForm != null)
                    {
                        ParentForm.AcceptButton = AcceptButton;
                        ParentForm.CancelButton = CancelButton;
                    }
                }
            }
        }

        private bool AutocompleteActive()
        {
            lock (this)
            {
                return (acf != null);
            }
        }

        private string GetChosenAutocomplete()
        {
            lock (this)
            {
                string temp = acf.GetChosenAutocomplete();
                HideAutocomplete();
                return temp;
            }
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.PageDown || e.KeyData == Keys.PageUp)
            {
                if (AutocompleteActive() == true)
                {
                    if (e.KeyData == Keys.Up)
                    {
                        acf.PreviousAutocompleteItem();
                    }
                    if (e.KeyData == Keys.Down)
                    {
                        acf.NextAutocompleteItem();
                    }
                    if (e.KeyData == Keys.PageUp)
                    {
                        acf.PreviousAutocompletePage();
                    }
                    if (e.KeyData == Keys.PageDown)
                    {
                        acf.NextAutocompletePage();
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                HideAutocomplete();
            }
        }

        /// <summary>一次筛选、排序并保存补全候选，后续计数与显示不再重复枚举。</summary>
        private string[] GetAutocompleteSuggestions(IEnumerable<string> source, string match) =>
            source.Where(item => item.StartsWith(match, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(match, item, StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item, StringComparer.Ordinal).ToArray();

        /// <summary>记录候选替换的范围；名称中的括号按普通字符插入。</summary>
        private string[] MatchCompletion(IEnumerable<string> source, string match, bool completeName = false)
        {
            var candidates = GetAutocompleteSuggestions(source, match);
            if (candidates.Length > 0)
            {
                CurrentMatch = match;
                isNameAutocomplete = completeName;
            }
            return candidates;
        }

        /// <summary>合并已存在名称与尚未保存的动态名称。</summary>
        private static HashSet<string> GetAutofillNames(AutofillTypeEnum type, bool persistent)
        {
            var names = new HashSet<string>(GetExistingAutofillNameList(type, persistent));
            names.UnionWith(GetDynamicAutofillNameList(type, persistent) ?? Enumerable.Empty<string>());
            return names;
        }

        private void ApplyAutoComplete()
        {
            string ac = GetChosenAutocomplete();
            int cursorPos = textBox1.SelectionStart;

            // remove the existing part to fix the capitalization typos
            if (CurrentMatch.Length > 0 && cursorPos >= CurrentMatch.Length)
            {
                textBox1.Text = textBox1.Text.Remove(cursorPos - CurrentMatch.Length, CurrentMatch.Length);
                textBox1.SelectionStart = cursorPos - CurrentMatch.Length;
            }

            // when the autofilled string contains '(' or '[', e.g. method(arg1, arg2)
            // remove the parameters, and trigger the TextChanged event with the correct cursor position
            if (isNameAutocomplete)
            {
                textBox1.Paste(ac);
                return;
            }
            int pIndex = ac.IndexOf('(');
            int bIndex = ac.IndexOf("[");
            if (pIndex >= 0)
            {
                textBox1.Paste(ac.Substring(0, pIndex) + ")");
                textBox1.SelectionStart--;
                textBox1.Paste("(");
            }
            else if (bIndex >= 0)
            {
                textBox1.Paste(ac.Substring(0, bIndex) + "]");
                textBox1.SelectionStart--;
                textBox1.Paste("[");
            }
            else
                textBox1.Paste(ac);
        }


        /// <summary>定时器只负责刷新弹窗；候选匹配与窗口显示互相独立。</summary>
        private void ProcessAutocomplete()
        {
            acfDebounceTimer.Stop();
            if (textBox1.ReadOnly || !AutocompleteAvailable)
            {
                HideAutocomplete();
                return;
            }
            if (!textBox1.Focused) 
                return;
            var candidates = GetAutocompleteCandidates(textBox1.Text.Substring(0, textBox1.SelectionStart));
            if (candidates.Length == 0) 
                HideAutocomplete();
            else 
                ShowAutocomplete(candidates);
        }

        /// <summary>按原来的优先级匹配语法，只返回候选，不创建或显示窗口。</summary>
        private string[] GetAutocompleteCandidates(string text)
        {
            isNameAutocomplete = false;

            Match match = rexPrefix.Match(text);
            if (match.Success) 
                return MatchCompletion(CurrentRegexGroupsAndPrefixes, match.Groups["prefix"].Value);

            match = rexFunc.Match(text);
            if (match.Success) 
                return MatchCompletion(funcs, match.Groups["funcid"].Value);

            match = rexStorageFunc.Match(text);
            if (match.Success) 
                return MatchCompletion(GetStorageFuncDescs(), match.Groups["funcid"].Value);

            match = rexVarName.Match(text);
            if (match.Success)
            {
                StringToAutofillEnum.TryGetValue(match.Groups["type"].Value, out AutofillTypeEnum type);
                return MatchCompletion(GetAutofillNames(type, match.Groups["persist"].Value == "p"), match.Groups["name"].Value, completeName: true);
            }

            match = rexRowHeader.Match(text);
            if (match.Success)
                return MatchCompletion(GetTableHeaders(match.Groups["name1"].Value + match.Groups["name2"].Value,
                    match.Groups["persist"].Value == "p", isRowHeader: true), match.Groups["key"].Value);

            match = rexColHeader.Match(text);
            if (match.Success)
                return MatchCompletion(GetTableHeaders(match.Groups["name"].Value,
                    match.Groups["persist"].Value == "p", isRowHeader: false), match.Groups["key"].Value);

            match = rexDictKey.Match(text);
            if (match.Success)
            {
                var dictionaries = RealPlugin.Instance.GetVariableStore(match.Groups["persist"].Value == "p").Dict;
                List<string> keys;
                lock (dictionaries)
                    keys = dictionaries.TryGetValue(match.Groups["name"].Value, out var dictionary)
                        ? dictionary.Values.Keys.ToList() : new List<string>();
                return MatchCompletion(keys, match.Groups["key"].Value);
            }

            match = rexStructKey.Match(text);
            if (match.Success) 
                return MatchCompletion(GetStructureKeys(match.Groups["struct"].Value), match.Groups["key"].Value);

            match = rexEnvironment.Match(text);
            if (match.Success)
            {
                var form = FindForm();
                Trigger trigger = form is TriggerForm triggerForm ? triggerForm.Trigger : (form as ActionForm)?.ParentTrigger;
                var names = trigger?.Parent?.RecursiveGetEnvironmentVariables()?.Keys?.ToList() ?? new List<string>();
                return MatchCompletion(names, match.Groups["key"].Value, completeName: true);
            }

            // 找到光标所在的未闭合表达式；内层已闭合的表达式不参与属性前缀匹配。
            string expression = GetCurrentAutocompleteExpression(text);
            if (expression != null)
            {
                if (expression.StartsWith("n:", StringComparison.Ordinal) || 
                    expression.StartsWith("numeric:", StringComparison.Ordinal) || 
                    expression.StartsWith("if:", StringComparison.Ordinal))
                {
                    match = rexMath.Match(expression);
                    if (match.Success) 
                        return MatchCompletion(math, match.Value);
                }

                match = rexVarProp.Match(expression);
                if (match.Success)
                {
                    string type = match.Groups["type"].Value;
                    return MatchCompletion(type == "l" ? lvarProps : type == "t" ? tvarProps : dvarProps, match.Groups["prop"].Value);
                }

                match = rexMeProp.Match(expression);
                if (match.Success) 
                    return MatchCompletion(XivEntityProps, match.Groups["prop"].Value);

                match = rexStructProp.Match(expression);
                if (match.Success) 
                    return MatchCompletion(GetStructureProperties(match.Groups["struct"].Value), match.Groups["prop"].Value);
            }

            if (AutofillType != AutofillTypeEnum.None)
                return MatchCompletion(GetAutofillNames(AutofillType, IsPersistent), text, completeName: true);

            if (ExpressionType == SupportedExpressionTypeEnum.Numeric)
            {
                match = rexMath.Match(expression ?? text);
                if (match.Success) 
                    return MatchCompletion(math, match.Value);
            }

            return Array.Empty<string>();
        }

        /// <summary>在同一存储区锁内读取表格首行或首列，避免补全过程中被修改。</summary>
        private static List<string> GetTableHeaders(string name, bool persistent, bool isRowHeader)
        {
            var tables = RealPlugin.Instance.GetVariableStore(persistent).Table;
            lock (tables)
            {
                var headers = new List<string>();
                if (!tables.TryGetValue(name, out var table)) return headers;
                int length = isRowHeader ? table.Height : table.Width;
                for (int i = 1; i <= length; i++) headers.Add((isRowHeader ? table.Peek(1, i) : table.Peek(i, 1)).ToString());
                return headers;
            }
        }

        /// <summary>按结构类型取得键名快照，沿用悬浮窗名称的动态补全历史。</summary>
        private static IEnumerable<string> GetStructureKeys(string type)
        {
            var plugin = RealPlugin.Instance;
            switch (type)
            {
                case "const":
                    return GetKeysWithLock(plugin.Config.Constants);
                case "textaura":
                    return GetExistingAutofillNameList(AutofillTypeEnum.Text, false).Concat(tmpTextNames);
                case "imageaura":
                    return GetExistingAutofillNameList(AutofillTypeEnum.Image, false).Concat(tmpImageNames);
                case "config":
                    return configurations;
                case "storage":
                    return GetKeysWithLock(plugin.scriptingStorage);
                default:
                    return Array.Empty<string>();
            }
        }

        /// <summary>仅选择候选属性表，不读取游戏实体或执行表达式。</summary>
        private static IEnumerable<string> GetStructureProperties(string type)
        {
            switch (type)
            {
                case "ffxivparty":
                case "ffxiventity":
                case "party":
                case "entity":
                    return XivEntityProps;
                case "textaura":
                    return textAuraProps;
                case "imageaura":
                    return imageAuraProps;
                case "job":
                    return XivJobProps;
                default:
                    return Array.Empty<string>();
            }
        }

        /// <summary>取光标前最近未闭合花括号的内容；纯文本返回 null。</summary>
        private static string GetCurrentAutocompleteExpression(string text)
        {
            int depth = 0;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                if (text[i] == '}') 
                    depth--;
                else if (text[i] == '{') 
                    depth++;
                if (depth != 1) 
                    continue;
                string expression = text.Substring(i + 1);
                if (expression.IndexOf('}') >= 0)
                    expression = expression.Substring(0, expression.IndexOf('{')) + expression.Substring(expression.LastIndexOf('}') + 1);
                return expression;
            }
            return null;
        }

        #region Store Variable Names Dynamically

        private static List<string> registeredNames = new List<string>();
        private static List<string> tmpScalarNames = new List<string>();
        private static List<string> tmpListNames = new List<string>();
        private static List<string> tmpTableNames = new List<string>();
        private static List<string> tmpDictNames = new List<string>();
        private static List<string> tmpTextNames = new List<string>();
        private static List<string> tmpImageNames = new List<string>();

        private static List<string> prsScalarNames = new List<string>();
        private static List<string> prsListNames = new List<string>();
        private static List<string> prsTableNames = new List<string>();
        private static List<string> prsDictNames = new List<string>();

        public enum AutofillTypeEnum { None, Scalar, List, Table, Dict, Image, Text, Callback, Storage, Registered }

        public static Dictionary<string, AutofillTypeEnum> StringToAutofillEnum = new Dictionary<string, AutofillTypeEnum> {
            { "v", AutofillTypeEnum.Scalar },
            { "reg", AutofillTypeEnum.Registered },
            { "l", AutofillTypeEnum.List },
            { "t", AutofillTypeEnum.Table },
            { "d", AutofillTypeEnum.Dict },
            { "image",    AutofillTypeEnum.Image },
            { "text",     AutofillTypeEnum.Text },
            { "callback", AutofillTypeEnum.Callback },
            { "storage",  AutofillTypeEnum.Storage },
        };

        public AutofillTypeEnum AutofillType { get; set; } = AutofillTypeEnum.None;

        /// <summary> 
        /// Returns the list for dynamically storing the names in the expression textboxes, 
        /// based on the autofill type. 
        /// </summary>
        private static List<string> GetDynamicAutofillNameList(AutofillTypeEnum type, bool isPersistent)
        {
            switch (type)
            {
                case AutofillTypeEnum.Registered: return registeredNames;
                case AutofillTypeEnum.Scalar: return isPersistent ? prsScalarNames : tmpScalarNames;
                case AutofillTypeEnum.List:   return isPersistent ? prsListNames   : tmpListNames;
                case AutofillTypeEnum.Table:  return isPersistent ? prsTableNames  : tmpTableNames;
                case AutofillTypeEnum.Dict:   return isPersistent ? prsDictNames   : tmpDictNames;
                case AutofillTypeEnum.Text:   return tmpTextNames;
                case AutofillTypeEnum.Image:  return tmpImageNames;
                default: return null;
            }
        }

        /// <summary> 
        /// Returns the list of existing variable/aura/... names corresponding to the autofill type.
        /// </summary>
        private static List<string> GetExistingAutofillNameList(AutofillTypeEnum type, bool isPersistent)
        {
            VariableStore vs = RealPlugin.Instance.GetVariableStore(isPersistent);
            switch (type)
            {
                case AutofillTypeEnum.Registered: return GetKeysWithLock(RealPlugin.Instance.RegisteredVariables);
                case AutofillTypeEnum.Scalar: return GetKeysWithLock(vs.Scalar);
                case AutofillTypeEnum.List: return GetKeysWithLock(vs.List);
                case AutofillTypeEnum.Table: return GetKeysWithLock(vs.Table);
                case AutofillTypeEnum.Dict: return GetKeysWithLock(vs.Dict);
                case AutofillTypeEnum.Text: return (RealPlugin.Instance.sc != null) ? GetKeysWithLock(RealPlugin.Instance.sc.textitems) : GetKeysWithLock(RealPlugin.Instance.textauras);
                case AutofillTypeEnum.Image: return (RealPlugin.Instance.sc != null) ? GetKeysWithLock(RealPlugin.Instance.sc.imageitems) : GetKeysWithLock(RealPlugin.Instance.imageauras);
                // Callback names are modified under the ID dictionary's lock.
                case AutofillTypeEnum.Callback: return GetKeysWithLock(RealPlugin.Instance.callbacksByName, RealPlugin.Instance.callbacksById);
                case AutofillTypeEnum.Storage: return GetKeysWithLock(RealPlugin.Instance.scriptingStorage);
                default: return new List<string>();
            }
        }

        /// <summary>
        /// Copies current names under the dictionary's synchronization lock.
        /// </summary>
        private static List<string> GetKeysWithLock<T>(Dictionary<string, T> variables, object lockObject = null)
        {
            lock (lockObject ?? variables) return variables.Keys.ToList();
        }

        /// <summary>检查名称是否已存在，只查询支持动态补全历史的数据源。</summary>
        private static bool IsExistingAutofillName(string name, AutofillTypeEnum type, bool isPersistent)
        {
            bool Contains<T>(Dictionary<string, T> variables)
            {
                lock (variables) return variables.ContainsKey(name);
            }
            var plug = RealPlugin.Instance;
            var store = plug.GetVariableStore(isPersistent);
            switch (type)
            {
                case AutofillTypeEnum.Registered:
                    return Contains(plug.RegisteredVariables);
                case AutofillTypeEnum.Scalar:
                    return Contains(store.Scalar);
                case AutofillTypeEnum.List:
                    return Contains(store.List);
                case AutofillTypeEnum.Table:
                    return Contains(store.Table);
                case AutofillTypeEnum.Dict:
                    return Contains(store.Dict);
                case AutofillTypeEnum.Text:
                    return plug.sc != null ? Contains(plug.sc.textitems) : Contains(plug.textauras);
                case AutofillTypeEnum.Image:
                    return plug.sc != null ? Contains(plug.sc.imageitems) : Contains(plug.imageauras);
                default:
                    return false;
            }
        }

        private void RegisterAutofillNames()
        {
            if (ExpressionType == SupportedExpressionTypeEnum.Regex)
                return;

            // search for all expressions like ${var:xxx}, ${l:xxx.prop}, ${pd:xxx[key]},
            // and add the names "xxx" to their corresponding namelist. 
            foreach (Match match in rexDynamicNames.Matches(Text))
            {
                bool isPersistent = match.Groups["persist"].ToString() == "p";
                string typeString = match.Groups["type"].ToString();
                if (!StringToAutofillEnum.TryGetValue(typeString, out AutofillTypeEnum type))
                {
                    type = AutofillTypeEnum.None;
                }
                string name = match.Groups["name"].ToString();
                RegisterDynamicVarName(name, type, isPersistent);
            }

            // If the textbox is used for entering variable names,
            // add the name to the corresponding namelist.
            if (Text.Count(c => c == '$') <= 1) // skip variable names with too many expressions
            {
                RegisterDynamicVarName(Text, AutofillType, IsPersistent);
            }
        }

        private static void RegisterDynamicVarName(string name, AutofillTypeEnum type, bool isPersistent)
        {
            List<string> dynamicNames = GetDynamicAutofillNameList(type, isPersistent);
            if (dynamicNames == null)
            {
                return;
            }
            dynamicNames.Remove(name);

            if (!IsExistingAutofillName(name, type, isPersistent))
            {
                dynamicNames.Add(name);
                if (dynamicNames.Count > 30)
                {
                    dynamicNames.RemoveAt(0);
                }
            }
        }

        internal static void RegisterAllDynamicVarNamesOnForm(Control parent)
        {
            if (parent is ExpressionTextBox exp)
            {
                exp.RegisterAutofillNames();
            }
            foreach (Control children in parent.Controls)
            {
                RegisterAllDynamicVarNamesOnForm(children);
            }
        }
        #endregion

    }
}
