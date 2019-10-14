using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

    /// <summary>
    /// An EllipseArc is a subset of an Ellipse
    /// </summary>
    public class EllipseArc3d : NurbsCurve3d
    {

        /// <summary>
        /// Create an Circle.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius">the radius</param>
        public EllipseArc3d(Vector3d center, double radius)
                : this(center, radius, radius, 0, Math.PI * 2, Vector3d.UnitX, Vector3d.UnitY)
        {

        }

        /// <summary>
        /// Create an Arc.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius">the radius</param>
        /// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
        /// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
        public EllipseArc3d(Vector3d center, double radius, double minAngle, double maxAngle)
                : this(center, radius, radius, minAngle, maxAngle, Vector3d.UnitX, Vector3d.UnitY)
        {

        }

        /// <summary>
        /// Create an Ellipse.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xradius">the x radius</param>
        /// <param name="yradius">the y radius</param>
        public EllipseArc3d(Vector3d center, double xradius, double yradius)
                : this(center, xradius, yradius, 0, Math.PI * 2, Vector3d.UnitX, Vector3d.UnitY)
        {

        }

        /// <summary>
        /// Create an EllipseArc.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xradius">the x radius</param>
        /// <param name="yradius">the y radius</param>
        /// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
        /// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
        public EllipseArc3d(Vector3d center, double xradius, double yradius, double minAngle, double maxAngle)
                : this(center, xradius, yradius, minAngle, maxAngle, Vector3d.UnitX, Vector3d.UnitY)
        {

        }

        /// <summary>
        /// Create an EllipseArc.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xradius">the x radius</param>
        /// <param name="yradius">the y radius</param>
        /// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
        /// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
        /// <param name="xaxis">the x axis</param>
        /// <param name="yaxis">the y axis</param>
        public EllipseArc3d(Vector3d center, double xradius, double yradius, double minAngle, double maxAngle, Vector3d xaxis, Vector3d yaxis)
                : base(NurbsFunctions.EllipseArc(center, xradius, yradius, minAngle, maxAngle, xaxis, yaxis))
        {
            Center = center;
            XRadius = xradius;
            YRadius = yradius;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
            XAxis = xaxis;
            YAxis = yaxis;
        }

        public Vector3d Center { get; private set; }

        public double XRadius { get; private set; }

        public double YRadius { get; private set; }

        public double MinAngle { get; private set; }

        public double MaxAngle { get; private set; }

        public Vector3d XAxis { get; private set; }

        public Vector3d YAxis { get; private set; }

    }
}