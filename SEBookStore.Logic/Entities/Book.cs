//@CodeCopy
namespace SEBookStore.Logic.Entities
{
    /// <summary>
    /// Represents a book entity.
    /// </summary>
    [Table("Books")]
    [Index(nameof(ISBNNumber), IsUnique = true)]
    public partial class Book : EntityObject
    {
        private string iSBNNumber = string.Empty;

        /// <summary>
        /// Gets or sets the ISBN number of the book.
        /// </summary>
        [Required, MaxLength(10)]
        public string ISBNNumber 
        { 
            get => iSBNNumber; 
            set => iSBNNumber = value.Replace("-", string.Empty); 
        }
        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        [Required, MaxLength(128)]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        [Required, MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the book.
        /// </summary>
        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year of release of the book.
        /// </summary>
        public int YearOfRelease { get; set; }

        /// <summary>
        /// Gets or sets the price of the book.
        /// </summary>
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Author} - {Title}";
        }
    }

}
