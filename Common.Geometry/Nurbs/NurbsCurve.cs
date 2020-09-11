using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{


	/// <summary>
	/// Class for holding a polynomial B-spline curve
	/// </summary>
	public class NurbsCurve
	{

		public int Degree;

		public List<double> Knots;

		public List<Vector> ControlPoints;

		public NurbsCurve()
		{
			
		}

		public NurbsCurve(RationalNurbsCurve crv) : 
			this(crv.Degree, crv.Knots, crv.ControlPoints) 
		{
			
		}

		public NurbsCurve(int degree, IList<double> knots, IList<Vector> control_points)
		{
			this.Degree = degree;
			this.Knots = knots.ToList();
			this.ControlPoints = NurbsUtil.ToList(control_points);
		}

		public bool IsValid => NurbsCheck.CurveIsValid(this);

		public Vector Point(double u)
		{
			return NurbsEval.CurvePoint(this, u);
		}

		public Vector Tangent(double u)
		{
			return NurbsEval.CurveTangent(this, u);
		}

		public List<Vector> Tessellate(int samples)
		{
			return NurbsTess.Regular(this, 0, 1, samples);
		}

		public static NurbsCurve InsertKnot(NurbsCurve curve, double u, int repeats = 1)
        {
			return NurbsModify.CurveKnotInsert(curve, u, repeats);
        }

	}

	/// <summary>
	/// Class for holding a rational B-spline curve
	/// </summary>
	public class RationalNurbsCurve
	{
		public int Degree;

		public List<double> Knots;

		public List<Vector> ControlPoints;

		public List<double> Weights;

		public RationalNurbsCurve()
		{
			
		}

		public RationalNurbsCurve(NurbsCurve crv) :
			this(crv, null)
		{
		}

		public RationalNurbsCurve(NurbsCurve crv, IList<double> weights) :
			this(crv.Degree, crv.Knots, crv.ControlPoints, weights)
		{
		}

		public RationalNurbsCurve(int degree, IList<double> knots, IList<Vector> control_points, IList<double> weights)
		{
			Degree = degree;
			Knots = knots.ToList();
			ControlPoints = NurbsUtil.ToList(control_points);

			if(weights == null)
			{
				Weights = new List<double>(control_points.Count);
				Weights.AddRange(control_points.Count, 1);
			}
			else
			{
				Weights = weights.ToList();
			}
		}

		public Vector Point(double u)
		{
			return NurbsEval.CurvePoint(this, u);
		}

		public Vector Tangent(double u)
		{
			return NurbsEval.CurveTangent(this, u);
		}

		public List<Vector> Tessellate(int samples)
		{
			return NurbsTess.Regular(this, 0, 1, samples);
		}


	}

} 












