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

        /// <summary>
        /// Object that can be used as a lock.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// The time period between steps in milliseconds.
        /// </summary>
        private double timePerIncrement;

        /// <summary>
        /// The current number of steps that have elapsed.
        /// </summary>
        private volatile int progress;

        /// <summary>
        /// The total number of steps that need to be performed.
        /// </summary>
        private volatile int steps;

        /// <summary>
        /// Ifg the task should be cancelled.
        /// </summary>
        private volatile bool cancelled;

        /// <summary>
        /// A queue of messages sent by the task.
        /// </summary>
        private Queue<string> messages;

        /// <summary>
        /// Create a new token. Defaults to 100 steps.
        /// </summary>
        public ThreadingToken()
        {
            lockObject = new object();
            UseThreading = true;
            Steps = 100;
            TimePeriodFormat = TIME_PERIOD.MILLISECONDS;
        }

        /// <summary>
        /// Create a new token.
        /// </summary>
        /// <param name="steps">The number of steps the tasks will perform.</param>
        public ThreadingToken(int steps)
        {
            lockObject = new object();
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
        /// The time periods units.
        /// </summary>
        public string TimePeriodUnit => Timer.TimePeriodUnits(TimePeriodFormat);

        /// <summary>
        /// How many messages have been left by the task.
        /// </summary>
        public int NumMessages
        {
            get
            {
                lock(lockObject)
                {
                    if(messages == null)
                        return 0;
                    else
                        return messages.Count;
                }
                    
            }
        }

        /// <summary>
        /// Reset the token to a state it can be reused.
        /// </summary>
        public void Reset()
        {
            progress = 0;
            cancelled = false;
            timePerIncrement = 0;
            timer?.Reset();
            messages?.Clear();
        }

        /// <summary>
        /// Only resets the progress and leaves reset of token settings.
        /// </summary>
        public void ResetProgress()
        {
            progress = 0;
            timePerIncrement = 0;
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
                double ms = timer.ElapsedMilliseconds;
                timePerIncrement = ms / progress;

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

        /// <summary>
        /// Add a new message.
        /// </summary>
        /// <param name="msg">The message.</param>
        public void EnqueueMessage(string msg)
        {
            lock(lockObject)
            {
                if(messages == null)
                    messages = new Queue<string>();

                messages.Enqueue(msg);
            }
        }

        /// <summary>
        /// Get and remove the next message.
        /// </summary>
        /// <returns>The next message.</returns>
        public string DequeueMessage()
        {
            lock (lockObject)
            {
                if (messages == null || messages.Count == 0)
                    return "";

                return messages.Dequeue();
            }
        }

        /// <summary>
        /// Clear the queue of all messages.
        /// </summary>
        public void ClearMessages()
        {
            lock (lockObject)
            {
                if (messages != null)
                    messages.Clear();
            }
        }

    }
}
