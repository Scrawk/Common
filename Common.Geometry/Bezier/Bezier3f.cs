using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using REAL = System.Single;
using VECTOR3 = Common.Core.Numerics.Vector3f;
using POINT3 = Common.Core.Numerics.Point3f;

namespace Common.Geometry.Bezier
{
    /// <summary>
    /// A bezier curve of arbitrary degree using a Bernstein Polynominal.
    /// </summary>
    public class Bezier3f : Bezier
    {

        /// <summary>
        /// The curves degree. 1 is linear, 2 quadratic, etc.
        /// </summary>
        public int Degree { get { return Control.Length - 1; } }

        /// <summary>
        /// The control points.
        /// </summary>
        public POINT3[] Control { get; private set; }

        public Bezier3f(BEZIER_DEGREE degree)
            : this((int)degree)
        {

        }

        public Bezier3f(int degree)
        {
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            Control = new POINT3[degree + 1];
        }

        public Bezier3f(IList<POINT3> control)
        {
            int degree = control.Count - 1;
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            int count = control.Count;
            Control = new POINT3[count];
            control.CopyTo(Control, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Bezier3f: Degree={0}]", Degree);
        }

        /// <summary>
        /// The position on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public POINT3 Point(REAL t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            POINT3 p = new POINT3();

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
        public VECTOR3 Tangent(REAL t)
        {
            VECTOR3 d = FirstDerivative(t);
            return d.Normalized;
        }

        /// <summary>
        /// The first derivative on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public VECTOR3 FirstDerivative(REAL t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            REAL inv = 1.0f / degree;
            VECTOR3 d = new VECTOR3();

            for (int i = 0; i < n - 1; i++)
            {
                REAL basis = Bernstein(degree - 1, i, t);
                VECTOR3 c = Control[i + 1] - Control[i];
                d += basis * inv * c;
            }

            return d * 4.0f;
        }

        /// <summary>
        /// Fills the array with positions on the curve.
        /// </summary>
        public void GetPoints(List<POINT3> points, int samples)
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
                return POINT3.Distance(Control[0], Control[1]);
            else
            {
                steps = Math.Max(steps, 2);
                REAL len = 0;
                POINT3 previous = Point(0);

                for (int i = 1; i < steps; i++)
                {
                    REAL t = i / (steps - 1.0f);
                    POINT3 p = Point(t);

                    len += POINT3.Distance(previous, p);
                    previous = p;
                }

                return len;
            }
        }

        /// <summary>
        /// Returns the position at t using DeCasteljau's algorithm.
        /// Same as Position(t) but slower. Used for Testing.
        /// </summary>
        internal POINT3 DeCasteljau(REAL t)
        {
            int count = Control.Length;
            POINT3[] Q = new POINT3[count];
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
        public (Bezier3f left, Bezier3f right) Split(REAL t)
        {
            int count = Control.Length;
            POINT3[] Q = new POINT3[count];
            Array.Copy(Control, Q, count);

            var b0 = new Bezier3f(Degree);
            var b1 = new Bezier3f(Degree);

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
