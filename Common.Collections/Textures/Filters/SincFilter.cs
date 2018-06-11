using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class SincFilter : Filter
    {
        public SincFilter(float width = 3.0f) : base(width)
        {

        }

        public override float Evaluate(float x)
        {
            return Sinc((float)Math.PI * x);
        }
    }
}
