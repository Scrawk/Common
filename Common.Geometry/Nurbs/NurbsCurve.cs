using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{


	/// <summary>
	/// Class for holding a polynomial B-spline curve
	/// </summary>
	public class NurbsCurve3d
	{

		public NurbsCurve3d()
		{

		}

		public NurbsCurve3d(RationalNurbsCurve3d crv) :
			this(crv.Degree, crv.Knots, crv.ControlPoints)
		{

		}

		public NurbsCurve3d(int degree, IList<double> knots, IList<Vector3d> control_points)
		{
			Degree = degree;
			Knots = knots.ToArray();
			ControlPoints = control_points.ToArray();
		}

		public bool IsRational => this is RationalNurbsCurve3d;

		public int Degree { get; private set; }

		public double FirstKnot => Knots[0];

		public double LastKnot => Knots.Last();

		public double[] Knots { get; private set; }

		public Vector3d[] ControlPoints { get; private set; }

		public bool IsValid => NurbsCheck.CurveIsValid(this);

		public bool IsClosed => NurbsCheck.CurveIsClosed(this);

		public Vector3d Point(double u)
		{
			return NurbsEval.CurvePoint(this, u);
		}

		public Vector3d Tangent(double u)
		{
			return NurbsEval.CurveTangent(this, u);
		}

		public List<Vector3d> Tessellate(int samples)
        {
			return NurbsTess.Regular(this, 0, 1, samples);
        }

		public List<Vector3d> Tessellate(double start, double end, int samples)
		{
			return NurbsTess.Regular(this, start, end, samples);
		}

		public void NormalizeKnots()
        {
			double fisrt = FirstKnot;
			double last = LastKnot;

			for (int i = 0; i < Knots.Length; i++)
				Knots[i] = MathUtil.Normalize(Knots[i], fisrt, last);
		}

		public static NurbsCurve3d InsertKnot(NurbsCurve3d crv, double u, int repeat = 1)
		{
			return NurbsModify.CurveKnotInsert(crv, u, repeat);
        }

		public static (NurbsCurve3d, NurbsCurve3d) Split(NurbsCurve3d crv, double u)
		{
			var curves = NurbsModify.CurveSplit(crv, u);
			curves.Item1.NormalizeKnots();
			curves.Item2.NormalizeKnots();
			return curves;
		}

	}

	/// <summary>
	/// Class for holding a rational B-spline curve
	/// </summary>
	public class RationalNurbsCurve3d : NurbsCurve3d
	{

		public RationalNurbsCurve3d()
		{

		}

		public RationalNurbsCurve3d(NurbsCurve3d crv) :
			this(crv, null)
		{
		}

		public RationalNurbsCurve3d(NurbsCurve3d crv, IList<double> weights) :
			this(crv.Degree, crv.Knots, crv.ControlPoints, weights)
		{
		}

		public RationalNurbsCurve3d(int degree, IList<double> knots, IList<Vector3d> control_points, IList<double> weights) :
			base(degree, knots, control_points)
		{
			if (weights == null)
			{
				Weights = new double[control_points.Count];
				Weights.Fill(1);
			}
			else
			{
				Weights = weights.ToArray();
			}
		}

		public double[] Weights { get; private set; }

		public static RationalNurbsCurve3d InsertKnot(RationalNurbsCurve3d crv, double u, int repeat = 1)
		{
			return NurbsModify.RationalCurveKnotInsert(crv, u, repeat);
		}

		public static (RationalNurbsCurve3d, RationalNurbsCurve3d) Split(RationalNurbsCurve3d crv, double u)
		{
			var curves = NurbsModify.RationalCurveSplit(crv, u);
			curves.Item1.NormalizeKnots();
			curves.Item2.NormalizeKnots();
			return curves;
		}

	}

}












