using BookClass;
using Interfaces;
using Moq;
using NUnit.Framework;
using Predicates;

namespace BookServiceTask.Tests.MoqTests
{
    [TestFixture]
    public class PredicateTests
    {
        [TestCaseSource(typeof(PredicateTestSource), nameof(PredicateTestSource.VerifyTestCasesReturnTrue))]
        public void VerifyTests_Return_True(Book book)
        {
            var mockPredicate = new Mock<IPredicate<Book>>();

            mockPredicate
                .Setup(p => p.Verify(It.Is<Book>(i =>
                    new BookPredicate().Verify(i))))
                .Returns(true);

            IPredicate<Book> predicate = mockPredicate.Object;

            Assert.IsTrue(predicate.Verify(book));

            mockPredicate.Verify(p => p.Verify(It.IsAny<Book>()), Times.Exactly(1));
        }

        [TestCaseSource(typeof(PredicateTestSource), nameof(PredicateTestSource.VerifyTestCasesReturnFalse))]
        public void VerifyTests_Return_False(Book value)
        {
            Mock<IPredicate<Book>> mockPredicate = new Mock<IPredicate<Book>>();

            mockPredicate
                .Setup(p => p.Verify(It.Is<Book>(i =>
                    new BookPredicate().Verify(i))))
                .Returns(true);

            IPredicate<Book> predicate = mockPredicate.Object;

            Assert.IsFalse(predicate.Verify(value));

            mockPredicate.Verify(p => p.Verify(It.IsAny<Book>()), Times.Exactly(1));
        }
    }
}
