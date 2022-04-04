using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Extensions;

using REAL = System.Single;
using VECTOR = Common.Core.Numerics.Vector2f;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Vector2fTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(VECTOR.One.Equals(new VECTOR(1, 1)));
            Assert.IsTrue(VECTOR.One == new VECTOR(1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(VECTOR.One != new VECTOR(2, 2));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(VECTOR.One + VECTOR.One, new VECTOR(2, 2));
            Assert.AreEqual(VECTOR.One + 1, new VECTOR(2, 2));
            Assert.AreEqual(1 + VECTOR.One, new VECTOR(2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(VECTOR.One - VECTOR.One, new VECTOR(0, 0));
            Assert.AreEqual(VECTOR.One - 1, new VECTOR(0, 0));
            Assert.AreEqual(1 - VECTOR.One, new VECTOR(0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(VECTOR.One * new VECTOR(2, 2), new VECTOR(2, 2));
            Assert.AreEqual(VECTOR.One * 2, new VECTOR(2, 2));
            Assert.AreEqual(2 * VECTOR.One, new VECTOR(2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(VECTOR.One / new VECTOR(2, 2), new VECTOR(0.5f, 0.5f));
            Assert.AreEqual(VECTOR.One / 2, new VECTOR(0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(VECTOR.Dot(VECTOR.One, VECTOR.One), 2.0f);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(VECTOR.One.Magnitude, MathUtil.Sqrt(2.0f)));
        }

        [TestMethod]
        public void Absolute()
        {
            VECTOR v = new VECTOR(-1, -2);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
        }

        [TestMethod]
        public void PerpendicularCCW()
        {
            VECTOR v = new VECTOR(0, 1);
            Assert.AreEqual(new VECTOR(-1, 0), v.PerpendicularCCW);

            v = new VECTOR(1, 0);
            Assert.AreEqual(new VECTOR(0, 1), v.PerpendicularCCW);
        }

        [TestMethod]
        public void PerpendicularCW()
        {
            VECTOR v = new VECTOR(0, 1);
            Assert.AreEqual(new VECTOR(1, 0), v.PerpendicularCW);

            v = new VECTOR(1, 0);
            Assert.AreEqual(new VECTOR(0, -1), v.PerpendicularCW);
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(VECTOR.One.SqrMagnitude, 2.0);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random2(0).Normalized.Magnitude, 1.0f));

            VECTOR v = Random2(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1.0f));
        }

        [TestMethod]
        public void Cross()
        {
            VECTOR v01 = new VECTOR(0, 1);
            VECTOR v10 = new VECTOR(1, 0);

            Assert.AreEqual(VECTOR.Cross(v01, v10), -1.0);
        }

        [TestMethod]
        public void Angle180()
        {
            REAL error = 1e-2f;
            VECTOR v = new VECTOR(1, 0);

            for (int i = 0; i <= 360; i++)
            {
                REAL di = i / 360.0f;

                REAL theta = 2.0f * MathUtil.PI_32 * di;
                REAL x = MathUtil.Cos(theta);
                REAL y = MathUtil.Sin(theta);

                int j = (i > 180) ? 360 - i : i;

                Assert.IsTrue(NearlyEqual(j, (REAL)VECTOR.Angle180(v, new VECTOR(x, y)).angle, error));
            }

            v = new VECTOR(1, -1);

            for (int i = 0; i <= 360; i++)
            {
                REAL di = i / 360.0f;

                REAL theta = 2.0f * MathUtil.PI_32 * di;
                REAL x = MathUtil.Cos(theta);
                REAL y = MathUtil.Sin(theta);

                int j = (i + 45) % 360;
                j = (j > 180) ? 360 - j : j;

                Assert.IsTrue(NearlyEqual(j, (REAL)VECTOR.Angle180(v, new VECTOR(x, y)).angle, error));
            }
        }

        [TestMethod]
        public void Angle360()
        {
            REAL error = 1e-4f;
            VECTOR v = new VECTOR(1, 0);

            for (int i = 0; i < 360; i++)
            {
                float di = i / 360.0f;

                REAL theta = 2.0f * MathUtil.PI_32 * di;
                REAL x = MathUtil.Cos(theta);
                REAL y = MathUtil.Sin(theta);

                Assert.IsTrue(NearlyEqual(i, (REAL)VECTOR.Angle360(v, new VECTOR(x, y)).angle, error));
            }

            Assert.IsTrue(NearlyEqual(0, (REAL)VECTOR.Angle360(v, new VECTOR(1, 0)).angle, error));
            Assert.IsTrue(NearlyEqual(45, (REAL)VECTOR.Angle360(v, new VECTOR(1, 1)).angle, error));
            Assert.IsTrue(NearlyEqual(90, (REAL)VECTOR.Angle360(v, new VECTOR(0, 1)).angle, error));
            Assert.IsTrue(NearlyEqual(135, (REAL)VECTOR.Angle360(v, new VECTOR(-1, 1)).angle, error));
            Assert.IsTrue(NearlyEqual(180, (REAL)VECTOR.Angle360(v, new VECTOR(-1, 0)).angle, error));
            Assert.IsTrue(NearlyEqual(225, (REAL)VECTOR.Angle360(v, new VECTOR(-1, -1)).angle, error));
            Assert.IsTrue(NearlyEqual(270, (REAL)VECTOR.Angle360(v, new VECTOR(0, -1)).angle, error));

            v = new VECTOR(-1, 0);

            Assert.IsTrue(NearlyEqual(0, (REAL)VECTOR.Angle360(v, new VECTOR(-1, 0)).angle, error));
            Assert.IsTrue(NearlyEqual(90, (REAL)VECTOR.Angle360(v, new VECTOR(0, -1)).angle, error));
            Assert.IsTrue(NearlyEqual(180, (REAL)VECTOR.Angle360(v, new VECTOR(1, 0)).angle, error));
            Assert.IsTrue(NearlyEqual(270, (REAL)VECTOR.Angle360(v, new VECTOR(0, 1)).angle, error));

            v = new VECTOR(0, 1);

            Assert.IsTrue(NearlyEqual(0, (REAL)VECTOR.Angle360(v, new VECTOR(0, 1)).angle, error));
            Assert.IsTrue(NearlyEqual(90, (REAL)VECTOR.Angle360(v, new VECTOR(-1, 0)).angle, error));
            Assert.IsTrue(NearlyEqual(180, (REAL)VECTOR.Angle360(v, new VECTOR(0, -1)).angle, error));
            Assert.IsTrue(NearlyEqual(270, (REAL)VECTOR.Angle360(v, new VECTOR(1, 0)).angle, error));
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            VECTOR v = new VECTOR();
            v[0] = 1;
            v[1] = 2;

            Assert.AreEqual(v[0], 1);
            Assert.AreEqual(v[1], 2);
        }

        [TestMethod]
        public void Min()
        {
            VECTOR v = VECTOR.One;
            v = VECTOR.Min(v, 0.5f);
            Assert.AreEqual(v, new VECTOR(0.5f, 0.5f));

            v = VECTOR.One;
            v = VECTOR.Min(v, new VECTOR(0.5f, 0.6f));
            Assert.AreEqual(v, new VECTOR(0.5f, 0.6f));
        }

        [TestMethod]
        public void Max()
        {
            VECTOR v = VECTOR.One;
            v = VECTOR.Max(v, 1.5f);
            Assert.AreEqual(v, new VECTOR(1.5f, 1.5f));

            v = VECTOR.One;
            v = VECTOR.Max(v, new VECTOR(1.5f, 1.6f));
            Assert.AreEqual(v, new VECTOR(1.5f, 1.6f));
        }

        [TestMethod]
        public void Clamp()
        {
            VECTOR v = new VECTOR(0.4f, 1.6f);
            v = VECTOR.Clamp(v, 0.5f, 1.5f);
            Assert.AreEqual(v, new VECTOR(0.5f, 1.5f));

            v = new VECTOR(0.4f, 1.6f);
            v = VECTOR.Clamp(v, new VECTOR(0.5f, 1.5f), new VECTOR(0.5f, 1.5f));
            Assert.AreEqual(v, new VECTOR(0.5f, 1.5f));
        }

        [TestMethod]
        public new void ToString()
        {
            VECTOR v = new VECTOR(1, 2);
            Assert.AreEqual("1,2", v.ToString());
        }

        bool NearlyEqual(REAL f1, REAL f2, REAL eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        VECTOR Random2(int seed)
        {
            Random rnd = new Random(seed);
            return new VECTOR(rnd.NextFloat(), rnd.NextFloat());
        }

    }
}
