using System;

namespace Wordclock
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Rounds the time down to the next full increment
        /// </summary>
        /// <param name="dt">Time to round</param>
        /// <param name="d">Rounding increment in minutes</param>
        /// <returns></returns>
        public static DateTime RoundDown(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            return new DateTime(dt.Ticks - delta);
        }
    }
}
