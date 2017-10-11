using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Gol.Core.Data;

namespace Gol.Core.Controls.Models
{
    /// <summary>
    /// Mono life grid model.
    /// </summary>
    [DataContract]
    public class MonoLifeGrid<TValue>
    {
        [DataMember]
        private TValue[][] _sourceArray;

        /// <summary>
        /// Constructor for <see cref="MonoLifeGrid{TValue}"/>.
        /// </summary>
        public MonoLifeGrid()
        {
        }

        /// <summary>
        /// Constructor for <see cref="MonoLifeGrid{TValue}"/>.
        /// </summary>
        public MonoLifeGrid(TValue[][] sourceArray, Guid lifeId)
        {
            Initialize(sourceArray, lifeId);
        }

        /// <summary>
        /// Constructor for <see cref="MonoLifeGrid{TValue}"/>.
        /// </summary>
        public MonoLifeGrid(TValue[,] sourceArray, Guid lifeId)
        {
            Initialize(sourceArray.ToJaggedArray(), lifeId);
        }

        private void Initialize(TValue[][] sourceArray, Guid lifeId)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            if (lifeId == Guid.Empty)
            {
                throw new ArgumentException(nameof(lifeId));
            }

            if (sourceArray == null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (sourceArray[i] == null)
                {
                    throw new ArgumentNullException($"sourceArray [{i}] is null");
                }
            }

            _sourceArray = sourceArray;
            LifeId = lifeId;
        }

        /// <summary>
        /// Life field identity.
        /// </summary>
        [DataMember]
        public Guid LifeId { get; private set; }

        /// <summary>
        /// Grid width.
        /// </summary>
        public int Width => _sourceArray.GetLength(0);

        /// <summary>
        /// Grid height.
        /// </summary>
        public int Height => _sourceArray[0].GetLength(0);

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
                return _sourceArray[x][y];
            }

            set
            {
                _sourceArray[x][y] = value;
            }
        }

        /// <summary>
        /// Create array copy.
        /// </summary>
        /// <returns></returns>
        public MonoLifeGrid<TValue> Clone()
        {
            var copy = new TValue[Width][];
            for (int i = 0; i < Width; i++)
            {
                var second = new TValue[Height];
                Array.Copy(_sourceArray[i], second, Height);
                copy[i] = second;
            }
            
            return new MonoLifeGrid<TValue>(copy, LifeId);
        }
    }
}