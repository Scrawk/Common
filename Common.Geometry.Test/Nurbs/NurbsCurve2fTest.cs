using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Nurbs;
using Common.Geometry.Bezier;
using Common.Geometry.Polygons;

namespace Common.Geometry.Test.Nurbs
{
    [TestClass]
    public class NurbsCurve2dTest
    {
        [TestMethod]
        public void Positions()
        {
            var points = new Vector2d[]
            {
                new Vector2d(-10,0),
                new Vector2d(10,0),
                new Vector2d(10,10),
                new Vector2d(0,10),
                new Vector2d(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);

            var positions = new List<Vector2d>();

            for (double t = 0; t <= 1; t += 0.01)
            {
                t = Math.Round(t, 2);
                var p = curve.Position((double)t);
                positions.Add(p);
            }

        }

        [TestMethod]
        public void Split()
        {
            var points = new Vector2d[]
            {
                new Vector2d(-10,0),
                new Vector2d(10,0),
                new Vector2d(10,10),
                new Vector2d(0,10),
                new Vector2d(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);
            var curves = curve.Split(0.4f);

            for (double t = 0; t <= 1; t += 0.01)
            {
                t = Math.Round(t, 2);
                var p = curves[1].Position(t);
            }
        }

        [TestMethod]
        public void Length()
        {
            var points = new Vector2d[]
            {
                new Vector2d(-10,0),
                new Vector2d(10,0),
                new Vector2d(10,10),
                new Vector2d(0,10),
                new Vector2d(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);
            var line = CreatePolyline(curve);

            var len = curve.Length(0.5f);
            var u = curve.ParamAtLength(len);

            Console.WriteLine(u);
            Console.WriteLine(len);
        }

        [TestMethod]
        public void Divide()
        {
            var points = new Vector2d[]
            {
                new Vector2d(-10,0),
                new Vector2d(10,0),
                new Vector2d(10,10),
                new Vector2d(0,10),
                new Vector2d(5,5)
            };

            var curve = NurbsCurve2f.FromPoints(3, points);

            var samples = curve.DivideByEqualArcLength(20);

            Console.WriteLine(samples.Count);

            foreach (var sample in samples)
            {
                Console.WriteLine(sample.u + " " + sample.len);
            }
        }

        private List<Vector2f> CreateLine(NurbsCurve2f curve)
        {
            var points = new List<Vector2f>();

            for (double t = 0; t <= 1; t += 0.01)
            {
                t = Math.Round(t, 2);
                var p = curve.Position(t);
                points.Add((Vector2f)p);
            }

            return points;
        }

        private Polyline2f CreatePolyline(NurbsCurve2f curve)
        {
            var points = CreateLine(curve);
            var line = new Polyline2f(0, points);
            line.Calculate();

            return line;
        }
    }
}
