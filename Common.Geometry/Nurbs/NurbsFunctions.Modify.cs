using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static partial class NurbsFunctions
    {

        /// <summary>
        /// Insert a collection of knots on a curve.
        /// Corresponds to Algorithm A5.4 (Piegl & Tiller).
        /// </summary>
        public static NurbsCurveData3d KnotRefine(NurbsCurveData3d curve, IList<double> knotsToInsert)
        {
            if (knotsToInsert.Count == 0) return curve.Copy();

            int degree = curve.Degree;
            var controlPoints = curve.Control;
            var knots = curve.Knots;

            int i = 0;
            int n = controlPoints.Length - 1;
            int m = n + degree + 1;
            int r = knotsToInsert.Count - 1;
            int a = FindSpan(knotsToInsert[0], degree, knots);
            int b = FindSpan(knotsToInsert[r], degree, knots);

            int controlLen = n + r + 2;
            int knotLen = m + r + 2;

            var controlPoints_post = new Vector4d[controlLen];
            var knots_post = new double[knotLen];

            for (i = 0; i < a - degree + 1; i++)
                controlPoints_post[i] = controlPoints[i];

            for (i = b - 1; i < n + 1; i++)
                controlPoints_post[i + r + 1] = controlPoints[i];

            for (i = 0; i < a + 1; i++)
                knots_post[i] = knots[i];

            for (i = b + degree; i < m + 1; i++)
                knots_post[i + r + 1] = knots[i];

            i = b + degree - 1;
            int k = b + degree + r;
            int j = r;

            while (j >= 0)
            {
                while (knotsToInsert[j] <= knots[i] && i > a)
                {
                    controlPoints_post[k - degree - 1] = controlPoints[i - degree - 1];
                    knots_post[k] = knots[i];
                    k = k - 1;
                    i = i - 1;
                }

                controlPoints_post[k - degree - 1] = controlPoints_post[k - degree];

                for (int jj = 1; jj < degree + 1; jj++)
                {
                    var ind = k - degree + jj;
                    var alfa = knots_post[k + jj] - knotsToInsert[j];

                    if (Math.Abs(alfa) < FMath.EPS)
                        controlPoints_post[ind - 1] = controlPoints_post[ind];
                    else
                    {
                        alfa = alfa / (knots_post[k + jj] - knots[i - degree + jj]);
                        controlPoints_post[ind - 1] = alfa * controlPoints_post[ind - 1] + (1.0f - alfa) * controlPoints_post[ind];
                    }
                }

                knots_post[k] = knotsToInsert[j];
                k = k - 1;

                j--;
            }

            return new NurbsCurveData3d(degree, controlPoints_post, knots_post);
        }

        /// <summary>
        /// Decompose a NURBS curve into a collection of bezier's.  Useful
        /// as each bezier fits into it's convex hull.  This is a useful starting
        /// point for intersection, closest point, divide & conquer algorithms
        /// </summary>
        /// <param name="curve">NurbsCurveData object representing the curve</param>
        /// <returns>Array of NurbsCurveData objects, defined by degree, knots, and control points</returns>
        public static List<NurbsCurveData3d> DecomposeIntoBeziers(NurbsCurveData3d curve)
        {
            var degree = curve.Degree;
            var controlPoints = curve.Control;
            var knots = curve.Knots;

            //find all of the unique knot values and their multiplicity
            //for each, increase their multiplicity to degree + 1

            var knotmults = KnotMultiplicities(knots);
            var reqMult = degree + 1;

            //insert the knots
            foreach(var knotmult in knotmults)
            { 
                if (knotmult.mult < reqMult)
                {
                    int num = reqMult - knotmult.mult;
                    var knotsInsert = new List<double>(num);
                    knotsInsert.AddRange(num, knotmult.knot);
                    var res = KnotRefine(new NurbsCurveData3d(degree, controlPoints, knots), knotsInsert);

                    knots = res.Knots;
                    controlPoints = res.Control;
                }
            }

            var numCrvs = knots.Length / reqMult - 1;
            var crvKnotLength = reqMult * 2;
            var crvs = new List<NurbsCurveData3d>(numCrvs);

            var i = 0;
            while (i < controlPoints.Length)
            {
                var kts = knots.Slice(i, i + crvKnotLength);
                var pts = controlPoints.Slice(i, i + reqMult);

                crvs.Add(new NurbsCurveData3d(degree, pts, kts));

                i += reqMult;
            }

            return crvs;
        }

    }
}
