using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using REAL = System.Double;
using VECTOR = Common.Core.Numerics.Vector4d;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Vector4dTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(VECTOR.One.Equals(new VECTOR(1, 1, 1, 1)));
            Assert.IsTrue(VECTOR.One == new VECTOR(1, 1, 1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(VECTOR.One != new VECTOR(2, 2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random4(0).AlmostEqual(Random4(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(VECTOR.One + VECTOR.One, new VECTOR(2, 2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(VECTOR.One - VECTOR.One, new VECTOR(0, 0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(VECTOR.One * new VECTOR(2, 2, 2, 2), new VECTOR(2, 2, 2, 2));
            Assert.AreEqual(VECTOR.One * 2, new VECTOR(2, 2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(VECTOR.One / new VECTOR(2, 2, 2, 2), new VECTOR(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(VECTOR.One / 2, new VECTOR(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(VECTOR.Dot(VECTOR.One, VECTOR.One), 4);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(VECTOR.One.Magnitude, Math.Sqrt(4)));
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(VECTOR.One.SqrMagnitude, 4);
        }

        [TestMethod]
        public void Absolute()
        {
            VECTOR v = new VECTOR(-1, -2, -3, -4);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
            Assert.AreEqual(3, v.Absolute.z);
            Assert.AreEqual(4, v.Absolute.w);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random4(0).Normalized.Magnitude, 1));

            VECTOR v = Random4(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1));
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            VECTOR v = new VECTOR();
            v[0] = 1;
            v[1] = 2;
            v[2] = 3;
            v[3] = 4;

            Assert.AreEqual(v[0], 1);
            Assert.AreEqual(v[1], 2);
            Assert.AreEqual(v[2], 3);
            Assert.AreEqual(v[3], 4);
        }

        [TestMethod]
        public void Min()
        {
            VECTOR v = VECTOR.One;
            v = VECTOR.Min(v, 0.5f);
            Assert.AreEqual(v, new VECTOR(0.5f, 0.5f, 0.5f, 0.5f));

            v = VECTOR.One;
            v = VECTOR.Min(v, new VECTOR(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(v, new VECTOR(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            VECTOR v = VECTOR.One;
            v = VECTOR.Max(v, 1.5f);
            Assert.AreEqual(v, new VECTOR(1.5f, 1.5f, 1.5f, 1.5f));

            v = VECTOR.One;
            v = VECTOR.Max(v, new VECTOR(1.5f, 1.5f, 1.5f, 1.5f));
            Assert.AreEqual(v, new VECTOR(1.5f, 1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamped()
        {
            VECTOR v = new VECTOR(0.4f, 1.6f, 0.1f, 1.7f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new VECTOR(0.5f, 1.5f, 0.5f, 1.5f));

            v = new VECTOR(0.4f, 1.6f, 0.1f, 1.7f);
            v.Clamp(new VECTOR(0.5f, 1.5f, 0.5f, 1.5f), new VECTOR(0.5f, 1.5f, 0.5f, 1.5f));
            Assert.AreEqual(v, new VECTOR(0.5f, 1.5f, 0.5f, 1.5f));
        }

        [TestMethod]
        public void Abs()
        {
            VECTOR v = new VECTOR(-1, -2, -3, -4);
            v.Abs();
            Assert.AreEqual(v, new VECTOR(1, 2, 3, 4));
        }

        [TestMethod]
        public new void ToString()
        {
            VECTOR v = new VECTOR(1, 2, 3, 4);
            Assert.AreEqual("1,2,3,4", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            VECTOR v = new VECTOR(1, 2, 3, 4);
            Assert.AreEqual(v, VECTOR.FromString("1,2,3,4"));
        }

        bool NearlyEqual(REAL f1, REAL f2, REAL eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        VECTOR Random4(int seed)
        {
            Random rnd = new Random(seed);
            return new VECTOR(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
        }

    }
}
