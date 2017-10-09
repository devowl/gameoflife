using Gol.Core.Controls.Models;

namespace Gol.Core.Data
{
    /// <summary>
    /// Life control methods.
    /// </summary>
    public interface ILifeControl<TValue>
    {
        /// <summary>
        /// Previous generation.
        /// </summary>
        MonoLifeGrid<TValue> Previous { get; }

        /// <summary>
        /// Current generation.
        /// </summary>
        MonoLifeGrid<TValue> Current { get; }

        /// <summary>
        /// Set start generation.
        /// </summary>
        /// <param name="grid">Grid object reference.</param>
        void SetCurrent(MonoLifeGrid<TValue> grid);

        /// <summary>
        /// Start\Resume life cycle.
        /// </summary>
        bool Start();

        /// <summary>
        /// Stop life.
        /// </summary>
        void Stop();
    }
}