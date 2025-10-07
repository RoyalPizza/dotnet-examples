using System.Windows;

namespace MEC.Views
{
    /// <summary>
    /// Interaction logic for TroubleView.xaml
    /// </summary>
    public partial class TroubleView : Window
    {
        public TroubleView()
        {
            InitializeComponent();
            if (DataContext is TroubleViewModel viewModel)
                viewModel.Window = this;
        }
    }
}
