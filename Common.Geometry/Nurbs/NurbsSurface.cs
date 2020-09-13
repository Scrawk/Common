using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	/// <summary>
	/// Class for representing a non-rational NURBS surface
	/// </summary>
	public class NurbsSurface3d
	{


		public NurbsSurface3d()
		{

		}

		public NurbsSurface3d(RationalNurbsSurface3d srf) :
			this(srf.DegreeU, srf.DegreeV, srf.KnotsU, srf.KnotsV, srf.ControlPoints)
		{
		}

		public NurbsSurface3d(int degree_u, int degree_v, IList<double> knots_u, IList<double> knots_v, Vector3d[,] control_points)
		{
			DegreeU = degree_u;
			DegreeV = degree_v;
			KnotsU = knots_u.ToArray();
			KnotsV = knots_v.ToArray();
			ControlPoints = control_points.Copy();
		}

		public bool IsRational => this is RationalNurbsSurface3d;

		public int DegreeU { get; private set; }

		public int DegreeV { get; private set; }

		public double FirstKnotU => KnotsU[0];

		public double LastKnotU => KnotsU.Last();

		public double FirstKnotV => KnotsV[0];

		public double LastKnotV => KnotsV.Last();

		public double[] KnotsU { get; private set; }

		public double[] KnotsV { get; private set; }

		public Vector3d[,] ControlPoints { get; private set; }

		public bool IsValid => NurbsCheck.SurfaceIsValid(this);

		public bool IsClosedU => NurbsCheck.SurfaceIsClosedU(this);

		public bool IsClosedV => NurbsCheck.SurfaceIsClosedV(this);

		public Vector3d Point(double u, double v)
		{
			return NurbsEval.SurfacePoint(this, u, v);
		}

		public (Vector3d, Vector3d) Tangent(double u, double v)
		{
			return NurbsEval.SurfaceTangent(this, u, v);
		}

		public Vector3d Normal(double u, double v)
		{
			return NurbsEval.SurfaceNormal(this, u, v);
		}

		public void NormalizeKnotsU()
		{
			double fisrt = FirstKnotU;
			double last = LastKnotU;

			for (int i = 0; i < KnotsU.Length; i++)
				KnotsU[i] = MathUtil.Normalize(KnotsU[i], fisrt, last);
		}

		public void NormalizeKnotsV()
		{
			double fisrt = FirstKnotV;
			double last = LastKnotV;

			for (int i = 0; i < KnotsV.Length; i++)
				KnotsV[i] = MathUtil.Normalize(KnotsV[i], fisrt, last);
		}

		public static NurbsSurface3d InsertKnotU(NurbsSurface3d srf, double u, int repeat = 1)
		{
			return NurbsModify.SurfaceKnotInsertU(srf, u, repeat);
		}

		public static NurbsSurface3d InsertKnotV(NurbsSurface3d srf, double v, int repeat = 1)
		{
			return NurbsModify.SurfaceKnotInsertV(srf, v, repeat);
		}

		public static (NurbsSurface3d, NurbsSurface3d) SplitU(NurbsSurface3d srf, double u)
		{
			var surfaces = NurbsModify.SurfaceSplitU(srf, u);
			surfaces.Item1.NormalizeKnotsU();
			surfaces.Item2.NormalizeKnotsU();
			return surfaces;
		}

		public static (NurbsSurface3d, NurbsSurface3d) SplitV(NurbsSurface3d srf, double v)
		{
			var surfaces = NurbsModify.SurfaceSplitV(srf, v);
			surfaces.Item1.NormalizeKnotsV();
			surfaces.Item2.NormalizeKnotsV();
			return surfaces;
		}
	}

	/// <summary>
	/// Class for representing a non-rational NURBS surface
	/// </summary>
	public class RationalNurbsSurface3d : NurbsSurface3d
	{

		public RationalNurbsSurface3d()
		{

		}

		public RationalNurbsSurface3d(NurbsSurface3d srf) :
			this(srf, null)
		{
		}

		public RationalNurbsSurface3d(NurbsSurface3d srf, double[,] weights) :
			this(srf.DegreeU, srf.DegreeV, srf.KnotsU, srf.KnotsV, srf.ControlPoints, weights)
		{
		}

		public RationalNurbsSurface3d(int degree_u, int degree_v, IList<double> knots_u, IList<double> knots_v, Vector3d[,] control_points, double[,] weights) :
				base(degree_u, degree_v, knots_u, knots_v, control_points)
		{
			if (weights == null)
			{
				int width = control_points.GetLength(0);
				int height = control_points.GetLength(1);
				Weights = new double[width, height];
				Weights.Fill(1);
			}
			else
			{
				Weights = weights.Copy();
			}

		}

		public double[,] Weights { get; private set; }

		public static RationalNurbsSurface3d InsertKnotU(RationalNurbsSurface3d srf, double u, int repeat = 1)
		{
			return NurbsModify.RationalSurfaceKnotInsertU(srf, u, repeat);
		}

		public static RationalNurbsSurface3d InsertKnotV(RationalNurbsSurface3d srf, double v, int repeat = 1)
		{
			return NurbsModify.RationalSurfaceKnotInsertV(srf, v, repeat);
		}

		public static (RationalNurbsSurface3d, RationalNurbsSurface3d) SplitU(RationalNurbsSurface3d srf, double u)
        {
			var surfaces = NurbsModify.RationalSurfaceSplitU(srf, u);
			surfaces.Item1.NormalizeKnotsU();
			surfaces.Item2.NormalizeKnotsU();
			return surfaces;
		}

		public static (RationalNurbsSurface3d, RationalNurbsSurface3d) SplitV(RationalNurbsSurface3d srf, double v)
		{
			var surfaces = NurbsModify.RationalSurfaceSplitV(srf, v);
			surfaces.Item1.NormalizeKnotsV();
			surfaces.Item2.NormalizeKnotsV();
			return surfaces;
		}

	}

}

