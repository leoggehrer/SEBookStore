using Avalonia.Controls;
using SEBookStore.MVVMApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEBookStore.MVVMApp.ViewModels
{
    public partial class BooksViewModel : GenericItemsViewModel<Models.Book>
    {
        protected override GenericItemViewModel<Book> CreateViewModel()
        {
            return new BookViewModel();
        }

        protected override Window CreateWindow()
        {
            throw new NotImplementedException();
        }
    }
}
