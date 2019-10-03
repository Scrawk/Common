using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static class NurbsFunctions
    {

        /// <summary>
        /// Confirm the relations between degree (p), number of control points(n+1),
        /// and the number of knots (m+1).
        /// </summary>
        /// <param name="p">The degree</param>
        /// <param name="n">Number of control points</param>
        /// <param name="m">Number of knots</param>
        /// <returns></returns>
        public static bool AreValidRelations(int p, int n, int m)
        {
            return n + p + 1 - m == 0;
        }

        /// <summary>
        /// Given the degree (p) and number of control points how 
        /// many knots are required to make a valid curve.
        /// </summary>
        /// <param name="p">The degree</param>
        /// <param name="n">Number of control points</param>
        /// <returns></returns>
        public static int RequiredKnots(int p, int n)
        {
            return n + p + 1;
        }

        /// <summary>
        /// Determine the knot span index.
        /// Corresponds to algorithm 2.1 from The NURBS book, Piegl & Tiller 2nd edition
        /// </summary>
        /// <param name="p">Degree of function</param>
        /// <param name="u">The parameter</param>
        /// <param name="U">The knot vector</param>
        /// <returns>The knot span index</returns>
        public static int FindSpan(float u, int p, IList<float> U)
        {
            int n = U.Count - p - 2;
            return FindSpan(u, p, n, U);
        }

        /// <summary>
        /// Determine the knot span index.
        /// Corresponds to algorithm 2.1 from The NURBS book, Piegl & Tiller 2nd edition
        /// </summary>
        /// <param name="n">Number of basis functions - 1 = U.Count - degree - 2</param>
        /// <param name="p">Degree of function</param>
        /// <param name="u">The parameter</param>
        /// <param name="U">The knot vector</param>
        /// <returns>The knot span index</returns>
        public static int FindSpan(float u, int p, int n, IList<float> U)
        {
            //Special case.
            if (u == U[n + 1]) return n;

            //Do binary search.
            int low = p;
            int high = n + 1;
            int mid = (low + high) / 2;

            while (u < U[mid] || u >= U[mid + 1])
            {
                if (u < U[mid])
                    high = mid;
                else
                    low = mid;

                mid = (low + high) / 2;
            }

            return mid;
        }

        /// <summary>
        /// Compute the non-vanishing basis functions
        /// Corresponds to algorithm 2.2 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="u">The parameter</param>
        /// <param name="p">Degree of function</param>
        /// <param name="U">The knot vector</param>
        /// <returns></returns>
        public static float[] BasisFunctions(float u, int p, IList<float> U)
        {
            int i = FindSpan(u, p, U);
            return BasisFunctions(u, p, i, U);
        }

        /// <summary>
        /// Compute the non-vanishing basis functions
        /// Corresponds to algorithm 2.2 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="i">The knot span index</param>
        /// <param name="u">The parameter</param>
        /// <param name="p">Degree of function</param>
        /// <param name="U">The knot vector</param>
        /// <returns></returns>
        public static float[] BasisFunctions(float u, int p, int i, IList<float> U)
        {
            var N = new float[p + 1];
            var left = new float[p + 1];
            var right = new float[p + 1];

            N[0] = 1.0f;

            for (int j = 1; j <= p; j++)
            {
                left[j] = u - U[i + 1 - j];
                right[j] = U[i + j] - u;

                float saved = 0;

                for (int r = 0; r < j; r++)
                {
                    float temp = N[r] / (right[r + 1] + left[j - r]);

                    N[r] = saved + right[r + 1] * temp;
                    saved = left[j - r] * temp;
                }

                N[j] = saved;
            }

            return N;
        }

        /// <summary>
        /// Compute the non-vanishing basis functions and their derivatives.
        /// Corresponds to algorithm 2.3 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="u">The parameter</param>
        /// <param name="p">Degree of function</param>
        /// <param name="U">The knot vector</param>
        /// <returns>d array of basis and derivative values of size (n+1, p+1) The nth row is
        /// the nth derivative and the first row is made up of the basis function values.</returns>
        public static float[,] DerivativeBasisFunctions(float u, int p, IList<float> U)
        {
            int n = U.Count - p - 2;
            int i = FindSpan(u, p, n, U);
            return DerivativeBasisFunctions(u, p, i, n, U);
        }

        /// <summary>
        /// Compute the non-vanishing basis functions and their derivatives.
        /// Corresponds to algorithm 2.3 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="i">The knot span index</param>
        /// <param name="u">The parameter</param>
        /// <param name="p">Degree of function</param>
        /// <param name="n">Number of basis functions - 1 = U.Count - degree - 2</param>
        /// <param name="U">The knot vector</param>
        /// <returns>d array of basis and derivative values of size (n+1, p+1) The nth row is
        /// the nth derivative and the first row is made up of the basis function values.</returns>
        public static float[,] DerivativeBasisFunctions(float u, int p, int i, int n, IList<float> U)
        {
            var ndu = new float[p + 1, p + 1];
            var left = new float[p + 1];
            var right = new float[p + 1];
            float saved, temp;

            ndu[0, 0] = 1.0f;

            for (int j = 1; j <= p; j++)
            {
                left[j] = u - U[i + 1 - j];
                right[j] = U[i + j] - u;
                saved = 0.0f;

                for (int r = 0; r < j; r++)
                {
                    //Lower triangle.
                    ndu[j, r] = right[r + 1] + left[j - r];
                    temp = ndu[r, j - 1] / ndu[j, r];

                    //Upper triangle.
                    ndu[r, j] = saved + right[r + 1] * temp;
                    saved = left[j - r] * temp;

                }
                ndu[j, j] = saved;
            }

            var ders = new float[n + 1, p + 1];
            var a = new float[2, p + 1];

            //Load the basis functions.
            for (int j = 0; j <= p; j++)
                ders[0, j] = ndu[j, p];

            //This section computes the derivatives (Eq 2.9).
            for (int r = 0; r <= p; r++)
            {
                //Alternate rows in array.
                int s1 = 0, s2 = 1;
                a[0, 0] = 1.0f;

                //Loop to compute kth derivative.
                for (int k = 1; k <= n; k++)
                {
                    int j1, j2;
                    float d = 0.0f;
                    int rk = r - k;
                    int pk = p - k;

                    if (r >= k)
                    {
                        a[s2, 0] = a[s1, 0] / ndu[pk + 1, rk];
                        d = a[s2, 0] * ndu[rk, pk];
                    }

                    if (rk >= -1)
                        j1 = 1;
                    else
                        j1 = -rk;

                    if (r - 1 <= pk)
                        j2 = k - 1;
                    else
                        j2 = p - r;

                    for (int j = j1; j <= j2; j++)
                    {
                        a[s2, j] = (a[s1, j] - a[s1, j - 1]) / ndu[pk + 1, rk + j];
                        d += a[s2, j] * ndu[rk + j, pk];
                    }

                    if (r <= pk)
                    {
                        a[s2, k] = -a[s1, k - 1] / ndu[pk + 1, r];
                        d += a[s2, k] * ndu[r, pk];
                    }

                    ders[k, r] = d;

                    //Switch rows.
                    int tmp = s1;
                    s1 = s2;
                    s2 = tmp;
                }
            }

            var acc = p;
            for (int k = 1; k <= n; k++)
            {
                for (int j = 0; j <= p; j++)
                {
                    ders[k, j] *= acc;
                }
                acc *= (p - k);
            }

            return ders;
        }

        public static NurbsCurveData2f RationalInterpCurve(int degree, IList<Vector2f> points)
        {
            // 0) build knot vector for curve by normalized chord length
            // 1) construct effective basis function in square matrix (W)
            // 2) construct set of coordinattes to interpolate vector (p)
            // 3) set of control points (c)

            //Wc = p

            // 4) solve for c in all 3 dimensions

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
