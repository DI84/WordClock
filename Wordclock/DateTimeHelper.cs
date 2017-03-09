using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wordclock
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Rounds the time up to the next full increment
        /// </summary>
        /// <param name="dt">Time to round</param>
        /// <param name="d">Rounding increment in minutes</param>
        /// <returns></returns>
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            var delta = (d.Ticks - (dt.Ticks % d.Ticks)) % d.Ticks;
            return new DateTime(dt.Ticks + delta);
        }

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

        /// <summary>
        /// Rounds the time to the nearest full increment
        /// </summary>
        /// <param name="dt">Time to round</param>
        /// <param name="d">Rounding increment in minutes</param>
        /// <returns></returns>
        public static DateTime RoundToNearest(this DateTime dt, TimeSpan d)
        {
            var delta = dt.Ticks % d.Ticks;
            bool roundUp = delta > d.Ticks / 2;

            return roundUp ? RoundUp(dt, d) : RoundDown(dt, d);
        }
    }
}
