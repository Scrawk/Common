using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using REAL = System.Single;
using VECTOR = Common.Core.Numerics.Vector3f;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Vector3fTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(VECTOR.One.Equals(new VECTOR(1, 1, 1)));
            Assert.IsTrue(VECTOR.One == new VECTOR(1, 1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(VECTOR.One != new VECTOR(2, 2, 2));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(VECTOR.One + VECTOR.One, new VECTOR(2, 2, 2));
            Assert.AreEqual(VECTOR.One + 1, new VECTOR(2, 2, 2));
            Assert.AreEqual(1 + VECTOR.One, new VECTOR(2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(VECTOR.One - VECTOR.One, new VECTOR(0, 0, 0));
            Assert.AreEqual(VECTOR.One - 1, new VECTOR(0, 0, 0));
            Assert.AreEqual(1 - VECTOR.One, new VECTOR(0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(VECTOR.One * new VECTOR(2, 2, 2), new VECTOR(2, 2, 2));
            Assert.AreEqual(VECTOR.One * 2, new VECTOR(2, 2, 2));
            Assert.AreEqual(2 * VECTOR.One, new VECTOR(2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(VECTOR.One / new VECTOR(2, 2, 2), new VECTOR(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(VECTOR.One / 2, new VECTOR(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(VECTOR.Dot(VECTOR.One, VECTOR.One), 3);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(VECTOR.One.Magnitude, MathUtil.Sqrt(3)));
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(VECTOR.One.SqrMagnitude, 3);
        }

        [TestMethod]
        public void Absolute()
        {
            VECTOR v = new VECTOR(-1, -2, -3);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
            Assert.AreEqual(3, v.Absolute.z);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random3(0).Normalized.Magnitude, 1));

            VECTOR v = Random3(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1));
        }

        [TestMethod]
        public void Angle180()
        {
            REAL error = 1e-4f;
            VECTOR v = new VECTOR(1, 0, 0);

            Assert.IsTrue(NearlyEqual(0, VECTOR.Angle180(v, new VECTOR(1, 0, 0)), error));
            Assert.IsTrue(NearlyEqual(45, VECTOR.Angle180(v, new VECTOR(1, 1, 0)), error));
            Assert.IsTrue(NearlyEqual(90, VECTOR.Angle180(v, new VECTOR(0, 1, 0)), error));
            Assert.IsTrue(NearlyEqual(135, VECTOR.Angle180(v, new VECTOR(-1, 1, 0)), error));
            Assert.IsTrue(NearlyEqual(180, VECTOR.Angle180(v, new VECTOR(-1, 0, 0)), error));
            Assert.IsTrue(NearlyEqual(135, VECTOR.Angle180(v, new VECTOR(-1, -1, 0)), error));
            Assert.IsTrue(NearlyEqual(90, VECTOR.Angle180(v, new VECTOR(0, -1, 0)), error));
            Assert.IsTrue(NearlyEqual(45, VECTOR.Angle180(v, new VECTOR(1, -1, 0)), error));

            v = new VECTOR(0, 1, 0);

            Assert.IsTrue(NearlyEqual(0, VECTOR.Angle180(v, new VECTOR(0, 1, 0)), error));
            Assert.IsTrue(NearlyEqual(45, VECTOR.Angle180(v, new VECTOR(-1, 1, 0)), error));
            Assert.IsTrue(NearlyEqual(90, VECTOR.Angle180(v, new VECTOR(1, 0, 0)), error));
            Assert.IsTrue(NearlyEqual(135, VECTOR.Angle180(v, new VECTOR(-1, -1, 0)), error));
            Assert.IsTrue(NearlyEqual(180, VECTOR.Angle180(v, new VECTOR(0, -1, 0)), error));
            Assert.IsTrue(NearlyEqual(135, VECTOR.Angle180(v, new VECTOR(1, -1, 0)), error));
            Assert.IsTrue(NearlyEqual(90, VECTOR.Angle180(v, new VECTOR(1, 0, 0)), error));
            Assert.IsTrue(NearlyEqual(45, VECTOR.Angle180(v, new VECTOR(1, 1, 0)), error));

            v = new VECTOR(0, 0, 1);

            Assert.IsTrue(NearlyEqual(0, VECTOR.Angle180(v, new VECTOR(0, 0, 1)), error));
            Assert.IsTrue(NearlyEqual(45, VECTOR.Angle180(v, new VECTOR(0, 1, 1)), error));
            Assert.IsTrue(NearlyEqual(90, VECTOR.Angle180(v, new VECTOR(0, 1, 0)), error));
            Assert.IsTrue(NearlyEqual(135, VECTOR.Angle180(v, new VECTOR(0, 1, -1)), error));
            Assert.IsTrue(NearlyEqual(180, VECTOR.Angle180(v, new VECTOR(0, 0, -1)), error));
            Assert.IsTrue(NearlyEqual(135, VECTOR.Angle180(v, new VECTOR(0, -1, -1)), error));
            Assert.IsTrue(NearlyEqual(90, VECTOR.Angle180(v, new VECTOR(0, -1, 0)), error));
            Assert.IsTrue(NearlyEqual(45, VECTOR.Angle180(v, new VECTOR(0, -1, 1)), error));

        }

        [TestMethod]
        public void Cross()
        {
            VECTOR v010 = new VECTOR(0, 1, 0);
            VECTOR v100 = new VECTOR(1, 0, 0);
            VECTOR v001 = new VECTOR(0, 0, 1);

            Assert.AreEqual(VECTOR.Cross(v100, v010), v001);
            Assert.AreEqual(VECTOR.Cross(v100, v010), VECTOR.Cross(v010, v100) * -1);
            Assert.AreEqual(v100.Cross(v010), v001);
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            VECTOR v = new VECTOR();
            v[0] = 1;
            v[1] = 2;
            v[2] = 3;

            Assert.AreEqual(v[0], 1);
            Assert.AreEqual(v[1], 2);
            Assert.AreEqual(v[2], 3);
        }

        [TestMethod]
        public void Min()
        {
            VECTOR v = VECTOR.One;
            v = VECTOR.Min(v, 0.5f);
            Assert.AreEqual(v, new VECTOR(0.5f, 0.5f, 0.5f));

            v = VECTOR.One;
            v = VECTOR.Min(v, new VECTOR(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(v, new VECTOR(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            VECTOR v = VECTOR.One;
            v = VECTOR.Max(v, 1.5f);
            Assert.AreEqual(v, new VECTOR(1.5f, 1.5f, 1.5f));

            v = VECTOR.One;
            v = VECTOR.Max(v, new VECTOR(1.5f, 1.5f, 1.5f));
            Assert.AreEqual(v, new VECTOR(1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamp()
        {
            VECTOR v = new VECTOR(0.4f, 1.6f, 0.1f);
            v = VECTOR.Clamp(v, 0.5f, 1.5f);
            Assert.AreEqual(v, new VECTOR(0.5f, 1.5f, 0.5f));

            v = new VECTOR(0.4f, 1.6f, 0.1f);
            v = VECTOR.Clamp(v, new VECTOR(0.5f, 1.5f, 0.5f), new VECTOR(0.5f, 1.5f, 0.5f));
            Assert.AreEqual(v, new VECTOR(0.5f, 1.5f, 0.5f));
        }

        [TestMethod]
        public new void ToString()
        {
            VECTOR v = new VECTOR(1, 2, 3);
            Assert.AreEqual("1,2,3", v.ToString());
        }

        bool NearlyEqual(REAL f1, REAL f2, REAL eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        VECTOR Random3(int seed)
        {
            Random rnd = new Random(seed);
            return new VECTOR(rnd.NextFloat(), rnd.NextFloat(), rnd.NextFloat());
        }

    }
}
