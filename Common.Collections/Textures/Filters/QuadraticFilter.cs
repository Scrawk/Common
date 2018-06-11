using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class QuadraticFilter : Filter
    {
        public QuadraticFilter() : base(1.5f)
        {

        }

        public override float Evaluate(float x)
        {
            x = Math.Abs(x);
            if (x < 0.5f) return 0.75f - x * x;
            if (x < 1.5f)
            {
                float t = x - 1.5f;
                return 0.5f * t * t;
            }
            return 0.0f;
        }
    }
}
