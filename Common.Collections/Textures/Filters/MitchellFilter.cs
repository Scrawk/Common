using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class MitchellFilter : Filter
    {

        private float p0, p2, p3;
        private float q0, q1, q2, q3;

        public MitchellFilter() : base(2.0f)
        {

            float b = 1.0f / 3.0f;
            float c = 1.0f / 3.0f;

            p0 = (6.0f - 2.0f * b) / 6.0f;
            p2 = (-18.0f + 12.0f * b + 6.0f * c) / 6.0f;
            p3 = (12.0f - 9.0f * b - 6.0f * c) / 6.0f;
            q0 = (8.0f * b + 24.0f * c) / 6.0f;
            q1 = (-12.0f * b - 48.0f * c) / 6.0f;
            q2 = (6.0f * b + 30.0f * c) / 6.0f;
            q3 = (-b - 6.0f * c) / 6.0f;

        }

        public override float Evaluate(float x)
        {
            x = Math.Abs(x);
            if (x < 1.0f) return p0 + x * x * (p2 + x * p3);
            if (x < 2.0f) return q0 + x * (q1 + x * (q2 + x * q3));
            return 0.0f;
        }
    }
}
