using System.Windows;

namespace MEC.Views;

/// <summary>
/// Interaction logic for CarrierView.xaml
/// </summary>
public partial class CarrierView : Window
{
    public CarrierView()
    {
        InitializeComponent();

        if (DataContext is CarrierViewModel viewModel)
            viewModel.Window = this;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is CarrierViewModel viewModel)
            viewModel.Loaded();
    }
}
