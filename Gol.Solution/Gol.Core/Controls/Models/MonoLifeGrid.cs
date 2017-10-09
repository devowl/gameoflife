using System;

namespace Gol.Core.Controls.Models
{
    /// <summary>
    /// Mono life grid model.
    /// </summary>
    public class MonoLifeGrid<TValue>
    {
        private readonly TValue[,] _sourceArray;

        /// <summary>
        /// Constructor for <see cref="MonoLifeGrid{TValue}"/>.
        /// </summary>
        public MonoLifeGrid(TValue[,] sourceArray, Guid lifeId)
            : this(sourceArray)
        {
            LifeId = lifeId;
        }

        /// <summary>
        /// Constructor for <see cref="MonoLifeGrid{TValue}"/>.
        /// </summary>
        public MonoLifeGrid(TValue[,] sourceArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            _sourceArray = (TValue[,])sourceArray.Clone();
            LifeId = Guid.NewGuid();
        }

        /// <summary>
        /// Life field identity.
        /// </summary>
        public Guid LifeId { get; private set; }

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
        /// <returns>Is black point.</returns>
        public TValue this[int x, int y]
        {
            get
            {
                return _sourceArray[x, y];
            }

            set
            {
                _sourceArray[x, y] = value;
            }
        }

        /// <summary>
        /// Create array copy.
        /// </summary>
        /// <returns></returns>
        public MonoLifeGrid<TValue> Clone()
        {
            var array = (TValue[,])_sourceArray.Clone();
            return new MonoLifeGrid<TValue>(array) { LifeId = LifeId };
        }
    }
}