using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Triggernometry.Core.Serialization;

namespace Triggernometry.Core.Variables
{

    public class VariableStore
    {
       
        public SerializableDictionary<string, VariableScalar> Scalar { get; set; } = new SerializableDictionary<string, VariableScalar>();

        public SerializableDictionary<string, VariableList> List { get; set; } = new SerializableDictionary<string, VariableList>();

        public SerializableDictionary<string, VariableTable> Table { get; set; } = new SerializableDictionary<string, VariableTable>();

        public SerializableDictionary<string, VariableDictionary> Dict { get; set; } = new SerializableDictionary<string, VariableDictionary>();

        /// <summary> Return a new instance if not exist. Store the new instance if <paramref name="storeNew"/>.</summary>
        public TValue GetVariable<TValue>(Dictionary<string, TValue> variables, string name, bool storeNew) where TValue : new()
        {
            lock (variables)
            {
                if (variables.TryGetValue(name, out var existing))
                {
                    return existing;
                }
                TValue vl = new TValue();
                if (storeNew == true)
                {
                    variables[name] = vl;
                }
                return vl;
            }
        }

        /// <summary> Return a new instance if not exist. Store the new instance if <paramref name="storeNew"/>.</summary>
        public VariableScalar GetScalarVariable(string name, bool storeNew)
        {
            return GetVariable(Scalar, name, storeNew);
        }

        /// <summary> Return a new instance if not exist. Store the new instance if <paramref name="storeNew"/>.</summary>
        public VariableList GetListVariable(string name, bool storeNew)
        {
            return GetVariable(List, name, storeNew);
        }

        /// <summary> Return a new instance if not exist. Store the new instance if <paramref name="storeNew"/>.</summary>
        public VariableTable GetTableVariable(string name, bool storeNew)
        {
            return GetVariable(Table, name, storeNew);
        }

        /// <summary> Return a new instance if not exist. Store the new instance if <paramref name="storeNew"/>.</summary>
        public VariableDictionary GetDictVariable(string name, bool storeNew)
        {
            return GetVariable(Dict, name, storeNew);
        }

        /// <summary> Null if not exist. </summary>
        public TValue GetVariable<TValue>(Dictionary<string, TValue> variables, string name) where TValue : class
        {
            lock (variables)
            {
                return variables.TryGetValue(name, out var existing) ? existing : null;
            }
        }

        /// <summary> Null if not exist. </summary>
        public VariableScalar GetScalarVariable(string name) => GetVariable(Scalar, name);

        /// <summary> Null if not exist. </summary>
        public VariableList GetListVariable(string name) => GetVariable(List, name);

        /// <summary> Null if not exist. </summary>
        public VariableTable GetTableVariable(string name) => GetVariable(Table, name);

        /// <summary> Null if not exist. </summary>
        public VariableDictionary GetDictVariable(string name) => GetVariable(Dict, name);


        public void UnsetAllVariables<TValue>(Dictionary<string, TValue> variables)
        {
            lock (variables)
            {
                variables.Clear();
            }
        }

        public void UnsetVariable<TValue>(Dictionary<string, TValue> variables, string name)
        {
            lock (variables)
            {
                if (variables.ContainsKey(name) == true)
                {
                    variables.Remove(name);
                }
            }
        }

        public void UnsetVariableRegex<TValue>(Dictionary<string, TValue> variables, Regex rex)
        {            
            lock (variables)
            {
                List<string> keysToRemove = variables.Keys.Where(key => rex.IsMatch(key)).ToList();
                foreach (string key in keysToRemove)
                {
                    variables.Remove(key);
                }
            }
        }

        public void UnsetVariableRegex<TValue>(Dictionary<string, TValue> variables, string rex)
        {
            UnsetVariableRegex(variables, new Regex(rex));
        }

    }

}
