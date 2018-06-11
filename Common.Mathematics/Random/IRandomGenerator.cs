
using System.Collections;

namespace Common.Mathematics.Random
{
    /// <summary>
    /// A interface for generating a random number.
    /// </summary>
    public interface IRandomGenerator
    {
        /// <summary>
        /// Random double between 0 and 1 (inclusive).
        /// </summary>
        double Value { get; }

        /// <summary>
        /// The seed for the random generator.
        /// </summary>
        int Seed { get; }

        /// <summary>
        /// A random int between min and max (inclusive).
        /// </summary>
        int Range(int min, int max);

        /// <summary>
        /// A random double between min and max (inclusive).
        /// </summary>
        double Range(double min, double max);

        /// <summary>
        /// A random float between min and max (inclusive).
        /// </summary>
        float Range(float min, float max);

        /// <summary>
        /// Update the seed.
        /// </summary>
        void UpdateSeed(int seed);
    }

}
