using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Common.Core.Numerics
{

    /// <summary>
    /// Matrix is column major. Data is accessed as: row + (column*2). 
    /// Matrices can be indexed like 2D arrays but in an expression like mat[a, b], 
    /// a refers to the row index, while b refers to the column index 
    /// (note that this is the opposite way round to Cartesian coordinates).
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix2x2d
    {
        /// <summary>
        /// The matrix
        /// </summary>
        public double m00, m10;
        public double m01, m11;

        /// <summary>
        /// The Matrix Idenity.
        /// </summary>
        static readonly public Matrix2x2d Identity = new Matrix2x2d(1, 0, 0, 1);

        /// <summary>
        /// A matrix from the following varibles.
        /// </summary>
        public Matrix2x2d(double m00, double m01, double m10, double m11)
        {
			this.m00 = m00; this.m01 = m01;
			this.m10 = m10; this.m11 = m11;
        }

        /// <summary>
        /// A matrix from the following column vectors.
        /// </summary>
        public Matrix2x2d(Vector2d c0, Vector2d c1)
        {
            m00 = c0.x; m01 = c1.x;
            m10 = c0.y; m11 = c1.y;
        }

        /// <summary>
        /// A matrix from the following varibles.
        /// </summary>
        public Matrix2x2d(double v)
        {
            m00 = v; m01 = v;
            m10 = v; m11 = v;
        }

        /// <summary>
        /// A matrix copied from a array of varibles.
        /// </summary>
        public Matrix2x2d(double[,] m)
        {
            m00 = m[0,0]; m01 = m[0,1];
            m10 = m[1,0]; m11 = m[1,1];
        }

        /// <summary>
        /// Access the varible at index i
        /// </summary>
        unsafe public double this[int i]
        {
            get
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Matrix2x2d index out of range.");

                fixed (Matrix2x2d* array = &this) { return ((double*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Matrix2x2d index out of range.");

                fixed (double* array = &m00) { array[i] = value; }
            }
        }

        /// <summary>
        /// Access the varible at index ij
        /// </summary>
        public double this[int i, int j]
        {
            get => this[i + j * 2];
            set => this[i + j * 2] = value;
        }

        /// <summary>
        /// The transpose of the matrix. The rows and columns are flipped.
        /// </summary>
        public Matrix2x2d Transpose
        {
            get
            {
                Matrix2x2d kTranspose = new Matrix2x2d();
                kTranspose.m00 = m00;
                kTranspose.m10 = m01;
                kTranspose.m01 = m10;
                kTranspose.m11 = m11;

                return kTranspose;
            }
        }

        /// <summary>
        /// The determinate of a matrix. 
        /// </summary>
        public double Determinant
        {
            get
            {
                return m00 * m11 - m10 * m01;
            }
        }

        /// <summary>
        /// The inverse of the matrix.
        /// A matrix multipled by its inverse is the idenity.
        /// </summary>
        public Matrix2x2d Inverse
        {
            get
            {
                Matrix2x2d kInverse = Identity;
                TryInverse(ref kInverse);
                return kInverse;
            }
        }

        public double Trace
        {
            get
            {
                return m00 + m11;
            }
        }

        /// <summary>
        /// Add two matrices.
        /// </summary>
        public static Matrix2x2d operator +(Matrix2x2d m1, Matrix2x2d m2)
        {
            Matrix2x2d kSum = new Matrix2x2d();
            kSum.m00 = m1.m00 + m2.m00;
            kSum.m10 = m1.m10 + m2.m10;
            kSum.m01 = m1.m01 + m2.m01;
            kSum.m11 = m1.m11 + m2.m11;

            return kSum;
        }

        /// <summary>
        /// Subtract two matrices.
        /// </summary>
        public static Matrix2x2d operator -(Matrix2x2d m1, Matrix2x2d m2)
        {
            Matrix2x2d kSum = new Matrix2x2d();
            kSum.m00 = m1.m00 - m2.m00;
            kSum.m10 = m1.m10 - m2.m10;
            kSum.m01 = m1.m01 - m2.m01;
            kSum.m11 = m1.m11 - m2.m11;
            return kSum;
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        public static Matrix2x2d operator *(Matrix2x2d m1, Matrix2x2d m2)
        {
            Matrix2x2d kProd = new Matrix2x2d();
            kProd.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10;
            kProd.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10;
            kProd.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11;
            kProd.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11;

            return kProd;
        }

        /// <summary>
        /// Multiply  a vector by a matrix.
        /// </summary>
        public static Vector2d operator *(Matrix2x2d m, Vector2d v)
        {
            Vector2d kProd = new Vector2d();

			kProd.x = m.m00 * v.x + m.m01 * v.y;
			kProd.y = m.m10 * v.x + m.m11 * v.y;

            return kProd;
        }

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        public static Matrix2x2d operator *(Matrix2x2d m, double s)
        {
            Matrix2x2d kProd = new Matrix2x2d();
            kProd.m00 = m.m00 * s;
            kProd.m10 = m.m10 * s;
            kProd.m01 = m.m01 * s;
            kProd.m11 = m.m11 * s;

            return kProd;
        }

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        public static Matrix2x2d operator *(double s, Matrix2x2d m)
        {
            Matrix2x2d kProd = new Matrix2x2d();
            kProd.m00 = m.m00 * s;
            kProd.m10 = m.m10 * s;
            kProd.m01 = m.m01 * s;
            kProd.m11 = m.m11 * s;

            return kProd;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public static bool operator ==(Matrix2x2d m1, Matrix2x2d m2)
        {

            if (m1.m00 != m2.m00) return false;
            if (m1.m10 != m2.m10) return false;
            if (m1.m01 != m2.m01) return false;
            if (m1.m11 != m2.m11) return false;

            return true;
        }

        /// <summary>
        /// Are these matrices not equal.
        /// </summary>
        public static bool operator !=(Matrix2x2d m1, Matrix2x2d m2)
        {
            if (m1.m00 != m2.m00) return true;
            if (m1.m10 != m2.m10) return true;
            if (m1.m01 != m2.m01) return true;
            if (m1.m11 != m2.m11) return true;

            return false;
        }

		/// <summary>
		/// Are these matrices equal.
		/// </summary>
		public override bool Equals (object obj)
		{
			if(!(obj is Matrix2x2d)) return false;
			
			Matrix2x2d mat = (Matrix2x2d)obj;
			
			return this == mat;
		}

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public bool Equals(Matrix2x2d mat)
        {
            return this == mat;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public static bool AlmostEqual(Matrix2x2d m0, Matrix2x2d m1, double eps = MathUtil.EPS_64)
        {
            if (Math.Abs(m0.m00 - m1.m00) > eps) return false;
            if (Math.Abs(m0.m10 - m1.m10) > eps) return false;

            if (Math.Abs(m0.m01 - m1.m01) > eps) return false;
            if (Math.Abs(m0.m11 - m1.m11) > eps) return false;

            return true;
        }

        /// <summary>
        /// Matrices hash code. 
        /// </summary>
        public override int GetHashCode()
		{
            unchecked
            {
                int hash = (int)2166136261;

                for (int i = 0; i < 4; i++)
                    hash = (hash * 16777619) ^ this[i].GetHashCode();

                return hash;
            }
        }

        /// <summary>
        /// A matrix as a string.
        /// </summary>
        public override string ToString()
        {
            return this[0, 0] + "," + this[0, 1] + "\n" + this[1, 0] + "," + this[1, 1];
        }

        /// <summary>
        /// The Inverse of the matrix copied into mInv.
        /// Returns false if the matrix has no inverse.
        /// A matrix multipled by its inverse is the idenity.
        /// </summary>
        public bool TryInverse(ref Matrix2x2d mInv)
        {
            double det = Determinant;

            if (MathUtil.IsZero(det))
                return false;

            double invDet = 1.0 / det;

			mInv.m00 = m11 * invDet;
			mInv.m01 = -m01 * invDet;
			mInv.m10 = -m10 * invDet;
			mInv.m11 = m00 * invDet;
            return true;
        }

        /// <summary>
        /// Get the ith column as a vector.
        /// </summary>
        public Vector2d GetColumn(int iCol)
        {
			return new Vector2d(this[0, iCol], this[1, iCol]);
        }

        /// <summary>
        /// Set the ith column from avector.
        /// </summary>
        public void SetColumn(int iCol, Vector2d v)
        {
			this[0, iCol] = v.x;
			this[1, iCol] = v.y;
        }

        /// <summary>
        /// Get the ith row as a vector.
        /// </summar
        public Vector2d GetRow(int iRow)
        {
			return new Vector2d(this[iRow, 0], this[iRow, 1]);
        }

        /// <summary>
        /// Set the ith row from avector.
        /// </summary>
        public void SetRow(int iRow, Vector2d v)
        {
			this[iRow, 0] = v.x;
			this[iRow, 1] = v.y;
        }

        /// <summary>
        /// Create a rotation out of a angle.
        /// </summary>
        static public Matrix2x2d Rotate(double angle)
        {
            double ca = Math.Cos(angle * Math.PI / 180.0);
            double sa = Math.Sin(angle * Math.PI / 180.0);

            return new Matrix2x2d(ca, -sa,
                                  sa, ca);
        }

        /// <summary>
        /// Convert to a float precision 3 dimension matrix.
        /// </summary>
        public Matrix3x3d ToMatrix3x3d()
        {
			return new Matrix3x3d(m00, m01, 0.0,
			                      m10, m11, 0.0,
                                  0.0, 0.0, 0.0);
        }

	}

}

























