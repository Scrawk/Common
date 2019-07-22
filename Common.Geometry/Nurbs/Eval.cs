using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static class Eval
    {
        /// <summary>
        /// Dehomogenize a point
        /// </summary>
        public static Vector3d Dehomogenize(Vector4d homoPoint)
        {
            return homoPoint.xyz / homoPoint.w;
        }

        /// <summary>
        /// Obtain the weight from a collection of points in homogeneous space.
        /// </summary>
        public static double[] Weight1d(IList<Vector4d> homoPoints)
        {
            int count = homoPoints.Count;
            var weights = new double[count];
            for (int i = 0; i < count; i++)
                weights[i] = homoPoints[i].w;

            return weights;
        }

        /// <summary>
        /// Dehomogenize an array of points
        /// </summary>
        public static Vector3d[] Dehomogenize1d(IList<Vector4d> homoPoints)
        {
            int count = homoPoints.Count;
            var points = new Vector3d[count];
            for (int i = 0; i < count; i++)
                points[i] = Dehomogenize(homoPoints[i]);

            return points;
        }

        /// <summary>
        /// Transform a 1d array of points into their homogeneous equivalents
        /// </summary>
        public static Vector4d[] Homogenize1d(IList<Vector3d> controlPoints, IList<double> weights = null)
        {
            var rows = controlPoints.Count;
            var homo_controlPoints = new Vector4d[rows];

            for (int i = 0; i < rows; i++)
            {
                double wt = (weights != null) ? weights[i] : 1.0;
                homo_controlPoints[i] = new Vector4d(controlPoints[i] * wt, wt);
            }

            return homo_controlPoints;
        }
    }
}
