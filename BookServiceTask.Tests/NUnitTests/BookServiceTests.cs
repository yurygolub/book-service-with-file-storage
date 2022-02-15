using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookClass;
using BookStorages;
using Comparers;
using Interfaces;
using NUnit.Framework;
using Predicates;

namespace BookServiceTask.Tests.NUnitTests
{
    [TestFixture]
    public class BookServiceTests
    {
        [Test]
        public void Add_BookIsNull_ThrowArgumentNullException()
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentNullException>(() => bookService.Add(null));
        }

        [TestCase("3-598-21508-8")]
        public void Add_BookAlreadyExist_ThrowArgumentException(string isbn)
        {
            BookService bookService = new BookService();
            bookService.Add(new Book("", "", "", isbn));
            Assert.Throws<ArgumentException>(
                () => bookService.Add(new Book("", "", "", isbn)));
        }

        [Test]
        public void Remove_BookIsNull_ThrowArgumentNullException()
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentNullException>(() => bookService.Remove(null));
        }

        [TestCase("3-598-21508-8")]
        public void Remove_BookDoesNotExist_ThrowArgumentException(string isbn)
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentException>(
                () => bookService.Remove(new Book("", "", "", isbn)));
        }

        [Test]
        public void FindBy_PredicateIsNull_ThrowArgumentNullException()
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentNullException>(() => bookService.FindBy(null));
        }

        [TestCaseSource(typeof(TestSource), nameof(TestSource.TestCasesForFindBy))]
        public void FindBy_ReturnResult(Book[] collection, Book[] expected)
        {
            BookService bookService = new BookService(collection);
            var actual = bookService.FindBy(new BookPredicate());
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SortBy_ComparerIsNull_ThrowArgumentNullException()
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentNullException>(() => bookService.SortBy(null));
        }

        [TestCaseSource(typeof(TestSource), nameof(TestSource.TestCasesForSortBy))]
        public void SortBy_ReturnResult(Book[] collection, Book[] expected)
        {
            BookService bookService = new BookService(collection);
            var actual = bookService.SortBy(new BookComparer()).ToArray();
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < collection.Length; i++)
            {
                Assert.AreEqual(expected[i].Author, actual[i].Author);
                Assert.AreEqual(expected[i].Pages, actual[i].Pages);
                Assert.AreEqual(expected[i].Price, actual[i].Price);
            }
        }

        [Test]
        public void Load_StorageIsNull_ThrowArgumentNullException()
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentNullException>(() => bookService.Load(null));
        }

        [TestCaseSource(typeof(TestSource), nameof(TestSource.TestCasesForSave))]
        public void Load_Test(Book[] collection)
        {
            BookService bookService = new BookService();
            IStorage<Book> storage = new Storage(collection);
            bookService.Load(storage);

            Storage temp = new Storage();
            bookService.Save(temp);
            var actual = temp.Load();

            CollectionAssert.AreEqual(collection, actual);
        }

        [Test]
        public void Save_StorageIsNull_ThrowArgumentNullException()
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentNullException>(() => bookService.Save(null));
        }

        [TestCaseSource(typeof(TestSource), nameof(TestSource.TestCasesForSave))]
        public void Save_Test(Book[] collection)
        {
            BookService bookService = new BookService(collection);
            IStorage<Book> storage = new Storage();
            bookService.Save(storage);

            var actual = storage.Load();

            CollectionAssert.AreEqual(collection, actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void SaveToFile_PathIsNullOrWhiteSpace_ThrowArgumentException(string path)
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentException>(() => bookService.SaveToFile(path));
        }

        [TestCaseSource(typeof(TestSource), nameof(TestSource.TestCasesForSaveToFile))]
        public void SaveToFileTests(Book[] collection)
        {
            const string FileName = "data.bin";

            BookService bookService = new BookService(collection);
            bookService.SaveToFile(FileName);

            CheckFileIsClosed(FileName);

            using MemoryStream expected = new MemoryStream();
            using BinaryWriter binaryWriter = new BinaryWriter(expected);
            foreach (Book book in collection)
            {
                binaryWriter.Write(book.Author);
                binaryWriter.Write(book.Title);
                binaryWriter.Write(book.Publisher);
                binaryWriter.Write(book.ISBN);
                binaryWriter.Write(book.Pages);
                binaryWriter.Write(book.Price);
                binaryWriter.Write(book.Currency);
            }

            using FileStream fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);

            byte[] actualBytes = new byte[fileStream.Length];
            int returnedByte, position = 0;
            while ((returnedByte = fileStream.ReadByte()) != -1)
            {
                actualBytes[position++] = (byte)returnedByte;
            }

            CollectionAssert.AreEqual(expected.ToArray(), actualBytes);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void ReadFromFile_PathIsNullOrWhiteSpace_ThrowArgumentException(string path)
        {
            BookService bookService = new BookService();
            Assert.Throws<ArgumentException>(() => bookService.SaveToFile(path));
        }

        [Test]
        public void ReadFromFile_FileDoesNotExist_ThrowArgumentException()
        {
            Assert.Throws<FileNotFoundException>(() => BookService.ReadFromFile("DoesNotExist!"));
        }

        [TestCaseSource(typeof(TestSource), nameof(TestSource.TestCasesForSaveToFile))]
        public void ReadFromFileTests(Book[] collection)
        {
            const string FileName = "data.bin";

            using FileStream fileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            WriteBooksToStream(fileStream, collection);

            using MemoryStream expectedStream = new MemoryStream();
            WriteBooksToStream(expectedStream, collection);

            var readBooks = BookService.ReadFromFile(FileName);

            using MemoryStream actualStream = new MemoryStream();
            WriteBooksToStream(actualStream, readBooks);

            CheckFileIsClosed(FileName);

            CollectionAssert.AreEqual(expectedStream.ToArray(), actualStream.ToArray());

            void WriteBooksToStream(Stream stream, IEnumerable<Book> books)
            {
                using BinaryWriter binaryWriter = new BinaryWriter(stream);
                foreach (Book book in books)
                {
                    binaryWriter.Write(book.Author);
                    binaryWriter.Write(book.Title);
                    binaryWriter.Write(book.Publisher);
                    binaryWriter.Write(book.ISBN);
                    binaryWriter.Write(book.Pages);
                    binaryWriter.Write(book.Price);
                    binaryWriter.Write(book.Currency);
                }
            }
        }

        private void CheckFileIsClosed(string fileName)
        {
            try
            {
                using var stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                Assert.Fail("Source stream is not closed! Please use 'using' statement.");
            }
        }
    }
}
