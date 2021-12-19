using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Geometry.DCEL
{
    internal static class DCELGeometry
    {
        /// <summary>
        /// Do the 3 points form a striaght line.
        /// </summary>
        internal static bool Collinear(Point2d a, Point2d b, Point2d c)
        {
            return MathUtil.IsZero(Area2(a, b, c));
        }

        /// <summary>
        /// True if b is within the cone formed by the
        /// edges incident at the from vertex of edge.
        /// </summary>
        internal static bool InCone(Point2d a0, Point2d a, Point2d a1, Point2d b)
        {
            //if a is a convex vertex.
            if (LeftOn(a, a1, a0))
                return Left(a, b, a0) && Left(b, a, a1);

            //else a is reflex vertex.
            return !(LeftOn(a, b, a1) && LeftOn(b, a, a0));
        }

        /// <summary>
        /// Is c left of the line ab.
        /// </summary>
        internal static bool Left(Point2d a, Point2d b, Point2d c)
        {
            return Area2(a, b, c) > 0.0;
        }

        /// <summary>
        /// Is c left of or on the line ab.
        /// </summary>
        internal static bool LeftOn(Point2d a, Point2d b, Point2d c)
        {
            return Area2(a, b, c) >= 0.0;
        }

        /// <summary>
        /// Cross product area of a quadrilateral.
        /// </summary>
        internal static double Area2(Point2d a, Point2d b, Point2d c)
        {
            return (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
        }
    }
}
