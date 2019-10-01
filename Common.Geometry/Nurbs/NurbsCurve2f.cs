using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A non-uniform rational basis spline curve in 2D space with control points in 3D homogeneous space.
    /// </summary>
    public class NurbsCurve2f
    {

        public NurbsCurve2f(int degree, IList<Vector3f> control, IList<int> knots)
        {
            if (!NurbsFunctions.AreValidRelations(degree, control.Count, knots.Count))
                throw new ArgumentException("Not a valid curve.");

            Degree = degree;

            int count = control.Count;
            Control = new Vector3f[count];
            control.CopyTo(Control, 0);

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = Control[i].xy;
                var w = Control[i].z;
                Control[i] = new Vector3f(c * w, w);
            }

            count = knots.Count;
            Knots = new int[count];
            knots.CopyTo(Knots, 0);

            NumberOfBasisFunctions = Knots.Length - Degree - 2;
        }

        /// <summary>
        /// The curves degree.
        /// </summary>
        public readonly int Degree;

        /// <summary>
        /// The number of basis functions in curve, ie n.
        /// </summary>
        public readonly int NumberOfBasisFunctions;

        /// <summary>
        /// The control points.
        /// </summary>
        public Vector3f[] Control { get; private set; }

        /// <summary>
        /// The knot vector.
        /// </summary>
        public int[] Knots { get; private set; }

        /// <summary>
        /// Compute a point on a non-uniform, rational b-spline curve.
        /// Corresponds to algorithm 4.1 from The NURBS book, Piegl & Tiller 2nd edition
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        public Vector2f Position(float u)
        {
            if (u < 0) u = 0;
            if (u > 1) u = 1;

            int n = NumberOfBasisFunctions;
            var span = NurbsFunctions.FindSpan(u, Degree, n, Knots);
            var basis = NurbsFunctions.BasisFunctions(u, Degree, span, Knots);

            Vector3f p = new Vector3f();
            for (int i = 0; i <= Degree; i++)
                p = p + basis[i] * Control[span - Degree + i];

            return p.xy / p.z;
        }

        /// <summary>
        /// The tangent on the curve at u.
        /// </summary>
        /// <param name="u">Number between 0 and 1.</param>
        public Vector2f Tangent(float u)
        {
            Vector2f d = Derivatives(u, 1)[1];
            return d.Normalized;
        }

        /// <summary>
        /// The normal on the curve at u.
        /// </summary>
        /// <param name="u">Number between 0 and 1.</param>
        public Vector2f Normal(float u)
        {
            Vector2f d = Derivatives(u, 1)[1];
            return d.Normalized.PerpendicularCW;
        }

        /// <summary>
        /// Determine the derivatives of a non-uniform, rational B-spline curve at a given parameter.
        /// Corresponds to algorithm 4.2 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        /// <param name="numDerivs">The number of derivatives to compute.</param>
    	public Vector2f[] Derivatives(float u, int numDerivs)
        {
            if (u < 0) u = 0;
            if (u > 1) u = 1;

            var ders = HomogeniseDerivatives(u, numDerivs);

            var CK = new Vector2f[numDerivs + 1];

            for (int k = 0; k <= numDerivs; k++)
            {
                var v = ders[k].xy;

                for (int i = 1; i <= k; i++)
                    v = v - (float)IMath.Binomial(k, i) * ders[i].z * CK[k - i];

                CK[k] = v / ders[0].z;
            }

            return CK;
        }

        private Vector3f[] HomogeniseDerivatives(float u, int numDerivs)
        {
            numDerivs = Math.Min(Degree, numDerivs);
            var span = NurbsFunctions.FindSpan(u, Degree, Knots);
            var nders = NurbsFunctions.DerivativeBasisFunctions(u, Degree, span, numDerivs, Knots);

            var CK = new Vector3f[numDerivs + 1];

            for (int k = 0; k <= numDerivs; k++)
            {
                for (int j = 0; j <= Degree; j++)
                    CK[k] = CK[k] + nders[k, j] * Control[span - Degree + j];
            }

            return CK;
        }

    }
}
