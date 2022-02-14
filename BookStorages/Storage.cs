using System;
using System.Collections.Generic;
using BookClass;
using Interfaces;

namespace BookStorages
{
    /// <summary>
    /// Storage for book class.
    /// </summary>
    public class Storage : IStorage<Book>
    {
        private readonly List<Book> books;

        /// <summary>
        /// Initializes a new instance of the <see cref="Storage"/> class.
        /// </summary>
        public Storage()
        {
            this.books = new List<Book>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Storage"/> class.
        /// </summary>
        /// <param name="books">Books.</param>
        public Storage(IEnumerable<Book> books)
        {
            if (books is null)
            {
                throw new ArgumentNullException(nameof(books));
            }

            this.books = new List<Book>(books);
        }

        /// <summary>
        /// Saves books to current storage.
        /// </summary>
        /// <param name="collection">Collection of books to save.</param>
        public void Save(IEnumerable<Book> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            this.books.AddRange(collection);
        }

        /// <summary>
        /// Loads books from current storage.
        /// </summary>
        /// <returns>Collection of books to load.</returns>
        public IEnumerable<Book> Load()
        {
            return this.books;
        }
    }
}
