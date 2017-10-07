using Gol.Application.Presentation.Views;
using Gol.Core.Controls.Models;

namespace Gol.Application.Presentation.ViewModels
{
    /// <summary>
    /// <see cref="MainWindow"/> ViewModel.
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// Constructor for <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            var map = new [,]
            {
                { true, false, true, false },
                { false, true, true, false },
                { true, false, true, false },
                { true, false, true, false },
                { true, false, true, false },
            };

            MonoGrid = new MonoLifeGrid(map);    
        }

        /// <summary>
        /// Map for mono grid.
        /// </summary>
        public MonoLifeGrid MonoGrid { get; private set; }
    }
}