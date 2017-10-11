using System;
using System.IO;
using System.Runtime.Serialization;

using Gol.Application.Presentation.Views;
using Gol.Application.Utils;
using Gol.Core.Algorithm;
using Gol.Core.Controls.Models;
using Gol.Core.Prism;

using Microsoft.Win32;

namespace Gol.Application.Presentation.ViewModels
{
    /// <summary>
    /// <see cref="MainWindow"/> ViewModel.
    /// </summary>
    public class MainWindowViewModel : NotificationObject
    {
        private const int DefaultFieldWidth = 250;

        private const int DefaultFieldHeight = 150;

        private DoubleStateLife _doubleStateLife;

        /// <summary>
        /// Constructor for <see cref="MainWindowViewModel"/>.
        /// </summary>
        public MainWindowViewModel()
        {
            var grid = new MonoLifeGrid<bool>(new bool[DefaultFieldWidth, DefaultFieldHeight], Guid.NewGuid());

            _doubleStateLife = new DoubleStateLife(grid);
            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            SaveCommand = new DelegateCommand(Save);
            ExitCommand = new DelegateCommand(Exit);
            OpenCommand = new DelegateCommand(Open);
            NewCommand = new DelegateCommand(New);
        }

        /// <summary>
        /// Mono grid controller.
        /// </summary>
        public DoubleStateLife DoubleStateLife
        {
            get
            {
                return _doubleStateLife;
            }

            private set
            {
                _doubleStateLife = value;
                RaisePropertyChanged(() => DoubleStateLife);
            }
        }

        /// <summary>
        /// Exit command.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// New command.
        /// </summary>
        public DelegateCommand NewCommand { get; private set; }

        /// <summary>
        /// Start command.
        /// </summary>
        public DelegateCommand StartCommand { get; private set; }

        /// <summary>
        /// Open command.
        /// </summary>
        public DelegateCommand OpenCommand { get; private set; }

        /// <summary>
        /// Save command.
        /// </summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>
        /// Stop command.
        /// </summary>
        public DelegateCommand StopCommand { get; private set; }

        private void Open(object obj)
        {
            Stream fileStream;
            if (FileUtils.TryGetOpenFile(out fileStream))
            {
                using (fileStream)
                {
                    var grid = SerializationUtils.Read<MonoLifeGrid<bool>>(fileStream);
                    DoubleStateLife = new DoubleStateLife(grid);
                }
            }
        }

        private void Save(object obj)
        {
            Stream fileStream;
            if (FileUtils.TryGetSaveFile(out fileStream))
            {
                using (fileStream)
                {
                    SerializationUtils.Save(fileStream, DoubleStateLife.Current);
                }
            }
        }

        private void Exit(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Stop(object obj)
        {
            DoubleStateLife.Stop();
        }

        private void Start(object obj)
        {
            DoubleStateLife.Start();
        }

        private void New(object obj)
        {
            var grid = new MonoLifeGrid<bool>(new bool[DefaultFieldWidth, DefaultFieldHeight], Guid.NewGuid());
            DoubleStateLife = new DoubleStateLife(grid);
        }
    }
}