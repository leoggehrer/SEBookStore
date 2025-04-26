using Avalonia.Controls;
using System;
using System.Threading.Tasks;

namespace SEBookStore.MVVMApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
#if ACCOUNT_OFF
        await Task.Delay(1000); // Simulate a delay for demonstration purposes
#else
        if (ViewModels.LogonViewModel.LogonSession?.SessionToken == default)
        {
            var logonWindow = new LogonWindow();

            await logonWindow.ShowDialog(this);
        }

        if ( ViewModels.LogonViewModel.LogonSession?.SessionToken == default)
        {
            Close();
        }
#endif
    }
}
