using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Triggernometry;
using Triggernometry.Core;
using Triggernometry.Core.Scripting;
using Triggernometry.Expressions.Maths;
using Triggernometry.FFXIV.ExtractedCsv;
using ActionRow = Triggernometry.FFXIV.ExtractedCsv.Rows.Action;

RealPlugin.Instance.RegisterNamedCallback("CsvQueryAction", new System.Action<object, string>(CsvQueryAction.QueryActionCallback));

public static class CsvQueryAction
{
    public static void Log(string message)
    {
        RealPlugin.Instance.InvokeNamedCallback("command", "/e " + message);
        ScriptHelper.Log(RealPlugin.DebugLevelEnum.Custom, message);
    }

    public static void QueryActionCallback(object _, string input)
    {
        input = input.Trim();
        if (input == "")
        {
            ShowActionListForm();
            return;
        }

        if (input.ToLowerInvariant() == "load")
        {
            LoadAllTables(true);
            return;
        }

        int id = 0;

        // decimal ID: 0d12345 (0d as decimal prefix)
        if (input.StartsWith("0d"))
        {
            try
            {
                id = (int)MathParser.Parse(input.Substring(2));
            }
            catch (ArithmeticException) { }
        }
        // hex ID: 0x1A2B (0x as hex prefix), or 1A2B (no prefix)
        else
        {
            try
            {
                var hexString = input.StartsWith("0x") ? input : "0x" + input;
                id = (int)MathParser.Parse(hexString);
            }
            catch (ArithmeticException) { }
        }

        QueryAction(id);
    }

    public static CsvManager OldManager = new CsvManager();

    public static void LoadAllTables(bool forceReload)
    {
        if (!forceReload && CsvManager.Instance.Count > 0 && OldManager.Count > 0)
            return;

        RealPlugin.Instance.TtsPlaybackHook("正在读取文件");

        if (forceReload || CsvManager.Instance.Count == 0)
        {
            CsvManager.Instance.LoadAllTables();
        }
        if (forceReload || OldManager.Count == 0)
        {
            var folder = ScriptHelper.EvaluateStringExpression("$" + "{_const[XivExtractedCsvPathOld]}");
            folder = folder.TrimEnd('\\', '/');
            if (string.IsNullOrWhiteSpace(folder))
            {
                MessageBox.Show(
                    "未设置旧版本解包文件目录。\n" +
                    "需要在触发器配置 - 常量中添加：\n" +
                    "键 = XivExtractedCsvPathOld\n" +
                    "值 = 存放csv的目录，如 ...\\SaintCoinach.Cmd\\2025.03.18.0000.0000\\rawexd",
                    "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                OldManager.LoadTable(folder + "\\Action.csv");
            }
        }
        RealPlugin.Instance.TtsPlaybackHook("读取完成");
    }

    private static Dictionary<string, List<string>> LoadGimmickAvfxMap()
    {
        var csvPath = ScriptHelper.EvaluateStringExpression("$" + "{_const[XivGimmickVfxPath]}").TrimEnd('\\', '/');
        if (string.IsNullOrWhiteSpace(csvPath))
        {
            MessageBox.Show(
                "未设置 Gimmick Vfx 文件路径。\n" +
                "需要在触发器配置 - 常量中添加：\n" +
                "键 = XivGimmickVfxPath\n" +
                "值 = csv的位置，如 ...\\...\\xxx.csv\n" +
                "每行内容如 mon_sp/gimmick/xxx,vfx/xxx.avfx,vfx/xxx.avfx,...",
                "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return null;
        }

        var dict = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var line in File.ReadAllLines(csvPath, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // 跳过 header
            if (line.StartsWith("timeline", StringComparison.OrdinalIgnoreCase))
                continue;

            var parts = line.Split(',');
            if (parts.Length <= 1)
                continue;

            var key = parts[0].Trim();
            if (string.IsNullOrEmpty(key))
                continue;

            var vfxs = parts.Skip(1).Select(x => x.Trim()).Where(x => x.Length > 0).ToList();
            if (vfxs.Count == 0)
                continue;

            dict[key] = vfxs;
        }

        return dict;
    }

    public static void QueryAction(int actionId)
    {
        LoadAllTables(false);
        var actions = CsvManager.Instance.Get<ActionRow>();
        if (actions.TryGetValue(actionId, out var action))
        {
            OutPutAction(action);
        }
    }

    public static void OutPutAction(ActionRow a)
    {
        var result = $" · 技能名称　{a.Name}\n · 技能ＩＤ　 0x{(int)a.Index:X} ({a.Index})";

        result += $"\n · 咏唱时间　{a.CastTime:0.0} s";

        if (a.Shape != ActionRow.ShapeEnum.None || a.ScaleX > 0 || a.ScaleY > 0)
            result += $"\n · 技能范围　{GetShapeDescWithScale(a)}";

        if (a.OmenId != 0)
            result += $"\n · 预兆　　　{a.Omen?.Name ?? ""} (#{a.Omen.Index})";

        if (a.AnimationEndId != 0)
            result += $"\n · 结束　　　{a.AnimationEnd?.Name ?? ""} (#{a.AnimationEnd.Index})";
        /*
                if (a.CastVfxId != 0)
                    result += $"\n · 咏唱特效　{a.CastVfx.Vfx.Name} (#{a.CastVfx.VfxId})";

                if (a.ActionStartId != 0)
                    result += $"\n · 开始　　　{a.AnimationStart.Name} (#{a.AnimationStart.Index})";

                if ((int)a.AnimationStartVfx.Index != 0)
                    result += $"\n · 开始特效　{a.AnimationStartVfx.Name} (#{a.AnimationStartVfx.Index})";

                if (a.ActionTimelineHitId != 0)
                    result += $"\n · 命中　　　{a.ActionTimelineHit.Name} (#{a.ActionTimelineHit.Index})";
        */

        Log("—————————————\n" + result);
    }

    public static bool HasData(ActionRow a)
    {
        if (a.IconId == 0)
            return false;
        return !string.IsNullOrEmpty(a.Name) ||
            a.ActionCategory != ActionRow.ActionCategoryEnum.None ||
            a.Aspect != 0 ||
            a.AttackType != ActionRow.AttackTypeEnum.无 ||
            a.ScaleY != 0 ||
            a.Scale2X != 0 ||
            a.CastTime != 0 ||
            a.OmenId != 0;
    }

    private static Dictionary<string, string> _omenAbbrevs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "general02f", "Rect" },
        { "general_x02f", "Rect2" },
        { "general_1bf", "Circle" },
        { "gl_fan030_1bf", "Fan30" },
        { "gl_fan045_1bf", "Fan45" },
        { "gl_fan060_1bf", "Fan60" },
        { "gl_fan090_1bf", "Fan90" },
        { "gl_fan120_1bf", "Fan120" },
        { "gl_fan150_1bf", "Fan150" },
        { "gl_fan180_1bf", "Fan180" },
        { "gl_fan210_1bf", "Fan210" },
    };

