using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.Expressions.Tests
{
    public class BasicTest : TestBase
    {

        public BasicTest() : base()
        {
            testItems = new List<TestItem>
            {
                new TestItem("", ""),
                new TestItem("\nA\r\nB\rC\nD⏎E\n", "\r\nA\r\nB\r\nC\r\nD\r\nE\r\n"),
            };
        }
    }
}
