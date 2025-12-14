using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Triggernometry.Core;

namespace Triggernometry
{
    public static class Debug
    {
        public static void Show(params object[] objs)
        {
            string data = string.Join(Environment.NewLine, objs.Select(o => o?.ToString() ?? "(null)"));
            MessageBox.Show(data, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Core.ActionOld.ClipboardSetText(data);
        }

        public static void Log(params object[] objs)
        {
            RealPlugin.Instance.InvokeNamedCallback("command", $"/e \n" + string.Join(Environment.NewLine, objs.Select(o => o?.ToString() ?? "(null)")));
        }

        public static object Show(this object o)
        {
            MessageBox.Show(o?.ToString() ?? "(null)", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Core.ActionOld.ClipboardSetText(o?.ToString() ?? "(null)");
            return o;
        }

        public static object Log(this object o)
        {
            RealPlugin.Instance.InvokeNamedCallback("command", "/e \n" + o?.ToString() ?? "(null)");
            return o;
        }

        public static string Reflect<T>() => Reflect(typeof(T));

        public static string Reflect(this Type type)
        {
            if (type == null) return "Type is null";

            var sb = new StringBuilder();
            sb.AppendLine($"Reflecting Type: {type.FullName}");

            // Fields
            sb.AppendLine();
            sb.AppendLine("Fields:");
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                             .Where(f => !f.IsDefined(typeof(CompilerGeneratedAttribute), false));
            foreach (var f in fields)
            {
                sb.AppendLine($"  {f.Name} : {f.FieldType.Name};");
            }

            // Properties
            sb.AppendLine();
            sb.AppendLine("Properties:");
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                 .Where(p => !p.IsDefined(typeof(CompilerGeneratedAttribute), false));
            foreach (var p in properties)
            {
                sb.AppendLine($"  {p.Name} : {p.PropertyType.Name};");
            }

            // Methods
            sb.AppendLine();
            sb.AppendLine("Methods:");
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                              .Where(m => !m.IsDefined(typeof(CompilerGeneratedAttribute), false) && !m.IsSpecialName);
            foreach (var m in methods)
            {
                string parameters = string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                sb.AppendLine($"  {m.Name} : {m.ReturnType.Name} ({parameters});");
            }

            // Nested Types
            var nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
            if (nestedTypes.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Nested Types:");
                foreach (var nt in nestedTypes)
                    sb.AppendLine($"  {nt.FullName}");
            }

            return sb.ToString();
        }

        public static string Reflect(this object obj)
        {
            if (obj == null) return "Object is null";

            Type type = obj.GetType();
            var sb = new StringBuilder();
            sb.AppendLine($"Reflecting: {type.Name}");

            // Fields
            sb.AppendLine();
            sb.AppendLine("Fields:");
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                             .Where(f => !f.IsDefined(typeof(CompilerGeneratedAttribute), false));
            foreach (var f in fields)
            {
                object value;
                try { value = f.GetValue(obj); }
                catch { value = "[inaccessible]"; }
                sb.AppendLine($"  {f.Name} : {f.FieldType.Name} = {value};");
            }

            // Properties
            sb.AppendLine();
            sb.AppendLine("Properties:");
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                 .Where(p => !p.IsDefined(typeof(CompilerGeneratedAttribute), false));
            foreach (var p in properties)
            {
                object value;
                try
                {
                    value = p.GetIndexParameters().Length == 0 ? p.GetValue(obj) : "[Indexed Property]";
                }
                catch
                {
                    value = "[inaccessible]";
                }
                sb.AppendLine($"  {p.Name} : {p.PropertyType.Name} = {value};");
            }

            // Methods
            sb.AppendLine();
            sb.AppendLine("Methods:");
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                              .Where(m => !m.IsDefined(typeof(CompilerGeneratedAttribute), false) && !m.IsSpecialName);
            foreach (var m in methods)
            {
                string parameters = string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
                sb.AppendLine($"  {m.Name} : {m.ReturnType.Name} ({parameters});");
            }

            return sb.ToString();
        }

        public static T Method<T>(this object o, string name, params object[] args)
        { 
            return (T)o.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Invoke(o, args);
        }

        public static T Property<T>(this object o, string name)
        {
            return (T)o.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).GetValue(o);
        }

        public static T Field<T>(this object o, string name)
        {
            return (T)o.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).GetValue(o);
        }

        public static string[] TimeIt(int iterationsPerSample, int sampleCount, params System.Action[] testActions)
        {
            if (testActions == null)
                throw new ArgumentNullException(nameof(testActions));
            if (testActions.Length == 0)
                throw new ArgumentException("At least one test action is required.", nameof(testActions));
            if (iterationsPerSample <= 0)
                throw new ArgumentOutOfRangeException(nameof(iterationsPerSample));
            if (sampleCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(sampleCount));

            int actionCount = testActions.Length;

            // 为每个 Action 维护一组采样结果
            var results = new List<double>[actionCount];
            for (int i = 0; i < actionCount; i++)
            {
                results[i] = new List<double>(sampleCount);
            }

            var stopwatch = new Stopwatch();

            // 交替执行 sample：
            // 一个 sample 的流程是：
            //   对每个 action：
            //     连续执行 iterationsPerSample 次，记录本次 sample 的平均耗时
            for (int sample = 0; sample < sampleCount; sample++)
            {
                for (int i = 0; i < actionCount; i++)
                {
                    System.Action action = testActions[i];
                    if (action == null)
                        throw new ArgumentException($"testActions[{i}] is null.", nameof(testActions));

                    stopwatch.Restart();
                    for (int j = 0; j < iterationsPerSample; j++)
                    {
                        action();
                    }
                    stopwatch.Stop();

                    // 记录“每组调用”的平均耗时（毫秒）
                    results[i].Add(stopwatch.Elapsed.TotalMilliseconds);
                }
            }

            // 对每组 action 计算统计量并输出一段字符串
            var output = new string[actionCount];
            for (int i = 0; i < actionCount; i++)
            {
                var msResults = results[i];
                msResults.Sort();

                double avr = msResults.Average();
                double min = msResults[0];
                double max = msResults[msResults.Count - 1];

                int n = msResults.Count;
                double p10 = msResults[(int)(n * 0.1)];
                double p25 = msResults[(int)(n * 0.25)];
                double p50 = msResults[(int)(n * 0.5)];
                double p75 = msResults[(int)(n * 0.75)];
                double p90 = msResults[(int)(n * 0.9)];

                output[i] = $@"Avr: {avr:0.000} ms / {iterationsPerSample} times ({avr / iterationsPerSample:0.00000} ms / time)
Min: {min:0.000} ms
10%: {p10:0.000} ms
25%: {p25:0.000} ms
50%: {p50:0.000} ms
75%: {p75:0.000} ms
90%: {p90:0.000} ms
Max: {max:0.000} ms";
            }

            return output;
        }

    }
}