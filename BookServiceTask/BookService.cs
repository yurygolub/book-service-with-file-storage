using System;
using System.Collections.Generic;
using System.IO;
using BookClass;
using Interfaces;

namespace BookServiceTask
{
    /// <summary>
    /// BookService.
    /// </summary>
    public class BookService
    {
        private readonly HashSet<Book> books;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        public BookService()
        {
            this.books = new HashSet<Book>(new EqualityComparer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookService"/> class.
        /// </summary>
        /// <param name="books">The collection to copy elements from.</param>
        public BookService(IEnumerable<Book> books)
        {
            if (books is null)
            {
                throw new ArgumentNullException(nameof(books));
            }

            this.books = new HashSet<Book>(books, new EqualityComparer());
        }

        /// <summary>
        /// Read book collection from binary file.
        /// </summary>
        /// <param name="sourcePath">Path to source file.</param>
        /// <returns>Book collection read from file.</returns>
        public static IEnumerable<Book> ReadFromFile(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentException($"{nameof(sourcePath)} cannot be null or empty or whitespace.", nameof(sourcePath));
            }

            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException($"File '{sourcePath}' not found. Parameter name: {nameof(sourcePath)}.");
            }

            using FileStream fileStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using BinaryReader binaryReader = new BinaryReader(fileStream);

            int length = (int)binaryReader.BaseStream.Length;
            List<Book> books = new List<Book>();
            while (binaryReader.PeekChar() != -1)
            {
                string author = binaryReader.ReadString();
                string title = binaryReader.ReadString();
                string publisher = binaryReader.ReadString();
                string isbn = binaryReader.ReadString();
                int pages = binaryReader.ReadInt32();
                decimal price = binaryReader.ReadDecimal();
                string currency = binaryReader.ReadString();

                Book book = new Book(author, title, publisher, isbn) { Pages = pages, };
                book.SetPrice(price, currency);

                books.Add(book);
            }

            return books;
        }

        /// <summary>
        /// Saves books to binary file.
        /// </summary>
        /// <param name="destinationPath">Path to destination file.</param>
        public void SaveToFile(string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(destinationPath))
            {
                throw new ArgumentException($"{nameof(destinationPath)} cannot be null or empty or whitespace", nameof(destinationPath));
            }

            using FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
            using BinaryWriter binaryWriter = new BinaryWriter(fileStream);
            foreach (Book book in this.books)
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

        /// <summary>
        /// Adds book to service.
        /// </summary>
        /// <param name="book">Book.</param>
        public void Add(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (this.books.Contains(book))
            {
                throw new ArgumentException("This book already exists.");
            }
            else
            {
                this.books.Add(book);
            }
        }

        /// <summary>
        /// Removes book from service.
        /// </summary>
        /// <param name="book">Book.</param>
        public void Remove(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (this.books.Contains(book))
            {
                this.books.Remove(book);
            }
            else
            {
                throw new ArgumentException("This book does not exist.");
            }
        }

        /// <summary>
        /// Returns books which meet specified predicate.
        /// </summary>
        /// <param name="predicate">Defines a condition to find books.</param>
        /// <returns>Books which meet specified predicate.</returns>
        public IEnumerable<Book> FindBy(IPredicate<Book> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return GetEnumerable(predicate);

            IEnumerable<Book> GetEnumerable(IPredicate<Book> predicate)
            {
                foreach (Book book in this.books)
                {
                    if (predicate.Verify(book))
                    {
                        yield return book;
                    }
                }
            }
        }

        /// <summary>
        /// Sorts books using specified comparer.
        /// </summary>
        /// <param name="comparer">Determines the order of books.</param>
        /// <returns>Books sorted using the specified comparer.</returns>
        public IEnumerable<Book> SortBy(IComparer<Book> comparer)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var (buffer, count) = BufferArray.BufferData.ToArray(this.books);
            Array.Sort(buffer, 0, count, comparer);
            return buffer;
        }

        /// <summary>
        /// Loads books from specified storage.
        /// </summary>
        /// <param name="storage">Storage.</param>
        public void Load(IStorage<Book> storage)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            IEnumerable<Book> data = storage.Load();
            foreach (Book book in data)
            {
                this.books.Add(book);
            }
        }

        /// <summary>
        /// Saves books to specified storage.
        /// </summary>
        /// <param name="storage">Storage.</param>
        public void Save(IStorage<Book> storage)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            storage.Save(this.books);
        }

        private class EqualityComparer : IEqualityComparer<Book>
        {
            /// <summary>
            /// Determines whether or not books are equal based on the equality of ISBN, author name, count of pages and price.
            /// </summary>
            /// <param name="x">First book.</param>
            /// <param name="y">Second book.</param>
            /// <returns>true if books are equal; otherwise, false.</returns>
            public bool Equals(Book x, Book y)
            {
                if (x is null)
                {
                    throw new ArgumentNullException(nameof(x));
                }

                if (y is null)
                {
                    throw new ArgumentNullException(nameof(y));
                }

                return string.Equals(x.ISBN, y.ISBN, StringComparison.InvariantCulture) &&
                    string.Equals(x.Author, y.Author, StringComparison.InvariantCulture) &&
                    x.Pages.Equals(y.Pages) &&
                    x.Price.Equals(y.Price);
            }

            /// <summary>
            /// Calculates the hash code for book object.
            /// </summary>
            /// <param name="obj">Book object.</param>
            /// <returns>A 32-bit signed integer hash code.</returns>
            public int GetHashCode(Book obj)
            {
                if (obj is null)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                return obj.ISBN.GetHashCode(StringComparison.InvariantCulture) +
                    obj.Author.GetHashCode(StringComparison.InvariantCulture) +
                    obj.Pages.GetHashCode() +
                    obj.Price.GetHashCode();
            }
        }
    }
}
