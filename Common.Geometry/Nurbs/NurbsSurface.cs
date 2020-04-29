using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	/// <summary>
	/// Class for representing a non-rational NURBS surface
	/// </summary>
	public class NurbsSurface
	{
		public int degree_u, degree_v;

		public List<double> knots_u, knots_v;

		public Vector[,] control_points;

		public NurbsSurface()
		{

		}

		public NurbsSurface(RationalNurbsSurface srf) :
			this(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v, srf.control_points)
		{
		}

		public NurbsSurface(int degree_u, int degree_v, IList<double> knots_u,
			IList<double> knots_v, Vector[,] control_points)
		{
			this.degree_u = degree_u;
			this.degree_v = degree_v;
			this.knots_u = knots_u.ToList();
			this.knots_v = knots_v.ToList();
			this.control_points = NurbsUtil.ToArray(control_points);
		}
	}

	/// <summary>
	/// Class for representing a non-rational NURBS surface
	/// </summary>
	public class RationalNurbsSurface
	{
		public int degree_u, degree_v;

		public List<double> knots_u, knots_v;

		public Vector[,] control_points;

		public double[,] weights;

		public RationalNurbsSurface()
		{

		}

		public RationalNurbsSurface(NurbsSurface srf) :
			this(srf, null)
		{
		}

		public RationalNurbsSurface(NurbsSurface srf, double[,] weights) :
			this(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v, srf.control_points, weights)
		{
		}

		public RationalNurbsSurface(int degree_u, int degree_v, List<double> knots_u,
			List<double> knots_v, Vector[,] control_points, double[,] weights)
		{
			this.degree_u = degree_u;
			this.degree_v = degree_v;
			this.knots_u = knots_u.ToList();
			this.knots_v = knots_v.ToList();
			this.control_points = NurbsUtil.ToArray(control_points);

			int width = control_points.GetLength(0);
			int height = control_points.GetLength(1);
			this.weights = new double[width, height];

			if (weights == null)
				this.weights.Fill(1);
			else
			{
				weights.CopyTo(this.weights, 0);
			}

		}

	}

}