    public static (float x, float y) GetScales(ActionRow a)
    {
        float x = a.ScaleX;
        float y = a.ScaleY;
        switch (a.Shape)
        {
            case ActionRow.ShapeEnum.Circle:
            case ActionRow.ShapeEnum.Fan:
            case ActionRow.ShapeEnum.Ring:
                if (x == 0)
                    x = y;
                break;
            case ActionRow.ShapeEnum.RectTo:
            case ActionRow.ShapeEnum.RectThrough:
                if (y == 0)
                    y = 40;
                break;
            case ActionRow.ShapeEnum.None: // 偶尔有一些绑定类型错误的
                if (y == 0)
                    y = 40;
                if (x == 0)
                    x = y;
                break;
            default: // 矩形 三角
                break; // do nothing
        }
        return (x, y);
    }

    // _=Tail Screw: 4 m 圆形 (2) | omen=Circle | t=2.2 | Scale=4,4,1
    public static string GetOmenCommand(ActionRow a)
    {
        string omenName = a.Omen.Name;
        omenName = _omenAbbrevs.TryGetValue(omenName, out var result) ? result : omenName; // common omens convert to abbrevs

        string cmd = $"_={(a.Name.StartsWith("_rsv") ? a.Name.Substring(10) : a.Name)}: {GetShapeDescWithScale(a)}";

        bool hasName = !string.IsNullOrEmpty(omenName);
        bool hasRange = a.ScaleX != 0 || a.ScaleY != 0 || a.Shape != ActionRow.ShapeEnum.None;
        bool isLarge = a.Shape == ActionRow.ShapeEnum.Circle && a.ScaleY >= 40;
        bool ttsOnly = !hasRange || isLarge;
        if (!hasName || ttsOnly)
            return cmd + " | tts=？";

        if (string.IsNullOrEmpty(omenName))
        {
            switch (a.Shape)
            {
                case ActionRow.ShapeEnum.Circle:
                    omenName = "Circle";
                    break;
                case ActionRow.ShapeEnum.Fan:
                    omenName = "Fan60";
                    break;
                case ActionRow.ShapeEnum.Rect:
                    omenName = "Rect";
                    break;
                case ActionRow.ShapeEnum.RectTo:
                    omenName = "Rect";
                    break;
                case ActionRow.ShapeEnum.RectThrough:
                    omenName = "Rect";
                    break;
                case ActionRow.ShapeEnum.Ring:
                    omenName = "gl_sircle_2010bf";
                    break;
                case ActionRow.ShapeEnum.Cross:
                    omenName = "Rect2";
                    break;
                case ActionRow.ShapeEnum.Triangle:

                default:
                    break;
            }
        }

        if (string.IsNullOrEmpty(omenName))
            return cmd;

        var t = Math.Max(0, a.CastTime - 0.3);
        (var x, var y) = GetScales(a);
        cmd += $" | omen={omenName} | t={t:0.0} | Scale={x},{y},1";
        if (a.Shape == ActionRow.ShapeEnum.Cross)
            cmd += " | cross=1";
        return cmd;
    }

    public static string GetShapeDescWithScale(ActionRow a)
    {
        string desc;
        switch (a.Shape)
        {
            case ActionRow.ShapeEnum.Circle:
                desc = $"{a.ScaleY} m ";
                break;
            case ActionRow.ShapeEnum.Fan:
                desc = $"{a.ScaleY} m ";
                break;
            case ActionRow.ShapeEnum.Rect:
                desc = $"{a.ScaleX} × {a.ScaleY} m ";
                break;
            case ActionRow.ShapeEnum.RectTo:
                desc = $"{a.ScaleX} × ? m ";
                break;
            case ActionRow.ShapeEnum.RectThrough:
                desc = $"{a.ScaleX} × ? m ";
                break;
            case ActionRow.ShapeEnum.Ring:
                desc = $"{a.ScaleY} m ";
                break;
            case ActionRow.ShapeEnum.Cross:
                desc = $"{a.ScaleX} × {a.ScaleY} m ";
                break;
            case ActionRow.ShapeEnum.Triangle:
                desc = $"{a.ScaleX} × {a.ScaleY} m ";
                break;
            default:
                if (a.ScaleX > 0 || a.ScaleY > 0)
                    desc = $"{a.ScaleX} × {a.ScaleY} m ";
                else
                    desc = "";
                break;
        }
        desc += GetShapeDesc(a.ShapeType);
        return desc;
    }

    public static string GetShapeDesc(byte type)
    {
        switch (type)
        {
            case 0: return "";
            case 1: return "无 (1)";
            case 2: return "圆形 (2)";
            case 3: return "扇形/环 (3)";
            case 4: return "矩形 (4)";
            case 5: return "圆形 (5)";
            case 6: return "圆形 (6)";
            case 7: return "圆形 (7)";
            case 8: return "矩形至 (8)";
            // case 9: return "无 (9)"; 没见到
            case 10: return "月环 (10)";
            case 11: return "十字 (11)";
            case 12: return "矩形 (12)";
            case 13: return "扇形 (13)";
            case 14: return "三角 (14)";
            case 15: return "矩形穿 (15)";
            case 16: return "右矩形至 (16)";
            case 17: return "左矩形至 (17)";
            default:
                return $"未知 ({type})";
        }
    }

