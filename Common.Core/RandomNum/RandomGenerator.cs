using System;

using Common.Core.Numerics;
using Common.Core.Shapes;
using Common.Core.Colors;

namespace Common.Core.RandomNum
{
    /// <summary>
    /// A base class for generating a random number.
    /// </summary>
    public abstract class RandomGenerator
    {
        private const double INV_MAX = 1.0 / int.MaxValue;

        /// <summary>
        /// Create a random number with a randow seed.
        /// </summary>
        public RandomGenerator()
        {
            var seed = Guid.NewGuid().GetHashCode();
            UpdateSeed(seed);
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
        /// A random double greater than or equal to 0 and less than 1.
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
        /// A random int greater than or equal to 0 and less than MaxInt.
        /// </summary>
        public abstract int Next();

        /// <summary>
        /// A random int greater than or equal to 0 and less than max.
        /// </summary>
        public int Next(int max)
        {
            return Next() % max;
        }

        /// <summary>
        /// A random int greater than or equal to min and less than max.
        /// </summary>
        public int Next(int min, int max)
        {
            return min + Next() % (max - min);
        }

        /// <summary>
        /// A random double greater than or equal to min and less than max.
        /// </summary>
        public double NextDouble(double min, double max)
        {
            return min + Value * (max - min);
        }

        /// <summary>
        /// A random double greater than or equal to 0 and less than 1.
        /// </summary>
        public double NextDouble()
        {
            return Value;
        }

        public float NextFloat(float min, float max)
        {
            return min + (float)NextDouble() * (max - min);
        }

        public float NextFloat()
        {
            return (float)NextDouble();
        }

        public bool NextBool()
        {
            return NextDouble() > 0.5;
        }

        public Vector2f NextVector2f(float min, float max)
        {
            float x = NextFloat(min, max);
            float y = NextFloat(min, max);
            return new Vector2f(x, y);
        }

        public Vector2d NextVector2d(double min, double max)
        {
            double x = NextDouble(min, max);
            double y = NextDouble(min, max);
            return new Vector2d(x, y);
        }

        public Vector3f NextVector3f(float min, float max)
        {
            float x = NextFloat(min, max);
            float y = NextFloat(min, max);
            float z = NextFloat(min, max);
            return new Vector3f(x, y, z);
        }

        public Vector3d NextVector3d(double min, double max)
        {
            double x = NextDouble(min, max);
            double y = NextDouble(min, max);
            double z = NextDouble(min, max);
            return new Vector3d(x, y, z);
        }

        public Point2f NextPoint2f(float min, float max)
        {
            float x = NextFloat(min, max);
            float y = NextFloat(min, max);
            return new Point2f(x, y);
        }

        public Point2f NextPoint2f(IShape2f shape)
        {
            var p = new Point2f();
            var bounds = shape.Bounds;

            int count = 0;

            do
            {
                p.x = NextFloat(bounds.Min.x, bounds.Max.x);
                p.y = NextFloat(bounds.Min.y, bounds.Max.y);

                if (count++ > 1000)
                    throw new Exception("Could not find a point within shape bounds.");
            }
            while (!shape.Contains(p, true));

            return p;
        }

        public Point2d NextPoint2d(double min, double max)
        {
            double x = NextDouble(min, max);
            double y = NextDouble(min, max);
            return new Point2d(x, y);
        }

        public Point2d NextPoint2d(IShape2d shape)
        {
            var p = new Point2d();
            var bounds = shape.Bounds;

            int count = 0;

            do
            {
                p.x = NextDouble(bounds.Min.x, bounds.Max.x);
                p.y = NextDouble(bounds.Min.y, bounds.Max.y);

                if (count++ > 1000)
                    throw new Exception("Could not find a point within shape bounds.");
            }
            while (!shape.Contains(p, true));

            return p;
        }

        public Point3f NextPoint3f(float min, float max)
        {
            float x = NextFloat(min, max);
            float y = NextFloat(min, max);
            float z = NextFloat(min, max);
            return new Point3f(x, y, z);
        }

        public Point3f NextPoint3f(IShape3f shape)
        {
            var p = new Point3f();
            var bounds = shape.Bounds;

            int count = 0;

            do
            {
                p.x = NextFloat(bounds.Min.x, bounds.Max.x);
                p.y = NextFloat(bounds.Min.y, bounds.Max.y);
                p.z = NextFloat(bounds.Min.z, bounds.Max.z);

                if (count++ > 1000)
                    throw new Exception("Could not find a point within shape bounds.");
            }
            while (!shape.Contains(p, true));

            return p;
        }

        public Point3d NextPoint3d(double min, double max)
        {
            double x = NextDouble(min, max);
            double y = NextDouble(min, max);
            double z = NextDouble(min, max);
            return new Point3d(x, y, z);
        }

        public Point3d NextPoint3d(IShape3d shape)
        {
            var p = new Point3d();
            var bounds = shape.Bounds;

            int count = 0;

            do
            {
                p.x = NextDouble(bounds.Min.x, bounds.Max.x);
                p.y = NextDouble(bounds.Min.y, bounds.Max.y);
                p.z = NextDouble(bounds.Min.z, bounds.Max.z);

                if (count++ > 1000)
                    throw new Exception("Could not find a point within shape bounds.");
            }
            while (!shape.Contains(p, true));

            return p;
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


