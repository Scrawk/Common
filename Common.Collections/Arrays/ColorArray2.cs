using Common.Core.Numerics;
using System;
using System.Collections.Generic;

using Common.Core.Colors;

namespace Common.Collections.Arrays
{
    public class ColorArray2 : Array2<ColorRGB>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="width">The size of the arrays 1st dimention.</param>
        /// <param name="height">The size of the arrays 2st dimention.</param>
        public ColorArray2(int width, int height)
            : base(width, height)
        {

        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public ColorArray2(Vector2i size)
            : base(size.x, size.y)
        {

        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="data">The data form the array. Will be deep copied.</param>
        public ColorArray2(ColorRGB[,] data)
            : base(data)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ColorArray2: Width={0}, Height={1}, Count={2}]", Width, Height, Count);
        }

        /// <summary>
        /// Sample the array by clamped bilinear interpolation.
        /// </summary>
        public ColorRGB GetClamped(float u, float v)
        {
            float x = u * (Width-1);
            float y = v * (Height-1);

            int xi = (int)x;
            int yi = (int)y;

            var v00 = base.GetClamped(xi, yi);
            var v10 = base.GetClamped(xi + 1, yi);
            var v01 = base.GetClamped(xi, yi + 1);
            var v11 = base.GetClamped(xi + 1, yi + 1);

            var col = new ColorRGB();
            col.r = MathUtil.Blerp(v00.r, v10.r, v01.r, v11.r, x - xi, y - yi);
            col.g = MathUtil.Blerp(v00.g, v10.g, v01.g, v11.g, x - xi, y - yi);
            col.b = MathUtil.Blerp(v00.b, v10.b, v01.b, v11.b, x - xi, y - yi);
            return col;
        }

        /// <summary>
        /// Sample the array by wrapped bilinear interpolation.
        /// </summary>
        public ColorRGB GetWrapped(float u, float v)
        {
            float x = u * (Width - 1);
            float y = v * (Height - 1);

            int xi = (int)x;
            int yi = (int)y;

            var v00 = base.GetWrapped(xi, yi);
            var v10 = base.GetWrapped(xi + 1, yi);
            var v01 = base.GetWrapped(xi, yi + 1);
            var v11 = base.GetWrapped(xi + 1, yi + 1);

            var col = new ColorRGB();
            col.r = MathUtil.Blerp(v00.r, v10.r, v01.r, v11.r, x - xi, y - yi);
            col.g = MathUtil.Blerp(v00.g, v10.g, v01.g, v11.g, x - xi, y - yi);
            col.b = MathUtil.Blerp(v00.b, v10.b, v01.b, v11.b, x - xi, y - yi);
            return col;
        }

    }
}
