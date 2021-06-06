using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Matrix4x4dTest
    {

        const int HALF_SIZE = 4;
        const int SIZE = 16;

        [TestMethod]
        public void CreatedFromValues()
        {
            Matrix4x4d m = new Matrix4x4d(0, 4, 8, 12,
                                            1, 5, 9, 13,
                                            2, 6, 10, 14,
                                            3, 7, 11, 15);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void CreatedFromSingleValue()
        {
            float v = 1.1234f;

            Matrix4x4d m = new Matrix4x4d(v);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(v, m[i]);
        }

        [TestMethod]
        public void CreatedFromArray2()
        {
            double[,] d = new double[,]{{0, 4, 8, 12},
                                        {1, 5, 9, 13},
                                        {2, 6, 10, 14},
                                        {3, 7, 11, 15}};

            Matrix4x4d m = new Matrix4x4d(d);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void IndexAssignedTo()
        {
            Matrix4x4d m = new Matrix4x4d();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);

        }

        [TestMethod]
        public void Index2AssignedTo()
        {
            Matrix4x4d m = new Matrix4x4d();

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
            Assert.IsTrue(Random4x4(0).Equals(Random4x4(0)));
            Assert.IsTrue(Random4x4(0) == Random4x4(0));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Random4x4(0) != Random4x4(1));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(new Matrix4x4d(2), (new Matrix4x4d(1)) + (new Matrix4x4d(1)));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(new Matrix4x4d(1), (new Matrix4x4d(2)) - (new Matrix4x4d(1)));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(new Matrix4x4d(4), (new Matrix4x4d(2)) * 2);
            Assert.AreEqual(new Matrix4x4d(4), 2 * (new Matrix4x4d(2)));

            Matrix4x4d m = new Matrix4x4d( 56, 152, 248, 344,
                                            62, 174, 286, 398,
                                            68, 196, 324, 452,
                                            74, 218, 362, 506);

            Assert.AreEqual(m, Indexed4x4() * Indexed4x4());
            Assert.AreEqual(new Vector4d(24, 28, 32, 36), Indexed4x4() * Vector4d.One);
        }

        [TestMethod]
        public void Transpose()
        {
            Matrix4x4d m = new Matrix4x4d();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[j, i] = i + j * HALF_SIZE;

            Assert.AreEqual(Indexed4x4().Transpose, m);

        }

        [TestMethod]
        public void Inverse()
        {
            Assert.IsTrue(Matrix4x4d.AlmostEqual(Random4x4(0).Inverse * Random4x4(0), Matrix4x4d.Identity, 1e-6f));
        }

        [TestMethod]
        public void TryInverse()
        {
            Matrix4x4d m = Random4x4(0);
            Matrix4x4d inverse = Matrix4x4d.Identity;
            m.TryInverse(ref inverse);

            Assert.IsTrue(Matrix4x4d.AlmostEqual(inverse * Random4x4(0), Matrix4x4d.Identity, 1e-6f));
        }

        [TestMethod]
        public void Trace()
        {
            Assert.AreEqual(Indexed4x4().Trace, 0 + 5 + 10 + 15);
        }

        [TestMethod]
        public void GetColumn()
        {
            Matrix4x4d m = Indexed4x4();

            Assert.AreEqual(new Vector4d(0, 1, 2, 3), m.GetColumn(0));
            Assert.AreEqual(new Vector4d(4, 5, 6, 7), m.GetColumn(1));
            Assert.AreEqual(new Vector4d(8, 9, 10, 11), m.GetColumn(2));
            Assert.AreEqual(new Vector4d(12, 13, 14, 15), m.GetColumn(3));
        }

        [TestMethod]
        public void SetColumn()
        {
            Matrix4x4d m = new Matrix4x4d();
            m.SetColumn(0, new Vector4d(0, 1, 2, 3));
            m.SetColumn(1, new Vector4d(4, 5, 6, 7));
            m.SetColumn(2, new Vector4d(8, 9, 10, 11));
            m.SetColumn(3, new Vector4d(12, 13, 14, 15));

            Assert.AreEqual(new Vector4d(0, 1, 2, 3), m.GetColumn(0));
            Assert.AreEqual(new Vector4d(4, 5, 6, 7), m.GetColumn(1));
            Assert.AreEqual(new Vector4d(8, 9, 10, 11), m.GetColumn(2));
            Assert.AreEqual(new Vector4d(12, 13, 14, 15), m.GetColumn(3));
        }

        [TestMethod]
        public void GetRow()
        {
            Matrix4x4d m = Indexed4x4();

            Assert.AreEqual(new Vector4d(0, 4, 8, 12), m.GetRow(0));
            Assert.AreEqual(new Vector4d(1, 5, 9, 13), m.GetRow(1));
            Assert.AreEqual(new Vector4d(2, 6, 10, 14), m.GetRow(2));
            Assert.AreEqual(new Vector4d(3, 7, 11, 15), m.GetRow(3));
        }

        [TestMethod]
        public void SetRow()
        {
            Matrix4x4d m = new Matrix4x4d();
            m.SetRow(0, new Vector4d(0, 4, 8, 12));
            m.SetRow(1, new Vector4d(1, 5, 9, 13));
            m.SetRow(2, new Vector4d(2, 6, 10, 14));
            m.SetRow(3, new Vector4d(3, 7, 11, 15));

            Assert.AreEqual(new Vector4d(0, 4, 8, 12), m.GetRow(0));
            Assert.AreEqual(new Vector4d(1, 5, 9, 13), m.GetRow(1));
            Assert.AreEqual(new Vector4d(2, 6, 10, 14), m.GetRow(2));
            Assert.AreEqual(new Vector4d(3, 7, 11, 15), m.GetRow(3));
        }

        [TestMethod]
        public void ToMatrix3x3d()
        {
            Assert.AreEqual(Indexed3x3(), Indexed4x4().ToMatrix3x3d());
        }

        Matrix4x4d Random4x4(int seed)
        {
            Random rnd = new RandomNum(seed);
            Matrix4x4d m = new Matrix4x4d();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = rnd.NextDouble();

            return m;
        }

        Matrix4x4d Indexed4x4()
        {
            Matrix4x4d m = new Matrix4x4d();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            return m;
        }

        Matrix3x3d Indexed3x3()
        {
            Matrix3x3d m = new Matrix3x3d();

            for (int i = 0; i < HALF_SIZE; i++)
            {
                for (int j = 0; j < HALF_SIZE; j++)
                {
                    if (i < 3 && j < 3) m[i, j] = i + j * HALF_SIZE;
                }
            }

            return m;
        }

    }

}