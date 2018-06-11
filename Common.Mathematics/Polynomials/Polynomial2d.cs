using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Common.Mathematics.Polynomials
{

    /// <summary>
    /// Quadratic polynomial ax^2 + b*x + c
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Polynomial2d
    {

        public double a;

        public double b;

        public double c;

        public Polynomial2d(double a, double b, double c)
        {
            if (a == 0.0)
                throw new InvalidOperationException("First term in polynomial can not be 0.");

            this.a = a;
            this.b = b;
            this.c = c;
        }

        public double Solve(double x)
        {
            return a * x * x + b * x + c;
        }

        public PolynomialRoots2d Solve()
        {
            return Solve(a, b, c);
        }

        public static PolynomialRoots2d Solve(double a, double b, double c)
        {
            if (a == 0.0)
                throw new InvalidOperationException("First term in polynomial can not be 0.");

            PolynomialRoots2d roots = new PolynomialRoots2d();

            //discriminant
            double d = b * b - 4 * a * c;

            if (d < 0)
            {
                roots.real = 0;
            }
            else if (d == 0)
            {
                roots.real = 1;
                roots.x0 = -b / (2.0 * a);
            }
            else
            {
                if (b == 0)
                {
                    roots.real = 2;
                    roots.x0 = Math.Sqrt(Math.Abs(c) / Math.Abs(a));
                    roots.x1 = -roots.x0;
                }
                else
                {
                    double q = -0.5 * (b + Sign(b) * Math.Sqrt(d));

                    roots.real = 2;
                    roots.x0 = q / a;
                    roots.x1 = c / q;
                }
            }

            if (roots.x1 == 0)
                roots.real = 1;

            return roots;
        }

        private static double Sign(double b)
        {
            if (b > 0) return 1;
            if(b < 0) return -1;
            return 0;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PolynomialRoots2d
    {
        public int real;
        public double x0, x1;

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x0;
                    case 1: return x1;
                    default: throw new IndexOutOfRangeException("Index out of range: " + i);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[PolynomialRoots2d: real={0}, x0={1}, x1={2}", real, x0, x1);
        }
    }
}
