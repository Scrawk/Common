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
        /// Get the value at clamped index x,y.
        /// </summary>
        public float GetValue(int x, int y)
        {
            x = MathUtil.Clamp(x, 0, Width - 1);
            y = MathUtil.Clamp(y, 0, Height - 1);
            return Data[x, y];
        }

        /// <summary>
        /// Sample the array by clamped bilinear interpolation.
        /// </summary>
        public float GetBilinear(float u, float v)
        {
            float x = u * Width;
            float y = v * Height;

            int xi = (int)x;
            int yi = (int)y;

            var v00 = GetValue(xi, yi);
            var v10 = GetValue(xi + 1, yi);
            var v01 = GetValue(xi, yi + 1);
            var v11 = GetValue(xi + 1, yi + 1);

            return Blerp(v00, v10, v01, v11, x - xi, y - yi);
        }

        private static float Lerp(float s, float e, float t)
        {
            return s + (e - s) * t;
        }

        private static float Blerp(float c00, float c10, float c01, float c11, float tx, float ty)
        {
            return Lerp(Lerp(c00, c10, tx), Lerp(c01, c11, tx), ty);
        }
    }
}
