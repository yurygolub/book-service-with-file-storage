using System;
using System.Globalization;

namespace BookClass
{
    public class BookFormat : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var book = arg as Book;
            if (book == null)
            {
                if (arg is IFormattable formattable)
                {
                    return formattable.ToString(format, CultureInfo.CurrentCulture);
                }
                else if (arg != null)
                {
                    return arg.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            if (string.IsNullOrEmpty(format))
            {
                format = "H";
            }

            return format.ToUpperInvariant() switch
            {
                "H" => "Product details:\n" +
                    $"Author: {book.Author}\n" +
                    $"Title: {book.Title}\n" +
                    $"Publisher: {book.Publisher}, {book.Year}\n" +
                    $"ISBN-13: {book.ISBN}\n" +
                    $"Paperback: {book.Pages} pages",
                _ => throw new FormatException($"The {format} format string is not supported."),
            };
        }
    }
}
