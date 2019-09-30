using System;
using System.Collections.Generic;
using System.Text;

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
        public static int FindSpan(float u, int p, IList<int> U)
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
        public static int FindSpan(float u, int p, int n, IList<int> U)
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
        public static float[] BasisFunctions(float u, int p, IList<int> U)
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
        public static float[] BasisFunctions(float u, int p, int i, IList<int> U)
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
        public static float[,] DerivativeBasisFunctions(float u, int p, IList<int> U)
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
        public static float[,] DerivativeBasisFunctions(float u, int p, int i, int n, IList<int> U)
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
    }
}
