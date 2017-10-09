using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Gol.Application.Presentation.Behaviors
{
    /// <summary>
    /// Life scroll viewer behavior.
    /// </summary>
    public class GridScrollViewerBehavior : Behavior<ScrollViewer>
    {
        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += ScrollLoaded;
        }

        private void ScrollLoaded(object sender, RoutedEventArgs args)
        {
            AssociatedObject.ScrollToVerticalOffset(AssociatedObject.ScrollableHeight / 2);
            AssociatedObject.ScrollToHorizontalOffset(AssociatedObject.ScrollableWidth / 2);
        }
    }
}