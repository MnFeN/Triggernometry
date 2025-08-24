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

        public static string Reflect(this object obj)
        {
            if (obj == null) return "Object is null";

            Type type = obj.GetType();
            List<string> result = new List<string> { $"Reflecting: {type.Name}" };

            result.Add($"\nFields:");
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                             .Where(f => !f.IsDefined(typeof(CompilerGeneratedAttribute), false))
                             .Select(f => $"  {f.Name} : {f.FieldType.Name} = {f.GetValue(obj)};");
            result.AddRange(fields);

            result.Add($"\nProperties:");
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                                 .Where(p => !p.IsDefined(typeof(CompilerGeneratedAttribute), false))
                                 .Select(p => $"  {p.Name} : {p.PropertyType.Name} = {(p.GetIndexParameters().Length == 0 ? p.GetValue(obj) : "[Indexed Property]")};");
            result.AddRange(properties);

            result.Add($"\nMethods:");
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                              .Where(m => !m.IsDefined(typeof(CompilerGeneratedAttribute), false) && !m.IsSpecialName)
                              .Select(m => $"  {m.Name} : {m.ReturnType.Name} ({string.Join(", ", m.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"))});");
            result.AddRange(methods);

            return string.Join(Environment.NewLine, result);
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