using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Core_Numerics_Vector3dTest
    {

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Vector3d.One.Equals(new Vector3d(1, 1, 1)));
            Assert.IsTrue(Vector3d.One == new Vector3d(1, 1, 1));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Vector3d.One != new Vector3d(2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random3(0).EqualsWithError(Random3(0), 1e-6));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(Vector3d.One + Vector3d.One, new Vector3d(2, 2, 2));
            Assert.AreEqual(Vector3d.One + 1, new Vector3d(2, 2, 2));
            Assert.AreEqual(1 + Vector3d.One, new Vector3d(2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(Vector3d.One - Vector3d.One, new Vector3d(0, 0, 0));
            Assert.AreEqual(Vector3d.One - 1, new Vector3d(0, 0, 0));
            Assert.AreEqual(1 - Vector3d.One, new Vector3d(0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(Vector3d.One * new Vector3d(2, 2, 2), new Vector3d(2, 2, 2));
            Assert.AreEqual(Vector3d.One * 2, new Vector3d(2, 2, 2));
            Assert.AreEqual(2 * Vector3d.One, new Vector3d(2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(Vector3d.One / new Vector3d(2, 2, 2), new Vector3d(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(Vector3d.One / 2, new Vector3d(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Dot()
        {
            Assert.AreEqual(Vector3d.Dot(Vector3d.One, Vector3d.One), 3);
        }

        [TestMethod]
        public void Magnitude()
        {
            Assert.IsTrue(NearlyEqual(Vector3d.One.Magnitude, Math.Sqrt(3)));
        }

        [TestMethod]
        public void SqrMagnitude()
        {
            Assert.AreEqual(Vector3d.One.SqrMagnitude, 3);
        }

        [TestMethod]
        public void Absolute()
        {
            Vector3d v = new Vector3d(-1, -2, -3);
            Assert.AreEqual(1, v.Absolute.x);
            Assert.AreEqual(2, v.Absolute.y);
            Assert.AreEqual(3, v.Absolute.z);
        }

        [TestMethod]
        public void Normalized()
        {
            Assert.IsTrue(NearlyEqual(Random3(0).Normalized.Magnitude, 1));

            Vector3d v = Random3(0);
            v.Normalize();

            Assert.IsTrue(NearlyEqual(v.Magnitude, 1));
        }

        [TestMethod]
        public void Cross()
        {
            Vector3d v010 = new Vector3d(0, 1, 0);
            Vector3d v100 = new Vector3d(1, 0, 0);
            Vector3d v001 = new Vector3d(0, 0, 1);

            Assert.AreEqual(Vector3d.Cross(v100, v010), v001);
            Assert.AreEqual(Vector3d.Cross(v100, v010), Vector3d.Cross(v010, v100) * -1);
            Assert.AreEqual(v100.Cross(v010), v001);
        }

        [TestMethod]
        public void AccessedByIndex()
        {
            Vector3d v = new Vector3d();
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
            Vector3d v = Vector3d.One;
            v.Min(0.5f);
            Assert.AreEqual(v, new Vector3d(0.5f, 0.5f, 0.5f));

            v = Vector3d.One;
            v.Min(new Vector3d(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(v, new Vector3d(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            Vector3d v = Vector3d.One;
            v.Max(1.5f);
            Assert.AreEqual(v, new Vector3d(1.5f, 1.5f, 1.5f));

            v = Vector3d.One;
            v.Max(new Vector3d(1.5f, 1.5f, 1.5f));
            Assert.AreEqual(v, new Vector3d(1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamp()
        {
            Vector3d v = new Vector3d(0.4f, 1.6f, 0.1f);
            v.Clamp(0.5f, 1.5f);
            Assert.AreEqual(v, new Vector3d(0.5f, 1.5f, 0.5f));

            v = new Vector3d(0.4f, 1.6f, 0.1f);
            v.Clamp(new Vector3d(0.5f, 1.5f, 0.5f), new Vector3d(0.5f, 1.5f, 0.5f));
            Assert.AreEqual(v, new Vector3d(0.5f, 1.5f, 0.5f));
        }

        [TestMethod]
        public void Abs()
        {
            Vector3d v = new Vector3d(-1, -2, -3);
            v.Abs();
            Assert.AreEqual(v, new Vector3d(1,2,3));
        }

        [TestMethod]
        public new void ToString()
        {
            Vector3d v = new Vector3d(1, 2, 3);
            Assert.AreEqual("1,2,3", v.ToString());
        }

        [TestMethod]
        public void FromString()
        {
            Vector3d v = new Vector3d(1, 2, 3);
            Assert.AreEqual(v, Vector3d.FromString("1,2,3"));
        }

        bool NearlyEqual(double f1, double f2, double eps = 1e-6)
        {
            return Math.Abs(f1 - f2) < eps;
        }

        Vector3d Random3(int seed)
        {
            Random rnd = new Random(seed);
            return new Vector3d(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
        }

    }
}
