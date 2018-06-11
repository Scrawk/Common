using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;

namespace Common.Core.Test.LinearAlgebra
{

    [TestClass]
    public class Core_LinearAlgebra_Matrix2x2fTest
    {
        const int HALF_SIZE = 2;
        const int SIZE = 4;

        [TestMethod]
        public void CreatedFromValues()
        {
            Matrix2x2f m = new Matrix2x2f(0, 2,
                                           1, 3);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void MCreatedFromSingleValue()
        {
            float v = 1;
            Matrix2x2f m = new Matrix2x2f(v);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(v, m[i]);
        }

        [TestMethod]
        public void CreatedFromArray()
        {
            float[] d = new float[] { 0, 1, 2, 3 };

            Matrix2x2f m = new Matrix2x2f(d);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void CreatedFromArray2()
        {
            float[,] d = new float[,]{{0, 2},
                                        {1, 3}};

            Matrix2x2f m = new Matrix2x2f(d);

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);
        }

        [TestMethod]
        public void IndexAssignedTo()
        {
            Matrix2x2f m = new Matrix2x2f();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            for (int i = 0; i < SIZE; i++)
                Assert.AreEqual(i, m[i]);

        }

        [TestMethod]
        public void Index2AssignedTo()
        {
            Matrix2x2f m = new Matrix2x2f();

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
            Assert.IsTrue(Random2x2(0).EqualsWithError(Random2x2(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(new Matrix2x2f(2), (new Matrix2x2f(1)) + (new Matrix2x2f(1)));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(new Matrix2x2f(1), (new Matrix2x2f(2)) - (new Matrix2x2f(1)));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(new Matrix2x2f(4), (new Matrix2x2f(2)) * 2);
            Assert.AreEqual(new Matrix2x2f(4), 2 * (new Matrix2x2f(2)));
            Assert.AreEqual(new Matrix2x2f(2, 6, 3, 11), Indexed2x2() * Indexed2x2());
            Assert.AreEqual(new Vector2f(2, 4), Indexed2x2() * Vector2f.One);
        }

        [TestMethod]
        public void Transpose()
        {
            Matrix2x2f m = new Matrix2x2f();

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
            Assert.IsTrue((Random2x2(0).Inverse * Random2x2(0)).EqualsWithError(Matrix2x2f.Identity, 1e-6f));
        }

        [TestMethod]
        public void TryInverse()
        {
            Matrix2x2f m = Random2x2(0);
            Matrix2x2f inverse = Matrix2x2f.Identity;
            m.TryInverse(ref inverse);

            Assert.IsTrue((inverse * Random2x2(0)).EqualsWithError(Matrix2x2f.Identity, 1e-6f));
        }

        [TestMethod]
        public void Trace()
        {
            Assert.AreEqual(Indexed2x2().Trace, 0 + 3);
        }

        [TestMethod]
        public void GetColumn()
        {
            Matrix2x2f m = Indexed2x2();

            Assert.AreEqual(new Vector2f(0, 1), m.GetColumn(0));
            Assert.AreEqual(new Vector2f(2, 3), m.GetColumn(1));
        }

        [TestMethod]
        public void SetColumn()
        {
            Matrix2x2f m = new Matrix2x2f();
            m.SetColumn(0, new Vector2f(0, 1));
            m.SetColumn(1, new Vector2f(2, 3));

            Assert.AreEqual(new Vector2f(0, 1), m.GetColumn(0));
            Assert.AreEqual(new Vector2f(2, 3), m.GetColumn(1));
        }

        [TestMethod]
        public void GetRow()
        {
            Matrix2x2f m = Indexed2x2();

            Assert.AreEqual(new Vector2f(0, 2), m.GetRow(0));
            Assert.AreEqual(new Vector2f(1, 3), m.GetRow(1));
        }

        [TestMethod]
        public void SetRowt()
        {
            Matrix2x2f m = new Matrix2x2f();
            m.SetRow(0, new Vector2f(0, 2));
            m.SetRow(1, new Vector2f(1, 3));

            Assert.AreEqual(new Vector2f(0, 2), m.GetRow(0));
            Assert.AreEqual(new Vector2f(1, 3), m.GetRow(1));
        }

        [TestMethod]
        public void ToMatrix3x3f()
        {
            Assert.AreEqual(Indexed3x3(), Indexed2x2().ToMatrix3x3f());
        }

        Matrix2x2f Random2x2(int seed)
        {
            Random rnd = new Random(seed);
            Matrix2x2f m = new Matrix2x2f();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = (float)rnd.NextDouble();

            return m;
        }

        Matrix2x2f Indexed2x2()
        {
            Matrix2x2f m = new Matrix2x2f();
            for (int i = 0; i < SIZE; i++) m[i] = i;

            return m;
        }

        Matrix3x3f Indexed3x3()
        {
            Matrix3x3f m = new Matrix3x3f();

            for (int i = 0; i < HALF_SIZE; i++)
                for (int j = 0; j < HALF_SIZE; j++)
                    m[i, j] = i + j * HALF_SIZE;

            return m;
        }

    }

}