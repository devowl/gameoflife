using System;

namespace Gol.Core.Controls.Models
{
    /// <summary>
    /// Mono life grid model.
    /// </summary>
    public class MonoLifeGridModel
    {
        private readonly bool[,] _sourceArray;

        /// <summary>
        /// Constructor for <see cref="MonoLifeGridModel"/>.
        /// </summary>
        public MonoLifeGridModel(bool[,] sourceArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            _sourceArray = (bool[,])sourceArray.Clone();
        }

        /// <summary>
        /// Grid width.
        /// </summary>
        public int Width => _sourceArray.GetLength(0);

        /// <summary>
        /// Grid height.
        /// </summary>
        public int Height => _sourceArray.GetLength(1);

        /// <summary>
        /// Get point black state.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns></returns>
        public bool this[int x, int y] => _sourceArray[x, y];
    }
}