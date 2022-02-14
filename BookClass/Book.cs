using System;
using VerificationService;

namespace BookClass
{
    /// <summary>
    /// Represents the book as a type of publication.
    /// </summary>
    public sealed class Book : IEquatable<Book>, IComparable<Book>
    {
        private int totalPages;
        private bool published;
        private DateTime datePublished;

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="author">Autor of the book.</param>
        /// <param name="title">Title of the book.</param>
        /// <param name="publisher">Publisher of the book.</param>
        /// <exception cref="ArgumentNullException">Throw when author or title or publisher is null.</exception>
        public Book(string author, string title, string publisher)
            : this(author, title, publisher, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Book"/> class.
        /// </summary>
        /// <param name="author">Autor of the book.</param>
        /// <param name="title">Title of the book.</param>
        /// <param name="publisher">Publisher of the book.</param>
        /// <param name="isbn">International Standard Book Number.</param>
        /// <exception cref="ArgumentNullException">Throw when author or title or publisher or ISBN is null.</exception>
        /// <exception cref="ArgumentException">Throw when ISBN is invalid.</exception>
        public Book(string author, string title, string publisher, string isbn)
        {
            if (author is null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (publisher is null)
            {
                throw new ArgumentNullException(nameof(publisher));
            }

            if (isbn is null)
            {
                throw new ArgumentNullException(nameof(isbn));
            }

            if (isbn.Length != 0 && !IsbnVerifier.IsValid(isbn))
            {
                throw new ArgumentException("ISBN is invalid.");
            }

            this.Author = author;
            this.Title = title;
            this.Publisher = publisher;
            this.ISBN = isbn;
        }

        /// <summary>
        /// Gets author of the book.
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// Gets title of the book.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets publisher of the book.
        /// </summary>
        public string Publisher { get; }

        /// <summary>
        /// Gets or sets total pages in the book.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throw when Pages less or equal zero.</exception>
        public int Pages
        {
            get
            {
                return this.totalPages;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Pages cannot be less or equal zero.");
                }

                this.totalPages = value;
            }
        }

        /// <summary>
        /// Gets International Standard Book Number.
        /// </summary>
        public string ISBN { get; }

        /// <summary>
        /// Gets price.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// Gets currency.
        /// </summary>
        public string Currency { get; private set; } = System.Globalization.RegionInfo.CurrentRegion.ISOCurrencySymbol;

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><see langword="true"/> if both operands are equal, <see langword="false"/> otherwise.</returns>
        public static bool operator ==(Book left, Book right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            return !(left is null) && left.Equals(right);
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><see langword="true"/> if both operands are not equal, <see langword="false"/> otherwise.</returns>
        public static bool operator !=(Book left, Book right) => !(left == right);

        /// <summary>
        /// Greater than operator.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><see langword="true"/> if its left-hand operand is greater than its right-hand operand, <see langword="false"/> otherwise.</returns>
        public static bool operator >(Book left, Book right) => !(left is null) && left.CompareTo(right) > 0;

        /// <summary>
        /// Greater than or equal operator.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>
        /// <see langword="true"/> if its left-hand operand is greater than or equal to its right-hand operand, <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator >=(Book left, Book right) => !(left is null) && left.CompareTo(right) >= 0;

        /// <summary>
        /// Less than operator.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns><see langword="true"/> if its left-hand operand is less than its right-hand operand, <see langword="false"/> otherwise.</returns>
        public static bool operator <(Book left, Book right) => !(left is null) && left.CompareTo(right) < 0;

        /// <summary>
        /// Less than or equal operator.
        /// </summary>
        /// <param name="left">Left operand.</param>
        /// <param name="right">Right operand.</param>
        /// <returns>
        /// <see langword="true"/> if its left-hand operand is less than or equal to its right-hand operand, <see langword="false"/> otherwise.
        /// </returns>
        public static bool operator <=(Book left, Book right) => !(left is null) && left.CompareTo(right) <= 0;

        /// <summary>
        /// Publishes the book if it has not yet been published.
        /// </summary>
        /// <param name="dateTime">Date of publish.</param>
        public void Publish(DateTime dateTime)
        {
            this.published = true;
            this.datePublished = dateTime;
        }

        /// <summary>
        /// String representation of book.
        /// </summary>
        /// <returns>Representation of book.</returns>
        public override string ToString() => $"{this.Title} by {this.Author}";

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => this.Equals(obj as Book);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Book other) => !(other is null) && this.ISBN == other.ISBN;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => this.ISBN.GetHashCode(StringComparison.InvariantCulture);

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(Book other)
        {
            if (other is null)
            {
                throw new ArgumentException($"{nameof(other)} can not be null.");
            }

            return string.Compare(this.Title, other.Title, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Gets a information about time of publish.
        /// </summary>
        /// <returns>The string "NYP" if book not published, and the value of the datePublished if it is published.</returns>
        public string GetPublicationDate()
        {
            if (!this.published)
            {
                return "NYP";
            }
            else
            {
                return this.datePublished.ToString("d", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Sets the prise and currency of the book.
        /// </summary>
        /// <param name="price">Price of book.</param>
        /// <param name="currency">Currency of book.</param>
        /// <exception cref="ArgumentException">Throw when Price less than zero or currency is invalid.</exception>
        /// <exception cref="ArgumentNullException">Throw when currency is null.</exception>
        public void SetPrice(decimal price, string currency)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (price < 0)
            {
                throw new ArgumentException("Price cannot be less than zero.");
            }

            if (!IsoCurrencyValidator.IsValid(currency))
            {
                throw new ArgumentException("Currency is invalid.");
            }

            this.Price = price;
            this.Currency = currency;
        }
    }
}
