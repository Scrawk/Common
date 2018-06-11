using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Filters
{
    public abstract class Filter
    {

        public float Width { get; private set; }

        public Filter(float width)
        {
            Width = width;
        }

        public abstract float Evaluate(float x);

        public float SampleBox(float x, float scale, int samples)
        {
            float sum = 0;
            float isamples = 1.0f / samples;

            for (int s = 0; s < samples; s++)
            {
                float p = (x + (s + 0.5f) * isamples) * scale;
                float value = Evaluate(p);
                sum += value;
            }

            return sum * isamples;
        }

        protected float Sinc(float x)
	    {
		    if (Math.Abs(x) < 0.0001f)
            {
			    //return 1.0;
			    return 1.0f + x* x*(-1.0f/6.0f + x* x*1.0f/120.0f);
            }
		    else
            {
			return (float)Math.Sin(x) / x;
		    }
	    }

    }
}
