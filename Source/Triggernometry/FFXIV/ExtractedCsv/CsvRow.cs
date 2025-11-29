using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Triggernometry.Utilities;

namespace Triggernometry.FFXIV.ExtractedCsv
{
    /// <summary>
    /// CSV 行基类，包装原始的字段字典，提供通用访问接口。
    /// </summary>
    public class CsvRow
    {
        public CsvTable Table { get; private set; }
        public CsvManager Manager => Table.Manager;
        public IReadOnlyList<string> Headers => Table.Headers;
        public IReadOnlyList<string> Types => Table.Types;
        public IReadOnlyDictionary<RowIndexKey, CsvRow> Rows => Table.Rows;
        internal Dictionary<string, int> HeaderIndex => Table.HeaderIndex;

        /// <summary>
        /// 当前行的原始数据（列名 → 值）。
        /// </summary>
        public IReadOnlyList<string> Fields { get; private set; }

        /// <summary> 仅用于子类自动生成无参构造函数，供自动转换类型的表达式使用。 </summary>
        protected CsvRow() { }

        internal CsvRow(CsvTable table, string[] fields)
        {
            Fields = fields ?? throw new ArgumentNullException(nameof(fields));
            Table = table ?? throw new ArgumentNullException(nameof(table));
        }

        /// <summary>
        /// 获取字段并转换类型。
        /// </summary>
        public T Get<T>(string key) => Get(key).FromDataString<T>();

        /// <summary>
        /// 根据列名获取字段。
        /// </summary>
        public string Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (!HeaderIndex.TryGetValue(key, out int colIndex))
                throw new KeyNotFoundException("Column '" + key + "' not found.");

            if (colIndex < 0 || colIndex >= Fields.Count)
                throw new IndexOutOfRangeException("Column index " + colIndex + " out of range.");

            return Fields[colIndex];
        }

        /// <summary>
        /// 根据列号获取字段。
        /// </summary>
        public string Get(int colIndex)
        {
            if (colIndex < 0 || colIndex >= Fields.Count)
                throw new IndexOutOfRangeException($"Column index {colIndex} out of range.");

            return Fields[colIndex];
        }

        /// <summary>
        /// 根据列号获取字段并转换类型。
        /// </summary>
        public T Get<T>(int colIndex) => Get(colIndex).FromDataString<T>();

        /// <summary>
        /// 从另一行复制基础字段（Table / Fields）。
        /// 供工厂方法 <see cref="GetOrCreateFactory" /> 使用。
        /// </summary>
        protected void CopyBaseFrom(CsvRow other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Table = other.Table;
            Fields = other.Fields;
        }

        private static readonly Dictionary<Type, Func<CsvRow, CsvRow>> _factoryCache = new Dictionary<Type, Func<CsvRow, CsvRow>>();
        private static readonly object _factoryLock = new object();
        internal static Func<CsvRow, CsvRow> GetOrCreateFactory(Type type)
        {
            lock (_factoryLock)
            {
                if (_factoryCache.TryGetValue(type, out Func<CsvRow, CsvRow> func))
                    return func;

                // 子类自动生成的无参构造函数：
                var ctor = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    Type.EmptyTypes,
                    null) ?? throw new Exception($"CsvRow 子类 {type.Name} 必须实现无参构造函数");

                // 参数 row
                var pRow = Expression.Parameter(typeof(CsvRow), "row");

                // new TRow()
                var newExpr = Expression.New(ctor);

                // TRow 强类型变量
                var varTRow = Expression.Variable(type, "obj");

                // obj = new TRow()
                var assignObj = Expression.Assign(varTRow, newExpr);

                // 找到 CsvRow.CopyBaseFrom(CsvRow) 方法
                var csvRowType = typeof(CsvRow);
                var copyMethod = csvRowType.GetMethod("CopyBaseFrom", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new Exception("CsvRow.CopyBaseFrom(CsvRow) 方法未找到");

                // ((CsvRow)obj).CopyBaseFrom(row);
                var callCopyBase = Expression.Call(
                    Expression.Convert(varTRow, csvRowType),
                    copyMethod,
                    pRow
                );

                // return obj;
                var returnLabel = Expression.Label(typeof(CsvRow));
                var returnExpr = Expression.Return(returnLabel, varTRow, typeof(CsvRow));
                var returnLabelTarget = Expression.Label(returnLabel, Expression.Default(typeof(CsvRow)));

                // 组合表达式块
                var block = Expression.Block(
                    new[] { varTRow },
                    assignObj,
                    callCopyBase,
                    returnExpr,
                    returnLabelTarget
                );

                // 编译成 lambda: (CsvRow row) => TRow
                func = Expression.Lambda<Func<CsvRow, CsvRow>>(block, pRow).Compile();
                _factoryCache[type] = func;
                return func;
            }
        }

        public TRow As<TRow>() where TRow : CsvRow
        {
            var factory = GetOrCreateFactory(typeof(TRow));
            return (TRow)factory(this);
        }

    }

}
