using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace Gol.Application.Presentation.Views
{
    /// <summary>
    /// About window.
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Constructor for <see cref="AboutWindow"/>.
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}