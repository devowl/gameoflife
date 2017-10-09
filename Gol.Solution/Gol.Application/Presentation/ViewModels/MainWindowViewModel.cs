using Gol.Application.Presentation.Views;
using Gol.Core.Algorithm;
using Gol.Core.Controls.Models;
using Gol.Core.Prism;

namespace Gol.Application.Presentation.ViewModels
{
    /// <summary>
    /// <see cref="MainWindow"/> ViewModel.
    /// </summary>
    public class MainWindowViewModel : NotificationObject
    {

        /// <summary>
        /// Constructor for <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            DoubleStateLife = new DoubleStateLife();
            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            DoubleStateLife.SetCurrent(new MonoLifeGrid<bool>(new bool[25, 20]));
            DoubleStateLife.PropertyChanged += (sender, args) => RaisePropertyChanged(nameof(DoubleStateLife));
        }

        private void Stop(object obj)
        {
            DoubleStateLife.Stop();
        }

        private void Start(object obj)
        {
            DoubleStateLife.Start();
        }

        /// <summary>
        /// Map for mono grid.
        /// </summary>
        public DoubleStateLife DoubleStateLife { get; private set; }

        /// <summary>
        /// Start command.
        /// </summary>
        public DelegateCommand StartCommand { get; private set; }

        /// <summary>
        /// Stop command.
        /// </summary>
        public DelegateCommand StopCommand { get; private set; }
    }
}