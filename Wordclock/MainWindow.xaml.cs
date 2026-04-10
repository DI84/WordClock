using System;
using System.Windows;
using System.Windows.Input;

namespace Wordclock
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ResizeDelta = 30;
        private const double MaxHeightWindow = 1000;

        public MainWindow()
        {
            InitializeComponent();

            var windowService = new WindowService(this);
            var clockEngine = new WordClockEngine();
            var viewModel = new WindowViewModel(windowService, clockEngine);

            DataContext = viewModel;

            MouseWheel += MainWindow_MouseWheel;
            MouseLeftButtonDown += MainWindow_LMouseButtonDown;
            Closed += (s, e) => viewModel.Dispose();
        }

        /// <summary>
        /// Resizes the window according to the mousewheel delta
        /// </summary>
        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (WindowState != WindowState.Normal)
                return;

            if (e.Delta > 0 && ActualHeight + ResizeDelta <= MaxHeightWindow)
            {
                Height += ResizeDelta;
                Width += ResizeDelta;
            }
            else if (e.Delta < 0 && ActualHeight - ResizeDelta >= MinHeight)
            {
                Height -= ResizeDelta;
                Width -= ResizeDelta;
            }
        }

        /// <summary>
        /// Drag window on left mouse down
        /// </summary>
        private void MainWindow_LMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
