using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{
    /// <summary>将文本表达式的结果写入系统剪贴板。</summary>
    [Action("Copy to clipboard", ActionAttribute.CategoryType.SystemInteraction)]
    [XmlRoot(ElementName = "ActionClipboard")]
    public class ActionClipboard : ActionBase
    {
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Value")]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value, "", omitWhitespace: false);
            set => Value = value;
        }

        /// <summary>显示要写入剪贴板的文本表达式。</summary>
        internal override string DescribeImplementation()
            => I18n.Translate("internal/Action/descclipboard", "Copy ({0}) to clipboard", Value);

        /// <summary>解析表达式，并通过现有 UI 线程入口写入或清空剪贴板。</summary>
        internal override void ExecuteImplementation(Context ctx)
        {
            string text = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
            RealPlugin.ClipboardSetText(text);
            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose,
                I18n.Translate("internal/Action/clipboardset", "Set text ({0}) to clipboard", text));
        }

        /// <summary>将旧版标量动作中的剪贴板操作转换为文本表达式。</summary>
        public static explicit operator ActionClipboard(ActionOld oldAction)
        {
            var action = new ActionClipboard();
            oldAction.CopyCommonPropertiesTo(action);
            // 旧逻辑优先读取非空白的变量名；永久变量通过 pvar 模板访问。
            action.Value = !string.IsNullOrWhiteSpace(oldAction._VariableName)
                ? "${" + (oldAction._VariablePersist ? "pvar:" : "var:") + oldAction._VariableName + "}"
                : oldAction._VariableExpression;
            return action;
        }
    }
}
