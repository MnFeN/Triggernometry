using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Triggernometry.Localization;
using Triggernometry.Core;
using Triggernometry.Utilities;

namespace Triggernometry.UI.CustomControls
{
    /// <summary>为配置对象提供编辑控件的联动规则。</summary>
    public interface IEditorRulesProvider
    {
        /// <summary>获取字段引用并创建控件联动规则。</summary>
        Action CreateEditorRules(ConfigEditor editor);
    }

    /// <summary>保存可编辑成员的反射信息和编辑器元数据。</summary>
    public sealed class EditorField
    {
        public MemberInfo Member { get; }
        public EditorFieldAttribute Options { get; }
        public Type ValueType { get; }
        public string TranslationKey => Options.TranslationKey ??
            "internal/EditorFields/" + Member.DeclaringType.Name + "/" + Member.Name;

        /// <summary>保存可编辑成员及其编辑器元数据。</summary>
        internal EditorField(MemberInfo member, EditorFieldAttribute options)
        {
            Member = member;
            Options = options;
            ValueType = member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;
        }

        /// <summary>读取对象中该字段或属性的值。</summary>
        public object GetValue(object instance) => Member is PropertyInfo property
            ? property.GetValue(instance) : ((FieldInfo)Member).GetValue(instance);

        /// <summary>将编辑结果写入对象的字段或属性。</summary>
        public void SetValue(object instance, object value)
        {
            if (Member is PropertyInfo property)
                property.SetValue(instance, value);
            else ((FieldInfo)Member).SetValue(instance, value);
        }

        public string DisplayName
        {
            get
            {
                string label = I18n.Translate(TranslationKey,
                    Options.Label ?? ConfigEditor.SplitName(Member.Name));
                if (Options.LabelQualifier == null)
                    return label;
                return I18n.Translate(Options.QualifierTranslationKey ?? TranslationKey + "/Qualifier", Options.LabelQualifier) + " / " + label;
            }
        }
    }

    /// <summary>封装字段控件的取值、赋值和只读操作。</summary>
    public sealed class ConfigValueEditor
    {
        public Control Control { get; }
        /// <summary>读取控件当前值，不写回配置对象。</summary>
        public Func<object> GetValue { get; }
        /// <summary>向控件载入配置值。</summary>
        public Action<object> SetValue { get; }
        /// <summary>设置具体控件的只读或禁用状态。</summary>
        public Action<bool> SetReadOnly { get; }
        /// <summary>让标签和编辑控件分别占满整行。</summary>
        public bool FullWidth { get; set; }
        /// <summary>字段的表达式输入框；普通控件为空。</summary>
        public ExpressionTextBox Expression { get; set; }
        public EditorField Field { get; internal set; }
        public Label Label { get; internal set; }
        /// <summary>联动规则设定的启用状态，独立于整体只读限制。</summary>
        public bool Enabled { get; internal set; } = true;

        /// <summary>将字段启用状态和整体只读限制应用到控件及标签。</summary>
        internal void ApplyState(bool readOnly)
        {
            Control.Enabled = Enabled;
            SetReadOnly(readOnly || !Enabled);
            if (Label != null)
                Label.Enabled = Enabled;
        }

        /// <summary>设置变量名输入框的校验模式和名称补全来源。</summary>
        public void SetVariableNameMode(ExpressionTextBox.AutofillTypeEnum autofill, bool regex)
        {
            Expression.ExpressionType = regex ? ExpressionTextBox.SupportedExpressionTypeEnum.Regex : ExpressionTextBox.SupportedExpressionTypeEnum.String;
            Expression.AutofillType = regex ? ExpressionTextBox.AutofillTypeEnum.None : autofill;
        }

        /// <summary>将当前控件与指定成员的编辑器放在同一行。</summary>
        public string InlineWith { get; set; }

        /// <summary>绑定控件的取值、赋值和只读操作。</summary>
        public ConfigValueEditor(Control control, Func<object> readValue, Action<object> loadValue, Action<bool> setReadOnly)
        {
            Control = control;
            Expression = control as ExpressionTextBox;
            GetValue = readValue;
            SetValue = loadValue;
            SetReadOnly = setReadOnly;
        }
    }

