using System;

namespace Common.Core.Random
{
    /// <summary>
    /// A base class for generating a random number.
    /// </summary>
    public abstract class RandomGenerator : IRandomGenerator
    {
        private const double INV_MAX = 1.0 / int.MaxValue;

        /// <summary>
        /// Create a random number with a randow seed.
        /// </summary>
        public RandomGenerator()
        {
            var rnd = new System.Random();
            UpdateSeed(rnd.Next());
        }

        /// <summary>
        /// Create a random number.
        /// </summary>
        /// <param name="seed">The seed for the generator.</param>
        public RandomGenerator(int seed)
        {
            UpdateSeed(seed);
        }

        /// <summary>
        /// A random double between 0 and 1.
        /// </summary>
        public double Value
        {
            get { return Next() * INV_MAX; }
        }

        /// <summary>
        /// The seed for the random generator.
        /// </summary>
        public int Seed { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[RandomGenerator: Seed={0}]", Seed);
        }

        /// <summary>
        /// Update seed.
        /// Called when seed changes.
        /// </summary>
        public abstract void UpdateSeed(int seed);

        /// <summary>
        /// A random int between 0 - MaxInt.
        /// </summary>
        public abstract int Next();

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        public int Next(int max)
        {
            return Next() % max;
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        public int Next(int min, int max)
        {
            return min + Next() % (max - min);
        }

        /// <summary>
        /// Returns a random double that is within a specified range.
        /// </summary>
        public double NextDouble(double min, double max)
        {
            return min + Value * (max - min);
        }

        /// <summary>
        /// A random double between 0 - 1.
        /// </summary>
        public double NextDouble()
        {
            return Value;
        }

    }

}


