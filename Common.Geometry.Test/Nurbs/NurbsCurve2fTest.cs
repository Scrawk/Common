using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Nurbs;
using Common.Geometry.Bezier;

namespace Common.Geometry.Test.Nurbs
{
    [TestClass]
    public class NurbsCurve2fTest
    {
        [TestMethod]
        public void Constructor()
        {

            var control2 = new Vector2f[]
            {
                new Vector2f(0,0),
                new Vector2f(0,1),
                new Vector2f(1,1),
                new Vector2f(1,0)
            };

            var control3 = new Vector3f[]
            {
                new Vector3f(0,0,1),
                new Vector3f(0,1,1),
                new Vector3f(1,1,1),
                new Vector3f(1,0,1)
            };

            var knots = new int[]
            {
                0,0,0,0,
                1,1,1,1
            };

            int degree = 3;

            Console.WriteLine("Required knots = " + NurbsFunctions.RequiredKnots(degree, control2.Length));

            var bezier = new Bezier2f(control2);
            var spline = new BSplineCurve2f(degree, control2, knots);
            var nurbs = new NurbsCurve2f(degree, control3, knots);

            for (double u = 0; u <= 1; u += 0.1f)
            {
                float t = (float)u;
                Vector2f p, d;

                p = bezier.Position(t);
                d = bezier.Tangent(t);
                Console.WriteLine("Bezier = " + p + " " + d);

                p = spline.Position(t);
                d = spline.Tangent(t);
                Console.WriteLine("Spline = " + p + " " + d);

                p = nurbs.Position(t);
                d = nurbs.Tangent(t);
                Console.WriteLine("nurbs = " + p + " " + d);
            }

        }
    }
}
