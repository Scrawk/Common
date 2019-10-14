using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public static partial class NurbsFunctions
    {
        /// <summary>
        /// 0) build knot vector for curve by normalized chord length
        /// 1) construct effective basis function in square matrix (W)
        /// 2) construct set of coordinattes to interpolate vector (p)
        /// 3) set of control points (c)
        /// 4) solve for c in all 3 dimensions
        /// </summary>
        /// <param name="degree">The degree of the curve.</param>
        /// <param name="points">The points to interp curve from.</param>
        /// <returns></returns>
        public static NurbsCurveData3d RationalInterpolate(int degree, IList<Vector3d> points)
        {
            if (points.Count < degree + 1)
                throw new ArgumentException("You need to supply at least degree + 1 points.");

            var us = new List<double>();
            us.Add(0);

            for (int i = 1; i < points.Count; i++)
            {
                double chord = (points[i] - points[i - 1]).Magnitude;
                double last = us[us.Count - 1];
                us.Add(last + chord);
            }

            //normalize
            var max = us[us.Count - 1];
            for (int i = 1; i < us.Count; i++)
                us[i] = us[i] / max;

            var knotsStart = new List<double>(degree);
            knotsStart.AddRange(degree + 1, 0);

            //we need two more control points, two more knots
            var start = 1;
            var end = us.Count - degree;

            for (int i = start; i < end; i++)
            {
                double weightSums = 0.0;
                for (int j = 0; j < degree; j++)
                {
                    weightSums += us[i + j];
                }

                knotsStart.Add((1.0 / degree) * weightSums);
            }

            var knots = new List<double>(knotsStart);
            knots.AddRange(degree + 1, 1.0f);

            //build matrix of basis function coeffs (TODO: use sparse rep)
            var A = new List<List<double>>();
            var n = points.Count - 1;
            var ld = points.Count - (degree + 1);

            foreach (var u in us)
            {
                int span = FindSpan(u, degree, n, knots);
                var basisFuncs = BasisFunctions(u, degree, span, knots);

                int ls = span - degree;

                var row = new List<double>();
                row.AddRange(ls, 0);
                row.AddRange(basisFuncs);
                row.AddRange(ld - ls, 0);

                A.Add(row);
            }

            //for each dimension, solve
            var rows = 3;
            var columns = points.Count;
            var xs = new Matrix(rows, columns);
            var M = new Matrix(A);

            for (int i = 0; i < rows; i++)
            {
                var b = new Vector(columns);

                int j = 0;
                foreach (var p in points)
                    b[j++] = p[i];

                var x = Matrix.Solve(M, b);
                xs.SetRow(i, x);
            }

            var controlPts = new List<Vector3d>();
            for (int i = 0; i < columns; i++)
            {
                var v = new Vector3d(xs[0, i], xs[1, i], xs[2, i]);
                controlPts.Add(v);
            }

            return new NurbsCurveData3d(degree, controlPts, knots, null);
        }

        /// <summary>
        /// Generate the control points, weights, and knots for a bezier curve of any degree.
        /// </summary>
        /// <param name="controlPoints">Points in counter-clockwise form.</param>
        /// <returns></returns>
    	public static NurbsCurveData3d RationalBezierCurve(IList<Vector3d> controlPoints, IList<double> weights = null)
        {
            int count = controlPoints.Count;
            int degree = count - 1;

            var knots = new List<double>(count * 2);
            knots.AddRange(degree + 1, 0);
            knots.AddRange(degree + 1, 1);

            //if weights aren't provided, build uniform weights
            if (weights == null)
            {
                var w = new List<double>(count);
                w.AddRange(count, 1);
                weights = w;
            }

            return new NurbsCurveData3d(degree, controlPoints, knots, weights);
        }

        /// <summary>
        /// Generate the control points, weights, and knots of an elliptical arc.
        /// (Corresponds to Algorithm A7.1 from Piegl & Tiller)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="xradius">the x radius</param>
        /// <param name="yradius">the y radius</param>
        /// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
        /// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
        /// <param name="xaxis">the x axis</param>
        /// <param name="yaxis">the y axis</param>
        /// <returns></returns>
        public static NurbsCurveData3d EllipseArc(Vector3d center, double xradius, double yradius, double startAngle, double endAngle, Vector3d xaxis, Vector3d yaxis)
        {
            xaxis = xaxis.Normalized;
            yaxis = yaxis.Normalized;

            //if the end angle is less than the start angle, do a circle
            if (endAngle < startAngle)
                endAngle = 2.0 * Math.PI + startAngle;

            double theta = endAngle - startAngle;
            int numArcs = 0;

            //how many arcs?
            if (theta <= Math.PI / 2.0)
                numArcs = 1;
            else
            {
                if (theta <= Math.PI)
                    numArcs = 2;
                else if (theta <= 3 * Math.PI / 2.0)
                    numArcs = 3;
                else
                    numArcs = 4;
            }

            double dtheta = theta / numArcs;
            int n = 2 * numArcs;
            double w1 = Math.Cos(dtheta / 2.0);

            var P0 = center + (xradius * Math.Cos(startAngle) * xaxis) + (yradius * Math.Sin(startAngle) * yaxis);
            var T0 = (Math.Cos(startAngle) * yaxis) - (Math.Sin(startAngle) * xaxis);

            var controlPoints = new List<Vector3d>(numArcs * 2 + 1);
            controlPoints.AddRange(numArcs * 2 + 1, Vector3d.Zero);

            var knots = new List<double>(2 * numArcs + 3 + 1);
            knots.AddRange(2 * numArcs + 3 + 1, 0);

            int index = 0;
            double angle = startAngle;

            var weights = new List<double>(numArcs * 2 + 1);
            weights.AddRange(2 * numArcs + 1, 0);

            controlPoints[0] = P0;
            weights[0] = 1.0;

            for (int i = 1; i < numArcs + 1; i++)
            {
                angle += dtheta;
                var P2 = center + (xradius * Math.Cos(angle) * xaxis) + (yradius * Math.Sin(angle) * yaxis);

                weights[index + 2] = 1;
                controlPoints[index + 2] = P2;

                var T2 = (Math.Cos(angle) * yaxis) - (Math.Sin(angle) * xaxis);

                var inters = IntersectRays(P0, (1.0 / T0.Magnitude) * T0, P2, (1.0 / T2.Magnitude) * T2);

                var P1 = P0 + (inters.u0 * T0);

                weights[index + 1] = w1;
                controlPoints[index + 1] = P1;

                index += 2;

                if (i < numArcs)
                {
                    P0 = P2;
                    T0 = T2;
                }
            }

            int j = 2 * numArcs + 1;

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
                    knots[3] = knots[4] = 1.0 / 3.0;
                    knots[5] = knots[6] = 2.0 / 3.0;
                    break;
                case 4:
                    knots[3] = knots[4] = 0.25;
                    knots[5] = knots[6] = 0.5;
                    knots[7] = knots[8] = 0.75;
                    break;
            }

            return new NurbsCurveData3d(2, controlPoints, knots, weights);
        }
    }
}








