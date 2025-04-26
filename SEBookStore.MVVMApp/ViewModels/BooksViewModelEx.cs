#if GENERATEDCODE_ON
using Avalonia.Controls;
using SEBookStore.MVVMApp.Models;

namespace SEBookStore.MVVMApp.ViewModels
{
    partial class BooksViewModel
    {
        protected override GenericItemViewModel<Book> CreateViewModel()
        {
            return new BookViewModel();
        }

        protected override Window CreateWindow()
        {
            return new Views.BookWindow();
        }
    }
}
#endif