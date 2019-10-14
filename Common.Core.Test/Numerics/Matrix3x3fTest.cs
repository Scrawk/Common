using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Matrix3x3fTest
    {

        const int HALF_SIZE = 3;
        const int SIZE = 9;

        [TestMethod]
        public void CreatedFromValues()
        {
            Matrix3x3f m = new Matrix3x3f(0, 3, 6,
                                           1, 4, 7,
                                           2, 5, 8);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void CreatedFromSingleValue()
        {
            float v = 1.1234f;

            Matrix3x3f m = new Matrix3x3f(v);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(v, m[i]);
        }

        [TestMethod]
        public void CreatedFromArray2()
        {
            float[,] d = new float[,]{{0, 3, 6},
                                        {1, 4, 7},
                                        {2, 5, 8}};

            Matrix3x3f m = new Matrix3x3f(d);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void IndexAssignedTo()
        {
            Matrix3x3f m = new Matrix3x3f();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);

        }

        [TestMethod]
        public void Index2AssignedTo()
        {
            Matrix3x3f m = new Matrix3x3f();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = i + j * HALF_SIZE;

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    Assert.AreEqual(i + j * HALF_SIZE, m[i, j]);

        }

        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(Random3x3(0).Equals(Random3x3(0)));
            Assert.IsTrue(Random3x3(0) == Random3x3(0));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Random3x3(0) != Random3x3(1));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random3x3(0).EqualsWithError(Random3x3(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(new Matrix3x3f(2), (new Matrix3x3f(1)) + (new Matrix3x3f(1)));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(new Matrix3x3f(1), (new Matrix3x3f(2)) - (new Matrix3x3f(1)));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(new Matrix3x3f(4), (new Matrix3x3f(2)) * 2);
            Assert.AreEqual(new Matrix3x3f(4), 2 * (new Matrix3x3f(2)));
            Assert.AreEqual(new Matrix3x3f(15, 42, 69, 18, 54, 90, 21, 66, 111), Indexed3x3() * Indexed3x3());
            Assert.AreEqual(new Vector3f(9, 12, 15), Indexed3x3() * Vector3f.One);
        }

        [TestMethod]
        public void Transpose()
        {
            Matrix3x3f m = new Matrix3x3f();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[j, i] = i + j * HALF_SIZE;

            Assert.AreEqual(Indexed3x3().Transpose, m);

        }

        [TestMethod]
        public void Inverse()
        {
            Assert.IsTrue((Random3x3(0).Inverse * Random3x3(0)).EqualsWithError(Matrix3x3f.Identity, 1e-4f));
        }

        [TestMethod]
        public void TryInverse()
        {
            Matrix3x3f m = Random3x3(0);
            Matrix3x3f inverse = Matrix3x3f.Identity;
            m.TryInverse(ref inverse);

            Assert.IsTrue((inverse * Random3x3(0)).EqualsWithError(Matrix3x3f.Identity, 1e-4f));
        }

        [TestMethod]
        public void Trace()
        {
            Assert.AreEqual(Indexed3x3().Trace, 0 + 4 + 8);
        }

        [TestMethod]
        public void GetColumn()
        {
            Matrix3x3f m = Indexed3x3();

            Assert.AreEqual(new Vector3f(0, 1, 2), m.GetColumn(0));
            Assert.AreEqual(new Vector3f(3, 4, 5), m.GetColumn(1));
            Assert.AreEqual(new Vector3f(6, 7, 8), m.GetColumn(2));
        }

        [TestMethod]
        public void SetColumn()
        {
            Matrix3x3f m = new Matrix3x3f();
            m.SetColumn(0, new Vector3f(0, 1, 2));
            m.SetColumn(1, new Vector3f(3, 4, 5));
            m.SetColumn(2, new Vector3f(6, 7, 8));

            Assert.AreEqual(new Vector3f(0, 1, 2), m.GetColumn(0));
            Assert.AreEqual(new Vector3f(3, 4, 5), m.GetColumn(1));
            Assert.AreEqual(new Vector3f(6, 7, 8), m.GetColumn(2));
        }

        [TestMethod]
        public void GetRow()
        {
            Matrix3x3f m = Indexed3x3();

            Assert.AreEqual(new Vector3f(0, 3, 6), m.GetRow(0));
            Assert.AreEqual(new Vector3f(1, 4, 7), m.GetRow(1));
            Assert.AreEqual(new Vector3f(2, 5, 8), m.GetRow(2));
        }

        [TestMethod]
        public void SetRow()
        {
            Matrix3x3f m = new Matrix3x3f();
            m.SetRow(0, new Vector3f(0, 3, 6));
            m.SetRow(1, new Vector3f(1, 4, 7));
            m.SetRow(2, new Vector3f(2, 5, 8));

            Assert.AreEqual(new Vector3f(0, 3, 6), m.GetRow(0));
            Assert.AreEqual(new Vector3f(1, 4, 7), m.GetRow(1));
            Assert.AreEqual(new Vector3f(2, 5, 8), m.GetRow(2));
        }

        [TestMethod]
        public void ToMatrix4x43()
        {
            Assert.AreEqual(Indexed4x4(), Indexed3x3().ToMatrix4x4f());
        }

        Matrix3x3f Random3x3(int seed)
        {
            Random rnd = new Random(seed);
            Matrix3x3f m = new Matrix3x3f();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = (float)rnd.NextDouble();

            return m;
        }

        Matrix3x3f Indexed3x3()
        {
            Matrix3x3f m = new Matrix3x3f();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            return m;
        }

        Matrix4x4f Indexed4x4()
        {
            Matrix4x4f m = new Matrix4x4f();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = i + j * HALF_SIZE;

            return m;
        }

    }

}