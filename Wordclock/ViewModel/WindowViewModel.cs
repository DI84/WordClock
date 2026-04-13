using System.Windows.Input;

namespace Wordclock
{
    public class WindowViewModel : BaseViewModel
    {
        private readonly IWindowService mWindowService;

        /// <summary>
        /// The window min height
        /// </summary>
        public double MinHeightWindow { get; set; } = 300;

        /// <summary>
        /// The window min width
        /// </summary>
        public double MinWidthWindow { get; set; } = 300;

        /// <summary>
        /// Whether the window is currently maximized
        /// </summary>
        public bool WindowIsMaximized { get; set; }

        /// <summary>
        /// Window is always on the foreground if true
        /// </summary>
        public bool IsTopmostOn { get; set; }

        /// <summary>
        /// Close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Maximize / minimize the window
        /// </summary>
        public ICommand MaxMinCommand { get; set; }

        /// <summary>
        /// Command to switch between window topmost true/false
        /// </summary>
        public ICommand TopmostCommand { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="windowService">Abstraction for window operations</param>
        public WindowViewModel(IWindowService windowService)
        {
            mWindowService = windowService;

            // React to external window state changes
            mWindowService.WindowStateChanged += (s, e) => WindowIsMaximized = mWindowService.IsMaximized;

            // Create commands
            CloseCommand = new RelayCommand(() => mWindowService.Close());
            MaxMinCommand = new RelayCommand(() => ToggleMaximize());
            TopmostCommand = new RelayCommand(() => SwitchTopmost());
        }

        /// <summary>
        /// Switch between topmost true/false
        /// </summary>
        private void ToggleMaximize()
        {
            mWindowService.ToggleMaximize();
            WindowIsMaximized = mWindowService.IsMaximized;
        }

        /// <summary>
        /// Switch between topmost true/false
        /// </summary>
        private void SwitchTopmost()
        {
            IsTopmostOn = !IsTopmostOn;
            mWindowService.SetTopmost(IsTopmostOn);
        }
    }
}
