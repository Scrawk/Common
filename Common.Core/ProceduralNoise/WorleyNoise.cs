using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.ProceduralNoise
{
    public class WorleyNoise : Noise
    {

        private static readonly float[] OFFSET_I = new float[] { -1.0f, 0.0f, 1.0f };

        private static readonly float[] OFFSET_F = new float[] { -0.5f, 0.5f, 1.5f };

        private const float K = 1.0f / 7.0f;

        private const float Ko = 3.0f / 7.0f;

        public WorleyNoise(int seed, float frequency, float jitter, float amplitude = 1.0f) 
            : base(seed, frequency, amplitude)
        {
            Jitter = jitter;
            Distance = VORONOI_DISTANCE.EUCLIDIAN;
            Combination = VORONOI_COMBINATION.D1_D0;
        }

        public float Jitter { get; set; }

        public VORONOI_DISTANCE Distance { get; set; }

        public VORONOI_COMBINATION Combination { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[WorleyNoise: Frequency={0}, Amplitude={1}, Offset={2}, Jitter={3}, Distance={4}, Combination={5}]", 
                Frequency, Amplitude, Offset, Jitter, Distance, Combination);
        }

        public override float Sample1D(float x)
        {
            x = (x + Offset.x) * Frequency;

            int Pi0 = (int)Math.Floor(x);
            float Pf0 = Frac(x);

            Vector3i pX = new Vector3i();
            pX[0] = Perm[Pi0 - 1];
            pX[1] = Perm[Pi0];
            pX[2] = Perm[Pi0 + 1];

            float d0, d1, d2;
            float F0 = float.PositiveInfinity;
            float F1 = float.PositiveInfinity;
            float F2 = float.PositiveInfinity;
            float S = 0;

            int px, py, pz;
            float oxx, oxy, oxz;

            px = Perm[pX[0]];
            py = Perm[pX[1]];
            pz = Perm[pX[2]];

            oxx = Frac(px * K) - Ko;
            oxy = Frac(py * K) - Ko;
            oxz = Frac(pz * K) - Ko;

            d0 = Distance1(Pf0, OFFSET_F[0] + Jitter * oxx);
            d1 = Distance1(Pf0, OFFSET_F[1] + Jitter * oxy);
            d2 = Distance1(Pf0, OFFSET_F[2] + Jitter * oxz);

            if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; S = px * Perm.Inverse; }
            else if (d0 < F1) { F2 = F1; F1 = d0; }
            else if (d0 < F2) { F2 = d0; }

            if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; S = py * Perm.Inverse; }
            else if (d1 < F1) { F2 = F1; F1 = d1; }
            else if (d1 < F2) { F2 = d1; }

            if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; S = pz * Perm.Inverse; }
            else if (d2 < F1) { F2 = F1; F1 = d2; }
            else if (d2 < F2) { F2 = d2; }

            return Combine(F0, F1, F2, S) * Amplitude;
        }

        public override float Sample2D(float x, float y)
        {
            x = (x + Offset.x) * Frequency;
            y = (y + Offset.y) * Frequency;

            int Pi0 = (int)Math.Floor(x);
            int Pi1 = (int)Math.Floor(y);

            float Pf0 = Frac(x);
            float Pf1 = Frac(y);

            Vector3i pX = new Vector3i();
            pX[0] = Perm[Pi0 - 1];
            pX[1] = Perm[Pi0];
            pX[2] = Perm[Pi0 + 1];

            float d0, d1, d2;
            float F0 = float.PositiveInfinity;
            float F1 = float.PositiveInfinity;
            float F2 = float.PositiveInfinity;
            float S = 0;

            int px, py, pz;
            float oxx, oxy, oxz;
            float oyx, oyy, oyz;

            for (int i = 0; i < 3; i++)
            {
                px = Perm[pX[i], Pi1 - 1];
                py = Perm[pX[i], Pi1];
                pz = Perm[pX[i], Pi1 + 1];

                oxx = Frac(px * K) - Ko;
                oxy = Frac(py * K) - Ko;
                oxz = Frac(pz * K) - Ko;

                oyx = Mod(MathUtil.Floor(px * K), 7.0f) * K - Ko;
                oyy = Mod(MathUtil.Floor(py * K), 7.0f) * K - Ko;
                oyz = Mod(MathUtil.Floor(pz * K), 7.0f) * K - Ko;

                d0 = Distance2(Pf0, Pf1, OFFSET_F[i] + Jitter * oxx, -0.5f + Jitter * oyx);
                d1 = Distance2(Pf0, Pf1, OFFSET_F[i] + Jitter * oxy, 0.5f + Jitter * oyy);
                d2 = Distance2(Pf0, Pf1, OFFSET_F[i] + Jitter * oxz, 1.5f + Jitter * oyz);

                if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; S = px * Perm.Inverse; }
                else if (d0 < F1) { F2 = F1; F1 = d0; }
                else if (d0 < F2) { F2 = d0; }

                if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; S = py * Perm.Inverse; }
                else if (d1 < F1) { F2 = F1; F1 = d1; }
                else if (d1 < F2) { F2 = d1; }

                if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; S = pz * Perm.Inverse; }
                else if (d2 < F1) { F2 = F1; F1 = d2; }
                else if (d2 < F2) { F2 = d2; }

            }

            return Combine(F0, F1, F2, S) * Amplitude;
        }

        public override float Sample3D(float x, float y, float z)
        {
            x = (x + Offset.x) * Frequency;
            y = (y + Offset.y) * Frequency;
            z = (z + Offset.z) * Frequency;

            int Pi0 = (int)Math.Floor(x);
            int Pi1 = (int)Math.Floor(y);
            int Pi2 = (int)Math.Floor(z);

            float Pf0 = Frac(x);
            float Pf1 = Frac(y);
            float Pf2 = Frac(z);

            Vector3i pX = new Vector3i();
            pX[0] = Perm[Pi0 - 1];
            pX[1] = Perm[Pi0];
            pX[2] = Perm[Pi0 + 1];

            Vector3i pY = new Vector3i();
            pY[0] = Perm[Pi1 - 1];
            pY[1] = Perm[Pi1];
            pY[2] = Perm[Pi1 + 1];

            float d0, d1, d2;
            float F0 = float.PositiveInfinity;
            float F1 = float.PositiveInfinity;
            float F2 = float.PositiveInfinity;
            float S = 0;

            int px, py, pz;
            float oxx, oxy, oxz;
            float oyx, oyy, oyz;
            float ozx, ozy, ozz;

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

                    oyx = Mod(MathUtil.Floor(px * K), 7.0f) * K - Ko;
                    oyy = Mod(MathUtil.Floor(py * K), 7.0f) * K - Ko;
                    oyz = Mod(MathUtil.Floor(pz * K), 7.0f) * K - Ko;

                    px = Perm[px];
                    py = Perm[py];
                    pz = Perm[pz];

                    ozx = Frac(px * K) - Ko;
                    ozy = Frac(py * K) - Ko;
                    ozz = Frac(pz * K) - Ko;

                    d0 = Distance3(Pf0, Pf1, Pf2, OFFSET_F[i] + Jitter * oxx, OFFSET_F[j] + Jitter * oyx, -0.5f + Jitter * ozx);
                    d1 = Distance3(Pf0, Pf1, Pf2, OFFSET_F[i] + Jitter * oxy, OFFSET_F[j] + Jitter * oyy, 0.5f + Jitter * ozy);
                    d2 = Distance3(Pf0, Pf1, Pf2, OFFSET_F[i] + Jitter * oxz, OFFSET_F[j] + Jitter * oyz, 1.5f + Jitter * ozz);

                    if (d0 < F0) { F2 = F1; F1 = F0; F0 = d0; S = px * Perm.Inverse; }
                    else if (d0 < F1) { F2 = F1; F1 = d0; }
                    else if (d0 < F2) { F2 = d0; }

                    if (d1 < F0) { F2 = F1; F1 = F0; F0 = d1; S = py * Perm.Inverse; }
                    else if (d1 < F1) { F2 = F1; F1 = d1; }
                    else if (d1 < F2) { F2 = d1; }

                    if (d2 < F0) { F2 = F1; F1 = F0; F0 = d2; S = pz * Perm.Inverse; }
                    else if (d2 < F1) { F2 = F1; F1 = d2; }
                    else if (d2 < F2) { F2 = d2; }
                }
            }

            return Combine(F0, F1, F2, S) * Amplitude;
        }

        private float Mod(float x, float y)
        {
            return x - y * MathUtil.Floor(x / y);
        }

        private float Frac(float v)
        {
            return v - MathUtil.Floor(v);
        }

        private float Distance1(float p1x, float p2x)
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

        private float Distance2(float p1x, float p1y, float p2x, float p2y)
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

        private float Distance3(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z)
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

        private float Combine(float f0, float f1, float f2, float s)
        {
            switch (Combination)
            {
                case VORONOI_COMBINATION.D0:
                    return f0;

                case VORONOI_COMBINATION.D1_D0:
                    return f1 - f0;

                case VORONOI_COMBINATION.D2_D0:
                    return f2 - f0;

                case VORONOI_COMBINATION.SOLID:
                    return s;
            }

            return 0;
        }
    }
}
