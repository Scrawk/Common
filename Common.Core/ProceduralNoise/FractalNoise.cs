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
        public float Frequency { get; set; }

        /// <summary>
        /// The amplitude of the fractal.
        /// </summary>
        public float Amplitude { get; set; }

        /// <summary>
        /// The offset applied to each dimension.
        /// </summary>
        public Vector3f Offset { get; set; }

        /// <summary>
        /// The rate at which the amplitude changes.
        /// </summary>
        public float Lacunarity { get; set; }

        /// <summary>
        /// The rate at which the frequency changes.
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// The noises to sample from to generate the fractal.
        /// </summary>
        public INoise Noise { get; set; }

        public FractalNoise(INoise noise, int octaves, float frequency, float amplitude = 1.0f)
        {

            Octaves = octaves;
            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3f.Zero;
            Lacunarity = 2.0f;
            Gain = 0.5f;
            Noise = noise;
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
        /// Samples a 1D fractal.
        /// </summary>
        /// <param name="x">A value on the x axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
        public virtual float Sample1D(float x)
        {
			x = x + Offset.x;

            float amp = 0.5f;
            float frq = Frequency;

            float sum = 0;
			for(int i = 0; i < Octaves; i++) 
	        {
                sum += Noise.Sample1D(x * frq) * amp;

                amp *= Gain;
                frq *= Lacunarity;
            }
			return sum * Amplitude;
        }

        /// <summary>
        /// Samples a 2D fractal.
        /// </summary>
        /// <param name="x">A value on the x axis.</param>
        /// <param name="y">A value on the y axis.</param>
        /// <returns>A noise value between -Amp and Amp.</returns>
        public virtual float Sample2D(float x, float y)
        {
			x = x + Offset.x;
			y = y + Offset.y;

            float amp = 0.5f;
            float frq = Frequency;

            float sum = 0;
	        for(int i = 0; i < Octaves; i++) 
	        {
                sum += Noise.Sample2D(x * frq, y * frq) * amp;

                amp *= Gain;
                frq *= Lacunarity;
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
        public virtual float Sample3D(float x, float y, float z)
        {
			x = x + Offset.x;
			y = y + Offset.y;
			z = z + Offset.z;

            float amp = 0.5f;
            float frq = Frequency;

            float sum = 0;
			for(int i = 0; i < Octaves; i++) 
	        {
                sum += Noise.Sample3D(x * frq, y * frq, z * frq) * amp;

                amp *= Gain;
                frq *= Lacunarity;
            }
			return sum * Amplitude;
        }

    }

}














