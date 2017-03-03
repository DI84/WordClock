using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Wordclock
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private members

        private MainWindow mWindow;

        private DispatcherTimer mTimer;

        #endregion

        #region PublicProperties

        bool[] CIA = new bool[110];

        #endregion

        public ICommand CloseCommand { get; set; }

        #region Public constructor

        public WindowViewModel (MainWindow window)
        {
            mWindow = window;

            EvaluateTime(this, null);

            mTimer = new DispatcherTimer();
            mTimer.Interval = new TimeSpan(0, 0, 10);
            mTimer.Tick += EvaluateTime;
            mTimer.Start();

            // Create Commands
            CloseCommand = new RelayCommand(() => mWindow.Close());

            // Mouse Events
            mWindow.MouseWheel += mWindow_MouseWheel;
            mWindow.MouseLeftButtonDown += mWindow_LMouseButtonDown;
        }

        #endregion

        #region Private methods

        private void EvaluateTime(object sender, object e)
        {
            for (int i = CIA.Length -1; i >= 0; --i)
            {
                CIA[i] = false;
            }

            var dt = DateTime.Now;

            if (dt.Minute >= 28)
                dt = dt.AddHours(1);

            // Es ist
            CIA[0] = CIA[1] = CIA[3] = CIA[4] = CIA[5] = true;
            
            // Minutenauswertung (Grob)
            if(dt.Minute <= 37 && dt.Minute >= 23)
            {
                // halb
                CIA[44] = CIA[45] = CIA[46] = CIA[47] = true;
            }

            if (dt.Minute > 37 && dt.Minute < 58)
            {
                // vor
                CIA[39] = CIA[40] = CIA[41] = true;
            }
            else if (dt.Minute < 23 && dt.Minute > 2)
            {
                // nach
                CIA[35] = CIA[36] = CIA[37] = CIA[38] = true;
            }
            else if (dt.Minute > 32 && dt.Minute < 38)
            {
                // nach halb
                CIA[35] = CIA[36] = CIA[37] = CIA[38] = true;
            }
            else if (dt.Minute < 28 && dt.Minute > 22)
            {
                // vor halb
                CIA[39] = CIA[40] = CIA[41] = true;
            }
            else if (dt.Minute >= 58 || dt.Minute <= 2)
            {
                // Uhr
                CIA[107] = CIA[108] = CIA[109] = true;
            }

            // Minutenauswertung (fein)
            if ((dt.Minute > 2 && dt.Minute <= 7) || (dt.Minute <= 57 && dt.Minute > 52))
            {
                // Fünf (nach/vor)
                CIA[7] = CIA[8] = CIA[9] = CIA[10] = true;
            }
            else if ((dt.Minute > 7 && dt.Minute <= 12) || (dt.Minute <= 52 && dt.Minute > 47))
            {
                // Zehn (nach/vor)
                CIA[11] = CIA[12] = CIA[13] = CIA[14] = true;
            }
            else if ((dt.Minute > 12 && dt.Minute <= 17) || (dt.Minute <= 47 && dt.Minute > 42))
            {
                // Viertel (nach/vor)
                CIA[26] = CIA[27] = CIA[28] = CIA[29] = CIA[30] = CIA[31] = CIA[32] = true;
            }
            else if ((dt.Minute > 17 && dt.Minute <= 22) || (dt.Minute <= 42 && dt.Minute > 37))
            {
                // Zwanzig (nach/vor)
                CIA[15] = CIA[16] = CIA[17] = CIA[18] = CIA[19] = CIA[20] = CIA[21] = true;
            }
            else if ((dt.Minute > 22 && dt.Minute <= 27) || (dt.Minute <= 37 && dt.Minute > 32))
            {
                // Fünf (nach/vor)
                CIA[7] = CIA[8] = CIA[9] = CIA[10] = true;
            }

            // Stundenauswertung
            if(dt.Hour == 1 || dt.Hour == 13)
            {
                CIA[58] = CIA[59] = CIA[60] = CIA[61] = true;
            }
            else if (dt.Hour == 2 || dt.Hour == 14)
            {
                CIA[56] = CIA[57] = CIA[58] = CIA[59] = true;
            }
            else if (dt.Hour == 3 || dt.Hour == 15)
            {
                CIA[67] = CIA[68] = CIA[69] = CIA[70] = true;
            }
            else if (dt.Hour == 4 || dt.Hour == 16)
            {
                CIA[84] = CIA[85] = CIA[86] = CIA[87] = true;
            }
            else if (dt.Hour == 5 || dt.Hour == 17)
            {
                CIA[73] = CIA[74] = CIA[75] = CIA[76] = true;
            }
            else if (dt.Hour == 6 || dt.Hour == 18)
            {
                CIA[100] = CIA[101] = CIA[102] = CIA[103] = CIA[104] = true;
            }
            else if (dt.Hour == 7 || dt.Hour == 19)
            {
                CIA[60] = CIA[61] = CIA[62] = CIA[63] = CIA[64] = CIA[65] = true;
            }
            else if (dt.Hour == 8 || dt.Hour == 20)
            {
                CIA[89] = CIA[90] = CIA[91] = CIA[92] = true;
            }
            else if (dt.Hour == 9 || dt.Hour == 21)
            {
                CIA[80] = CIA[81] = CIA[82] = CIA[83] = true;
            }
            else if (dt.Hour == 10 || dt.Hour == 22)
            {
                CIA[93] = CIA[94] = CIA[95] = CIA[96] = true;
            }
            else if (dt.Hour == 11 || dt.Hour == 23)
            {
                CIA[77] = CIA[78] = CIA[79] = true;
            }
            else if (dt.Hour == 12 || dt.Hour == 0)
            {
                CIA[49] = CIA[50] = CIA[51] = CIA[52] = CIA[53] = true;
            }


            // Update UI
            for (int i = CIA.Length - 1; i >= 0; --i)
            {
                var TB = (TextBlock)((Viewbox)mWindow.CharGrid.Children[i]).Child;

                if(CIA[i])
                    TB.Foreground = (SolidColorBrush)Application.Current.FindResource("CharActivBrush");
                else
                    TB.Foreground = (SolidColorBrush)Application.Current.FindResource("CharInactivBrush");
            }
        }
        

        #endregion

        #region Public methods

        public void mWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            int ResizeDelta = 30;

            if (e.Delta > 0 && mWindow.ActualHeight + ResizeDelta <= mWindow.MaxHeight)
            {
                mWindow.Height += ResizeDelta;
                mWindow.Width += ResizeDelta;
            }
            else if (e.Delta < 0 && mWindow.ActualHeight - ResizeDelta >= mWindow.MinHeight)
            {
                mWindow.Height -= ResizeDelta;
                mWindow.Width -= ResizeDelta;
            }
        }

        public void mWindow_LMouseButtonDown(object sender, MouseEventArgs e)
        {
            mWindow.DragMove();
        }

        #endregion
    }
}
