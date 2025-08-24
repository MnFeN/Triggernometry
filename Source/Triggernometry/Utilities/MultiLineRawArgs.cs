using System;
using System.Collections.Generic;
using System.Linq;

namespace Triggernometry.Utilities
{
    public class MultiLineRawArgs
    {
        private readonly string _originalData;
        private readonly Dictionary<string, string> _data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyDictionary<string, string> Data => _data;

        public MultiLineRawArgs(string rawLines)
        {
            _originalData = rawLines;
            _ = rawLines ?? throw new ArgumentNullException(nameof(rawLines));
            var lines = rawLines
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => line.Contains(":") && !line.StartsWith("//"));
            foreach (var line in lines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                var k = parts[0].Trim();
                var v = parts[1].Trim();
                if (!_data.ContainsKey(k))
                    _data[k] = v;
                else
                    throw new ArgumentException($"Duplicate key '{k}' found in the rawLines input.");
            }
        }

        /// <summary> 尝试查找 key 对应的值，大小写不敏感。</summary>
        public bool TryGet(string key, out string value) => _data.TryGetValue(key, out value);
        /// <summary> 依次尝试查找多个 key 别名对应的值，大小写不敏感。</summary>
        public bool TryGet(out string value, params string[] keys)
        {
            foreach (var key in keys)
            {
                if (_data.TryGetValue(key, out value))
                    return true;
            }
            value = null;
            return false;
        }

        /// <summary> 查找 key 对应的值，大小写不敏感。 </summary>
        /// <exception cref="KeyNotFoundException">键不存在。</exception>
        public string Get(string key) => TryGet(key, out var value)
            ? value
            : throw new KeyNotFoundException($"Key '{key}' was not found.");

        /// <summary> 依次查找多个 key 别名对应的值，大小写不敏感。 </summary>
        /// <exception cref="KeyNotFoundException">键不存在。</exception>
        public string Get(params string[] keys) => TryGet(out var value, keys)
            ? value
            : throw new KeyNotFoundException($"None of the keys '{string.Join("', '", keys)}' were found.");

        public override string ToString() => _originalData;
    }
}