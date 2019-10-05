using System;
using System.Runtime.CompilerServices;

namespace Common.Core.Numerics
{
    public class FMath
    {
        public const float EPS = 1e-9f;

        public const float PI = (float)Math.PI;

        public const float SQRT2 = 1.414213562373095f;

        public const float Rad2Deg = 180.0f / PI;

        public const float Deg2Rad = PI / 180.0f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Asin(float a)
        {
            return (float)Math.Asin(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float a)
        {
            return (float)Math.Cos(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Acos(float a)
        {
            return (float)Math.Acos(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Tan(float a)
        {
            return (float)Math.Tan(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan(float a)
        {
            return (float)Math.Atan(a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Atan2(float x, float y)
        {
            return (float)Math.Atan2(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqrt(float d)
        {
            return (float)Math.Sqrt(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Exp(float d)
        {
            return (float)Math.Exp(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Log(float d)
        {
            return (float)Math.Log(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Floor(float d)
        {
            return (float)Math.Floor(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceilling(float d)
        {
            return (float)Math.Ceiling(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float d, int n)
        {
            return (float)Math.Round(d, n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeAcos(float r)
        {
            return (float)Math.Acos(Math.Min(1.0f, Math.Max(-1.0f, r)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeAsin(float r)
        {
            return (float)Math.Asin(Math.Min(1.0f, Math.Max(-1.0f, r)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeSqrt(float v)
        {
            if (v <= 0.0f) return 0.0f;
            return (float)Math.Sqrt(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeLog(float v)
        {
            if (v <= 0.0f) return 0.0f;
            return (float)Math.Log(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeLog10(float v)
        {
            if (v <= 0.0f) return 0.0f;
            return (float)Math.Log10(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeInvSqrt(float n, float d, float eps = EPS)
        {
            if (d <= 0.0f) return 0.0f;
            d = (float)Math.Sqrt(d);
            if (d < eps) return 0.0f;
            return n / d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeInv(float v, float eps = EPS)
        {
            if (Math.Abs(v) < eps) return 0.0f;
            return 1.0f / v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeDiv(float n, float d, float eps = EPS)
        {
            if (Math.Abs(d) < eps) return 0.0f;
            return n / d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(float v, float eps = EPS)
        {
            return Math.Abs(v) < eps;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(float f)
        {
            return !(float.IsInfinity(f) || float.IsNaN(f));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sqr(float v)
        {
            return v * v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float v, float min, float max)
        {
            if (v < min) v = min;
            if (v > max) v = max;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp01(float v)
        {
            if (v < 0.0f) v = 0.0f;
            if (v > 1.0f) v = 1.0f;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SmoothStep(float edge0, float edge1, float x)
        {
            float t = Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            return t * t * (3.0f - 2.0f * t);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Frac(float x)
        {
            return x - (float)Math.Floor(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float v0, float v1, float a)
        {
            return v0 * (1.0f - a) + v1 * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SignOrZero(float v)
        {
            if (v == 0) return 0;
            return Math.Sign(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float a, float b, float c)
        {
            return Math.Min(a, Math.Min(b, c));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float a, float b, float c)
        {
            return Math.Max(a, Math.Max(b, c));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Normalize(float a, float min, float max)
        {
            float len = max - min;
            if (len <= 0) return 0;
            return (a - min) / len;
        }

    }
}




















