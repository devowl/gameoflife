using System.Windows;

using Gol.Application.Presentation.ViewModels;

namespace Gol.Application.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor for <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
    }
}
