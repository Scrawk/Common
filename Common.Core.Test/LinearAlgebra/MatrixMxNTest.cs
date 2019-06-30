using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{
    [TestClass]
    public class Core_Numerics_MatrixMxNTest
    {
        [TestMethod]
        public void Multiply2x3Scalar()
        {
            double[,] matrix = new double[,]
            {
                { 5, 2, 11},
                { 9, 4, 14}
            };

            double[,] multiplied = MatrixMxN.MultiplyScalar(matrix, 3.0);

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
        public void Multiply2x2by2x2()
        {
            double[,] matrix1 = new double[,]
            {
                { 1, 2},
                { 4, 5}
            };

            double[,] matrix2 = new double[,]
            {
                { 7, 8},
                { 9, 10}
            };

            double[,] multiplied = MatrixMxN.MultiplyMatrix(matrix1, matrix2);

            Assert.AreEqual(2, multiplied.GetLength(0));
            Assert.AreEqual(2, multiplied.GetLength(1));

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

        }

        [TestMethod]
        public void Multiply2x3by3x2()
        {
            double[,] matrix1 = new double[,]
            {
                { 1, 2, 3},
                { 4, 5, 6}
            };

            double[,] matrix2 = new double[,]
            {
                { 7, 8},
                { 9, 10},
                { 11, 12 }
            };

            double[,] multiplied = MatrixMxN.MultiplyMatrix(matrix1, matrix2);

            Assert.AreEqual(2, multiplied.GetLength(0));
            Assert.AreEqual(2, multiplied.GetLength(1));

            double[,] expected = new double[,]
            {
                { 58, 64},
                { 139, 154}
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                    Assert.AreEqual(expected[i, j], Math.Round(multiplied[i, j], 5));
            }
        }

        [TestMethod]
        public void Multiply1x3by3x4()
        {
            double[,] matrix1 = new double[,]
            {
                { 3, 4, 2}
            };

            double[,] matrix2 = new double[,]
            {
                { 13, 9, 7, 15},
                { 8, 7, 4, 6},
                { 6, 4, 0, 3 }
            };

            double[,] multiplied = MatrixMxN.MultiplyMatrix(matrix1, matrix2);

            Assert.AreEqual(1, multiplied.GetLength(0));
            Assert.AreEqual(4, multiplied.GetLength(1));

            double[,] expected = new double[,]
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
        public void MultiplySingle1x3by3x4()
        {
            double[] matrix1 = new double[]
            {
                3, 4, 2
            };

            double[,] matrix2 = new double[,]
            {
                { 13, 9, 7, 15},
                { 8, 7, 4, 6},
                { 6, 4, 0, 3 }
            };

            double[] multiplied = MatrixMxN.MultiplyMatrix(matrix1, matrix2);

            Assert.AreEqual(4, multiplied.Length);

            double[] expected = new double[]
            {
                83, 63, 37, 75
            };

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(expected[i], Math.Round(multiplied[i], 5));
            }
        }


        [TestMethod]
        public void Multiply4x3Vector3()
        {
            double[] vector = new double[]
            {
                -2, 1, 0
            };

            double[,] matrix = new double[,]
            {
                { 1, 2, 3},
                { 4, 5, 6},
                { 7, 8, 9},
                { 10, 11, 12}
            };

            double[] multiplied = MatrixMxN.MultiplyVector(matrix, vector);

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
            double[,] matrix = new double[,]
            {
                { 1, 2},
                { 3, 4}
            };

            double[,] inverted = MatrixMxN.Inverse(matrix);

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
        }

        [TestMethod]
        public void Inverse3x3()
        {

            double[,] matrix = new double[,]
            {
                { 1, 5, 3},
                { 1, 3, 2},
                { 2, 4, -6}
            };

            double[,] inverted = MatrixMxN.Inverse(matrix);

            double[,] expected = new double[,]
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
        public void Determinant3x3()
        {
            double[,] matrix = new double[,]
            {
                { 0, 1, 3},
                { 1, 2, 0},
                { 0, 3, 4}
            };

            Assert.AreEqual(5.0,  MatrixMxN.Determinant(matrix));
        }

        [TestMethod]
        public void Determinant4x4()
        {
            double[,] matrix = new double[,]
            {
                { 2, 3, 3, 1},
                { 0, 4, 3, -3},
                { 2, -1, -1, -3},
                { 0, -4, -3, 2}
            };

            Assert.AreEqual(8.0, MatrixMxN.Determinant(matrix));

        }

    }
}
