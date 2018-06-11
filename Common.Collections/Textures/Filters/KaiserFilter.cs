using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public class KaiserFilter : Filter
    {

        private float m_alpha = 4.0f;

        private float m_stretch = 1.0f;

        public KaiserFilter(float width = 3.0f) : base(width)
        {

        }

        public override float Evaluate(float x)
        {
            float pi = (float)Math.PI;
            float sinc_value = Sinc(pi * x * m_stretch);
            float t = x / Width;

            if ((1 - t * t) >= 0) return sinc_value * Bessel0(m_alpha * (float)Math.Sqrt(1 - t * t)) / Bessel0(m_alpha);
            else return 0;
        }

        /// <summary>
        /// http://mathworld.wolfram.com/BesselFunctionoftheFirstKind.html
        /// http://en.wikipedia.org/wiki/Bessel_function
        /// </summary>
        private float Bessel0(float x)
        {
            const float EPSILON_RATIO = 1e-6f;
            float xh, sum, pow, ds;
            int k;

            xh = 0.5f * x;
            sum = 1.0f;
            pow = 1.0f;
            k = 0;
            ds = 1.0f;
            while (ds > sum * EPSILON_RATIO)
            {
                ++k;
                pow = pow * (xh / k);
                ds = pow * pow;
                sum = sum + ds;
            }

            return sum;
        }
    }
}
