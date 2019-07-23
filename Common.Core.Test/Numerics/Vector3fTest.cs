using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Vector3fTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Vector3f.One.Equals(new Vector3f(1, 1, 1)));
            Assert.IsTrue(Vector3f.One == new Vector3f(1, 1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Vector3f.One != new Vector3f(2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random3(0).EqualsWithError(Random3(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(Vector3f.One + Vector3f.One, new Vector3f(2, 2, 2));
            Assert.AreEqual(Vector3f.One + 1, new Vector3f(2, 2, 2));
            Assert.AreEqual(1 + Vector3f.One, new Vector3f(2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(Vector3f.One - Vector3f.One, new Vector3f(0, 0, 0));
            Assert.AreEqual(Vector3f.One - 1, new Vector3f(0, 0, 0));
            Assert.AreEqual(1 - Vector3f.One, new Vector3f(0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(Vector3f.One * new Vector3f(2, 2, 2), new Vector3f(2, 2, 2));
            Assert.AreEqual(Vector3f.One * 2, new Vector3f(2, 2, 2));
            Assert.AreEqual(2 * Vector3f.One, new Vector3f(2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(Vector3f.One / new Vector3f(2, 2, 2), new Vector3f(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(Vector3f.One / 2, new Vector3f(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(Vector3f.Dot(Vector3f.One, Vector3f.One), 3);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(Vector3f.One.Magnitude, (float)Math.Sqrt(3)));
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(Vector3f.One.SqrMagnitude, 3);
        }

        [TestMethod]
        public void Absolute()
        {
            Vector3f v = new Vector3f(-1, -2, -3);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
            Assert.AreEqual(3, v.Absolute.z);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random3(0).Normalized.Magnitude, 1));

            Vector3f v = Random3(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1));
        }

        [TestMethod]
        public void Cross()
        {
            Vector3f v010 = new Vector3f(0, 1, 0);
            Vector3f v100 = new Vector3f(1, 0, 0);
            Vector3f v001 = new Vector3f(0, 0, 1);

            Assert.AreEqual(Vector3f.Cross(v100, v010), v001);
            Assert.AreEqual(Vector3f.Cross(v100, v010), Vector3f.Cross(v010, v100) * -1);
            Assert.AreEqual(v100.Cross(v010), v001);
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            Vector3f v = new Vector3f();
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
            Vector3f v = Vector3f.One;
            v = Vector3f.Min(v, 0.5f);
            Assert.AreEqual(v, new Vector3f(0.5f, 0.5f, 0.5f));

            v = Vector3f.One;
            v = Vector3f.Min(v, new Vector3f(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(v, new Vector3f(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            Vector3f v = Vector3f.One;
            v = Vector3f.Max(v,1.5f);
            Assert.AreEqual(v, new Vector3f(1.5f, 1.5f, 1.5f));

            v = Vector3f.One;
            v = Vector3f.Max(v, new Vector3f(1.5f, 1.5f, 1.5f));
            Assert.AreEqual(v, new Vector3f(1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamp()
        {
            Vector3f v = new Vector3f(0.4f, 1.6f, 0.1f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new Vector3f(0.5f, 1.5f, 0.5f));

            v = new Vector3f(0.4f, 1.6f, 0.1f);
            v.Clamp(new Vector3f(0.5f, 1.5f, 0.5f), new Vector3f(0.5f, 1.5f, 0.5f));
            Assert.AreEqual(v, new Vector3f(0.5f, 1.5f, 0.5f));
        }

        [TestMethod]
        public void Abs()
        {
            Vector3f v = new Vector3f(-1, -2, -3);
            v.Abs();
            Assert.AreEqual(v, new Vector3f(1, 2, 3));
        }

        [TestMethod]
        public new void ToString()
        {
            Vector3f v = new Vector3f(1, 2, 3);
            Assert.AreEqual("1,2,3", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            Vector3f v = new Vector3f(1, 2, 3);
            Assert.AreEqual(v, Vector3f.FromString("1,2,3"));
        }

        bool NearlyEqual(float f1, float f2, float eps = 1e-6f)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        Vector3f Random3(int seed)
        {
            Random rnd = new Random(seed);
            return new Vector3f((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        }

    }
}
