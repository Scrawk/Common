﻿using System;
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
            var points = new Vector3d[]
            {
                new Vector3d(-10,0),
                new Vector3d(10,0),
                new Vector3d(10,10),
                new Vector3d(0,10),
                new Vector3d(5,5)
            };

            var curve = NurbsCurve3d.FromPoints(3, points);

            Console.WriteLine(curve.PrintControl());
            Console.WriteLine(curve.PrintKnots());
            Console.WriteLine(curve.PrintWeights());

        }

        [TestMethod]
        public void Split()
        {
            var points = new Vector3d[]
            {
                new Vector3d(-10,0),
                new Vector3d(10,0),
                new Vector3d(10,10),
                new Vector3d(0,10),
                new Vector3d(5,5)
            };

            var curve = NurbsCurve3d.FromPoints(3, points);
            var curves = curve.Split(0.4f);

            Console.WriteLine(curves[0].PrintKnots());
            Console.WriteLine(curves[1].PrintKnots());

        }

        [TestMethod]
        public void Length()
        {
            var points = new Vector3d[]
            {
                new Vector3d(-10,0),
                new Vector3d(10,0),
                new Vector3d(10,10),
                new Vector3d(0,10),
                new Vector3d(5,5)
            };

            var curve = NurbsCurve3d.FromPoints(3, points);

            var len = curve.Length(0.5f);
            var u = curve.ParamAtLength(len);

            Console.WriteLine(u);
            Console.WriteLine(len);
        }

        [TestMethod]
        public void Divide()
        {
            var points = new Vector3d[]
            {
                new Vector3d(-10,0),
                new Vector3d(10,0),
                new Vector3d(10,10),
                new Vector3d(0,10),
                new Vector3d(5,5)
            };

            var curve = NurbsCurve3d.FromPoints(3, points);
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
            var points = new Vector3d[]
            {
                new Vector3d(-10,0),
                new Vector3d(10,0),
                new Vector3d(10,10),
                new Vector3d(0,10),
                new Vector3d(5,5)
            };

            var curve = NurbsCurve3d.FromPoints(3, points);

            var tess = curve.Tessellate(100);

            Console.WriteLine(tess.Count);

        }

    }
}
