using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;
using POINT2 = Common.Core.Numerics.Point2d;

namespace Common.Geometry.Bezier
{
    /// <summary>
    /// A bezier curve of arbitrary degree using a Bernstein Polynominal.
    /// </summary>
    public class Bezier2d : Bezier
    {

        /// <summary>
        /// The curves degree. 1 is linear, 2 quadratic, etc.
        /// </summary>
        public int Degree { get { return Control.Length - 1; } }

        /// <summary>
        /// The control points.
        /// </summary>
        public POINT2[] Control { get; private set; }

        public Bezier2d(BEZIER_DEGREE degree)
            : this((int)degree)
        {

        }

        public Bezier2d(int degree)
        {
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            Control = new POINT2[degree + 1];
        }

        public Bezier2d(IList<POINT2> control)
        {
            int degree = control.Count - 1;
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            int count = control.Count;
            Control = new POINT2[count];
            control.CopyTo(Control, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Bezier2d: Degree={0}]", Degree);
        }

        /// <summary>
        /// The position on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public POINT2 Point(REAL t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            POINT2 p = new POINT2();

            for (int i = 0; i < n; i++)
            {
                REAL basis = Bernstein(degree, i, t);
                p += basis * Control[i];
            }

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
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            REAL inv = 1.0f / degree;
            VECTOR2 d = new VECTOR2();

            for (int i = 0; i < n - 1; i++)
            {
                REAL basis = Bernstein(degree - 1, i, t);
                VECTOR2 c = Control[i + 1] - Control[i];
                d += basis * inv * c;
            }

            return d * 4.0f;
        }

        /// <summary>
        /// Fills the array with positions on the curve.
        /// </summary>
        public void GetPoints(List<POINT2> points, int samples)
        {
            int n = Control.Length;
            int degree = Degree;

            REAL t = 0;
            REAL step = 1.0f / (samples - 1.0f);

            for (int i = 0; i < samples; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    REAL basis = Bernstein(degree, j, t);
                    points.Add(basis * Control[j]);
                }

                t += step;
            }
        }

        /// <summary>
        /// Length of curve.
        /// </summary>
        public REAL EstimateLength(int steps)
        {
            if (Degree == 1)
                return POINT2.Distance(Control[0], Control[1]);
            else
            {
                steps = Math.Max(steps, 2);
                REAL len = 0;
                POINT2 previous = Point(0);

                for (int i = 1; i < steps; i++)
                {
                    REAL t = i / (steps - 1.0f);
                    POINT2 p = Point(t);

                    len += POINT2.Distance(previous, p);
                    previous = p;
                }

                return len;
            }
        }

        /// <summary>
        /// Returns the position at t using DeCasteljau's algorithm.
        /// Same as Position(t) but slower. Used for Testing.
        /// </summary>
        internal POINT2 DeCasteljau(REAL t)
        {
            int count = Control.Length;
            POINT2[] Q = new POINT2[count];
            Array.Copy(Control, Q, count);

            for (int k = 1; k < count; k++)
            {
                for (int i = 0; i < count - k; i++)
                    Q[i] = (1.0f - t) * Q[i] + t * Q[i + 1];
            }

            return Q[0];
        }

        /// <summary>
        /// Splits the bezier at t and returns the two curves.
        /// </summary>
        /// <param name="t">Position to split (0 to 1).</param>
        /// <returns>The curve from 0 to t and from t to 1.</returns>
        public (Bezier2d left, Bezier2d right) Split(REAL t)
        {
            int count = Control.Length;
            POINT2[] Q = new POINT2[count];
            Array.Copy(Control, Q, count);

            var b0 = new Bezier2d(Degree);
            var b1 = new Bezier2d(Degree);

            b0.Control[0] = Control[0];
            b1.Control[count - 1] = Control[count - 1];

            for (int k = 1; k < count; k++)
            {
                int len = count - k;
                for (int i = 0; i < len; i++)
                    Q[i] = (1.0f - t) * Q[i] + t * Q[i + 1];

                b0.Control[k] = Q[0];
                b1.Control[len - 1] = Q[len - 1];
            }

            return (b0, b1);
        }

    }
}
