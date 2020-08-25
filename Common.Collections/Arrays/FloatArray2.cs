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
        public float GetClamped(float u, float v)
        {
            float x = u * (Width-1);
            float y = v * (Height-1);

            int xi = (int)x;
            int yi = (int)y;

            var v00 = base.GetClamped(xi, yi);
            var v10 = base.GetClamped(xi + 1, yi);
            var v01 = base.GetClamped(xi, yi + 1);
            var v11 = base.GetClamped(xi + 1, yi + 1);

            return MathUtil.Blerp(v00, v10, v01, v11, x - xi, y - yi);
        }


        /// <summary>
        /// Sample the array by wrapped bilinear interpolation.
        /// </summary>
        public float GetWrapped(float u, float v)
        {
            float x = u * (Width - 1);
            float y = v * (Height - 1);

            int xi = (int)x;
            int yi = (int)y;

            var v00 = base.GetWrapped(xi, yi);
            var v10 = base.GetWrapped(xi + 1, yi);
            var v01 = base.GetWrapped(xi, yi + 1);
            var v11 = base.GetWrapped(xi + 1, yi + 1);

            return MathUtil.Blerp(v00, v10, v01, v11, x - xi, y - yi);
        }

    }
}
