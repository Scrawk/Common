using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.RandomNum
{
    public interface IRandomGenerator
    {
        /// <summary>
        /// A random double greater than or equal to 0 and less than 1.
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
        /// A random int greater than or equal to 0 and less than MaxInt.
        /// </summary>
        int Next();

        /// <summary>
        /// A random int greater than or equal to 0 and less than max.
        /// </summary>
        int Next(int max);

        /// <summary>
        /// A random int greater than or equal to min and less than max.
        /// </summary>
        int Next(int min, int max);

        /// <summary>
        /// A random double greater than or equal to min and less than max.
        /// </summary>
        double NextDouble(double min, double max);

        /// <summary>
        /// A random double greater than or equal to 0 and less than 1.
        /// </summary>
        double NextDouble();
    }
}