    /// <summary>允许宿主为当前配置成员提供专用编辑控件。</summary>
    public sealed class CreateConfigurationEditorEventArgs : EventArgs
    {
        public EditorField Field { get; }
        public object Instance { get; }
        public ConfigValueEditor Editor { get; set; }

        /// <summary>提供当前成员和配置对象，供宿主替换字段编辑器。</summary>
        internal CreateConfigurationEditorEventArgs(EditorField field, object instance)
        {
            Field = field;
            Instance = instance;
        }
    }

    /// <summary>根据编辑器元数据生成配置表，允许宿主替换字段控件。</summary>
    public class ConfigEditor : UserControl
    {
        private static readonly Dictionary<Type, EditorField[]> fieldCache = new Dictionary<Type, EditorField[]>();
        private TableLayoutPanel table;
        private List<ConfigValueEditor> editors = new List<ConfigValueEditor>();
        private object instance;
        private bool readOnly;
        private bool updating;
        private Action updateRules;

        public event EventHandler<CreateConfigurationEditorEventArgs> CreateEditor;
        public event EventHandler ValueChanged;

        /// <summary>初始化通用配置编辑器及其表格布局。</summary>
        public ConfigEditor()
        {
            // 标签在创建时翻译，控件中的配置值不参与界面翻译。
            Tag = I18n.DoNotTranslate;
            AutoScroll = true;
            DoubleBuffered = true;
            table = CreateTable();
            Controls.Add(table);
        }

        /// <summary>创建标签与编辑控件共用的双列表格。</summary>
        private TableLayoutPanel CreateTable()
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Font = Font, Width = ClientSize.Width,
                Padding = new Padding(6), GrowStyle = TableLayoutPanelGrowStyle.AddRows
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            table.Layout += UpdateLabelColumnWidth;
            return table;
        }

        /// <summary>按最长标签设置列宽，并限制为可用宽度的三分之二。</summary>
        private static void UpdateLabelColumnWidth(object sender, EventArgs e)
        {
            var table = (TableLayoutPanel)sender;
            int preferredWidth = 0;
            foreach (Label label in table.Controls.OfType<Label>())
                if (table.GetColumnSpan(label) == 1)
                    preferredWidth = Math.Max(preferredWidth, label.GetPreferredSize(System.Drawing.Size.Empty).Width + label.Margin.Horizontal);
            int maximumWidth = Math.Max(0, (table.ClientSize.Width - table.Padding.Horizontal) * 2 / 3);
            int width = Math.Min(preferredWidth, maximumWidth);
            // 统一设置标签列宽，避免逐个调整标签而反复触发布局。
            if (table.ColumnStyles[0].Width != width)
                table.ColumnStyles[0].Width = width;
        }

        /// <summary>拆分标识符中的单词，生成缺省显示文本。</summary>
        internal static string SplitName(string name) => Regex.Replace(name, @"(?<=[a-z0-9])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])", " ");

        /// <summary>取得按顺序排列的可编辑成员，并缓存反射结果。</summary>
        public static IReadOnlyList<EditorField> GetFields(Type type)
        {
            lock (fieldCache)
            {
                if (!fieldCache.TryGetValue(type, out EditorField[] fields))
                {
                    fields = type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                        .Where(member => member is FieldInfo field && !field.IsInitOnly ||
                            member is PropertyInfo property && property.CanRead && property.CanWrite && property.GetIndexParameters().Length == 0)
                        .Select(member => new { Member = member, Options = member.GetCustomAttribute<EditorFieldAttribute>() })
                        .Where(item => item.Options != null)
                        .Select(item => new EditorField(item.Member, item.Options))
                        .OrderBy(field => field.Options.Order).ThenBy(field => field.Member.Name, StringComparer.Ordinal).ToArray();
                    fieldCache.Add(type, fields);
                }
                return Array.AsReadOnly(fields);
            }
        }

