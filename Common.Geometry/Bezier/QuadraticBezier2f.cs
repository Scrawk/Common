using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using POINT2 = Common.Core.Numerics.Point2f;

namespace Common.Geometry.Bezier
{
    /// <summary>
    /// A bezier curve of quadratic degree using a polynominal.
    /// </summary>
    public class QuadraticBezier2f
    {

        /// <summary>
        /// The control points.
        /// </summary>
        public POINT2 C0 { get; set; }
        public POINT2 C1 { get; set; }
        public POINT2 C2 { get; set; }

        public QuadraticBezier2f()
        {

        }

        public QuadraticBezier2f(POINT2 c0, POINT2 c1, POINT2 c2)
        {
            C0 = c0;
            C1 = c1;
            C2 = c2;
        }

        /// <summary>
        /// The length of the curve.
        /// </summary>
        public REAL Length
        {
            get
            {
                REAL ax = C0.x - 2.0f * C1.x + C2.x;
                REAL az = C0.y - 2.0f * C1.y + C2.y;
                REAL bx = 2.0f * (C1.x - C0.x);
                REAL bz = 2.0f * (C1.y - C0.y);

                REAL A = 4.0f * (ax * ax + az * az);
                REAL B = 4.0f * (ax * bx + az * bz);
                REAL C = bx * bx + bz * bz;

                REAL Sabc = 2.0f * MathUtil.Sqrt(A + B + C);
                REAL A2 = MathUtil.Sqrt(A);
                REAL A32 = 2.0f * A * A2;
                REAL CSQ = 2.0f * MathUtil.Sqrt(C);
                REAL BA = B / A2;

                return (A32 * Sabc + A2 * B * (Sabc - CSQ) + (4 * C * A - B * B) * MathUtil.Log((2 * A2 + BA + Sabc) / (BA + CSQ))) / (4 * A32);
            }
        }

        /// <summary>
        /// The position on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public POINT2 Position(REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;

            REAL t1 = 1.0f - t;

            POINT2 p = new POINT2();
            p.x = t1 * (t1 * C0.x + t * C1.x) + t * (t1 * C1.x + t * C2.x);
            p.y = t1 * (t1 * C0.y + t * C1.y) + t * (t1 * C1.y + t * C2.y);

            return p;
        }

        /// <summary>
        /// The tangent on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public VECTOR2 Tangent(REAL t)
        {
            VECTOR2 d = FirstDerivative(t);
            return d.Normalized;
        }

        /// <summary>
        /// The normal on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public VECTOR2 Normal(REAL t)
        {
            VECTOR2 d = FirstDerivative(t);
            return d.Normalized.PerpendicularCW;
        }

        /// <summary>
        /// The first derivative on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public VECTOR2 FirstDerivative(REAL t)
        {
            if (t < 0.0) t = 0.0f;
            if (t > 1.0) t = 1.0f;

            REAL t1 = 1.0f - t;

            VECTOR2 p = new VECTOR2();
            p.x = 2.0f * t1 * (C1.x - C0.x) + 2.0f * t * (C2.x - C1.x);
            p.y = 2.0f * t1 * (C1.y - C0.y) + 2.0f * t * (C2.y - C1.y);

            return p;
        }

        /// <summary>
        /// The closest point on the curve to the point p.
        /// </summary>
        public POINT2 Closest(POINT2 p)
        {
            REAL px = C0.x - p.x;
            REAL pz = C0.y - p.y;
            REAL ax = C1.x - C0.x;
            REAL az = C1.y - C0.y;
            REAL bx = C0.x - 2.0f * C1.x + C2.x;
            REAL bz = C0.y - 2.0f * C1.y + C2.y;

            REAL a = bx * bx + bz * bz;
            REAL b = 3 * (ax * bx + az * bz);
            REAL c = 2 * (ax * ax + az * az) + px * bx + pz * bz;
            REAL d = px * ax + pz * az;

            var roots = Polynomial3f.Solve(a, b, c, d);

            REAL min = REAL.PositiveInfinity;
            POINT2 closest = new POINT2(min, min);

            for(int i = 0; i < roots.real; i++)
            {
                REAL t = roots[i];

                POINT2 v = Position(t);
                REAL dist = POINT2.SqrDistance(v, p);
                if (dist < min)
                {
                    min = dist;
                    closest = v;
                }
            }

            return closest;
        }

        /// <summary>
        /// If the segment ab intersects the curve.
        /// </summary>
        public bool Intersects(POINT2 a, POINT2 b)
        {
            //coefficients of quadratic
            VECTOR2 c2 = (C0 + C1 * -2.0f + C2).Vector2f;
            VECTOR2 c1 = (C0 * -2.0f + C1 * 2.0f).Vector2f;

            //Convert line to normal form: ax + by + c = 0
            //Find normal to line: negative inverse of original line's slope
            VECTOR2 n = new VECTOR2(a.y - b.y, b.x - a.x);

            //c coefficient for normal form of line
            REAL c = a.x * b.y - b.x * a.y;

            //Transform coefficients to line's coordinate system and find roots of cubic
            var roots = Polynomial3f.Solve(1, VECTOR2.Dot(n, c2), VECTOR2.Dot(n, c1), VECTOR2.Dot(n, C0.Vector2f) + c);

            VECTOR2 min, max;
            min.x = Math.Min(a.x, b.x);
            min.y = Math.Min(a.y, b.y);

            max.x = Math.Max(a.x, b.x);
            max.y = Math.Max(a.y, b.y);

            for (int i = 0; i < roots.real; i++)
            {
                REAL t = roots[i];
                if (t < 0.0 || t > 1.0) continue;

                POINT2 v0 = Position(t);

                if (a.x == b.x)
                {
                    if (min.y <= v0.y && v0.y <= max.y)
                        return true;
                }
                else if (a.y == b.y)
                {
                    if (min.x <= v0.x && v0.x <= max.x)
                        return true;
                }
                else if(min.x <= v0.x && v0.x <= max.x && min.y <= v0.y && v0.y <= max.y)
                    return true;
            }

            return false;

        }

    }
}
