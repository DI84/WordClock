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
            var viewModel = new WindowViewModel(windowService, clockEngine);

            mainWindow.DataContext = viewModel;
            mainWindow.Closed += (s, args) => viewModel.Dispose();
            mainWindow.Show();
        }
    }
}
