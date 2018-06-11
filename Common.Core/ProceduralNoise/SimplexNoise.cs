using System;

namespace Common.Core.ProceduralNoise
{
    /// <summary>
    /// Implementation of the Perlin simplex noise, an improved Perlin noise algorithm.
    /// Based loosely on SimplexNoise1234 by Stefan Gustavson 
    /// <http://staffwww.itn.liu.se/~stegu/aqsis/aqsis-newnoise/>
    /// </summary>
	public class SimplexNoise : Noise
    {

        public SimplexNoise(int seed, double frequency, double amplitude = 1.0) 
            : base(seed, frequency, amplitude)
        {

        }

		public override double Sample1D(double x)
        {
            //The 0.5 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.5;

            int i0 = (int)Math.Floor(x);
            int i1 = i0 + 1;
            double x0 = x - i0;
            double x1 = x0 - 1.0;

            double n0, n1;

            double t0 = 1.0 - x0*x0;
            t0 *= t0;
			n0 = t0 * t0 * Grad(Perm[i0], x0);

            double t1 = 1.0 - x1*x1;
            t1 *= t1;
			n1 = t1 * t1 * Grad(Perm[i1], x1);

            // The maximum value of this noise is 8*(3/4)^4 = 2.53125
            // A factor of 0.395 scales to fit exactly within [-1,1]
            return 0.395 * (n0 + n1) * Amplitude;
        }

