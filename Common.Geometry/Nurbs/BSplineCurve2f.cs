using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{


    /// <summary>
    /// A non-uniform non-rational basis spline curve in 2D space.
    /// </summary>
    public class BSplineCurve2f
    {

        public BSplineCurve2f(int degree, IList<Vector2f> control, IList<int> knots)
        {
            if (!NurbsFunctions.AreValidRelations(degree, control.Count, knots.Count))
                throw new ArgumentException("Not a valid curve.");

            Degree = degree;

            int count = control.Count;
            Control = new Vector2f[count];
            control.CopyTo(Control, 0);

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
        public Vector2f[] Control { get; private set; }

        /// <summary>
        /// The knot vector.
        /// </summary>
        public int[] Knots { get; private set; }

        /// <summary>
        /// Compute a point on a non-uniform, non-rational b-spline curve.
        /// Corresponds to algorithm 3.1 from The NURBS book, Piegl & Tiller 2nd edition
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        public Vector2f Position(float u)
        {
            if (u < 0) u = 0;
            if (u > 1) u = 1;

            int n = NumberOfBasisFunctions;
            var span = NurbsFunctions.FindSpan(u, Degree, n, Knots);
            var basis = NurbsFunctions.BasisFunctions(u, Degree, span, Knots);

            Vector2f p = new Vector2f();
            for (int i = 0; i <= Degree; i++)
                p = p + basis[i] * Control[span - Degree + i];

            return p;
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
        /// Determine the derivatives of a non-uniform, non-rational B-spline curve at a given parameter.
        /// Corresponds to algorithm 3.2 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        /// <param name="numDerivs">The number of derivatives to compute.</param>
    	public Vector2f[] Derivatives(float u, int numDerivs)
        {
            if (u < 0) u = 0;
            if (u > 1) u = 1;

            numDerivs = Math.Min(Degree, numDerivs);
            var span = NurbsFunctions.FindSpan(u, Degree, Knots);
            var nders = NurbsFunctions.DerivativeBasisFunctions(u, Degree, span, numDerivs, Knots);

            var CK = new Vector2f[numDerivs + 1];

            for (int k = 0; k <= numDerivs; k++)
            {
                for (int j = 0; j <= Degree; j++)
                    CK[k] = CK[k] + nders[k, j] * Control[span - Degree + j];
            }

            return CK;
        }

    }
}
