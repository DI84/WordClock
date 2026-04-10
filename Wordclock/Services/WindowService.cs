using System;
using System.Windows;

namespace Wordclock
{
    /// <summary>
    /// Window service implementation wrapping a WPF Window
    /// </summary>
    public class WindowService : IWindowService
    {
        private readonly Window m_Window;

        public WindowService(Window window)
        {
            m_Window = window;
            m_Window.StateChanged += (s, e) => WindowStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsMaximized => m_Window.WindowState == WindowState.Maximized;

        public event EventHandler WindowStateChanged;

        public void Close() => m_Window.Close();

        public void ToggleMaximize() => m_Window.WindowState ^= WindowState.Maximized;

        public void SetTopmost(bool topmost) => m_Window.Topmost = topmost;
    }
}
