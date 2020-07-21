using Common.Core.Numerics;
using System;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    public class FloatArray2 : Array2<float>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="width">The size of the arrays 1st dimention.</param>
        /// <param name="height">The size of the arrays 2st dimention.</param>
        public FloatArray2(int width, int height) 
            : base(width, height)
        {
   
        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public FloatArray2(Vector2i size)
            : base(size.x, size.y)
        {

        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="data">The data form the array. Will be deep copied.</param>
        public FloatArray2(float[,] data)
            : base(data)
        {
 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[FloatArray2: Width={0}, Height={1}, Count={2}]", Width, Height, Count);
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

            var v00 = GetClamped(xi, yi);
            var v10 = GetClamped(xi + 1, yi);
            var v01 = GetClamped(xi, yi + 1);
            var v11 = GetClamped(xi + 1, yi + 1);

            return MathUtil.Blerp(v00, v10, v01, v11, x - xi, y - yi);
        }

    }
}
