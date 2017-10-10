using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Gol.Application.Presentation.Views;
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
        private DoubleStateLife _doubleStateLife;

        private const int DefaultFieldWidth = 250;

        private const int DefaultFieldHeight = 200;

        private const string ExamplesDirectoryName = "Examples";

        private const string FilesFilter = "Life saves (*.life)|*.life|All files (*.*)|*.*";

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
        }

        private void Open(object obj)
        {
            var openDialog = new OpenFileDialog()
            {
                InitialDirectory = GetInitialDirectory(),
                Filter = FilesFilter,
                RestoreDirectory = true,
                Multiselect = false,
                
            };
            var dialogResult = openDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var openFile = File.Open(openDialog.FileName, FileMode.Open))
                {
                    var type = typeof(MonoLifeGrid<bool>);
                    var serializer = new DataContractSerializer(type);
                    var grid = (MonoLifeGrid<bool>)serializer.ReadObject(openFile);
                    DoubleStateLife = new DoubleStateLife(grid);
                }
            }
        }

        private string GetInitialDirectory()
        {
            var initialDirectory = Environment.CurrentDirectory;
            var examplePath = Path.Combine(Environment.CurrentDirectory, ExamplesDirectoryName);
            if (Directory.Exists(examplePath))
            {
                initialDirectory = examplePath;
            }

            return initialDirectory;
        }

        private void Save(object obj)
        {
            var saveDialog = new SaveFileDialog()
            {
                InitialDirectory = GetInitialDirectory(),
                Filter = FilesFilter,
                RestoreDirectory = true
            };

            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                using (var saveFile = saveDialog.OpenFile())
                {
                    var type = typeof(MonoLifeGrid<bool>);
                    var serializer = new DataContractSerializer(type);
                    serializer.WriteObject(saveFile, DoubleStateLife.Current);
                }
            }
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
    }
}