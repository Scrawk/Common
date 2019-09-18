using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        public void MultiplyScalar()
        {
            var matrix = new Matrix(2, 3);
            matrix.SetRow(0, 5, 2, 11);
            matrix.SetRow(1, 9, 4, 14);
            
            var multiplied = matrix * 3.0;

            double[,] expected = new double[,]
            {
                { 15, 6, 33},
                { 27, 12, 42}
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 3; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(multiplied[i, j], 5));
            }
        }

        [TestMethod]
        public void MultiplyMatrix()
        {
            var matrix1 = new Matrix(2, 2);
            matrix1.SetRow(0, 1, 2);
            matrix1.SetRow(1, 4, 5);
            
            var matrix2 = new Matrix(2, 2);
            matrix2.SetRow(0, 7, 8);
            matrix2.SetRow(1, 9, 10);

            var multiplied = matrix1 * matrix2;

            Assert.AreEqual(2, multiplied.Rows);
            Assert.AreEqual(2, multiplied.Columns);

            double[,] expected = new double[,]
            {
                { 25, 28},
                { 73, 82}
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(multiplied[i, j], 5));
            }

            matrix1 = new Matrix(2, 3);
            matrix1.SetRow(0, 1, 2, 3);
            matrix1.SetRow(1, 4, 5, 6);

            matrix2 = new Matrix(3, 2);
            matrix2.SetRow(0, 7, 8);
            matrix2.SetRow(1, 9, 10);
            matrix2.SetRow(2, 11, 12);

            multiplied = matrix1 * matrix2;

            Assert.AreEqual(2, multiplied.Rows);
            Assert.AreEqual(2, multiplied.Columns);

            expected = new double[,]
            {
                { 58, 64},
                { 139, 154}
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(multiplied[i, j], 5));
            }

            matrix1 = new Matrix(1, 3);
            matrix1.SetRow(0, 3, 4, 2);

            matrix2 = new Matrix(3, 4);
            matrix2.SetRow(0, 13, 9, 7, 15);
            matrix2.SetRow(1, 8, 7, 4, 6);
            matrix2.SetRow(2, 6, 4, 0, 3);

            multiplied = matrix1 * matrix2;

            Assert.AreEqual(1, multiplied.Rows);
            Assert.AreEqual(4, multiplied.Columns);

            expected = new double[,]
            {
                { 83, 63, 37, 75 }
            };

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 4; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(multiplied[i, j], 5));
            }
        }

        [TestMethod]
        public void MultiplyVector()
        {
            var vec = new Vector(-2, 1, 0);

            double[,] array = new double[,]
            {
                { 1, 2, 3},
                { 4, 5, 6},
                { 7, 8, 9},
                { 10, 11, 12}
            };

            var mat = new Matrix(array);

            var multiplied = mat * vec;

            Assert.AreEqual(4, multiplied.Length);

            double[] expected = new double[]
            {
                0, -3, -6, -9
            };

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expected[i], Math.Round(multiplied[i], 5));
            }
        }

        [TestMethod]
        public void Inverse2x2()
        {
            double[,] array = new double[,]
            {
                { 1, 2},
                { 3, 4}
            };

            var mat = new Matrix(array);
            var inverted = mat.Inverse;

            double[,] expected = new double[,]
            {
                { -2, 1},
                { 1.5, -0.5}
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(inverted[i, j], 5));
            }

            array = new double[,]
            {
                { 1, 5, 3},
                { 1, 3, 2},
                { 2, 4, -6}
            };

            mat = new Matrix(array);
            inverted = mat.Inverse;

            expected = new double[,]
            {
                { -1.44444, 2.33333, 0.05556},
                { 0.55556, -0.66667, 0.05556},
                { -0.11111, 0.33333, -0.11111}
            };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(inverted[i, j], 5));
            }

        }

        [TestMethod]
        public void Determinant3()
        {
            double[,] array = new double[,]
            {
                { 0, 1, 3},
                { 1, 2, 0},
                { 0, 3, 4}
            };

            var mat = new Matrix(array);

            Assert.AreEqual(5.0,  mat.Determinant);

            array = new double[,]
            {
                { 2, 3, 3, 1},
                { 0, 4, 3, -3},
                { 2, -1, -1, -3},
                { 0, -4, -3, 2}
            };

            mat = new Matrix(array);

            Assert.AreEqual(8.0, mat.Determinant);
        }

    }
}
