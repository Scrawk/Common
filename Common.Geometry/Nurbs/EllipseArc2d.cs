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
        public EllipseArc2d(Vector2d center, Vector2d xaxis, Vector2d yaxis, double minAngle, double maxAngle)
                : base(NurbsFunctions.EllipseArc(center, xaxis, yaxis, minAngle, maxAngle))
        {
            Center = center;
            XAxis = xaxis;
            YAxis = yaxis;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
        }

        public Vector2d Center { get; private set; }

        public Vector2d XAxis { get; private set; }

        public Vector2d YAxis { get; private set; }

        public double MinAngle { get; private set; }

        public double MaxAngle { get; private set; }

    }
}