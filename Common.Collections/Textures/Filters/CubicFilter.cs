using System;
using System.Collections.Generic;


namespace Common.Collections.Textures.Filters
{
    public class CubicFilter : Filter
    {
        public CubicFilter() : base(1.0f)
        {

        }

        public override float Evaluate(float x)
        {
            // f(t) = 2|t|^3 - 3|t|^2 + 1, -1 <= t <= 1
            x = Math.Abs(x);
            if (x < 1.0f) return ((2.0f * x - 3.0f) * x * x + 1.0f);
            return 0.0f;
        }
    }
}
