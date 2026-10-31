using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Expressions.Maths;
using Triggernometry.Localization;

namespace Triggernometry.Core.Conditions
{

    public sealed class ConditionSingle : ConditionComponent
    {
        
        public enum ExprTypeEnum
        {
            String,
            Numeric
        }

        public enum CndTypeEnum
        {
            NumericEqual,
            NumericNotEqual,
            NumericGreater,
            NumericGreaterEqual,
            NumericLess,
            NumericLessEqual,
            NumericApproxEqual,
            NumericApproxNotEqual,
            NumericTrue,
            NumericFalse,
            StringEqualCase,
            StringEqualNocase,
            StringNotEqualCase,
            StringNotEqualNocase,
            RegexMatch,
            RegexNotMatch,
            ListContains,
            ListDoesNotContain
        }

        private string _ExpressionL;
        private ExprTypeEnum _ExpressionTypeL;
        private string _ExpressionR;
        private ExprTypeEnum _ExpressionTypeR;
        private CndTypeEnum _ConditionType;


        [XmlAttribute("L")]
        public string ExpressionL
        {
            get => _ExpressionL;
            set
            {
                if (value != _ExpressionL)
                {
                    _ExpressionL = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        [XmlAttribute]
        public ExprTypeEnum ExpressionTypeL
        {
            get => _ExpressionTypeL;
            set
            {
                if (value != _ExpressionTypeL)
                {
                    _ExpressionTypeL = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        [XmlAttribute("R")]
        public string ExpressionR
        {
            get => _ExpressionR;
            set
            {
                if (value != _ExpressionR)
                {
                    _ExpressionR = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        [XmlAttribute]
        public ExprTypeEnum ExpressionTypeR
        {
            get => _ExpressionTypeR;
            set
            {
                if (value != _ExpressionTypeR)
                {
                    _ExpressionTypeR = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        [XmlAttribute("Op")]
        public CndTypeEnum ConditionType
        {
            get => _ConditionType;
            set
            {
                if (value != _ConditionType)
                {
                    _ConditionType = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        /// <summary>数值条件的操作符已确定表达式类型，无需重复写入。</summary>
        public bool ShouldSerializeExpressionTypeL() => !IsNumericComparison;

        /// <summary>仅保存参与当前比较的右侧表达式。</summary>
        public bool ShouldSerializeExpressionR() => !IsUnaryComparison;

        /// <summary>仅非数值、双侧比较需要保存右侧表达式类型。</summary>
        public bool ShouldSerializeExpressionTypeR() => !IsNumericComparison && !IsUnaryComparison;

        // 旧属性只参与读取，保存时统一使用 L、R 和 Op。
        [XmlAttribute("ExpressionL")]
        public string Xml_ExpressionL
        {
            get => null;
            set => ExpressionL = value;
        }

        [XmlAttribute("ExpressionR")]
        public string Xml_ExpressionR
        {
            get => null;
            set => ExpressionR = value;
        }

        [XmlAttribute("ConditionType")]
        public string Xml_ConditionType
        {
            get => null;
            set => ConditionType = XmlAttr.Enum<CndTypeEnum>(value);
        }

        /// <summary>数值条件始终按数值表达式求值，忽略旧配置中的表达式类型。</summary>
        internal bool IsNumericComparison => ConditionType >= CndTypeEnum.NumericEqual && ConditionType <= CndTypeEnum.NumericFalse;

        /// <summary>真假判断仅使用左侧表达式，右侧保留供切换比较方式时使用。</summary>
        internal bool IsUnaryComparison => ConditionType == CndTypeEnum.NumericTrue || ConditionType == CndTypeEnum.NumericFalse;

        private string Capitalize(string str)
        {
            if (str == null)
            {
                return null;
            }
            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }
            return str.ToUpper();
        }

        public override string ToString()
        {
            string desc = "";
            string descL = ExpressionL != null && ExpressionL.Length > 256 ? ExpressionL.Substring(0, 256) + "..." : ExpressionL;
            string descR = ExpressionR != null && ExpressionR.Length > 256 ? ExpressionR.Substring(0, 256) + "..." : ExpressionR;
            if (ConditionType == CndTypeEnum.NumericTrue)
                return I18n.Translate("internal/ConditionSingle/numerictrue", "({0}) is true", descL);
            if (ConditionType == CndTypeEnum.NumericFalse)
                return I18n.Translate("internal/ConditionSingle/numericfalse", "({0}) is false", descL);
            if (ConditionType == CndTypeEnum.ListContains || ConditionType == CndTypeEnum.ListDoesNotContain)
            {
                desc = I18n.Translate("internal/ConditionSingle/listvar", "List named by");
                desc += " ";
                switch (ExpressionTypeL)
                {
                    case ExprTypeEnum.Numeric:
                        desc += I18n.Translate("internal/ConditionSingle/numericexpression", "number ({0})", descL);
                        break;
                    case ExprTypeEnum.String:
                        desc += I18n.Translate("internal/ConditionSingle/stringexpression", "text ({0})", descL);
                        break;
                }
                desc += " ";
                switch (ConditionType)
                {
                    case CndTypeEnum.ListContains:
                        desc += I18n.Translate("internal/ConditionSingle/listmustcontain", "contains");
                        break;
                    case CndTypeEnum.ListDoesNotContain:
                        desc += I18n.Translate("internal/ConditionSingle/listcantcontain", "does not contain");
                        break;
                }
            }
            else
            {
                switch (IsNumericComparison ? ExprTypeEnum.Numeric : ExpressionTypeL)
                {
                    case ExprTypeEnum.Numeric:
                        desc = IsNumericComparison ? $"({descL})"
                            : Capitalize(I18n.Translate("internal/ConditionSingle/numericexpression", "number ({0})", descL));
                        break;
                    case ExprTypeEnum.String:
                        desc = Capitalize(I18n.Translate("internal/ConditionSingle/stringexpression", "text ({0})", descL));
                        break;
                }
                desc += " ";
                switch (ConditionType)
                {
                    case CndTypeEnum.NumericEqual:
                        desc += I18n.Translate("internal/ConditionSingle/numericequal", "numerically =");
                        break;
                    case CndTypeEnum.NumericNotEqual:
                        desc += I18n.Translate("internal/ConditionSingle/numericnotequal", "numerically ≠");
                        break;
                    case CndTypeEnum.NumericGreater:
                        desc += I18n.Translate("internal/ConditionSingle/numericgreater", "numerically >");
                        break;
                    case CndTypeEnum.NumericGreaterEqual:
                        desc += I18n.Translate("internal/ConditionSingle/numericgreaterequal", "numerically ≥");
                        break;
                    case CndTypeEnum.NumericLess:
                        desc += I18n.Translate("internal/ConditionSingle/numericless", "numerically <");
                        break;
                    case CndTypeEnum.NumericLessEqual:
                        desc += I18n.Translate("internal/ConditionSingle/numericlessequal", "numerically ≤");
                        break;
                    case CndTypeEnum.NumericApproxEqual:
                        desc += I18n.Translate("internal/ConditionSingle/numericapproxequal", "numerically ≈");
                        break;
                    case CndTypeEnum.NumericApproxNotEqual:
                        desc += I18n.Translate("internal/ConditionSingle/numericapproxnotequal", "numerically ≄");
                        break;
                    case CndTypeEnum.StringEqualCase:
                        desc += I18n.Translate("internal/ConditionSingle/stringequalcase", "equals (case-sensitive)");
                        break;
                    case CndTypeEnum.StringEqualNocase:
                        desc += I18n.Translate("internal/ConditionSingle/stringequalnocase", "equals (ignore case)");
                        break;
                    case CndTypeEnum.StringNotEqualCase:
                        desc += I18n.Translate("internal/ConditionSingle/stringnotequalcase", "differs from (case-sensitive)");
                        break;
                    case CndTypeEnum.StringNotEqualNocase:
                        desc += I18n.Translate("internal/ConditionSingle/stringnotequalnocase", "differs from (ignore case)");
                        break;
                    case CndTypeEnum.RegexMatch:
                        desc += I18n.Translate("internal/ConditionSingle/regexmatch", "matches regex");
                        break;
                    case CndTypeEnum.RegexNotMatch:
                        desc += I18n.Translate("internal/ConditionSingle/noregexmatch", "does not match regex");
                        break;
                }
            }            
            switch (IsNumericComparison ? ExprTypeEnum.Numeric : ExpressionTypeR)
            {
                case ExprTypeEnum.Numeric:
                    desc += " " + (IsNumericComparison ? $"({descR})"
                        : I18n.Translate("internal/ConditionSingle/numericexpression", "number ({0})", descR));
                    break;
                case ExprTypeEnum.String:
                    desc += " " + I18n.Translate("internal/ConditionSingle/stringexpression", "text ({0})", descR);
                    break;
            }            
            return Capitalize(desc);
        }

        internal override ConditionComponent Duplicate()
        {
            ConditionSingle cs = new ConditionSingle();
            cs.ConditionType = ConditionType;
            cs.Enabled = Enabled;
            cs.ExpressionL = ExpressionL;
            cs.ExpressionR = ExpressionR;
            cs.ExpressionTypeL = ExpressionTypeL;
            cs.ExpressionTypeR = ExpressionTypeR;            
            return cs;
        }

        internal string GetExpressionResultL(Context ctx, Context.LoggerDelegate logger, object o)
            => GetExpressionResult(ExpressionL ?? "", IsNumericComparison ? ExprTypeEnum.Numeric : ExpressionTypeL, ctx, logger, o);
        internal string GetExpressionResultR(Context ctx, Context.LoggerDelegate logger, object o)
            => GetExpressionResult(ExpressionR ?? "", IsNumericComparison ? ExprTypeEnum.Numeric : ExpressionTypeR, ctx, logger, o);

        private string GetExpressionResult(string expr, ExprTypeEnum exprType, Context ctx, Context.LoggerDelegate logger, object o)
        {
            switch (exprType)
            {
                case ExprTypeEnum.Numeric:
                    return I18n.ThingToString(ctx.EvaluateNumericExpression(logger, o, expr));
                case ExprTypeEnum.String:
                    return ctx.EvaluateStringExpression(logger, o, expr);
                default:
                    throw new Exception(I18n.Translate("internal/ConditionSingle/invalidexpressiontype", "Invalid expression type: {0}", exprType));
            }
        }

        internal override bool Check(Context ctx, Context.LoggerDelegate logger, object o)
        {
            try
            {
                if (Enabled == false)
                {
                    return false;
                }
                if (IsNumericComparison)
                {
                    double left = ctx.EvaluateNumericExpression(logger, o, ExpressionL ?? "");
                    if (ConditionType == CndTypeEnum.NumericTrue)
                        return !MathParser.IsZero(left);
                    if (ConditionType == CndTypeEnum.NumericFalse)
                        return MathParser.IsZero(left);
                    double right = ctx.EvaluateNumericExpression(logger, o, ExpressionR ?? "");
                    // 保留浮点容限，避免表达式的舍入误差影响相等及边界比较。
                    switch (ConditionType)
                    {
                        case CndTypeEnum.NumericEqual:
                            return MathParser.IsZero(left - right);
                        case CndTypeEnum.NumericNotEqual:
                            return !MathParser.IsZero(left - right);
                        case CndTypeEnum.NumericGreater:
                            return left > right + MathParser.TOLERANCE;
                        case CndTypeEnum.NumericGreaterEqual:
                            return left + MathParser.TOLERANCE >= right;
                        case CndTypeEnum.NumericLess:
                            return left + MathParser.TOLERANCE < right;
                        case CndTypeEnum.NumericLessEqual:
                            return left <= right + MathParser.TOLERANCE;
                        case CndTypeEnum.NumericApproxEqual:
                            return Math.Abs(left - right) <= 0.5;
                        case CndTypeEnum.NumericApproxNotEqual:
                            return Math.Abs(left - right) > 0.5;
                    }
                }
                string lval = GetExpressionResultL(ctx, logger, o);
                string rval = GetExpressionResultR(ctx, logger, o);
                switch (ConditionType)
                {
                    case CndTypeEnum.StringEqualCase:
                        {
                            return string.Compare(lval, rval, false) == 0;
                        }
                    case CndTypeEnum.StringEqualNocase:
                        {
                            return string.Compare(lval, rval, true) == 0;
                        }
                    case CndTypeEnum.StringNotEqualCase:
                        {
                            return string.Compare(lval, rval, false) != 0;
                        }
                    case CndTypeEnum.StringNotEqualNocase:
                        {
                            return string.Compare(lval, rval, true) != 0;
                        }
                    case CndTypeEnum.RegexMatch:
                        {
                            return Regex.IsMatch(lval, rval);
                        }
                    case CndTypeEnum.RegexNotMatch:
                        {
                            return Regex.IsMatch(lval, rval) == false;
                        }
                    case CndTypeEnum.ListContains:
                        {
                            lock (ctx.Plugin.GetVariableStore(false).List)
                            {
                                if (ctx.Plugin.GetVariableStore(false).List.ContainsKey(lval) == true)
                                {
                                    if (ctx.Plugin.GetVariableStore(false).List[lval].IndexOf(rval) > 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                            return false;
                        }
                    case CndTypeEnum.ListDoesNotContain:
                        {
                            lock (ctx.Plugin.GetVariableStore(false).List)
                            {
                                if (ctx.Plugin.GetVariableStore(false).List.ContainsKey(lval) == true)
                                {
                                    if (ctx.Plugin.GetVariableStore(false).List[lval].IndexOf(rval) > 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                            return true;
                        }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

    }

}
