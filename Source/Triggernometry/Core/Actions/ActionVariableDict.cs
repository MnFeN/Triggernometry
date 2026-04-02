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

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Dictionary variable operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Variable)]
    [XmlRoot(ElementName = "VariableDict")]
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
        public enum ExpressionTypeEnum
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
        [Action(order: 1)]
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
        [Action(order: 2)]
        public ExpressionTypeEnum KeyType { get; set; } = ExpressionTypeEnum.String;

        [XmlAttribute("KeyType")]
        public string Xml_KeyType
        {
            get => XmlAttr.Enum(KeyType, ExpressionTypeEnum.String);
            set => KeyType = XmlAttr.Enum<ExpressionTypeEnum>(value);
        }

        /// <summary>
        /// Type of the value expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public ExpressionTypeEnum ValueType { get; set; } = ExpressionTypeEnum.String;

        [XmlAttribute("ValueType")]
        public string Xml_ValueType
        {
            get => XmlAttr.Enum(ValueType, ExpressionTypeEnum.String);
            set => ValueType = XmlAttr.Enum<ExpressionTypeEnum>(value);
        }

        /// <summary>
        /// Name of the dictionary variable
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name);
            set => Name = value;
        }

        /// <summary>
        /// Name of the target variable for some operations
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public string TargetVariable { get; set; } = "";

        [XmlAttribute("TargetVariable")]
        public string Xml_TargetVariable
        {
            get => XmlAttr.String(TargetVariable);
            set => TargetVariable = value;
        }

        [XmlIgnore]
        [Action(order: 6)]
        public string VariableLength { get; set; } = "";

        [XmlAttribute("VariableLength")]
        public string Xml_VariableLength
        {
            get => XmlAttr.String(VariableLength);
            set => VariableLength = value;
        }

        /// <summary>
        /// Key expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 7)]
        public string Key { get; set; } = "";

        [XmlAttribute("Key")]
        public string Xml_Key
        {
            get => XmlAttr.String(Key);
            set => Key = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 8)]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value);
            set => Value = value;
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [Action(order: 9)] // todo need to couple this with variable on editor
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
        [Action(order: 10)] // todo need to couple this with variable on editor
        public bool TargetPersistent { get; set; } = false;

        [XmlAttribute("TargetPersistent")]
        public string Xml_TargetPersistent
        {
            get => XmlAttr.Bool(TargetPersistent, false);
            set => TargetPersistent = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string sPersistD = I18n.TrlVarPersist(Persistent);
            string tPersistD = I18n.TrlVarPersist(TargetPersistent);
            string keyType = I18n.TrlExprType(KeyType == ExpressionTypeEnum.String);
            string valueType = I18n.TrlExprType(ValueType == ExpressionTypeEnum.String);
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
                    int crcIndex = Value.IndexOf("¡è{");
                    if (
                        ValueType == ExpressionTypeEnum.String
                        && dollarIndex != 0 && dollarIndex != 1 && crcIndex != 0 && crcIndex != 1
                    )
                    {
                        return I18n.Translate(
                            "internal/Action/descdictbuild",
                            "build {1}dict variable ({0}) from string ({2}) separated by ({3}) ({4})",
                            TargetVariable, tPersistD,
                            Value.Length < 2 ? "" : Value.Substring(2),
                            Value.Length < 1 ? "" : Value.Substring(0, 1),
                            Value.Length < 2 ? "" : Value.Substring(1, 1)
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/descdictbuildraw",
                        "build {1}dict variable ({0}) from {3} expression ({2}) separated by its first 2 characters",
                        TargetVariable, tPersistD, Value, valueType
                    );
                case OperationEnum.Filter:
                    return I18n.Translate(
                        "internal/Action/descdictfilter",
                        "Use expression ({4}) to filter {1}dict ({0}) into {3}dict ({2})",
                        Name, sPersistD, TargetVariable, tPersistD, Value
                    );
                case OperationEnum.SetAll:
                    if (string.IsNullOrWhiteSpace(VariableLength))
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
                        Name, sPersistD, Key, keyType, Value, valueType, VariableLength
                    );
                case OperationEnum.Merge:
                    return I18n.Translate(
                        "internal/Action/descdictmerge",
                        "merge {1}dict variable ({0}) into {3}dict variable ({2}), and keep the values of repeated keys",
                        Name, sPersistD, TargetVariable, tPersistD
                    );                    
                case OperationEnum.MergeHard:
                    return I18n.Translate(
                        "internal/Action/descdictmergehard",
                        "merge {1}dict variable ({0}) into {3}dict variable ({2}), and overwrite the values of repeated keys",
                        Name, sPersistD, TargetVariable, tPersistD
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

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string sourcename = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVariable);
            VariableStore svs = plug.GetVariableStore(Persistent);
            VariableStore tvs = plug.GetVariableStore(TargetPersistent);
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);

            string ParseKey()
            {
                if (KeyType == ExpressionTypeEnum.String)
                    return ctx.EvaluateStringExpression(ActionContextLogger, ctx, Key);
                else
                    return I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Key));

            }
            string ParseValue()
            {
                if (ValueType == ExpressionTypeEnum.String)
                    return ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                else
                    return I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value));
            }

            string vdchanger;
            if (ctx.Trigger != null)
                vdchanger = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe());
            else
                vdchanger = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe());

            switch (Operation)
            {
                case OperationEnum.UnsetAll:
                    lock (svs.Dict)
                    {
                        svs.Dict.Clear();
                    }
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictunsetall",
                        "All {0}dict variables unset", sPersist));
                    break;
                case OperationEnum.UnsetRegex:                    
                    svs.UnsetVariableRegex(svs.Dict, Name);
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictunsetregex",
                        "All {0}dict variables matching ({1}) unset", sPersist, Name));
                    break;
                case OperationEnum.Unset:
                    svs.UnsetVariable(svs.Dict, sourcename);
                    AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictunset",
                        "Unset {1}dict variable ({0})", sourcename, sPersist));
                    break;
                case OperationEnum.Set:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_this}", "${_idx}", "${_key}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);

                        string key = ParseKey();
                        string value;
                        lock (svs.Dict)
                        {
                            VariableDictionary vd = svs.GetDictVariable(sourcename, true);
                            ctx.dictValue = vd.GetValue(key).ToString(); // for ${_val}
                            value = ParseValue();
                            vd.SetValue(key, value, vdchanger);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictset",
                            "Value of key ({2}) in {1}dict variable ({0}) set to ({3})", sourcename, sPersist, key, value));
                    }
                    break;
                case OperationEnum.Remove:
                    {
                        string key = ParseKey();
                        lock (svs.Dict)
                        {
                            VariableDictionary vd = svs.GetDictVariable(sourcename, true);
                            vd.RemoveKey(key, vdchanger);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictremove",
                            "Removed key ({2}) from {1}dict variable ({0})", sourcename, sPersist, key));
                    }
                    break;
                case OperationEnum.Merge:
                case OperationEnum.MergeHard:
                    {
                        bool shouldOverwrite = Operation == OperationEnum.MergeHard;
                        VariableDictionary svdCopy;
                        lock (svs.Dict)
                        {
                            svdCopy = (VariableDictionary)svs.GetDictVariable(sourcename, false).Duplicate();
                        }
                        lock (tvs.Dict)
                        {
                            VariableDictionary tvd = tvs.GetDictVariable(targetname, true);
                            tvd.Merge(svdCopy, overwriteExistingKeys: shouldOverwrite);
                        }
                        if (shouldOverwrite)
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictmergehard",
                                "Merged {1}dict variable ({0}) into {3}dict variable ({2}) (overwrite repeated keys)",
                                sourcename, sPersist, targetname, tPersist));
                        else
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictmerge",
                                "Merged {1}dict variable ({0}) into {3}dict variable ({2}) (keep repeated keys)",
                                sourcename, sPersist, targetname, tPersist));
                    }
                    break;
                case OperationEnum.GetEntity:
                    {
                        string filterExpr = ParseValue();
                        var entity = XivEntityParser.GetEntityByCondition(filterExpr);

                        var memberExprs = string.IsNullOrWhiteSpace(Key)
                            ? FFXIV.Entity.RecommendedEntityPropNames.Concat(FFXIV.Job.LegalJobPropNames)
                            : ArgHelper.SplitArguments(ParseKey(), false);

                        var vd = new VariableDictionary(memberExprs.ToDictionary(
                            member => member,
                            member => XivEntityParser.EvaluateEntityMember(entity, member)
                        ));

                        lock (svs.Dict)
                        {
                            svs.Dict[sourcename] = vd;
                        }
                        if (entity.Exist)
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictgetentity",
                                "Saved the data of entity ({2}) into {1}dict variable ({0})",
                                sourcename, sPersist, filterExpr));
                        else
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/dictgetentityfail",
                                "Entity ({2}) not found when trying to save into {1}dict variable ({0})",
                                sourcename, sPersist, filterExpr));
                    }
                    break;
                case OperationEnum.Build:
                    {   // Using the first 2 characters in the expression as the separator to split the remaining part into a new dict
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
                            vt = VariableDictionary.Build(splitval, kvSeparator, pairSeparator, vdchanger);
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictbuild",
                                "{1}Dictionary ({0}) built from expression ({2}) splitted by ({3}) ({4})",
                                targetname, tPersist, splitval, kvSeparator, pairSeparator));
                        }
                        else
                        {
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/dictbuildfail",
                                "{1}Dictionary ({0}) cannot be built since expression ({2}) length < 2",
                                targetname, tPersist, expr));
                        }
                        lock (tvs.Dict)
                        {
                            tvs.Dict[targetname] = vt;
                        }
                    }
                    break;
                case OperationEnum.Filter:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_this}", "${_idx}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        VariableDictionary vdResult = new VariableDictionary();
                        lock (svs.Dict)
                        {
                            VariableDictionary vd = svs.GetDictVariable(sourcename, false);
                            foreach (var pair in vd.Values)
                            {
                                ctx.dictKey = pair.Key;                 // for ${_key}
                                ctx.dictValue = pair.Value.ToString();  // for ${_val}
                                double result = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value);
                                if (!MathParser.IsZero(result))
                                {
                                    vdResult.Values[pair.Key] = pair.Value.Duplicate();
                                }
                            }
                        }
                        vdResult.LastChanger = vdchanger;
                        vdResult.LastChanged = DateTime.Now;
                        lock (tvs.Dict)
                        {
                            tvs.Dict[targetname] = vdResult;
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictfilter",
                                "Filtered {4} key-value pairs from {1}dict ({0}) into {3}dict ({2})",
                                sourcename, sPersist, targetname, tPersist, vdResult.Size));
                        }
                    }
                    break;
                case OperationEnum.SetAll:
                    {
                        bool isLengthMode = !string.IsNullOrWhiteSpace(VariableLength);
                        int length = isLengthMode ? (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, VariableLength) : 0;
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_this}" };
                        CheckInvalidDymanicExpr(Key, invalidExprs);
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        VariableDictionary vdNew = new VariableDictionary();
                        lock (svs.Dict)
                        {
                            VariableDictionary vd = svs.GetDictVariable(sourcename, false);
                            ctx.varName = (Persistent ? "pdvar:" : "dvar:") + sourcename;

                            if (isLengthMode)
                            {   // should only use ${_idx} to generate each key/value
                                for (int i = 0; i < length; i++)
                                {
                                    ctx.listIndex = i + 1;
                                    ctx.dictKey = i < vd.Size ? vd.Values.ElementAt(i).Key : "";
                                    ctx.dictValue = i < vd.Size ? vd.Values.ElementAt(i).Value.ToString() : "";
                                    string k = ParseKey();
                                    string v = ParseValue();
                                    vdNew.SetValue(k, v, vdchanger);
                                }
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictsetallbyindex",
                                    "{4} key value pairs in {1}dictionary ({0}) set to ({2}): ({3})",
                                    sourcename, sPersist, Key, Value, length));
                            }
                            else
                            {   // should only use ${_key} and ${_val} to rewrite the list
                                foreach (var pair in vd.Values)
                                {
                                    ctx.dictKey = pair.Key;
                                    ctx.dictValue = pair.Value.ToString();
                                    string k = ParseKey();
                                    string v = ParseValue();
                                    vdNew.SetValue(k, v, vdchanger);
                                }
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/dictsetall",
                                    "All key value pairs in {1}dictionary ({0}) set to ({2}): ({3})",
                                    sourcename, sPersist, Key, Value));
                            }
                            svs.Dict[sourcename] = vdNew;
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
            action.KeyType = (ExpressionTypeEnum)(int)oldAction._DictVariableKeyType;
            action.ValueType = (ExpressionTypeEnum)(int)oldAction._DictVariableValueType;
            action.Name = oldAction._DictVariableName;
            action.TargetVariable = oldAction._DictVariableTarget;
            action.VariableLength = oldAction._DictVariableLength;
            action.Key = oldAction._DictVariableKey;
            action.Value = oldAction._DictVariableValue;
            action.Persistent = oldAction._DictSourcePersist;
            action.TargetPersistent = oldAction._DictTargetPersist;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionVariableDict action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.DictVariable;
            oldAction._DictVariableOp = (ActionOld.DictVariableOpEnum)(int)action.Operation;
            oldAction._DictVariableKeyType = (ActionOld.DictVariableExpTypeEnum)(int)action.KeyType;
            oldAction._DictVariableValueType = (ActionOld.DictVariableExpTypeEnum)(int)action.ValueType;
            oldAction._DictVariableName = action.Name;
            oldAction._DictVariableTarget = action.TargetVariable;
            oldAction._DictVariableLength = action.VariableLength;
            oldAction._DictVariableKey = action.Key;
            oldAction._DictVariableValue = action.Value;
            oldAction._DictSourcePersist = action.Persistent;
            oldAction._DictTargetPersist = action.TargetPersistent;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
