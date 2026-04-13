using System.Windows;

namespace Wordclock
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow();

            var windowService = new WindowService(mainWindow);
            var clockEngine = new WordClockEngine();
            var clock = new ClockViewModel(clockEngine);
            var viewModel = new WindowViewModel(windowService);

            mainWindow.DataContext = viewModel;
            mainWindow.ClockView.DataContext = clock;
            mainWindow.Closed += (s, args) => clock.Dispose();
            mainWindow.Show();
        }
    }
}
