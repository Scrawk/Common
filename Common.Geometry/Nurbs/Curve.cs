using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	public interface ICurve
	{
		List<Vector> Tessellate(int samples);

		List<Vector> CartesianControlPoints();
	}

	/// <summary>
	/// Class for holding a polynomial B-spline curve
	/// </summary>
	public class Curve : ICurve
	{

		public int Degree;

		public List<double> Knots;

		public List<Vector> ControlPoints;

		public Curve()
		{
			
		}

		public Curve(RationalCurve crv) : 
			this(crv.Degree, crv.Knots, crv.ControlPoints) 
		{
			
		}

		public Curve(int degree, IList<double> knots, IList<Vector> control_points)
		{
			this.Degree = degree;
			this.Knots = knots.ToList();
			this.ControlPoints = Util.ToList(control_points);
		}

		public List<Vector> Tessellate(int samples)
		{
			return Tess.Regular(this, 0, 1, samples);
		}

		public List<Vector> CartesianControlPoints()
		{
			return Util.HomogenousToCartesian(ControlPoints);
		}

	}

	/// <summary>
	/// Class for holding a rational B-spline curve
	/// </summary>
	public class RationalCurve : ICurve
	{
		public int Degree;

		public List<double> Knots;

		public List<Vector> ControlPoints;

		public List<double> Weights;

		public RationalCurve()
		{
			
		}

		public RationalCurve(Curve crv) :
			this(crv, null)
		{
		}

		public RationalCurve(Curve crv, IList<double> weights) :
			this(crv.Degree, crv.Knots, crv.ControlPoints, weights)
		{
		}

		public RationalCurve(int degree, IList<double> knots, IList<Vector> control_points, IList<double> weights)
		{
			this.Degree = degree;
			this.Knots = knots.ToList();
			this.ControlPoints = Util.ToList(control_points);

			if(weights == null)
			{
				this.Weights = new List<double>(control_points.Count);
				this.Weights.AddRange(control_points.Count, 1);
			}
			else
			{
				this.Weights = weights.ToList();
			}
		}

		public List<Vector> Tessellate(int samples)
		{
			return Tess.Regular(this, 0, 1, samples);
		}

		public List<Vector> CartesianControlPoints()
		{
			return Util.HomogenousToCartesian(ControlPoints);
		}
	}

} 












