using System;
using System.Runtime.CompilerServices;

namespace Common.Core.Mathematics
{
    public class FMath
    {
        public static readonly float EPS = 1e-9f;

        public static readonly float PI = (float)Math.PI;

        public static readonly float Rad2Deg = 180.0f / PI;

        public static readonly float Deg2Rad = PI / 180.0f;

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
        public static float SafeInvSqrt(float n, float d)
        {
            if (d <= 0.0f) return 0.0f;
            d = (float)Math.Sqrt(d);
            if (d < EPS) return 0.0f;
            return n / d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeInv(float v)
        {
            if (Math.Abs(v) < EPS) return 0.0f;
            return 1.0f / v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SafeDiv(float n, float d)
        {
            if (Math.Abs(d) < EPS) return 0.0f;
            return n / d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(float v)
        {
            return Math.Abs(v) < EPS;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(float f)
        {
            return !(float.IsInfinity(f) || float.IsNaN(f));
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

    }
}




















