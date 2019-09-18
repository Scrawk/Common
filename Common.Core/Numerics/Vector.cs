using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.Numerics
{
    public class Vector
    {

        private double[] array;

        public Vector(double x, double y)
        {
            array = new double[] { x, y };
        }

        public Vector(double x, double y, double z)
        {
            array = new double[] { x, y, z };
        }

        public Vector(double x, double y, double z, double w)
        {
            array = new double[] { x, y, z, w };
        }

        public Vector(int length)
        {
            array = new double[length];
        }

        public Vector(double[] array)
        {
            this.array = array;
        }
        public Vector(IList<float> list)
        {
            array = new double[list.Count];
            for (int i = 0; i < Length; i++)
                array[i] = list[i];
        }

        public int Length => array.Length;

        public double x => array[0];

        public double y => array[1];

        public double z => array[2];

        public double w => array[3];

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
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] + v2[i];

            return vec;
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static Vector operator +(Vector v1, double s)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] + s;

            return vec;
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static Vector operator +(double s, Vector v1)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] + s;

            return vec;
        }

        /// <summary>
        /// Negate vector.
        /// </summary>
        public static Vector operator -(Vector v)
        {
            var vec = new Vector(v.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = -v[i];

            return vec;
        }

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        public static Vector operator -(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] - v2[i];

            return vec;
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static Vector operator -(Vector v1, double s)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] - s;

            return vec;
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static Vector operator -(double s, Vector v1)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = s - v1[i];

            return vec;
        }

        /// <summary>
        /// Multiply two vectors.
        /// </summary>
        public static Vector operator *(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] * v2[i];

            return vec;
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static Vector operator *(Vector v, double s)
        {
            var vec = new Vector(v.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v[i] * s;

            return vec;
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static Vector operator *(double s, Vector v)
        {
            var vec = new Vector(v.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = s * v[i];

            return vec;
        }

        /// <summary>
        /// Divide two vectors.
        /// </summary>
        public static Vector operator /(Vector v1, Vector v2)
        {
            var vec = new Vector(v1.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v1[i] * v2[i];

            return vec;
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        public static Vector operator /(Vector v, double s)
        {
            var vec = new Vector(v.Length);
            for (int i = 0; i < vec.Length; i++)
                vec[i] = v[i] / s;

            return vec;
        }

        public double Magnitude
        {
            get => Math.Sqrt(SqrMagnitude);
        }

        public double SqrMagnitude
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Length; i++)
                    sum += array[i] * array[i];

                return sum;
            }
        }

        public Vector Normalized
        {
            get
            {
                double invLength = DMath.SafeInvSqrt(1.0, SqrMagnitude);

                var vec = new Vector(Length);
                for (int i = 0; i < Length; i++)
                    vec[i] = array[i] * invLength;

                return vec;
            }
        }

        public void Normalize()
        {
            double invLength = DMath.SafeInvSqrt(1.0, SqrMagnitude);

            for (int i = 0; i < Length; i++)
                array[i] *= invLength;
        }

        public static double Dot(Vector v0, Vector v1)
        {
            double sum = 0;
            for (int i = 0; i < v0.Length; i++)
                sum += v0[i] * v1[i];

            return sum;
        }

        public static double Cross2(Vector v0, Vector v1)
        {
            return v0.x * v1.y - v0.y * v1.x;
        }

        public static Vector Cross3(Vector v0, Vector v1)
        {
            return new Vector(v0.y * v1.z - v0.z * v1.y, v0.z * v1.x - v0.x * v1.z, v0.x * v1.y - v0.y * v1.x);
        }

        public static double Distance(Vector v0, Vector v1)
        {
            return Math.Sqrt(SqrDistance(v0, v1));
        }

        public static double SqrDistance(Vector v0, Vector v1)
        {
            double sum = 0;
            for (int i = 0; i < v0.Length; i++)
            {
                double diff = v0[i] - v1[i];
                sum += diff * diff;
            }
                
            return sum;
        }

        public void Mul(double s)
        {
            for (int i = 0; i < Length; i++)
                array[i] *= s;
        }

        public void Mul(Vector v)
        {
            for (int i = 0; i < Length; i++)
                array[i] *= v[i];
        }

        public void Add(double s)
        {
            for (int i = 0; i < Length; i++)
                array[i] += s;
        }

        public void Add(Vector v)
        {
            for (int i = 0; i < Length; i++)
                array[i] += v[i];
        }

        public void Sub(double s)
        {
            for (int i = 0; i < Length; i++)
                array[i] -= s;
        }

        public void Sub(Vector v)
        {
            for (int i = 0; i < Length; i++)
                array[i] -= v[i];
        }

        public void AddMul(double s, Vector v)
        {
            for (int i = 0; i < Length; i++)
                array[i] += s * v[i];
        }

        public void SubMul(double s, Vector v)
        {
            for (int i = 0; i < Length; i++)
                array[i] -= s * v[i];
        }

        public Vector Copy()
        {
            var copy = new double[Length];
            Array.Copy(array, copy, array.Length);
            return new Vector(copy);
        }

        public List<float> ToList()
        {
            var list = new List<float>();
            for (int i = 0; i < Length; i++)
                list.Add((float)array[i]);

            return list;
        }
    }
}
