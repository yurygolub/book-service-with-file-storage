using System;
using System.Globalization;
using NUnit.Framework;

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
#pragma warning disable SA1118 // Parameter should not span multiple lines

namespace BookClass.Tests
{
    [TestFixture]
    public class BookFormatTests
    {
        [TestCase("A", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532")]
        public void Format_InvalidFormat_ThrowFormatException(string format, string title, string author, int year, string publisher, int pages, string isbn)
        {
            Book book = new Book(author, title, publisher, isbn) { Pages = pages };
            book.Publish(new DateTime(year, 1, 1));

            Assert.Throws<FormatException>(() => new BookFormat().Format(format, book, CultureInfo.GetCultureInfo("en-US")));
        }

        [TestCase("h", 5)]
        public void Format_NotBookInstance_InvalidFormat_ThrowFormatException(string format, object arg)
        {
            Assert.Throws<FormatException>(() => new BookFormat().Format(format, arg, CultureInfo.GetCultureInfo("en-US")));
        }

        [TestCase("", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532",
            ExpectedResult =
            "Product details:\n" +
            "Author: Jon Skeet\n" +
            "Title: C# in Depth\n" +
            "Publisher: Manning Publications, 2019\n" +
            "ISBN-13: 9781617294532\n" +
            "Paperback: 528 pages")]
        [TestCase(null, "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532",
            ExpectedResult =
            "Product details:\n" +
            "Author: Jon Skeet\n" +
            "Title: C# in Depth\n" +
            "Publisher: Manning Publications, 2019\n" +
            "ISBN-13: 9781617294532\n" +
            "Paperback: 528 pages")]
        public string Format_FormatIsNullOrEmpty_DefaultFormat(string format, string title, string author, int year, string publisher, int pages, string isbn)
        {
            Book book = new Book(author, title, publisher, isbn) { Pages = pages };
            book.Publish(new DateTime(year, 1, 1));

            return new BookFormat().Format(format, book, CultureInfo.GetCultureInfo("en-US"));
        }

        [TestCase("d", 5, ExpectedResult = "5")]
        [TestCase("H", "book", ExpectedResult = "book")]
        public string Format_NotBookInstance_ReturnResult(string format, object arg)
        {
            return new BookFormat().Format(format, arg, CultureInfo.GetCultureInfo("en-US"));
        }

        [TestCase("H", null, ExpectedResult = "")]
        public string Format_ArgIsNull_ReturnEmptyString(string format, object arg)
        {
            return new BookFormat().Format(format, arg, CultureInfo.GetCultureInfo("en-US"));
        }

        [TestCase("H", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532",
            ExpectedResult =
            "Product details:\n" +
            "Author: Jon Skeet\n" +
            "Title: C# in Depth\n" +
            "Publisher: Manning Publications, 2019\n" +
            "ISBN-13: 9781617294532\n" +
            "Paperback: 528 pages")]
        public string Format_ReturnResult(string format, string title, string author, int year, string publisher, int pages, string isbn)
        {
            Book book = new Book(author, title, publisher, isbn) { Pages = pages };
            book.Publish(new DateTime(year, 1, 1));

            return new BookFormat().Format(format, book, CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