    public static string GetActionCategoryDesc(byte type)
    {
        switch (type)
        {
            case 0: return "";
            case 1: return "普攻 (1)";
            case 2: return "魔法 (2)";
            case 3: return "战技 (3)";
            case 4: return "能力 (4)";
            case 5: return "物品 (5)";
            case 6: return "采集 (6)";
            case 7: return "生产 (7)";
            case 8: return "事件 (8)";
            case 9: return "LB  (9)";
            case 10: return "系统 (10)";
            case 11: return "系统2 (11)";
            case 12: return "坐骑 (12)";
            case 13: return "特殊 (13)";
            case 14: return "交互 (14)";
            case 15: return "LB2 (15)";
            case 17: return "炮击 (17)";
            case 18: return "时尚 (18)";
            default:
                return $"未知 ({type})";
        }
    }

    public static string GetAttackTypeDesc(sbyte type)
    {
        switch (type)
        {
            case -1: return "无  (-1)";
            case 0: return "";
            case 1: return "斩击 (1)";
            case 2: return "突刺 (2)";
            case 3: return "打击 (3)";
            case 4: return "射击 (4)";
            case 5: return "魔法 (5)";
            case 6: return "吐息 (6)";
            case 7: return "音波 (7)";
            case 8: return "极限 (8)";
            default:
                return $"未知 ({type})";
        }
    }

    public static string GetAspectDesc(byte type)
    {
        switch (type)
        {
            case 0: return "";
            case 1: return "火 (1)";
            case 2: return "冰 (2)";
            case 3: return "风 (3)";
            case 4: return "土 (4)";
            case 5: return "雷 (5)";
            case 6: return "水 (6)";
            case 7: return "无 (7)";
            default:
                return $"？ ({type})";
        }
    }

    public static void ShowActionListForm()
    {
        LoadAllTables(false);
        var list = CsvManager.Instance.Get<ActionRow>().Values.ToList();
        var map = LoadGimmickAvfxMap();

        var thread = new Thread(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var form = new ActionListForm(list, map))
            {
                Application.Run(form);
            }
        });

        thread.IsBackground = true; // 插件退出时一起结束
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
    }
}

public sealed class ActionListForm : Form
{
    private sealed class ColumnDef
    {
        public string Name { get; private set; }
        public string HeaderText { get; private set; }
        public float FillWeight { get; private set; }
        public Func<ActionRow, object> ValueSelector { get; private set; }
        public Func<ActionRow, IComparable> SortKeySelector { get; private set; }
        public Func<ActionRow, string> CopySelector { get; private set; }

        public ColumnDef(
            string name,
            string headerText,
            float fillWeight,
            Func<ActionRow, object> valueSelector,
            Func<ActionRow, IComparable> sortKeySelector,
            Func<ActionRow, string> copySelector)
        {
            Name = name;
            HeaderText = headerText;
            FillWeight = fillWeight;
            ValueSelector = valueSelector;
            SortKeySelector = sortKeySelector;
            CopySelector = copySelector;
        }
    }

    // all rows from CSV
    private readonly List<ActionRow> _allRows;
    // current filtered view
    private readonly List<ActionRow> _filteredRows;
    private readonly HashSet<int> _oldRowIndexes;

    private readonly List<ColumnDef> _columns;
    private DataGridView _grid;
    private Control _filterPanel;
    private DataGridViewColumn _sortColumn;
    private bool _sortAscending = true;

    private readonly Dictionary<string, List<string>> _timelineToVfxs;

    // 右键菜单
    private ContextMenuStrip _contextMenu;
    private ToolStripMenuItem _miCopyCell;
    private ToolStripMenuItem _miCopyAsOmenRegisterActions;
    private ToolStripMenuItem _miDisplayOmen;
    private ToolStripMenuItem _miDisplayGimmickVfxs;

    // filters
    private CheckBox _chkNewOnly;
    private CheckBox _chkShapeAll;
    private bool _isUpdatingShapeChk;
    private CheckBox[] _shapeTypeChecks;   // 0-15

    private TextBox _txtHexIdRegex;
    private TextBox _txtNameRegex;
    private TextBox _txtExpr;
    private TextBox _txtOmenRegex;
    private TextBox _txtAnimationEndRegex;

    private Label _lblResultCount;
    private Button _btnSearch;
    private Button _btnExport;
    private Button _btnReset;

    private readonly ToolTip _toolTip = new ToolTip
    {
        InitialDelay = 100,
        AutoPopDelay = 30000,
        ReshowDelay = 100,
        ShowAlways = true
    };

    public ActionListForm(List<ActionRow> rows, Dictionary<string, List<string>> timelineToVfxs)
    {
        _allRows = rows?.Where(CsvQueryAction.HasData).ToList() ?? new List<ActionRow>();
        _filteredRows = new List<ActionRow>(_allRows);
        _oldRowIndexes = new HashSet<int>(CsvQueryAction.OldManager.Get<ActionRow>().Values.Where(CsvQueryAction.HasData).Select(a => (int)a.Index));

        _timelineToVfxs = timelineToVfxs ?? new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        Text = "FFXIV 技能检索";
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular);
        Padding = new Padding(12);

        // 开启窗体级双缓冲，减少整体重绘闪烁
        SetStyle(ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint, true);
        UpdateStyles();

        // 居中、按屏幕计算初始大小
        var screen = Screen.FromPoint(Cursor.Position);
        var work = screen.WorkingArea;
        var min = Math.Min(work.Width, work.Height);
        Width = min * 6 / 4;
        Height = min * 4 / 5;
        StartPosition = FormStartPosition.CenterScreen;

        Shown += (s, e) =>
        {
            System.Media.SystemSounds.Asterisk.Play();

            // 即使 ACT 在后台也能强制显示窗体
            TopMost = true;
            Activate();
            BringToFront();
            TopMost = false;

        };

        _columns = CreateColumnDefs();

        _filterPanel = CreateFilterPanel();
        _filterPanel.Dock = DockStyle.Top;

        AcceptButton = _btnSearch;

        InitContextMenu();

        // 不在这里 Add，让 RebuildGrid 统一处理两个控件的添加顺序
        RebuildGrid();

