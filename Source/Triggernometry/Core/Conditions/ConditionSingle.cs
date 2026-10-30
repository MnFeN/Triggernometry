using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Linq.Expressions;
using Triggernometry.Localization;
using Triggernometry.Expressions.Maths;

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


        [XmlAttribute]
        public string ExpressionL
        {
            get
            {
                return _ExpressionL;
            }
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
            get
            {
                return _ExpressionTypeL;
            }
            set
            {
                if (value != _ExpressionTypeL)
                {
                    _ExpressionTypeL = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        [XmlAttribute]
        public string ExpressionR
        {
            get
            {
                return _ExpressionR;
            }
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
            get
            {
                return _ExpressionTypeR;
            }
            set
            {
                if (value != _ExpressionTypeR)
                {
                    _ExpressionTypeR = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        [XmlAttribute]
        public CndTypeEnum ConditionType
        {
            get
            {
                return _ConditionType;
            }
            set
            {
                if (value != _ConditionType)
                {
                    _ConditionType = value;
                    TriggerOnPropertyChange();
                }
            }
        }

        /// <summary>数值比较始终将两侧作为数值表达式，忽略旧配置中的表达式类型。</summary>
        internal bool IsNumericComparison => ConditionType >= CndTypeEnum.NumericEqual && ConditionType <= CndTypeEnum.NumericLessEqual;

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
