using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Triggernometry.Utilities
{
    public static class DataStringHelper
    {
        public static string ToDataString(this object prop)
        {
            if (prop == null) return "";
            switch (prop)
            {
                case string s:
                    return s;
                case bool b:
                    return b ? "1" : "0";
                case Enum e:
                    return e.ToString();
                case Vector2 v2:
                    return $"{I18n.ThingToString(v2.X)}, {I18n.ThingToString(v2.Y)}";
                case Vector3 v3:
                    return $"{I18n.ThingToString(v3.X)}, {I18n.ThingToString(v3.Y)}, {I18n.ThingToString(v3.Z)}";
                case float f:
                    try
                    {
                        return I18n.ThingToString(f);
                    }
                    catch (Exception)
                    {
                        RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, $"float ({f}) => decimal failed");
                        return f.ToString();
                    }
                case double d:
                    try
                    {
                        return I18n.ThingToString(d);
                    }
                    catch (Exception)
                    {
                        RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, $"double ({d}) => decimal failed");
                        return d.ToString();
                    }
                case IFormattable formattable:
                    return formattable.ToString(null, CultureInfo.InvariantCulture);
                case IEnumerable data:
                    return string.Join(", ", data.Cast<object>().Select(x => x.ToDataString()));
                default:
                    return prop.ToString();
            }
        }

        public static T FromDataString<T>(this string input)
            => (T)input.FromDataString(typeof(T));

        public static object FromDataString(this string input, Type targetType)
        {
            try
            {
                // Nullable<T>
                Type underlyingType = Nullable.GetUnderlyingType(targetType);
                if (underlyingType != null)
                {
                    if (string.IsNullOrEmpty(input))
                        return null;
                    targetType = underlyingType;
                }

                if (targetType == typeof(string))
                    return input;

                if (targetType == typeof(bool))
                {
                    if (bool.TryParse(input, out bool result))
                        return result;
                    else
                        return !MathParser.IsZero(MathParser.Parse(input));
                }

                if (targetType.IsEnum)
                    return Enum.Parse(targetType, input, true);

                if (targetType == typeof(IntPtr))
                    return (IntPtr)(long)MathParser.Parse(input);

                if (targetType == typeof(UIntPtr))
                    return (UIntPtr)(long)MathParser.Parse(input);

                if (targetType == typeof(Guid))
                    return Guid.Parse(input);

                if (targetType == typeof(HexOrDecId))
                    return new HexOrDecId(input);

                if (targetType.IsNumericType())
                {
                    double result = MathParser.Parse(input);
                    return Convert.ChangeType(result, targetType, CultureInfo.InvariantCulture);
                }

                return Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            { 
                throw new ArgumentException($"无法将字符串 '{input}' 转换为 {targetType.Name} 类型。", ex);
            }
        }

        public static bool IsNumericType(this Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        public static T RawInvoke<T>(this Delegate _delegate, params string[] rawArgs)
            => (T)_delegate.RawInvoke(rawArgs);

        public static object RawInvoke(this Delegate _delegate, params string[] rawArgs)
        {
            ParameterInfo[] paramsInfo = _delegate.Method.GetParameters();

            rawArgs = rawArgs ?? new string[0];
            if (rawArgs.Length > paramsInfo.Length)
                throw new ArgumentException($"参数数量过多：期望最多 {paramsInfo.Length} 个参数，但提供了 {rawArgs.Length} 个");

            object[] parameters = new object[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                if (i < rawArgs.Length)
                {
                    Type paramType = paramsInfo[i].ParameterType;
                    try
                    {
                        parameters[i] = rawArgs[i].FromDataString(paramType);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"参数 {i} ({paramsInfo[i].Name}) 无法转换为类型 {paramType}", ex);
                    }
                }
                else
                {
                    if (paramsInfo[i].HasDefaultValue)
                        parameters[i] = paramsInfo[i].DefaultValue;
                    else
                        throw new ArgumentException($"参数 {i} ({paramsInfo[i].Name}) 缺少值且无默认值");
                }
            }

            // 调用内置的 DynamicInvoke 方法执行委托
            object result = _delegate.DynamicInvoke(parameters);
            return _delegate.Method.ReturnType == typeof(void) ? "" : result;
        }

        public readonly struct DefaultArg
        {
            public int Index { get; }
            public object Value { get; }

            public DefaultArg(int index, object value)
            {
                Index = index;
                Value = value;
            }

            public static implicit operator DefaultArg((int, object) tuple)
                => new DefaultArg(tuple.Item1, tuple.Item2);
        }

        public static T ParseArgs<T>(string rawData, T defaultValue)
            => ParseArgs<T>(rawData, (0, defaultValue));

        public static T1 ParseArgs<T1>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict);
        }

        public static (T1, T2) ParseArgs<T1, T2>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict),
                GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict)
            );
        }

        public static (T1, T2, T3) ParseArgs<T1, T2, T3>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict),
                GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict)
            );
        }

        public static (T1, T2, T3, T4) ParseArgs<T1, T2, T3, T4>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5) ParseArgs<T1, T2, T3, T4, T5>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6) ParseArgs<T1, T2, T3, T4, T5, T6>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7) ParseArgs<T1, T2, T3, T4, T5, T6, T7>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict),
                GetArgAndConvertOrDefault<T11>(rawArgs, 10, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict),
                GetArgAndConvertOrDefault<T11>(rawArgs, 10, defaultDict), GetArgAndConvertOrDefault<T12>(rawArgs, 11, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict),
                GetArgAndConvertOrDefault<T11>(rawArgs, 10, defaultDict), GetArgAndConvertOrDefault<T12>(rawArgs, 11, defaultDict),
                GetArgAndConvertOrDefault<T13>(rawArgs, 12, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict),
                GetArgAndConvertOrDefault<T11>(rawArgs, 10, defaultDict), GetArgAndConvertOrDefault<T12>(rawArgs, 11, defaultDict),
                GetArgAndConvertOrDefault<T13>(rawArgs, 12, defaultDict), GetArgAndConvertOrDefault<T14>(rawArgs, 13, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict),
                GetArgAndConvertOrDefault<T11>(rawArgs, 10, defaultDict), GetArgAndConvertOrDefault<T12>(rawArgs, 11, defaultDict),
                GetArgAndConvertOrDefault<T13>(rawArgs, 12, defaultDict), GetArgAndConvertOrDefault<T14>(rawArgs, 13, defaultDict),
                GetArgAndConvertOrDefault<T15>(rawArgs, 14, defaultDict)
            );
        }

        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string rawData, params DefaultArg[] defaults)
        {
            var defaultDict = defaults.ToDictionary(t => t.Index, t => t.Value);
            var rawArgs = Context.SplitArguments(rawData);
            return (
                GetArgAndConvertOrDefault<T1>(rawArgs, 0, defaultDict), GetArgAndConvertOrDefault<T2>(rawArgs, 1, defaultDict),
                GetArgAndConvertOrDefault<T3>(rawArgs, 2, defaultDict), GetArgAndConvertOrDefault<T4>(rawArgs, 3, defaultDict),
                GetArgAndConvertOrDefault<T5>(rawArgs, 4, defaultDict), GetArgAndConvertOrDefault<T6>(rawArgs, 5, defaultDict),
                GetArgAndConvertOrDefault<T7>(rawArgs, 6, defaultDict), GetArgAndConvertOrDefault<T8>(rawArgs, 7, defaultDict),
                GetArgAndConvertOrDefault<T9>(rawArgs, 8, defaultDict), GetArgAndConvertOrDefault<T10>(rawArgs, 9, defaultDict),
                GetArgAndConvertOrDefault<T11>(rawArgs, 10, defaultDict), GetArgAndConvertOrDefault<T12>(rawArgs, 11, defaultDict),
                GetArgAndConvertOrDefault<T13>(rawArgs, 12, defaultDict), GetArgAndConvertOrDefault<T14>(rawArgs, 13, defaultDict),
                GetArgAndConvertOrDefault<T15>(rawArgs, 14, defaultDict), GetArgAndConvertOrDefault<T16>(rawArgs, 15, defaultDict)
            );
        }

        public static T GetArgAndConvertOrDefault<T>(string[] rawArgs, int index, Dictionary<int, object> defaultDict)
        {
            if (index < rawArgs.Length)
            {
                return (T)rawArgs[index].FromDataString(typeof(T));
            }
            else if (defaultDict.TryGetValue(index, out object defaultValue))
            {
                try { return (T)defaultValue; }
                catch { return defaultValue.ToDataString().FromDataString<T>(); } // 如 (object)0 => uint
            }
            else throw new ArgumentException($"缺少 {typeof(T).Name} 参数 #{index} 且未提供默认值。提供的参数：{string.Join(", ", rawArgs)}");
        }

        /// <summary>
        /// 作为 uint 的形式上的 “子类”，用于配合 ToDataString、ParseArgs 自动解析用户输入的多种格式的 id。
        /// </summary>
        public struct HexOrDecId
        {
            public uint Value;
            public const uint Default = 0xE0000000;

            private static readonly Regex HexRegex = new Regex(@"^[0-9A-Fa-f]+$", RegexOptions.Compiled);
            public HexOrDecId(uint value) => Value = value;
            public HexOrDecId(string input)
            {
                input = input.Trim();
                if (string.IsNullOrWhiteSpace(input))
                    Value = Default;
                else if (HexRegex.IsMatch(input))
                    Value = Convert.ToUInt32(input, 16);
                else
                    Value = (uint)MathParser.Parse(input);
            }

            public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

            public static implicit operator uint(HexOrDecId id) => id.Value;
            public static implicit operator HexOrDecId(uint v) => new HexOrDecId(v);
        }
    }
}
