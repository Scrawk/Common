using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

    /// <summary>
    /// An Arc is a three dimensional curve representing a subset of a full Circle
    /// </summary>
    public class Arc2d : NurbsCurve2d
    {

        /// <summary>
        /// Constructor for Arc
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xaxis"></param>
        /// <param name="yaxis"></param>
        /// <param name="radius">Radius of the arc</param>
        /// <param name="minAngle">Start angle in radians</param>
        /// <param name="maxAngle">End angle in radians</param>
        public Arc2d(Vector2d center, Vector2d xaxis, Vector2d yaxis, double radius, double minAngle, double maxAngle)
            : base(NurbsFunctions.Arc(center, xaxis, yaxis, radius, minAngle, maxAngle))
        {
            Center = center;
            XAxis = xaxis;
            YAxis = yaxis;
            Radius = radius;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
        }

        public Vector2d Center { get; private set; }

        public Vector2d XAxis { get; private set; }

        public Vector2d YAxis { get; private set; }

        public double Radius { get; private set; }

        public double MinAngle { get; private set; }

        public double MaxAngle { get; private set; }

    }

}