        /// <summary>绑定配置草稿并初始化控件与联动规则。</summary>
        public void Bind(object value, Predicate<EditorField> include = null, bool applyRules = true)
        {
            updating = true;
            // 需要支持取消编辑时，调用方应传入配置副本。
            try { BindControls(value, include, applyRules); }
            finally { updating = false; }
        }

        /// <summary>为相同字段换绑配置对象，复用当前控件。</summary>
        public void Rebind(object value, bool applyRules = true)
        {
            if (value == null || editors.Any(entry => !entry.Field.Member.DeclaringType.IsInstanceOfType(value)))
                throw new ArgumentException(nameof(value));
            instance = value;
            updateRules = applyRules && value is IEditorRulesProvider provider ? provider.CreateEditorRules(this) : null;
            ReloadValues();
        }

        /// <summary>先构建完整的新表格，再替换当前编辑界面。</summary>
        private void BindControls(object value, Predicate<EditorField> include, bool applyRules)
        {
            // 先在未显示的表格中完成构建，避免切换时绘制不完整的界面。
            var nextTable = CreateTable();
            var nextEditors = new List<ConfigValueEditor>();
            var previousEditors = editors;
            Action nextRules = null;
            nextTable.SuspendLayout();
            try
            {
                if (value != null)
                {
                    foreach (EditorField field in GetFields(value.GetType()).Where(field => include == null || include(field)))
                    {
                        var args = new CreateConfigurationEditorEventArgs(field, value);
                        CreateEditor?.Invoke(this, args);
                        ConfigValueEditor editor = args.Editor ?? CreateDefaultEditor(field);
                        editor.Field = field;
                        editor.Control.Font = Font;
                        editor.Control.Name = field.Member.Name;
                        editor.Control.Dock = DockStyle.Fill;
                        editor.Control.Margin = new Padding(4);
                        editor.SetValue(field.GetValue(value));
                        editor.SetReadOnly(readOnly);
                        if (editor.Expression != null)
                            editor.Expression.TextChanged += (s, e) => OnValueChanged();
                        else if (editor.Control is TextBoxBase text)
                            text.TextChanged += (s, e) => OnValueChanged();
                        nextEditors.Add(editor);
                    }
                    foreach (var entry in nextEditors.Where(entry => entry.InlineWith == null))
                    {
                        Control control = entry.Control;
                        var inline = nextEditors.Where(item => item.InlineWith == entry.Field.Member.Name).ToArray();
                        if (inline.Length > 0)
                        {
                            var group = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, ColumnCount = inline.Length + 1, RowCount = 1 };
                            group.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                            group.Controls.Add(control, 0, 0);
                            for (int i = 0; i < inline.Length; i++)
                            {
                                group.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                                group.Controls.Add(inline[i].Control, i + 1, 0);
                            }
                            control = group;
                        }
                        int row = nextTable.RowCount++;
                        nextTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                        var label = new Label
                        {
                            Text = entry.Field.DisplayName, Tag = I18n.DoNotTranslate, AutoSize = true,
                            Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Margin = new Padding(4, 8, 4, 8)
                        };
                        entry.Label = label;
                        nextTable.Controls.Add(label, 0, row);
                        if (entry.FullWidth)
                        {
                            nextTable.SetColumnSpan(label, 2);
                            row = nextTable.RowCount++;
                            nextTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                            nextTable.Controls.Add(control, 0, row);
                            nextTable.SetColumnSpan(control, 2);
                        }
                        else nextTable.Controls.Add(control, 1, row);
                    }
                }
                if (applyRules && value is IEditorRulesProvider provider)
                {
                    editors = nextEditors;
                    nextRules = provider.CreateEditorRules(this);
                    // 显示新表格前先应用初始联动状态。
                    nextRules?.Invoke();
                }
            }
            catch
            {
                // 构建失败时继续使用原来的编辑控件。
                editors = previousEditors;
                foreach (var entry in nextEditors) entry.Control.Dispose();
                nextTable.Dispose();
                throw;
            }

