using System.Collections.Generic;
using BookClass;
using NUnit.Framework;

namespace BookServiceTask.Tests.MoqTests
{
    public class PredicateTestSource
    {
        public static IEnumerable<TestCaseData> VerifyTestCasesReturnTrue
        {
            get
            {
                yield return new TestCaseData(new Book("Jon Skeet", "C# in Depth", "Manning Publications", "978-0-901-69066-1"));
                yield return new TestCaseData(new Book("Jon Skeet", "C# in Depth", "Manning Publications", "3-598-21508-8"));
            }
        }

        public static IEnumerable<TestCaseData> VerifyTestCasesReturnFalse
        {
            get
            {
                yield return new TestCaseData(new Book("Jon Skeet", "C# in Depth", "Manning Publications", ""));
                yield return new TestCaseData(new Book("qwerty", "asdf", "zxcvb"));
                yield return new TestCaseData(new Book("aaaaa", "bbbbbb", "ccc", ""));
            }
        }
    }
}
