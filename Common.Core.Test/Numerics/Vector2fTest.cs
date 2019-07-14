using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Core_Numerics_Vector2fTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Vector2f.One.Equals(new Vector2f(1, 1)));
            Assert.IsTrue(Vector2f.One == new Vector2f(1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Vector2f.One != new Vector2f(2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random2(0).EqualsWithError(Random2(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(Vector2f.One + Vector2f.One, new Vector2f(2, 2));
            Assert.AreEqual(Vector2f.One + 1, new Vector2f(2, 2));
            Assert.AreEqual(1 + Vector2f.One, new Vector2f(2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(Vector2f.One - Vector2f.One, new Vector2f(0, 0));
            Assert.AreEqual(Vector2f.One - 1, new Vector2f(0, 0));
            Assert.AreEqual(1 - Vector2f.One, new Vector2f(0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(Vector2f.One * new Vector2f(2, 2), new Vector2f(2, 2));
            Assert.AreEqual(Vector2f.One * 2, new Vector2f(2, 2));
            Assert.AreEqual(2 * Vector2f.One, new Vector2f(2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(Vector2f.One / new Vector2f(2, 2), new Vector2f(0.5f, 0.5f));
            Assert.AreEqual(Vector2f.One / 2, new Vector2f(0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(Vector2f.Dot(Vector2f.One, Vector2f.One), 2.0f);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(Vector2f.One.Magnitude, (float)Math.Sqrt(2)));
        }

        [TestMethod]
        public void Absolute()
        {
            Vector2f v = new Vector2f(-1, -2);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
        }

        [TestMethod]
        public void PerpendicularCCW()
        {
            Vector2f v = new Vector2f(0, 1);
            Assert.AreEqual(new Vector2f(-1, 0), v.PerpendicularCCW);

            v = new Vector2f(1, 0);
            Assert.AreEqual(new Vector2f(0, 1), v.PerpendicularCCW);
        }

        [TestMethod]
        public void PerpendicularCW()
        {
            Vector2f v = new Vector2f(0, 1);
            Assert.AreEqual(new Vector2f(1, 0), v.PerpendicularCW);

            v = new Vector2f(1, 0);
            Assert.AreEqual(new Vector2f(0, -1), v.PerpendicularCW);
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(Vector2f.One.SqrMagnitude, 2);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random2(0).Normalized.Magnitude, 1));

            Vector2f v = Random2(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1.0f));
        }

        [TestMethod]
        public void Cross()
        {
            Vector2f v01 = new Vector2f(0, 1);
            Vector2f v10 = new Vector2f(1, 0);

            Assert.AreEqual(Vector2f.Cross(v01, v10), -1.0f);
        }

        [TestMethod]
        public void Distance()
        {
            Vector2f a = new Vector2f(1, 1);
            Vector2f b = new Vector2f(2, 2);

            float len = (float)Math.Sqrt(2);

            Assert.IsTrue(NearlyEqual(len, Vector2f.Distance(a, b)));
        }

        [TestMethod]
        public void SqrDistance()
        {
            Vector2f a = new Vector2f(1, 1);
            Vector2f b = new Vector2f(2, 2);

            float len = (float)Math.Sqrt(2);
            len = len * len;

            Assert.IsTrue(NearlyEqual(len, Vector2f.SqrDistance(a, b)));
        }

        [TestMethod]
        public void Angle180()
        {
            float error = 1e-4f;
            Vector2f v = new Vector2f(1, 0);

            for (int i = 0; i <= 360; i++)
            {
                double di = i / 360.0f;

                double theta = 2.0 * Math.PI * di;
                float x = (float)Math.Cos(theta);
                float y = (float)Math.Sin(theta);

                int j = (i > 180) ? 360 - i : i;

                Assert.IsTrue(NearlyEqual(j, Vector2f.Angle180(v, new Vector2f(x, y)), error));
            }

            v = new Vector2f(1, -1);

            for (int i = 0; i <= 360; i++)
            {
                double di = i / 360.0f;

                double theta = 2.0 * Math.PI * di;
                float x = (float)Math.Cos(theta);
                float y = (float)Math.Sin(theta);

                int j = (i + 45) % 360;
                j = (j > 180) ? 360 - j : j;

                Assert.IsTrue(NearlyEqual(j, Vector2f.Angle180(v, new Vector2f(x, y)), error));
            }

        }

        [TestMethod]
        public void Angle360()
        {
            float error = 1e-4f;
            Vector2f v = new Vector2f(1, 0);

            for (int i = 0; i <= 360; i++)
            {
                double di = i / 360.0f;

                double theta = 2.0 * Math.PI * di;
                float x = (float)Math.Cos(theta);
                float y = (float)Math.Sin(theta);

                Assert.IsTrue(NearlyEqual(i, Vector2f.Angle360(v, new Vector2f(x, y)), error));
            }

            Assert.IsTrue(NearlyEqual(0, Vector2f.Angle360(v, new Vector2f(1, 0)), error));
            Assert.IsTrue(NearlyEqual(45, Vector2f.Angle360(v, new Vector2f(1, 1)), error));
            Assert.IsTrue(NearlyEqual(90, Vector2f.Angle360(v, new Vector2f(0, 1)), error));
            Assert.IsTrue(NearlyEqual(135, Vector2f.Angle360(v, new Vector2f(-1, 1)), error));
            Assert.IsTrue(NearlyEqual(180, Vector2f.Angle360(v, new Vector2f(-1, 0)), error));
            Assert.IsTrue(NearlyEqual(225, Vector2f.Angle360(v, new Vector2f(-1, -1)), error));
            Assert.IsTrue(NearlyEqual(270, Vector2f.Angle360(v, new Vector2f(0, -1)), error));

            v = new Vector2f(-1, 0);

            Assert.IsTrue(NearlyEqual(0, Vector2f.Angle360(v, new Vector2f(-1, 0)), error));
            Assert.IsTrue(NearlyEqual(90, Vector2f.Angle360(v, new Vector2f(0, -1)), error));
            Assert.IsTrue(NearlyEqual(180, Vector2f.Angle360(v, new Vector2f(1, 0)), error));
            Assert.IsTrue(NearlyEqual(270, Vector2f.Angle360(v, new Vector2f(0, 1)), error));

            v = new Vector2f(0, 1);

            Assert.IsTrue(NearlyEqual(0, Vector2f.Angle360(v, new Vector2f(0, 1)), error));
            Assert.IsTrue(NearlyEqual(90, Vector2f.Angle360(v, new Vector2f(-1, 0)), error));
            Assert.IsTrue(NearlyEqual(180, Vector2f.Angle360(v, new Vector2f(0, -1)), error));
            Assert.IsTrue(NearlyEqual(270, Vector2f.Angle360(v, new Vector2f(1, 0)), error));

        }

        [TestMethod]
        public void AccessedByIndex()
        {
            Vector2f v = new Vector2f();
            v[0] = 1;
            v[1] = 2;

            Assert.AreEqual(v[0], 1);
            Assert.AreEqual(v[1], 2);
        }

        [TestMethod]
        public void Min()
        {
            Vector2f v = Vector2f.One;
            v.Min(0.5f);
            Assert.AreEqual(v, new Vector2f(0.5f, 0.5f));

            v = Vector2f.One;
            v.Min(new Vector2f(0.5f, 0.6f));
            Assert.AreEqual(v, new Vector2f(0.5f, 0.6f));
        }

        [TestMethod]
        public void Max()
        {
            Vector2f v = Vector2f.One;
            v.Max(1.5f);
            Assert.AreEqual(v, new Vector2f(1.5f, 1.5f));

            v = Vector2f.One;
            v.Max(new Vector2f(1.5f, 1.6f));
            Assert.AreEqual(v, new Vector2f(1.5f, 1.6f));
        }

        [TestMethod]
        public void Abs()
        {
            Vector2f v = new Vector2f(-1, -2);
            v.Abs();
            Assert.AreEqual(v, new Vector2f(1, 2));
        }

        [TestMethod]
        public void Clamp()
        {
            Vector2f v = new Vector2f(0.4f, 1.6f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new Vector2f(0.5f, 1.5f));

            v = new Vector2f(0.4f, 1.6f);
            v.Clamp(new Vector2f(0.5f, 1.5f), new Vector2f(0.5f, 1.5f));
            Assert.AreEqual(v, new Vector2f(0.5f, 1.5f));
        }

        [TestMethod]
        public new void ToString()
        {
            Vector2f v = new Vector2f(1, 2);
            Assert.AreEqual("1,2", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            Vector2f v = new Vector2f(1, 2);
            Assert.AreEqual(v, Vector2f.FromString("1,2"));
        }

        bool NearlyEqual(float f1, float f2, float eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        Vector2f Random2(int seed)
        {
            Random rnd = new Random(seed);
            return new Vector2f((float)rnd.NextDouble(), (float)rnd.NextDouble());
        }

    }
}
