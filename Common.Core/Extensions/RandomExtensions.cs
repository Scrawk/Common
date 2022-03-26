using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Colors;

namespace System
{
    public static class RandomExtensions
    {

        public static double NextDouble(this Random rnd, double min, double max)
        {
            return min + rnd.NextDouble() * (max - min);
        }

        public static float NextFloat(this Random rnd, float min, float max)
        {
            return min + (float)rnd.NextDouble() * (max-min);
        }

        public static float NextFloat(this Random rnd)
        {
            return (float)rnd.NextDouble();
        }

        public static bool NextBool(this Random rnd)
        {
            return rnd.NextDouble() > 0.5;
        }

        public static Vector2f NextVector2f(this Random rnd, Vector2f min, Vector2f max)
        {
            float x = rnd.NextFloat(min.x, max.x);
            float y = rnd.NextFloat(min.y, max.y);
            return new Vector2f(x, y);
        }

        public static Vector2f NextVector2f(this Random rnd, float min = 0, float max = 1)
        {
            float x = rnd.NextFloat(min, max);
            float y = rnd.NextFloat(min, max);
            return new Vector2f(x, y);
        }

        public static Vector2d NextVector2d(this Random rnd, Vector2d min, Vector2d max)
        {
            double x = rnd.NextDouble(min.x, max.x);
            double y = rnd.NextDouble(min.y, max.y);
            return new Vector2d(x, y);
        }

        public static Vector2d NextVector2d(this Random rnd, double min = 0, double max = 1)
        {
            double x = rnd.NextDouble(min, max);
            double y = rnd.NextDouble(min, max);
            return new Vector2d(x, y);
        }

        public static Vector3f NextVector3f(this Random rnd, Vector3f min, Vector3f max)
        {
            float x = rnd.NextFloat(min.x, max.x);
            float y = rnd.NextFloat(min.y, max.y);
            float z = rnd.NextFloat(min.z, max.z);
            return new Vector3f(x, y, z);
        }

        public static Vector3f NextVector3f(this Random rnd, float min = 0, float max = 1)
        {
            float x = rnd.NextFloat(min, max);
            float y = rnd.NextFloat(min, max);
            float z = rnd.NextFloat(min, max);
            return new Vector3f(x, y, z);
        }

        public static Vector3d NextVector3d(this Random rnd, Vector3d min, Vector3d max)
        {
            double x = rnd.NextDouble(min.x, max.x);
            double y = rnd.NextDouble(min.y, max.y);
            double z = rnd.NextDouble(min.y, max.y);
            return new Vector3d(x, y, z);
        }

        public static Vector3d NextVector3d(this Random rnd, double min = 0, double max = 1)
        {
            double x = rnd.NextDouble(min, max);
            double y = rnd.NextDouble(min, max);
            double z = rnd.NextDouble(min, max);
            return new Vector3d(x, y, z);
        }

        public static ColorRGB NextColorRGB(this Random rnd)
        {
            float r = rnd.NextFloat();
            float g = rnd.NextFloat();
            float b = rnd.NextFloat();

            return new ColorRGB(r, g, b);
        }

        public static ColorRGBA NextColorRGBA(this Random rnd)
        {
            float r = rnd.NextFloat();
            float g = rnd.NextFloat();
            float b = rnd.NextFloat();

            return new ColorRGBA(r, g, b, 1);
        }

        public static ColorHSV NextColorHSV(this Random rnd)
        {
            float h = rnd.NextFloat();
            float s = rnd.NextFloat();
            float v = rnd.NextFloat();

            return new ColorHSV(h, s, v);
        }


    }
}