        UpdateResultUi();
    }

    private Control CreateFilterPanel()
    {
        var root = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(0, 2, 0, 2)
        };

        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // row1
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // row2
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // row3

        // ===== row 1: left chk, right (count + search + reset) =====

        var row1 = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            ColumnCount = 2
        };
        row1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));             // chk
        row1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));        // right block

        _chkNewOnly = new CheckBox
        {
            Text = "仅显示新增项 (?)",
            AutoSize = true,
            Margin = new Padding(0, 2, 0, 2)
        };
        SetTip(_chkNewOnly, "仅显示在最新版本中新增的技能。需要同时设置旧版本解包文件目录。");
        row1.Controls.Add(_chkNewOnly, 0, 0);

        var rightPanel = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            Margin = new Padding(0)
        };

        _lblResultCount = new Label
        {
            AutoSize = true,
            Margin = new Padding(0, 3, 4, 3),
            TextAlign = ContentAlignment.MiddleRight
        };

        _btnSearch = new Button
        {
            Text = "搜索",
            AutoSize = true,
            Margin = new Padding(4, 1, 0, 1)
        };
        _btnSearch.Click += SearchButton_Click;

        _btnExport = new Button
        {
            Text = "复制所选行",
            AutoSize = true,
            Margin = new Padding(4, 1, 0, 1)
        };
        _btnExport.Click += ExportButton_Click;

        _btnReset = new Button
        {
            Text = "重置",
            AutoSize = true,
            Margin = new Padding(4, 1, 0, 1)
        };
        _btnReset.Click += ResetButton_Click;

        rightPanel.Controls.Add(_btnReset);
        rightPanel.Controls.Add(_btnExport);
        rightPanel.Controls.Add(_btnSearch);
        rightPanel.Controls.Add(_lblResultCount);

        row1.Controls.Add(rightPanel, 1, 0);
        root.Controls.Add(row1, 0, 0);

        // ===== row 2: ShapeType 0-15 checkboxes =====

        var row2 = new FlowLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Top,
            WrapContents = true,
            Margin = new Padding(0, 2, 0, 2)
        };

        _shapeTypeChecks = new CheckBox[16];

        // 全选
        _chkShapeAll = new CheckBox
        {
            Text = "全选",
            AutoSize = true,
            Margin = new Padding(0, 1, 12, 1)
        };
        _chkShapeAll.CheckedChanged += ShapeAll_CheckedChanged;
        row2.Controls.Add(_chkShapeAll);

        // 0–15
        for (byte i = 0; i <= 15; i++)
        {
            var chk = new CheckBox
            {
                Text = CsvQueryAction.GetShapeDesc(i),
                AutoSize = true,
                Margin = new Padding(0, 1, 8, 1),
                Tag = i
            };
            chk.CheckedChanged += ShapeSingle_CheckedChanged;

            _shapeTypeChecks[i] = chk;
            row2.Controls.Add(chk);
        }

        root.Controls.Add(row2, 0, 1);

        // ===== row 3: 3 columns (50% / 25% / 25%) =====

        var row3 = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Dock = DockStyle.Fill,
            ColumnCount = 5,
            Margin = new Padding(0, 2, 0, 2)
        };
        row3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
        row3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
        row3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
        row3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
        row3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

        var tip0 = "使用正则表达式过滤技能 HexID，大小写不敏感。";
        Control c0 = CreateLabelTextCell("HexID", tip0, false, out _txtHexIdRegex);

        var tip1 = "使用正则表达式过滤技能名称。";
        Control c1 = CreateLabelTextCell("技能名", tip1, false, out _txtNameRegex);

        var tip2 = "使用数学表达式过滤技能 ID，遵循触发器数学语法。例如：\n" +
                   "· ID > 30000 && min(X, Y) > 0\n\n" +
                   "支持的动作属性占位符（大小写不敏感）：\n" +
                   "· id：十进制技能 ID\n" +
                   "· t：咏唱秒数\n" +
                   "· omen：预兆 (Omen) ID\n" +
                   "· shape：形状 ID (0-15)\n" +
                   "· vfx：咏唱特效 ID\n" +
                   "· x：技能范围（x）\n" +
                   "· y：技能范围（y）\n" +
                   "· category: 技能类型 (0-18)\n" +
                   "· attack：攻击类型（-1-8）\n" +
                   "· aspect：属性（0-7）\n" +
                   "· animationEnd: AnimationEnd Id\n" +
                   "· vfxCount：AnimationEnd Vfx 数量\n" +
                   "· isPlayer：是否为玩家技能";
        Control c2 = CreateLabelTextCell("过滤表达式", tip2, false, out _txtExpr);

        var tip3 = "使用正则表达式过滤预兆 (Omen) 名称。";
        Control c3 = CreateLabelTextCell("预兆 (Omen)", tip3, false, out _txtOmenRegex);

        var tip4 = "使用正则表达式过滤结束时间轴 (AnimationEnd) 名称。";
        Control c4 = CreateLabelTextCell("结束时间轴", tip4, true, out _txtAnimationEndRegex);

        row3.Controls.Add(c0, 0, 0);
        row3.Controls.Add(c1, 1, 0);
        row3.Controls.Add(c2, 2, 0);
        row3.Controls.Add(c3, 3, 0);
        row3.Controls.Add(c4, 4, 0);

        root.Controls.Add(row3, 0, 2);

        return root;
    }

    private Control CreateLabelTextCell(string labelText, string tip, bool isLast, out TextBox textBox)
    {
        // 用一个 2 列的 TableLayout 来放 label 和 textbox
        var layout = new TableLayoutPanel
        {
            ColumnCount = 2,
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(0),
            Padding = new Padding(0, 0, isLast ? 0 : 20, 0)
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));           // label 宽度自适应
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));      // textbox 占满剩余

        var lbl = new Label
        {
            Text = labelText,
            AutoSize = true,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 3, 4, 3),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        SetTip(lbl, tip);

        textBox = new TextBox
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 2, 0, 2),
        };

        layout.Controls.Add(lbl, 0, 0);
        layout.Controls.Add(textBox, 1, 0);

        return layout;
    }

    private void InitContextMenu()
    {
        _contextMenu = new ContextMenuStrip();

        _miCopyCell = new ToolStripMenuItem("复制当前单元格");
        _miCopyCell.ShortcutKeys = Keys.Control | Keys.C;
        _miCopyCell.ShowShortcutKeys = true;
        _miCopyCell.Click += CopyCurrentCell_Click;

        _miCopyAsOmenRegisterActions = new ToolStripMenuItem("将选中行复制为简易播报动作模板");
        _miCopyAsOmenRegisterActions.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
        _miCopyAsOmenRegisterActions.ShowShortcutKeys = true;
        _miCopyAsOmenRegisterActions.Click += CopyAsOmenRegisterActions_Click;

        _miDisplayOmen = new ToolStripMenuItem("显示 Omen");
        _miDisplayOmen.Click += ShowOmen;

        _miDisplayGimmickVfxs = new ToolStripMenuItem("显示 Gimmick VFX"); // 打开时动态生成子菜单
        _miDisplayGimmickVfxs.Click += GimmickVfxMenuItem_Click;

        _contextMenu.Items.AddRange(new ToolStripItem[]
        {
            _miCopyCell,
            _miCopyAsOmenRegisterActions,
            new ToolStripSeparator(),
            _miDisplayOmen,
            _miDisplayGimmickVfxs
        });

        _contextMenu.Opening += ContextMenu_Opening;
    }

    private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        _miDisplayOmen.Visible = false;
        _miDisplayGimmickVfxs.Tag = null;
        _miDisplayGimmickVfxs.DropDownItems.Clear();

        var rowIndex = -1;
        if ((_grid?.SelectedRows.Count ?? 0) > 0)
        {
            rowIndex = _grid.SelectedRows[0].Index;
        }
        if (rowIndex < 0 || rowIndex >= _filteredRows.Count) return;

        var action = _filteredRows[rowIndex];

        // 显示 Omen 菜单
        _miDisplayOmen.Visible = action.OmenId != 0;

        // 选取 AnimationEnd Timeline 名称作为 key
        var key = (action.AnimationEnd?.Name ?? "").Trim();
        if (key.Length == 0) return;

        if (!_timelineToVfxs.TryGetValue(key, out var vfxs) || vfxs == null || vfxs.Count == 0)
        {
            _miDisplayGimmickVfxs.Visible = false;
            return;
        }

        foreach (var vfxPath in vfxs)
        {
            var name = Path.GetFileName(vfxPath);
            var mi = new ToolStripMenuItem(name);
            mi.Tag = vfxPath;
            mi.Click += GimmickVfxMenuItem_Click;
            _miDisplayGimmickVfxs.DropDownItems.Add(mi);
        }

        if (vfxs.Count >= 1)
        {
            _miDisplayGimmickVfxs.Tag = vfxs[0];
        }

        _miDisplayGimmickVfxs.Visible = true;
    }

    private void GimmickVfxMenuItem_Click(object sender, EventArgs e)
    {
        var mi = sender as ToolStripMenuItem;
        if (mi == _miDisplayGimmickVfxs)
            _contextMenu.Close();

        var vfxPath = mi?.Tag as string;
        if (string.IsNullOrWhiteSpace(vfxPath))
            return;
        var me = Triggernometry.FFXIV.Entity.GetMyself();
        var entity = me;
        if (me.TargetID != 0 && me.TargetID != 0xE0000000)
        {
            var tgt = Triggernometry.FFXIV.Entity.GetEntityByID(me.TargetID);
            if (tgt.Exist)
                entity = tgt;
        }
        var cmd = $"{entity.Address}, {entity.Address}, {vfxPath}, -1"; // -1: 无限时长，自动消除
        RealPlugin.Instance.InvokeNamedCallback("ActorVfx", cmd);
    }

    // 新建并挂载 DataGridView：表单初始化和每次刷新都走这里
    // 如果保持 DataGridView，只更新数据源和行数，当 RowCount 变动很大时设置 RowCount 会卡几十秒，没找到解决方法
    private void RebuildGrid()
    {
        SuspendLayout();
        try
        {
            var newGrid = CreateGrid();

            if (_grid != null)
            {
                Controls.Remove(_grid);
                _grid.Dispose();
            }

            _grid = newGrid;

            // 先加 Grid（Fill）
            Controls.Add(_grid);

            // 再加 FilterPanel（Top），确保它是最后一个添加的控件
            if (_filterPanel != null)
            {
                if (Controls.Contains(_filterPanel))
                    Controls.Remove(_filterPanel);

                Controls.Add(_filterPanel);
            }
        }
        finally
        {
            ResumeLayout();
        }
    }

    // 新建一个 DataGridView 并初始化属性、事件、列
    private DataGridView CreateGrid()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            RowHeadersVisible = false,
            MultiSelect = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoGenerateColumns = false,
            VirtualMode = true,
            AllowUserToOrderColumns = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
        };

        // 给 DataGridView 开双缓冲，避免自身重绘闪烁
        EnableDoubleBuffer(grid);

        grid.CellValueNeeded += Grid_CellValueNeeded;
        grid.ColumnHeaderMouseClick += Grid_ColumnHeaderMouseClick;
        grid.CellMouseDown += Grid_CellMouseDown;

        grid.ContextMenuStrip = _contextMenu;

        CreateColumns(grid);
        grid.RowCount = _filteredRows.Count;

        return grid;
    }

    private static void EnableDoubleBuffer(DataGridView grid)
    {
        var prop = typeof(DataGridView).GetProperty(
            "DoubleBuffered",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic);

        prop.SetValue(grid, true, null);
    }

    private void SearchButton_Click(object sender, EventArgs e)
    {
        IEnumerable<ActionRow> query = _allRows;

        if (_chkNewOnly != null && _chkNewOnly.Checked)
        {   // 新版有数据，旧版没有数据
            query = query.Where(a => CsvQueryAction.HasData(a) && !_oldRowIndexes.Contains((int)a.Index));
        }

        // 形状筛选
        bool[] selections = new bool[0];
        if (_shapeTypeChecks != null)
        {
            selections = Enumerable.Range(0, _shapeTypeChecks.Length)
                .Select(i => _shapeTypeChecks[i].Checked)
                .ToArray();
        }
        if (!selections.All(b => b) && !selections.All(b => !b)) // 如果全没勾，或者全勾，则不过滤
        {
            query = query.Where(
                a => a.ShapeType >= 0 &&
                a.ShapeType < selections.Length &&
                selections[a.ShapeType]);
        }

        bool ApplyRegexFilter(ref IEnumerable<ActionRow> src, string pattern, Func<ActionRow, string> selector)
        {
            pattern = pattern?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(pattern))
                return true; // 不填就不筛

            Regex regex;
            try
            {
                regex = new Regex(pattern,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
                    TimeSpan.FromMilliseconds(100));
            }
            catch
            {
                MessageBox.Show($"正则表达式无效：\n{pattern}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            src = src.Where(a =>
            {
                var text = selector(a);
                return text != null && regex.IsMatch(text);
            });

            return true;
        }

        // HexID 正则
        if (!ApplyRegexFilter(ref query, _txtHexIdRegex?.Text, a => $"{(int)a.Index:X}"))
            return;

        // 技能正则
        if (!ApplyRegexFilter(ref query, _txtNameRegex?.Text, a => a.Name))
            return;

        // 预兆正则
        if (!ApplyRegexFilter(ref query, _txtOmenRegex?.Text, a => a.Omen?.Name))
            return;

        // AnimationEnd 正则
        if (!ApplyRegexFilter(ref query, _txtAnimationEndRegex?.Text, a => a.AnimationEnd?.Name))
            return;

        // 过滤表达式
        if (!string.IsNullOrWhiteSpace(_txtExpr?.Text))
        {
            try
            {
                var rawExpr = _txtExpr.Text;
                var rawTokens = MathParser.Lexer(rawExpr);
                var funcTokens = rawTokens.Select<string, Func<ActionRow, string>>(token =>
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        return a => token;
                    }

                    switch (token.ToLowerInvariant())
                    {
                        case "id": return a => ((int)a.Index).ToString(CultureInfo.InvariantCulture);
                        case "t": return a => a.CastTime.ToString(CultureInfo.InvariantCulture);
                        case "omen": return a => a.OmenId.ToString(CultureInfo.InvariantCulture);
                        case "shape": return a => a.ShapeType.ToString(CultureInfo.InvariantCulture);
                        case "vfx": return a => a.CastVfx?.VfxId.ToString(CultureInfo.InvariantCulture) ?? "0";
                        case "x": return a => a.ScaleX.ToString(CultureInfo.InvariantCulture);
                        case "y": return a => a.ScaleY.ToString(CultureInfo.InvariantCulture);
                        case "category": return a => ((byte)a.ActionCategory).ToString(CultureInfo.InvariantCulture);
                        case "attack": return a => ((sbyte)a.AttackType).ToString(CultureInfo.InvariantCulture);
                        case "aspect": return a => ((byte)a.Aspect).ToString(CultureInfo.InvariantCulture);
                        case "animationend": return a => a.AnimationEndId.ToString(CultureInfo.InvariantCulture);
                        case "vfxcount":
                            return a =>
                            {
                                var key = (a.AnimationEnd?.Name ?? "").Trim();
                                if (_timelineToVfxs.TryGetValue(key, out var vfxs) && vfxs != null)
                                    return vfxs.Count.ToString(CultureInfo.InvariantCulture);
                                return "0";
                            };
                        case "isplayer": return a => a.IconId != 405 ? "1" : "0"; // 405 是系统技能图标
                        default: return a => token;
                    }
                }).ToList();
                query = query.Where(a =>
                {
                    var tokens = funcTokens.Select(f => f(a)).ToList();
                    var value = MathParser.MathParserLogic(tokens);
                    return !MathParser.IsZero(value);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("过滤表达式无效：\n" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        List<ActionRow> newRows;
        try
        {
            newRows = query.ToList();
        }
        catch (RegexMatchTimeoutException ex)
        {
            MessageBox.Show(
                "正则匹配超时（已中止本次搜索）。\n\n" +
                $"Timeout: {ex.MatchTimeout.TotalMilliseconds:0} ms\n" +
                $"Pattern: {ex.Pattern}",
                "正则超时", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _filteredRows.Clear();
        _filteredRows.AddRange(newRows);

        // 每次刷新都重建一个新的 DataGridView
        RebuildGrid();
        UpdateResultUi();
    }

    private void ExportButton_Click(object sender, EventArgs e)
    {
        if (_grid == null || _grid.SelectedRows == null || _grid.SelectedRows.Count == 0)
        {
            MessageBox.Show("请先选中要复制的行。", "复制到 Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedRows = _grid.SelectedRows
            .Cast<DataGridViewRow>()
            .OrderBy(r => r.Index)               // 按显示顺序
            .Select(r => _filteredRows[r.Index]) // 映射回数据行（VirtualMode 下 r.Index 就是行号）
            .ToList();

        if (selectedRows.Count == 0)
        {
            MessageBox.Show("请先选中要复制的行。", "复制到 Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            // Excel 友好：TSV（Tab 分隔），行用 \r\n
            var sb = new StringBuilder();

            // header
            sb.Append(string.Join("\t", _columns.Select(c => EscapeTsv(c.HeaderText))));
            sb.Append("\r\n");

            // rows
            foreach (var row in selectedRows)
            {
                var cells = _columns.Select(c =>
                {
                    object v = null;
                    try { v = c.ValueSelector(row); } catch { }
                    var s = Convert.ToString(v, CultureInfo.InvariantCulture) ?? "";
                    return EscapeTsv(s);
                });

                sb.Append(string.Join("\t", cells));
                sb.Append("\r\n");
            }

            Clipboard.SetText(sb.ToString());
            RealPlugin.Instance.TtsPlaybackHook($"已复制 {selectedRows.Count} 行");
        }
        catch (Exception ex)
        {
            MessageBox.Show("复制失败：\n" + ex.Message, "复制所选行", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string EscapeTsv(string s)
    {
        if (s == null) return "";

        // TSV 不需要引号规则，直接把会破坏分列/换行的字符处理掉即可
        // - Tab 会导致错列：替换为空格
        // - CR/LF 会导致多行：替换为空格
        return s.Replace("\t", "    ").Replace("\r", " ").Replace("\n", " ");
    }


    private void ResetButton_Click(object sender, EventArgs e)
    {
        _chkNewOnly.Checked = false;

        if (_chkShapeAll != null)
            _chkShapeAll.Checked = false;

        if (_txtHexIdRegex != null) _txtHexIdRegex.Text = string.Empty;
        if (_txtNameRegex != null) _txtNameRegex.Text = string.Empty;
        if (_txtExpr != null) _txtExpr.Text = string.Empty;
        if (_txtOmenRegex != null) _txtOmenRegex.Text = string.Empty;
        if (_txtAnimationEndRegex != null) _txtAnimationEndRegex.Text = string.Empty;

        // 恢复为全量数据
        _filteredRows.Clear();
        _filteredRows.AddRange(_allRows);

        // 重建 DataGridView
        RebuildGrid();
        UpdateResultUi();
    }

    private void UpdateResultUi()
    {
        if (_lblResultCount != null)
        {
            _lblResultCount.Text = "当前 " + _filteredRows.Count + " 条结果";
        }
    }

    private List<ColumnDef> CreateColumnDefs()
    {
        var list = new List<ColumnDef>
        {
            // 1. ID (decimal)
            new ColumnDef(
                "ID", "ID", 75f,
                a => (int)a.Index,
                a => (int)a.Index,
                a => a.Index.ToString()
            ),

            // 2. HexID
            new ColumnDef(
                "HexID", "Hex", 65f,
                a => $"{(int)a.Index:X}",
                a => (int)a.Index,
                a => $"{(int)a.Index:X}"
            ),

            // 3. 技能名
            new ColumnDef(
                "Name", "技能名", 180f,
                a => {
                    if (a.Name?.StartsWith("_rsv") == true)
                    {
                        // _rsv_47038_-1_4_0_0_SE2DC5B04_EE2DC5B04
                        var idx = a.Name.IndexOf('_', 5);
                        return idx > 5 ? a.Name.Substring(0, idx) : a.Name;
                    }
                    return a.Name ?? "";
                },
                a => a.Name ?? "",
                a => a.Name ?? ""
            ),

            // 4. 咏唱时间 (0.0)
            new ColumnDef(
                "CastTime", "咏唱", 100f,
                a => a.CastTime == 0 ? "瞬时" : $"{a.CastTime.ToString("0.0", CultureInfo.InvariantCulture)} s",
                a => a.CastTime,
                a => a.CastTime.ToString("0.0", CultureInfo.InvariantCulture)
            ),

            // 5. 形状: "#{ShapeType} ({Shape})"
            new ColumnDef(
                "Shape", "形状", 140f,
                a => CsvQueryAction.GetShapeDesc(a.ShapeType),
                a => a.ShapeType,
                a => CsvQueryAction.GetShapeDesc(a.ShapeType)
            ),

            // 6. X: ScaleX (0.#)
            new ColumnDef(
                "X", "X", 70f,
                a => a.ScaleX == 0 ? "" : a.ScaleX.ToString("0.#", CultureInfo.InvariantCulture),
                a => a.ScaleX,
                a => a.ScaleX.ToString("0.#", CultureInfo.InvariantCulture)
            ),

            // 7. Y: ScaleY (int)
            new ColumnDef(
                "Y", "Y", 70f,
                a => a.ScaleY == 0 ? "" : a.ScaleY.ToString(),
                a => a.ScaleY,
                a => a.ScaleY.ToString()
            ),

            // 8. 技能类型
            new ColumnDef(
                "ActionCategory", "分类", 100f,
                a => CsvQueryAction.GetActionCategoryDesc((byte)a.ActionCategory),
                a => (byte)a.ActionCategory,
                a => CsvQueryAction.GetActionCategoryDesc((byte)a.ActionCategory)
            ),

            // 9. 攻击类型
            new ColumnDef(
                "AttackType", "攻击", 100f,
                a => CsvQueryAction.GetAttackTypeDesc((sbyte)a.AttackType),
                a => (sbyte)a.AttackType,
                a => CsvQueryAction.GetAttackTypeDesc((sbyte)a.AttackType)
            ),

            // 10. 属性类型
            new ColumnDef(
                "Aspect", "属性", 80f,
                a => CsvQueryAction.GetAspectDesc((byte)a.Aspect),
                a => (byte)a.Aspect,
                a => CsvQueryAction.GetAspectDesc((byte)a.Aspect)
            ),

            // 11. 预兆
            new ColumnDef(
                "Omen", "预兆名称 (ID)", 280f,
                a => a.OmenId == 0 ? "" : $"{a.Omen?.Name}  ({a.OmenId})",
                a => a.OmenId,
                a => a.Omen?.Name ?? ""
            ),

            // 12. 特效
            new ColumnDef(
                "AnimationEnd", "结束时间轴名称 (ID)", 500f,
                a => {
                    if (a.AnimationEndId == 0)
                        return "";
                    var hasVfx = _timelineToVfxs.TryGetValue(a.AnimationEnd?.Name ?? "", out var vfxs) && vfxs != null && vfxs.Count > 0;
                    return $"{(hasVfx ? $"[{vfxs.Count}] " : "")}{a.AnimationEnd?.Name}  ({a.AnimationEndId})";
                },
                a => a.AnimationEndId,
                a => a.AnimationEnd?.Name ?? ""
            ),

        };

        return list;
    }

    private void CreateColumns(DataGridView grid)
    {
        foreach (ColumnDef def in _columns)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = def.Name,
                HeaderText = def.HeaderText,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                FillWeight = def.FillWeight
            };
            grid.Columns.Add(col);
        }
    }

    private void SetTip(Control ctrl, string tip)
    {
        if (tip == null) return;

        ctrl.Cursor = Cursors.Help;

        _toolTip.SetToolTip(ctrl, tip);

        ctrl.MouseEnter += (s, e) =>
        {
            ctrl.ForeColor = Color.FromArgb(34, 102, 255);
        };

        ctrl.MouseLeave += (s, e) =>
        {
            ctrl.ForeColor = Color.Black;
        };
    }

    private void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _filteredRows.Count) return;
        if (e.ColumnIndex < 0 || e.ColumnIndex >= _columns.Count) return;

        ActionRow row = _filteredRows[e.RowIndex];
        ColumnDef def = _columns[e.ColumnIndex];

        object value;
        try
        {
            value = def.ValueSelector(row);
        }
        catch
        {
            value = null;
        }

        e.Value = value;
    }

    private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.ColumnIndex < 0 || e.ColumnIndex >= _columns.Count) return;

        DataGridViewColumn column = _grid.Columns[e.ColumnIndex];
        ColumnDef def = _columns[e.ColumnIndex];

        if (_sortColumn == column)
        {
            _sortAscending = !_sortAscending;
        }
        else
        {
            _sortColumn = column;
            _sortAscending = true;
        }

        Comparison<ActionRow> cmp = delegate (ActionRow x, ActionRow y)
        {
            IComparable kx = null;
            IComparable ky = null;

            try
            {
                kx = def.SortKeySelector != null
                    ? def.SortKeySelector(x)
                    : def.ValueSelector(x) as IComparable;
            }
            catch
            {
                kx = null;
            }

            try
            {
                ky = def.SortKeySelector != null
                    ? def.SortKeySelector(y)
                    : def.ValueSelector(y) as IComparable;
            }
            catch
            {
                ky = null;
            }

            int result;
            if (kx == null && ky == null) result = 0;
            else if (kx == null) result = -1;
            else if (ky == null) result = 1;
            else result = kx.CompareTo(ky);

            if (!_sortAscending)
                result = -result;

            // main key different: return
            if (result != 0)
                return result;

            // ThenBy Index
            return ((int)x.Index).CompareTo((int)y.Index);
        };

        _filteredRows.Sort(cmp);
        _grid.Invalidate();
        UpdateResultUi();

        foreach (DataGridViewColumn col in _grid.Columns)
        {
            col.HeaderCell.SortGlyphDirection =
                col == _sortColumn
                    ? (_sortAscending ? SortOrder.Ascending : SortOrder.Descending)
                    : SortOrder.None;
        }
    }

    private void Grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right)
            return;

        if (e.RowIndex < 0 || e.ColumnIndex < 0)
            return;

        var row = _grid.Rows[e.RowIndex];

        // 如果右键的是未选中的行，就改成只选这一行
        if (!row.Selected)
        {
            _grid.ClearSelection();
            row.Selected = true;
        }

        _grid.CurrentCell = _grid[e.ColumnIndex, e.RowIndex];
    }

    private void CopyCurrentCell_Click(object sender, EventArgs e)
    {
        var cell = _grid.CurrentCell;
        if (cell == null)
            return;

        int rowIndex = cell.RowIndex;
        int colIndex = cell.ColumnIndex;

        if (rowIndex < 0 || rowIndex >= _filteredRows.Count)
            return;
        if (colIndex < 0 || colIndex >= _columns.Count)
            return;

        var row = _filteredRows[rowIndex];
        var def = _columns[colIndex];

        string text = null;
        try
        {
            var obj = def.CopySelector?.Invoke(row) ?? "";
            text = Convert.ToString(obj, CultureInfo.InvariantCulture);
        }
        catch { }

        if (!string.IsNullOrEmpty(text))
        {
            Clipboard.SetText(text);
            RealPlugin.Instance.TtsPlaybackHook("已复制");
        }
    }

    private void CopyAsOmenRegisterActions_Click(object sender, EventArgs e)
    {
        if (_grid.SelectedRows.Count == 0)
            return;

        var selectedRows = _grid.SelectedRows
            .Cast<DataGridViewRow>()
            .OrderBy(r => r.Index)
            .ToList();

        var actionsXml = selectedRows
            .Select(row => _filteredRows[row.Index])
            .Select((a, i) =>
                $"    <Action " +
                $"OrderNumber=\"{i + 1}\" " +
                $"ActionType=\"DictVariable\" " +
                $"DictVariableOp=\"Set\" " +
                $"DictVariableName=\"简易播报\" " +
                $"DictVariableKey=\"{(int)a.Index:X}\" " +
                $"DictVariableValue=\"{CsvQueryAction.GetOmenCommand(a)}\" />");
        var xml = $"<ActionBundle>\n  <Actions>\n{string.Join("\n", actionsXml)}\n  </Actions>\n</ActionBundle>";

        if (!string.IsNullOrEmpty(xml))
        {
            Clipboard.SetText(xml);
            RealPlugin.Instance.TtsPlaybackHook($"已复制{selectedRows.Count}条动作");
        }
    }

    private void ShowOmen(object sender, EventArgs e)
    {
        var cell = _grid.CurrentCell;
        if (cell == null)
            return;

        int rowIndex = cell.RowIndex;
        if (rowIndex < 0 || rowIndex >= _filteredRows.Count)
            return;

        ActionRow actionRow = _filteredRows[rowIndex];
        var omenName = actionRow.Omen.Name;
        if (string.IsNullOrWhiteSpace(omenName))
            return;

        (var x, var y) = CsvQueryAction.GetScales(actionRow);
        var me = Triggernometry.FFXIV.Entity.GetMyself();
        var pos = me.Pos;
        var heading = me.Heading;
        var commands = new List<string>
        {
            $"Omen: {omenName}\n" +
            $"t: 10\n" +
            $"O: {pos.X}, {pos.Y}, {pos.Z}\n" +
            $"θ: {heading}\n" +
            $"Scale: {x}, {y}, 1\n" +
            $"Angle: -π"
        };
        if (actionRow.Shape == ActionRow.ShapeEnum.Cross)
        {
            commands.Add(commands[0] + "/2"); // Angle: -π/2
        }
        commands.ForEach(cmd => RealPlugin.Instance.InvokeNamedCallback("PictoACT", cmd));
        if (commands[0].Length > 0)
            Clipboard.SetText(commands[0]);
    }

    private void ShapeAll_CheckedChanged(object sender, EventArgs e)
    {
        if (_isUpdatingShapeChk || _shapeTypeChecks == null || _chkShapeAll == null) return;

        _isUpdatingShapeChk = true;
        try
        {
            foreach (var chk in _shapeTypeChecks)
            {
                if (chk != null)
                    chk.Checked = _chkShapeAll.Checked;
            }
        }
        finally
        {
            _isUpdatingShapeChk = false;
        }
    }

    private void ShapeSingle_CheckedChanged(object sender, EventArgs e)
    {
        if (_isUpdatingShapeChk || _shapeTypeChecks == null || _chkShapeAll == null) return;
        _isUpdatingShapeChk = true;

        try
        {
            bool allChecked = _shapeTypeChecks.All(chk => chk?.Checked == true);
            _chkShapeAll.Checked = allChecked;
        }
        finally
        {
            _isUpdatingShapeChk = false;
        }
    }

}