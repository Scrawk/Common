using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.Numerics
{
    /// <summary>
    /// A general matrix class of arbitary rows and columns.
    /// </summary>
    public class Matrix
    {

        private double[,] array;

        public Matrix(int rows, int columns)
        {
            array = new double[rows, columns];
        }

        public Matrix(float[,] mat)
        {
            int rows = mat.GetLength(0);
            int columns = mat.GetLength(1);
            array = new double[rows, columns];
            Array.Copy(mat, array, mat.Length);
        }

        public Matrix(double[,] mat)
        {
            int rows = mat.GetLength(0);
            int columns = mat.GetLength(1);
            array = new double[rows, columns];
            Array.Copy(mat, array, mat.Length);
        }

        public Matrix(List<List<float>> mat)
        {
            int rows = mat.Count;
            int columns = mat[0].Count;

            array = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = mat[i][j];
                }
            }
        }

        public Matrix(List<List<double>> mat)
        {
            int rows = mat.Count;
            int columns = mat[0].Count;

            array = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = mat[i][j];
                }
            }
        }

        public int Rows => array.GetLength(0);

        public int Columns => array.GetLength(1);

        public double this[int i, int j]
        {
            get => array[i, j];
            set => array[i, j] = value;
        }

        /// <summary>
        /// Multiply the matrix with a vector.
        /// </summary>
        public static Vector operator *(Matrix m, Vector v)
        {
            int M = m.Rows;
            int N = m.Columns;

            if (v.Dimension != N)
                throw new ArgumentException("Matrix must have same number of columns as vectors length.");

            var vec = new Vector(M);

            for (int i = 0; i < M; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < N; j++)
                {
                    sum += v[j] * m[i, j];
                }

                vec[i] = sum;
            }

            return vec;
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int M = m1.Rows;
            int N = m2.Columns;

            if (m1.Columns != m2.Rows)
                throw new ArgumentException("Matrix2 must have same number of rows as matrix1 has columns.");

            var mat = new Matrix(M, N);

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < m1.Columns; k++)
                    {
                        sum += m1[i, k] * m2[k, j];
                    }

                    mat[i, j] = sum;
                }
            }

            return mat;
        }

        /// <summary>
        /// Multiply a matrix and a scalar..
        /// </summary>
        public static Matrix operator *(Matrix m, double s)
        {
            int M = m.Rows;
            int N = m.Columns;

            var mat = new Matrix(M, N);

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                    mat[i, j] = m[i, j] * s;
            }

            return mat;
        }

        /// <summary>
        /// Add two matrices.
        /// </summary>
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            int M = m1.Rows;
            int N = m1.Columns;

            if (M != m2.Rows || N != m2.Columns)
                throw new ArgumentException("Two matrices should be the same dimension to add.");

            var mat = new Matrix(M, N);

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                    mat[i, j] = m1[i, j] + m2[i, j];
            }

            return mat;
        }

        /// <summary>
        /// Subtract two matrices.
        /// </summary>
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int M = m1.Rows;
            int N = m1.Columns;

            if (M != m2.Rows || N != m2.Columns)
                throw new ArgumentException("Two matrices should be the same dimension to subtract.");

            var mat = new Matrix(M, N);

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                    mat[i, j] = m1[i, j] - m2[i, j];
            }

            return mat;
        }

        /// <summary>
        /// Transpose the matrix.
        /// </summary>
        public Matrix Transpose
        {
            get
            {
                int M = Rows;
                int N = Columns;

                var transposed = new Matrix(N, M);

                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        transposed[j, i] = array[i, j];
                    }
                }
                return transposed;
            }
        }

        /// <summary>
        /// Inverse the matrix.
        /// </summary>
        public Matrix Inverse
        {
            get
            {
                return Cofactor.Transpose * MathUtil.SafeInv(Determinant);
            }
        }

        /// <summary>
        /// Find the matrix determinant.
        /// </summary>
        public double Determinant
        {
            get
            {
                int M = Rows;
                int N = Columns;

                if (N != M)
                    throw new ArgumentException("Matrix need to be square to find determinant.");

                if (M == 1)
                    return array[0, 0];

                if (M == 2)
                    return array[0, 0] * array[1, 1] - array[0, 1] * array[1, 0];

                double sum = 0.0;
                for (int i = 0; i < N; i++)
                    sum += ChangeSign(i) * array[0, i] * SubMatrix(0, i).Determinant;

                return sum;
            }
        }

        /// <summary>
        /// Find the matrix cofactor.
        /// </summary>
        public Matrix Cofactor
        {
            get
            {
                int M = Rows;
                int N = Columns;

                var mat = new Matrix(M, N);

                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        mat[i, j] = ChangeSign(i) * ChangeSign(j) * SubMatrix(i, j).Determinant;
                    }
                }

                return mat;
            }
        }

        /// <summary>
        /// Returns a new matrix that is one row and column
        /// smaller than the original.
        /// </summary>
        public Matrix SubMatrix(int excluding_row, int excluding_col)
        {
            int M = Rows;
            int N = Columns;

            var mat = new Matrix(M - 1, N - 1);

            int r = -1;
            for (int i = 0; i < M; i++)
            {
                if (i == excluding_row) continue;

                r++;
                int c = -1;
                for (int j = 0; j < N; j++)
                {
                    if (j == excluding_col) continue;

                    mat[r, ++c] = array[i, j];
                }
            }

            return mat;
        }

        private static int ChangeSign(int i)
        {
            if (i % 2 == 0)
                return 1;
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Matrix: Rows={0}, Columns={1}]", Rows, Columns);
        }

        public Vector GetRow(int i)
        {
            var vec = new Vector(Columns);
            for (int j = 0; j < Columns; j++)
                vec[j] = this[i, j];

            return vec;
        }

        public void SetRow(int i, double x, double y)
        {
            array[i, 0] = x;
            array[i, 1] = y;
        }

        public void SetRow(int i, double x, double y, double z)
        {
            array[i, 0] = x;
            array[i, 1] = y;
            array[i, 2] = z;
        }

        public void SetRow(int i, double x, double y, double z, double w)
        {
            array[i, 0] = x;
            array[i, 1] = y;
            array[i, 2] = z;
            array[i, 3] = w;
        }

        public void SetRow(int i, IList<double> row)
        {
            for (int j = 0; j < Columns; j++)
                array[i, j] = row[j];
        }

        public void SetRow(int i, Vector row)
        {
            for (int j = 0; j < Columns; j++)
                array[i, j] = row[j];
        }

        public Vector GetColumn(int j)
        {
            var vec = new Vector(Rows);
            for (int i = 0; i < Rows; i++)
                vec[i] = this[i, j];

            return vec;
        }

        public void SetColumn(int j, double x, double y)
        {
            array[0, j] = x;
            array[1, j] = y;
        }

        public void SetColumn(int j, double x, double y, double z)
        {
            array[0, j] = x;
            array[1, j] = y;
            array[2, j] = z;
        }

        public void SetColumn(int j, double x, double y, double z, double w)
        {
            array[0, j] = x;
            array[1, j] = y;
            array[2, j] = z;
            array[3, j] = w;
        }

        public void SetColumn(int j, IList<double> col)
        {
            for (int i = 0; i < Rows; i++)
                array[i, j] = col[i];
        }

        public void SetColumn(int j, Vector col)
        {
            for (int i = 0; i < Rows; i++)
                array[i, j] = col[i];
        }

        /// <summary>
        /// Return a copy of the matrix.
        /// </summary>
        public Matrix Copy()
        {
            var copy = new double[Rows, Columns];
            Array.Copy(array, copy, array.Length);
            return new Matrix(copy);
        }

        /// <summary>
        /// Return a copy of the matrix as a array.
        /// </summary>
        public double[,] ToArray()
        {
            var copy = new double[Rows, Columns];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        /// <summary>
        /// Return a copy of the matrix as a list of lists.
        /// </summary>
        /// <returns></returns>
        public List<List<double>> ToList()
        {
            var mat = new List<List<double>>();

            for (int i = 0; i < Rows; i++)
            {
                var row = new List<double>();

                for (int j = 0; j < Columns; j++)
                {
                    row.Add(array[i, j]);
                }

                mat.Add(row);
            }

            return mat;
        }

        /// <summary>
        /// Return a list where each row in matrix is a vector.
        /// </summary>
        public List<Vector> GetRowVectors()
        {
            var vectors = new List<Vector>(Rows);

            for (int i = 0; i < Rows; i++)
                vectors.Add(GetRow(i));

            return vectors;
        }

        /// <summary>
        /// Return a list where each column in matrix is a vector.
        /// </summary>
        public List<Vector> GetColumnVectors()
        {
            var vectors = new List<Vector>(Columns);

            for (int i = 0; i < Columns; i++)
                vectors.Add(GetColumn(i));

            return vectors;
        }

        /// <summary>
        /// Solve a system of equations.
        /// </summary>
        public static Vector Solve(Matrix A, Vector b)
        {
            return LUsolve(LU(A.Copy()), b);
        }

        /// <summary>
        /// Solve a system of equations.
        /// Based on methods from numeric.js
        /// </summary>
        private static Vector LUsolve(LUDecomp LUP, Vector b)
        {
            int i, j;
            var LU = LUP.LU;
            var n = LU.Rows;
            var x = b.Copy();
            var P = LUP.P;
            int Pi;

            i = n - 1;
            while (i != -1)
            {
                x[i] = b[i];
                --i;
            }

            i = 0;
            while (i < n)
            {
                Pi = P[i];
                if (P[i] != i)
                {
                    double tmp = x[i];
                    x[i] = x[Pi];
                    x[Pi] = tmp;
                }

                j = 0;
                while (j < i)
                {
                    x[i] -= x[j] * LU[i,j];
                    ++j;
                }
                ++i;
            }

            i = n - 1;
            while (i >= 0)
            {
                j = i + 1;
                while (j < n)
                {
                    x[i] -= x[j] * LU[i,j];
                    ++j;
                }

                x[i] /= LU[i,i];
                --i;
            }

            return x;
        }

        /// <summary>
        /// Solve a system of equations.
        /// Based on methods from numeric.js
        /// </summary>
        private static LUDecomp LU(Matrix A)
        {
            int n = A.Rows, n1 = n - 1;
            var P = new int[n];

            int k = 0;
            while (k < n)
            {
                int Pk = k;
                var Ak = A.GetRow(k);
                double max = Math.Abs(Ak[k]);

                int j = k + 1;
                while (j < n)
                {
                    double absAjk = Math.Abs(A[j,k]);
                    if (max < absAjk)
                    {
                        max = absAjk;
                        Pk = j;
                    }
                    ++j;
                }
                P[k] = Pk;

                if (Pk != k)
                {
                    A.SetRow(k, A.GetRow(Pk));
                    A.SetRow(Pk, Ak);
                    Ak = A.GetRow(k);
                }

                var Akk = Ak[k];

                int i = k + 1;
                while (i < n)
                {
                    A[i,k] /= Akk;
                    ++i;
                }

                i = k + 1;
                while (i < n)
                {
                    var Ai = A.GetRow(i);
                    j = k + 1;
                    while (j < n1)
                    {
                        Ai[j] -= Ai[k] * Ak[j];
                        ++j;
                        Ai[j] -= Ai[k] * Ak[j];
                        ++j;
                    }

                    if (j == n1) Ai[j] -= Ai[k] * Ak[j];

                    A.SetRow(i, Ai);
                    ++i;
                }

                ++k;
            }

            return new LUDecomp(A, P);
        }

        private class LUDecomp
        {
            public Matrix LU;

            public int[] P;

            public LUDecomp(Matrix lu, int[] p)
            {
                this.LU = lu;
                this.P = p;
            }
        }

    }
}
