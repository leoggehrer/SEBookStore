﻿//@GeneratedCode
namespace SEBookStore.MVVMApp.ViewModels
{
    using System;
    /// <summary>
    /// Generated by the generator.
    /// </summary>
    public partial class BookViewModel : GenericItemViewModel<Models.Book>
    {
        /// <summary>
        /// Initializes the class (created by the generator).
        /// </summary>
        static BookViewModel()
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
        /// Initializes a new instance of the <see cref="BookViewModel"/> class (created by the generator.)
        /// </summary>
        public BookViewModel()
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
        public System.String ISBNNumber
        {
            get
            {
                return Model.ISBNNumber;
            }
            set
            {
                Model.ISBNNumber = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property 'Author' generated by the generator.
        /// </summary>
        public System.String Author
        {
            get
            {
                return Model.Author;
            }
            set
            {
                Model.Author = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property 'Title' generated by the generator.
        /// </summary>
        public System.String Title
        {
            get
            {
                return Model.Title;
            }
            set
            {
                Model.Title = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property 'Description' generated by the generator.
        /// </summary>
        public System.String Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                Model.Description = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property 'YearOfRelease' generated by the generator.
        /// </summary>
        public System.Int32 YearOfRelease
        {
            get
            {
                return Model.YearOfRelease;
            }
            set
            {
                Model.YearOfRelease = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property 'Price' generated by the generator.
        /// </summary>
        public System.Double Price
        {
            get
            {
                return Model.Price;
            }
            set
            {
                Model.Price = value;
                OnPropertyChanged();
            }
        }
    }
}
