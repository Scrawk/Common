using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Nurbs
{
    public class NurbsCurveData3d
    {
        public NurbsCurveData3d(int degree, IList<double> knots, IList<Vector4d> controlPoints)
        {
            Degree = degree;
            ControlPoints = new Vector4d[controlPoints.Count];
            controlPoints.CopyTo(ControlPoints, 0);
            Knots = new double[knots.Count];
            knots.CopyTo(Knots, 0);
        }

        public int Degree;

        public Vector4d[] ControlPoints;

        public double[] Knots;
    }
}
