using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Random
{
    public interface IRandomGenerator
    {
        /// <summary>
        /// A random double between 0 and 1.
        /// </summary>
        double Value { get; }

        /// <summary>
        /// The seed for the random generator.
        /// </summary>
        int Seed { get; }

        /// <summary>
        /// Update seed.
        /// Called when seed changes.
        /// </summary>
        void UpdateSeed(int seed);

        /// <summary>
        /// A random int between 0 - MaxInt.
        /// </summary>
        int Next();

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        int Next(int max);

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        int Next(int min, int max);

        /// <summary>
        /// Returns a random double that is within a specified range.
        /// </summary>
        double NextDouble(double min, double max);

        /// <summary>
        /// A random double between 0 - 1.
        /// </summary>
        double NextDouble();
    }
}
