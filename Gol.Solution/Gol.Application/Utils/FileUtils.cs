using System;
using System.IO;
using System.Runtime.Serialization;

using Gol.Core.Algorithm;
using Gol.Core.Controls.Models;

using Microsoft.Win32;

namespace Gol.Application.Utils
{
    /// <summary>
    /// File utility methods.
    /// </summary>
    public static class FileUtils
    {
        private const string ExamplesDirectoryName = "Examples";

        private const string FilesFilter = "Life saves (*.life)|*.life|All files (*.*)|*.*";

        /// <summary>
        /// Get example folder location.
        /// </summary>
        /// <returns>Return default initial directory path.</returns>
        public static string GetInitialDirectory()
        {
            var initialDirectory = Environment.CurrentDirectory;
            var examplePath = Path.Combine(Environment.CurrentDirectory, ExamplesDirectoryName);
            if (Directory.Exists(examplePath))
            {
                initialDirectory = examplePath;
            }

            return initialDirectory;
        }

        public static bool TryGetSaveFile(out Stream saveStream)
        {
            saveStream = null;
            var saveDialog = new SaveFileDialog()
            {
                InitialDirectory = GetInitialDirectory(),
                Filter = FilesFilter,
                RestoreDirectory = true
            };

            var dialogResult = saveDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                saveStream = saveDialog.OpenFile();
                return true;
            }

            return false;
        }

        public static bool TryGetOpenFile(out Stream openStream)
        {
            openStream = null;
            var openDialog = new OpenFileDialog()
            {
                InitialDirectory = GetInitialDirectory(),
                Filter = FilesFilter,
                RestoreDirectory = true
            };

            var dialogResult = openDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                openStream = openDialog.OpenFile();
                return true;
            }

            return false;
        }
    }
}