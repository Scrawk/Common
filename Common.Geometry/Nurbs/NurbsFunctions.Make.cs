using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static partial class NurbsFunctions
    {
        /// <summary>
        /// 0) build knot vector for curve by normalized chord length
        /// 1) construct effective basis function in square matrix (W)
        /// 2) construct set of coordinattes to interpolate vector (p)
        /// 3) set of control points (c)
        /// 4) solve for c in all 3 dimensions
        /// </summary>
        /// <param name="degree">The degree of the curve.</param>
        /// <param name="points">The points to interp curve from.</param>
        /// <returns></returns>
        public static NurbsCurveData2f Interpolate(int degree, IList<Vector2f> points)
        {

            if (points.Count < degree + 1)
                throw new ArgumentException("You need to supply at least degree + 1 points.");

            var us = new List<float>();
            us.Add(0);

            for (int i = 1; i < points.Count; i++)
            {
                float chord = (points[i] - points[i - 1]).Magnitude;
                float last = us[us.Count - 1];
                us.Add(last + chord);
            }

            //normalize
            var max = us[us.Count - 1];
            for (int i = 1; i < us.Count; i++)
                us[i] = us[i] / max;

            var knotsStart = new List<float>(degree);
            knotsStart.AddRange(degree + 1, 0);

            //we need two more control points, two more knots
            var start = 1;
            var end = us.Count - degree;

            for (int i = start; i < end; i++)
            {
                float weightSums = 0.0f;
                for (int j = 0; j < degree; j++)
                {
                    weightSums += us[i + j];
                }

                knotsStart.Add((1.0f / degree) * weightSums);
            }

            var knots = new List<float>(knotsStart);
            knots.AddRange(degree + 1, 1.0f);

            //build matrix of basis function coeffs (TODO: use sparse rep)
            var A = new List<List<float>>();
            var n = points.Count - 1;
            var ld = points.Count - (degree + 1);

            foreach (var u in us)
            {
                int span = FindSpan(u, degree, n, knots);
                var basisFuncs = BasisFunctions(u, degree, span, knots);

                int ls = span - degree;

                var row = new List<float>();
                row.AddRange(ls, 0);
                row.AddRange(basisFuncs);
                row.AddRange(ld - ls, 0);

                A.Add(row);
            }

            //for each dimension, solve
            var rows = 2;
            var columns = points.Count;
            var xs = new Matrix(rows, columns);
            var M = new Matrix(A);

            for (int i = 0; i < rows; i++)
            {
                var b = new Vector(columns);

                int j = 0;
                foreach (var p in points)
                    b[j++] = p[i];

                var x = Matrix.Solve(M, b);
                xs.SetRow(i, x);
            }

            var controlPts = new List<Vector2f>();
            for (int i = 0; i < columns; i++)
            {
                var v = new Vector2f((float)xs[0, i], (float)xs[1, i]);
                controlPts.Add(v);
            }

            return new NurbsCurveData2f(degree, controlPts, knots, null);
        }
    }
}
