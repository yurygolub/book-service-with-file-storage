using System;
using System.Text.RegularExpressions;

#pragma warning disable CA1305 // Specify IFormatProvider

namespace VerificationService
{
    /// <summary>
    /// Verifies if the string representation of number is a valid ISBN-10 or ISBN-13 identification number of book.
    /// </summary>
    public static class IsbnVerifier
    {
        /// <summary>
        /// Verifies if the string representation of number is a valid ISBN-10 or ISBN-13 identification number of book.
        /// </summary>
        /// <param name="number">The string representation of book's isbn.</param>
        /// <returns>true if number is a valid ISBN-10 or ISBN-13 identification number of book, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if isbn is null.</exception>
        public static bool IsValid(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentNullException(nameof(number));
            }

            Regex regex = new Regex("^[0-9]{0,3}-{0,1}[0-9]{1}-{0,1}[0-9]{3}-{0,1}[0-9]{5}-{0,1}[0-9X]{1}$");
            if (!regex.IsMatch(number))
            {
                return false;
            }

            string isbn = number.Replace("-", string.Empty, StringComparison.InvariantCulture);
            return isbn.Length switch
            {
                10 => GetIsbn10CheckSum(isbn) % 11 == 0,
                13 => char.IsDigit(isbn[12]) && GetIsbn13CheckSum(isbn) % 10 == 0,
                _ => false,
            };

            static int GetIsbn10CheckSum(string isbn)
            {
                int checkSum = 0;
                for (int i = 0, j = isbn.Length; i < isbn.Length; i++, j--)
                {
                    checkSum += char.IsDigit(isbn[i]) switch
                    {
                        true => int.Parse(isbn[i].ToString()) * j,
                        false => 10,
                    };
                }

                return checkSum;
            }

            static int GetIsbn13CheckSum(string isbn)
            {
                int checkSum = 0;
                for (int i = 0; i < isbn.Length; i++)
                {
                    checkSum += int.Parse(isbn[i].ToString()) * ((i % 2 == 0) ? 1 : 3);
                }

                return checkSum;
            }
        }
    }
}
