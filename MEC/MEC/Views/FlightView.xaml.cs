using System.Windows;

namespace MEC.Views;

/// <summary>
///     Interaction logic for FlightView.xaml
/// </summary>
public partial class FlightView : Window
{
    public FlightView()
    {
        InitializeComponent();

        if (DataContext is FlightViewModel viewModel)
            viewModel.Window = this;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is FlightViewModel viewModel)
            viewModel.Loaded();
    }
}