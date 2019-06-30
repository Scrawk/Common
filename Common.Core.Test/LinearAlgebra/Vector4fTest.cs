using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Core_Numerics_Vector4fTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Vector4f.One.Equals(new Vector4f(1, 1, 1, 1)));
            Assert.IsTrue(Vector4f.One == new Vector4f(1, 1, 1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Vector4f.One != new Vector4f(2, 2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random4(0).EqualsWithError(Random4(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(Vector4f.One + Vector4f.One, new Vector4f(2, 2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(Vector4f.One - Vector4f.One, new Vector4f(0, 0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(Vector4f.One * new Vector4f(2, 2, 2, 2), new Vector4f(2, 2, 2, 2));
            Assert.AreEqual(Vector4f.One * 2, new Vector4f(2, 2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(Vector4f.One / new Vector4f(2, 2, 2, 2), new Vector4f(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(Vector4f.One / 2, new Vector4f(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(Vector4f.Dot(Vector4f.One, Vector4f.One), 4);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(Vector4f.One.Magnitude, (float)Math.Sqrt(4)));
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(Vector4f.One.SqrMagnitude, 4);
        }

        [TestMethod]
        public void Absolute()
        {
            Vector4f v = new Vector4f(-1, -2, -3, -4);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
            Assert.AreEqual(3, v.Absolute.z);
            Assert.AreEqual(4, v.Absolute.w);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random4(0).Normalized.Magnitude, 1));

            Vector4f v = Random4(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1));
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            Vector4f v = new Vector4f();
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
            Vector4f v = Vector4f.One;
            v.Min(0.5f);
            Assert.AreEqual(v, new Vector4f(0.5f, 0.5f, 0.5f, 0.5f));

            v = Vector4f.One;
            v.Min(new Vector4f(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(v, new Vector4f(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            Vector4f v = Vector4f.One;
            v.Max(1.5f);
            Assert.AreEqual(v, new Vector4f(1.5f, 1.5f, 1.5f, 1.5f));

            v = Vector4f.One;
            v.Max(new Vector4f(1.5f, 1.5f, 1.5f, 1.5f));
            Assert.AreEqual(v, new Vector4f(1.5f, 1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamped()
        {
            Vector4f v = new Vector4f(0.4f, 1.6f, 0.1f, 1.7f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new Vector4f(0.5f, 1.5f, 0.5f, 1.5f));

            v = new Vector4f(0.4f, 1.6f, 0.1f, 1.7f);
            v.Clamp(new Vector4f(0.5f, 1.5f, 0.5f, 1.5f), new Vector4f(0.5f, 1.5f, 0.5f, 1.5f));
            Assert.AreEqual(v, new Vector4f(0.5f, 1.5f, 0.5f, 1.5f));
        }

        [TestMethod]
        public void Abs()
        {
            Vector4f v = new Vector4f(-1, -2, -3, -4);
            v.Abs();
            Assert.AreEqual(v, new Vector4f(1, 2, 3, 4));
        }

        [TestMethod]
        public new void ToString()
        {
            Vector4f v = new Vector4f(1, 2, 3, 4);
            Assert.AreEqual("1,2,3,4", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            Vector4f v = new Vector4f(1, 2, 3, 4);
            Assert.AreEqual(v, Vector4f.FromString("1,2,3,4"));
        }

        bool NearlyEqual(float f1, float f2, float eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        Vector4f Random4(int seed)
        {
            Random rnd = new Random(seed);
            return new Vector4f((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        }

    }
}
