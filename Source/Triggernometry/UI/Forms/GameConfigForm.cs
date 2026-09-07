using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Variables;
using Triggernometry.UI.CustomControls;

namespace Triggernometry.UI.Forms
{
    public partial class GameConfigForm : Form
    {
        public readonly struct ConfigInfo
        {
            public readonly string Name;
            public readonly string Version;
            public readonly string Author;
            public readonly string ConfigName;   // 保存配置的触发器永久变量名

            public string Description
            {
                get
                {
                    var value = Name;
                    if (Version != null) value += $"  v{Version}";
                    if (Author != null) value += $"  by {Author}";
                    return value;
                }
            }

            public ConfigInfo(string name, string version, string author, string configName)
            {
                Name = name;
                Version = version;
                Author = author;
                ConfigName = configName;
            }
        }

        public readonly ConfigInfo Info;
        public Font UserFont = new Font("微软雅黑", 10);

        /// <summary> 储存表单中所有 Option 控件的列表。 </summary>
        private List<Option> _options = new List<Option>();

        /// <summary> （可选）表单绑定的小队列表控件。 </summary>
        private PartyListPanel _partyListPanel;

        /// <summary> 用于储存用户配置的触发器字典变量。 </summary>
        public VariableDictionary Config = new VariableDictionary();

        /// <summary> 表单上方用于放置所有选项组的 Panel，可滚动。 </summary>
        Panel mainPanel = new BackgroundPanel();

        /// <summary> 表单下方用于放置按钮等控件的 TableLayoutPanel。 </summary>
        TableLayoutPanel bottomPanel = new BottomTableLayoutPanel { RowCount = 1, ColumnCount = 1 };

        public Button btnSave = new MyButton { Text = "保存配置" };

        public GameConfigForm(ConfigInfo info)
        {
            // suspend until run
            SuspendLayout();
            Info = info;

            // load config
            Config = RealPlugin.Instance.GetVariableStore(true).Dict.TryGetValue(Info.ConfigName, out var cfg)
                ? (VariableDictionary)cfg.Duplicate()
                : new VariableDictionary();

            // basic props
            Text = Info.Description;
            Font = UserFont;
            StartPosition = FormStartPosition.CenterScreen;
            int width = (TextRenderer.MeasureText("AAAA", UserFont).Width) * 16;
            MinimumSize = new Size(width, width); // To-do：autoadjust by minimum height
            Controls.Add(mainPanel);
            Controls.Add(bottomPanel);
            bottomPanel.Controls.Add(btnSave);

            // events
            Shown += (sender, e) =>
            {
                _options.ForEach(o => o.InitializeData());
                mainPanel.AutoScrollPosition = new Point(0, 0);
                TopMost = true;
                BringToFront();
                Activate();
                TopMost = false;
            };

            btnSave.Click += btnSave_Click;
        }

        /// <summary>
        /// 在表单上方的 mainPanel 区域添加一个 Panel - GroupBox - OptionsTableLayoutPanel 的结构，并返回这个 OptionsTableLayoutPanel。
        /// </summary>
        /// <param name="groupName">GroupBox 上方显示的名称，建议首尾添加空格。</param>
        /// <returns>生成的 OptionsTableLayoutPanel，用于填充该分组的选项。</returns>
        public OptionsTableLayoutPanel AddOptionGroup(string groupName)
        {
            var table = new OptionsTableLayoutPanel();
            var group = new MyGroupBox(groupName);
            var panel = new GroupPanel();

            mainPanel.Controls.Add(panel);
            panel.Controls.Add(group);
            group.Controls.Add(table);

            return table;
        }

        /// <summary>
        /// 在表单上方的 mainPanel 区域添加一个 Panel - GroupBox - PartyListPanel 的结构，并返回这个 PartyListPanel。
        /// </summary>
        /// <param name="groupName">GroupBox 上方显示的名称，建议首尾添加空格。</param>
        /// <param name="pListPanel">包含每个队员职能描述的 string[]，如 { "MT", "ST", ... }。小队人数由 Array 长度决定。</param>
        /// <returns>生成的 PartyListPanel，用于显示当前队员并调整顺序。</returns>
        public void AddPartyGroup(string groupName, PartyListPanel pListPanel)
        {
            _partyListPanel = pListPanel;
            var group = new MyGroupBox(groupName);
            var panel = new GroupPanel();

            mainPanel.Controls.Add(panel);
            panel.Controls.Add(group);
            group.Controls.Add(_partyListPanel);
        }

