#if GENERATEDCODE_ON
using SEBookStore.Logic.Exceptions;

namespace SEBookStore.ConApp
{
    partial class Program
    {
        static partial void ImportData()
        {
            int index = 0;
            var filePath = Path.Combine("Data", "book_dataset.csv");
            var books = File.ReadAllLines(filePath).Skip(1)
                .Select(line => line.Split(';'))
                .Select(parts => new Logic.Entities.Book
                {
                    ISBNNumber = parts[0],
                    Author = parts[1],
                    Description = parts[2],
                    Price = double.Parse(parts[3]),
                    Title = parts[4],
                    YearOfRelease = int.Parse(parts[5])
                });

            foreach (var book in books)
            {
                using var context = CreateContext();

                try
                {
                    index++;
                    context.BookSet.Add(book);
                    context.SaveChanges();
                }
                catch (BusinessException ex)
                {
                    Console.WriteLine($"Error on line {index} {book}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error on line {index}: {ex.InnerException}");
                }
            }
        }
    }
}
#endif