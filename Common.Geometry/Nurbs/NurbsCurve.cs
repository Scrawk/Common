using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	public interface INurbsCurve
	{
		List<Vector> Tessellate(int samples);

		List<Vector> CartesianControlPoints();
	}

	/// <summary>
	/// Class for holding a polynomial B-spline curve
	/// </summary>
	public class NurbsCurve : INurbsCurve
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

		public List<Vector> Tessellate(int samples)
		{
			return NurbsTess.Regular(this, 0, 1, samples);
		}

		public List<Vector> CartesianControlPoints()
		{
			return NurbsUtil.HomogenousToCartesian(ControlPoints);
		}

	}

	/// <summary>
	/// Class for holding a rational B-spline curve
	/// </summary>
	public class RationalNurbsCurve : INurbsCurve
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

		public List<Vector> Tessellate(int samples)
		{
			return NurbsTess.Regular(this, 0, 1, samples);
		}

		public List<Vector> CartesianControlPoints()
		{
			return NurbsUtil.HomogenousToCartesian(ControlPoints);
		}
	}

} 












