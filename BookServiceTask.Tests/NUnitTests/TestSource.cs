using System;
using System.Collections.Generic;
using BookClass;
using NUnit.Framework;

namespace BookServiceTask.Tests.NUnitTests
{
    public class TestSource
    {
        public static IEnumerable<TestCaseData> TestCasesForFindBy
        {
            get
            {
                yield return new TestCaseData(
                    new Book[]
                    {
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", "978-0-901-69066-1"),
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", ""),
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", "3-598-21508-8"),
                    },
                    new Book[]
                    {
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", "978-0-901-69066-1"),
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", "3-598-21508-8"),
                    });

                yield return new TestCaseData(
                    new Book[]
                    {
                        new Book("qwerty", "asdf", "zxcvb", "978-0-901-69066-1"),
                        new Book("aaaaa", "bbbbbb", "ccc", ""),
                        new Book("wwwww", "aaaaa", "sssss", "3-598-21508-8"),
                    },
                    new Book[]
                    {
                        new Book("qwerty", "asdf", "zxcvb", "978-0-901-69066-1"),
                        new Book("wwwww", "aaaaa", "sssss", "3-598-21508-8"),
                    });
            }
        }

        public static IEnumerable<TestCaseData> TestCasesForSortBy
        {
            get
            {
                var books1 = new Book[]
                {
                    new Book("a", string.Empty, string.Empty),
                    new Book("c", string.Empty, string.Empty),
                    new Book("b", string.Empty, string.Empty),
                };

                var books1Expected = new Book[]
                {
                    new Book("a", string.Empty, string.Empty),
                    new Book("b", string.Empty, string.Empty),
                    new Book("c", string.Empty, string.Empty),
                };

                var books2 = new Book[]
                {
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                    new Book("a", string.Empty, string.Empty) { Pages = 5 },
                    new Book("a", string.Empty, string.Empty) { Pages = 15 },
                };

                var books2Expected = new Book[]
                {
                    new Book("a", string.Empty, string.Empty) { Pages = 5 },
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                    new Book("a", string.Empty, string.Empty) { Pages = 15 },
                };

                var books3 = new Book[]
                {
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                };
                books3[0].SetPrice(100m, "USD");
                books3[1].SetPrice(50m, "USD");
                books3[2].SetPrice(150m, "USD");

                var books3Expected = new Book[]
                {
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                    new Book("a", string.Empty, string.Empty) { Pages = 10 },
                };
                books3Expected[0].SetPrice(50m, "USD");
                books3Expected[1].SetPrice(100m, "USD");
                books3Expected[2].SetPrice(150m, "USD");

                yield return new TestCaseData(books1, books1Expected);
                yield return new TestCaseData(books2, books2Expected);
                yield return new TestCaseData(books3, books3Expected);
            }
        }

        public static IEnumerable<TestCaseData> TestCasesForSave
        {
            get
            {
                yield return new TestCaseData(arg:
                    new Book[]
                    {
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", "978-0-901-69066-1"),
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", ""),
                        new Book("Jon Skeet", "C# in Depth", "Manning Publications", "3-598-21508-8"),
                    });

                yield return new TestCaseData(arg:
                    new Book[]
                    {
                        new Book("qwerty", "asdf", "zxcvb", "978-0-901-69066-1"),
                        new Book("aaaaa", "bbbbbb", "ccc", ""),
                        new Book("wwwww", "aaaaa", "sssss", "3-598-21508-8"),
                    });

                yield return new TestCaseData(arg: Array.Empty<Book>());
            }
        }
    }
}