        /// <summary> 将选项添加至表单，并放置在 GroupBox 中的 Table 末尾。 </summary>
        public void AddOption(Option option, TableLayoutPanel table)
        {
            _options.Add(option);
            option.AppendToTable(table);
        }

        /// <summary> 将选项添加至表单，并返回该选项。 </summary>
        public T AddOption<T>(T option, TableLayoutPanel table) where T : Option
        {
            AddOption((Option)option, table);
            return option;
        }

        public void AddOptions(TableLayoutPanel table, params Option[] options)
        {
            foreach (var option in options)
                AddOption(option, table);
        }

        /// <summary> Add a separator line at the end of the GroupBox. </summary>
        public void AddSeparatorLine(TableLayoutPanel table)
        {
            table.RowCount++;
            Panel separator = new SeperatorPanel();
            table.Controls.Add(separator, 0, table.RowCount - 1);
            table.SetColumnSpan(separator, 2);
        }

        /// <summary> 在 GroupBox 中的 Table 末尾添加一个文本 Label。 </summary>
        public Label AddLabel(string desc, TableLayoutPanel table, string hint = null)
        {
            var dummyOption = new OptionLbl(desc, hint);
            dummyOption.AppendToTable(table); // 不用 AddOption，因为不需要保存到配置
            return dummyOption.Label;
        }

        public Control AddControl(Control ctrl, TableLayoutPanel table)
        {
            table.RowCount++;
            table.Controls.Add(ctrl, 0, table.RowCount - 1);
            table.SetColumnSpan(ctrl, 2);
            return ctrl;
        }

        public Option GetOption(string configKey)
            => _options.Where(o => o.ConfigKey == configKey).FirstOrDefault();

        public IReadOnlyList<Option> GetOptions()
            => _options;

        /// <summary> 从触发器变量中读取全部已保存配置，若校验合法则设置到表单。 </summary>
        public void LoadFromConfig()
        {
            _partyListPanel?.LoadFromConfig(); // 设置了小队列表控件

            foreach (Option option in _options)
            {
                option.LoadFromConfig(Config);
            }
        }

        /// <summary>
        /// 从预设字典中加载配置。若预设字典中有对应的配置项，则将其应用到表单。
        /// </summary>
        public void ApplyPreset(Dictionary<string, string> preset)
            => ApplyPreset(new VariableDictionary(preset));

        /// <summary>
        /// 从预设字典中加载配置。若预设字典中有对应的配置项，则将其应用到表单。
        /// </summary>
        public void ApplyPreset(VariableDictionary preset)
        {
            foreach (var pair in preset.Values)
            {
                GetOption(pair.Key)?.LoadFromConfig(preset);
            }
        }

        public bool TryGetPreset(int index, out VariableDictionary preset)
        {
            preset = RealPlugin.Instance.GetVariableStore(true).Dict.TryGetValue(
                $"{Info.ConfigName}{index}",
                out var currentPreset)
                ? (VariableDictionary)currentPreset.Duplicate()
                : null;

            return preset != null;
        }

        public void SaveToConfig()
        {
            _partyListPanel?.SaveToConfig();

            foreach (Option option in _options)
            {
                option.SaveToConfig(Config);
            }

            Config.SetValue(
                "env",
                "${_env[COMPUTERNAME]} ${_env[USERNAME]}"); // 储存系统环境变量以保证用户不是 copy 了别人的配置

            Config.SetValue("version", Info.Version);

            RealPlugin.Instance.GetVariableStore(true).Dict[Info.ConfigName] = Config;
            RealPlugin.Instance.InvokeNamedCallback("command", "/e <se.10>");

            // ↓ 应该修改
            RealPlugin.Instance.InvokeNamedCallback(
                "command",
                $"/{Config.GetValue("cnlPrivate")} 已保存配置。");

            this.Close();
        }

