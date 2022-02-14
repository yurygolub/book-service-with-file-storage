using System;
using System.Collections.Generic;
using BookClass;

namespace Comparers
{
    /// <summary>
    /// Exposes a method that compares two objects.
    /// </summary>
    public class BookComparer : IComparer<Book>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of x and y.</returns>
        public int Compare(Book x, Book y)
        {
            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (string.Compare(x.Author, y.Author, StringComparison.InvariantCulture) != 0)
            {
                return string.Compare(x.Author, y.Author, StringComparison.InvariantCulture);
            }
            else if (x.Pages.CompareTo(y.Pages) != 0)
            {
                return x.Pages.CompareTo(y.Pages);
            }
            else if (x.Price.CompareTo(y.Price) != 0)
            {
                return x.Price.CompareTo(y.Price);
            }

            return 0;
        }
    }
}
