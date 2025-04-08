using SEBookStore.Logic.Contracts;
using SEBookStore.Logic.Exceptions;

namespace SEBookStore.Logic.Entities
{
    partial class Book : IValidatableEntity
    {
        public void Validate(IContext context)
        {
            if (CheckISBNNumer(ISBNNumber) == false)
            {
                throw new BusinessException("Invalid ISBN number");
            }

            if (Author.Length < 3)
            {
                throw new BusinessException("The character length of the author must be at least 3 characters long.");
            }

            if (Title.Length < 5)
            {
                throw new BusinessException("The character length of the title must be at least 5 characters long.");
            }

            if (YearOfRelease < 1900 || YearOfRelease > DateTime.Now.Year + 1)
            {
                throw new BusinessException($"The publication must be between 1900 and {DateTime.Now.Year + 1}.");
            }

            if (Price < 1.0 || Price > 10_000.0)
            {
                throw new BusinessException("The price must be between EUR 1 and EUR 10,000.");
            }
        }

        public static bool CheckISBNNumer(string number)
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
