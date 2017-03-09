using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Wordclock
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private members

        /// <summary>
        /// The window the viemodel is binding to
        /// </summary>
        private MainWindow mWindow;

        /// <summary>
        /// Timer to keep up the good time
        /// </summary>
        private DispatcherTimer mTimer;

        /// <summary>
        /// Window topmost state
        /// </summary>
        private bool mWindowTopemost;

        /// <summary>
        /// the chars displyed in the clock
        /// </summary>
        private string[] mClockChars = { "E","S","K","I","S","T","L","F","Ü","N","F","Z","E","H","N","Z","W","A","N","Z","I","G",
                                        "D","R","E","I","V","I","E","R","T","E","L","T","G","N","A","C","H","V","O","R","J","M",
                                        "H","A","L","B","Q","Z","W","Ö","L","F","P","Z","W","E","I","N","S","I","E","B","E","N",
                                        "K","D","R","E","I","R","H","F","Ü","N","F","E","L","F","N","E","U","N","V","I","E","R",
                                        "W","A","C","H","T","Z","E","H","N","R","S","B","S","E","C","H","S","F","M","U","H","R",
                                      };

        private ObservableCollection<ShowChar> mClockCharCollection;


        #endregion

        #region PublicProperties

        /// <summary>
        /// The window min height
        /// </summary>
        public Double MinHeightWindow { get; set; } = 300;

        /// <summary>
        /// The window max height
        /// </summary>
        public Double MaxHeightWindow { get; set; } = 1000;

        /// <summary>
        /// The window min wisth
        /// </summary>
        public Double MinWidthWindow { get; set; } = 300;
        
        /// <summary>
        /// The resize delta on mouse scroll wheel event
        /// </summary>
        public Double ResizeDelta { get; set; } = 30;

        /// <summary>
        /// returns the actual window state
        /// </summary>
        public WindowState StateOfWindow { get { return mWindow.WindowState; } }

        /// <summary>
        /// Window is allways on the forground if true
        /// </summary>
        bool IsTopmostOn { get { return mWindowTopemost; } }

        /// <summary>
        /// Basic opacity of inactiv chars
        /// </summary>
        double BasicOpacity { get; set; } = 0.2;

        /// <summary>
        /// The collection that is making up the clock
        /// </summary>
        public ObservableCollection<ShowChar> ClockCharCollection  { get { return mClockCharCollection; } set { mClockCharCollection = value; } }

        /// <summary>
        /// Actual time that the clock shows
        /// </summary>
        public DateTime ActTime { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Close the mWindow
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Maximize / minimize the mWindow
        /// </summary>
        public ICommand MaxMinCommand { get; set; }

        /// <summary>
        /// command to switch between window topemost true/false
        /// </summary>
        public ICommand TopmostCommand { get; set; }


        #endregion

        #region Public constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel (MainWindow window)
        {
            mWindow = window;

            // Init
            EvaluateTime(this, null);

            // Set and activate the timer. Updates the clock every 10sec.
            mTimer = new DispatcherTimer();
            mTimer.Interval = new TimeSpan(0, 0, 5);
            mTimer.Tick += EvaluateTime;
            mTimer.Start();

            // Create commands
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MaxMinCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            TopmostCommand = new RelayCommand(() => SwitchTopemost());

            // Get the mouse events
            mWindow.MouseWheel += mWindow_MouseWheel;
            mWindow.MouseLeftButtonDown += mWindow_LMouseButtonDown;
        }

        #endregion

        #region Private methods
        
        /// <summary>
        /// Adds an item to the ClockCharCollection
        /// </summary>
        /// <param name="i"></param>
        private void UpdateClockCharCollection(int i)
        {
            ClockCharCollection.Insert(i, new ShowChar() { Row = (int)(i / 11.0), Column = i % 11, Text = mClockChars[i], Opacity = 1.0 });
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

            if (dt.Minute >= 25)
                dt = dt.AddHours(1);

            ClockCharCollection = new ObservableCollection<ShowChar>();
            for (int i = mClockChars.Length - 1; i >= 0; --i)
                ClockCharCollection.Add(new ShowChar() { Row = (int)(i / 11.0), Column = i % 11, Text = mClockChars[i], Opacity = BasicOpacity });

            // Es ist
            for (int i = 0; i <= 1; i++)
                UpdateClockCharCollection(i);
            for (int i = 3; i <= 5; i++)
                UpdateClockCharCollection(i);

            // Minutenauswertung (Grob)
            if (dt.Minute >= 25 && dt.Minute <= 35)
            {
                // halb
                for (int i = 44; i <= 47; i++)
                    UpdateClockCharCollection(i);
            }

            if ((dt.Minute > 35 && dt.Minute <= 55) || dt.Minute == 25)
            {
                // vor
                for (int i = 39; i <= 41; i++)
                    UpdateClockCharCollection(i);
            }
            else if ((dt.Minute > 0 && dt.Minute < 25) || dt.Minute == 35)
            {
                // nach
                for (int i = 35; i <= 38; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Minute == 0)
            {
                // Uhr
                for (int i = 107; i <= 109; i++)
                    UpdateClockCharCollection(i);
            }
            
            // Minutenauswertung (fein)
            if (dt.Minute == 5 || dt.Minute == 55 || dt.Minute == 25 || dt.Minute == 35)
            {
                // Fünf (nach/vor)
                for (int i = 7; i <= 10; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Minute == 10 || dt.Minute == 50)
            {
                // Zehn (nach/vor)
                for (int i = 11; i <= 14; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Minute == 15 || dt.Minute == 45)
            {
                // Viertel (nach/vor)
                for (int i = 26; i <= 32; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Minute == 20 || dt.Minute == 40)
            {
                // Zwanzig (nach/vor)
                for (int i = 15; i <= 21; i++)
                    UpdateClockCharCollection(i);
            }

            // Stundenauswertung
            if(dt.Hour == 1 || dt.Hour == 13)
            {
                if (dt.Minute == 0)
                {
                    for (int i = 57; i <= 59; i++)
                        UpdateClockCharCollection(i);
                }
                else
                {
                    for (int i = 57; i <= 60; i++)
                        UpdateClockCharCollection(i);
                }
            }
            else if (dt.Hour == 2 || dt.Hour == 14)
            {
                for (int i = 55; i <= 58; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 3 || dt.Hour == 15)
            {
                for (int i = 67; i <= 70; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 4 || dt.Hour == 16)
            {
                for (int i = 84; i <= 87; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 5 || dt.Hour == 17)
            {
                for (int i = 73; i <= 76; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 6 || dt.Hour == 18)
            {
                for (int i = 100; i <= 104; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 7 || dt.Hour == 19)
            {
                for (int i = 60; i <= 65; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 8 || dt.Hour == 20)
            {
                for (int i = 89; i <= 92; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 9 || dt.Hour == 21)
            {
                for (int i = 80; i <= 83; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 10 || dt.Hour == 22)
            {
                for (int i = 93; i <= 96; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 11 || dt.Hour == 23)
            {
                for (int i = 77; i <= 79; i++)
                    UpdateClockCharCollection(i);
            }
            else if (dt.Hour == 12 || dt.Hour == 0)
            {
                for (int i = 49; i <= 53; i++)
                    UpdateClockCharCollection(i);
            }
        }
        
        #endregion

        #region Public methods/events

        /// <summary>
        /// Resizes the mWindow according to the mousewheel delta
        /// </summary>
        /// <param name="sender">Mouse</param>
        /// <param name="e">MouseWheelEventArgs</param>
        public void mWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (mWindow.WindowState != WindowState.Normal)
                return;

            if (e.Delta > 0 && mWindow.ActualHeight + ResizeDelta <= MaxHeightWindow)
            {
                mWindow.Height += ResizeDelta;
                mWindow.Width += ResizeDelta;
            }
            else if (e.Delta < 0 && mWindow.ActualHeight - ResizeDelta >= MinHeightWindow)
            {
                mWindow.Height -= ResizeDelta;
                mWindow.Width -= ResizeDelta;
            }
        }

        /// <summary>
        /// Drag mWindow on left mouse down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void mWindow_LMouseButtonDown(object sender, MouseEventArgs e)
        {
            mWindow.DragMove();
        }

        /// <summary>
        /// Switch between topemost true/false
        /// </summary>
        public void SwitchTopemost()
        {
            mWindow.Topmost = mWindowTopemost = !mWindowTopemost;
        }

        #endregion
    }
}
