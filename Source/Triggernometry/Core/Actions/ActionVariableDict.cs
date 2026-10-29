using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Evaluators;
using Triggernometry.Expressions.String.Parsers;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using Autofill = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Dictionary variable operations
    /// </summary>
    [Action("Dict variable operation", ActionAttribute.CategoryType.Variables)]
    [XmlRoot(ElementName = "ActionVariableDict")]
    public class ActionVariableDict : ActionBase
    {

        #region Properties

        /// <summary>
        /// Dictionary variable operations
        /// </summary>
        public enum OperationEnum
        {
            Unset,
            Set,
            Remove,
            SetAll,
            Build,
            Filter,
            Merge,
            MergeHard,
            GetEntity,
            UnsetAll,
            UnsetRegex,
            [Obsolete] GetEntityByName, // => GetEntity
            [Obsolete] GetEntityById, // => GetEntity
        }

        /// <summary>
        /// Expression types
        /// </summary>
        public enum ExpressionType
        {
            /// <summary>
            /// String expression
            /// </summary>
            String,
            /// <summary>
            /// Numeric expression
            /// </summary>
            Numeric
        }

        /// <summary>
        /// Dictionary variable operation type
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation type",
            ChoiceLabels = new[]
        {
            "Unset dictionary variable",
            "Set value of the given key",
            "Remove the given key",
            "Set all keys and values",
            "Build from the expression",
            "Filter into another dictionary",
            "Merge two dictionaries (keep)",
            "Merge two dictionaries (overwrite)",
            "Get specified properties of a specified FFXIV entity",
            "Unset all dictionary variables",
            "Unset dictionary variables matching regular expression",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.Unset;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Unset);
            set => Operation = XmlAttr.Enum<OperationEnum>(value); // todo 似乎没有处理旧的两个 enum？
        }

        /// <summary>
        /// Type of the key expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(5, Label = "Key Expr type",
            ChoiceLabels = new[]
        {
            "String",
            "Numeric",
        })]
        public ExpressionType KeyType { get; set; } = ExpressionType.String;

        [XmlAttribute("KeyType")]
        public string Xml_KeyType
        {
            get => XmlAttr.Enum(KeyType, ExpressionType.String);
            set => KeyType = XmlAttr.Enum<ExpressionType>(value);
        }

        /// <summary>
        /// Type of the value expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(7, Label = "Value Expr type",
            ChoiceLabels = new[]
        {
            "String",
            "Numeric",
        })]
        public ExpressionType ValueType { get; set; } = ExpressionType.String;

        [XmlAttribute("ValueType")]
        public string Xml_ValueType
        {
            get => XmlAttr.Enum(ValueType, ExpressionType.String);
            set => ValueType = XmlAttr.Enum<ExpressionType>(value);
        }

        /// <summary>
        /// Name of the dictionary variable
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Source variable name", Autofill = EditorAutofillType.Dict)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name, "", omitWhitespace: false);
            set => Name = value;
        }

        /// <summary>
        /// Name of the target variable for some operations
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(9, Label = "Target variable name", Autofill = EditorAutofillType.Dict)]
        public string TargetVar { get; set; } = "";

        [XmlAttribute("TargetVar")]
        public string Xml_TargetVar
        {
            get => XmlAttr.String(TargetVar, "", omitWhitespace: false);
            set => TargetVar = value;
        }

        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(4, Label = "Length")]
        public string Count { get; set; } = "";

        [XmlAttribute("Count")]
        public string Xml_Count
        {
            get => XmlAttr.String(Count, "", omitWhitespace: false);
            set => Count = value;
        }

        /// <summary>
        /// Key expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(6, Label = "Key")]
        public string Key { get; set; } = "";

        [XmlAttribute("Key")]
        public string Xml_Key
        {
            get => XmlAttr.String(Key, "", omitWhitespace: false);
            set => Key = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(8, Label = "Value")]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value, "", omitWhitespace: false);
            set => Value = value;
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(3, nameof(Name), Label = "Source variable name")]
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Indicates whether referenced target variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(10, nameof(TargetVar), Label = "Target variable name")]
        public bool TargetPersistent { get; set; } = false;

        [XmlAttribute("TargetPersistent")]
        public string Xml_TargetPersistent
        {
            get => XmlAttr.Bool(TargetPersistent, false);
            set => TargetPersistent = XmlAttr.Bool(value);
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var name = editor.GetEditor(nameof(Name));
            var length = editor.GetEditor(nameof(Count));
            var key = editor.GetEditor(nameof(Key));
            var keyType = editor.GetEditor(nameof(KeyType));
            var value = editor.GetEditor(nameof(Value));
            var valueType = editor.GetEditor(nameof(ValueType));
            var target = editor.GetEditor(nameof(TargetVar));
            var persistent = editor.GetEditor(nameof(Persistent));
            var targetPersistent = editor.GetEditor(nameof(TargetPersistent));
            return () =>
            {
                var op = (OperationEnum)operation.GetValue();
                switch (op)
                {
                    case OperationEnum.UnsetAll: 
                        editor.EnableOnly(operation); break;
                    case OperationEnum.Set: 
                        editor.EnableOnly(operation, name, key, keyType, value, valueType); break;
                    case OperationEnum.Remove: 
                        editor.EnableOnly(operation, name, key, keyType); break;
                    case OperationEnum.SetAll: 
                        editor.EnableOnly(operation, name, length, key, keyType, value, valueType); break;
                    case OperationEnum.Build: 
                        editor.EnableOnly(operation, value, valueType, target); break;
                    case OperationEnum.Filter: 
                        editor.EnableOnly(operation, name, value, target); break;
                    case OperationEnum.Merge:
                    case OperationEnum.MergeHard: 
                        editor.EnableOnly(operation, name, target); break;
                    case OperationEnum.GetEntity: 
                        editor.EnableOnly(operation, name, key, value); break;
                    default: 
                        editor.EnableOnly(operation, name); break;
                }
                if (op == OperationEnum.Filter) valueType.SetValue(ExpressionType.Numeric);
                else if (op == OperationEnum.GetEntity)
                {
                    keyType.SetValue(ExpressionType.String);
                    valueType.SetValue(ExpressionType.String);
                }
                editor.SetExpressionType((ExpressionType)keyType.GetValue() == ExpressionType.Numeric 
                    ? ExpressionTextBox.SupportedExpressionTypeEnum.Numeric : ExpressionTextBox.SupportedExpressionTypeEnum.String, key);
                editor.SetExpressionType((ExpressionType)valueType.GetValue() == ExpressionType.Numeric 
                    ? ExpressionTextBox.SupportedExpressionTypeEnum.Numeric : ExpressionTextBox.SupportedExpressionTypeEnum.String, value);
                editor.SetExpressionType(ExpressionTextBox.SupportedExpressionTypeEnum.Numeric, length);
                name.SetVariableNameMode(Autofill.Dict, op == OperationEnum.UnsetRegex);
                target.Expression.AutofillType = Autofill.Dict;
                editor.SetEnabled(name.Enabled || op == OperationEnum.UnsetAll, persistent);
                editor.SetEnabled(target.Enabled, targetPersistent);
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            string sPersistD = I18n.TrlVarPersist(Persistent);
            string tPersistD = I18n.TrlVarPersist(TargetPersistent);
            string keyType = I18n.TrlExprType(KeyType == ExpressionType.String);
            string valueType = I18n.TrlExprType(ValueType == ExpressionType.String);
            switch (Operation)
            {
                case OperationEnum.Unset:
                    return I18n.Translate(
                        "internal/Action/descdictunset",
                        "unset {1}dict variable ({0})",
                        Name, sPersistD
                    );                    
                case OperationEnum.Set:
                    return I18n.Translate("internal/Action/descdictset",
                        "set the value of {3} key ({2}) in the {1}dict variable ({0}) to {5} expression ({4})",
                        Name, sPersistD, Key, keyType, Value, valueType
                    );
                case OperationEnum.Remove:
                    return I18n.Translate(
                        "internal/Action/descdictremove",
                        "remove the {3} key ({2}) in the {1}dict variable ({0})",
                        Name, sPersistD, Key, keyType
                    );
                case OperationEnum.Build:
                    int dollarIndex = Value.IndexOf("$");
                    int crcIndex = Value.IndexOf("¤{");
                    if (
                        ValueType == ExpressionType.String
                        && dollarIndex != 0 && dollarIndex != 1 && crcIndex != 0 && crcIndex != 1
                    )
                    {
                        return I18n.Translate(
                            "internal/Action/descdictbuild",
                            "build {1}dict variable ({0}) from string ({2}) separated by ({3}) ({4})",
                            TargetVar, tPersistD,
                            Value.Length < 2 ? "" : Value.Substring(2),
                            Value.Length < 1 ? "" : Value.Substring(0, 1),
                            Value.Length < 2 ? "" : Value.Substring(1, 1)
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/descdictbuildraw",
                        "build {1}dict variable ({0}) from {3} expression ({2}) separated by its first 2 characters",
                        TargetVar, tPersistD, Value, valueType
                    );
                case OperationEnum.Filter:
                    return I18n.Translate(
                        "internal/Action/descdictfilter",
                        "Use expression ({4}) to filter {1}dict ({0}) into {3}dict ({2})",
                        Name, sPersistD, TargetVar, tPersistD, Value
                    );
                case OperationEnum.SetAll:
                    if (string.IsNullOrWhiteSpace(Count))
                    {
                        return I18n.Translate(
                            "internal/Action/descdictsetall",
                            "rewrite all key value pairs in {1}dict ({0}) to {3} expr ({2}) : {5} expr ({4})",
                            Name, sPersistD, Key, keyType, Value, valueType
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/descdictsetallbyindex",
                        "set {6} key value pairs in {1}dict ({0}) to {3} expr ({2}) : {5} expr ({4})",
                        Name, sPersistD, Key, keyType, Value, valueType, Count
                    );
                case OperationEnum.Merge:
                    return I18n.Translate(
                        "internal/Action/descdictmerge",
                        "merge {1}dict variable ({0}) into {3}dict variable ({2}), and keep the values of repeated keys",
                        Name, sPersistD, TargetVar, tPersistD
                    );                    
                case OperationEnum.MergeHard:
                    return I18n.Translate(
                        "internal/Action/descdictmergehard",
                        "merge {1}dict variable ({0}) into {3}dict variable ({2}), and overwrite the values of repeated keys",
                        Name, sPersistD, TargetVar, tPersistD
                    );
                case OperationEnum.GetEntity:
                    {
                        bool hasSpecifiedProps = !string.IsNullOrWhiteSpace(Key);
                        string key = hasSpecifiedProps ? "internal/Action/descdictgetentitygivenprops"
                                                       : "internal/Action/descdictgetentity";
                        string trl = hasSpecifiedProps ? "Store ({3}) properties of the entity ({2}) in {1}dictionary ({0})"
                                                       : "Store all properties of the entity ({2}) in {1}dictionary ({0})";
                        return I18n.Translate(key, trl, Name, sPersistD, Value, Key);
                    }
                case OperationEnum.UnsetAll:
                    return I18n.Translate(
                        "internal/Action/descdictunsetall",
                        "unset all {0}dict variables",
                        sPersistD
                    );
                case OperationEnum.UnsetRegex:
                    return I18n.Translate(
                        "internal/Action/descdictunsetregex",
                        "unset all {0}dict variables matching regular expression ({1})",
                        sPersistD, Name
                    );
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);

            string ParseKey()
            {
                if (KeyType == ExpressionType.String)
                    return EvalString(Key, ctx);
                else
                    return I18n.ThingToString(EvalNumeric(Key, ctx));

            }
            string ParseValue()
            {
                if (ValueType == ExpressionType.String)
                    return EvalString(Value, ctx);
                else
                    return I18n.ThingToString(EvalNumeric(Value, ctx));
            }

            string changer = DescribeVariableChanger(ctx);

            switch (Operation)
            {
                case OperationEnum.UnsetAll:
                    {
                        var srcStore = ctx.Plugin.GetVariableStore(Persistent);
                        lock (srcStore.Dict)
                        {
                            srcStore.Dict.Clear();
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictunsetall",
                            "All {0}dict variables unset", sPersist));
                    }
                    break;
                case OperationEnum.UnsetRegex:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        srcStore.UnsetVariableRegex(srcStore.Dict, srcName);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictunsetregex",
                            "All {0}dict variables matching ({1}) unset", sPersist, srcName));
                    }
                    break;
                case OperationEnum.Unset:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        srcStore.UnsetVariable(srcStore.Dict, srcName);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictunset",
                            "Unset {1}dict variable ({0})", srcName, sPersist));
                    }
                    break;
                case OperationEnum.Set:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_this}", "${_idx}", "${_key}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);

                        string key = ParseKey();
                        string value;
                        lock (srcStore.Dict)
                        {
                            VariableDictionary vd = srcStore.GetDictVariable(srcName, true);
                            ctx.dictValue = vd.GetValue(key).ToString(); // for ${_val}
                            value = ParseValue();
                            vd.SetValue(key, value, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictset",
                            "Value of key ({2}) in {1}dict variable ({0}) set to ({3})", srcName, sPersist, key, value));
                    }
                    break;
                case OperationEnum.Remove:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        string key = ParseKey();
                        lock (srcStore.Dict)
                        {
                            VariableDictionary vd = srcStore.GetDictVariable(srcName, true);
                            vd.RemoveKey(key, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictremove",
                            "Removed key ({2}) from {1}dict variable ({0})", srcName, sPersist, key));
                    }
                    break;
                case OperationEnum.Merge:
                case OperationEnum.MergeHard:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        var (tgtName, tgtStore) = ResolveVariable(ctx, TargetVar, TargetPersistent);
                        bool shouldOverwrite = Operation == OperationEnum.MergeHard;
                        VariableDictionary svdCopy;
                        lock (srcStore.Dict)
                        {
                            svdCopy = (VariableDictionary)srcStore.GetDictVariable(srcName, false).Duplicate();
                        }
                        lock (tgtStore.Dict)
                        {
                            VariableDictionary tvd = tgtStore.GetDictVariable(tgtName, true);
                            tvd.Merge(svdCopy, overwriteExistingKeys: shouldOverwrite);
                            tvd.LastChanged = DateTime.Now;
                            tvd.LastChanger = changer;
                        }
                        if (shouldOverwrite)
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictmergehard",
                                "Merged {1}dict variable ({0}) into {3}dict variable ({2}) (overwrite repeated keys)",
                                srcName, sPersist, tgtName, tPersist));
                        else
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictmerge",
                                "Merged {1}dict variable ({0}) into {3}dict variable ({2}) (keep repeated keys)",
                                srcName, sPersist, tgtName, tPersist));
                    }
                    break;
                case OperationEnum.GetEntity:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        string filterExpr = ParseValue();
                        var entity = XivEntityParser.GetEntityByCondition(filterExpr);

                        var memberExprs = string.IsNullOrWhiteSpace(Key)
                            ? FFXIV.Entity.RecommendedEntityPropNames.Concat(FFXIV.Job.LegalJobPropNames)
                            : ArgHelper.SplitArguments(ParseKey(), false);

                        var vd = new VariableDictionary(memberExprs.ToDictionary(
                            member => member,
                            member => XivEntityParser.EvaluateEntityMember(entity, member)
                        ));
                        vd.LastChanger = changer;

                        lock (srcStore.Dict)
                        {
                            srcStore.Dict[srcName] = vd;
                        }
                        if (entity.Exist)
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictgetentity",
                                "Saved the data of entity ({2}) into {1}dict variable ({0})",
                                srcName, sPersist, filterExpr));
                        else
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/dictgetentityfail",
                                "Entity ({2}) not found when trying to save into {1}dict variable ({0})",
                                srcName, sPersist, filterExpr));
                    }
                    break;
                case OperationEnum.Build:
                    {   // Using the first 2 characters in the expression as the separator to split the remaining part into a new dict
                        var (tgtName, tgtStore) = ResolveVariable(ctx, TargetVar, TargetPersistent);
                        // e.g. expr = ":,aaa:1,bbb:2,ccc:3"
                        VariableDictionary vt = new VariableDictionary();
                        string expr = ParseValue();
                        if (expr.Length > 1)
                        {
                            if (expr[1] == '\n' || expr.Substring(1).StartsWith("\r\n"))
                                expr = ParserCommon.ReplaceLineBreak(expr);
                            char kvSeparator = expr[0];
                            char pairSeparator = expr[1];
                            string splitval = expr.Substring(2);
                            vt = VariableDictionary.Build(splitval, kvSeparator, pairSeparator, changer);
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictbuild",
                                "{1}Dictionary ({0}) built from expression ({2}) splitted by ({3}) ({4})",
                                tgtName, tPersist, splitval, kvSeparator, pairSeparator));
                        }
                        else
                        {
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/dictbuildfail",
                                "{1}Dictionary ({0}) cannot be built since expression ({2}) length < 2",
                                tgtName, tPersist, expr));
                        }
                        lock (tgtStore.Dict)
                        {
                            tgtStore.Dict[tgtName] = vt;
                        }
                    }
                    break;
                case OperationEnum.Filter:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        var (tgtName, tgtStore) = ResolveVariable(ctx, TargetVar, TargetPersistent);
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_this}", "${_idx}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        VariableDictionary vdResult = new VariableDictionary();
                        lock (srcStore.Dict)
                        {
                            VariableDictionary vd = srcStore.GetDictVariable(srcName, false);
                            foreach (var pair in vd.Values)
                            {
                                ctx.dictKey = pair.Key;                 // for ${_key}
                                ctx.dictValue = pair.Value.ToString();  // for ${_val}
                                double result = EvalNumeric(Value, ctx);
                                if (!MathParser.IsZero(result))
                                {
                                    vdResult.Values[pair.Key] = pair.Value.Duplicate();
                                }
                            }
                        }
                        vdResult.LastChanger = changer;
                        vdResult.LastChanged = DateTime.Now;
                        lock (tgtStore.Dict)
                        {
                            tgtStore.Dict[tgtName] = vdResult;
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictfilter",
                                "Filtered {4} key-value pairs from {1}dict ({0}) into {3}dict ({2})",
                                srcName, sPersist, tgtName, tPersist, vdResult.Size));
                        }
                    }
                    break;
                case OperationEnum.SetAll:
                    {
                        var (srcName, srcStore) = ResolveVariable(ctx, Name, Persistent);
                        bool isLengthMode = !string.IsNullOrWhiteSpace(Count);
                        int length = isLengthMode ? (int)EvalNumeric(Count, ctx) : 0;
                        // Only length-based generation establishes an index for each entry.
                        string[] invalidExprs = isLengthMode
                            ? new[] { "${_row", "${_col", "${_this}" }
                            : new[] { "${_row", "${_col", "${_this}", "${_idx}" };
                        CheckInvalidDymanicExpr(Key, invalidExprs);
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        VariableDictionary vdNew = new VariableDictionary();
                        lock (srcStore.Dict)
                        {
                            VariableDictionary vd = srcStore.GetDictVariable(srcName, false);
                            ctx.varName = (Persistent ? "pdvar:" : "dvar:") + srcName;

                            if (isLengthMode)
                            {   // should only use ${_idx} to generate each key/value
                                // 手动枚举 Values，超出原长度使用空值
                                using (var entries = vd.Values.GetEnumerator())
                                {
                                    for (int i = 0; i < length; i++)
                                    {
                                        bool hasEntry = entries.MoveNext();
                                        ctx.listIndex = i + 1;
                                        ctx.dictKey = hasEntry ? entries.Current.Key : "";
                                        ctx.dictValue = hasEntry ? entries.Current.Value.ToString() : "";
                                        string k = ParseKey();
                                        string v = ParseValue();
                                        vdNew.SetValue(k, v, changer);
                                    }
                                }
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictsetallbyindex",
                                    "{4} key value pairs in {1}dictionary ({0}) set to ({2}): ({3})",
                                    srcName, sPersist, Key, Value, length));
                            }
                            else
                            {   // should only use ${_key} and ${_val} to rewrite the list
                                foreach (var pair in vd.Values)
                                {
                                    ctx.dictKey = pair.Key;
                                    ctx.dictValue = pair.Value.ToString();
                                    string k = ParseKey();
                                    string v = ParseValue();
                                    vdNew.SetValue(k, v, changer);
                                }
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictsetall",
                                    "All key value pairs in {1}dictionary ({0}) set to ({2}): ({3})",
                                    srcName, sPersist, Key, Value));
                            }
                            srcStore.Dict[srcName] = vdNew;
                        }
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionVariableDict(ActionOld oldAction)
        {
            var action = new ActionVariableDict();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._DictVariableOp;
            action.KeyType = (ExpressionType)(int)oldAction._DictVariableKeyType;
            action.ValueType = (ExpressionType)(int)oldAction._DictVariableValueType;
            action.Name = oldAction._DictVariableName;
            action.TargetVar = oldAction._DictVariableTarget;
            action.Count = oldAction._DictVariableLength;
            action.Key = oldAction._DictVariableKey;
            action.Value = oldAction._DictVariableValue;
            action.Persistent = oldAction._DictSourcePersist;
            action.TargetPersistent = oldAction._DictTargetPersist;
            return action;
        }

        #endregion Old Action Converter
    }

}
