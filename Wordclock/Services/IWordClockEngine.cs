using System;
using System.Collections.Generic;

namespace Wordclock
{
    /// <summary>
    /// Determines which character indices should be active for a given time
    /// </summary>
    public interface IWordClockEngine
    {
        /// <summary>
        /// The characters that make up the clock grid
        /// </summary>
        string[] ClockChars { get; }

        /// <summary>
        /// Returns the set of active character indices for the given time
        /// </summary>
        /// <param name="time">Time rounded to the nearest 5-minute increment</param>
        HashSet<int> GetActiveIndices(DateTime time);
    }
}
