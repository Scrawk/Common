using System;
using System.Collections;

using Common.Core.Numerics;

namespace Common.Core.ProceduralNoise
{
    /// <summary>
    /// A concrete class for generating fractal noise.
    /// </summary>
	public class FractalNoise
    {

        /// <summary>
        /// The number of octaves in the fractal.
        /// </summary>
        public int Octaves { get; set; }

        /// <summary>
        /// The frequency of the fractal.
        /// </summary>
        public Vector3d Frequency { get; set; }

        /// <summary>
        /// The amplitude of the fractal.
        /// </summary>
        public double Amplitude { get; set; }

        /// <summary>
        /// The offset applied to each dimension.
        /// </summary>
        public Vector3d Offset { get; set; }

        /// <summary>
        /// The rate at which the amplitude changes.
        /// </summary>
        public double Lacunarity { get; set; }

        /// <summary>
        /// The rate at which the frequency changes.
        /// </summary>
        public double Gain { get; set; }

        /// <summary>
        /// The noises to sample from to generate the fractal.
        /// </summary>
        public INoise[] Noises { get; set; }

        /// <summary>
        /// The amplitudes for each octave.
        /// </summary>
        public double[] Amplitudes { get; set; }

        /// <summary>
        /// The frequencies for each octave.
        /// </summary>
        public Vector3d[] Frequencies { get; set; }

        public FractalNoise(INoise noise, int octaves, double frequency, double amplitude = 1.0)
        {

            Octaves = octaves;
            Frequency = new Vector3d(frequency);
            Amplitude = amplitude;
            Offset = Vector3d.Zero;
            Lacunarity = 2.0;
            Gain = 0.5;

            UpdateTable(new INoise[] { noise });
        }

        public FractalNoise(INoise noise, int octaves, Vector3d frequency, double amplitude = 1.0)
        {

            Octaves = octaves;
            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3d.Zero;
            Lacunarity = 2.0;
            Gain = 0.5;

            UpdateTable(new INoise[] { noise });
        }

        public FractalNoise(INoise[] noises, int octaves, Vector3d frequency, double amplitude = 1.0)
        {

            Octaves = octaves;
            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3d.Zero;
            Lacunarity = 2.0;
            Gain = 0.5;

            UpdateTable(noises);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[FractalNoise: Octaves={0}, Frequency={1}, Amplitude={2}, Offset={3}, Lacunarity={4}, Gain={5}]",
                Octaves, Frequency, Amplitude, Offset, Lacunarity, Gain);
        }

        /// <summary>
        /// Calculates the amplitudes and frequencies tables for each octave
        /// based on the fractal settings. The tables are used so individual 
        /// octaves can be sampled. Must be called when object is first created
        /// and when ever the settings are changed.
        /// </summary>
        public virtual void UpdateTable()
        {
            UpdateTable(Noises);
        }

        protected virtual void UpdateTable(INoise[] noises)
		{
			Amplitudes = new double[Octaves];
			Frequencies = new Vector3d[Octaves];
            Noises = new INoise[Octaves];

            int numNoises = noises.Length;
			
			double amp = 0.5;
            Vector3d frq = Frequency;
			for(int i = 0; i < Octaves; i++)
			{
                Noises[i] = noises[Math.Min(i, numNoises - 1)];
				Frequencies[i] = frq;
				Amplitudes[i] = amp;
				amp *= Gain;
				frq *= Lacunarity;
			}

		}
		
        /// <summary>
        /// Returns the noise value from a octave in a 1D fractal.
        /// </summary>
        /// <param name="i">The octave to sample.</param>
        /// <param name="x">A value on the x axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
		public virtual double Octave1D(int i, double x)
		{
            if (i >= Octaves) return 0.0;
            if (Noises[i] == null) return 0.0;

			x = x + Offset.x;

			double fx = Frequencies[i].x;
			return Noises[i].Sample1D(x * fx) * Amplitudes[i] * Amplitude;
		}
		
        /// <summary>
        /// Returns the noise value from a octave in a 2D fractal.
        /// </summary>
        /// <param name="i">The octave to sample.</param>
        /// <param name="x">A value on the x axis.</param>
        /// <param name="y">A value on the y axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
		public virtual double Octave2D(int i, double x, double y)
		{
            if (i >= Octaves) return 0.0;
            if (Noises[i] == null) return 0.0;

			x = x + Offset.x;
			y = y + Offset.y;

			double fx = Frequencies[i].x;
            double fy = Frequencies[i].y;
            return Noises[i].Sample2D(x * fx, y * fy) * Amplitudes[i] * Amplitude;
		}
		
        /// <summary>
        /// Returns the noise value from a octave in a 3D fractal.
        /// </summary>
        /// <param name="i">The octave to sample.</param>
        /// <param name="x">A value on the x axis.</param>
        /// <param name="y">A value on the y axis.</param>
        /// <param name="z">A value on the z axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
		public virtual double Octave3D(int i, double x, double y, double z)
		{
            if (i >= Octaves) return 0.0;
            if (Noises[i] == null) return 0.0;

			x = x + Offset.x;
			y = y + Offset.y;
			z = z + Offset.z;

            double fx = Frequencies[i].x;
            double fy = Frequencies[i].y;
            double fz = Frequencies[i].z;
            return Noises[i].Sample3D(x * fx, y * fy, z * fz) * Amplitudes[i] * Amplitude;
		}

        /// <summary>
        /// Samples a 1D fractal.
        /// </summary>
        /// <param name="x">A value on the x axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
        public virtual double Sample1D(double x)
        {
			x = x + Offset.x;

	        double sum = 0;
			for(int i = 0; i < Octaves; i++) 
	        {
                double fx = Frequencies[i].x;

                if (Noises[i] != null)
                    sum += Noises[i].Sample1D(x * fx) * Amplitudes[i];
	        }
			return sum * Amplitude;
        }

        /// <summary>
        /// Samples a 2D fractal.
        /// </summary>
        /// <param name="x">A value on the x axis.</param>
        /// <param name="y">A value on the y axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
        public virtual double Sample2D(double x, double y)
        {
			x = x + Offset.x;
			y = y + Offset.y;

	        double sum = 0;
	        for(int i = 0; i < Octaves; i++) 
	        {
                double fx = Frequencies[i].x;
                double fy = Frequencies[i].y;

                if (Noises[i] != null)
                    sum += Noises[i].Sample2D(x * fx, y * fy) * Amplitudes[i];
			}
			return sum * Amplitude;
        }

        /// <summary>
        /// Samples a 3D fractal.
        /// </summary>
        /// <param name="x">A value on the x axis.</param>
        /// <param name="y">A value on the y axis.</param>
        /// <param name="z">A value on the z axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
        public virtual double Sample3D(double x, double y, double z)
        {
			x = x + Offset.x;
			y = y + Offset.y;
			z = z + Offset.z;

	        double sum = 0;
			for(int i = 0; i < Octaves; i++) 
	        {
                double fx = Frequencies[i].x;
                double fy = Frequencies[i].y;
                double fz = Frequencies[i].z;

                if (Noises[i] != null)
                    sum += Noises[i].Sample3D(x * fx, y * fy, z * fz) * Amplitudes[i];
	        }
			return sum * Amplitude;
        }

    }

}














