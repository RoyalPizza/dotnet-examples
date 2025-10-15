using System.Windows;
using System.Windows.Controls;

namespace MEC.Views;

/// <summary>
///     Interaction logic for StatusView.xaml
/// </summary>
public partial class DispatchView : UserControl
{
    public DispatchView()
    {
        InitializeComponent();
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is DispatchViewModel viewModel)
            viewModel.Unloaded();
    }
}