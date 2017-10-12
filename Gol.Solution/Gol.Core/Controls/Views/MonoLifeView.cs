using System;
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
        /// Dependency property for <see cref="CellBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty CellBrushProperty;

        private CellData[,] _cellGrid = new CellData[0, 0];

        private int _currentX = -1;

        private int _currentY = -1;

        private bool _mouseMoving;

        /// <summary>
        /// Static constructor for <see cref="MonoLifeView"/>.
        /// </summary>
        static MonoLifeView()
        {
            CellBrushProperty = DependencyProperty.Register(
                nameof(CellBrush),
                typeof(Brush),
                typeof(MonoLifeView),
                new PropertyMetadata(Brushes.GreenYellow));

            IsReadOnlyProperty = DependencyProperty.Register(
                nameof(IsReadOnly),
                typeof(bool),
                typeof(MonoLifeView),
                new PropertyMetadata(default(bool)));

            LineBrushProperty = DependencyProperty.Register(
                nameof(LineBrush),
                typeof(Brush),
                typeof(MonoLifeView),
                new PropertyMetadata(new BrushConverter().ConvertFrom("#2d2d2d")));

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
                    (source, args) => ((MonoLifeView)source).MonoLifeGridModelChanged((MonoLifeGrid<bool>)args.OldValue)));
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
        /// Cell brush color.
        /// </summary>
        public Brush CellBrush
        {
            get
            {
                return (Brush)GetValue(CellBrushProperty);
            }

            set
            {
                SetValue(CellBrushProperty, value);
            }
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

        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs args)
        {
            base.OnMouseUp(args);
            if (args.LeftButton == MouseButtonState.Released && !_mouseMoving && !IsReadOnly)
            {
                var cell = GetMouseCell(args);
                ProcessMouseSelection(cell);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs args)
        {
            base.OnMouseMove(args);
            if (args.LeftButton == MouseButtonState.Pressed && !IsReadOnly)
            {
                _mouseMoving = true;
                var cell = GetMouseCell(args);
                if (cell.X == _currentX && cell.Y == _currentY)
                {
                    return;
                }

                ProcessMouseSelection(cell, true);
                _currentX = cell.X;
                _currentY = cell.Y;
            }

            if (args.LeftButton == MouseButtonState.Released)
            {
                _currentY = _currentX = -1;
                _mouseMoving = false;
            }
        }

        private void ProcessMouseSelection(IntPoint cell, bool isOnlyDraw = false)
        {
            if (0 <= cell.X && cell.X < MonoLifeGrid.Width && 0 <= cell.Y && cell.Y < MonoLifeGrid.Height)
            {
                var current = MonoLifeGrid[cell.X, cell.Y];
                bool setValue = true;

                if (current)
                {
                    if (!isOnlyDraw)
                    {
                        _cellGrid[cell.X, cell.Y].ClearRectangle();
                    }
                    else
                    {
                        setValue = false;
                    }
                }
                else
                {
                    DrawSquare(cell.X, cell.Y);
                }

                if (setValue)
                {
                    MonoLifeGrid[cell.X, cell.Y] = !current;
                }
            }
        }

        private IntPoint GetMouseCell(MouseEventArgs args)
        {
            var position = args.GetPosition(this);
            int x = (int)(position.X / CellSize), y = (int)(position.Y / CellSize);
            return new IntPoint(x, y);
        }

        private void MonoLifeGridModelChanged(MonoLifeGrid<bool> lastGrid)
        {
            if (CanvasRef == null)
            {
                return;
            }

            var lastLifeId = lastGrid?.LifeId;
            if (Dispatcher.CheckAccess())
            {
                GridRender(lastLifeId);
            }
            else
            {
                Dispatcher.Invoke(new Action(() => GridRender(lastLifeId)));
            }
        }

        private void GridRender(Guid? lastLifeId)
        {
            if (MonoLifeGrid == null || MonoLifeGrid.Height == 0 || MonoLifeGrid.Width == 0)
            {
                return;
            }

            bool isNewGrid = !(lastLifeId.HasValue && lastLifeId == MonoLifeGrid?.LifeId);
            if (isNewGrid)
            {
                CanvasRef.Children.Clear();
                SquareGrid();
                _cellGrid = new CellData[MonoLifeGrid.Width, MonoLifeGrid.Height];
                Width = MonoLifeGrid.Width * CellSize;
                Height = MonoLifeGrid.Height * CellSize;
            }

            for (int i = 0; i < MonoLifeGrid.Width; i++)
            {
                for (int j = 0; j < MonoLifeGrid.Height; j++)
                {
                    CellData cell;
                    if (isNewGrid)
                    {
                        _cellGrid[i, j] = cell = new CellData(i, j, CanvasRef, this);
                    }
                    else
                    {
                        cell = _cellGrid[i, j];
                    }

                    if (MonoLifeGrid[i, j] && !cell.IsBlack)
                    {
                        DrawSquare(i, j);
                    }
                    else if (!MonoLifeGrid[i, j] && cell.IsBlack)
                    {
                        cell.ClearRectangle();
                    }
                }
            }
        }

        private void DrawSquare(int x, int y)
        {
            _cellGrid[x, y].DrawRectangle();
        }

        private void SquareGrid()
        {
            var mainColor = LineBrush;
            var tenthLine = new SolidColorBrush(ChangeColorBrightness(((SolidColorBrush)mainColor).Color, -0.6f));
            
            // Rows
            for (int i = 0; i <= MonoLifeGrid.Height; i++)
            {
                var horizontalLine = CreateLine(0, i * CellSize, MonoLifeGrid.Width * CellSize, i * CellSize);
                horizontalLine.Stroke = i % 10 == 0 ? LineBrush : tenthLine;
                CanvasRef.Children.Add(horizontalLine);
            }

            // Columns
            for (int j = 0; j <= MonoLifeGrid.Width; j++)
            {
                var verticalLine = CreateLine(j * CellSize, 0, j * CellSize, MonoLifeGrid.Height * CellSize);
                verticalLine.Stroke = j % 10 == 0 ? LineBrush : tenthLine;
                CanvasRef.Children.Add(verticalLine);
            }
        }

        private Line CreateLine(int x1, int y1, int x2, int y2)
        {
            return new Line { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, StrokeThickness = 1 };
        }

        private struct IntPoint
        {
            /// <summary>
            /// Constructor for <see cref="IntPoint"/>.
            /// </summary>
            public IntPoint(int x, int y)
            {
                X = x;
                Y = y;
            }

            /// <summary>
            /// X ordinate.
            /// </summary>
            public int X { get; }

            /// <summary>
            /// Y ordinate.
            /// </summary>
            public int Y { get; }
        }

        private class CellData
        {
            private readonly Canvas _canvas;

            private readonly MonoLifeView _parent;

            /// <summary>
            /// Constructor for <see cref="CellData"/>.
            /// </summary>
            public CellData(int x, int y, Canvas canvas, MonoLifeView parent)
            {
                _canvas = canvas;
                _parent = parent;
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
            /// Is cell is black.
            /// </summary>
            public bool IsBlack
            {
                get
                {
                    return _canvas.Children.Contains(Rectangle);
                }
            }

            /// <summary>
            /// Set rectangle value.
            /// </summary>
            public void DrawRectangle()
            {
                if (Rectangle == null)
                {
                    Rectangle = new Rectangle()
                    {
                        Fill = _parent.CellBrush,
                        Width = _parent.CellSize,
                        Height = _parent.CellSize
                    };
                }
                else if (_canvas.Children.Contains(Rectangle))
                {
                    return;
                }

                _canvas.Children.Add(Rectangle);
                Canvas.SetLeft(Rectangle, X * _parent.CellSize);
                Canvas.SetTop(Rectangle, Y * _parent.CellSize);
                Panel.SetZIndex(Rectangle, -1);
            }

            /// <summary>
            /// Clear saved rectangle.
            /// </summary>
            public void ClearRectangle()
            {
                if (Rectangle == null)
                {
                    throw new ArgumentException("Rectangle is not drawed here");
                }

                _canvas.Children.Remove(Rectangle);
            }
        }
    }
}