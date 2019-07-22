using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public class Arc3d : NurbsCurve3d
    {
        //Constructor for Arc
        //
        //**params**
        //
        //* Length 3 array representing the center of the arc
        //* Length 3 array representing the xaxis
        //* Length 3 array representing the perpendicular yaxis
        //* Radius of the arc arc
        //* Start angle in radians
        //* End angle in radians

        public Arc3d(Vector3d center, Vector3d xaxis, Vector3d yaxis, double radius, double minAngle, double maxAngle)
        {
            var data = Make.Arc(center, xaxis, yaxis, radius, minAngle, maxAngle);
            m_data = Check.IsValidNurbsCurveData(data);

            Center = center;
            XAxis = xaxis;
            YAxis = yaxis;
            Radius = radius;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
        }

        public Vector3d Center { get; private set; }

        public Vector3d XAxis { get; private set; }

        public Vector3d YAxis { get; private set; }

        public double Radius { get; private set; }

        public double MinAngle { get; private set; }

        public double MaxAngle { get; private set; }
    }
}
