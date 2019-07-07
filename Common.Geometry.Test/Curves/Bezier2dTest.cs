using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Curves;

namespace Common.Geometry.Test.Curves
{
    [TestClass]
    public class Geometry_Curves_Bezier2dTest
    {
        const int PRECISION = 9;

        [TestMethod]
        public void ComparePositionToQuadratic()
        {

            QuadraticBezier2d quadratic = new QuadraticBezier2d();
            quadratic.C0 = new Vector2d(0.027, 0.065);
            quadratic.C1 = new Vector2d(1.234, 0.012);
            quadratic.C2 = new Vector2d(0.816, 1.298);

            Bezier2d bezier = new Bezier2d(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            int count = 8;
            for (int i = 0; i < count; i++)
            {
                double t = i / (count - 1.0);

                Vector2d p0 = quadratic.Position(t);
                Vector2d p1 = bezier.Position(t);

                Assert.AreEqual(Math.Round(p0.x, PRECISION), Math.Round(p1.x, PRECISION));
                Assert.AreEqual(Math.Round(p0.y, PRECISION), Math.Round(p1.y, PRECISION));
            }
        }

        [TestMethod]
        public void CompareFirstDerivativeToQuadratic()
        {

            QuadraticBezier2d quadratic = new QuadraticBezier2d();
            quadratic.C0 = new Vector2d(0.027, 0.065);
            quadratic.C1 = new Vector2d(1.234, 0.012);
            quadratic.C2 = new Vector2d(0.816, 1.298);

            Bezier2d bezier = new Bezier2d(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            int count = 8;
            for (int i = 0; i < count; i++)
            {
                double t = i / (count - 1.0);

                Vector2d d0 = quadratic.FirstDerivative(t);
                Vector2d d1 = bezier.FirstDerivative(t);

                Assert.AreEqual(Math.Round(d0.x, PRECISION), Math.Round(d1.x, PRECISION));
                Assert.AreEqual(Math.Round(d0.y, PRECISION), Math.Round(d1.y, PRECISION));
            }
        }

        [TestMethod]
        public void CompareTangentToQuadratic()
        {

            QuadraticBezier2d quadratic = new QuadraticBezier2d();
            quadratic.C0 = new Vector2d(0.027, 0.065);
            quadratic.C1 = new Vector2d(1.234, 0.012);
            quadratic.C2 = new Vector2d(0.816, 1.298);

            Bezier2d bezier = new Bezier2d(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            int count = 8;
            for (int i = 0; i < count; i++)
            {
                double t = i / (count - 1.0);

                Vector2d t0 = quadratic.Tangent(t);
                Vector2d t1 = bezier.Tangent(t);

                Assert.AreEqual(Math.Round(t0.x, PRECISION), Math.Round(t1.x, PRECISION));
                Assert.AreEqual(Math.Round(t0.y, PRECISION), Math.Round(t1.y, PRECISION));
            }
        }

        [TestMethod]
        public void CompareLengthToQuadratic()
        {

            QuadraticBezier2d quadratic = new QuadraticBezier2d();
            quadratic.C0 = new Vector2d(0.027, 0.065);
            quadratic.C1 = new Vector2d(1.234, 0.012);
            quadratic.C2 = new Vector2d(0.816, 1.298);

            Bezier2d bezier = new Bezier2d(2);
            bezier.Control[0] = quadratic.C0;
            bezier.Control[1] = quadratic.C1;
            bezier.Control[2] = quadratic.C2;

            double len0 = quadratic.Length;
            double len1 = bezier.Length(100);

            Assert.AreEqual(Math.Round(len0, 4), Math.Round(len1, 4));

        }

        [TestMethod]
        public void Split()
        {

            Bezier2d bezier = new Bezier2d(3);
            bezier.Control[0] = new Vector2d(0.0f, 0.0);
            bezier.Control[1] = new Vector2d(0.0f, 2.5);
            bezier.Control[2] = new Vector2d(2.5, 5.0);
            bezier.Control[3] = new Vector2d(5, 5);

            double split = 0.5;

            Bezier2d b0, b1;
            bezier.Split(split, out b0, out b1);

            Assert.AreEqual(bezier.Degree, b0.Degree);
            Assert.AreEqual(bezier.Degree, b1.Degree);

            Assert.AreEqual(bezier.Control[0], b0.Control[0]);
            Assert.AreEqual(bezier.Control[3], b1.Control[3]);

            Vector2d p = bezier.Position(split);
            Assert.AreEqual(p, b0.Control[3]);
            Assert.AreEqual(p, b1.Control[0]);

            Assert.AreEqual(Math.Round(bezier.Length(100), 4), Math.Round(b0.Length(50) + b1.Length(50), 4));

        }
    }
}
