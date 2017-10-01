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
        /// Dependency property for <see cref="MonoLifeGridModel"/> property.
        /// </summary>
        public static readonly DependencyProperty MonoLifeGridModelProperty;

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

            MonoLifeGridModelProperty = DependencyProperty.Register(
                nameof(MonoLifeGridModel),
                typeof(MonoLifeGridModel),
                typeof(MonoLifeView),
                new PropertyMetadata(
                    default(MonoLifeGridModel),
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

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs args)
        {
            base.OnMouseUp(args);

            if (!IsReadOnly && args.LeftButton == MouseButtonState.Released)
            {
                var position = args.GetPosition(this);
                int x = (int)(position.X / CellSize),
                    y = (int)(position.Y / CellSize);

                if (0 <= x && x < MonoLifeGridModel.Width &&
                    0 <= y && y < MonoLifeGridModel.Height)
                {
                    MonoLifeGridModel[x, y] = true;
                    // TODO Draw point
                }
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
        public MonoLifeGridModel MonoLifeGridModel
        {
            get
            {
                return (MonoLifeGridModel)GetValue(MonoLifeGridModelProperty);
            }

            set
            {
                SetValue(MonoLifeGridModelProperty, value);
            }
        }

        private Canvas CanvasRef { get; set; }

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
            if (MonoLifeGridModel.Height == 0 || MonoLifeGridModel.Width == 0)
            {
                return;
            }

            SquareGrid();

            for (int i = 0; i < MonoLifeGridModel.Height; i++)
            {
                for (int j = 0; j < MonoLifeGridModel.Width; j++)
                {
                }
            }
        }

        private void SquareGrid()
        {
            // Rows
            for (int i = 0; i <= MonoLifeGridModel.Height; i++)
            {
                CanvasRef.Children.Add(CreateLine(0, i * CellSize, MonoLifeGridModel.Width * CellSize, i * CellSize));
            }

            // Columns
            for (int j = 0; j <= MonoLifeGridModel.Width; j++)
            {
                CanvasRef.Children.Add(CreateLine(j * CellSize, 0, j * CellSize, MonoLifeGridModel.Height * CellSize));
            }
        }

        private Line CreateLine(int x1, int y1, int x2, int y2)
        {
            return new Line { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2, StrokeThickness = 1, Stroke = LineBrush };
        }
    }
}