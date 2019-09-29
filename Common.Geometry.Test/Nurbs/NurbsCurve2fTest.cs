using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Nurbs;

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

            var knots = new int[]
            {
                0,0,0,0,
                1,1,1,1
            };

            var curve = new NurbsCurve2f(3, control, knots);

            for(float u = 0; u <= 1; u += 0.1f)
            {
                var p = curve.Position(u);
                Console.WriteLine(p);
            }

        }
    }
}
