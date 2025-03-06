using Avalonia.Controls;

namespace AvaloniaPrueba.Views;

public partial class MainWindowView : Window
{
    public MainWindowView()
    {
        InitializeComponent();
        DataContext = new ViewModels.MainWindowViewModel();
    }
}