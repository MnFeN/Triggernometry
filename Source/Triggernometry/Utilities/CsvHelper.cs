using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Triggernometry.Utilities
{
    public static class CsvHelper
    {
        public static List<string[]> ReadCsv(string filePath)
        {
            var result = new List<string[]>(512);

            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                int expected = -1;
                int row = 0;

                // 行内字符缓冲区，整文件复用，减少每行 new char[]
                char[] sb = null;

                string logicalLine;
                while ((logicalLine = ReadLogicalCsvLine(sr)) != null)
                {
                    row++;

                    string[] arr;

                    if (expected == -1)
                    {
                        // 第一行：列数未知，用不限制列数的解析器
                        arr = ParseFirstLine(logicalLine, ref sb);
                        expected = arr.Length;

                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(arr[i]))
                            {
                                arr[i] = "unk_" + i;
                            }
                        }
                    }
                    else
                    {
                        // 后续行：按固定列数解析
                        arr = ParseLine(logicalLine, expected, ref sb);

                        if (arr.Length != expected)
                        {
                            throw new InvalidDataException($"CSV {filePath} 第 {row} 行的列数不一致：期望 {expected}，实际 {arr.Length}");
                        }
                    }

                    result.Add(arr);
                }
            }

            return result;
        }

        // ----------------------------------------------------
        // 带引号的多行字段处理
        // ----------------------------------------------------
        private static string ReadLogicalCsvLine(StreamReader sr)
        {
            string line = sr.ReadLine();
            if (line == null)
                return null;

            StringBuilder sb = new StringBuilder();
            sb.Append(line);

            // 判断当前行的引号数量是否是奇数 → 引号未闭合，说明当前行最后一个字段包含换行
            int quoteCount = CountQuotes(line);

            while (quoteCount % 2 != 0)
            {
                string next = sr.ReadLine();
                if (next == null)
                    break;

                sb.Append("\n");
                sb.Append(next);
                quoteCount += CountQuotes(next);
            }

            return sb.ToString();
        }

        private static int CountQuotes(string s)
        {
            int c = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '"')
                    c++;
            }
            return c;
        }

        // -----------------------------------------
        // 第一行用：列数未知 → 用 List 自动扩容
        // 使用外部复用的 char[] 缓冲区
        // -----------------------------------------
        private static string[] ParseFirstLine(string line, ref char[] sb)
        {
            ReadOnlySpan<char> span = line.AsSpan();
            int len = span.Length;

            if (sb == null || sb.Length < len)
            {
                sb = new char[len];
            }

            List<string> list = new List<string>(32);
            int sbLen = 0;
            bool inQuotes = false;

            for (int i = 0; i < len; i++)
            {
                char c = span[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < len && span[i + 1] == '"')
                        {
                            sb[sbLen++] = '"';
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb[sbLen++] = c;
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        list.Add(new string(sb, 0, sbLen));
                        sbLen = 0;
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        sb[sbLen++] = c;
                    }
                }
            }

            list.Add(new string(sb, 0, sbLen));
            return list.ToArray();
        }

        // -----------------------------------------
        // 后续行用：已知列数 → 预分配数组，最高性能
        // 使用外部复用的 char[] 缓冲区
        // -----------------------------------------
        private static string[] ParseLine(string line, int expectedColumns, ref char[] sb)
        {
            ReadOnlySpan<char> span = line.AsSpan();
            int len = span.Length;

            if (sb == null || sb.Length < len)
            {
                sb = new char[len];
            }

            string[] tmp = new string[expectedColumns];
            int count = 0;

            int sbLen = 0;
            bool inQuotes = false;

            for (int i = 0; i < len; i++)
            {
                char c = span[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < len && span[i + 1] == '"')
                        {
                            sb[sbLen++] = '"';
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb[sbLen++] = c;
                    }
                }
                else
                {
                    if (c == ',')
                    {
                        tmp[count++] = new string(sb, 0, sbLen);
                        sbLen = 0;
                    }
                    else if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else
                    {
                        sb[sbLen++] = c;
                    }
                }
            }

            tmp[count++] = new string(sb, 0, sbLen);
            return tmp;
        }
    }
}
