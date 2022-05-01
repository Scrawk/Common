using System;
using System.Collections.Generic;

using Common.Core.Time;

namespace Common.Core.Threading
{
    /// <summary>
    /// A token that can be passed to threaded tasks as a helper
    /// for cancelling the task and keeping track of the tasks progress.
    /// </summary>
    public class ThreadingToken
    {
        /// <summary>
        /// A timer that can be used to 
        /// determine the tasks estimate completion time.
        /// </summary>
        private Timer timer;

        private volatile int progress;

        private volatile int steps;

        private volatile bool cancelled;

        private double timePerIncrement;

        /// <summary>
        /// 
        /// </summary>
        public ThreadingToken()
        {
            UseThreading = true;
            TimePeriodFormat = TIME_PERIOD.MILLISECONDS;
        }

        /// <summary>
        /// Create a new token.
        /// </summary>
        /// <param name="steps">The number of steps the tasks will perform.</param>
        public ThreadingToken(int steps)
        {
            UseThreading = true;
            Steps = steps;
            TimePeriodFormat = TIME_PERIOD.MILLISECONDS;
        }

        /// <summary>
        /// THe number of steps the task will perform to be complete.
        /// </summary>
        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        /// <summary>
        /// Tell the task it should be cancelled.
        /// </summary>
        public bool Cancelled
        {
            get { return cancelled; }
            set { cancelled = value; }
        }

        /// <summary>
        /// Has the task completed all its steps.
        /// </summary>
        public bool IsComplete => progress >= steps;

        /// <summary>
        /// Should threading be used. 
        /// Need to disable for debugging/testing sometimes.
        /// </summary>
        public bool UseThreading { get; set; }

        /// <summary>
        /// What format should the timer use.
        /// </summary>
        public TIME_PERIOD TimePeriodFormat {  get; set; }  

        /// <summary>
        /// Reset the token to a state it can be reused.
        /// </summary>
        public void Reset()
        {
            progress = 0;
            cancelled = false;
            timePerIncrement = 0;
            timer?.Reset();
        }

        /// <summary>
        /// The percentage (0-1) of steps the task has completed.
        /// </summary>
        /// <returns>The percentage (0-1) of steps the task has completed.</returns>
        public float PercentageProgress()
        {
            if (steps == 0)
                return 0;
            else
                return progress / (float)steps;
        }

        /// <summary>
        /// Increment the tasks progress.
        /// </summary>
        /// <returns>The estimated completion time if a timer is being used.</returns>
        public double IncrementProgess()
        {
            progress++;

            if (progress > steps)
                progress = steps;

            if(timer != null)
            {
                double t = timer.ElapsedTime(TimePeriodFormat);
                timePerIncrement = t / progress;

                return EstimatedCompletionTime();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the ellapsed time that has passed since timer was called.
        /// </summary>
        /// <returns>Get the ellapsed time that has passed since timer was called.</returns>
        public double ElapsedTime()
        {
            if(timer == null)
                return 0;
            else
                return timer.ElapsedTime(TimePeriodFormat);
        }

        /// <summary>
        /// The estimated completion time if a timer is being used.
        /// </summary>
        /// <returns>The estimated completion time if a timer is being used.</returns>
        public double EstimatedCompletionTime()
        {
            switch(TimePeriodFormat)
            {
                case TIME_PERIOD.MILLISECONDS:
                    return timePerIncrement * steps;

                case TIME_PERIOD.SECONDS:
                    return timePerIncrement / 1000.0 * steps;

                case TIME_PERIOD.MINUTES:
                    return timePerIncrement / (1000.0 * 60.0) * steps;

                case TIME_PERIOD.HOURS:
                    return timePerIncrement / (1000.0 * 60.0 * 60.0) * steps;

                case TIME_PERIOD.DAYS:
                    return timePerIncrement / (1000.0 * 60.0 * 60.0 * 24.0) * steps;
            }

            return 0;
        }

        /// <summary>
        /// Starts the time which will be used to calculate the completion time.
        /// </summary>
        public void StartTimer()
        {
            if(timer == null)
                timer = new Timer();

            timer.Reset();
            timer.Start();
        }

        /// <summary>
        /// Stops the time which will be used to calculate the completion time.
        /// </summary>
        /// <returns>The elapsed time.</returns>
        public double StopTimer()
        {
            if (timer == null)
                return 0;
            else
                return timer.Stop(TimePeriodFormat);
        }

    }
}
