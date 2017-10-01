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
                { true, false },
                { false, true },
                { true, false },
                { true, false },
                { true, false },
            };

            MonoGrid = new MonoLifeGridModel(map);    
        }

        /// <summary>
        /// Map for mono grid.
        /// </summary>
        public MonoLifeGridModel MonoGrid { get; private set; }
    }
}