using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    public class CsvManager
    {
        public static CsvManager Instance { get; } = new CsvManager();
        
        public int Count
        {
            get
            {
                lock (_lockTables)
                    return _tables.Count;
            }
        }

        public int TypedCount
        {
            get
            {
                lock (_lockTypedTables)
                    return _typedTables.Count;
            }
        }

        // 全局缓存（文件名 → 对应的表实例）
        private readonly Dictionary<string, CsvTable> _tables =
            new Dictionary<string, CsvTable>(StringComparer.OrdinalIgnoreCase);
        private readonly object _lockTables = new object();
        
        // 全局缓存（类型 → 对应的 ReadOnlyCollection<TRow> 表实例）
        private readonly Dictionary<Type, object> _typedTables =
            new Dictionary<Type, object>();
        private readonly object _lockTypedTables = new object();

        public CsvTable this[string tableName]
            => TryGetTable(tableName, out CsvTable table) ? table
                : throw new KeyNotFoundException($"Table '{tableName}.csv' was not loaded.");

        public bool TryGetTable(string tableName, out CsvTable table)
        {
            lock (_lockTables)
                return _tables.TryGetValue(tableName, out table);
        }

        public IReadOnlyList<T> Get<T>() where T : TypedCsvRow
        {
            lock (_lockTypedTables)
            {
                if (_typedTables.TryGetValue(typeof(T), out object o))
                {
                    if (o is IReadOnlyList<T> list)
                        return list;

                    throw new InvalidCastException($"Cached list of '{typeof(T)}' is not of type IReadOnlyList<{typeof(T).Name}>.");
                }
            }
            throw new KeyNotFoundException($"Table '{typeof(T)}.csv' was not loaded.");
        }

        /// <summary>
        /// 如未指定 folder，从配置中获取 csv 文件夹路径 XivExtractedCsvPath。
        /// </summary>
        private string GetFolderPath(string folder = null)
        {
            if (folder == null &&
                (!RealPlugin.plug.cfg.Constants.TryGetValue("XivExtractedCsvPath", out var v) || string.IsNullOrEmpty(folder = v.Value.Trim()))
            )
                throw new InvalidOperationException("The current configuration does not contain valid key: 'XivExtractedCsvPath'.");

            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException($"Folder not exist: {folder}");

            return folder;
        }

        /// <summary>
        /// 加载指定名称的 csv 表格文件。<br />
        /// csvName 为文件名，如 "Action", "Status"。<br />
        /// folder 为文件夹路径，若为 null 则从配置中读取默认路径 XivExtractedCsvPath。
        /// </summary>
        public void LoadTable(string csvName, string folderOrDefault)
        {
            var folder = GetFolderPath(folderOrDefault);
            // ensure .csv extension
            if (!csvName.Trim().EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                csvName = csvName.Trim() + ".csv";
            }
            var path = Path.Combine(folder, csvName);
            LoadTable(path);
        }

        /// <summary>
        /// 加载指定路径的 csv 表格文件。
        /// </summary>
        public void LoadTable(string path, Type type = null)
        {
            var table = new CsvTable(this, path);
            var typeName = Path.GetFileNameWithoutExtension(path);
            lock (_lockTables)
            {
                _tables[typeName] = table;
            }

            // if the type is not given and the strong row type is defined, create strong typed table (List<TRow>)

            if (type == null)
            {
                type = GetType().Assembly.GetTypes().FirstOrDefault(t =>
                    t.Namespace == "Triggernometry.FFXIV.ExtractedCsv.Rows" &&
                    t.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase) &&
                    typeof(TypedCsvRow).IsAssignableFrom(t));
            }

            if (type == null) return;

            var rowFactory = CsvRow.GetOrCreateFactory(type);
            var array = table.Rows.Values.Select(csvRow => rowFactory(csvRow)).ToArray();

            lock (_lockTypedTables)
            {
                _typedTables[type] = array;
            }
        }

        /// <summary>
        /// 尝试加载单个表，失败时记录日志并返回 false。
        /// </summary>
        private void TryLoadTable(string filePath)
        {
            try
            {
                LoadTable(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load CSV file '{filePath}': {ex.Message}", ex); // 需要改
            }
        }

        public void LoadAllTables(string folder = null, bool multiThread = true)
        {
            var files = Directory.GetFiles(
                GetFolderPath(folder),
                "*.csv",
                SearchOption.TopDirectoryOnly);

            if (multiThread)
            {
                Parallel.ForEach(
                    files,
                    new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                    TryLoadTable);
            }
            else
            {
                foreach (var filePath in files)
                {
                    TryLoadTable(filePath);
                }
            }
        }

        public void Clear()
        {
            lock (_lockTables)
                _tables.Clear();
            lock (_lockTypedTables)
                _typedTables.Clear();
        }
    }
}