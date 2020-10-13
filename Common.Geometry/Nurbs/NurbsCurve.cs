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

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NurbsCurve3d()
		{

		}

		/// <summary>
		/// Create a curve using the properties of another curve.
		/// </summary>
		/// <param name="crv">The other curve.</param>
		public NurbsCurve3d(RationalNurbsCurve3d crv) :
			this(crv.Degree, crv.Knots, crv.ControlPoints)
		{

		}

		/// <summary>
		/// Create a curve new curve.
		/// </summary>
		/// <param name="degree">The curves degree.</param>
		/// <param name="knots">The curves knots.</param>
		/// <param name="control_points">The curves control points.</param>
		public NurbsCurve3d(int degree, IList<double> knots, IList<Vector3d> control_points)
		{
			Degree = degree;
			Knots = knots.ToArray();
			ControlPoints = control_points.ToArray();
		}

		/// <summary>
		/// Is this a rational curve.
		/// </summary>
		public bool IsRational => this is RationalNurbsCurve3d;

		/// <summary>
		/// The curves degree.
		/// </summary>
		public int Degree { get; private set; }

		/// <summary>
		/// The value of the first knot.
		/// </summary>
		public double FirstKnot => Knots[0];

		/// <summary>
		/// The value of the last knot.
		/// </summary>
		public double LastKnot => Knots.Last();

		/// <summary>
		/// The curves knots.
		/// </summary>
		public double[] Knots { get; private set; }

		/// <summary>
		/// The curves control points.
		/// </summary>
		public Vector3d[] ControlPoints { get; private set; }

		/// <summary>
		/// Is this a valid curve.
		/// </summary>
		public bool IsValid => NurbsCheck.CurveIsValid(this);

		/// <summary>
		/// Is this a closed curve.
		/// </summary>
		public bool IsClosed => NurbsCheck.CurveIsClosed(this);

		/// <summary>
		/// Get the point at parameter u.
		/// </summary>
		/// <param name="u">The parameter.</param>
		/// <returns>The point at u.</returns>
		public Vector3d Point(double u)
		{
			return NurbsEval.CurvePoint(this, u);
		}

		/// <summary>
		/// Get the tangent at parameter u.
		/// </summary>
		/// <param name="u">The parameter.</param>
		/// <returns>The tangent at u.</returns>
		public Vector3d Tangent(double u)
		{
			return NurbsEval.CurveTangent(this, u);
		}

		/// <summary>
		/// Create a tessellation of the curve with a 
		/// given number of samples.
		/// </summary>
		/// <param name="samples">The numbers times to sample the curve.</param>
		/// <returns>The sampled points.</returns>
		public List<Vector3d> Tessellate(int samples)
        {
			return NurbsTess.Regular(this, 0, 1, samples);
        }

		/// <summary>
		/// Create a tessellation of the curve with a 
		/// given number of samples and range.
		/// </summary>
		/// <param name="start">The parameter to start sampling.</param>
		/// <param name="end">The parameter to end sampling.</param>
		/// <param name="samples">The numbers times to sample the curve.</param>
		/// <returns>The sampled points.</returns>
		public List<Vector3d> Tessellate(double start, double end, int samples)
		{
			return NurbsTess.Regular(this, start, end, samples);
		}

		/// <summary>
		/// Estimate the length of the curve.
		/// </summary>
		/// <param name="samples">The numbers times to sample the curve.</param>
		/// <returns>The estimated length.</returns>
		public double EstimateLength(int samples)
		{
			return NurbsTess.EstimateLength(this, 0, 1, samples);
		}

		/// <summary>
		/// Normlize the curves knots so the first knot starts at 0
		/// and the last knot ends at 1.
		/// </summary>
		public void NormalizeKnots()
        {
			double fisrt = FirstKnot;
			double last = LastKnot;

			for (int i = 0; i < Knots.Length; i++)
				Knots[i] = MathUtil.Normalize(Knots[i], fisrt, last);
		}

		/// <summary>
		/// Insert a new knot into the curve and return as a new curve.
		/// </summary>
		/// <param name="crv">The curve to insert the knot into.</param>
		/// <param name="u">The parameter to insert the knot at.</param>
		/// <param name="repeat">The number of times to repeat the knot.</param>
		/// <returns>A new curve with the inserted knots.</returns>
		public static NurbsCurve3d InsertKnot(NurbsCurve3d crv, double u, int repeat = 1)
		{
			return NurbsModify.CurveKnotInsert(crv, u, repeat);
        }

		/// <summary>
		/// Split the curve a the parameter and return the two new curves.
		/// </summary>
		/// <param name="crv">The curve to split.</param>
		/// <param name="u">The parameter to split the curve at</param>
		/// <returns>The two new curves.</returns>
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












