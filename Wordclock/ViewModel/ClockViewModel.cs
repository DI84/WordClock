using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Wordclock
{
    public class ClockViewModel : BaseViewModel, IDisposable
    {
        private readonly IWordClockEngine mClockEngine;
        private readonly DispatcherTimer mTimer;
        private int mLastRemainderMinutes;

        /// <summary>
        /// Basic opacity of inactive chars
        /// </summary>
        double BasicOpacity { get; set; } = 0.2;

        /// <summary>
        /// The collection that is making up the clock
        /// </summary>
        public ObservableCollection<ShowChar> ClockCharCollection { get; set; }

        /// <summary>
        /// Minute dot visibility: upper-right corner
        /// </summary>
        public bool Dot1Visible { get; set; }

        /// <summary>
        /// Minute dot visibility: lower-right corner
        /// </summary>
        public bool Dot2Visible { get; set; }

        /// <summary>
        /// Minute dot visibility: lower-left corner
        /// </summary>
        public bool Dot3Visible { get; set; }

        /// <summary>
        /// Minute dot visibility: upper-left corner
        /// </summary>
        public bool Dot4Visible { get; set; }

        /// <summary>
        /// Actual time that the clock shows
        /// </summary>
        public DateTime ActTime { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="clockEngine">Engine that determines active clock indices</param>
        public ClockViewModel(IWordClockEngine clockEngine)
        {
            mClockEngine = clockEngine;

            // Init
            EvaluateTime(this, null);

            // Set and activate the timer. Updates the clock every 5 seconds.
            mTimer = new DispatcherTimer();
            mTimer.Interval = new TimeSpan(0, 0, 5);
            mTimer.Tick += EvaluateTime;
            mTimer.Start();
        }

        /// <summary>
        /// Evaluates the time and fills the ClockCharCollection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvaluateTime(object sender, object e)
        {
            var now = DateTime.Now;
            var dt = now.RoundDown(TimeSpan.FromMinutes(5));
            var remainderMinutes = (now - dt).Minutes;

            if (ActTime == dt && mLastRemainderMinutes == remainderMinutes)
                return;

            ActTime = dt;
            mLastRemainderMinutes = remainderMinutes;

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
            Dot1Visible = remainderMinutes >= 1;
            Dot2Visible = remainderMinutes >= 2;
            Dot3Visible = remainderMinutes >= 3;
            Dot4Visible = remainderMinutes >= 4;
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
