using SEBookStore.Logic.Contracts;
using SEBookStore.Logic.Exceptions;

namespace SEBookStore.Logic.Entities
{
    /// <summary>
    /// Represents a book entity with validation logic.
    /// </summary>
    partial class Book : IValidatableEntity
    {
        /// <summary>
        /// Validates the properties of the book entity.o
        /// </summary>
        /// <param name="context">The context in which the validation is performed.</param>
        /// <exception cref="BusinessException">
        /// Thrown when any of the following validation rules are violated:
        /// <list type="bullet">
        /// <item><description>The ISBN number is invalid.</description></item>
        /// <item><description>The author's name is less than 3 characters long.</description></item>
        /// <item><description>The title is less than 5 characters long.</description></item>
        /// <item><description>The year of release is not between 1900 and the next calendar year.</description></item>
        /// <item><description>The price is not between EUR 1 and EUR 10,000.</description></item>
        /// </list>
        /// </exception>
        public void Validate(IContext context, EntityState entityState)
        {
            // A1
            if (CheckISBNNumber(ISBNNumber) == false)
            {
                throw new BusinessException("Invalid ISBN number");
            }
            // A2
            if (Author.Length < 3)
            {
                throw new BusinessException("The character length of the author must be at least 3 characters long.");
            }
            // A3
            if (Title.Length < 5)
            {
                throw new BusinessException("The character length of the title must be at least 5 characters long.");
            }
            // A4
            if (YearOfRelease < 1900 || YearOfRelease > DateTime.Now.Year + 1)
            {
                throw new BusinessException($"The publication must be between 1900 and {DateTime.Now.Year + 1}.");
            }
            // A5
            if (Price < 1.0 || Price > 10_000.0)
            {
                throw new BusinessException("The price must be between EUR 1 and EUR 10,000.");
            }
        }

        /// <summary>
        /// Validates the given ISBN number based on the ISBN-10 standard.
        /// </summary>
        /// <param name="number">The ISBN number to validate.</param>
        /// <returns>
        /// Returns <c>true</c> if the ISBN number is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The method checks if the input string is 10 characters long, where the first 9 characters are digits,
        /// and the last character can be a digit or 'X' (case-insensitive). It also verifies the checksum
        /// using the modulo 11 algorithm.
        /// </remarks>
        public static bool CheckISBNNumber(string number)
        {
            var result = number != null && number.Where((c, i) => i == 9 ? (c == 'X' || c == 'x' || char.IsDigit(c)) : char.IsDigit(c)).Count() == 10;

            var sum = 0;
            var rest = 0;

            for (int i = 0; result && i < number?.Length - 1; i++)
            {
                sum += (number == null ? 0 : number[i] - '0') * (i + 1);
            }

            rest = sum % 11;

            return result && number != null && ((rest == 10 && char.ToUpper(number[^1]) == 'X') || (rest == number[^1] - '0'));
        }
    }
}
