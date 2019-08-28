using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Curves
{

    /// <summary>
    /// Cubic polynomial ax^3 + b*x^2 + c*x + d = 0
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Polynomial3f
    {

        internal float a;

        internal float b;

        internal float c;

        internal float d;

        internal Polynomial3f(float a, float b, float c, float d)
        {
            if (a == 0.0)
                throw new InvalidOperationException("First term in polynomial can not be 0.");

            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        internal float Solve(float x)
        {
            return a * x * x * x + b * x * x + c * x + d;
        }

        internal PolynomialRoots3f Solve()
        {
            return Solve(a, b, c, d);
        }

        internal static PolynomialRoots3f Solve(float a, float b, float c, float d)
        {
            if (a == 0.0)
                throw new InvalidOperationException("First term in polynomial can not be 0.");

            PolynomialRoots3f roots = new PolynomialRoots3f();

            float B = b / a;
            float C = c / a;
            float D = d / a;

            float b2 = B * B;
            float q = (b2 - 3 * C) / 9.0f;
            float r = (B * (2.0f * b2 - 9.0f * C) + 27 * D) / 54.0f;

            // equation x^3 + q*x + r = 0
            float r2 = r * r;
            float q3 = q * q * q;
         
            if (r2 < q3)
            {
                float t = r / FMath.Sqrt(q3);
                if (t < -1.0) t = -1.0f;
                if (t > 1.0) t = 1.0f;
                t = FMath.Acos(t);
                B /= 3.0f;
                q = -2.0f * FMath.Sqrt(q);
                roots.real = 3;
                roots.x0 = q * FMath.Cos(t / 3.0f) - B;
                roots.x1 = q * FMath.Cos((t + FMath.PI * 2.0f) / 3.0f) - B;
                roots.x2 = q * FMath.Cos((t - FMath.PI * 2.0f) / 3.0f) - B;
            }
            else
            {
                float s = -CubeRoot(Math.Abs(r) + FMath.Sqrt(r2 - q3));
                if (r < 0) s = -s;
                float t = s == 0 ? 0 : t = q / s;

                B /= 3.0f;
                roots.real = 1;
                roots.x0 = (s + t) - B;
            }

            return roots;
        }

        private static float CubeRoot(float x)
        {
            if (x > 0)
                return Root3(x);
            else if (x < 0)
                return -Root3(-x);
            else
                return 0.0f;
        }

        private static float Root3(float x)
        {
            float s = 1.0f;
            while (x < 1.0f)
            {
                x *= 8.0f;
                s *= 0.50f;
            }
            while (x > 8.0f)
            {
                x *= 0.125f;
                s *= 2.0f;
            }

            float r = 1.5f;
            float third = 1.0f / 3.0f;
            r -= third * (r - x / (r * r));
            r -= third * (r - x / (r * r));
            r -= third * (r - x / (r * r));
            r -= third * (r - x / (r * r));
            r -= third * (r - x / (r * r));
            r -= third * (r - x / (r * r));
            return r * s;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PolynomialRoots3f
    {
        internal int real;
        internal float x0, x1, x2;

        internal float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x0;
                    case 1: return x1;
                    case 2: return x2;
                    default: throw new IndexOutOfRangeException("Index out of range: " + i);
                }
            }
        }
    }
}
