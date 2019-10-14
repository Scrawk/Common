using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

    /// <summary>
    /// An EllipseArc is a subset of an Ellipse
    /// </summary>
    public class EllipseArc2d : NurbsCurve2d
    {

        /// <summary>
        /// Create an EllipseArc
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xaxis"></param>
        /// <param name="yaxis"></param>
        /// <param name="minAngle">Minimum angle of the EllipseArc</param>
        /// <param name="maxAngle">Maximum angle of the EllipseArc</param>
        public EllipseArc2d(Vector3d center, Vector3d xaxis, Vector3d yaxis, double minAngle, double maxAngle)
                : base(NurbsFunctions.EllipseArc(center, xaxis, yaxis, minAngle, maxAngle))
        {
            Center = center;
            XAxis = xaxis;
            YAxis = yaxis;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
        }

        public Vector3d Center { get; private set; }

        public Vector3d XAxis { get; private set; }

        public Vector3d YAxis { get; private set; }

        public double MinAngle { get; private set; }

        public double MaxAngle { get; private set; }

    }
}