using System.Windows;
using System.Windows.Controls;

using Gol.Core.Controls.Models;

namespace Gol.Core.Controls.Views
{
    /// <summary>
    /// Mono life grid control.
    /// </summary>
    public class MonoLifeView : Control
    {
        /// <summary>
        /// Dependency property for <see cref="MonoLifeGridModel"/> property.
        /// </summary>
        public static readonly DependencyProperty MonoLifeGridModelProperty;

        /// <summary>
        /// Static constructor for <see cref="MonoLifeView"/>.
        /// </summary>
        static MonoLifeView()
        {
            MonoLifeGridModelProperty = DependencyProperty.Register(
                nameof(MonoLifeGridModel),
                typeof(MonoLifeGridModel),
                typeof(MonoLifeView),
                new PropertyMetadata(default(MonoLifeGridModel)));
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
    }
}