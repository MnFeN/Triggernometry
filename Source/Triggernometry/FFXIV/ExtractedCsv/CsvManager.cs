using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    /// <summary>
    /// Central manager for loading and caching CSV tables and typed row maps.
    /// </summary>
    public class CsvManager
    {
        /// <summary>
        /// Default CsvManager instance used by the plugin.
        /// </summary>
        public static CsvManager Instance { get; } = new CsvManager();

        /// <summary>
        /// Global cache of raw tables (table name → CsvTable).
        /// </summary>
        private readonly Dictionary<string, CsvTable> _tables = new Dictionary<string, CsvTable>(StringComparer.OrdinalIgnoreCase);
        private readonly object _lockTables = new object();

        /// <summary>
        /// Global cache of typed tables (row type → IReadOnlyDictionary&lt;RowIndexKey, TRow&gt; where TRow : TypedCsvRow).
        /// </summary>
        private readonly Dictionary<Type, object> _typedTables = new Dictionary<Type, object>();
        private readonly object _lockTypedTables = new object();

        /// <summary>
        /// Cached mapping from row type name (without suffix) to the corresponding TypedCsvRow type.
        /// </summary>
        private static readonly Dictionary<string, Type> _rowTypeByName = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        static CsvManager()
        {
            // Static initializer: scans assembly and builds name → row type map.

            var baseType = typeof(TypedCsvRow);
            var asm = baseType.Assembly;

            foreach (var t in asm.GetTypes())
            {
                if (t.Namespace == "Triggernometry.FFXIV.ExtractedCsv.Rows" &&
                    !t.IsAbstract &&
                    baseType.IsAssignableFrom(t))
                {
                    // e.g. "ActionRow" → typeof(ActionRow)
                    _rowTypeByName[t.Name] = t;
                }
            }
        }

        /// <summary> Number of loaded raw CSV tables. </summary>
        public int Count
        {
            get
            {
                lock (_lockTables)
                    return _tables.Count;
            }
        }

        /// <summary> Number of loaded typed tables. </summary>
        public int TypedCount
        {
            get
            {
                lock (_lockTypedTables)
                    return _typedTables.Count;
            }
        }

        /// <summary>
        /// Get a loaded raw CSV table by name (without .csv extension, ignore case).
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown if the table has not been loaded.</exception>
        public CsvTable this[string tableName]
            => TryGetTable(tableName, out CsvTable table) ? table
                : throw new KeyNotFoundException($"Table '{tableName}.csv' was not loaded.");

        /// <summary>
        /// Try to get a loaded raw CSV table by name (without .csv extension, ignore case).
        /// </summary>
        /// <returns><c>true</c> if the table is found; otherwise <c>false</c>.</returns>
        public bool TryGetTable(string tableName, out CsvTable table)
        {
            lock (_lockTables)
                return _tables.TryGetValue(tableName, out table);
        }

        /// <summary>
        /// Get a loaded typed table for the specified row type (subclass of TypedCsvRow).
        /// </summary>
        /// <returns>Read-only dictionary of row index to typed row.</returns>
        /// <exception cref="InvalidCastException"> The cached entry does not match the expected type. </exception>
        /// <exception cref="KeyNotFoundException"> The typed table has not been loaded. </exception>
        public IReadOnlyDictionary<RowIndexKey, T> Get<T>() where T : TypedCsvRow
        {
            lock (_lockTypedTables)
            {
                if (_typedTables.TryGetValue(typeof(T), out var o))
                {
                    if (o is IReadOnlyDictionary<RowIndexKey, T> dict)
                        return dict;

                    throw new InvalidCastException(
                        $"Cached table of '{typeof(T)}' is not IReadOnlyDictionary<RowIndexKey, {typeof(T).Name}>."
                    );
                }
            }
            throw new KeyNotFoundException($"Table '{typeof(T)}.csv' was not loaded.");
        }

        /// <summary>
        /// Resolve the CSV folder path. If <paramref name="folder"/> is null,
        /// the path is taken from configuration key 'XivExtractedCsvPath'.
        /// </summary>
        /// <param name="folder">Explicit folder path, or null to use configuration.</param>
        /// <returns>Validated folder path.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when configuration does not contain a valid XivExtractedCsvPath.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// Thrown when the resolved folder does not exist.
        /// </exception>
        private string GetFolderPath(string folder = null)
        {
            if (folder == null &&
                (!RealPlugin.plug.cfg.Constants.TryGetValue("XivExtractedCsvPath", out var v) ||
                 string.IsNullOrEmpty(folder = v.Value.Trim())))
            {
                throw new InvalidOperationException("The current configuration does not contain valid key: 'XivExtractedCsvPath'.");
            }

            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException($"Folder not exist: {folder}");

            return folder;
        }

        /// <summary>
        /// Loads a CSV table by name.
        /// </summary>
        /// <param name="csvName">File name without extension or with '.csv', e.g. "Action" or "Action.csv".</param>
        /// <param name="folderOrDefault">
        /// Folder path. If null, the default XivExtractedCsvPath from configuration is used.
        /// </param>
        public void LoadTable(string csvName, string folderOrDefault)
        {
            var folder = GetFolderPath(folderOrDefault);

            // Ensure .csv extension.
            if (!csvName.Trim().EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                csvName = csvName.Trim() + ".csv";
            }

            var path = Path.Combine(folder, csvName);
            LoadTable(path);
        }

        /// <summary>
        /// Loads a CSV table from the specified path and optionally binds it to a TypedCsvRow subclass.
        /// </summary>
        /// <param name="path">Full file system path to the CSV file.</param>
        /// <param name="type">
        /// Optional TypedCsvRow subclass. If null, the type is resolved by table name.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if the provided type is not a subclass of TypedCsvRow.
        /// </exception>
        public void LoadTable(string path, Type type = null)
        {
            var table = new CsvTable(this, path);
            var typeName = Path.GetFileNameWithoutExtension(path);

            // Cache raw table by name.
            lock (_lockTables)
            {
                _tables[typeName] = table;
            }

            // If type is not given, try to resolve it by table name.
            if (type == null)
            {
                if (!_rowTypeByName.TryGetValue(typeName, out type))
                    return;
            }
            // If type is given explicitly, validate it.
            else if (!typeof(TypedCsvRow).IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    $"The provided type '{type.FullName}' is not a subclass of {typeof(TypedCsvRow).FullName}.",
                    nameof(type));
            }

            // Create strongly-typed dictionary via reflection: CreateTypedDict<TRow>(table).
            var genericMethod = _createTypedDictMethod.MakeGenericMethod(type);
            var dictObj = genericMethod.Invoke(this, new object[] { table });

            // Cache typed table by row type.
            lock (_lockTypedTables)
            {
                _typedTables[type] = dictObj; // Dictionary<RowIndexKey, TRow>
            }
        }

        /// <summary>
        /// Attempts to load a single CSV table, wrapping exceptions with additional context.
        /// </summary>
        /// <param name="filePath">Full file path of the CSV to load.</param>
        private void TryLoadTable(string filePath)
        {
            try
            {
                LoadTable(filePath);
            }
            catch (Exception ex)
            {
                // TODO: replace with proper logging and error reporting if needed.
                throw new Exception($"Failed to load CSV file '{filePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads all CSV files from the specified folder.
        /// </summary>
        /// <param name="folder">
        /// Folder path. If null, the default XivExtractedCsvPath from configuration is used.
        /// </param>
        /// <param name="multiThread">
        /// If true, tables are loaded in parallel; otherwise loaded sequentially.
        /// </param>
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

        /// <summary>
        /// Clears all loaded raw and typed tables.
        /// </summary>
        public void Clear()
        {
            lock (_lockTables)
                _tables.Clear();
            lock (_lockTypedTables)
                _typedTables.Clear();
        }

        /// <summary>
        /// Cached MethodInfo for the generic CreateTypedDict&lt;TRow&gt; method.
        /// </summary>
        private static readonly MethodInfo _createTypedDictMethod =
            typeof(CsvManager).GetMethod("CreateTypedDict", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException("Cannot find method CreateTypedDict in CsvManager.");

        /// <summary>
        /// Creates a strongly-typed dictionary for a given CsvTable and row type.
        /// </summary>
        /// <typeparam name="TRow">TypedCsvRow subclass for this table.</typeparam>
        /// <param name="table">Source CsvTable instance.</param>
        /// <returns>Dictionary mapping RowIndexKey to TRow.</returns>
        [SuppressMessage("Style", "IDE0051:Remove unused private members", Justification = "Used via reflection by _createTypedDictMethod.")]
        private Dictionary<RowIndexKey, TRow> CreateTypedDict<TRow>(CsvTable table) where TRow : TypedCsvRow
        {
            var rowFactory = CsvRow.GetOrCreateFactory(typeof(TRow)); // Func<CsvRow, CsvRow>

            return table.Rows.Values
                .Select(csvRow => (TRow)rowFactory(csvRow))
                .ToDictionary(
                    row => row.Index,
                    row => row
                );
        }
    }
}
