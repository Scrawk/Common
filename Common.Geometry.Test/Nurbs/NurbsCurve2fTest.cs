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

            var control = new Vector2f[]
            {
                new Vector2f(0,0),
                new Vector2f(0,1),
                new Vector2f(1,1),
                new Vector2f(1,0)
            };

            var knots = new float[]
            {
                0,0,0,0,
                1,1,1,1
            };

            var weights = new float[]
            {
                1,1,1,1
            };

            int degree = 3;

            Console.WriteLine("Required knots = " + NurbsFunctions.RequiredKnots(degree, control.Length));

            var bezier = new Bezier2f(control);
            var nurbs = new NurbsCurve2f(degree, control, knots);

            for (double u = 0; u <= 1; u += 0.1f)
            {
                float t = (float)u;
                Vector2d p, d;

                //p = bezier.Position(t);
                //d = bezier.Tangent(t);
                //Console.WriteLine("Bezier = " + p + " " + d);

                p = nurbs.Position(t);
                d = nurbs.Derivatives(t, 1)[1];
                p.Round(2);
                d.Round(2);
                Console.WriteLine("nurbs = " + p + " " + d);
            }

        }
    }
}
