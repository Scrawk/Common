using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Core.ProceduralNoise
{
    public class WorleyNoise : Noise
    {

        private static readonly double[] OFFSET_I = new double[] { -1.0, 0.0, 1.0 };

        private static readonly double[] OFFSET_F = new double[] { -0.5, 0.5, 1.5 };

        private const double K = 1.0 / 7.0;

        private const double Ko = 3.0 / 7.0;

        public double Jitter { get; set; }

        public VORONOI_DISTANCE Distance { get; set; }

        public VORONOI_COMBINATION Combination { get; set; }

        public WorleyNoise(int seed, double frequency, double jitter, double amplitude = 1.0) 
            : base(seed, frequency, amplitude)
        {
            Jitter = jitter;
            Distance = VORONOI_DISTANCE.EUCLIDIAN;
            Combination = VORONOI_COMBINATION.D1_D0;
        }

        public override double Sample1D(double x)
        {
            x = (x + Offset.x) * Frequency;

            int Pi0 = (int)Math.Floor(x);
            double Pf0 = Frac(x);

            Vector3i pX = new Vector3i();
            pX[0] = Perm[Pi0 - 1];
            pX[1] = Perm[Pi0];
            pX[2] = Perm[Pi0 + 1];

            double d0, d1, d2;
            double F0 = double.PositiveInfinity;
            double F1 = double.PositiveInfinity;
            double F2 = double.PositiveInfinity;

            int px, py, pz;
            double oxx, oxy, oxz;

            px = Perm[pX[0]];
            py = Perm[pX[1]];
            pz = Perm[pX[2]];

            oxx = Frac(px * K) - Ko;
            oxy = Frac(py * K) - Ko;
            oxz = Frac(pz * K) - Ko;

            d0 = Distance1(Pf0, OFFSET_F[0] + Jitter * oxx);
            d1 = Distance1(Pf0, OFFSET_F[1] + Jitter * oxy);
            d2 = Distance1(Pf0, OFFSET_F[2] + Jitter * oxz);

            if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; }
            else if (d0 < F1) { F2 = F1; F1 = d0; }
            else if (d0 < F2) { F2 = d0; }

            if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; }
            else if (d1 < F1) { F2 = F1; F1 = d1; }
            else if (d1 < F2) { F2 = d1; }

            if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; }
            else if (d2 < F1) { F2 = F1; F1 = d2; }
            else if (d2 < F2) { F2 = d2; }

            return Combine(F0, F1, F2) * Amplitude;
        }

        public override double Sample2D(double x, double y)
        {

            x = (x + Offset.x) * Frequency;
            y = (y + Offset.y) * Frequency;

            int Pi0 = (int)Math.Floor(x);
            int Pi1 = (int)Math.Floor(y);

            double Pf0 = Frac(x);
            double Pf1 = Frac(y);

            Vector3i pX = new Vector3i();
            pX[0] = Perm[Pi0 - 1];
            pX[1] = Perm[Pi0];
            pX[2] = Perm[Pi0 + 1];

            double d0, d1, d2;
            double F0 = double.PositiveInfinity;
            double F1 = double.PositiveInfinity;
            double F2 = double.PositiveInfinity;

            int px, py, pz;
            double oxx, oxy, oxz;
            double oyx, oyy, oyz;

            for (int i = 0; i < 3; i++)
            {
                px = Perm[pX[i], Pi1 - 1];
                py = Perm[pX[i], Pi1];
                pz = Perm[pX[i], Pi1 + 1];

                oxx = Frac(px * K) - Ko;
                oxy = Frac(py * K) - Ko;
                oxz = Frac(pz * K) - Ko;

                oyx = Mod(Math.Floor(px * K), 7.0) * K - Ko;
                oyy = Mod(Math.Floor(py * K), 7.0) * K - Ko;
                oyz = Mod(Math.Floor(pz * K), 7.0) * K - Ko;

                d0 = Distance2(Pf0, Pf1, OFFSET_F[i] + Jitter * oxx, -0.5f + Jitter * oyx);
                d1 = Distance2(Pf0, Pf1, OFFSET_F[i] + Jitter * oxy, 0.5f + Jitter * oyy);
                d2 = Distance2(Pf0, Pf1, OFFSET_F[i] + Jitter * oxz, 1.5f + Jitter * oyz);

                if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; }
                else if (d0 < F1) { F2 = F1; F1 = d0; }
                else if (d0 < F2) { F2 = d0; }

                if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; }
                else if (d1 < F1) { F2 = F1; F1 = d1; }
                else if (d1 < F2) { F2 = d1; }

                if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; }
                else if (d2 < F1) { F2 = F1; F1 = d2; }
                else if (d2 < F2) { F2 = d2; }

            }

            return Combine(F0, F1, F2) * Amplitude;
        }

        public override double Sample3D(double x, double y, double z)
        {

            x = (x + Offset.x) * Frequency;
            y = (y + Offset.y) * Frequency;
            z = (z + Offset.z) * Frequency;

            int Pi0 = (int)Math.Floor(x);
            int Pi1 = (int)Math.Floor(y);
            int Pi2 = (int)Math.Floor(z);

            double Pf0 = Frac(x);
            double Pf1 = Frac(y);
            double Pf2 = Frac(z);

            Vector3i pX = new Vector3i();
            pX[0] = Perm[Pi0 - 1];
            pX[1] = Perm[Pi0];
            pX[2] = Perm[Pi0 + 1];

            Vector3i pY = new Vector3i();
            pY[0] = Perm[Pi1 - 1];
            pY[1] = Perm[Pi1];
            pY[2] = Perm[Pi1 + 1];

            double d0, d1, d2;
            double F0 = 1e6;
            double F1 = 1e6;
            double F2 = 1e6;

            int px, py, pz;
            double oxx, oxy, oxz;
            double oyx, oyy, oyz;
            double ozx, ozy, ozz;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    px = Perm[pX[i], pY[j], Pi2 - 1]; 
                    py = Perm[pX[i], pY[j], Pi2]; 
                    pz = Perm[pX[i], pY[j], Pi2 + 1];

                    oxx = Frac(px * K) - Ko;
                    oxy = Frac(py * K) - Ko;
                    oxz = Frac(pz * K) - Ko;

                    oyx = Mod(Math.Floor(px * K), 7.0) * K - Ko;
                    oyy = Mod(Math.Floor(py * K), 7.0) * K - Ko;
                    oyz = Mod(Math.Floor(pz * K), 7.0) * K - Ko;

                    px = Perm[px];
                    py = Perm[py];
                    pz = Perm[pz];

                    ozx = Frac(px * K) - Ko;
                    ozy = Frac(py * K) - Ko;
                    ozz = Frac(pz * K) - Ko;

                    d0 = Distance3(Pf0, Pf1, Pf2, OFFSET_F[i] + Jitter * oxx, OFFSET_F[j] + Jitter * oyx, -0.5f + Jitter * ozx);
                    d1 = Distance3(Pf0, Pf1, Pf2, OFFSET_F[i] + Jitter * oxy, OFFSET_F[j] + Jitter * oyy, 0.5f + Jitter * ozy);
                    d2 = Distance3(Pf0, Pf1, Pf2, OFFSET_F[i] + Jitter * oxz, OFFSET_F[j] + Jitter * oyz, 1.5f + Jitter * ozz);

                    if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; }
                    else if (d0 < F1) { F2 = F1; F1 = d0; }
                    else if (d0 < F2) { F2 = d0; }

                    if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; }
                    else if (d1 < F1) { F2 = F1; F1 = d1; }
                    else if (d1 < F2) { F2 = d1; }

                    if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; }
                    else if (d2 < F1) { F2 = F1; F1 = d2; }
                    else if (d2 < F2) { F2 = d2; }
                }
            }

            return Combine(F0, F1, F2) * Amplitude;
        }

        private double Mod(double x, double y)
        {
            return x - y * Math.Floor(x / y);
        }

        private double Frac(double v)
        {
            return v - Math.Floor(v);
        }

        private double Distance1(double p1x, double p2x)
        {
            switch (Distance)
            {
                case VORONOI_DISTANCE.EUCLIDIAN:
                    return (p1x - p2x) * (p1x - p2x);

                case VORONOI_DISTANCE.MANHATTAN:
                    return Math.Abs(p1x - p2x);

                case VORONOI_DISTANCE.CHEBYSHEV:
                    return Math.Abs(p1x - p2x);
            }

            return 0;
        }

        private double Distance2(double p1x, double p1y, double p2x, double p2y)
        {
            switch (Distance)
            {
                case VORONOI_DISTANCE.EUCLIDIAN:
                    return (p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y);

                case VORONOI_DISTANCE.MANHATTAN:
                    return Math.Abs(p1x - p2x) + Math.Abs(p1y - p2y);

                case VORONOI_DISTANCE.CHEBYSHEV:
                    return Math.Max(Math.Abs(p1x - p2x), Math.Abs(p1y - p2y));
            }

            return 0;
        }

        private double Distance3(double p1x, double p1y, double p1z, double p2x, double p2y, double p2z)
        {
            switch (Distance)
            {
                case VORONOI_DISTANCE.EUCLIDIAN:
                    return (p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y) + (p1z - p2z) * (p1z - p2z);

                case VORONOI_DISTANCE.MANHATTAN:
                    return Math.Abs(p1x - p2x) + Math.Abs(p1y - p2y) + Math.Abs(p1z - p2z);

                case VORONOI_DISTANCE.CHEBYSHEV:
                    return Math.Max(Math.Max(Math.Abs(p1x - p2x), Math.Abs(p1y - p2y)), Math.Abs(p1z - p2z));
            }

            return 0;
        }

        private double Combine(double f0, double f1, double f2)
        {
            switch (Combination)
            {
                case VORONOI_COMBINATION.D0:
                    return f0;

                case VORONOI_COMBINATION.D1_D0:
                    return f1 - f0;

                case VORONOI_COMBINATION.D2_D0:
                    return f2 - f0;
            }

            return 0;
        }
    }
}
