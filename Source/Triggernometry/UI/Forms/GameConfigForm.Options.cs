using System;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.UI.Forms
{
    public partial class GameConfigForm
    {
        public abstract class Option
        {
            public Label Lbl;               // 左侧的描述标签（如果控件不自带文本描述）
            public Control Ctrl;            // 控件，如 ComboBox
            private readonly ToolTip _tip = new MyToolTip();   // 鼠标悬停时显示提示文本

            /// <summary> 选项对应的触发器配置字典键名。 </summary>
            public string ConfigKey { get; set; } = null;

            public bool Enabled
            {
                get => Ctrl?.Enabled ?? Lbl?.Enabled ?? true;
                set
                {
                    if (Ctrl != null)
                        Ctrl.Enabled = value;
                    if (Lbl != null)
                        Lbl.Enabled = value;
                }
            }

            public bool Visible
            {
                get => Ctrl?.Visible ?? Lbl?.Visible ?? true;
                set
                {
                    if (Ctrl != null)
                        Ctrl.Visible = value;
                    if (Lbl != null)
                        Lbl.Visible = value;
                }
            }

            public event EventHandler DataChanged;
            private bool _isUpdatingData = false;

            protected virtual void OnDataChanged()
            {
                if (_isUpdatingData) return;
                try
                {
                    _isUpdatingData = true;
                    DataChanged?.Invoke(this, EventArgs.Empty);
                }
                finally
                {
                    _isUpdatingData = false;
                }
            }

            public void InitializeData() => OnDataChanged();

            /// <summary>
            /// 在 TableLayoutPanel 末尾添加空行，并将该选项置于这一行。
            /// </summary>
            /// <param name="table">选项所处的父对象 TableLayoutPanel。</param>
            internal virtual void AppendToTable(TableLayoutPanel table)
            {
                table.RowCount++;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                table.Controls.Add(Lbl, 0, table.RowCount - 1);
                if (Ctrl != null)
                    table.Controls.Add(Ctrl, 1, table.RowCount - 1);
                else
                    table.SetColumnSpan(Lbl, 2);
            }

            protected virtual void SetHint(string hint)
            {
                if (!string.IsNullOrWhiteSpace(hint))
                {
                    if (Lbl != null)
                    {
                        _tip.SetToolTip(Lbl, hint);
                        Lbl.Cursor = Cursors.Help;
                    }
                    if (Ctrl != null)
                    {
                        _tip.SetToolTip(Ctrl, hint);
                        Ctrl.Cursor = Cursors.Help;
                    }
                }
            }

            // 子类需要实现从 string 到控件数据的转换
            public abstract string Data { get; set; }

            public virtual void LoadFromConfig(VariableDictionary cfg)
            {
                if (ConfigKey == null) return;
                if (cfg != null && cfg.ContainsKey(ConfigKey))
                {
                    Data = cfg.GetValue(ConfigKey).ToString().Trim();
                }
            }

            public virtual void SaveToConfig(VariableDictionary cfg)
            {
                if (ConfigKey == null) return;
                cfg.SetValue(ConfigKey, Data);
            }
        }

        public class OptionLbl : Option
        {
            public Label Label => Lbl;

            /// <summary>
            /// 生成一个占据整行的 Label。脚本不应直接调用此构造函数。
            /// </summary>
            /// <param name="text">Label 文本</param>
            /// <param name="hint">鼠标悬停时显示的提示文本</param>
            internal OptionLbl(string text, string hint = null)
            {
                Lbl = new MyLabel { Text = text };
                Ctrl = null;
                ConfigKey = null;
                SetHint(hint);
            }

            public override string Data
            {
                get => Lbl.Text;
                set => Lbl.Text = value;
            }
        }

        public class OptionTxt : Option
        {
            public TextBox Txt => (TextBox)Ctrl;

            public OptionTxt(string desc, string configKey, string defaultText = "", string hint = null)
            {
                Lbl = new MyLabel { Text = desc };
                Ctrl = new MyTextBox { Text = defaultText };
                Txt.TextChanged += (sender, e) => OnDataChanged();
                ConfigKey = configKey;
                SetHint(hint);
            }

            public override string Data
            {
                get => Txt.Text.Trim();
                set => Txt.Text = value.Trim();
            }
        }

        public class OptionChk : Option
        {
            public CheckBox Chk => (CheckBox)Ctrl;

            public OptionChk(string desc, string configKey, bool defaultChecked = false, string hint = null)
            {
                Lbl = new MyLabel { Text = desc };
                Ctrl = new MyCheckBox { Checked = defaultChecked };
                Chk.CheckedChanged += (sender, e) => OnDataChanged();
                ConfigKey = configKey;
                SetHint(hint);
            }

            public override string Data
            {
                get => Chk.Checked ? "1" : "0";
                set => Chk.Checked = !MathParser.IsZero(MathParser.Parse(value));
            }

            public void BindChecked(Action<bool> action)
            {
                Chk.CheckedChanged += (sender, e) => action(Chk.Checked);
                action(Chk.Checked);
            }

            public void BindVisibility(params Option[] options)
            {
                void Update()
                {
                    foreach (var option in options)
                        option.Visible = Chk.Checked;
                }

                Chk.CheckedChanged += (sender, e) => Update();
                Update();
            }
        }

        public class OptionCbx : Option
        {
            public ComboBox Cbx => (ComboBox)Ctrl;
            private readonly BijectDictionary<string, string> _data;

            /// <summary>
            /// 根据双向字典 <paramref name="data"/> 生成一个 Label 和 ComboBox 的组合。
            /// </summary>
            /// <param name="desc">Description of the label (left side).</param>
            /// <param name="configKey">The key saved into config dictionary.</param>
            /// <param name="data">BijectDictionary containing the keys and values of the options.</param>
            public OptionCbx(string desc, string configKey, BijectDictionary<string, string> data, string defaultKey, string hint = null)
                : this(desc, configKey, data, data.Keys.IndexOf(defaultKey), hint) { }

            /// <summary>
            /// 根据双向字典 <paramref name="data"/> 生成一个 Label 和 ComboBox 的组合。
            /// </summary>
            /// <param name="desc">左侧 label 描述</param>
            /// <param name="configKey">存储到永久字典变量中的键</param>
            /// <param name="data">字典键与选项文本描述的双向字典</param>
            public OptionCbx(string desc, string configKey, BijectDictionary<string, string> data, int defaultIndex = 0, string hint = null)
            {
                Lbl = new MyLabel { Text = desc };
                _data = data;
                Ctrl = new MyComboBox();
                Cbx.Items.AddRange(data.Values.ToArray());
                Cbx.SelectedIndex = Cbx.Items.Count == 0 ? -1 :
                    (defaultIndex >= 0 && defaultIndex < Cbx.Items.Count ? defaultIndex : 0);
                Cbx.DropDownStyle = ComboBoxStyle.DropDownList;
                Cbx.SelectedIndexChanged += (sender, e) => OnDataChanged();
                ConfigKey = configKey;
                SetHint(hint);
            }

            public override string Data
            {
                get
                {
                    string selection = Cbx.SelectedItem?.ToString() ?? Cbx.SelectedText;
                    return _data.GetKey(selection) ?? selection;
                }
                set
                {
                    string option = value.Trim();
                    Cbx.SelectedItem = _data[option] ?? option;
                }
            }
        }

        public class OptionNud : Option
        {
            public NumericUpDown Nud => (NumericUpDown)Ctrl;

            /// <summary>
            /// 根据数值范围生成一个 Label 和 NumericUpDown 的组合。
            /// </summary>
            /// <param name="desc">左侧 label 描述</param>
            /// <param name="configKey">存储到永久字典变量中的键</param>
            /// <param name="defaultValue">默认值</param>
            /// <param name="minimum">允许的最小值</param>
            /// <param name="maximum">允许的最大值</param>
            /// <param name="decimalPlaces">显示和输入的小数位数</param>
            /// <param name="increment">每次增减的步长</param>
            public OptionNud(
                string desc,
                string configKey,
                decimal defaultValue = 0m,
                decimal minimum = 0m,
                decimal maximum = 100m,
                int decimalPlaces = 0,
                decimal increment = 1m,
                string hint = null)
            {
                Lbl = new MyLabel { Text = desc };
                Ctrl = new MyNumericUpDown
                {
                    Minimum = minimum,
                    Maximum = maximum,
                    DecimalPlaces = decimalPlaces,
                    Increment = increment
                };

                Nud.Value = Math.Max(Nud.Minimum, Math.Min(Nud.Maximum, defaultValue));
                Nud.ValueChanged += (sender, e) => OnDataChanged();

                ConfigKey = configKey;
                SetHint(hint);
            }

            public override string Data
            {
                get => Nud.Value.ToStringInvariant();
                set
                {
                    if (value.TryParseDecimal(out var result))
                        Nud.Value = Math.Max(Nud.Minimum, Math.Min(Nud.Maximum, result));
                }
            }
        }

        public class OptionCustom : Option
        {
            private Func<Control, string> _getter;
            private Action<Control, string> _setter;

            public OptionCustom(
                string desc, string configKey, Control ctrl,
                Func<Control, string> getter,
                Action<Control, string> setter,
                string defaultData, string hint = null)
            {
                Lbl = new MyLabel { Text = desc };
                Ctrl = ctrl;
                ConfigKey = configKey;
                _getter = getter;
                _setter = setter;
                _setter(Ctrl, defaultData);
                SetHint(hint);
            }

            public override string Data
            {
                get => _getter(Ctrl);
                set => _setter(Ctrl, value);
            }
        }
    }
}