using System.Windows;

namespace MEC.Views;

/// <summary>
/// Interaction logic for PierView.xaml
/// </summary>
public partial class PierView : Window
{
    public PierView()
    {
        InitializeComponent();

        if (DataContext is PierViewModel viewModel)
            viewModel.Window = this;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is PierViewModel viewModel)
            viewModel.Loaded();
    }
}
