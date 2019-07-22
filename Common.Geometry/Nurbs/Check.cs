using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Nurbs
{
    public static class Check
    {

        public static bool IsValidKnotVector(double[] vec, int degree)
        {
            if (vec.Length < (degree + 1) * 2)
                return false;

            double rep = vec.First();
            for (int i = 0; i < degree + 1; i++)
            {
                if (Math.Abs(vec[i] - rep) > DMath.EPS) return false;
            }

            rep = vec.Last();
            for (int i = vec.Length - degree - 1; i < vec.Length; i++)
            {
                if (Math.Abs(vec[i] - rep) > DMath.EPS) return false;
            }

            return IsNonDecreasing(vec);
        }

        public static bool IsNonDecreasing(double[] vec)
        {
            double rep = vec.First();
            for (int i = 0; i < vec.Length; i++)
            {
                if (vec[i] < rep - DMath.EPS) return false;
                rep = vec[i];
            }

            return true;
        }

        public static NurbsCurveData3d IsValidNurbsCurveData(NurbsCurveData3d data)
        {
            if (data.ControlPoints == null)
                throw new ArgumentNullException("Control points array cannot be null!");

            if (data.Degree < 1)
                throw new ArgumentException("Degree must be greater than 1!");

            if (data.Knots == null)
                throw new ArgumentNullException("Knots cannot be null!");

            if (data.Knots.Length != data.ControlPoints.Length + data.Degree + 1)
                throw new ArgumentException("controlPoints.length + degree + 1 must equal knots.length!");

            if (!IsValidKnotVector(data.Knots, data.Degree))
                throw new ArgumentException("Invalid knot vector format!  Should begin with degree + 1 repeats and end with degree + 1 repeats!");

            return data;
        }

        public static NurbsSurfaceData3d IsValidNurbsSurfaceData(NurbsSurfaceData3d data)
        {
            if (data.ControlPoints == null)
                throw new ArgumentNullException("Control points array cannot be null!");

            if (data.DegreeU < 1)
                throw new ArgumentException("DegreeU must be greater than 1!");

            if (data.DegreeV < 1)
                throw new ArgumentException("DegreeV must be greater than 1!");

            if (data.KnotsU.Length != data.ControlPoints.Length + data.DegreeU + 1)
                throw new ArgumentException("controlPointsU.length + degreeU + 1 must equal knotsU.length!");

            if (data.KnotsV.Length != 3 + data.DegreeV + 1)
                throw new ArgumentException("controlPointsV.length + degreeV + 1 must equal knotsV.length!");

            if (!IsValidKnotVector(data.KnotsU, data.DegreeU) || !IsValidKnotVector(data.KnotsV, data.DegreeV))
                throw new ArgumentException("Invalid knot vector format!  Should begin with degree + 1 repeats and end with degree + 1 repeats!");

            return data;
        }

    }
}


