using System;
using System.Collections;

using Common.Core.Numerics;

namespace Common.Core.ProceduralNoise
{

	public interface INoise 
	{

        Vector3d Frequency { get; set; }

        double Amplitude { get; set; }

        Vector3d Offset { get; set; }

		double Sample1D(double x);

		double Sample2D(double x, double y);

		double Sample3D(double x, double y, double z);

        void UpdateSeed(int seed);

	}

}
