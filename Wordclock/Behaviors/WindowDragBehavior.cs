using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Wordclock
{
    /// <summary>
    /// Allows dragging a window by clicking and holding the left mouse button
    /// </summary>
    public class WindowDragBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= OnMouseLeftButtonDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.DragMove();
        }
    }
}
