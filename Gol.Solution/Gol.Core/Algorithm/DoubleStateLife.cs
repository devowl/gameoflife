using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Gol.Core.Controls.Models;
using Gol.Core.Data;
using Gol.Core.Prism;

namespace Gol.Core.Algorithm
{
    /// <summary>
    /// Black and White life cycle algorithm.
    /// </summary>
    public class DoubleStateLife : NotificationObject, ILifeControl<bool>
    {
        private readonly TimeSpan _realtimeDelay = TimeSpan.FromMilliseconds(50);

        private Thread _updateTimer;

        private bool _stopUpdateTimer;

        //    -1   0   1
        // -1 [ ] [ ] [ ]
        //  0 [ ] [X] [ ]
        //  1 [ ] [ ] [ ]
        private readonly Offset[] _offsets = {

            // Line 1
            new Offset(-1, -1),
            new Offset(0, -1),
            new Offset(1, -1),

            // Line 2
            new Offset(-1, 0),
            new Offset(1, 0),

            // Line 3
            new Offset(-1, 1),
            new Offset(0, 1),
            new Offset(1, 1),
        };

        private MonoLifeGrid<bool> _current;

        private MonoLifeGrid<bool> _previous;

        private int _gernerationNumber;

        /// <summary>
        /// Constructor for <see cref="DoubleStateLife"/>.
        /// </summary>
        public DoubleStateLife(MonoLifeGrid<bool> current) 
            : this()
        {
            SetCurrent(current);
        }

        /// <summary>
        /// Constructor for <see cref="DoubleStateLife"/>.
        /// </summary>
        public DoubleStateLife()
        {
        }

        /// <inheritdoc/>
        public MonoLifeGrid<bool> Previous
        {
            get
            {
                return _previous;
            }

            private set
            {
                _previous = value;
                RaisePropertyChanged(() => Previous);
            }
        }

        /// <inheritdoc/>
        public MonoLifeGrid<bool> Current
        {
            get
            {
                return _current;
            }

            private set
            {
                _current = value;
                RaisePropertyChanged(() => Current);
            }
        }

        /// <summary>
        /// Generation number.
        /// </summary>
        public int GernerationNumber
        {
            get
            {
                return _gernerationNumber;
            }

            set
            {
                _gernerationNumber = value;
                RaisePropertyChanged(nameof(GernerationNumber));
            }
        }

        /// <inheritdoc/>
        public void SetCurrent(MonoLifeGrid<bool> grid)
        {
            if (grid == null)
            {
                Stop();
            }
            else
            {
                GernerationNumber = 0;
                Current = grid;
            }
        }

        /// <inheritdoc/>
        public void Start()
        {
            if (Current == null || Current.Height == 0 || Current.Width == 0)
            {
                return;
            }

            if (_updateTimer != null)
            {
                Stop();
            }

            _stopUpdateTimer = false;
            _updateTimer = new Thread(TimerElapsed);
            _updateTimer.Start();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            _stopUpdateTimer = true;
            _updateTimer.Join();
            _updateTimer = null;
        }

        private void TimerElapsed(object obj)
        {
            while (!_stopUpdateTimer)
            {
                LifeStep();
                Thread.Sleep(_realtimeDelay);
            }
        }

        private int NearCells(int x, int y, MonoLifeGrid<bool> field)
        {
            return _offsets.Sum(offset => IsBlack(x + offset.Dx, y + offset.Dy, field));
        }

        private static int IsBlack(int x, int y, MonoLifeGrid<bool> field)
        {
            if ((0 <= x && x < field.Width) && (0 <= y && y < field.Height))
            {
                return field[x, y] ? 1 : 0;
            }

            return 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LifeStep()
        {
            /*
            https://ru.wikipedia.org/wiki/%D0%96%D0%B8%D0%B7%D0%BD%D1%8C_(%D0%B8%D0%B3%D1%80%D0%B0)
            - В пустой (мёртвой) клетке, рядом с которой ровно три живые клетки, зарождается жизнь;
            - Если у живой клетки есть две или три живые соседки, то эта клетка продолжает жить; в противном случае, если соседей 
              меньше двух или больше трёх, клетка умирает («от одиночества» или «от перенаселённости»)
            */
            _previous = _current;
            _current = _current.Clone();
            for (int i = 0; i < _current.Width; i ++)
            {
                for (int j = 0; j < _current.Height; j ++)
                {
                    var nearCells = NearCells(i, j, Previous);
                    bool isCreature = Previous[i, j];

                    if (isCreature)
                    {
                        // Если у живой клетки есть две или три живые соседки, то эта клетка продолжает жить;
                        _current[i, j] = nearCells == 2 || nearCells == 3;
                    }
                    else
                    {
                        // В пустой (мёртвой) клетке, рядом с которой ровно три живые клетки, зарождается жизнь;
                        _current[i, j] = nearCells == 3;
                    }

                }
            }

            GernerationNumber++;
            RaisePropertyChanged(nameof(Current));
            RaisePropertyChanged(nameof(Previous));
        }

        private class Offset
        {
            /// <summary>
            /// Constructor for <see cref="Offset"/>.
            /// </summary>
            public Offset(int dx, int dy)
            {
                Dx = dx;
                Dy = dy;
            }

            /// <summary>
            /// X offset.
            /// </summary>
            public int Dx { get; }

            /// <summary>
            /// Y offset.
            /// </summary>
            public int Dy { get; }
        }
    }
}