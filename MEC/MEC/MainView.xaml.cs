using MEC.Core;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace MEC;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainView : Window
{
    private ILogger<MainView> _logger;

    public MainView()
    {
        InitializeComponent();
        _logger = AppHost.shared.GetLogger<MainView>();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
            viewModel.Loaded();
    }

    private void Window_Unloaded(object sender, RoutedEventArgs e)
    {
        // TODO: Unloaded basically never fires. Use closing instead or something
        if (DataContext is MainViewModel viewModel)
            viewModel.Unloaded();
    }
}