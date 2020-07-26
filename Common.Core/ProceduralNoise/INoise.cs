using System;
using System.Collections;

using Common.Core.Numerics;

namespace Common.Core.ProceduralNoise
{

	public interface INoise 
	{
		int Seed { get; }

        float Frequency { get; set; }

        float Amplitude { get; set; }

        Vector3f Offset { get; set; }

		float Sample1D(float x);

		float Sample2D(float x, float y);

		float Sample3D(float x, float y, float z);

        void UpdateSeed(int seed);

	}

}
