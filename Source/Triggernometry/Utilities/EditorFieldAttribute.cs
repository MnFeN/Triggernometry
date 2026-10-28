using System;

namespace Triggernometry.Utilities
{
    /// <summary>声明配置项的标签和显示顺序。</summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class EditorFieldAttribute : Attribute
    {
        public int Order { get; }
        public string Label { get; set; }
        /// <summary>可选的标签翻译键；缺省根据声明类型和成员名生成。</summary>
        public string TranslationKey { get; set; }
        public string LabelQualifier { get; set; }
        /// <summary>可选的标签限定词翻译键。</summary>
        public string QualifierTranslationKey { get; set; }

        /// <summary>设置配置项的显示顺序。</summary>
        public EditorFieldAttribute(int order = 0) => Order = order;
    }

    /// <summary>使用文本框编辑配置项，可默认展开为多行。</summary>
    public class TextEditorAttribute : EditorFieldAttribute
    {
        /// <summary>创建编辑器时是否默认展开为多行。</summary>
        public bool Multiline { get; set; }
        /// <summary>设置文本编辑器的显示顺序。</summary>
        public TextEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>表达式输入框的校验类型。</summary>
    public enum EditorExpressionType { String, Numeric, Regex, Color }
    /// <summary>表达式输入框使用的名称补全来源。</summary>
    public enum EditorAutofillType { None, Scalar, List, Table, Dict, Image, Text, Callback, Storage, Registered }

    /// <summary>使用表达式输入框，并指定校验类型和名称补全来源。</summary>
    public class ExpressionEditorAttribute : TextEditorAttribute
    {
        public EditorExpressionType ExpressionType { get; }
        public EditorAutofillType Autofill { get; set; }
        /// <summary>设置表达式编辑器的显示顺序和校验类型。</summary>
        public ExpressionEditorAttribute(int order = 0, EditorExpressionType expressionType = EditorExpressionType.String) : base(order)
            => ExpressionType = expressionType;
    }

    /// <summary>使用枚举值或自定义选项生成下拉框。</summary>
    public class EnumChoiceEditorAttribute : EditorFieldAttribute
    {
        public string[] ChoiceLabels { get; set; }
        public int[] ChoiceValues { get; set; }
        /// <summary>可选的选项翻译键前缀；缺省使用字段的翻译键。</summary>
        public string ChoiceTranslationKey { get; set; }
        /// <summary>设置选项编辑器的显示顺序。</summary>
        public EnumChoiceEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>使用勾选列表编辑可组合的枚举标志位。</summary>
    public sealed class EnumFlagsEditorAttribute : EnumChoiceEditorAttribute
    {
        /// <summary>设置标志位编辑器的显示顺序。</summary>
        public EnumFlagsEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>使用复选框编辑布尔值。</summary>
    public sealed class CheckBoxEditorAttribute : EditorFieldAttribute
    {
        /// <summary>设置复选框编辑器的显示顺序。</summary>
        public CheckBoxEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>使用数值框编辑配置项，支持范围、精度和步长设置。</summary>
    public sealed class NumericEditorAttribute : EditorFieldAttribute
    {
        public double Minimum { get; set; } = int.MinValue;
        public double Maximum { get; set; } = int.MaxValue;
        public double Increment { get; set; } = 1;
        public int DecimalPlaces { get; set; }
        /// <summary>设置数值编辑器的显示顺序。</summary>
        public NumericEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>路径选择按钮使用的文件或目录类型。</summary>
    public enum EditorFileType { Any, Executable, Audio, Image, Directory }

    /// <summary>为路径表达式附加文件或目录选择按钮。</summary>
    public sealed class FileEditorAttribute : ExpressionEditorAttribute
    {
        public EditorFileType FileType { get; }
        /// <summary>设置路径编辑器的显示顺序和文件选择类型。</summary>
        public FileEditorAttribute(int order = 0, EditorFileType fileType = EditorFileType.Any) : base(order) => FileType = fileType;
    }

    /// <summary>引用选择器允许选择的对象类型。</summary>
    public enum EditorReferenceType { Trigger, Folder, Repository }

    /// <summary>通过节点选择对象，并将其 GUID 作为配置值。</summary>
    public sealed class ReferenceEditorAttribute : EditorFieldAttribute
    {
        public EditorReferenceType ReferenceType { get; }
        /// <summary>设置引用选择器的显示顺序和对象类型。</summary>
        public ReferenceEditorAttribute(int order, EditorReferenceType referenceType) : base(order) => ReferenceType = referenceType;
    }

    /// <summary>为颜色表达式附加颜色选择按钮。</summary>
    public sealed class ColorEditorAttribute : ExpressionEditorAttribute
    {
        /// <summary>设置颜色编辑器的显示顺序和颜色校验模式。</summary>
        public ColorEditorAttribute(int order = 0) : base(order, EditorExpressionType.Color) { }
    }

    /// <summary>为字体表达式附加字体选择按钮。</summary>
    public sealed class FontEditorAttribute : ExpressionEditorAttribute
    {
        /// <summary>设置字体编辑器的显示顺序。</summary>
        public FontEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>为按键表达式附加按键记录按钮。</summary>
    public sealed class KeypressEditorAttribute : ExpressionEditorAttribute
    {
        public bool Keycode { get; set; }
        /// <summary>设置按键编辑器的显示顺序。</summary>
        public KeypressEditorAttribute(int order = 0) : base(order) { }
    }

    /// <summary>在指定变量名输入框旁添加永久变量开关。</summary>
    public sealed class PersistenceEditorAttribute : EditorFieldAttribute
    {
        public string VariableMember { get; }
        /// <summary>指定永久变量开关的显示顺序和关联输入框。</summary>
        public PersistenceEditorAttribute(int order, string variableMember) : base(order) => VariableMember = variableMember;
    }
}
