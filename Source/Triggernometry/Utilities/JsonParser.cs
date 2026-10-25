using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.Utilities
{
    internal class JsonParser
    {
        /// <summary>
        /// Parses a JSON root into dictionaries, lists and scalar values.
        /// </summary>
        public object Parse(string json)
        {
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                return ParseElement(document.RootElement);
            }
        }

        /// <summary>
        /// Converts an element recursively without retaining the source document.
        /// </summary>
        private object ParseElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Null: return null;
                case JsonValueKind.True: return true;
                case JsonValueKind.False: return false;
                case JsonValueKind.String: return element.GetString();
                case JsonValueKind.Number:
                    return element.TryGetDecimal(out decimal value) ? (object)value : element.GetDouble();
                case JsonValueKind.Object:
                    var dictionary = new Dictionary<string, object>();
                    foreach (JsonProperty property in element.EnumerateObject())
                    {
                        dictionary[property.Name] = ParseElement(property.Value);
                    }
                    return dictionary;
                case JsonValueKind.Array:
                    var list = new List<object>();
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        list.Add(ParseElement(item));
                    }
                    return list;
                default: throw new JsonException();
            }
        }

        /// <summary>
        /// Formats scalar query results as text and preserves complex results as JSON.
        /// </summary>
        internal static string ToValueString(object value)
        {
            if (value is IDictionary || value is IList) return JsonSerializer.Serialize(value);
            if (value is double d) return d.ToStringInvariant();
            if (value is float f) return f.ToStringInvariant();
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