        public void SaveToPreset(int presetIdx, string presetName)
        {
            var preset = new VariableDictionary();

            foreach (Option option in _options)
            {
                option.SaveToConfig(preset);
            }

            preset.SetValue("version", Info.Version);
            preset.SetValue("PresetName", presetName);
            RealPlugin.Instance.GetVariableStore(true).Dict[
                $"{Info.ConfigName}{presetIdx}"] = preset;
        }

        void btnSave_Click(object sender, EventArgs e)
            => SaveToConfig();

        /// <summary> 读取配置，恢复表单布局，显示表单。</summary>
        public void Run()
        {
            LoadFromConfig();
            ResumeLayout();
            ShowDialog();
            Dispose();
        }

        /// <summary>
        /// 在独立 STA 线程中创建并显示配置表单。
        /// </summary>
        /// <param name="info">配置表单信息。</param>
        /// <param name="setup">用于构建表单内容的方法。</param>
        public static void ShowConfig(ConfigInfo info, Action<GameConfigForm> setup)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    CloseOpenForms();

                    var form = new GameConfigForm(info);
                    setup?.Invoke(form);
                    form.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "配置界面运行时遇到问题：\n\n" + ex,
                        info.Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private static void CloseOpenForms()
        {
            var forms = Application.OpenForms
                .OfType<GameConfigForm>()
                .ToArray();

            foreach (var form in forms)
            {
                if (form.IsDisposed)
                    continue;

                if (form.InvokeRequired)
                    form.Invoke(new Action(form.Close));
                else
                    form.Close();
            }
        }
    }

    /// <summary> 可以从值检索键的双射字典结构，可以用于将 ComboBox 选项和触发器内存储的键相互映射。</summary>
    public class BijectDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
        private Dictionary<TValue, TKey> _revDict = new Dictionary<TValue, TKey>();
        private List<TKey> _keys = new List<TKey>();
        private List<TValue> _values = new List<TValue>();

        public ReadOnlyCollection<TKey> Keys => _keys.AsReadOnly();
        public ReadOnlyCollection<TValue> Values => _values.AsReadOnly();

        public int Count { get => _dict.Count; }

        public bool ContainsKey(TKey key)
            => _dict.ContainsKey(key);

        public bool ContainsValue(TValue value)
            => _revDict.ContainsKey(value);

        public BijectDictionary()
            : this(new (TKey, TValue)[0])
        {
        }

        public BijectDictionary(params (TKey, TValue)[] items)
        {
            foreach (var (key, value) in items)
            {
                if (_dict.ContainsKey(key))
                    throw new Exception(
                        $"Key \"{key}\" is duplicated in the bijective dictionary.");

                if (_revDict.ContainsKey(value))
                    throw new Exception(
                        $"Value \"{value}\" is duplicated in the bijective dictionary.");

                _dict[key] = value;
                _revDict[value] = key;
                _keys.Add(key);
                _values.Add(value);
            }
        }

        public TValue this[TKey key]
        {
            get => _dict.TryGetValue(key, out TValue value)
                ? value
                : default;
        }

        public TKey GetKey(TValue value)
        {
            return _revDict.TryGetValue(value, out TKey key)
                ? key
                : default;
        }

        public bool RemoveKey(TKey key)
        {
            lock (this)
            {
                int index = _keys.IndexOf(key);

                if (index < 0)
                    return false;

                Remove(key, _values[index], index);
                return true;
            }
        }

        public bool RemoveValue(TValue value)
        {
            lock (this)
            {
                int index = _values.IndexOf(value);

                if (index < 0)
                    return false;

                Remove(_keys[index], value, index);
                return true;
            }
        }

        private void Remove(TKey key, TValue value, int index)
        {
            _keys.RemoveAt(index);
            _values.RemoveAt(index);
            _dict.Remove(key);
            _revDict.Remove(value);
        }

        public BijectDictionary<TKey, TValue> ShallowCopy()
        {
            var duplicate = new BijectDictionary<TKey, TValue>();

            foreach (var key in _keys)
            {
                var value = _dict[key];
                duplicate._dict.Add(key, value);
                duplicate._revDict.Add(value, key);
                duplicate._keys.Add(key);
                duplicate._values.Add(value);
            }

            return duplicate;
        }
    }
}