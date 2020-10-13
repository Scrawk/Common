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
        /// <param name="points">The list of sampled points.</param>
        internal static  void Regular(NurbsCurve3d curve, List<Vector3d> points, double start, double end, int numSamples)
        {
            if (numSamples < 2)
                numSamples = 2;

            double span = (end - start) / (numSamples - 1);

            for (int i = 0; i < numSamples; i++)
            {
                double u = start + span * i;
                points.Add(curve.Point(u));
            }
        }

        /// <summary>
        /// Estimate the length of the curve.
        /// </summary>
        /// <param name="curve">NurbsCurveData object</param>
        /// <param name="start">start parameter for sampling</param>
        /// <param name="end">end parameter for sampling</param>
        /// <param name="numSamples">integer number of samples</param>
        /// <returns>The curves estmated length.</returns>
        internal static double EstimateLength(NurbsCurve3d curve, double start, double end, int numSamples)
        {
            if (numSamples < 2)
                numSamples = 2;

            double span = (end - start) / (numSamples - 1);

            double len = 0;
            Vector3d previous = new Vector3d();

            for (int i = 0; i < numSamples; i++)
            {
                double u = start + span * i;
                var point = curve.Point(u);

                if (i > 0)
                    len += Vector3d.Distance(previous, point);

                previous = point;
            }

            return len;
        }

    }

}