		public override double Sample2D(double x, double y)
        {
            //The 0.5 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.5;
            y = (y + Offset.y) * Frequency * 0.5;

            const double F2 = 0.366025403; // F2 = 0.5*(sqrt(3.0)-1.0)
            const double G2 = 0.211324865; // G2 = (3.0-Math.sqrt(3.0))/6.0

            double n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            double s = (x+y)*F2; // Hairy factor for 2D
            double xs = x + s;
            double ys = y + s;
            int i = (int)Math.Floor(xs);
            int j = (int)Math.Floor(ys);

            double t = (i+j)*G2;
            double X0 = i-t; // Unskew the cell origin back to (x,y) space
            double Y0 = j-t;
            double x0 = x-X0; // The x,y distances from the cell origin
            double y0 = y-Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if(x0>y0) {i1=1; j1=0;} // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else {i1=0; j1=1;}      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            double x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            double y1 = y0 - j1 + G2;
            double x2 = x0 - 1.0 + 2.0 * G2; // Offsets for last corner in (x,y) unskewed coords
            double y2 = y0 - 1.0 + 2.0 * G2;

            // Calculate the contribution from the three corners
            double t0 = 0.5 - x0*x0-y0*y0;
            if(t0 < 0.0) n0 = 0.0;
            else {
                t0 *= t0;
				n0 = t0 * t0 * Grad(Perm[i, j], x0, y0); 
            }

            double t1 = 0.5 - x1*x1-y1*y1;
            if(t1 < 0.0) n1 = 0.0;
            else {
                t1 *= t1;
				n1 = t1 * t1 * Grad(Perm[i+i1, j+j1], x1, y1);
            }

            double t2 = 0.5 - x2*x2-y2*y2;
            if(t2 < 0.0) n2 = 0.0;
            else {
                t2 *= t2;
				n2 = t2 * t2 * Grad(Perm[i+1, j+1], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0 * (n0 + n1 + n2) * Amplitude; 
        }

        public override double Sample3D(double x, double y, double z)
        {
            //The 0.5 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.5;
            y = (y + Offset.y) * Frequency * 0.5;
            z = (z + Offset.z) * Frequency * 0.5;

            // Simple skewing factors for the 3D case
            const double F3 = 0.333333333;
            const double G3 = 0.166666667;

            double n0, n1, n2, n3; // Noise contributions from the four corners

            // Skew the input space to determine which simplex cell we're in
            double s = (x+y+z)*F3; // Very nice and simple skew factor for 3D
            double xs = x+s;
            double ys = y+s;
            double zs = z+s;
            int i = (int)Math.Floor(xs);
            int j = (int)Math.Floor(ys);
            int k = (int)Math.Floor(zs);

            double t = (i+j+k)*G3; 
            double X0 = i-t; // Unskew the cell origin back to (x,y,z) space
            double Y0 = j-t;
            double Z0 = k-t;
            double x0 = x-X0; // The x,y,z distances from the cell origin
            double y0 = y-Y0;
            double z0 = z-Z0;

            // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
            // Determine which simplex we are in.
            int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

            /* This code would benefit from a backport from the GLSL version! */
            if(x0>=y0) {
                if(y0>=z0)
                { i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
                else if(x0>=z0) { i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
                else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; } // Z X Y order
                }
            else { // x0<y0
                if(y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
                else if(x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
                else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
            }

            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
            // c = 1/6.

            double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;
            double x2 = x0 - i2 + 2.0*G3; // Offsets for third corner in (x,y,z) coords
            double y2 = y0 - j2 + 2.0*G3;
            double z2 = z0 - k2 + 2.0*G3;
            double x3 = x0 - 1.0 + 3.0*G3; // Offsets for last corner in (x,y,z) coords
            double y3 = y0 - 1.0 + 3.0*G3;
            double z3 = z0 - 1.0 + 3.0*G3;

            // Calculate the contribution from the four corners
            double t0 = 0.6 - x0*x0 - y0*y0 - z0*z0;
            if(t0 < 0.0) n0 = 0.0;
            else {
                t0 *= t0;
				n0 = t0 * t0 * Grad(Perm[i, j, k], x0, y0, z0);
            }

            double t1 = 0.6 - x1*x1 - y1*y1 - z1*z1;
            if(t1 < 0.0) n1 = 0.0;
            else {
                t1 *= t1;
				n1 = t1 * t1 * Grad(Perm[i+i1, j+j1, k+k1], x1, y1, z1);
            }

            double t2 = 0.6 - x2*x2 - y2*y2 - z2*z2;
            if(t2 < 0.0) n2 = 0.0;
            else {
                t2 *= t2;
				n2 = t2 * t2 * Grad(Perm[i+i2, j+j2, k+k2], x2, y2, z2);
            }

            double t3 = 0.6 - x3*x3 - y3*y3 - z3*z3;
            if(t3<0.0) n3 = 0.0;
            else {
                t3 *= t3;
				n3 = t3 * t3 * Grad(Perm[i+1, j+1, k+1], x3, y3, z3);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to stay just inside [-1,1]
            return 32.0 * (n0 + n1 + n2 + n3) * Amplitude;
        }

        private double Grad(int hash, double x)
        {
            int h = hash & 15;
            double grad = 1.0 + (h & 7);   // Gradient value 1.0, 2.0, ..., 8.0
            if ((h & 8) != 0) grad = -grad;// Set a random sign for the gradient
            return (grad * x);           // Multiply the gradient with the distance
        }

        private double Grad(int hash, double x, double y)
        {
            int h = hash & 7;           // Convert low 3 bits of hash code
            double u = h < 4 ? x : y;     // into 8 simple gradient directions,
            double v = h < 4 ? y : x;     // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0 * v : 2.0 * v);
        }

        private double Grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;      // Convert low 4 bits of hash code into 12 simple
            double u = h < 8 ? x : y; // gradient directions, and compute dot product.
            double v = h < 4 ? y : h == 12 || h == 14 ? x : z; // Fix repeats at h = 12 to 15
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v);
        }

        private double Grad(int hash, double x, double y, double z, double t)
        {
            int h = hash & 31;          // Convert low 5 bits of hash code into 32 simple
            double u = h < 24 ? x : y;    // gradient directions, and compute dot product.
            double v = h < 16 ? y : z;
            double w = h < 8 ? z : t;
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
        }

    }
}




