using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.Core.Time
{

    public enum TIME_PERIOD
    {
        MILLISECONDS,
        SECONDS,
        MINUTES,
        HOURS,
        DAYS
    }

    /// <summary>
    /// Helper timer class wrapping the Stopwatch class.
    /// </summary>
    public class Timer : IDisposable
    {

        private Stopwatch m_watch;

        /// <summary>
        /// A static instance of the timer.
        /// </summary>
        private static Timer s_timer;

        /// <summary>
        /// 
        /// </summary>
        public Timer()
        {
            m_watch = new Stopwatch();
        }

        public double ElapsedMilliseconds => m_watch.ElapsedMilliseconds;

        public double ElapsedSeconds => ElapsedMilliseconds / 1000.0;

        public double ElapsedMinutes => ElapsedMilliseconds / 60.0;

        public double ElapsedHours => ElapsedMinutes / 60.0;

        public double ElapsedDays => ElapsedHours / 24.0;

        public long ElapsedTicks => m_watch.ElapsedTicks;

        public bool IsHighPerformance => Stopwatch.IsHighResolution;

        public bool IsRunning => m_watch.IsRunning;

        public long NanoSecondsPerTick => (1000L * 1000L * 1000L) / Stopwatch.Frequency;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Timer: IsHighPerformance={0}, NanoSecondsPerTick={1}, ElapsedTicks={2}, ElapsedSeconds={3}]",
                IsHighPerformance, NanoSecondsPerTick, ElapsedTicks, ElapsedSeconds);
        }

        /// <summary>
        /// Get the ellapsed time that has passed since timer was called.
        /// </summary>
        /// <param name="period">The time period.</param>
        /// <returns>Get the ellapsed time that has passed since timer was called.</returns>
        public double ElapsedTime(TIME_PERIOD period)
        {
            switch (period)
            {
                case TIME_PERIOD.MILLISECONDS:
                    return ElapsedMilliseconds;

                case TIME_PERIOD.SECONDS:
                    return ElapsedSeconds;

                case TIME_PERIOD.MINUTES:
                    return ElapsedMinutes;

                case TIME_PERIOD.HOURS:
                    return ElapsedHours;

                case TIME_PERIOD.DAYS:
                    return ElapsedDays;
            }

            return 0;
        }

        /// <summary>
        /// Start the timer.
        /// </summary>
        public void Start()
        {
            m_watch.Start();
        }

        /// <summary>
        /// Stop the timer.
        /// </summary>
        /// <returns>The number of milliseconds that have passsed.</returns>
        public double Stop(TIME_PERIOD period = TIME_PERIOD.MILLISECONDS)
        {
            m_watch.Stop();
            return ElapsedTime(period); 
        }

        /// <summary>
        /// Reset the timer.
        /// </summary>
        public void Reset()
        {
            m_watch.Reset();
        }

        /// <summary>
        /// Start the static instance timer.
        /// </summary>
        public static void StartTimer()
        {
            s_timer = new Timer();
            s_timer.Start();
        }

        /// <summary>
        /// Stop the static instance timer.
        /// </summary>
        /// <returns>The number of milliseconds that have passsed.</returns>
        public static double StopTimer(TIME_PERIOD period = TIME_PERIOD.MILLISECONDS)
        {
            return s_timer.Stop(period);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_watch.Stop();
        }
    }
}
