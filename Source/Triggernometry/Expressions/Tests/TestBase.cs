using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.Expressions.Tests
{
    public abstract class TestBase
    {
        public readonly struct TestItem
        {
            public readonly string FullExpression;
            public readonly string ExpectedResult;
            public readonly bool UsePlaceholder;
            public TestItem(string fullExpression, string expectedResult, bool usePlaceholder = false)
            {
                FullExpression = fullExpression;
                ExpectedResult = expectedResult;
                UsePlaceholder = usePlaceholder;
            }
        }

        public readonly struct TestResult
        {
            public readonly string RealResult;
            public readonly string ExpectedResult;
            public readonly string ExMessage;
            public readonly bool? IsCorrect;

            public TestResult(string realResult, string exMessage, TestItem source)
            {
                RealResult = realResult;
                ExpectedResult = source.ExpectedResult;
                ExMessage = exMessage;

                // true = correct, false = incorrect, null = expected an Exception
                if (source.ExpectedResult != realResult)
                {
                    IsCorrect = false;
                }
                else if (realResult != null)
                {
                    IsCorrect = true;
                }
                else
                {
                    IsCorrect = null;
                }
            }

            public override string ToString()
            {
                var result = RealResult != null 
                    ? $"\"{ParserCommon.ReplaceLineBreak(RealResult)}\"" 
                    : $"(error: {ParserCommon.ReplaceLineBreak(ExMessage)})";
                var expected = ExpectedResult != null 
                    ? $"\"{ParserCommon.ReplaceLineBreak(ExpectedResult)}\"" 
                    : "(error)";
                switch (IsCorrect)
                {
                    case true:
                        return $"[PASS] Result: {result}";
                    case false:
                        return $"[FAIL] Result: {result}; Expected: {expected}";
                    case null:
                        return $"[EXCP] Result: {result}; Expected: {expected}";
                    default: throw new Exception();
                }
            }
        }

        protected List<TestItem> testItems;

        public List<TestResult> Test()
        {
            _ = testItems ?? throw new ArgumentNullException(nameof(testItems), $"Should initiate test data in the ctor of {GetType().Name}");

            var ctxRealValue = new Core.Context(null);
            var ctxPlaceholder = new Core.Context(null) { testByPlaceholder = true };

            return testItems.Select(testItem =>
            {
                var ctx = testItem.UsePlaceholder ? ctxPlaceholder : ctxRealValue;
                string result = null;
                string exMessage = null;
                try
                {
                    result = ctx.EvaluateStringExpression(null, null, testItem.FullExpression);
                }
                catch (Exception ex)
                {
                    exMessage = ex.Message;
                }
                return new TestResult(result, exMessage, testItem);
            }).ToList();
        }

    }
}
