using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;
using Common.Core.Colors;

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

        float NextFloat(float min, float max);

        float NextFloat();

        bool NextBool();

        Vector2f NextVector2f(float min, float max);

        Vector2d NextVector2d(double min, double max);

        Vector3f NextVector3f(float min, float max);

        Vector3d NextVector3d(double min, double max);

        Point2f NextPoint2f(float min, float max);

        Point2d NextPoint2d(double min, double max);

        Point3f NextPoint3f(float min, float max);

        Point3d NextPoint3d(double min, double max);

        ColorRGB NextColorRGB();

        ColorRGBA NextColorRGBA();

        ColorHSV NextColorHSV();
    }
}
