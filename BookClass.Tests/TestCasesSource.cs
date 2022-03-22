using System.Collections.Generic;
using NUnit.Framework;

namespace BookClass.Tests
{
    public sealed class TestCasesSource
    {
        public static IEnumerable<TestCaseData> TestCasesWithEqualValues
        {
            get
            {
                yield return new TestCaseData(
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21507-X"),
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21507-X"));

                yield return new TestCaseData(
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21508-8"),
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21508-8"));

                yield return new TestCaseData(
                    new Book(string.Empty, string.Empty, string.Empty, "359821507X"),
                    new Book(string.Empty, string.Empty, string.Empty, "359821507X"));
            }
        }

        public static IEnumerable<TestCaseData> TestCasesWithDifferentValues
        {
            get
            {
                yield return new TestCaseData(
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21507-X"),
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21508-8"));

                yield return new TestCaseData(
                    new Book(string.Empty, string.Empty, string.Empty, "359821507X"),
                    new Book(string.Empty, string.Empty, string.Empty, "3-598-21508-8"));

                yield return new TestCaseData(
                    new Book(string.Empty, string.Empty, string.Empty, "359821507X"),
                    new Book(string.Empty, string.Empty, string.Empty, "039304002X"));
            }
        }

        public static IEnumerable<TestCaseData> TestCasesForCompareToLessZero
        {
            get
            {
                yield return new TestCaseData(
                    new Book(string.Empty, "ABC", string.Empty),
                    new Book(string.Empty, "BCD", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "abcd", string.Empty),
                    new Book(string.Empty, "abce", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "abc", string.Empty),
                    new Book(string.Empty, "ABC", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "qwe", string.Empty),
                    new Book(string.Empty, "was", string.Empty));
            }
        }

        public static IEnumerable<TestCaseData> TestCasesForCompareToMoreZero
        {
            get
            {
                yield return new TestCaseData(
                    new Book(string.Empty, "BCD", string.Empty),
                    new Book(string.Empty, "ABC", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "abce", string.Empty),
                    new Book(string.Empty, "abcd", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "ABC", string.Empty),
                    new Book(string.Empty, "abc", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "was", string.Empty),
                    new Book(string.Empty, "qwe", string.Empty));
            }
        }

        public static IEnumerable<TestCaseData> TestCasesForCompareToEqualZero
        {
            get
            {
                yield return new TestCaseData(
                    new Book(string.Empty, "ABC", string.Empty),
                    new Book(string.Empty, "ABC", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "abcd", string.Empty),
                    new Book(string.Empty, "abcd", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "abc", string.Empty),
                    new Book(string.Empty, "abc", string.Empty));

                yield return new TestCaseData(
                    new Book(string.Empty, "qwe", string.Empty),
                    new Book(string.Empty, "qwe", string.Empty));
            }
        }
    }
}
