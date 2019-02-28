using System;
using System.Collections;

using Common.Core.LinearAlgebra;

namespace Common.Core.ProceduralNoise
{

	public abstract class Noise : INoise
	{

        public double Frequency { get; set; }

        public double Amplitude { get; set; }

        public Vector3d Offset { get; set; }

        protected PermutationTable Perm { get; set; }

		public Noise(int seed, double frequency, double amplitude)
		{
            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3d.Zero;
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

        public abstract double Sample1D(double x);

		public abstract double Sample2D(double x, double y);

		public abstract double Sample3D(double x, double y, double z);

        public virtual void UpdateSeed(int seed)
        {
            Perm.Build(seed);
        }
		
	}

}












