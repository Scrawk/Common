using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	internal static class NurbsTess
	{

        /// <summary>
        /// Sample a range of a NURBS curve at equally spaced parametric intervals.
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="start">start parameter for sampling</param>
        /// <param name="end">end parameter for sampling</param>
        /// <param name="numSamples">integer number of samples</param>
        /// <returns></returns>
        internal static List<Vector> Regular(NurbsCurve curve, double start, double end, int numSamples)
        {
            if (numSamples < 1)
                numSamples = 2;

            var points = new List<Vector>(numSamples);
            double span = (end - start) / (numSamples - 1);

            for (int i = 0; i < numSamples; i++)
            {
                double u = start + span * i;
                points.Add(NurbsEval.CurvePoint(curve, u));
            }

            return points;
        }

        /// <summary>
        /// Sample a range of a NURBS curve at equally spaced parametric intervals.
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="start">start parameter for sampling</param>
        /// <param name="end">end parameter for sampling</param>
        /// <param name="numSamples">integer number of samples</param>
        /// <returns></returns>
        internal static List<Vector> Regular(RationalNurbsCurve curve, double start, double end, int numSamples)
        {
            if (numSamples < 1)
                numSamples = 2;

            var points = new List<Vector>(numSamples);
            double span = (end - start) / (numSamples - 1);

            for (int i = 0; i < numSamples; i++)
            {
                double u = start + span * i;
                points.Add(NurbsEval.CurvePoint(curve, u));
            }

            return points;
        }

    }

}