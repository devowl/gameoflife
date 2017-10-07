using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using Gol.Core.Controls.Models;

namespace Gol.Core.Controls.Views
{
    /// <summary>
    /// Mono life grid control.
    /// </summary>
    public class MonoLifeView : StackPanel
    {
        private CellData[,] _cellGrid = new CellData[0, 0];

        /// <summary>
        /// Dependency property for <see cref="IsReadOnly"/> property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty;

        /// <summary>
        /// Dependency property for <see cref="LineBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty LineBrushProperty;

        /// <summary>
        /// Dependency property for <see cref="CellSize"/> property.
        /// </summary>
        public static readonly DependencyProperty CellSizeProperty;

        /// <summary>
        /// Dependency property for <see cref="MonoLifeGrid"/> property.
        /// </summary>
        public static readonly DependencyProperty MonoLifeGridProperty;

        /// <summary>
        /// Static constructor for <see cref="MonoLifeView"/>.
        /// </summary>
        static MonoLifeView()
        {
            IsReadOnlyProperty = DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(MonoLifeView),
                new PropertyMetadata(default(bool)));

            LineBrushProperty = DependencyProperty.Register(
                nameof(LineBrush),
                typeof(Brush),
                typeof(MonoLifeView),
                new PropertyMetadata(Brushes.DarkCyan));

            CellSizeProperty = DependencyProperty.Register(
                nameof(CellSize),
                typeof(int),
                typeof(MonoLifeView),
                new PropertyMetadata(10));

            MonoLifeGridProperty = DependencyProperty.Register(
                nameof(MonoLifeGrid),
                typeof(MonoLifeGrid<bool>),
                typeof(MonoLifeView),
                new PropertyMetadata(
                    default(MonoLifeGrid<bool>),
                    (source, args) => ((MonoLifeView)source).MonoLifeGridModelChanged()));
        }

        /// <summary>
        /// Constructor for <see cref="MonoLifeView"/>.
        /// </summary>
        public MonoLifeView()
        {
            CanvasRef = new Canvas();
            Children.Add(CanvasRef);
        }

        /// <summary>
        /// Is read only grid.
        /// </summary>
        /// <remarks>Prevent mouse marking.</remarks>
        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }

            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }

        /// <summary>
        /// Line brush color.
        /// </summary>
        public Brush LineBrush
        {
            get
            {
                return (Brush)GetValue(LineBrushProperty);
            }

            set
            {
                SetValue(LineBrushProperty, value);
            }
        }

        /// <summary>
        /// Cell size (pixels).
        /// </summary>
        public int CellSize
        {
            get
            {
                return (int)GetValue(CellSizeProperty);
            }

            set
            {
                SetValue(CellSizeProperty, value);
            }
        }

        /// <summary>
        /// Mono grid model.
        /// </summary>
        public MonoLifeGrid<bool> MonoLifeGrid
        {
            get
            {
                return (MonoLifeGrid<bool>)GetValue(MonoLifeGridProperty);
            }

            set
            {
                SetValue(MonoLifeGridProperty, value);
            }
        }

        private Canvas CanvasRef { get; }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs args)
        {
            base.OnMouseUp(args);
            
            if (!IsReadOnly && args.LeftButton == MouseButtonState.Released)
            {
                var position = args.GetPosition(this);
                int x = (int)(position.X / CellSize), y = (int)(position.Y / CellSize);

                if (0 <= x && x < MonoLifeGrid.Width && 0 <= y && y < MonoLifeGrid.Height)
                {
                    var value = !MonoLifeGrid[x, y];
                    MonoLifeGrid[x, y] = value;
                    if (value)
                    {
                        DrawSquare(x, y);
                    }
                    else
                    {
                        _cellGrid[x, y].ClearRectangle();
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void MonoLifeGridModelChanged()
        {
            if (CanvasRef == null)
            {
                return;
            }
            
            if (Dispatcher.CheckAccess())
            {
                GridRender();
            }
            else
            {
                Dispatcher.Invoke(new Action(GridRender));
            }
        }
        
        private void GridRender()
        {
            CanvasRef.Children.Clear();
            if (MonoLifeGrid.Height == 0 || MonoLifeGrid.Width == 0)
            {
                return;
            }

            SquareGrid();

            _cellGrid = new CellData[MonoLifeGrid.Width, MonoLifeGrid.Height];

            for (int i = 0; i < MonoLifeGrid.Height; i++)
            {
                for (int j = 0; j < MonoLifeGrid.Width; j++)
                {
                    _cellGrid[j, i] = new CellData(j, i, CanvasRef);

                    if (MonoLifeGrid[j, i])
                    {
                        DrawSquare(j, i);
                    }
                }
            }
        }

        private void DrawSquare(int x, int y)
        {
            var square = new Rectangle() { Fill = LineBrush, Width = CellSize, Height = CellSize };

            CanvasRef.Children.Add(square);
            Canvas.SetLeft(square, x * CellSize);
            Canvas.SetTop(square, y * CellSize);

            _cellGrid[x, y].SetRectangle(square);
        }

        private void SquareGrid()
        {
            // Rows
            for (int i = 0; i <= MonoLifeGrid.Height; i++)
            {
                CanvasRef.Children.Add(CreateLine(0, i * CellSize, MonoLifeGrid.Width * CellSize, i * CellSize));
            }

            // Columns
            for (int j = 0; j <= MonoLifeGrid.Width; j++)
            {
                CanvasRef.Children.Add(CreateLine(j * CellSize, 0, j * CellSize, MonoLifeGrid.Height * CellSize));
            }
        }

        private Line CreateLine(int x1, int y1, int x2, int y2)
        {
            return new Line { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, StrokeThickness = 1, Stroke = LineBrush };
        }

        private class CellData
        {
            private readonly Canvas _canvas;

            /// <summary>
            /// Constructor for <see cref="CellData"/>.
            /// </summary>
            public CellData(int x, int y, Canvas canvas)
            {
                _canvas = canvas;
                X = x;
                Y = y;
            }

            /// <summary>
            /// X value.
            /// </summary>
            public int X { get; private set; }

            /// <summary>
            /// Y value.
            /// </summary>
            public int Y { get; private set; }

            /// <summary>
            /// Cell rectangle reference.
            /// </summary>
            public Rectangle Rectangle { get; private set; }

            /// <summary>
            /// Set rectangle value.
            /// </summary>
            /// <param name="rectangle"><see cref="Rectangle"/> instance.</param>
            public void SetRectangle(Rectangle rectangle)
            {
                if (Rectangle != null)
                {
                    _canvas.Children.Remove(Rectangle);
                }

                Rectangle = rectangle;
            }

            /// <summary>
            /// Clear saved rectangle.
            /// </summary>
            public void ClearRectangle()
            {
                if (Rectangle != null)
                {
                    _canvas.Children.Remove(Rectangle);
                }
            }
        }
    }
}