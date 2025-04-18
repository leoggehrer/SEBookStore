﻿//@GeneratedCode
namespace SEBookStore.WebApi.Models
{
    using System;
    /// <summary>
    /// Generated by the generator.
    /// </summary>
    public partial class BookEdit
    {
        /// <summary>
        /// Initializes the class (created by the generator).
        /// </summary>
        static BookEdit()
        {
            ClassConstructing();
            ClassConstructed();
        }
        /// <summary>
        /// This method is called before the construction of the class.
        /// </summary>
        static partial void ClassConstructing();
        /// <summary>
        /// This method is called when the class is constructed.
        /// </summary>
        static partial void ClassConstructed();

        /// <summary>
        /// Initializes a new instance of the <see cref="BookEdit"/> class (created by the generator.)
        /// </summary>
        public BookEdit()
        {
            Constructing();

            Constructed();
        }
        /// <summary>
        /// This method is called the object is being constraucted.
        /// </summary>
        partial void Constructing();
        /// <summary>
        /// This method is called when the object is constructed.
        /// </summary>
        partial void Constructed();
        /// <summary>
        /// Property 'ISBNNumber' generated by the generator.
        /// </summary>
        /// <summary>
        /// Property 'ISBNNumber' generated by the generator.
        /// </summary>
        public System.String ISBNNumber { get; set; } = string.Empty;
        /// <summary>
        /// Property 'Author' generated by the generator.
        /// </summary>
        /// <summary>
        /// Property 'Author' generated by the generator.
        /// </summary>
        public System.String Author { get; set; } = string.Empty;
        /// <summary>
        /// Property 'Title' generated by the generator.
        /// </summary>
        /// <summary>
        /// Property 'Title' generated by the generator.
        /// </summary>
        public System.String Title { get; set; } = string.Empty;
        /// <summary>
        /// Property 'Description' generated by the generator.
        /// </summary>
        /// <summary>
        /// Property 'Description' generated by the generator.
        /// </summary>
        public System.String Description { get; set; } = string.Empty;
        /// <summary>
        /// Property 'YearOfRelease' generated by the generator.
        /// </summary>
        /// <summary>
        /// Property 'YearOfRelease' generated by the generator.
        /// </summary>
        public System.Int32 YearOfRelease { get; set; }
        /// <summary>
        /// Property 'Price' generated by the generator.
        /// </summary>
        /// <summary>
        /// Property 'Price' generated by the generator.
        /// </summary>
        public System.Double Price { get; set; }
    }
}
