using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace Wordclock
{
    public class WindowViewModel : BaseViewModel, IDisposable
    {
        private readonly IWindowService mWindowService;
        private readonly IWordClockEngine mClockEngine;
        private readonly DispatcherTimer mTimer;
        private bool mWindowTopmost;
        private ObservableCollection<ShowChar> mClockCharCollection;

        /// <summary>
        /// The window min height
        /// </summary>
        public double MinHeightWindow { get; set; } = 300;

        /// <summary>
        /// The window min width
        /// </summary>
        public double MinWidthWindow { get; set; } = 300;

        /// <summary>
        /// Basic opacity of inactive chars
        /// </summary>
        double BasicOpacity { get; set; } = 0.2;

        /// <summary>
        /// The collection that is making up the clock
        /// </summary>
        public ObservableCollection<ShowChar> ClockCharCollection { get { return mClockCharCollection; } set { mClockCharCollection = value; } }

        /// <summary>
        /// Actual time that the clock shows
        /// </summary>
        public DateTime ActTime { get; set; }

        /// <summary>
        /// Whether the window is currently maximized
        /// </summary>
        public bool WindowIsMaximized { get; set; }

        /// <summary>
        /// Window is always on the foreground if true
        /// </summary>
        public bool IsTopmostOn => mWindowTopmost;

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
        /// <param name="clockEngine">Engine that determines active clock indices</param>
        public WindowViewModel(IWindowService windowService, IWordClockEngine clockEngine)
        {
            mWindowService = windowService;
            mClockEngine = clockEngine;

            // React to external window state changes
            mWindowService.WindowStateChanged += (s, e) => WindowIsMaximized = mWindowService.IsMaximized;

            // Init
            EvaluateTime(this, null);

            // Set and activate the timer. Updates the clock every 5 seconds.
            mTimer = new DispatcherTimer();
            mTimer.Interval = new TimeSpan(0, 0, 5);
            mTimer.Tick += EvaluateTime;
            mTimer.Start();

            // Create commands
            CloseCommand = new RelayCommand(() => mWindowService.Close());
            MaxMinCommand = new RelayCommand(() => ToggleMaximize());
            TopmostCommand = new RelayCommand(() => SwitchTopmost());
        }

        /// <summary>
        /// Evaluates the time and fills the ClockCharCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvaluateTime(object sender, object e)
        {
            var dt = DateTimeHelper.RoundToNearest(DateTime.Now, TimeSpan.FromMinutes(5));

            if (ActTime == dt)
                return;
            else
                ActTime = dt;

            var activeIndices = mClockEngine.GetActiveIndices(dt);
            var chars = mClockEngine.ClockChars;

            var collection = new ObservableCollection<ShowChar>();
            for (int i = 0; i < chars.Length; i++)
            {
                collection.Add(new ShowChar
                {
                    Row = i / 11,
                    Column = i % 11,
                    Text = chars[i],
                    Opacity = activeIndices.Contains(i) ? 1.0 : BasicOpacity
                });
            }

            ClockCharCollection = collection;
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
            mWindowTopmost = !mWindowTopmost;
            mWindowService.SetTopmost(mWindowTopmost);
        }

        /// <summary>
        /// Disposes the timer
        /// </summary>
        public void Dispose()
        {
            mTimer.Stop();
        }
    }
}
