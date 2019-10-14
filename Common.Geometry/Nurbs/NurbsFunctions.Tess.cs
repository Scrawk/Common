using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static partial class NurbsFunctions
    {

        /// <summary>
        /// Sample a range of a NURBS curve at equally spaced parametric intervals.
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="numSamples">integer number of samples</param>
        /// <returns></returns>
        public static List<Vector3d> RationalRegularSampleRange(NurbsCurveData3d curve, int numSamples)
        {
            var start = curve.Knots[0];
            var end = curve.Knots.Last();
            return RationalRegularSampleRange(curve, start, end, numSamples);
        }

        /// <summary>
        /// Sample a range of a NURBS curve at equally spaced parametric intervals.
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="start">start parameter for sampling</param>
        /// <param name="end">end parameter for sampling</param>
        /// <param name="numSamples">integer number of samples</param>
        /// <returns></returns>
        public static List<Vector3d> RationalRegularSampleRange(NurbsCurveData3d curve, double start, double end, int numSamples)
        {
            if (numSamples < 1)
                numSamples = 2;

            var points = new List<Vector3d>(numSamples);
            double span = (end - start) / (numSamples - 1);

            for (int i = 0; i < numSamples; i++)
            {
                double u = start + span * i;
                points.Add(RationalPoint(curve, u));
            }

            return points;
        }

        /// <summary>
        /// Sample a NURBS curve over its entire domain, corresponds to [this algorithm](http://ariel.chronotext.org/dd/defigueiredo93adaptive.pdf)
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="tol">tol for the adaptive scheme</param>
        /// <returns>an array of dim + 1 length where the first element is the param where it was sampled and the remaining the pt</returns>
        public static List<Vector3d> RationalAdaptiveSample(NurbsCurveData3d curve, double tol = 1e-6)
        {
            //if degree is 1, just return the dehomogenized control points
            if (curve.Degree == 1)
                return curve.DehomogenisedControl();

            return RationalAdaptiveSampleRange(curve, curve.Knots[0], curve.Knots.Last(), tol);
        }

        /// <summary>
        /// Sample a NURBS curve at 3 points, facilitating adaptive sampling
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="start">start parameter for sampling</param>
        /// <param name="end">end parameter for sampling</param>
        /// <param name="tol">tol for the adaptive scheme</param>
        /// <returns>an array of dim + 1 length where the first element is the param where it was sampled and the remaining the pt</returns>
        public static List<Vector3d> RationalAdaptiveSampleRange(NurbsCurveData3d curve, double start, double end, double tol)
        {

            //sample curve at three pts
            var p1 = RationalPoint(curve, start);
            var p3 = RationalPoint(curve, end);

            var rnd = new Random();
            double t = 0.5 + 0.2 * rnd.NextDouble();

            var mid = start + (end - start) * t;
            var p2 = RationalPoint(curve, mid);

            //if the two end control points are coincident, the three point test will always return 0, let's split the curve
            var diff = p1 - p3;
            var diff2 = p1 - p2;

            var points = new List<Vector3d>();

            //the first condition checks if the curve makes up a loop, if so, we will need to continue evaluation
            if ((Vector3d.Dot(diff, diff) < tol && Vector3d.Dot(diff2, diff2) > tol) /*|| !Trig.threePointsAreFlat(p1, p2, p3, tol)*/ )
            {
                //get the exact middle
                var exact_mid = start + (end - start) * 0.5;

                //recurse on the two halves
                var left_pts = RationalAdaptiveSampleRange(curve, start, exact_mid, tol);
                var right_pts = RationalAdaptiveSampleRange(curve, exact_mid, end, tol);

                points.AddRange(left_pts.Slice(0, -1));
                points.AddRange(right_pts);
            }
            else
            {
                points.Add(p1, p3);
			}

            return points;
		}
    }
}
