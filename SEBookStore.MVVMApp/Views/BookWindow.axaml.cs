using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SEBookStore.MVVMApp.Views;

public partial class BookWindow : Window
{
    public BookWindow()
    {
        InitializeComponent();

#if GENERATEDCODE_ON
        if (DataContext is ViewModels.BookViewModel bvm)
        {
            bvm.CloseAction = () => Close();
        }
#endif
    }
}