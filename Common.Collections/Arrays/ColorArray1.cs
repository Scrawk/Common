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
        public ColorRGB GetClamped(float u)
        {
            float x = u * (Count-1);

            int xi = (int)x;

            var v0 = base.GetClamped(xi);
            var v1 = base.GetClamped(xi + 1);

            ColorRGB col;
            col.r = MathUtil.Lerp(v0.r, v1.r, x - xi);
            col.g = MathUtil.Lerp(v0.g, v1.g, x - xi);
            col.b = MathUtil.Lerp(v0.b, v1.b, x - xi);
            return col;
        }

        /// <summary>
        /// Sample the array by wrapped bilinear interpolation.
        /// </summary>
        public ColorRGB GetWrapped(float u)
        {
            float x = u * (Count - 1);

            int xi = (int)x;

            var v0 = base.GetWrapped(xi);
            var v1 = base.GetWrapped(xi + 1);

            ColorRGB col;
            col.r = MathUtil.Lerp(v0.r, v1.r, x - xi);
            col.g = MathUtil.Lerp(v0.g, v1.g, x - xi);
            col.b = MathUtil.Lerp(v0.b, v1.b, x - xi);
            return col;
        }

    }
}
