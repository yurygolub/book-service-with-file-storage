using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1062 // Проверить аргументы или открытые методы
#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable SA1117 // Parameters should be on same line or separate lines

namespace BookClass.Tests
{
    [TestFixture]
    public class BookTests
    {
        [TestCase("Jon Skeet", "C# in Depth", "Manning Publications")]
        public void Book_CreateNewThreeParameters(string author, string title, string publisher)
        {
            Book book = new Book(author, title, publisher); 
            Assert.IsTrue(book.Author == author && book.Title == title && book.Publisher == publisher); 
        }

        [TestCase("Jon Skeet", "C# in Depth", "Manning Publications", "978-0-901-69066-1")]
        [TestCase("Jon Skeet", "C# in Depth", "Manning Publications", "3-598-21508-8")]
        [TestCase("Jon Skeet", "C# in Depth", "Manning Publications", "")]
        public void Book_CreateNewFourParameters(string author, string title, string publisher, string isbn)
        {
            Book book = new Book(author, title, publisher, isbn);
            Assert.IsTrue(book.Author == author && book.Title == title && book.Publisher == publisher && book.ISBN == isbn);
        }

        [TestCase(null, "C# in Depth", "Manning Publications")]
        [TestCase("Jon Skeet", null, "Manning Publications")]
        [TestCase("Jon Skeet", "C# in Depth", null)]
        public void Book_CreateNewThreeParameters_ThrowArgumentNullException(string author, string title, string publisher)
        {
            Assert.Throws<ArgumentNullException>(() => new Book(author, title, publisher), "author or title or publisher cannot be null");
        }

        [TestCase(null, "C# in Depth", "Manning Publications", "978-0-901-69066-1")]
        [TestCase("Jon Skeet", null, "Manning Publications", "978-0-901-69066-1")]
        [TestCase("Jon Skeet", "C# in Depth", null, "978-0-901-69066-1")]
        [TestCase("Jon Skeet", "C# in Depth", "Manning Publications", null)]
        public void Book_CreateNewFourParameters_ThrowArgumentNullException(string author, string title, string publisher, string isbn)
        {
            Assert.Throws<ArgumentNullException>(() => new Book(author, title, publisher, isbn), "author or title or publisher or ISBN cannot be null");
        }

