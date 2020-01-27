// <copyright file="TimePoint.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace DhcpServer
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents an instant in time.
    /// </summary>
    public readonly struct TimePoint
    {
        private static readonly double TicksPerTimerTick = 10000000.0 / Stopwatch.Frequency;

        private readonly long timerTicks;

        private TimePoint(long timerTicks)
        {
            this.timerTicks = timerTicks;
        }

        /// <summary>
        /// Finds the difference between two time points.
        /// </summary>
        /// <param name="x">The minuend.</param>
        /// <param name="y">The subtrahend.</param>
        /// <returns>The difference as a <see cref="TimeSpan"/>.</returns>
        public static TimeSpan operator -(TimePoint x, TimePoint y)
        {
            long deltaTimerTicks = x.timerTicks - y.timerTicks;
            double deltaTicks = TicksPerTimerTick * deltaTimerTicks;
            return new TimeSpan((long)deltaTicks);
        }

        /// <summary>
        /// Creates a time point at the current time.
        /// </summary>
        /// <returns>A new time point.</returns>
        public static TimePoint Now() => new TimePoint(Stopwatch.GetTimestamp());

        /// <summary>
        /// Gets the time elapsed since this time point.
        /// </summary>
        /// <returns>The elapsed time.</returns>
        public TimeSpan Elapsed() => Now() - this;
    }
}
