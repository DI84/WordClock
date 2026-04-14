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

            var clockEngine = new WordClockEngine();
            var clockViwModel = new ClockViewModel(clockEngine);

            var mainWindow = new MainWindow();
            var windowService = new WindowService(mainWindow);
            var mainViewModel = new WindowViewModel(windowService);

            mainWindow.DataContext = mainViewModel;
            mainWindow.ClockView.DataContext = clockViwModel;
            mainWindow.Show();
        }
    }
}
