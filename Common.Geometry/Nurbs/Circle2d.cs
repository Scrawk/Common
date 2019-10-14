using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public class Circle2d : Arc2d
    {

        /// <summary>
        /// Create a circle
        /// </summary>
        /// <param name="center">Vector representing the center of the circle</param>
        /// <param name="xaxis">Vector representing the xaxis</param>
        /// <param name="yaxis">Vector representing the perpendicular yaxis</param>
        /// <param name="radius">Radius of the circle</param>
        public Circle2d(Vector3d center, Vector3d xaxis, Vector3d yaxis, double radius)
            : base(center, xaxis, yaxis, radius, 0, Math.PI * 2)
        {

        }
    }
}
