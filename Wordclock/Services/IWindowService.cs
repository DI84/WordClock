using System;

namespace Wordclock
{
    /// <summary>
    /// Abstracts window operations for ViewModel decoupling
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// Closes the window
        /// </summary>
        void Close();

        /// <summary>
        /// Toggles between maximized and normal window state
        /// </summary>
        void ToggleMaximize();

        /// <summary>
        /// Sets the window topmost state
        /// </summary>
        void SetTopmost(bool topmost);

        /// <summary>
        /// Gets whether the window is currently maximized
        /// </summary>
        bool IsMaximized { get; }

        /// <summary>
        /// Raised when the window state changes
        /// </summary>
        event EventHandler WindowStateChanged;
    }
}
