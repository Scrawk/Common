using System;
using System.Collections;

using Common.Core.LinearAlgebra;

namespace Common.Core.ProceduralNoise
{

	public interface INoise 
	{

        double Frequency { get; set; }

        double Amplitude { get; set; }

        Vector3d Offset { get; set; }

		double Sample1D(double x);

		double Sample2D(double x, double y);

		double Sample3D(double x, double y, double z);

        void UpdateSeed(int seed);

	}

}
