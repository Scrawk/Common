using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class BSplineFilter : Filter
    {

        public BSplineFilter() : base(2.0f)
        {

        }

        public override float Evaluate(float x)
        {
            x = Math.Abs(x);
            if (x < 1.0f) return (4.0f + x * x * (-6.0f + x * 3.0f)) / 6.0f;
            if (x < 2.0f)
            {
                float t = 2.0f - x;
                return t * t * t / 6.0f;
            }
            return 0.0f;
        }
    }
}
