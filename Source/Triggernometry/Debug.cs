using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Triggernometry
{
    public static class Debug
    {
        public static void Show(params object[] objs)
        {
            string data = string.Join(Environment.NewLine, objs.Select(o => o?.ToString() ?? "(null)"));
            MessageBox.Show(data, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Action.ClipboardSetText(data);
        }

        public static void Log(params object[] objs)
        {
            RealPlugin.plug.InvokeNamedCallback("command", $"/e \n" + string.Join(Environment.NewLine, objs.Select(o => o?.ToString() ?? "(null)")));
        }

        public static object Show(this object o)
        {
            MessageBox.Show(o?.ToString() ?? "(null)", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Action.ClipboardSetText(o?.ToString() ?? "(null)");
            return o;
        }

        public static object Log(this object o)
        {
            RealPlugin.plug.InvokeNamedCallback("command", "/e \n" + o?.ToString() ?? "(null)");
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

        public static string TimeIt(int times, System.Action testAction)
        {
            Stopwatch stopwatch = new Stopwatch();
            List<double> msResults = new List<double>();

            for (int i = 0; i < times; i++)
            {
                stopwatch.Restart();
                testAction();
                stopwatch.Stop();
                msResults.Add(stopwatch.Elapsed.TotalMilliseconds);
            }
            msResults = msResults.OrderBy(x => x).ToList();
            double avr = msResults.Average();
            double min = msResults[0];
            double max = msResults[times - 1];
            double med10 = msResults[(int)(times * 0.1)];
            double med25 = msResults[(int)(times * 0.25)];
            double med50 = msResults[(int)(times * 0.5)];
            double med75 = msResults[(int)(times * 0.75)];
            double med90 = msResults[(int)(times * 0.9)];
            return $@"Avr: {avr:0.000} ms
Min: {min:0.000} ms
10%: {med10:0.000} ms
25%: {med25:0.000} ms
50%: {med50:0.000} ms
75%: {med75:0.000} ms
90%: {med90:0.000} ms
Max: {max:0.000} ms";
        }


    }
}