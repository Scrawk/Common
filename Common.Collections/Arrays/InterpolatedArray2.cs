using Common.Core.Numerics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    public class InterpolatedArray2 : IEnumerable<float>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="width">The size of the arrays 1st dimention.</param>
        /// <param name="height">The size of the arrays 2st dimention.</param>
        public InterpolatedArray2(int width, int height)
        {
            Data = new float[width, height];
        }

        public float[,] Data { get; private set; }

        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        public int Count { get { return Data.Length; } }

        /// <summary>
        /// The size of the arrays 1st dimention.
        /// </summary>
        public int Width { get { return Data.GetLength(0); } }

        /// <summary>
        /// The size of the arrays 2st dimention.
        /// </summary>
        public int Height { get { return Data.GetLength(1); } }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        public float this[int x, int y]
        {
            get { return Data[x, y]; }
            set { Data[x, y] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[InterpolatedArray: Width={0}, Height={1}, Count={2}]", Width, Height, Count);
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> GetEnumerator()
        {
            foreach (var t in Data)
                yield return t;
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Sets all elements in the array to default value.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        /// <summary>
        /// Sample the array by clamped bilinear interpolation.
        /// </summary>
        public float GetBilinear(float u, float v)
        {
            u = MathUtil.Clamp(u * (Width - 1), 0, Width - 1);
            v = MathUtil.Clamp(v * (Height - 1), 0, Height - 1);

            int x0 = (int)u;
            int y0 = (int)v;

            int x1 = MathUtil.Clamp(x0+1, 0, Width - 1);
            int y1 = MathUtil.Clamp(y0+1, 0, Height - 1);

            float fx = x1 - u;
            float fy = y1 - v;

            float v0 = Data[x0, y0] * (1.0f - fx) + Data[x1, y0] * fx;
            float v1 = Data[x0, y1] * (1.0f - fx) + Data[x1, y1] * fx;

            return v0 * (1.0f - fy) + v1 * fy;
        }
    }
}
