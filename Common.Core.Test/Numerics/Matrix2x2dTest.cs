using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{

    [TestClass]
    public class Core_Numerics_Matrix2x2dTest
    {
        const int HALF_SIZE = 2;
        const int SIZE = 4;

        [TestMethod]
        public void CreatedFromValues()
        {
            Matrix2x2d m = new Matrix2x2d(0, 2,
                                           1, 3);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void CreatedFromSingleValue()
        {
            double v = 1;
            Matrix2x2d m = new Matrix2x2d(v);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(v, m[i]);
        }

        [TestMethod]
        public void CreatedFromArray()
        {
            double[] d = new double[]{0, 1, 2, 3};

            Matrix2x2d m = new Matrix2x2d(d);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void CreatedFromArray2()
        {
            double[,] d = new double[,]{{0, 2},
                                        {1, 3}};

            Matrix2x2d m = new Matrix2x2d(d);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void IndexAssignedTo()
        {
            Matrix2x2d m = new Matrix2x2d();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);

        }

        [TestMethod]
        public void Index2AssignedTo()
        {
            Matrix2x2d m = new Matrix2x2d();

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
            Assert.IsTrue(Random2x2(0).Equals(Random2x2(0)));
            Assert.IsTrue(Random2x2(0) == Random2x2(0));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(Random2x2(0) != Random2x2(1));
        }

        [TestMethod]
        public void EqualsWithError()
        {
            Assert.IsTrue(Random2x2(0).EqualsWithError(Random2x2(0), 1e-6));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(new Matrix2x2d(2), (new Matrix2x2d(1)) + (new Matrix2x2d(1)));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(new Matrix2x2d(1), (new Matrix2x2d(2)) - (new Matrix2x2d(1)));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(new Matrix2x2d(4), (new Matrix2x2d(2)) * 2);
            Assert.AreEqual(new Matrix2x2d(4), 2 * (new Matrix2x2d(2)));
            Assert.AreEqual(new Matrix2x2d(2, 6, 3, 11), Indexed2x2() * Indexed2x2());
            Assert.AreEqual(new Vector2d(2, 4), Indexed2x2() * Vector2d.One);
        }

        [TestMethod]
        public void Transpose()
        {
            Matrix2x2d m = new Matrix2x2d();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[j, i] = i + j * HALF_SIZE;

            Assert.AreEqual(Indexed2x2().Transpose, m);

        }

        [TestMethod]
        public void Determinant()
        {
            Assert.AreEqual(Indexed2x2().Determinant, 0 * 3 - 1 * 2);
        }

        [TestMethod]
        public void Inverse()
        {
            Assert.IsTrue((Random2x2(0).Inverse * Random2x2(0)).EqualsWithError(Matrix2x2d.Identity, 1e-6));
        }

        [TestMethod]
        public void TryInverse()
        {
            Matrix2x2d m = Random2x2(0);
            Matrix2x2d inverse = Matrix2x2d.Identity;
            m.TryInverse(ref inverse);

            Assert.IsTrue((inverse * Random2x2(0)).EqualsWithError(Matrix2x2d.Identity, 1e-6));
        }

        [TestMethod]
        public void Trace()
        {
            Assert.AreEqual(Indexed2x2().Trace, 0 + 3);
        }

        [TestMethod]
        public void GetColumn()
        {
            Matrix2x2d m = Indexed2x2();

            Assert.AreEqual(new Vector2d(0, 1), m.GetColumn(0));
            Assert.AreEqual(new Vector2d(2, 3), m.GetColumn(1));
        }

        [TestMethod]
        public void SetColumn()
        {
            Matrix2x2d m = new Matrix2x2d();
            m.SetColumn(0, new Vector2d(0, 1));
            m.SetColumn(1, new Vector2d(2, 3));

            Assert.AreEqual(new Vector2d(0, 1), m.GetColumn(0));
            Assert.AreEqual(new Vector2d(2, 3), m.GetColumn(1));
        }

        [TestMethod]
        public void GetRow()
        {
            Matrix2x2d m = Indexed2x2();

            Assert.AreEqual(new Vector2d(0, 2), m.GetRow(0));
            Assert.AreEqual(new Vector2d(1, 3), m.GetRow(1));
        }

        [TestMethod]
        public void SetRowt()
        {
            Matrix2x2d m = new Matrix2x2d();
            m.SetRow(0, new Vector2d(0, 2));
            m.SetRow(1, new Vector2d(1, 3));

            Assert.AreEqual(new Vector2d(0, 2), m.GetRow(0));
            Assert.AreEqual(new Vector2d(1, 3), m.GetRow(1));
        }

        [TestMethod]
        public void ToMatrix3x3d()
        {
            Assert.AreEqual(Indexed3x3(), Indexed2x2().ToMatrix3x3d());
        }

        Matrix2x2d Random2x2(int seed)
        {
            Random rnd = new Random(seed);
            Matrix2x2d m = new Matrix2x2d();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = rnd.NextDouble();

            return m;
        }

        Matrix2x2d Indexed2x2()
        {
            Matrix2x2d m = new Matrix2x2d();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            return m;
        }

        Matrix3x3d Indexed3x3()
        {
            Matrix3x3d m = new Matrix3x3d();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = i + j * HALF_SIZE;

            return m;
        }

    }

}