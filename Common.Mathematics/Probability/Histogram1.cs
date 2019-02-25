using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// A 1D histogram.
    /// </summary>
    public class Histogram1
    {
        /// <summary>
        /// The table containing the counts of each element added.
        /// </summary>
        private int[] m_table;

        public Histogram1(double min, double max, double step)
        {
            Min = min;
            Max = max;
            Step = step;
            Size = (int)Math.Floor((max - min) / step) + 1;

            m_table = new int[Size];
        }

        /// <summary>
        /// The minimum value that will be added.
        /// </summary>
        public double Min { get; private set; }

        /// <summary>
        /// The maximum value that will be added.
        /// </summary>
        public double Max { get; private set; }

        /// <summary>
        /// The distance between the values that 
        /// each index in the table represents.
        /// </summary>
        public double Step { get; private set; }

        /// <summary>
        /// The size of the table.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The total number of values that have been added.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The percentage of values added at this index.
        /// </summary>
        public double this[int i]
        {
            get
            {
                if (Count == 0) return 0;
                return m_table[i] / (Count * Step);
            }
        }

        public override string ToString()
        {
            return string.Format("[Histogram1: Min={0}, Max={1}, Step={2}, Size={3}, Count={4}]", 
                Min, Max, Step, Size, Count);
        }

        /// <summary>
        /// Clear the histogram.
        /// </summary>
        public void Clear()
        {
            Array.Clear(m_table, 0, Size);
            Count = 0;
        }

        /// <summary>
        /// Add a value to the histogram.
        /// </summary>
        /// <param name="v">A value between min and max (inculsive)</param>
        /// <returns>If the value was added.</returns>
        public bool Add(double v)
        {
            int i = (int)Math.Floor((v - Min) / Step);

            if (i < 0 || i >= Size) return false;

            m_table[i]++;
            Count++;
            return true;
        }

    }
}
