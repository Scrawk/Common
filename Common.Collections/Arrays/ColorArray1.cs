using Common.Core.Numerics;
using System;
using System.Collections.Generic;

using Common.Core.Colors;

namespace Common.Collections.Arrays
{
    public class ColorArray1 : Array1<ColorRGB>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="count">The size of the arrays 1st dimention.</param>
        public ColorArray1(int count)
            : base(count)
        {

        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="data">The data form the array. Will be deep copied.</param>
        public ColorArray1(ColorRGB[] data) 
            : base(data)
        {
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ColorArray1: Count={0}]", Count);
        }

        /// <summary>
        /// Sample the array by clamped bilinear interpolation.
        /// </summary>
        public ColorRGB GetBilinear01(float u)
        {
            float x = u * Count;
            return GetBilinear(x);
        }

        /// <summary>
        /// Sample the array by clamped bilinear interpolation.
        /// </summary>
        public ColorRGB GetBilinear(float x)
        {
            int xi = (int)x;

            var v0 = GetClamped(xi);
            var v1 = GetClamped(xi + 1);

            ColorRGB col;
            col.r = MathUtil.Lerp(v0.r, v1.r, x - xi);
            col.g = MathUtil.Lerp(v0.g, v1.g, x - xi);
            col.b = MathUtil.Lerp(v0.b, v1.b, x - xi);
            return col;
        }

    }
}
