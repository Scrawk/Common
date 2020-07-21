using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Collections.Arrays
{
    public class FloatArray1 : Array1<float>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="count">The size of the arrays 1st dimention.</param>
        public FloatArray1(int count) 
            : base(count)
        {
   
        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="data">The data form the array. Will be deep copied.</param>
        public FloatArray1(float[] data)
            : base(data)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[FloatArray1: Count={0}]", Count);
        }

        /// <summary>
        /// Sample the array by clamped bilinear interpolation.
        /// </summary>
        public float GetBilinear(float u)
        {
            float x = u * Count;

            int xi = (int)x;

            var v0 = GetClamped(xi);
            var v1 = GetClamped(xi + 1);

            return MathUtil.Lerp(v0, v1, x - xi);
        }

    }
}