            SuspendLayout();
            try
            {
                TableLayoutPanel previous = table;
                // 挂载控件前先确定标签列宽。
                UpdateLabelColumnWidth(nextTable, EventArgs.Empty);
                Controls.Add(nextTable);
                // 挂载后恢复字体继承，避免附加控件时反复切换字体。
                nextTable.ResetFont();
                foreach (var entry in nextEditors) entry.Control.ResetFont();
                table = nextTable;
                instance = value;
                updateRules = nextRules;
                editors = nextEditors;
                // 直接释放旧表格，避免先移除时向所有子控件传播后备字体。
                previous.Dispose();
            }
            finally
            {
                nextTable.ResumeLayout(true);
                ResumeLayout(true);
            }
        }

        /// <summary>先校验全部控件，再将编辑结果写回配置对象。</summary>
        public void WriteValues()
        {
            if (instance == null || readOnly)
                return;
            var values = new List<object>(editors.Count);
            foreach (var entry in editors)
            {
                try { values.Add(entry.GetValue()); }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException || ex is ArgumentException)
                {
                    entry.Control.Focus();
                    throw new ArgumentException(I18n.Translate("internal/ConfigurationEditor/invalidvalue",
                        "Invalid value for '{0}': {1}", entry.Field.DisplayName, ex.Message), ex);
                }
            }
            for (int i = 0; i < editors.Count; i++) editors[i].Field.SetValue(instance, values[i]);
        }

        /// <summary>重新加载配置值，并统一刷新联动规则。</summary>
        public void ReloadValues()
        {
            updating = true;
            try
            {
                foreach (var entry in editors) entry.SetValue(entry.Field.GetValue(instance));
            }
            finally { updating = false; }
            OnValueChanged();
        }

        /// <summary>更新联动规则后通知宿主，并阻止递归更新。</summary>
        private void OnValueChanged()
        {
            if (updating)
                return;
            updating = true;
            try { updateRules?.Invoke(); }
            finally { updating = false; }
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public IReadOnlyList<ConfigValueEditor> Editors => editors;

        /// <summary>按成员名称查找已生成的字段编辑器。</summary>
        public ConfigValueEditor GetEditor(string memberName) => editors.First(editor => editor.Field.Member.Name == memberName);

        /// <summary>仅启用指定字段，其余字段及标签设为禁用。</summary>
        public void EnableOnly(params ConfigValueEditor[] enabledFields)
        {
            foreach (var field in editors)
            {
                field.Enabled = enabledFields.Contains(field);
                field.ApplyState(readOnly);
            }
        }

        /// <summary>设置指定表达式输入框的校验类型。</summary>
        public void SetExpressionType(ExpressionTextBox.SupportedExpressionTypeEnum type, params ConfigValueEditor[] fields)
        {
            foreach (var field in fields) field.Expression.ExpressionType = type;
        }

        /// <summary>更新指定字段及标签的启用状态，并应用只读限制。</summary>
        public void SetEnabled(bool value, params ConfigValueEditor[] fields)
        {
            foreach (var field in fields)
            {
                field.Enabled = value;
                field.ApplyState(readOnly);
            }
        }

        /// <summary>设置整体只读状态，保留各字段原有的启用状态。</summary>
        public void SetReadOnly(bool value)
        {
            readOnly = value;
            foreach (var entry in editors) entry.ApplyState(readOnly);
        }

        /// <summary>根据字段元数据和数据类型创建默认编辑控件。</summary>
        private ConfigValueEditor CreateDefaultEditor(EditorField field)
        {
            Type type = field.ValueType;
            if (field.Options is EnumFlagsEditorAttribute flags)
                return CreateFlagsEditor(field, flags);
            if (field.Options is EnumChoiceEditorAttribute choice)
                return CreateChoiceEditor(field, choice);
            if (field.Options is NumericEditorAttribute numeric)
                return CreateNumericEditor(field, numeric);
            if (field.Options is ExpressionEditorAttribute)
                return CreateExpressionEditor(field);
            bool automatic = field.Options.GetType() == typeof(EditorFieldAttribute);
            if (field.Options is CheckBoxEditorAttribute || automatic && type == typeof(bool))
            {
                var box = new CheckBox { AutoSize = true, CheckAlign = System.Drawing.ContentAlignment.MiddleRight };
                box.CheckedChanged += (s, e) => OnValueChanged();
                return new ConfigValueEditor(box, () => box.Checked, value => box.Checked = (bool)value, value => box.Enabled = !value);
            }
            if (automatic && type.IsEnum)
                return type.IsDefined(typeof(FlagsAttribute), false)
                    ? CreateFlagsEditor(field, new EnumFlagsEditorAttribute()) : CreateChoiceEditor(field, new EnumChoiceEditorAttribute());
            bool multiline = field.Options is TextEditorAttribute options && options.Multiline;
            var text = new TextBox { Multiline = multiline, ScrollBars = multiline ? ScrollBars.Both : ScrollBars.None, MaxLength = 0 };
            if (text.Multiline) { text.Height = 100; text.AcceptsReturn = text.AcceptsTab = true; text.WordWrap = false; }
            object original = null;
            return new ConfigValueEditor(text, () =>
            {
                if (type == typeof(string))
                    return original == null && text.Text.Length == 0 ? null : text.Text;
                if (type == typeof(Guid))
                    return Guid.Parse(text.Text);
                if (type.IsEnum)
                    return Enum.Parse(type, text.Text);
                return Convert.ChangeType(text.Text, type, CultureInfo.InvariantCulture);
            }, value => { original = value; text.Text = Convert.ToString(value, CultureInfo.InvariantCulture); }, value => text.ReadOnly = value);
        }

        /// <summary>创建共用的表达式输入框，并载入校验与补全设置。</summary>
        internal static ConfigValueEditor CreateExpressionEditor(EditorField field, Context context = null)
        {
            var options = (ExpressionEditorAttribute)field.Options;
            var expression = new ExpressionTextBox(context)
            {
                ExpressionType = (ExpressionTextBox.SupportedExpressionTypeEnum)options.ExpressionType,
                AutofillType = (ExpressionTextBox.AutofillTypeEnum)options.Autofill
            };
            if (options.Multiline)
                expression.ToggleExpand();
            object original = null;
            return new ConfigValueEditor(expression,
                () => original == null && expression.Expression.Length == 0 ? null : expression.Expression,
                value => { original = value; expression.Expression = (string)value ?? ""; },
                value => expression.ReadOnly = value);
        }

        /// <summary>创建数值编辑器并应用范围、精度和步长设置。</summary>
        private ConfigValueEditor CreateNumericEditor(EditorField field, NumericEditorAttribute options)
        {
            var box = new NumericUpDown
            {
                Minimum = (decimal)options.Minimum, Maximum = (decimal)options.Maximum,
                DecimalPlaces = options.DecimalPlaces, Increment = (decimal)options.Increment
            };
            box.ValueChanged += (s, e) => OnValueChanged();
            return new ConfigValueEditor(box, () =>
            {
                // 保存时读取最新输入，包含尚未失焦的数值。
                return Convert.ChangeType(box.Value, field.ValueType, CultureInfo.InvariantCulture);
            }, value =>
            {
                decimal number = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                // 扩大可编辑范围以容纳旧值，避免载入时将其强制截断。
                box.Minimum = Math.Min((decimal)options.Minimum, number);
                box.Maximum = Math.Max((decimal)options.Maximum, number);
                box.Value = number;
            }, value => { box.ReadOnly = value; box.Enabled = !value; });
        }

        /// <summary>创建枚举或自定义选项的下拉编辑器。</summary>
        private ConfigValueEditor CreateChoiceEditor(EditorField field, EnumChoiceEditorAttribute options)
        {
            var box = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Tag = I18n.DoNotTranslate, DisplayMember = "Value" };
            box.SelectionChangeCommitted += (s, e) => OnValueChanged();
            var choices = new List<object>();
            if (options.ChoiceLabels == null)
            {
                // 未提供自定义选项时，保留枚举声明的实际值。
                choices.AddRange(field.ValueType == typeof(bool) ? new object[] { false, true }
                    : Enum.GetValues(field.ValueType).Cast<object>().Distinct());
                foreach (object value in choices) box.Items.Add(new KeyValuePair<object, string>(value, TranslateEnum(field.ValueType, value)));
            }
            else for (int i = 0; i < options.ChoiceLabels.Length; i++)
            {
                int number = options.ChoiceValues?[i] ?? i;
                object value = field.ValueType == typeof(bool) ? (object)(number != 0)
                    : field.ValueType.IsEnum ? Enum.ToObject(field.ValueType, number)
                    : Convert.ChangeType(number, field.ValueType, CultureInfo.InvariantCulture);
                choices.Add(value);
                box.Items.Add(new KeyValuePair<object, string>(value, TranslateChoice(field, options, i)));
            }
            return new ConfigValueEditor(box, () => ((KeyValuePair<object, string>)box.SelectedItem).Key, value =>
            {
                int index = choices.IndexOf(value);
                if (index < 0)
                {
                    // 保留配置中已有、但当前选项列表未包含的值。
                    index = choices.Count;
                    choices.Add(value);
                    box.Items.Add(new KeyValuePair<object, string>(value, TranslateEnum(field.ValueType, value)));
                }
                box.SelectedIndex = index;
            }, value => box.Enabled = !value);
        }

        /// <summary>使用字段对应的翻译键取得自定义选项文本。</summary>
        private static string TranslateChoice(EditorField field, EnumChoiceEditorAttribute options, int index)
        {
            string label = options.ChoiceLabels[index];
            return I18n.Translate((options.ChoiceTranslationKey ?? field.TranslationKey) + "[" + label + "]", label);
        }

        /// <summary>创建可独立勾选多个标志位的编辑器。</summary>
        private ConfigValueEditor CreateFlagsEditor(EditorField field, EnumFlagsEditorAttribute options)
        {
            Type type = field.ValueType;
            var box = new CheckedListBox { CheckOnClick = true, Tag = I18n.DoNotTranslate, Height = 85, IntegralHeight = false };
            var values = options.ChoiceValues == null ? Enum.GetValues(type).Cast<object>()
                : options.ChoiceValues.Select(value => Enum.ToObject(type, value));
            var flags = values.Where(value =>
            {
                long bits = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                return bits > 0 && (bits & (bits - 1)) == 0;
            }).Distinct().ToArray();
            for (int i = 0; i < flags.Length; i++)
                box.Items.Add(options.ChoiceLabels == null ? TranslateEnum(type, flags[i]) : TranslateChoice(field, options, i));
            bool loading = false;
            // ItemCheck 触发时勾选状态尚未提交，延后读取最终状态。
            box.ItemCheck += (s, e) =>
            {
                if (!loading && box.IsHandleCreated)
                    box.BeginInvoke(new Action(OnValueChanged));
            };
            // 保留当前选项未展示的标志位，避免保存时丢失。
            long unknownBits = 0;
            return new ConfigValueEditor(box, () =>
            {
                long result = unknownBits;
                foreach (int index in box.CheckedIndices) result |= Convert.ToInt64(flags[index], CultureInfo.InvariantCulture);
                return Enum.ToObject(type, result);
            }, value =>
            {
                loading = true;
                try
                {
                    long bits = Convert.ToInt64(value, CultureInfo.InvariantCulture);
                    unknownBits = bits;
                    for (int i = 0; i < flags.Length; i++)
                    {
                        long mask = Convert.ToInt64(flags[i], CultureInfo.InvariantCulture);
                        box.SetItemChecked(i, (bits & mask) != 0);
                        unknownBits &= ~mask;
                    }
                }
                finally { loading = false; }
            }, value => box.Enabled = !value);
        }

        /// <summary>取得枚举项的翻译，缺省使用拆分后的成员名称。</summary>
        internal static string TranslateEnum(Type type, object value) => I18n.Translate(
            "internal/EditorEnums/" + (type.DeclaringType == null ? "" : type.DeclaringType.Name + "/") + type.Name + "/" + value,
            SplitName(value.ToString()));
    }
}
