using System;
using System.Collections;
using Common.Core.LinearAlgebra;

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

        public ValueNoise(int seed, double frequency, double amplitude = 1.0) 
            : base(seed, frequency, amplitude)
        {

        }

        public ValueNoise(int seed, Vector3d frequency, double amplitude = 1.0)
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

        public override double Sample1D(double x)
        {
            x = (x + Offset.x) * Frequency.x;

            int ix0;
            double fx0;
            double s, n0, n1;

            ix0 = (int)Math.Floor(x);     // Integer part of x
            fx0 = x - ix0;                // Fractional part of x

            s = FADE(fx0);

            n0 = Perm[ix0];
            n1 = Perm[ix0 + 1];

            // rescale from 0 to 255 to -1 to 1.
            double n = LERP(s, n0, n1) * Perm.Inverse;
            n = n * 2.0 - 1.0;

            return n * Amplitude;
        }

        public override double Sample2D(double x, double y)
        {
            x = (x + Offset.x) * Frequency.x;
            y = (y + Offset.y) * Frequency.y;

            int ix0, iy0;
            double fx0, fy0, s, t, nx0, nx1, n0, n1;

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
            double n = LERP(s, n0, n1) * Perm.Inverse;
            n = n * 2.0 - 1.0;

            return n * Amplitude;
        }

        public override double Sample3D(double x, double y, double z)
        {

            x = (x + Offset.x) * Frequency.x;
            y = (y + Offset.y) * Frequency.y;
            z = (z + Offset.z) * Frequency.z;

            int ix0, iy0, iz0;
            double fx0, fy0, fz0;
            double s, t, r;
            double nxy0, nxy1, nx0, nx1, n0, n1;

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
            double n = LERP(s, n0, n1) * Perm.Inverse;
            n = n * 2.0 - 1.0;

            return n * Amplitude;
        }

        private double FADE(double t) { return t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }

        private double LERP(double t, double a, double b) { return a + t * (b - a); }

	}

}





