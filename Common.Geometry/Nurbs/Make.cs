using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static class Make
    {
        //Generate the control points, weights, and knots of an elliptical arc
        //
        //**params**
        //
        //* the center
        //* the scaled x axis
        //* the scaled y axis
        //* start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis
        //* end angle of the arc, between 0 and 2pi, greater than the start angle
        //
        //**returns**
        //
        //* a NurbsCurveData object representing a NURBS curve

        public static NurbsCurveData3d EllipseArc(Vector3d center, Vector3d xaxis, Vector3d yaxis, double startAngle, double endAngle)
        {
            var xradius = xaxis.Magnitude;
            var yradius = yaxis.Magnitude;

            xaxis.Normalize();
            yaxis.Normalize();

            //if the end angle is less than the start angle, do a circle
            if (endAngle < startAngle) endAngle = 2.0 * Math.PI + startAngle;

            double theta = endAngle - startAngle;
            int numArcs = 0;

            //how many arcs?
            if (theta <= Math.PI / 2)
                numArcs = 1;
            else
            {
                if (theta <= Math.PI)
                    numArcs = 2;
                else if (theta <= 3 * Math.PI / 2)
                    numArcs = 3;
                else
                    numArcs = 4;
            }

            var dtheta = theta / numArcs;
            var n = 2 * numArcs;
            var w1 = Math.Cos(dtheta / 2);

            var P0 = center + xradius * Math.Cos(startAngle) * xaxis + yradius * Math.Sin(startAngle) * yaxis;
            var T0 = Math.Cos(startAngle) * yaxis - Math.Sin(startAngle) * xaxis;

            var index = 0;
            var angle = startAngle;

            var controlPoints = new Vector3d[n + 1];
            var knots = new double[n + 4];
            var weights = new double[n + 1];

            controlPoints[0] = P0;
            weights[0] = 1.0;

            for (int i = 1; i < numArcs + 1; i++)
            {
                angle += dtheta;
                var P2 = center + xradius * Math.Cos(angle) * xaxis + yradius * Math.Sin(angle) * yaxis;

                weights[index + 2] = 1;
                controlPoints[index + 2] = P2;

                var T2 = Math.Cos(angle) * yaxis - Math.Sin(angle) * xaxis;
                var u0 = Rays(P0, 1 / T0.Magnitude * T0, P2, 1 / T2.Magnitude * T2);
                var P1 = P0 + u0 * T0;

                weights[index + 1] = w1;
                controlPoints[index + 1] = P1;

                index += 2;

                if (i < numArcs)
                {
                    P0 = P2;
                    T0 = T2;
                }
            }

            var j = 2 * numArcs + 1;

            for (int i = 0; i < 3; i++)
            {
                knots[i] = 0.0;
                knots[i + j] = 1.0;
            }

            switch (numArcs)
            {
                case 2:
                    knots[3] = knots[4] = 0.5;
                    break;
                case 3:
                    knots[3] = knots[4] = 1 / 3.0;
                    knots[5] = knots[6] = 2 / 3.0;
                    break;
                case 4:
                    knots[3] = knots[4] = 0.25;
                    knots[5] = knots[6] = 0.5;
                    knots[7] = knots[8] = 0.75;
                    break;
            }

            return new NurbsCurveData3d(2, knots, Eval.Homogenize1d(controlPoints, weights));
        }

        //Generate the control points, weights, and knots of an arbitrary arc
        // (Corresponds to Algorithm A7.1 from Piegl & Tiller)
        //
        //**params**
        //
        //* the center of the arc
        //* the xaxis of the arc
        //* orthogonal yaxis of the arc
        //* radius of the arc
        //* start angle of the arc, between 0 and 2pi
        //* end angle of the arc, between 0 and 2pi, greater than the start angle
        //
        //**returns**
        //
        //* a NurbsCurveData object representing a NURBS curve

        public static NurbsCurveData3d Arc(Vector3d center, Vector3d xaxis, Vector3d yaxis, double radius, double startAngle, double endAngle)
        {
            return EllipseArc(center, radius * xaxis.Normalized, radius * yaxis.Normalized, startAngle, endAngle);
        }

        //Find the closest parameter on two rays, see http://geomalgorithms.com/a07-_distance.html
        //
        //**params**
        //
        //* origin for ray 1
        //* direction of ray 1, assumed normalized
        //* origin for ray 1
        //* direction of ray 1, assumed normalized
        //
        //**returns**
        //
        //* a CurveCurveIntersection object

        public static Vector3d Rays(Vector3d a0, Vector3d a, Vector3d b0, Vector3d b)
        {
            var dab = Vector3d.Dot(a, b);
            var dab0 = Vector3d.Dot(a, b0);
            var daa0 = Vector3d.Dot(a, a0);
            var dbb0 = Vector3d.Dot(b, b0);
            var dba0 = Vector3d.Dot(b, a0);
            var daa = Vector3d.Dot(a, a);
            var dbb = Vector3d.Dot(b, b);
            var div = daa * dbb - dab * dab;

            if (Math.Abs(div) < DMath.EPS)
                throw new Exception("Rays do not intersect");

            var num = dab * (dab0 - daa0) - daa * (dbb0 - dba0);
            var w = num / div;
            var t = (dab0 - daa0 + w * dab) / daa;

            var p0 = a0 + a * t;
            var p1 = b0 + b * w;

            return p0;
        }
}
}
