using System.Windows.Controls;

namespace MEC.Views;

/// <summary>
///     Interaction logic for NumberpadView.xaml
/// </summary>
public partial class NumberpadView : UserControl
{
    public NumberpadView()
    {
        InitializeComponent();
    }

    private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is NumberpadViewModel viewModel)
        {
            viewModel.Unloaded();
        }
    }
}