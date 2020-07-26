using System;
using System.Collections;

using Common.Core.Numerics;

namespace Common.Core.ProceduralNoise
{

	public abstract class Noise : INoise
	{

        public float Frequency { get; set; }

        public float Amplitude { get; set; }

        public Vector3f Offset { get; set; }

        protected PermutationTable Perm { get; set; }

		public Noise(int seed, float frequency, float amplitude)
		{
            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3f.Zero;
            Perm = new PermutationTable(256, 255, seed);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Noise: Frequency={0}, Amplitude={1}, Offset={2}]", Frequency, Amplitude, Offset);
        }

        public int Seed => Perm.Seed;

        public abstract float Sample1D(float x);

		public abstract float Sample2D(float x, float y);

		public abstract float Sample3D(float x, float y, float z);

        public virtual void UpdateSeed(int seed)
        {
            Perm.Build(seed);
        }
		
	}

}












