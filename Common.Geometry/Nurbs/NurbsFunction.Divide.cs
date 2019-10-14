using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public struct CurveLengthSample
    {
        public double u;
        public double len;

        public CurveLengthSample(double u, double len)
        {
            this.u = u;
            this.len = len;
        }
    }

    public static partial class NurbsFunctions
    {
        /// <summary>
        /// Split a NURBS curve into two parts at a given parameter.
        /// </summary>
        /// <param name="curve">NurbsCurveData object representing the curve</param>
        /// <param name="u">location to split the curve</param>
        /// <returns>two new curves, defined by degree, knots, and control points</returns>
        public static NurbsCurveData2d[] Split(NurbsCurveData2d curve, double u)
        {
            int degree = curve.Degree;
            var knots = curve.Knots;

            var knots_to_insert = new double[degree + 1];

            for (int i = 0; i < degree + 1; i++)
                knots_to_insert[i] = u;

            var res = KnotRefine(curve, knots_to_insert);
            var s = FindSpan(u, degree, knots);

            var knots0 = res.Knots.Slice(0, s + degree + 2);
            var knots1 = res.Knots.Slice(s + 1);

            var cpts0 = res.Control.Slice(0, s + 1);
            var cpts1 = res.Control.Slice(s + 1);

            return new NurbsCurveData2d[]
            {
                new NurbsCurveData2d(degree, cpts0, knots0),
                new NurbsCurveData2d(degree, cpts1, knots1)
            };

        }

        /// <summary>
        /// Divide a NURBS curve given a given number of times, including the end points. The result is not split curves
    	/// but a collection of `CurveLengthSample` objects that can be used for splitting. As with all arc length methods,
    	/// the result is an approximation.
        /// </summary>
        /// <param name="curve">NurbsCurveData object representing the curve</param>
        /// <param name="num">The number of parts to split the curve into</param>
        /// <returns>An array of `CurveLengthSample` objects</returns>
    	public static List<CurveLengthSample> RationalByEqualArcLength(NurbsCurveData2d curve, int num)
        {
            var tlen = RationalArcLength(curve);
            var inc = tlen / num;

            return RationalByArcLength(curve, inc);
        }

        /// <summary>
        /// Divide a NURBS curve given a given number of times, including the end points.
        /// </summary>
        /// <param name="curve">NurbsCurveData object representing the curve</param>
        /// <param name="len">The arc length separating the resultant samples</param>
        /// <returns>An array of `CurveLengthSample` objects</returns>
    	public static List<CurveLengthSample> RationalByArcLength(NurbsCurveData2d curve, double len)
        {
            var crvs = DecomposeIntoBeziers(curve);
            var crvlens = new List<double>();
            double totlen = 0;

            for (int j = 0; j < crvs.Count; j++)
            {
                var crvLen = RationalBezierArcLength(crvs[j]);
                crvlens.Add(crvLen);
                totlen += crvLen;
            }

            var pts = new List<CurveLengthSample>();
            pts.Add(new CurveLengthSample(curve.Knots[0], 0.0));

            if (len > totlen) return pts;

            double inc = len;
            int i = 0;
            double lc = inc;
            double runsum = 0.0f;
            double runsum1 = 0.0f;
            double u;

            while (i < crvs.Count)
            {
                runsum += crvlens[i];

                while (lc < runsum + FMath.EPS)
                {
                    u = RationalBezierParamAtArcLength(crvs[i], lc - runsum1, 1e-6, crvlens[i]);
                    pts.Add(new CurveLengthSample(u, lc));
                    lc += inc;
                }
                
                runsum1 += crvlens[i];

                i++;
            }

            return pts;
        }

    }
}
