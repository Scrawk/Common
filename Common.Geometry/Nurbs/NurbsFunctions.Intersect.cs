using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    public class CurveCurveIntersection
    {

        //where the intersection took place
        public Vector3d point0;

        //where the intersection took place on the second curve
        public Vector3d point1;

        //the parameter on the first curve
        public double u0;

        //the parameter on the second curve
        public double u1;

        public CurveCurveIntersection(Vector3d point0, Vector3d point1, double u0, double u1)
        {
            this.point0 = point0;
            this.point1 = point1;
            this.u0 = u0;
            this.u1 = u1;
        }
    }

    public static partial class NurbsFunctions
    {
        /// <summary>
        /// Find the closest parameter on two rays, see http://geomalgorithms.com/a07-_distance.html
        /// </summary>
        /// <param name="a0">origin for ray 1</param>
        /// <param name="a">direction of ray 1, assumed normalized</param>
        /// <param name="b0">origin for ray 2</param>
        /// <param name="b">direction of ray 2, assumed normalized</param>
        /// <returns></returns>
        public static CurveCurveIntersection IntersectRays(Vector3d a0, Vector3d a, Vector3d b0, Vector3d b)
        {
            var dab = Vector3d.Dot(a, b);
            var dab0 = Vector3d.Dot(a, b0);
            var daa0 = Vector3d.Dot(a, a0);
            var dbb0 = Vector3d.Dot(b, b0);
            var dba0 = Vector3d.Dot(b, a0);
            var daa = Vector3d.Dot(a, a);
            var dbb = Vector3d.Dot(b, b);
            var div = daa * dbb - dab * dab;

            //parallel case
            if (Math.Abs(div) < FMath.EPS)
                return null;

            var num = dab * (dab0 - daa0) - daa * (dbb0 - dba0);
            var w = num / div;
            var t = (dab0 - daa0 + w * dab) / daa;

            var p0 = a0 + a * t;
            var p1 = b0 + b * w;

            return new CurveCurveIntersection(p0, p1, t, w);
        }

    }
}
