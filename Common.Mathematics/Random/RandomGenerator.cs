
using System;

namespace Common.Mathematics.Random
{
    /// <summary>
    /// A base class for generating a random number.
    /// </summary>
    public abstract class RandomGenerator : IRandomGenerator
    {
        /// <summary>
        /// A random double between 0 and 1 (inclusive).
        /// </summary>
        public abstract double Value { get; }

        /// <summary>
        /// The seed for the random generator.
        /// </summary>
        public int Seed { get; protected set; }

        /// <summary>
        /// Create a random number.
        /// </summary>
        /// <param name="seed">The seed for the generator.</param>
        public RandomGenerator(int seed)
        {
            UpdateSeed(seed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[RandomGenerator: Seed={0}]", Seed);
        }

        /// <summary>
        /// A random int between min and max (inclusive).
        /// </summary>
        public virtual int Range(int min, int max)
        {
            return (int)(Value * (max - min) + min);
        }

        /// <summary>
        /// A random double between min and max (inclusive).
        /// </summary>
        public virtual double Range(double min, double max)
        {
            return Value * (max - min) + min;
        }

        /// <summary>
        /// A random float between min and max (inclusive).
        /// </summary>
        public virtual float Range(float min, float max)
        {
            return (float)Value * (max - min) + min;
        }

        /// <summary>
        /// Update the seed.
        /// </summary>
        public abstract void UpdateSeed(int seed);
    }

}


