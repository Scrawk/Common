﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;

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
        public Vector3f[] Control { get; private set; }

        public Bezier3f(BEZIER_DEGREE degree)
            : this((int)degree)
        {

        }

        public Bezier3f(int degree)
        {
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            Control = new Vector3f[degree + 1];
        }

        public Bezier3f(IList<Vector3f> control)
        {
            int degree = control.Count - 1;
            if (degree > MAX_DEGREE || degree < MIN_DEGREE)
                throw new ArgumentException(string.Format("Degree can not be greater than {0} or less than {1}.", MAX_DEGREE, MIN_DEGREE));

            int count = control.Count;
            Control = new Vector3f[count];
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
        public Vector3f Position(float t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            Vector3f p = new Vector3f();

            for (int i = 0; i < n; i++)
            {
                float basis = Bernstein(degree, i, t);
                p += basis * Control[i];
            }

            return p;
        }

        /// <summary>
        /// The tangent on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public Vector3f Tangent(float t)
        {
            Vector3f d = FirstDerivative(t);
            return d.Normalized;
        }

        /// <summary>
        /// The first derivative on the curve at t.
        /// </summary>
        /// <param name="t">Number between 0 and 1.</param>
        public Vector3f FirstDerivative(float t)
        {
            if (t < 0) t = 0;
            if (t > 1) t = 1;

            int n = Control.Length;
            int degree = Degree;
            float inv = 1.0f / degree;
            Vector3f d = new Vector3f();

            for (int i = 0; i < n - 1; i++)
            {
                float basis = Bernstein(degree - 1, i, t);
                d += basis * inv * (Control[i + 1] - Control[i]);
            }

            return d * 4.0f;
        }

        /// <summary>
        /// Fills the array with positions on the curve.
        /// </summary>
        public void Tessellate(List<Vector3f> points, int samples)
        {
            int n = Control.Length;
            int degree = Degree;

            float t = 0;
            float step = 1.0f / (samples - 1.0f);

            for (int i = 0; i < samples; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    float basis = Bernstein(degree, j, t);
                    points.Add(basis * Control[j]);
                }

                t += step;
            }
        }

        /// <summary>
        /// Length of curve.
        /// </summary>
        public float EstimateLength(int steps, float tmax = 1.0f)
        {
            if (tmax <= 0) return 0;
            if (tmax > 1) tmax = 1;

            if (Degree == 1)
                return Vector3f.Distance(Control[0], Control[1]) * tmax;
            else
            {
                steps = Math.Max(steps, 2);
                float len = 0;
                Vector3f previous = Position(0);

                for (int i = 1; i < steps; i++)
                {
                    float t = i / (steps - 1.0f) * tmax;
                    Vector3f p = Position(t);

                    len += Vector3f.Distance(previous, p);
                    previous = p;
                }

                return len;
            }
        }

        /// <summary>
        /// Returns the position at t using DeCasteljau's algorithm.
        /// Same as Position(t) but slower. Used for Testing.
        /// </summary>
        internal Vector3f DeCasteljau(float t)
        {
            int count = Control.Length;
            Vector3f[] Q = new Vector3f[count];
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
        /// <param name="b0">The curve from 0 to t.</param>
        /// <param name="b1">The curve from t to 1.</param>
        public void Split(float t, out Bezier3f b0, out Bezier3f b1)
        {
            int count = Control.Length;
            Vector3f[] Q = new Vector3f[count];
            Array.Copy(Control, Q, count);

            b0 = new Bezier3f(Degree);
            b1 = new Bezier3f(Degree);

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
        }

    }
}
