using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Triggernometry.Utilities;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    /// <summary>
    /// 泛型 CSV 表类，用于加载并解析 CSV 文件，并将行映射为强类型 Entry。
    /// </summary>
    public class CsvTable
    {
        /// <summary> 存储这个表的管理器实例 </summary>
        public CsvManager Manager { get; }

        /// <summary>
        /// 表头（第二行）
        /// </summary>
        public IReadOnlyList<string> Headers { get; }

        /// <summary>
        /// 类型信息（第三行）
        /// </summary>
        public IReadOnlyList<string> Types { get; }

        /// <summary>
        /// 数据行（第四行开始）
        /// </summary>
        public IReadOnlyDictionary<RowIndexKey, CsvRow> Rows { get; }

        /// <summary>
        /// 列名 → 列索引（忽略大小写）
        /// </summary>
        internal Dictionary<string, int> HeaderIndex { get; }

        /// <summary>
        /// 加载 CSV 并构建强类型行实例。
        /// </summary>
        /// <summary>
        /// 传入 CSV 文件路径后自动读取，并解析出表头信息及全部数据。
        /// </summary>
        internal CsvTable(CsvManager manager, string filePath)
        {
            var tempTable = new Dictionary<RowIndexKey, CsvRow>();
            // 读取整个 CSV 文件
            var lines = Triggernometry.Utilities.CsvHelper.ReadCsv(filePath);
            if (lines.Count < 3)
                throw new InvalidDataException($"CSV 文件 {filePath} 行数 ({lines.Count}) 不足，无法解析数据。");
            if (lines[0].Length < 1)
                throw new InvalidDataException($"CSV 文件 {filePath} 列数 ({lines[0].Length}) 不足，无法解析数据。");

            // 如果首行是 key,0,1,2,... 的索引行，则跳过
            int headerLine = 0;
            if (lines[0].Length > 0 && lines[0][0].Equals("key", StringComparison.OrdinalIgnoreCase))
                headerLine = 1;

            // 第二行是字段名
            var headers = lines[headerLine];
            // 空字段名改成 unk_索引
            for (int i = 0; i < headers.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(headers[i]))
                    headers[i] = "unk_" + i;
            }
            Headers = headers;

            // 第三行是类型定义
            Types = lines[headerLine + 1];

            // 构建列名 → 列索引
            HeaderIndex = new Dictionary<string, int>(Headers.Count, StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < Headers.Count; i++)
            {
                string name = Headers[i];
                if (string.IsNullOrEmpty(name))
                    continue;

                if (HeaderIndex.ContainsKey(name))
                    throw new InvalidDataException($"CSV 文件 {filePath} 存在重复列名: {name}");

                HeaderIndex.Add(name, i);
            }

            // 第四行起为数据行
            for (int rowIdx = headerLine + 2; rowIdx < lines.Count; rowIdx++)
            {
                var row = lines[rowIdx];
                if (row.Length == 0)
                    throw new InvalidDataException($"CSV 文件 {filePath} 第 {rowIdx + 1} 行为空，无法解析数据。");

                var key = RowIndexKey.Parse(row[0]); // "1" or "1.0"
                if (tempTable.ContainsKey(key))
                    throw new InvalidDataException($"CSV {filePath} 存在重复 key: {key}");

                var rowInstance = new CsvRow(this, key, row);
                tempTable[key] = rowInstance;
            }
            Rows = tempTable;
            Manager = manager;
        }

        public CsvRow this[RowIndexKey index] => Rows[index];
        public CsvRow this[string index] => Rows[RowIndexKey.Parse(index)];
        public bool TryGetRow(RowIndexKey index, out CsvRow row) => Rows.TryGetValue(index, out row);
        public bool TryGetRow(string index, out CsvRow row) => Rows.TryGetValue(RowIndexKey.Parse(index), out row);

        /// <summary>
        /// 返回表头与类型信息的文本表示，每行一组 "Header: Type"。
        /// </summary>
        public string GetHeaderTypeInfo()
        {
            var sb = new StringBuilder();

            int count = System.Math.Min(Headers.Count, Types.Count);
            for (int i = 0; i < count; i++)
            {
                string header = Headers[i];
                string type = Types[i];
                sb.AppendLine($"{header}: {type}");
            }

            return sb.ToString();
        }
    }
}
