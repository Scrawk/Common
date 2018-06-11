using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.Core.Time
{
    public class Timer
    {

        public float ElapsedMilliseconds { get; private set; }

        public float ElapsedSeconds { get; private set; }

        public long ElapsedTicks {  get { return m_watch.ElapsedTicks; } }

        public bool IsHighPerformance {  get { return Stopwatch.IsHighResolution;  } }

        public long NanoSecondsPerTick { get { return (1000L * 1000L * 1000L) / Stopwatch.Frequency; } }

        Stopwatch m_watch;

        public Timer()
        {
            m_watch = new Stopwatch();
        }

        public void Start()
        {
            m_watch.Start();
        }

        public float Stop()
        {
            m_watch.Stop();

            ElapsedMilliseconds = (ElapsedTicks * NanoSecondsPerTick) / 1000000.0f;
            ElapsedSeconds = ElapsedMilliseconds / 1000.0f;

            return ElapsedMilliseconds;
        }

        public void Reset()
        {
            ElapsedMilliseconds = 0.0f;
            ElapsedSeconds = 0.0f;
            m_watch.Reset();
        }

    }
}
