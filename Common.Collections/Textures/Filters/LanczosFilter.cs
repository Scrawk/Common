using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class LanczosFilter : Filter
    {
        public LanczosFilter() : base(3.0f)
        {

        }

        public override float Evaluate(float x)
        {
            float pi = (float)Math.PI;
            x = Math.Abs(x);
            if (x < 3.0f) return Sinc(pi * x) * Sinc(pi * x / 3.0f);
            return 0.0f;
        }
    }
}
