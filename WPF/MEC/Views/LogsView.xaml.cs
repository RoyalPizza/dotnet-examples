using System.Windows;

namespace MEC.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView : Window
    {
        public LogsView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is LogsViewModel viewModel)
                viewModel.Loaded();
        }
    }
}
