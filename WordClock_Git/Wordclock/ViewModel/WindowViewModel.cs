using System;
using System.Windows;
using System.Windows.Controls;
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
        /// Array that saves the fontcolor of the textblocks as bool (activ/inactiv)
        /// </summary>
        /// 
        double[] CIA = new double[110];

        /// <summary>
        /// Basic opacity of inactiv chars
        /// </summary>
        double BasicOpacity { get; set; } = 0.2;

        /// <summary>
        /// Window is allways on the forground if true
        /// </summary>
        bool IsTopmostOn { get { return mWindowTopemost; } }

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

            EvaluateTime(this, null);

            // Set and activate the Timer. Updates the Clock every 10sec.
            mTimer = new DispatcherTimer();
            mTimer.Interval = new TimeSpan(0, 0, 10);
            mTimer.Tick += EvaluateTime;
            mTimer.Start();

            // Create Commands
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MaxMinCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            TopmostCommand = new RelayCommand(() => SwitchTopemost());

            // Get the mouse events
            mWindow.MouseWheel += mWindow_MouseWheel;
            mWindow.MouseLeftButtonDown += mWindow_LMouseButtonDown;
        }

        #endregion

        #region Private methods

        private void EvaluateTime(object sender, object e)
        {
            for (int i = CIA.Length -1; i >= 0; --i)
            {
                CIA[i] = BasicOpacity;
            }

            double FullOpac = 1.00;
            var dt = DateTime.Now;

            if (dt.Minute >= 23)
                dt = dt.AddHours(1);

            // Es ist
            CIA[0] = CIA[1] = CIA[3] = CIA[4] = CIA[5] = FullOpac;


            // Minutenauswertung (Grob)
            if (dt.Minute <= 37 && dt.Minute >= 23)
            {
                // halb
                CIA[44] = CIA[45] = CIA[46] = CIA[47] = FullOpac;
            }

            if (dt.Minute > 37 && dt.Minute < 58)
            {
                // vor
                CIA[39] = CIA[40] = CIA[41] = FullOpac;
            }
            else if (dt.Minute < 23 && dt.Minute > 2)
            {
                // nach
                CIA[35] = CIA[36] = CIA[37] = CIA[38] = FullOpac;
            }
            else if (dt.Minute > 32 && dt.Minute < 38)
            {
                // nach halb
                CIA[35] = CIA[36] = CIA[37] = CIA[38] = FullOpac;
            }
            else if (dt.Minute < 28 && dt.Minute > 22)
            {
                // vor halb
                CIA[39] = CIA[40] = CIA[41] = FullOpac;
            }
            else if (dt.Minute >= 58 || dt.Minute <= 2)
            {
                // Uhr
                CIA[107] = CIA[108] = CIA[109] = FullOpac;
            }

            // Minutenauswertung (fein)
            if ((dt.Minute > 2 && dt.Minute <= 7) || (dt.Minute <= 57 && dt.Minute > 52))
            {
                // Fünf (nach/vor)
                CIA[7] = CIA[8] = CIA[9] = CIA[10] = FullOpac;
            }
            else if ((dt.Minute > 7 && dt.Minute <= 12) || (dt.Minute <= 52 && dt.Minute > 47))
            {
                // Zehn (nach/vor)
                CIA[11] = CIA[12] = CIA[13] = CIA[14] = FullOpac;
            }
            else if ((dt.Minute > 12 && dt.Minute <= 17) || (dt.Minute <= 47 && dt.Minute > 42))
            {
                // Viertel (nach/vor)
                CIA[26] = CIA[27] = CIA[28] = CIA[29] = CIA[30] = CIA[31] = CIA[32] = FullOpac;
            }
            else if ((dt.Minute > 17 && dt.Minute <= 22) || (dt.Minute <= 42 && dt.Minute > 37))
            {
                // Zwanzig (nach/vor)
                CIA[15] = CIA[16] = CIA[17] = CIA[18] = CIA[19] = CIA[20] = CIA[21] = FullOpac;
            }
            else if ((dt.Minute > 22 && dt.Minute <= 27) || (dt.Minute <= 37 && dt.Minute > 32))
            {
                // Fünf (nach/vor)
                CIA[7] = CIA[8] = CIA[9] = CIA[10] = FullOpac;
            }

            // Stundenauswertung
            if(dt.Hour == 1 || dt.Hour == 13)
            {
                if(CIA[107] == FullOpac)
                    CIA[57] = CIA[58] = CIA[59] = FullOpac;
                else
                    CIA[57] = CIA[58] = CIA[59] = CIA[60] = FullOpac;
            }
            else if (dt.Hour == 2 || dt.Hour == 14)
            {
                CIA[55] = CIA[56] = CIA[57] = CIA[58] = FullOpac;
            }
            else if (dt.Hour == 3 || dt.Hour == 15)
            {
                CIA[67] = CIA[68] = CIA[69] = CIA[70] = FullOpac;
            }
            else if (dt.Hour == 4 || dt.Hour == 16)
            {
                CIA[84] = CIA[85] = CIA[86] = CIA[87] = FullOpac;
            }
            else if (dt.Hour == 5 || dt.Hour == 17)
            {
                CIA[73] = CIA[74] = CIA[75] = CIA[76] = FullOpac;
            }
            else if (dt.Hour == 6 || dt.Hour == 18)
            {
                CIA[100] = CIA[101] = CIA[102] = CIA[103] = CIA[104] = FullOpac;
            }
            else if (dt.Hour == 7 || dt.Hour == 19)
            {
                CIA[60] = CIA[61] = CIA[62] = CIA[63] = CIA[64] = CIA[65] = FullOpac;
            }
            else if (dt.Hour == 8 || dt.Hour == 20)
            {
                CIA[89] = CIA[90] = CIA[91] = CIA[92] = FullOpac;
            }
            else if (dt.Hour == 9 || dt.Hour == 21)
            {
                CIA[80] = CIA[81] = CIA[82] = CIA[83] = FullOpac;
            }
            else if (dt.Hour == 10 || dt.Hour == 22)
            {
                CIA[93] = CIA[94] = CIA[95] = CIA[96] = FullOpac;
            }
            else if (dt.Hour == 11 || dt.Hour == 23)
            {
                CIA[77] = CIA[78] = CIA[79] = FullOpac;
            }
            else if (dt.Hour == 12 || dt.Hour == 0)
            {
                CIA[49] = CIA[50] = CIA[51] = CIA[52] = CIA[53] = FullOpac;
            }


            // Update UI
            for (int i = CIA.Length - 1; i >= 0; --i)
            {
                var TB = (TextBlock)((Viewbox)mWindow.CharGrid.Children[i]).Child;

                TB.Opacity = CIA[i];
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
