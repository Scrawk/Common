using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class TriangleFilter : Filter
    {

        public TriangleFilter() : base(1.0f)
        {

        }

        public override float Evaluate(float x)
        {
            x = Math.Abs(x);
            if (x < Width) return Width - x;
            return 0.0f;
        }

    }
}
