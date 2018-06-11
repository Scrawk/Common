using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class BoxFilter : Filter
    {

        public BoxFilter() : base(0.5f)
        {
            
        }

        public override float Evaluate(float x)
        {
            if (Math.Abs(x) <= Width) return 1.0f;
            else return 0.0f;
        }

    }
}
