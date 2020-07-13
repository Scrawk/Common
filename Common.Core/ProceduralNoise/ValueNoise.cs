﻿using System;
using System.Collections;
using Common.Core.Numerics;

namespace Common.Core.ProceduralNoise
{

    /// <summary>
    /// Simple noise implementation by interpolating random values.
    /// Works same as Perlin noise but uses the values instead of gradients.
    /// Perlin noise uses gradients as it makes better noise but this still
   ///  looks good and might be a little faster.
    /// </summary>
	public class ValueNoise : Noise
	{

        public ValueNoise(int seed, float frequency, float amplitude = 1.0f) 
            : base(seed, frequency, amplitude)
        {

        }

        public ValueNoise(int seed, Vector3f frequency, float amplitude = 1.0f)
            : base(seed, frequency, amplitude)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ValueNoise: Frequency={0}, Amplitude={1}, Offset={2}]", Frequency, Amplitude, Offset);
        }

        public override float Sample1D(float x)
        {
            x = (x + Offset.x) * Frequency.x;

            int ix0;
            float fx0;
            float s, n0, n1;

            ix0 = (int)Math.Floor(x);     // Integer part of x
            fx0 = x - ix0;                // Fractional part of x

            s = FADE(fx0);

            n0 = Perm[ix0];
            n1 = Perm[ix0 + 1];

            // rescale from 0 to 255 to -1 to 1.
            float n = LERP(s, n0, n1) * Perm.Inverse;
            n = n * 2.0f - 1.0f;

            return n * Amplitude;
        }

        public override float Sample2D(float x, float y)
        {
            x = (x + Offset.x) * Frequency.x;
            y = (y + Offset.y) * Frequency.y;

            int ix0, iy0;
            float fx0, fy0, s, t, nx0, nx1, n0, n1;

            ix0 = (int)Math.Floor(x);   // Integer part of x
            iy0 = (int)Math.Floor(y);   // Integer part of y

            fx0 = x - ix0;              // Fractional part of x
            fy0 = y - iy0;        		// Fractional part of y

            t = FADE(fy0);
            s = FADE(fx0);

            nx0 = Perm[ix0, iy0];
            nx1 = Perm[ix0, iy0 + 1];

            n0 = LERP(t, nx0, nx1);

            nx0 = Perm[ix0 + 1, iy0];
            nx1 = Perm[ix0 + 1, iy0 + 1];

            n1 = LERP(t, nx0, nx1);

            // rescale from 0 to 255 to -1 to 1.
            float n = LERP(s, n0, n1) * Perm.Inverse;
            n = n * 2.0f - 1.0f;

            return n * Amplitude;
        }

        public override float Sample3D(float x, float y, float z)
        {

            x = (x + Offset.x) * Frequency.x;
            y = (y + Offset.y) * Frequency.y;
            z = (z + Offset.z) * Frequency.z;

            int ix0, iy0, iz0;
            float fx0, fy0, fz0;
            float s, t, r;
            float nxy0, nxy1, nx0, nx1, n0, n1;

            ix0 = (int)Math.Floor(x);   // Integer part of x
            iy0 = (int)Math.Floor(y);   // Integer part of y
            iz0 = (int)Math.Floor(z);   // Integer part of z
            fx0 = x - ix0;              // Fractional part of x
            fy0 = y - iy0;              // Fractional part of y
            fz0 = z - iz0;              // Fractional part of z

            r = FADE(fz0);
            t = FADE(fy0);
            s = FADE(fx0);

            nxy0 = Perm[ix0, iy0, iz0];
            nxy1 = Perm[ix0, iy0, iz0 + 1];
            nx0 = LERP(r, nxy0, nxy1);

            nxy0 = Perm[ix0, iy0 + 1, iz0];
            nxy1 = Perm[ix0, iy0 + 1, iz0 + 1];
            nx1 = LERP(r, nxy0, nxy1);

            n0 = LERP(t, nx0, nx1);

            nxy0 = Perm[ix0 + 1, iy0, iz0];
            nxy1 = Perm[ix0 + 1, iy0, iz0 + 1];
            nx0 = LERP(r, nxy0, nxy1);

            nxy0 = Perm[ix0 + 1, iy0 + 1, iz0];
            nxy1 = Perm[ix0 + 1, iy0 + 1, iz0 + 1];
            nx1 = LERP(r, nxy0, nxy1);

            n1 = LERP(t, nx0, nx1);

            // rescale from 0 to 255 to -1 to 1.
            float n = LERP(s, n0, n1) * Perm.Inverse;
            n = n * 2.0f - 1.0f;

            return n * Amplitude;
        }

        private static float FADE(float t) 
        { 
            return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f); 
        }

        private static float LERP(float t, float a, float b) 
        { 
            return a + t * (b - a); 
        }

	}

}





