using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public class Ellipse2d : EllipseArc2d
    {
        /// <summary>
        /// Create an ellipse
        /// </summary>
        /// <param name="center">Vector representing the center of the circle</param>
        /// <param name="xaxis">Vector representing the xaxis</param>
        /// <param name="yaxis">Vector representing the perpendicular yaxis</param>
        public Ellipse2d(Vector2d center, Vector2d xaxis, Vector2d yaxis)
            : base(center, xaxis, yaxis, 0, Math.PI * 2)
        {

        }
    }
}
