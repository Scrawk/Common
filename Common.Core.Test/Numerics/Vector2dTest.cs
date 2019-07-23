using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Vector2dTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Vector2d.One.Equals(new Vector2d(1, 1)));
            Assert.IsTrue(Vector2d.One == new Vector2d(1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Vector2d.One != new Vector2d(2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random2(0).EqualsWithError(Random2(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(Vector2d.One + Vector2d.One, new Vector2d(2, 2));
            Assert.AreEqual(Vector2d.One + 1, new Vector2d(2, 2));
            Assert.AreEqual(1 + Vector2d.One, new Vector2d(2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(Vector2d.One - Vector2d.One, new Vector2d(0, 0));
            Assert.AreEqual(Vector2d.One - 1, new Vector2d(0, 0));
            Assert.AreEqual(1 - Vector2d.One, new Vector2d(0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(Vector2d.One * new Vector2d(2, 2), new Vector2d(2, 2));
            Assert.AreEqual(Vector2d.One * 2, new Vector2d(2, 2));
            Assert.AreEqual(2 * Vector2d.One, new Vector2d(2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(Vector2d.One / new Vector2d(2, 2), new Vector2d(0.5f, 0.5f));
            Assert.AreEqual(Vector2d.One / 2, new Vector2d(0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(Vector2d.Dot(Vector2d.One, Vector2d.One), 2.0f);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(Vector2d.One.Magnitude, Math.Sqrt(2.0f)));
        }

        [TestMethod]
        public void Absolute()
        {
            Vector2d v = new Vector2d(-1, -2);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
        }

        [TestMethod]
        public void PerpendicularCCW()
        {
            Vector2d v = new Vector2d(0, 1);
            Assert.AreEqual(new Vector2d(-1, 0), v.PerpendicularCCW);

            v = new Vector2d(1, 0);
            Assert.AreEqual(new Vector2d(0, 1), v.PerpendicularCCW);
        }

        [TestMethod]
        public void PerpendicularCW()
        {
            Vector2d v = new Vector2d(0, 1);
            Assert.AreEqual(new Vector2d(1, 0), v.PerpendicularCW);

            v = new Vector2d(1, 0);
            Assert.AreEqual(new Vector2d(0, -1), v.PerpendicularCW);
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(Vector2d.One.SqrMagnitude, 2.0);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random2(0).Normalized.Magnitude, 1.0));

            Vector2d v = Random2(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1.0f));
        }

        [TestMethod]
        public void Cross()
        {
            Vector2d v01 = new Vector2d(0,1);
            Vector2d v10 = new Vector2d(1,0);

            Assert.AreEqual(Vector2d.Cross(v01, v10), -1.0f);
        }

        [TestMethod]
        public void Distance()
        {
            Vector2d a = new Vector2d(1, 1);
            Vector2d b = new Vector2d(2, 2);

            double len = Math.Sqrt(2);

            Assert.IsTrue(NearlyEqual(len, Vector2d.Distance(a, b)));
        }

        [TestMethod]
        public void SqrDistance()
        {
            Vector2d a = new Vector2d(1, 1);
            Vector2d b = new Vector2d(2, 2);

            double len = Math.Sqrt(2);
            len = len * len;

            Assert.IsTrue(NearlyEqual(len, Vector2d.SqrDistance(a, b)));
        }

        [TestMethod]
        public void Angle180()
        {
            double error = 1e-4f;
            Vector2d v = new Vector2d(1, 0);

            for (int i = 0; i <= 360; i++)
            {
                double di = i / 360.0f;

                double theta = 2.0 * Math.PI * di;
                double x = Math.Cos(theta);
                double y = Math.Sin(theta);

                int j = (i > 180) ? 360 - i : i;

                Assert.IsTrue(NearlyEqual(j, Vector2d.Angle180(v, new Vector2d(x, y)), error));
            }

            v = new Vector2d(1, -1);

            for (int i = 0; i <= 360; i++)
            {
                double di = i / 360.0f;

                double theta = 2.0 * Math.PI * di;
                double x = Math.Cos(theta);
                double y = Math.Sin(theta);

                int j = (i + 45) % 360;
                j = (j > 180) ? 360 - j : j;

                Assert.IsTrue(NearlyEqual(j, Vector2d.Angle180(v, new Vector2d(x, y)), error));
            }

        }

        [TestMethod]
        public void Angle360()
        {
            double error = 1e-4f;
            Vector2d v = new Vector2d(1, 0);

            for (int i = 0; i <= 360; i++)
            {
                float di = i / 360.0f;

                double theta = 2.0 * Math.PI * di;
                double x = Math.Cos(theta);
                double y = Math.Sin(theta);

                Assert.IsTrue(NearlyEqual(i, Vector2d.Angle360(v, new Vector2d(x, y)), error));
            }

            Assert.IsTrue(NearlyEqual(0, Vector2d.Angle360(v, new Vector2d(1, 0)), error));
            Assert.IsTrue(NearlyEqual(45, Vector2d.Angle360(v, new Vector2d(1, 1)), error));
            Assert.IsTrue(NearlyEqual(90, Vector2d.Angle360(v, new Vector2d(0, 1)), error));
            Assert.IsTrue(NearlyEqual(135, Vector2d.Angle360(v, new Vector2d(-1, 1)), error));
            Assert.IsTrue(NearlyEqual(180, Vector2d.Angle360(v, new Vector2d(-1, 0)), error));
            Assert.IsTrue(NearlyEqual(225, Vector2d.Angle360(v, new Vector2d(-1, -1)), error));
            Assert.IsTrue(NearlyEqual(270, Vector2d.Angle360(v, new Vector2d(0, -1)), error));

            v = new Vector2d(-1, 0);

            Assert.IsTrue(NearlyEqual(0, Vector2d.Angle360(v, new Vector2d(-1, 0)), error));
            Assert.IsTrue(NearlyEqual(90, Vector2d.Angle360(v, new Vector2d(0, -1)), error));
            Assert.IsTrue(NearlyEqual(180, Vector2d.Angle360(v, new Vector2d(1, 0)), error));
            Assert.IsTrue(NearlyEqual(270, Vector2d.Angle360(v, new Vector2d(0, 1)), error));

            v = new Vector2d(0, 1);

            Assert.IsTrue(NearlyEqual(0, Vector2d.Angle360(v, new Vector2d(0, 1)), error));
            Assert.IsTrue(NearlyEqual(90, Vector2d.Angle360(v, new Vector2d(-1, 0)), error));
            Assert.IsTrue(NearlyEqual(180, Vector2d.Angle360(v, new Vector2d(0, -1)), error));
            Assert.IsTrue(NearlyEqual(270, Vector2d.Angle360(v, new Vector2d(1, 0)), error));

        }

        [TestMethod]
        public void AccessedByIndex()
        {
            Vector2d v = new Vector2d();
            v[0] = 1;
            v[1] = 2;

            Assert.AreEqual(v[0], 1);
            Assert.AreEqual(v[1], 2);
        }

        [TestMethod]
        public void Min()
        {
            Vector2d v = Vector2d.One;
            v = Vector2d.Min(v, 0.5f);
            Assert.AreEqual(v, new Vector2d(0.5f, 0.5f));

            v = Vector2d.One;
            v = Vector2d.Min(v, new Vector2d(0.5f, 0.6f));
            Assert.AreEqual(v, new Vector2d(0.5f, 0.6f));
        }

        [TestMethod]
        public void Max()
        {
            Vector2d v = Vector2d.One;
            v = Vector2d.Max(v, 1.5f);
            Assert.AreEqual(v, new Vector2d(1.5f, 1.5f));

            v = Vector2d.One;
            v = Vector2d.Max(v, new Vector2d(1.5f, 1.6f));
            Assert.AreEqual(v, new Vector2d(1.5f, 1.6f));
        }

        [TestMethod]
        public void Abs()
        {
            Vector2d v = new Vector2d(-1, -2);
            v.Abs();
            Assert.AreEqual(v, new Vector2d(1, 2));
        }

        [TestMethod]
        public void Clamp()
        {
            Vector2d v = new Vector2d(0.4f, 1.6f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new Vector2d(0.5f, 1.5f));

            v = new Vector2d(0.4f, 1.6f);
            v.Clamp(new Vector2d(0.5f, 1.5f), new Vector2d(0.5f, 1.5f));
            Assert.AreEqual(v, new Vector2d(0.5f, 1.5f));
        }

        [TestMethod]
        public new void ToString()
        {
            Vector2d v = new Vector2d(1, 2);
            Assert.AreEqual("1,2", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            Vector2d v = new Vector2d(1, 2);
            Assert.AreEqual(v, Vector2d.FromString("1,2"));
        }

        bool NearlyEqual(double f1, double f2, double eps = 1e-6)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        Vector2d Random2(int seed)
        {
            Random rnd = new Random(seed);
            return new Vector2d(rnd.NextDouble(), rnd.NextDouble());
        }

    }
}
