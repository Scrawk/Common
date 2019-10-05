using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static partial class NurbsFunctions
    {
        /// <summary>
        /// Split a NURBS curve into two parts at a given parameter.
        /// </summary>
        /// <param name="curve">NurbsCurveData object representing the curve</param>
        /// <param name="u">location to split the curve</param>
        /// <returns>two new curves, defined by degree, knots, and control points</returns>
        public static NurbsCurveData2f[] Split(NurbsCurveData2f curve, float u)
        {
            int degree = curve.Degree;
            var knots = curve.Knots;

            var knots_to_insert = new float[degree + 1];

            for (int i = 0; i < degree + 1; i++)
                knots_to_insert[i] = u;

            var res = KnotRefine(curve, knots_to_insert);
            var s = FindSpan(u, degree, knots);

            var knots0 = res.Knots.Slice(0, s + degree + 2);
            var knots1 = res.Knots.Slice(s + 1);

            var cpts0 = res.Control.Slice(0, s + 1);
            var cpts1 = res.Control.Slice(s + 1);

            return new NurbsCurveData2f[]
            {
                new NurbsCurveData2f(degree, cpts0, knots0),
                new NurbsCurveData2f(degree, cpts1, knots1)
            };

        }
    }
}
