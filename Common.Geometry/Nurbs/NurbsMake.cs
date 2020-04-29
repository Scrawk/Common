using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	public static class NurbsMake
	{

		/// <summary>
		/// 0) build knot vector for curve by normalized chord length
		/// 1) construct effective basis function in square matrix (W)
		/// 2) construct set of coordinattes to interpolate vector (p)
		/// 3) set of control points (c)
		/// 4) solve for c in all 3 dimensions
		/// </summary>
		/// <param name="degree">The degree of the curve.</param>
		/// <param name="points">The points to interp curve from.</param>
		/// <returns></returns>
		public static NurbsCurve FromPoints(int degree, IList<Vector> points)
		{
			if (points.Count < degree + 1)
				throw new Exception("You need to supply at least degree + 1 points.");

			var us = new List<double>();
			us.Add(0);

			for (int i = 1; i < points.Count; i++)
			{
				var chord = (points[i] - points[i - 1]).Magnitude;
				var last = us[us.Count - 1];
				us.Add(last + chord);
			}

			//normalize
			var max = us[us.Count - 1];
			for (int i = 1; i < us.Count; i++)
				us[i] = us[i] / max;

			var knotsStart = new List<double>(degree + 1);
			knotsStart.AddRange(degree + 1, 0);

			//we need two more control points, two more knots
			int start = 1;
			int end = us.Count - degree;

			for (int i = start; i < end; i++)
			{
				double weightSums = 0;
				for (int j = 0; j < degree; j++)
				{
					weightSums += us[i + j];
				}

				knotsStart.Add((1.0 / degree) * weightSums);
			}

			var knots = knotsStart.ToList();
			knots.AddRange(degree + 1, 1);

			//build matrix of basis function coeffs (TODO: use sparse rep)
			var A = new List<List<double>>();
			int n = points.Count - 1;
			int ld = points.Count - (degree + 1);

			foreach (var u in us)
			{
				int span = NurbsBasis.FindSpan(degree, knots, u);
				var basisFuncs = NurbsBasis.BSplineBasis(degree, span, knots, u);

				int ls = span - degree;

				var row = new List<double>();
				row.AddRange(ls, 0);
				row.AddRange(basisFuncs);
				row.AddRange(ld - ls, 0);

				A.Add(row);
			}

			//for each dimension, solve
			int rows = points[0].Dimension;
			int columns = points.Count;
			Matrix xs = new Matrix(rows, columns);
			Matrix M = new Matrix(A);

			for (int i = 0; i < rows; i++)
			{
				var b = new Vector(columns);

				int j = 0;
				foreach (var p in points)
					b[j++] = p[i];

				var x = Matrix.Solve(M, b);
				xs.SetRow(i, x);
			}

			var controlPts = new List<Vector>();
			for (int i = 0; i < columns; i++)
			{
				var v = new Vector(rows + 1);

				for (int j = 0; j < rows; j++)
					v[j] = xs[j, i];

				v[rows] = 1.0f;

				controlPts.Add(v);
			}

			return new NurbsCurve(degree, knots, controlPts);
		}

		/// <summary>
		/// Create a bezier curve from the control points.
		/// </summary>
		/// <param name="controlPoints">Points in counter-clockwise form.</param>
		/// <returns></returns>
		public static NurbsCurve BezierCurve(IList<Vector> controlPoints)
		{
			int count = controlPoints.Count;
			int degree = count - 1;

			var knots = new double[count * 2];
			for (int i = 0; i < count; i++)
			{
				knots[i] = 0;
				knots[count + i] = 1;
			}

			return new NurbsCurve(degree, knots, controlPoints);
		}

		/// <summary>
		/// Create a rational bezier curve from the control points and weights.
		/// </summary>
		/// <param name="controlPoints">Points in counter-clockwise form.</param>
		/// <returns></returns>
		public static RationalNurbsCurve RationalBezierCurve(IList<Vector> controlPoints, IList<double> weights)
		{
			int count = controlPoints.Count;
			int degree = count - 1;

			var knots = new double[count * 2];
			for (int i = 0; i < count; i++)
			{
				knots[i] = 0;
				knots[count + i] = 1;
			}

			return new RationalNurbsCurve(degree, knots, controlPoints, weights);
		}

		/// <summary>
		/// Create an Circle.
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius">the radius</param>
		public static RationalNurbsCurve Circle(Vector center, double radius)
		{
			var unitX = Vector.UnitX(center.Dimension);
			var unitY = Vector.UnitY(center.Dimension);
			return EllipseArc(center, radius, radius, 0, Math.PI * 2, unitX, unitY);
		}

		/// <summary>
		/// Create an Arc.
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius">the radius</param>
		/// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
		/// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
		public static RationalNurbsCurve Arc(Vector center, double radius, double minAngle, double maxAngle)
		{
			var unitX = Vector.UnitX(center.Dimension);
			var unitY = Vector.UnitY(center.Dimension);
			return EllipseArc(center, radius, radius, minAngle, maxAngle, unitX, unitY);
		}

		/// <summary>
		/// Create an Ellipse.
		/// </summary>
		/// <param name="center"></param>
		/// <param name="xradius">the x radius</param>
		/// <param name="yradius">the y radius</param>
		public static RationalNurbsCurve Ellipse(Vector center, double xradius, double yradius)
		{
			var unitX = Vector.UnitX(center.Dimension);
			var unitY = Vector.UnitY(center.Dimension);
			return EllipseArc(center, xradius, yradius, 0, Math.PI * 2, unitX, unitY);
		}

		/// <summary>
		/// Create an EllipseArc.
		/// </summary>
		/// <param name="center"></param>
		/// <param name="xradius">the x radius</param>
		/// <param name="yradius">the y radius</param>
		/// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
		/// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
		public static RationalNurbsCurve EllipseArc(Vector center, double xradius, double yradius, double minAngle, double maxAngle)
		{
			var unitX = Vector.UnitX(center.Dimension);
			var unitY = Vector.UnitY(center.Dimension);
			return EllipseArc(center, xradius, yradius, minAngle, maxAngle, unitX, unitY);
		}

		/// <summary>
		/// Generate the control points, weights, and knots of an elliptical arc.
		/// (Corresponds to Algorithm A7.1 from Piegl & Tiller)
		/// </summary>
		/// <param name="center"></param>
		/// <param name="xradius">the x radius</param>
		/// <param name="yradius">the y radius</param>
		/// <param name="startAngle">start angle of the ellipse arc, between 0 and 2pi, where 0 points at the xaxis</param>
		/// <param name="endAngle">end angle of the arc, between 0 and 2pi, greater than the start angle</param>
		/// <param name="xaxis">the x axis</param>
		/// <param name="yaxis">the y axis</param>
		/// <returns></returns>
		public static RationalNurbsCurve EllipseArc(Vector center, double xradius, double yradius, double startAngle, double endAngle, Vector xaxis, Vector yaxis)
		{
			//if the end angle is less than the start angle, do a circle
			if (endAngle < startAngle)
				endAngle = 2.0 * Math.PI + startAngle;

			double theta = endAngle - startAngle;
			int numArcs = 0;

			//how many arcs?
			if (theta <= Math.PI / 2.0)
				numArcs = 1;
			else
			{
				if (theta <= Math.PI)
					numArcs = 2;
				else if (theta <= 3 * Math.PI / 2.0)
					numArcs = 3;
				else
					numArcs = 4;
			}

			double dtheta = theta / numArcs;
			double w1 = Math.Cos(dtheta / 2.0);

			var P0 = center + (xaxis * (xradius * Math.Cos(startAngle))) + (yaxis * (yradius * Math.Sin(startAngle)));
			var T0 = (yaxis * Math.Cos(startAngle)) - (xaxis * Math.Sin(startAngle));

			var controlPoints = new Vector[numArcs * 2 + 1];
			var knots = new double[2 * numArcs + 3 + 1];

			int index = 0;
			double angle = startAngle;

			var weights = new double[numArcs * 2 + 1];

			controlPoints[0] = P0;
			weights[0] = 1;

			for (int i = 1; i < numArcs + 1; i++)
			{
				angle += dtheta;
				var P2 = center + (xaxis * (xradius * Math.Cos(angle))) + (yaxis * (yradius * Math.Sin(angle)));

				weights[index + 2] = 1;
				controlPoints[index + 2] = P2;

				var T2 = (yaxis * Math.Cos(angle)) - (xaxis * Math.Sin(angle));

				CurveCurveIntersection result;
				IntersectRays(P0, T0 * (1.0 / T0.Magnitude), P2, T2 * (1.0 / T2.Magnitude), out result);

				var P1 = P0 + (T0 * result.u0);

				weights[index + 1] = w1;
				controlPoints[index + 1] = P1;

				index += 2;

				if (i < numArcs)
				{
					P0 = P2;
					T0 = T2;
				}
			}

			int j = 2 * numArcs + 1;

			for (int i = 0; i < 3; i++)
			{
				knots[i] = 0;
				knots[i + j] = 1;
			}

			switch (numArcs)
			{
				case 2:
					knots[3] = knots[4] = 0.5;
					break;
				case 3:
					knots[3] = knots[4] = 1.0 / 3.0;
					knots[5] = knots[6] = 2.0 / 3.0;
					break;
				case 4:
					knots[3] = knots[4] = 0.25;
					knots[5] = knots[6] = 0.5;
					knots[7] = knots[8] = 0.75;
					break;
			}

			return new RationalNurbsCurve(2, knots, controlPoints, weights);
		}


		private struct CurveCurveIntersection
		{
			//where the intersection took place
			public Vector point0;

			//where the intersection took place on the second curve
			public Vector point1;

			//the parameter on the first curve
			public double u0;

			//the parameter on the second curve
			public double u1;
		}

		/// <summary>
		/// Find the closest parameter on two rays, see http://geomalgorithms.com/a07-_distance.html
		/// </summary>
		/// <param name="a0">origin for ray 1</param>
		/// <param name="a">direction of ray 1, assumed normalized</param>
		/// <param name="b0">origin for ray 2</param>
		/// <param name="b">direction of ray 2, assumed normalized</param>
		/// <returns></returns>
		private static bool IntersectRays(Vector a0, Vector a, Vector b0, Vector b, out CurveCurveIntersection result)
		{
			result = new CurveCurveIntersection();

			var dab = Vector.Dot(a, b);
			var dab0 = Vector.Dot(a, b0);
			var daa0 = Vector.Dot(a, a0);
			var dbb0 = Vector.Dot(b, b0);
			var dba0 = Vector.Dot(b, a0);
			var daa = Vector.Dot(a, a);
			var dbb = Vector.Dot(b, b);
			var div = daa * dbb - dab * dab;

			//parallel case
			if (Math.Abs(div) < DMath.EPS)
				return false;

			var num = dab * (dab0 - daa0) - daa * (dbb0 - dba0);

			result.u1 = num / div;
			result.u0 = (dab0 - daa0 + result.u1 * dab) / daa;

			result.point0 = a0 + a * result.u0;
			result.point1 = b0 + b * result.u1;

			return true;
		}
	}

}









