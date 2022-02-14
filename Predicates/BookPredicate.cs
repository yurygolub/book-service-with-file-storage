using System;
using BookClass;
using Interfaces;

namespace Predicates
{
    /// <summary>
    /// Defines a predicate a book object.
    /// </summary>
    public class BookPredicate : IPredicate<Book>
    {
        /// <summary>
        /// Represents the method that determines whether the specified book object has an isbn number.
        /// </summary>
        /// <param name="obj">The object to compare against the criteria.</param>
        /// <returns><see langword="true"/> if book object has an isbn number, <see langword="false"/> otherwise.</returns>
        public bool Verify(Book obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return !string.IsNullOrEmpty(obj.ISBN);
        }
    }
}
