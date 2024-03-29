using System;
using System.Collections;
using System.Runtime.InteropServices;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using POINT2 = Common.Core.Numerics.Point2f;

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
    public struct Matrix2x2f
    {
        /// <summary>
        /// The matrix
        /// </summary>
        public REAL m00, m10;
        public REAL m01, m11;

        /// <summary>
        /// The Matrix Idenity.
        /// </summary>
        static readonly public Matrix2x2f Identity = new Matrix2x2f(1, 0, 0, 1);

        /// <summary>
        /// A matrix from the following varibles.
        /// </summary>
        public Matrix2x2f(REAL m00, REAL m01, REAL m10, REAL m11)
        {
			this.m00 = m00; this.m01 = m01;
			this.m10 = m10; this.m11 = m11;
        }

        /// <summary>
        /// A matrix from the following column vectors.
        /// </summary>
        public Matrix2x2f(VECTOR2 c0, VECTOR2 c1)
        {
            m00 = c0.x; m01 = c1.x;
            m10 = c0.y; m11 = c1.y;
        }

        /// <summary>
        /// A matrix from the following varibles.
        /// </summary>
        public Matrix2x2f(REAL v)
        {
            m00 = v; m01 = v;
            m10 = v; m11 = v;
        }

        /// <summary>
        /// A matrix copied from a array of varibles.
        /// </summary>
        public Matrix2x2f(REAL[,] m)
        {
            m00 = m[0,0]; m01 = m[0,1];
            m10 = m[1,0]; m11 = m[1,1];
        }

        /// <summary>
        /// Access the varible at index i
        /// </summary>
        unsafe public REAL this[int i]
        {
            get
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Matrix2x2f index out of range.");

                fixed (Matrix2x2f* array = &this) { return ((REAL*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Matrix2x2f index out of range.");

                fixed (REAL* array = &m00) { array[i] = value; }
            }
        }

        /// <summary>
        /// Access the varible at index ij
        /// </summary>
        public REAL this[int i, int j]
        {
            get => this[i + j * 2];
            set => this[i + j * 2] = value;
        }

        /// <summary>
        /// Is this the identity matrix.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        if (x == y)
                        {
                            if (!MathUtil.IsOne(this[x, y]))
                                return false;
                        }
                        else
                        {
                            if (!MathUtil.IsZero(this[x, y]))
                                return false;
                        }

                    }
                }

                return true;
            }
        }

        /// <summary>
        /// The transpose of the matrix. The rows and columns are flipped.
        /// </summary>
        public Matrix2x2f Transpose
        {
            get
            {
                Matrix2x2f kTranspose = new Matrix2x2f();
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
        public REAL Determinant
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
        public Matrix2x2f Inverse
        {
            get
            {
                Matrix2x2f kInverse = Identity;
                TryInverse(ref kInverse);
                return kInverse;
            }
        }

        public REAL Trace
        {
            get
            {
                return m00 + m11;
            }
        }

        /// <summary>
        /// Add two matrices.
        /// </summary>
        public static Matrix2x2f operator +(Matrix2x2f m1, Matrix2x2f m2)
        {
            Matrix2x2f kSum = new Matrix2x2f();
            kSum.m00 = m1.m00 + m2.m00;
            kSum.m10 = m1.m10 + m2.m10;
            kSum.m01 = m1.m01 + m2.m01;
            kSum.m11 = m1.m11 + m2.m11;

            return kSum;
        }

        /// <summary>
        /// Subtract two matrices.
        /// </summary>
        public static Matrix2x2f operator -(Matrix2x2f m1, Matrix2x2f m2)
        {
            Matrix2x2f kSum = new Matrix2x2f();
            kSum.m00 = m1.m00 - m2.m00;
            kSum.m10 = m1.m10 - m2.m10;
            kSum.m01 = m1.m01 - m2.m01;
            kSum.m11 = m1.m11 - m2.m11;
            return kSum;
        }

        /// <summary>
        /// Multiply two matrices.
        /// </summary>
        public static Matrix2x2f operator *(Matrix2x2f m1, Matrix2x2f m2)
        {
            Matrix2x2f kProd = new Matrix2x2f();
            kProd.m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10;
            kProd.m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10;
            kProd.m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11;
            kProd.m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11;

            return kProd;
        }

        /// <summary>
        /// Multiply  a vector by a matrix.
        /// </summary>
        public static VECTOR2 operator *(Matrix2x2f m, VECTOR2 v)
        {
            VECTOR2 kProd = new VECTOR2();

			kProd.x = m.m00 * v.x + m.m01 * v.y;
			kProd.y = m.m10 * v.x + m.m11 * v.y;

            return kProd;
        }

        /// <summary>
        /// Multiply a point by a matrix.
        /// </summary>
        public static POINT2 operator *(Matrix2x2f m, POINT2 v)
        {
            POINT2 kProd = new POINT2();

            kProd.x = m.m00 * v.x + m.m01 * v.y;
            kProd.y = m.m10 * v.x + m.m11 * v.y;

            return kProd;
        }

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        public static Matrix2x2f operator *(Matrix2x2f m, REAL s)
        {
            Matrix2x2f kProd = new Matrix2x2f();
            kProd.m00 = m.m00 * s;
            kProd.m10 = m.m10 * s;
            kProd.m01 = m.m01 * s;
            kProd.m11 = m.m11 * s;

            return kProd;
        }

        /// <summary>
        /// Multiply a matrix by a scalar.
        /// </summary>
        public static Matrix2x2f operator *(REAL s, Matrix2x2f m)
        {
            Matrix2x2f kProd = new Matrix2x2f();
            kProd.m00 = m.m00 * s;
            kProd.m10 = m.m10 * s;
            kProd.m01 = m.m01 * s;
            kProd.m11 = m.m11 * s;

            return kProd;
        }

        /// <summary>
        /// Cast to float matrix from a double matrix.
        /// </summary>
        /// <param name="m">The other matrix</param>
        public static explicit operator Matrix2x2f(Matrix2x2d m)
        {
            var m2 = new Matrix2x2f();
            for (int i = 0; i < 9; i++)
                m2[i] = (REAL)m[i];

            return m2;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public static bool operator ==(Matrix2x2f m1, Matrix2x2f m2)
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
        public static bool operator !=(Matrix2x2f m1, Matrix2x2f m2)
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
			if(!(obj is Matrix2x2f)) return false;
			
			Matrix2x2f mat = (Matrix2x2f)obj;
			
			return this == mat;
		}

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public bool Equals(Matrix2x2f mat)
        {
            return this == mat;
        }

        /// <summary>
        /// Are these matrices equal.
        /// </summary>
        public static bool AlmostEqual(Matrix2x2f m0, Matrix2x2f m1, REAL eps = MathUtil.EPS_32)
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
                int hash = (int)MathUtil.HASH_PRIME_1;
                for (int i = 0; i < 4; i++)
                    hash = (hash * MathUtil.HASH_PRIME_2) ^ this[i].GetHashCode();
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
        public bool TryInverse(ref Matrix2x2f mInv)
        {
            REAL det = Determinant;

            if (MathUtil.IsZero(det))
                return false;

            REAL invDet = 1.0f / det;

			mInv.m00 = m11 * invDet;
			mInv.m01 = -m01 * invDet;
			mInv.m10 = -m10 * invDet;
			mInv.m11 = m00 * invDet;
            return true;
        }

        /// <summary>
        /// Get the ith column as a vector.
        /// </summary>
        public VECTOR2 GetColumn(int iCol)
        {
			return new VECTOR2(this[0, iCol], this[1, iCol]);
        }

        /// <summary>
        /// Set the ith column from avector.
        /// </summary>
        public void SetColumn(int iCol, VECTOR2 v)
        {
			this[0, iCol] = v.x;
			this[1, iCol] = v.y;
        }

        /// <summary>
        /// Get the ith row as a vector.
        /// </summary>
        public VECTOR2 GetRow(int iRow)
        {
			return new VECTOR2(this[iRow, 0], this[iRow, 1]);
        }

        /// <summary>
        /// Set the ith row from avector.
        /// </summary>
        public void SetRow(int iRow, VECTOR2 v)
        {
			this[iRow, 0 ] = v.x;
			this[iRow, 1 ] = v.y;
        }

        /// <summary>
        /// Create a rotation out of a angle.
        /// </summary>
        static public Matrix2x2f Rotate(Radian radian)
        {
            REAL ca = (REAL)Math.Cos(radian.angle);
            REAL sa = (REAL)Math.Sin(radian.angle);

            return new Matrix2x2f(ca, -sa,
                                  sa, ca);
        }

        /// <summary>
        /// Convert to a single precision 3 dimension matrix.
        /// </summary>
        public Matrix3x3f ToMatrix3x3f()
        {
			return new Matrix3x3f(m00, m01, 0.0f,
			                      m10, m11, 0.0f,
                                  0.0f, 0.0f, 0.0f);
        }

    }

}

























