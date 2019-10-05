using System;
using System.Collections.Generic;
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
        public void Positions()
        {
            var points = new Vector2f[]
            {
                new Vector2f(-10,0),
                new Vector2f(10,0),
                new Vector2f(10,10),
                new Vector2f(0,10),
                new Vector2f(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);

            var positions = new List<Vector2f>();

            for (double t = 0; t <= 1; t += 0.01)
            {
                t = Math.Round(t, 2);
                var p = curve.Position((float)t);
                positions.Add(p);
            }

        }

        [TestMethod]
        public void Split()
        {
            var points = new Vector2f[]
            {
                new Vector2f(-10,0),
                new Vector2f(10,0),
                new Vector2f(10,10),
                new Vector2f(0,10),
                new Vector2f(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);
            var curves = curve.Split(0.4f);

            for (double t = 0; t <= 1; t += 0.01)
            {
                t = Math.Round(t, 2);
                var p = curves[1].Position((float)t);
            }
        }

        [TestMethod]
        public void Length()
        {
            var points = new Vector2f[]
            {
                new Vector2f(-10,0),
                new Vector2f(10,0),
                new Vector2f(10,10),
                new Vector2f(0,10),
                new Vector2f(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);

            var length = curve.Length(0.5f);

            Console.WriteLine(length);
        }

    }
}