        [Test]
        public void Book_PagesTest()
        {
            int expected = 10;
            Book book = new Book(string.Empty, string.Empty, string.Empty)
            {
                Pages = expected,
            };
            Assert.AreEqual(expected, book.Pages);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Book_PagesTest_ArgumentOutOfRangeException(int pages)
        {
            Book book = new Book(string.Empty, string.Empty, string.Empty);
            Assert.Throws<ArgumentOutOfRangeException>(() => book.Pages = pages, "Count of pages should be greater than zero.");
        }

        [Test]
        public void Book_Publish_GetPublicationDate_Tests()
        {
            DateTime expected = DateTime.Now;
            Book book = new Book(string.Empty, string.Empty, string.Empty);
            book.Publish(expected);

            Assert.AreEqual(FormattableString.Invariant($"{expected:d}"), book.GetPublicationDate());
        }

        [Test]
        public void Book_Publish_GetPublicationDate_Empty_Tests()
        {
            string expected = "NYP";
            Book book = new Book(string.Empty, string.Empty, string.Empty);
            Assert.AreEqual(expected, book.GetPublicationDate());
        }

        [Test]
        public void Book_SetPrice_Tests()
        {
            decimal price = 4.44m;
            string currency = "USD";
            Book book = new Book(string.Empty, string.Empty, string.Empty);
            book.SetPrice(price, currency);
            Assert.IsTrue(book.Price == price && book.Currency == currency);
        }

        [Test]
        public void Book_SetPrice_InvalidCurrency_ArgumentException()
        {
            decimal price = 4.44m;
            string currency = "_~_";
            Book book = new Book(string.Empty, string.Empty, string.Empty);

            Assert.Throws<ArgumentException>(() => book.SetPrice(price, currency), "Currency is invalid.");
        }

        [Test]
        public void Book_SetPrice_LessZero_ArgumentException()
        {
            decimal price = -4.44m;
            string currency = "USD";
            Book book = new Book(string.Empty, string.Empty, string.Empty);

            Assert.Throws<ArgumentException>(() => book.SetPrice(price, currency), "Price cannot be less than zero.");
        }

        [Test]
        public void Book_SetPrice_CurrencyIsNull_ArgumentNullException()
        {
            decimal price = 4.44m;
            Book book = new Book(string.Empty, string.Empty, string.Empty);

            Assert.Throws<ArgumentNullException>(() => book.SetPrice(price, null), "Currency cannot be null.");
        }

        [TestCase("Jon Skeet", "C# in Depth", "Manning Publications", ExpectedResult = "C# in Depth by Jon Skeet")]
        [TestCase("Jon Skeet", "", "Manning Publications", ExpectedResult = " by Jon Skeet")]
        [TestCase("", "C# in Depth", "Manning Publications", ExpectedResult = "C# in Depth by ")]
        public string Book_ToString(string author, string title, string publisher)
            => new Book(author, title, publisher).ToString();

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithEqualValues))]
        public void Equals_ForEqualValues_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs.Equals(rhs));
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithDifferentValues))]
        public void Equals_ForDifferentValues_ReturnFalse(Book lhs, Book rhs)
        {
            Assert.IsTrue(!rhs.Equals(lhs));
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithEqualValues))]
        public void OperatorEquality_ForEqualValues_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs == rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithDifferentValues))]
        public void OperatorEquality_ForDifferentValues_ReturnFalse(Book lhs, Book rhs)
        {
            Assert.IsFalse(lhs == rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithDifferentValues))]
        public void OperatorNotEquality_ForTheDifferentValues_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs != rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithEqualValues))]
        public void OperatorNotEquality_ForEqualValues_ReturnFalse(Book lhs, Book rhs)
        {
            Assert.IsFalse(lhs != rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithEqualValues))]
        public void GetHashCode_ForEqualValues_ReturnEqualCode(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs.Equals(rhs) && lhs.GetHashCode() == rhs.GetHashCode());
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithEqualValues))]
        public void ObjectEquals_ForEqualValues_ReturnTrue(Book lhs, object rhs)
        {
            Assert.IsTrue(lhs.Equals(rhs));
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesWithDifferentValues))]
        public void ObjectEquals_ForDifferentValues_ReturnFalse(Book lhs, object rhs)
        {
            Assert.IsTrue(!lhs.Equals(rhs));
        }

        [TestCase("Book")]
        [TestCase(12.45)]
        [TestCase(true)]
        [TestCase(null)]
        [TestCase('A')]
        public void ObjectEquals_OtherOperandIsNotBookInstance_ReturnFalse(object rhs)
        {
            Book lhs = new Book(string.Empty, string.Empty, string.Empty);
            Assert.IsTrue(!lhs.Equals(rhs));
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToLessZero))]
        public void GenericCompareTo_LeftHandSideOperandLessThanRightHandSideOperand_ReturnIntegerLessZero(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs.CompareTo(rhs) < 0);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToMoreZero))]
        public void GenericCompareTo_LeftHandSideOperandMoreThanRightHandSideOperand_ReturnIntegerMoreZero(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs.CompareTo(rhs) > 0);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToEqualZero))]
        public void GenericCompareTo_LeftHandSideOperandIsEqualToRightHandSideOperand_ReturnZero(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs.CompareTo(rhs) == 0);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToLessZero))]
        public void OperatorLess_LeftHandSideOperandLessThanRightHandSideOperand_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs < rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToMoreZero))]
        public void OperatorMore_LeftHandSideOperandMoreThanRightHandSideOperand_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs > rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToEqualZero))]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToLessZero))]
        public void OperatorLessOrEquals_LeftHandSideOperandLessOrEqualThanRightHandSideOperand_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs <= rhs);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToEqualZero))]
        [TestCaseSource(typeof(TestCasesSource), nameof(TestCasesSource.TestCasesForCompareToMoreZero))]
        public void OperatorMoreOrEquals_LeftHandSideOperandMoreOrEqualThanRightHandSideOperand_ReturnTrue(Book lhs, Book rhs)
        {
            Assert.IsTrue(lhs >= rhs);
        }

        [Test]
        public void Parse_SourceIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Book.Parse(null), "source string cannot be null");
        }

        [TestCase("")]
        [TestCase("\"Title\",Author,Year,Publisher,Pages,ISBN-13,Price")]
        [TestCase("qwe")]
        [TestCase("1234")]
        [TestCase("\"Title\",Author,2020,Publisher,100,3-598-21507-X,52")]
        public void Parse_SourceHasInvalidFormat_ThrowFormatException(string source)
        {
            Assert.Throws<FormatException>(() => Book.Parse(source), "source string cannot be null");
        }

        [TestCase("\"C# in Depth\",Jon Skeet,2019,Manning Publications,528,9781617294532,28,USD")]
        [TestCase("\"Title\",Author,2020,Publisher,100,3-598-21507-X,10,EUR")]
        public void Parse_SourceIsValid_ReturnResult(string source)
        {
            Assert.IsNotNull(Book.Parse(source));
        }

        [TestCase("")]
        [TestCase("\"Title\",Author,Year,Publisher,Pages,ISBN-13,Price")]
        [TestCase("qwe")]
        [TestCase("1234")]
        [TestCase("\"Title\",Author,2020,Publisher,100,3-598-21507-X,52")]
        public void TryParse_SourceHasInvalidFormat_ReturnFalse(string source)
        {
            Assert.IsFalse(Book.TryParse(source, out Book book));
            Assert.IsNull(book);
        }

        [TestCase("\"C# in Depth\",Jon Skeet,2019,Manning Publications,528,9781617294532,28,USD")]
        [TestCase("\"Title\",Author,2020,Publisher,100,3-598-21507-X,10,EUR")]
        public void TryParse_SourceIsValid_ReturnTrue(string source)
        {
            Assert.IsTrue(Book.TryParse(source, out Book book));
            Assert.IsNotNull(book);
        }

        [TestCase("C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD", ExpectedResult = "C# in Depth by Jon Skeet.")]
        [TestCase("C# in Depth", "", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD", ExpectedResult = "C# in Depth by .")]
        [TestCase("", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD", ExpectedResult = " by Jon Skeet.")]
        public string ToString_FormatIsNull_ReturnResult(string title, string author, int year, string publisher, int pages, string isbn, decimal price, string currency)
        {
            Book book = new Book(author, title, publisher, isbn) { Pages = pages };
            book.Publish(new DateTime(year, 1, 1));
            book.SetPrice(price, currency);

            return book.ToString(null);
        }

        [TestCase("C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD", ExpectedResult = "C# in Depth by Jon Skeet $27.99")]
        [TestCase("C# in Depth", "", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD", ExpectedResult = "C# in Depth by  $27.99")]
        [TestCase("", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD", ExpectedResult = " by Jon Skeet $27.99")]
        public string ToString_FormatProviderIsNull_ReturnResult(string title, string author, int year, string publisher, int pages, string isbn, decimal price, string currency)
        {
            Book book = new Book(author, title, publisher, isbn) { Pages = pages };
            book.Publish(new DateTime(year, 1, 1));
            book.SetPrice(price, currency);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            return book.ToString("E", null);
        }

        [TestCase("A", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD",
            ExpectedResult = "C# in Depth by Jon Skeet. 2019. Manning Publications. ISBN: 9781617294532. 528 pages. $27.99")]
        [TestCase("B", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD",
            ExpectedResult = "C# in Depth by Jon Skeet. 2019. Manning Publications. ISBN: 9781617294532. 528 pages.")]
        [TestCase("C", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD",
            ExpectedResult = "C# in Depth by Jon Skeet. 2019. Manning Publications. 528 pages.")]
        [TestCase("D", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD",
            ExpectedResult = "C# in Depth by Jon Skeet. 2019. 528 pages.")]
        [TestCase("E", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD",
            ExpectedResult = "C# in Depth by Jon Skeet $27.99")]
        [TestCase("F", "C# in Depth", "Jon Skeet", 2019, "Manning Publications", 528, "9781617294532", 27.99, "USD",
            ExpectedResult = "C# in Depth by Jon Skeet.")]
        public string ToString_CustomFormat_ReturnResult(string format, string title, string author, int year, string publisher, int pages, string isbn, decimal price, string currency)
        {
            Book book = new Book(author, title, publisher, isbn) { Pages = pages };
            book.Publish(new DateTime(year, 1, 1));
            book.SetPrice(price, currency);

            return book.ToString(format, CultureInfo.GetCultureInfo("en-US"));
        }
    }
}
