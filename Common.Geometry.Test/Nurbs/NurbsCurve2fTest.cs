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

            var curve = NurbsCurve2d.FromPoints(3, points);

            Console.WriteLine(curve.PrintControl());
            Console.WriteLine(curve.PrintKnots());
            Console.WriteLine(curve.PrintWeights());

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

            var curve = NurbsCurve2d.FromPoints(3, points);
            var curves = curve.Split(0.4f);

            Console.WriteLine(curves[0].PrintKnots());
            Console.WriteLine(curves[1].PrintKnots());

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

            var curve = NurbsCurve2d.FromPoints(3, points);
            var line = CreatePolyline(curve);

            var len = curve.Length(0.5f);
            var u = curve.ParamAtLength(len);

            Console.WriteLine(u);
            Console.WriteLine(len);
            Console.WriteLine(line.GetLength(0.5f));
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

            var curve = NurbsCurve2d.FromPoints(3, points);
            var samples = curve.DivideByEqualArcLength(20);

            for (int i = 0; i < samples.Count-1; i++)
            {
                var s0 = samples[i];
                var s1 = samples[i+1];

                Console.WriteLine(s0.u + " " + s0.len + " " + (s1.len - s0.len));
            }
        }

        [TestMethod]
        public void Tessellate()
        {
            var points = new Vector2d[]
            {
                new Vector2d(-10,0),
                new Vector2d(10,0),
                new Vector2d(10,10),
                new Vector2d(0,10),
                new Vector2d(5,5)
            };

            var curve = NurbsCurve2d.FromPoints(3, points);

            var tess = curve.Tessellate(100);

            Console.WriteLine(tess.Count);

        }

        private List<Vector2f> CreateLine(NurbsCurve2d curve)
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

        private Polyline2f CreatePolyline(NurbsCurve2d curve)
        {
            var points = CreateLine(curve);
            var line = new Polyline2f(0, points);
            line.Calculate();

            return line;
        }
    }
}
