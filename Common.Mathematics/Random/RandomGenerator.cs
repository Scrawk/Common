using System;
using Common.Core.Numerics;
using Common.Core.Colors;

namespace Common.Mathematics.Random
{
    /// <summary>
    /// A base class for generating a random number.
    /// </summary>
    public abstract class RandomGenerator
    {
        private const double INV_MAX = 1.0 / int.MaxValue;

        /// <summary>
        /// Create a random number.
        /// </summary>
        /// <param name="seed">The seed for the generator.</param>
        public RandomGenerator(uint seed)
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
        public uint Seed { get; protected set; }

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
        public abstract void UpdateSeed(uint seed);

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

        public double NextDouble(double min, double max)
        {
            return min + Value * (max - min);
        }

        public double NextDouble()
        {
            return Value;
        }

        public float NextFloat(float min, float max)
        {
            return min + (float)Value * (max - min);
        }

        public float NextFloat()
        {
            return (float)Value;
        }

        public Vector2f NextVector2f(Vector2f min, Vector2f max)
        {
            float x = NextFloat(min.x, max.x);
            float y = NextFloat(min.y, max.y);
            return new Vector2f(x, y);
        }

        public Vector2f NextVector2f(float min = 0, float max = 1)
        {
            float x = NextFloat(min, max);
            float y = NextFloat(min, max);
            return new Vector2f(x, y);
        }

        public Vector2d NextVector2d(Vector2d min, Vector2d max)
        {
            double x = NextDouble(min.x, max.x);
            double y = NextDouble(min.y, max.y);
            return new Vector2d(x, y);
        }

        public Vector2d NextVector2d(double min = 0, double max = 1)
        {
            double x = NextDouble(min, max);
            double y = NextDouble(min, max);
            return new Vector2d(x, y);
        }

        public Vector3f NextVector3f(Vector3f min, Vector3f max)
        {
            float x = NextFloat(min.x, max.x);
            float y = NextFloat(min.y, max.y);
            float z = NextFloat(min.z, max.z);
            return new Vector3f(x, y, z);
        }

        public Vector3f NextVector3f(float min = 0, float max = 1)
        {
            float x = NextFloat(min, max);
            float y = NextFloat(min, max);
            float z = NextFloat(min, max);
            return new Vector3f(x, y, z);
        }

        public Vector3d NextVector3d(Vector3d min, Vector3d max)
        {
            double x = NextDouble(min.x, max.x);
            double y = NextDouble(min.y, max.y);
            double z = NextDouble(min.y, max.y);
            return new Vector3d(x, y, z);
        }

        public Vector3d NextVector3d(double min = 0, double max = 1)
        {
            double x = NextDouble(min, max);
            double y = NextDouble(min, max);
            double z = NextDouble(min, max);
            return new Vector3d(x, y, z);
        }

        public ColorRGB NextColorRGB()
        {
            float r = NextFloat();
            float g = NextFloat();
            float b = NextFloat();

            return new ColorRGB(r, g, b);
        }

        public ColorRGBA NextColorRGBA()
        {
            float r = NextFloat();
            float g = NextFloat();
            float b = NextFloat();

            return new ColorRGBA(r, g, b, 1);
        }

        public ColorHSV NextColorHSV()
        {
            float h = NextFloat();
            float s = NextFloat();
            float v = NextFloat();

            return new ColorHSV(h, s, v);
        }


    }

}


