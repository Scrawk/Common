using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public class NurbsCurve2f
    {

        public NurbsCurve2f(int degree, IList<Vector2f> control, IList<int> knots)
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

    }
}
