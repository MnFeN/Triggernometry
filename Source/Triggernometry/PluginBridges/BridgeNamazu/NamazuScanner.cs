using System;
using System.Collections.Generic;

namespace Triggernometry.PluginBridges.BridgeNamazu
{
    /// <summary>
    /// Wrapper for PostNamazu.Common.SigScanner
    /// </summary>
    public class NamazuScanner
    {
        private readonly dynamic _scanner;
        public object RawScanner => _scanner;

        public NamazuScanner(object scanner)
        {
            _scanner = scanner ?? throw new ArgumentNullException(nameof(scanner));
        }

        // Underlying fields
        // public MemHelper MemHelper => _scanner._memhelper;
        public uint SizeOfCode => _scanner.SizeOfCode;
        public uint CodeBase => _scanner.CodeBase;
        public uint DataLength => _scanner._dataLength;
        public byte[] Data => _scanner._data;
        public IntPtr BaseAddress => _scanner._baseAddress;

        // Text section info
        public IntPtr TextSectionBase => _scanner.TextSectionBase;
        public long TextSectionOffset => _scanner.TextSectionOffset;
        public int TextSectionSize => _scanner.TextSectionSize;

        // Data section info
        public IntPtr DataSectionBase => _scanner.DataSectionBase;
        public long DataSectionOffset => _scanner.DataSectionOffset;
        public int DataSectionSize => _scanner.DataSectionSize;

        // Scanning methods
        public T ScanText<T>(string pattern, Func<IntPtr, T> visitor, string sigName = null)
            => _scanner.ScanText<T>(pattern, visitor, sigName);

        /// <summary>
        /// 从内存中扫描指定的内存签名，返回唯一匹配的地址，否则报错。<br /><br />
        /// 
        /// ? 或 ?? 表示普通通配符，如：<br />
        /// <c> 48 89 5C 24 ?? ... </c><br /><br />
        /// 
        /// * 或 ** 表示相对寻址通配符，如果使用，则仅能有连续四个，如：<br />
        /// <c> 48 8D 0D * * * * 4C 8B 85 ... </c> <br />
        /// <c> E8 * * * * 48 83 C4 ? E9 ? ? ? ? ... </c> <br />
        /// 相对寻址计算方式为 * * * * 后的地址 + 这四字节对应的 int 偏移量。<br /><br />
        /// </summary>
        public IntPtr ScanText(string pattern, string name = null)
            => _scanner.ScanText(pattern, name);

        public List<IntPtr> FindPattern(List<int> pattern)
            => _scanner.FindPattern(pattern);

        public IntPtr TryScan(string pattern, string name)
        {
            try
            {
                return ScanText(pattern, name);
            }
            catch (Exception ex)
            {
                RealPlugin.plug.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error, ex.Message);
                return IntPtr.Zero;
            }
        }

        public IntPtr TryScanMultiple(IEnumerable<string> patterns, string name)
        {
            Exception ex = null;
            foreach (var pattern in patterns)
            {
                try
                {
                    return ScanText(pattern, name);
                }
                catch (Exception e)
                {
                    ex = e;
                }
            }
            RealPlugin.plug.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error, ex?.Message ?? "");
            return IntPtr.Zero;
        }

    }
}
