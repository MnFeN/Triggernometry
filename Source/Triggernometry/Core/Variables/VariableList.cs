using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.Core.Variables
{

    [XmlRoot(ElementName = "VariableList")]
    public class VariableList : Variable
    {

        [XmlArrayItem(ElementName = "VariableScalar", Type = typeof(VariableScalar))]
        [XmlArrayItem(ElementName = "VariableList", Type = typeof(VariableList))]
        [XmlArrayItem(ElementName = "VariableDictionary", Type = typeof(VariableDictionary))]
        [XmlArrayItem(ElementName = "VariableTable", Type = typeof(VariableTable))]
        public List<Variable> Values { get; set; } = new List<Variable>();

        public VariableList() { }
        public VariableList(IEnumerable<string> values) { Values = values.Select(v => (Variable)new VariableScalar(v)).ToList(); }
        public VariableList(IEnumerable<double> values) : this(values.Select(v => v.ToStringInvariant())) { }
        public VariableList(IEnumerable<int> values) : this(values.Select(v => v.ToString(CultureInfo.InvariantCulture))) { }

        public int Size
        {
            get { return Values.Count; }
        }

        public VariableList(IEnumerable<object> objs)
        {
            foreach (object obj in objs)
            {
                Values.Add(new VariableScalar() { Value = obj is double || obj is float ? obj.ToDataString() : obj.ToString() });
            }
        }

        public override string ToString()
        {
            return string.Join(",", Values);
        }

        public override int CompareTo(object o)
        {
            if (o is Variable == false)
            {
                throw new InvalidOperationException();
            }
            if (o is VariableScalar)
            {
                return 1;
            }
            if (o is VariableList)
            {
                VariableList v = (VariableList)o;
                if (v.Values.Count > Values.Count)
                {
                    return -1;
                }
                if (v.Values.Count < Values.Count)
                {
                    return 1;
                }
                for (int i = 0; i < Values.Count; i++)
                {
                    int res = Values[i].CompareTo(v.Values[i]);
                    if (res != 0)
                    {
                        return res;
                    }
                }
                return 0;
            }
            return -1;
        }

        private int ProcessRawIndex(int rawIndex)
        {   // rawIndex: starts from 1; could be negative
            return rawIndex < 0 ? rawIndex + Size : rawIndex - 1;
        }

        public override Variable Duplicate()
        {
            VariableList v = new VariableList();
            foreach (Variable vx in Values)
            {
                // Copy values directly; restore the container metadata after cloning.
                v.Values.Add(vx.Duplicate());
            }
            v.LastChanger = LastChanger;
            v.LastChanged = LastChanged;
            return v;
        }

        public void Push(Variable value, string changer)
        {
            Values.Add(value);
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public Variable Pop(int rawIndex, string changer)
        {
            int index = ProcessRawIndex(rawIndex);
            if (index >= 0 && index < Size)
            {
                Variable x = Values[index];
                Values.RemoveAt(index);
                LastChanged = DateTime.Now;
                LastChanger = changer;
                return x;
            }
            else return new VariableScalar();
        }
        
        public void Insert(int rawIndex, Variable value, string changer)
        {
            int idx = ProcessRawIndex(rawIndex);
            if (idx < 0) { return; }
            if (idx >= Values.Count)
            {
                int needcre = idx - Values.Count;
                while (needcre > 0)
                {
                    Push(new VariableScalar() { Value = "" }, changer);
                    needcre--;
                }                
            }
            Values.Insert(idx, value);
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public void Insert(int rawIndex, string value, string changer)
        {
            Insert(rawIndex, new VariableScalar() { Value = value }, changer);
        }

        public void InsertList(int rawIndex, VariableList srcList, string changer)
        {
            int idx = ProcessRawIndex(rawIndex);
            if (idx < 0) { return; }
            // Copy before modifying the target, since the source may be this list.
            var values = srcList.Values.Select(value => value.Duplicate()).ToArray();
            if (idx >= Values.Count)
            {
                int needcre = idx - Values.Count;
                while (needcre > 0)
                {
                    Push(new VariableScalar() { Value = "" }, changer);
                    needcre--;
                }
            }
            Values.InsertRange(idx, values);
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public void Set(int rawIndex, Variable value, string changer)
        {
            int idx = ProcessRawIndex(rawIndex);
            if (idx < 0)
            {
                return;
            }
            if (idx >= Values.Count)
            {
                int needcre = idx - Values.Count + 1;
                while (needcre > 0)
                {
                    Push(new VariableScalar() { Value = "" }, changer);
                    needcre--;
                }
            }
            Values[idx] = value;
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public void Set(int rawIndex, string value, string changer)
        {
            Set(rawIndex, new VariableScalar() { Value = value }, changer);
        }

        public void Remove(int rawIndex, string changer)
        {
            int idx = ProcessRawIndex(rawIndex);
            if (idx >= 0 && idx < Values.Count)
            {
                Values.RemoveAt(idx);
                LastChanged = DateTime.Now;
                LastChanger = changer;
            }
        }

        public Variable Peek(int rawIndex, string defaultValue = "")
        {
            int idx = ProcessRawIndex(rawIndex);
            if (idx < 0 || idx >= Values.Count)
            {
                return new VariableScalar(defaultValue);
            }
            return Values[idx] ?? new VariableScalar(defaultValue);
        }

        public void RemoveAll(string changer)
        {
            Values.Clear();
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public int IndexOf(Variable value)
        {
            for (int i = 0; i < Values.Count; i++)
            {
                if (Values[i].CompareTo(value) == 0)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public int IndexOf(string value)
        {
            return IndexOf(new VariableScalar() { Value = value });
        }

        public int LastIndexOf(Variable value)
        {
            for (int i = Values.Count - 1; i >= 0; i--)
            {
                if (Values[i].CompareTo(value) == 0)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public int LastIndexOf(string value)
        {
            return LastIndexOf(new VariableScalar() { Value = value });
        }

        public string IndicesOf(string targetStr, string joiner, IEnumerable<int> indices)
        {   // accepts indices starts from 0; returns indices starts from 1
            var selectedIndices = indices.Where(i => Values[i].ToString() == targetStr).Select(i => i + 1).ToList();
            return string.Join(joiner, selectedIndices);
        }

        public int Count(string str, IEnumerable<int> indices)
        {
            return indices.Count(index => Values[index].ToString() == str);
        }

        public double Sum(IEnumerable<int> indices)
        {
            double sum = 0;
            foreach (int i in indices)
            {
                if (double.TryParse(Values[i].ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    sum += value;
            }
            return sum;
        }

        public string Join(string joiner, IEnumerable<int> indices)
        {
            return string.Join(joiner, indices.Select(index => Values[index].ToString()));
        }

        public string Join(string joiner)
        {
            return string.Join(joiner, Values);
        }

        public void SortAlphaAsc(string changer)
        {
            Values.Sort((a, b) => a.CompareTo(b));
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public void SortAlphaDesc(string changer)
        {
            Values.Sort((a, b) => a.CompareTo(b) * -1);
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        private void SortNumeric(string changer, bool descending)
        {
            if (Values.Count > 1)
            {
                var values = Values.ToArray();
                var keys = values.Select(value => decimal.Parse(value.ToString(), CultureInfo.InvariantCulture)).ToArray();
                Array.Sort(keys, values, descending
                    ? Comparer<decimal>.Create((a, b) => b.CompareTo(a))
                    : Comparer<decimal>.Default);
                Values.Clear();
                Values.AddRange(values);
            }
            LastChanged = DateTime.Now;
            LastChanger = changer;
        }

        public void SortNumericAsc(string changer) => SortNumeric(changer, false);

        public void SortNumericDesc(string changer) => SortNumeric(changer, true);



        public static VariableList Build(string expression, char separator, string changer)
        {   // in actions
            string[] elements = expression.Split(separator);
            VariableList vl = new VariableList();
            foreach (string element in elements)
            {
                vl.Values.Add(new VariableScalar { Value = element });
            }
            vl.LastChanger = changer;
            vl.LastChanged = DateTime.Now;
            return vl;
        }

        public static VariableList BuildTemp(string expression)
        {   // in expressions: ${?l: 1, 2, 3 [xxx]}
            string[] elements = ArgHelper.SplitArguments(expression);
            VariableList vl = new VariableList();
            foreach (string element in elements)
            {
                vl.Values.Add(new VariableScalar { Value = element });
            }
            return vl;
        }

    }

}
