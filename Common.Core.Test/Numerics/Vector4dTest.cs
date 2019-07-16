using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Vector4dTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Vector4d.One.Equals(new Vector4d(1, 1, 1, 1)));
            Assert.IsTrue(Vector4d.One == new Vector4d(1, 1, 1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Vector4d.One != new Vector4d(2, 2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random4(0).EqualsWithError(Random4(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(Vector4d.One + Vector4d.One, new Vector4d(2, 2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(Vector4d.One - Vector4d.One, new Vector4d(0, 0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(Vector4d.One * new Vector4d(2, 2, 2, 2), new Vector4d(2, 2, 2, 2));
            Assert.AreEqual(Vector4d.One * 2, new Vector4d(2, 2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(Vector4d.One / new Vector4d(2, 2, 2, 2), new Vector4d(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(Vector4d.One / 2, new Vector4d(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(Vector4d.Dot(Vector4d.One, Vector4d.One), 4);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(Vector4d.One.Magnitude, Math.Sqrt(4)));
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(Vector4d.One.SqrMagnitude, 4);
        }

        [TestMethod]
        public void Absolute()
        {
            Vector4d v = new Vector4d(-1, -2, -3, -4);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
            Assert.AreEqual(3, v.Absolute.z);
            Assert.AreEqual(4, v.Absolute.w);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random4(0).Normalized.Magnitude, 1));

            Vector4d v = Random4(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1));
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            Vector4d v = new Vector4d();
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
            Vector4d v = Vector4d.One;
            v.Min(0.5f);
            Assert.AreEqual(v, new Vector4d(0.5f, 0.5f, 0.5f, 0.5f));

            v = Vector4d.One;
            v.Min(new Vector4d(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(v, new Vector4d(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            Vector4d v = Vector4d.One;
            v.Max(1.5f);
            Assert.AreEqual(v, new Vector4d(1.5f, 1.5f, 1.5f, 1.5f));

            v = Vector4d.One;
            v.Max(new Vector4d(1.5f, 1.5f, 1.5f, 1.5f));
            Assert.AreEqual(v, new Vector4d(1.5f, 1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamped()
        {
            Vector4d v = new Vector4d(0.4f, 1.6f, 0.1f, 1.7f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new Vector4d(0.5f, 1.5f, 0.5f, 1.5f));

            v = new Vector4d(0.4f, 1.6f, 0.1f, 1.7f);
            v.Clamp(new Vector4d(0.5f, 1.5f, 0.5f, 1.5f), new Vector4d(0.5f, 1.5f, 0.5f, 1.5f));
            Assert.AreEqual(v, new Vector4d(0.5f, 1.5f, 0.5f, 1.5f));
        }

        [TestMethod]
        public void Abs()
        {
            Vector4d v = new Vector4d(-1, -2, -3, -4);
            v.Abs();
            Assert.AreEqual(v, new Vector4d(1, 2, 3, 4));
        }

        [TestMethod]
        public new void ToString()
        {
            Vector4d v = new Vector4d(1, 2, 3, 4);
            Assert.AreEqual("1,2,3,4", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            Vector4d v = new Vector4d(1, 2, 3, 4);
            Assert.AreEqual(v, Vector4d.FromString("1,2,3,4"));
        }

        bool NearlyEqual(double f1, double f2, double eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        Vector4d Random4(int seed)
        {
            Random rnd = new Random(seed);
            return new Vector4d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
        }

    }
}
