using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Nurbs
{
    public class NurbsSurfaceData3d
    {

        public int DegreeU;

        public int DegreeV;

        public double[] KnotsU;

        public double[] KnotsV;

        public Vector4d[] ControlPoints;
    }
}
