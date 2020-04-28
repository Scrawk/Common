using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.Numerics
{
    /// <summary>
    /// A general vector class of arbitary dimension.
    /// </summary>
    public class Vector
    {

        private double[] array;

        /// <summary>
        /// Create a 2D vector.
        /// </summary>
        public Vector(double x, double y)
        {
            array = new double[] { x, y };
        }

        /// <summary>
        /// Create a 3D vector.
        /// </summary>
        public Vector(double x, double y, double z)
        {
            array = new double[] { x, y, z };
        }

        /// <summary>
        /// Create a 4D vector.
        /// </summary>
        public Vector(double x, double y, double z, double w)
        {
            array = new double[] { x, y, z, w };
        }

        /// <summary>
        /// Create a vector of dimension.
        /// </summary>
        public Vector(int dimension)
        {
            array = new double[dimension];
        }

        /// <summary>
        /// Create a vector from a list.
        /// </summary>
        public Vector(IList<double> list)
        {
            array = new double[list.Count];
            for (int i = 0; i < Dimension; i++)
                array[i] = list[i];
        }

        /// <summary>
        /// Create a vector of dimension and
        /// copy the other vector v into this.
        /// Presumes v has the same or larger dimension.
        /// </summary>
        public Vector(int dimension, Vector v)
        {
            array = new double[dimension];

            for (int i = 0; i < Dimension; i++)
                array[i] = v[i];
        }

        /// <summary>
        /// Create a vector of other vector v's dimension + 1
        /// and copy the other vector v into this.
        /// Then set the last value to w.
        /// </summary>
        public Vector(Vector v, double w)
        {
            array = new double[v.Dimension + 1];

            for (int i = 0; i < Dimension - 1; i++)
                array[i] = v[i];

            array[Dimension - 1] = w;
        }

        /// <summary>
        /// Create a vector of dimension and
        /// copy the other vector v into this then 
        /// set the last value to w.
        /// Presumes v is the same or larger as dimension - 1.
        /// </summary>
        public Vector(int dimension, Vector v, double w)
        {
            array = new double[dimension];

            for (int i = 0; i < Dimension - 1; i++)
                array[i] = v[i];

            array[Dimension - 1] = w;
        }

        public int Dimension => array.Length;

        public double x => array[0];

        public double y => array[1];

        public double z => array[2];

        public double w => array[3];

        public Vector2d xy => new Vector2d(x, y);

        public Vector3d xyz => new Vector3d(x, y, z);

        public Vector4d xyzw => new Vector4d(x, y, z, w);

        public static Vector UnitX(int dimension)
        {
            var v = new Vector(dimension);
            v[0] = 1;
            return v;
        }

        public static Vector UnitY(int dimension)
        {
            var v = new Vector(dimension);
            v[1] = 1;
            return v;
        }

        public static Vector UnitZ(int dimension)
        {
            var v = new Vector(dimension);
            v[2] = 1;
            return v;
        }

        public double this[int i]
        {
            get => array[i];
            set => array[i] = value;
        }

        /// <summary>
        /// Add two vectors.
        /// </summary>
        public static Vector operator +(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] + v2[i];

            return vec;
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static Vector operator +(Vector v1, double s)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] + s;

            return vec;
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static Vector operator +(double s, Vector v1)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] + s;

            return vec;
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        public static Vector operator -(Vector v)
        {
            var vec = new Vector(v.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = -v[i];

            return vec;
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] - v2[i];

            return vec;
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static Vector operator -(Vector v1, double s)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] - s;

            return vec;
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static Vector operator -(double s, Vector v1)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = s - v1[i];

            return vec;
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        public static Vector operator *(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] * v2[i];

            return vec;
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static Vector operator *(Vector v, double s)
        {
            var vec = new Vector(v.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v[i] * s;

            return vec;
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static Vector operator *(double s, Vector v)
        {
            var vec = new Vector(v.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = s * v[i];

            return vec;
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        public static Vector operator /(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v1[i] * v2[i];

            return vec;
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        public static Vector operator /(Vector v, double s)
        {
            var vec = new Vector(v.Dimension);
            for (int i = 0; i < vec.Dimension; i++)
                vec[i] = v[i] / s;

            return vec;
        }

        /// <summary>
        /// The last value in the vector.
        /// </summary>
        public double Last
        {
            get => array[Dimension-1];
        }

        /// <summary>
        /// The vectors magnitude.
        /// </summary>
        public double Magnitude
        {
            get => Math.Sqrt(SqrMagnitude);
        }

        /// <summary>
        /// The vectors sqr magnitude.
        /// </summary>
        public double SqrMagnitude
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Dimension; i++)
                    sum += array[i] * array[i];

                return sum;
            }
        }

        /// <summary>
        /// Returns a normalized copy
        /// of the vector.
        /// </summary>
        public Vector Normalized
        {
            get
            {
                double invLength = DMath.SafeInvSqrt(1.0, SqrMagnitude);

                var vec = new Vector(Dimension);
                for (int i = 0; i < Dimension; i++)
                    vec[i] = array[i] * invLength;

                return vec;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch(Dimension)
            {
                case 2:
                    return string.Format("[Vector: x={0}, y={1}]", x, y);

                case 3:
                    return string.Format("[Vector: x={0}, y={1}, z={2}]", x, y, z);

                case 4:
                    return string.Format("[Vector: x={0}, y={1}, z={2}, w={3}]", x, y, z, w);

                default:
                    return string.Format("[Vector: Dimension={0}]", Dimension);
            }
            
        }

        /// <summary>
        /// Normalize the vector.
        /// </summary>
        public void Normalize()
        {
            double invLength = DMath.SafeInvSqrt(1.0, SqrMagnitude);

            for (int i = 0; i < Dimension; i++)
                array[i] *= invLength;
        }

        /// <summary>
        /// Return the dot product of two vectors.
        /// </summary>
        public static double Dot(Vector v0, Vector v1)
        {
            double sum = 0;
            for (int i = 0; i < v0.Dimension; i++)
                sum += v0[i] * v1[i];

            return sum;
        }

        /// <summary>
        /// Return the cross product of two vectors
        /// assuming they are two dimensional.
        /// </summary>
        public static double Cross2(Vector v0, Vector v1)
        {
            return v0.x * v1.y - v0.y * v1.x;
        }

        /// <summary>
        /// Return the cross product of two vectors
        /// assuming they are three dimensional.
        /// </summary>
        public static Vector Cross3(Vector v0, Vector v1)
        {
            return new Vector(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);
        }

        /// <summary>
        /// The distance between two vectors.
        /// </summary>
        public static double Distance(Vector v0, Vector v1)
        {
            return Math.Sqrt(SqrDistance(v0, v1));
        }

        /// <summary>
        /// The sqr distance between two vectors.
        /// </summary>
        public static double SqrDistance(Vector v0, Vector v1)
        {
            double sum = 0;
            for (int i = 0; i < v0.Dimension; i++)
            {
                double diff = v0[i] - v1[i];
                sum += diff * diff;
            }
                
            return sum;
        }

        /// <summary>
        /// Multiply the vector with a scalar.
        /// </summary>
        public void Mul(double s)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] *= s;
        }

        /// <summary>
        /// Multiply the vector with a vector.
        /// </summary>
        public void Mul(Vector v)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] *= v[i];
        }

        /// <summary>
        /// Add the vector and a scalar.
        /// </summary>
        public void Add(double s)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] += s;
        }

        /// <summary>
        /// Add the vectors.
        /// </summary>
        public void Add(Vector v)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] += v[i];
        }

        /// <summary>
        /// Subtract the vector and a scalar.
        /// </summary>
        public void Sub(double s)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] -= s;
        }

        /// <summary>
        /// Subtract the vectors.
        /// </summary>
        public void Sub(Vector v)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] -= v[i];
        }

        /// <summary>
        /// Multiply s and v and then add it to this vector.
        /// </summary>
        public void AddMul(double s, Vector v)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] += s * v[i];
        }

        /// <summary>
        /// Multiply s and v and then subtract it to this vector.
        /// </summary>
        public void SubMul(double s, Vector v)
        {
            for (int i = 0; i < Dimension; i++)
                array[i] -= s * v[i];
        }

        /// <summary>
        /// Return a copy of this vector.
        /// </summary>
        public Vector Copy()
        {
            var copy = new double[Dimension];
            Array.Copy(array, copy, array.Length);
            return new Vector(copy);
        }

        /// <summary>
        /// Return a copy of the vector as a array.
        /// </summary>
        public double[] ToArray()
        {
            var copy = new double[Dimension];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        /// <summary>
        /// Return a copy of the vector as a list.
        /// </summary>
        public List<double> ToList()
        {
            var list = new List<double>();
            for (int i = 0; i < Dimension; i++)
                list.Add(array[i]);

            return list;
        }
    }
}
