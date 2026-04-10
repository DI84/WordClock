using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Wordclock
{
    /// <summary>
    /// Resizes a window proportionally via the mouse wheel
    /// </summary>
    public class WindowResizeBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty ResizeDeltaProperty =
            DependencyProperty.Register(nameof(ResizeDelta), typeof(double), typeof(WindowResizeBehavior), new PropertyMetadata(30.0));

        public static readonly DependencyProperty MaxHeightLimitProperty =
            DependencyProperty.Register(nameof(MaxHeightLimit), typeof(double), typeof(WindowResizeBehavior), new PropertyMetadata(1000.0));

        /// <summary>
        /// The amount of pixels to resize per scroll step
        /// </summary>
        public double ResizeDelta
        {
            get => (double)GetValue(ResizeDeltaProperty);
            set => SetValue(ResizeDeltaProperty, value);
        }

        /// <summary>
        /// The maximum window height allowed when resizing
        /// </summary>
        public double MaxHeightLimit
        {
            get => (double)GetValue(MaxHeightLimitProperty);
            set => SetValue(MaxHeightLimitProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseWheel += OnMouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseWheel -= OnMouseWheel;
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var window = AssociatedObject;

            if (window.WindowState != WindowState.Normal)
                return;

            if (e.Delta > 0 && window.ActualHeight + ResizeDelta <= MaxHeightLimit)
            {
                window.Height += ResizeDelta;
                window.Width += ResizeDelta;
            }
            else if (e.Delta < 0 && window.ActualHeight - ResizeDelta >= window.MinHeight)
            {
                window.Height -= ResizeDelta;
                window.Width -= ResizeDelta;
            }
        }
    }
}
