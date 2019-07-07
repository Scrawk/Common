using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Curves
{
    /// <summary>
    /// A bezier curve of arbitrary degree using a Bernstein Polynominal.
    /// </summary>
    public class Bezier3d : Bezier
    {

        /// <summary>
        /// The curves degree. 1 is linear, 2 quadratic, etc.
        /// </summary>
        public int Degree { get { return Control.Length - 1; } }

        /// <summary>
        /// The control points.
        /// </summary>
        public Vector3d[] Control { get; private set; }

        public Bezier3d(BEZIER_DEGREE degree)
            : this((int)degree)
        {

        }

        public Bezier3d(int degree)
        {
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            Control = new Vector3d[degree + 1];
        }

        public Bezier3d(IList<Vector3d> control)
        {
            int degree = control.Count - 1;
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            int count = control.Count;
            Control = new Vector3d[count];
            for (int i = 0; i < count; i++)
                Control[i] = control[i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Bezier3d: Degree={0}]", Degree);
        }

        /// <summary>
        /// The position on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public Vector3d Position(double t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            Vector3d p = new Vector3d();

            for (int i = 0; i < n; i++)
            {
                double basis = Bernstein(degree, i, t);
                p += basis * Control[i];
            }

            return p;
        }

        /// <summary>
        /// The tangent on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public Vector3d Tangent(double t)
        {
            Vector3d d = FirstDerivative(t);
            return d.Normalized;
        }

        /// <summary>
        /// The first derivative on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public Vector3d FirstDerivative(double t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            double inv = 1.0 / degree;
            Vector3d d = new Vector3d();

            for (int i = 0; i < n - 1; i++)
            {
                double basis = Bernstein(degree - 1, i, t);
                d += basis * inv * (Control[i + 1] - Control[i]);
            }

            return d * 4.0f;
        }

        /// <summary>
        /// Fills the array with positions on the curve.
        /// </summary>
        public void GetPositions(IList<Vector3d> points)
        {
            int count = points.Count;
            int n = Control.Length;
            int degree = Degree;

            double t = 0;
            double step = 1.0 / (count - 1.0);

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double basis = Bernstein(degree, j, t);
                    points[i] += basis * Control[j];
                }

                t += step;
            }
        }

        /// <summary>
        /// Arc length of curve via intergration.
        /// </summary>
        public double Length(int steps, double tmax = 1.0)
        {
            if (tmax <= 0) return 0;
            if (tmax > 1) tmax = 1;

            if (Degree == 1)
                return Vector3d.Distance(Control[0], Control[1]) * tmax;
            else
            {
                steps = Math.Max(steps, 2);
                double len = 0;
                Vector3d previous = Position(0);

                for (int i = 1; i < steps; i++)
                {
                    double t = i / (steps - 1.0f) * tmax;
                    Vector3d p = Position(t);

                    len += Vector3d.Distance(previous, p);
                    previous = p;
                }

                return len;
            }
        }

        /// <summary>
        /// Returns the position at t using DeCasteljau's algorithm.
        /// Same as Position(t) but slower. Used for Testing.
        /// </summary>
        internal Vector3d DeCasteljau(double t)
        {
            int count = Control.Length;
            Vector3d[] Q = new Vector3d[count];
            Array.Copy(Control, Q, count);

            for (int k = 1; k < count; k++)
            {
                for (int i = 0; i < count - k; i++)
                    Q[i] = (1.0 - t) * Q[i] + t * Q[i + 1];
            }

            return Q[0];
        }

        /// <summary>
        /// Splits the bezier at t and returns the two curves.
        /// </summary>
        /// <param name="t">Position to split (0 to 1).</param>
        /// <param name="b0">The curve from 0 to t.</param>
        /// <param name="b1">The curve from t to 1.</param>
        public void Split(double t, out Bezier3d b0, out Bezier3d b1)
        {
            int count = Control.Length;
            Vector3d[] Q = new Vector3d[count];
            Array.Copy(Control, Q, count);

            b0 = new Bezier3d(Degree);
            b1 = new Bezier3d(Degree);

            b0.Control[0] = Control[0];
            b1.Control[count - 1] = Control[count - 1];

            for (int k = 1; k < count; k++)
            {
                int len = count - k;
                for (int i = 0; i < len; i++)
                    Q[i] = (1.0 - t) * Q[i] + t * Q[i + 1];

                b0.Control[k] = Q[0];
                b1.Control[len - 1] = Q[len - 1];
            }
        }

    }
}
