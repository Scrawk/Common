﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Bezier;

namespace Common.Geometry.Test.Bezier
{
    [TestClass]
    public class Bezier2fTest
    {
        const int PRECISION = 6;

        [TestMethod]
        public void ComparePositionToQuadratic()
        {

            QuadraticBezier2f quadratic = new QuadraticBezier2f();
            quadratic.C0 = new Point2f(0.027f, 0.065f);
            quadratic.C1 = new Point2f(1.234f, 0.012f);
            quadratic.C2 = new Point2f(0.816f, 1.298f);

            Bezier2f bezier = new Bezier2f(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            int count = 8;
            for (int i = 0; i < count; i++)
            {
                float t = i / (count - 1.0f);

                Point2f p0 = quadratic.Position(t);
                Point2f p1 = bezier.Point(t);

                Assert.AreEqual(Math.Round(p0.x, PRECISION), Math.Round(p1.x, PRECISION));
                Assert.AreEqual(Math.Round(p0.y, PRECISION), Math.Round(p1.y, PRECISION));
            }
        }

        [TestMethod]
        public void CompareFirstDerivativeToQuadratic()
        {

            QuadraticBezier2f quadratic = new QuadraticBezier2f();
            quadratic.C0 = new Point2f(0.027f, 0.065f);
            quadratic.C1 = new Point2f(1.234f, 0.012f);
            quadratic.C2 = new Point2f(0.816f, 1.298f);

            Bezier2f bezier = new Bezier2f(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            int count = 8;
            for (int i = 0; i < count; i++)
            {
                float t = i / (count - 1.0f);

                Vector2f d0 = quadratic.FirstDerivative(t);
                Vector2f d1 = bezier.FirstDerivative(t);

                Assert.AreEqual(Math.Round(d0.x, PRECISION), Math.Round(d1.x, PRECISION));
                Assert.AreEqual(Math.Round(d0.y, PRECISION), Math.Round(d1.y, PRECISION));
            }
        }

        [TestMethod]
        public void CompareTangentToQuadratic()
        {

            QuadraticBezier2f quadratic = new QuadraticBezier2f();
            quadratic.C0 = new Point2f(0.027f, 0.065f);
            quadratic.C1 = new Point2f(1.234f, 0.012f);
            quadratic.C2 = new Point2f(0.816f, 1.298f);

            Bezier2f bezier = new Bezier2f(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            int count = 8;
            for (int i = 0; i < count; i++)
            {
                float t = i / (count - 1.0f);

                Vector2f t0 = quadratic.Tangent(t);
                Vector2f t1 = bezier.Tangent(t);

                Assert.AreEqual(Math.Round(t0.x, PRECISION), Math.Round(t1.x, PRECISION));
                Assert.AreEqual(Math.Round(t0.y, PRECISION), Math.Round(t1.y, PRECISION));
            }
        }

        [TestMethod]
        public void CompareLengthToQuadratic()
        {

            QuadraticBezier2f quadratic = new QuadraticBezier2f();
            quadratic.C0 = new Point2f(0.027f, 0.065f);
            quadratic.C1 = new Point2f(1.234f, 0.012f);
            quadratic.C2 = new Point2f(0.816f, 1.298f);

            Bezier2f bezier = new Bezier2f(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            float len0 = quadratic.Length;
            float len1 = bezier.EstimateLength(100);

            Assert.AreEqual(Math.Round(len0, 4), Math.Round(len1, 4));

        }

        [TestMethod]
        public void Split()
        {

            Bezier2f bezier = new Bezier2f(3);
            bezier.Control[0] = new Point2f(0.0f, 0.0f);
            bezier.Control[1] = new Point2f(0.0f, 2.5f);
            bezier.Control[2] = new Point2f(2.5f, 5.0f);
            bezier.Control[3] = new Point2f(5, 5);

            float split = 0.5f;

            var pair = bezier.Split(split);
            var b0 = pair.left;
            var b1 = pair.right;

            Assert.AreEqual(bezier.Degree, b0.Degree);
            Assert.AreEqual(bezier.Degree, b1.Degree);

            Assert.AreEqual(bezier.Control[0], b0.Control[0]);
            Assert.AreEqual(bezier.Control[3], b1.Control[3]);

            Point2f p = bezier.Point(split);
            Assert.AreEqual(p, b0.Control[3]);
            Assert.AreEqual(p, b1.Control[0]);

            Assert.AreEqual(Math.Round(bezier.EstimateLength(100), 4), Math.Round(b0.EstimateLength(50) + b1.EstimateLength(50), 4));

        }
    }
}
