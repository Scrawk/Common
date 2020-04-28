using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	internal static class Modify
	{

		/**
		* Insert knots in the curve
		* @param[in] crv Curve object
		* @param[in] u Parameter to insert knot at
		* @param[in] repeat Number of times to insert
		* @return New curve with #repeat knots inserted at u
		*/
		internal static Curve CurveKnotInsert(Curve crv, double u, int repeat = 1)
		{
			var new_crv = new Curve();
			new_crv.Degree = crv.Degree;
			CurveKnotInsert(crv.Degree, crv.Knots, crv.ControlPoints, u, repeat, out new_crv.Knots, out new_crv.ControlPoints);
			return new_crv;
		}

		/**
		 * Insert knots in the rational curve
		 * @param[in] crv RationalCurve object
		 * @param[in] u Parameter to insert knot at
		 * @param[in] repeat Number of times to insert
		 * @return New RationalCurve object with #repeat knots inserted at u
		 */
		internal static RationalCurve CurveKnotInsert(RationalCurve crv, double u, int repeat = 1)
		{
			var new_crv = new RationalCurve();
			new_crv.Degree = crv.Degree;

			// Convert to homogenous coordinates
			var Cw = new List<Vector>(crv.ControlPoints.Count);

			for (int i = 0; i < crv.ControlPoints.Count; ++i)
			{
				Cw.Add(Util.CartesianToHomogenous(crv.ControlPoints[i], crv.Weights[i]));
			}

			// Perform knot insertion and get new knots and control points
			List<Vector> new_Cw;
			CurveKnotInsert(crv.Degree, crv.Knots, Cw, u, repeat, out new_crv.Knots, out new_Cw);

			// Convert back to cartesian coordinates
			//new_crv.control_points.reserve(new_Cw.size());
			//new_crv.weights.reserve(new_Cw.size());
			
			new_crv.ControlPoints.Clear();
			new_crv.Weights.Clear();
			
			for (int i = 0; i < new_Cw.Count; ++i)
			{
				new_crv.ControlPoints.Add(Util.HomogenousToCartesian(new_Cw[i]));
				new_crv.Weights.Add(new_Cw[i].Last);
			}
			
			return new_crv;
		}

		/**
		 * Insert knots in the surface along u-direction
		 * @param[in] srf Surface object
		 * @param[in] u Knot value to insert
		 * @param[in] repeat Number of times to insert
		 * @return New Surface object after knot insertion
		 */
		internal static Surface SurfaceKnotInsertU(Surface srf, double u, int repeat = 1)
		{
			var new_srf = new Surface();
			new_srf.degree_u = srf.degree_u;
			new_srf.degree_v = srf.degree_v;
			new_srf.knots_v = srf.knots_v;

			SurfaceKnotInsert(new_srf.degree_u, srf.knots_u, srf.control_points, u, repeat, true,
				out new_srf.knots_u, out new_srf.control_points);

			return new_srf;
		}

		/**
		 * Insert knots in the rational surface along u-direction
		 * @param[in] srf RationalSurface object
		 * @param[in] u Knot value to insert
		 * @param[in] repeat Number of times to insert
		 * @return New RationalSurface object after knot insertion
		 */
		internal static RationalSurface SurfaceKnotInsertU(RationalSurface srf, double u, int repeat = 1)
		{
			var new_srf = new RationalSurface();
			new_srf.degree_u = srf.degree_u;
			new_srf.degree_v = srf.degree_v;
			new_srf.knots_v = srf.knots_v;
			
			int width = srf.control_points.GetLength(0);
			int height = srf.control_points.GetLength(1);

			// Original control points in homogenous coordinates
			var Cw = new Vector[width, height];
			
			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					Cw[i, j] = Util.CartesianToHomogenous(srf.control_points[i, j], srf.weights[i, j]);
				}
			}

			// New knots and new homogenous control points after knot insertion
			Vector[,] new_Cw;
			SurfaceKnotInsert(srf.degree_u, srf.knots_u, Cw, u, repeat, true, out new_srf.knots_u, out new_Cw);
			
			width = new_Cw.GetLength(0);
			height = new_Cw.GetLength(1);

			// Convert back to cartesian coordinates
			new_srf.control_points = new Vector[width, height];
			new_srf.weights = new double[width, height];
			
			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					new_srf.control_points[i, j] = Util.HomogenousToCartesian(new_Cw[i, j]);
					new_srf.weights[i, j] = new_Cw[i, j].Last;
				}
			}
			
			return new_srf;
		}

		/**
		 * Insert knots in the surface along v-direction
		 * @param[in] srf Surface object
		 * @param[in] v Knot value to insert
		 * @param[in] repeat Number of times to insert
		 * @return New Surface object after knot insertion
		 */
		internal static Surface SurfaceKnotInsertV(Surface srf, double v, int repeat = 1)
		{
			var new_srf = new Surface();
			new_srf.degree_u = srf.degree_u;
			new_srf.degree_v = srf.degree_v;
			new_srf.knots_u = srf.knots_u;
			
			// New knots and new control points after knot insertion
			SurfaceKnotInsert(srf.degree_u, srf.knots_u, srf.control_points, v, repeat, false,
				out new_srf.knots_v, out new_srf.control_points);
				
			return new_srf;
		}

		/**
		 * Insert knots in the rational surface along v-direction
		 * @param[in] srf RationalSurface object
		 * @param[in] v Knot value to insert
		 * @param[in] repeat Number of times to insert
		 * @return New RationalSurface object after knot insertion
		 */
		internal static RationalSurface SurfaceKnotInsertV(RationalSurface srf, double v, int repeat = 1)
		{
			var new_srf = new RationalSurface();
			new_srf.degree_u = srf.degree_u;
			new_srf.degree_v = srf.degree_v;
			new_srf.knots_u = srf.knots_u;
			
			int width = srf.control_points.GetLength(0);
			int height = srf.control_points.GetLength(1);
			
			// Original control points in homogenous coordinates
			var Cw = new Vector[width, height];
			
			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					Cw[i, j] = Util.CartesianToHomogenous(srf.control_points[i, j], srf.weights[i, j]);
				}
			}

			// New knots and new homogenous control points after knot insertion
			Vector[,] new_Cw;
			SurfaceKnotInsert(srf.degree_v, srf.knots_v, Cw, v, repeat, false, out new_srf.knots_v, out new_Cw);

			width = new_Cw.GetLength(0);
			height = new_Cw.GetLength(1);

			// Convert back to cartesian coordinates
			new_srf.control_points = new Vector[width, height];
			new_srf.weights = new double[width, height];
			
			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					new_srf.control_points[i, j] = Util.HomogenousToCartesian(new_Cw[i, j]);
					new_srf.weights[i, j] = new_Cw[i, j].Last;
				}
			}
			
			return new_srf;
		}

		/**
		 * Split a curve into two
		 * @param[in] crv Curve object
		 * @param[in] u Parameter to split at
		 * @return Tuple with first half and second half of the curve
		 */
		internal static (Curve, Curve) CurveSplit(Curve crv, double u)
		{
			var left = new Curve();
			var right = new Curve();
			left.Degree = crv.Degree;
			right.Degree = crv.Degree;

			CurveSplit(crv.Degree, crv.Knots, crv.ControlPoints, u, 
				out left.Knots, out left.ControlPoints,
				out right.Knots, out right.ControlPoints);

			return (left, right);
		}

		/**
		 * Split a rational curve into two
		 * @param[in] crv RationalCurve object
		 * @param[in] u Parameter to split at
		 * @return Tuple with first half and second half of the curve
		 */
		internal static (RationalCurve, RationalCurve) CurveSplit(RationalCurve crv, double u)
		{
			var left = new RationalCurve();
			var right = new RationalCurve();
			left.Degree = crv.Degree;
			right.Degree = crv.Degree;

			var Cw = new List<Vector>(crv.ControlPoints.Count);
			List<Vector> left_Cw;
			List<Vector> right_Cw;

			for (int i = 0; i < crv.ControlPoints.Count; ++i)
			{
				Cw.Add(Util.CartesianToHomogenous(crv.ControlPoints[i], crv.Weights[i]));
			}

			CurveSplit(crv.Degree, crv.Knots, Cw, u, out left.Knots, out left_Cw, out right.Knots, out right_Cw);

			//left.control_points.reserve(left_Cw.size());
			//left.weights.reserve(left_Cw.size());
			//right.control_points.reserve(right_Cw.size());
			//right.weights.reserve(right_Cw.size());
			
			left.ControlPoints.Clear();
			left.Weights.Clear();
			right.ControlPoints.Clear();
			right.Weights.Clear();

			for (int i = 0; i < left_Cw.Count; ++i)
			{
				left.ControlPoints.Add(Util.HomogenousToCartesian(left_Cw[i]));
				left.Weights.Add(left_Cw[i].Last);
			}
			for (int i = 0; i < right_Cw.Count; ++i)
			{
				right.ControlPoints.Add(Util.HomogenousToCartesian(right_Cw[i]));
				right.Weights.Add(right_Cw[i].Last);
			}

			return (left, right);
		}

		/**
		 * Split a surface into two along u-direction
		 * @param[in] srf Surface object
		 * @param[in] u Parameter along u-direction to split the surface
		 * @return Tuple with first and second half of the surfaces
		 */
		internal static (Surface, Surface) SurfaceSplitU(Surface srf, double u)
		{
			var left = new Surface();
			var right = new Surface();
			left.degree_u = srf.degree_u;
			left.degree_v = srf.degree_v;
			left.knots_v = srf.knots_v;
			right.degree_u = srf.degree_u;
			right.degree_v = srf.degree_v;
			right.knots_v = srf.knots_v;

			SurfaceSplit(srf.degree_u, srf.knots_u, srf.control_points, u, true,
				out left.knots_u, out left.control_points,
				out right.knots_u, out right.control_points);

			return (left, right);
		}

		/**
		 * Split a rational surface into two along u-direction
		 * @param[in] srf RationalSurface object
		 * @param[in] u Parameter along u-direction to split the surface
		 * @return Tuple with first and second half of the surfaces
		 */
		internal static (RationalSurface, RationalSurface) SurfaceSplitU(RationalSurface srf, double u)
		{
			var left = new RationalSurface();
			var right = new RationalSurface();
			left.degree_u = srf.degree_u;
			left.degree_v = srf.degree_v;
			left.knots_v = srf.knots_v;
			right.degree_u = srf.degree_u;
			right.degree_v = srf.degree_v;
			right.knots_v = srf.knots_v;

			// Compute homogenous coordinates of control points and weights
			var Cw = Util.CartesianToHomogenous(srf.control_points, srf.weights);

			// Split surface with homogenous coordinates
			Vector[,] left_Cw, right_Cw;
			SurfaceSplit(srf.degree_u, srf.knots_u, Cw, u, true, 
				out left.knots_u, out left_Cw, out right.knots_u, out right_Cw);

			// Convert back to cartesian coordinates
			Util.HomogenousToCartesian(left_Cw, out left.control_points, out left.weights);
			Util.HomogenousToCartesian(right_Cw, out right.control_points, out right.weights);

			return (left, right);
		}

		/**
		 * Split a surface into two along v-direction
		 * @param[in] srf Surface object
		 * @param[in] v Parameter along v-direction to split the surface
		 * @return Tuple with first and second half of the surfaces
		 */
		static (Surface, Surface) SurfaceSplitV(Surface srf, double v)
		{
			var left = new Surface();
			var right = new Surface();
			left.degree_u = srf.degree_u;
			left.degree_v = srf.degree_v;
			left.knots_u = srf.knots_u;
			right.degree_u = srf.degree_u;
			right.degree_v = srf.degree_v;
			right.knots_u = srf.knots_u;

			SurfaceSplit(srf.degree_v, srf.knots_v, srf.control_points, v, false,
				out left.knots_v, out left.control_points,
				out right.knots_v, out right.control_points);

			return (left, right);
		}

		/**
		 * Split a rational surface into two along v-direction
		 * @param[in] srf RationalSurface object
		 * @param[in] v Parameter along v-direction to split the surface
		 * @return Tuple with first and second half of the surfaces
		 */
		internal static (RationalSurface, RationalSurface) SurfaceSplitV(RationalSurface srf, double v)
		{
			var left = new RationalSurface();
			var right = new RationalSurface();
			left.degree_u = srf.degree_u;
			left.degree_v = srf.degree_v;
			left.knots_u = srf.knots_u;
			right.degree_u = srf.degree_u;
			right.degree_v = srf.degree_v;
			right.knots_u = srf.knots_u;

			// Compute homogenous coordinates of control points and weights
			var Cw = Util.CartesianToHomogenous(srf.control_points, srf.weights);

			// Split surface with homogenous coordinates
			Vector[,] left_Cw, right_Cw;
			SurfaceSplit(srf.degree_v, srf.knots_v, Cw, v, false,
				out left.knots_v, out left_Cw, 
				out right.knots_v, out right_Cw);

			// Convert back to cartesian coordinates
			Util.HomogenousToCartesian(left_Cw, out left.control_points, out left.weights);
			Util.HomogenousToCartesian(right_Cw, out right.control_points, out right.weights);

			return (left, right);
		}

		/**
		 * Insert knots in the curve
		 * @param[in] deg Degree of the curve
		 * @param[in] knots Knot vector of the curve
		 * @param[in] cp Control points of the curve
		 * @param[in] u Parameter to insert knot(s) at
		 * @param[in] r Number of times to insert knot
		 * @param[out] new_knots Updated knot vector
		 * @param[out] new_cp Updated control points
		 */
		private static void CurveKnotInsert(int deg, List<double> knots, List<Vector> cp, double u, int r,
			out List<double> new_knots, out List<Vector> new_cp)
		{
			int k = Basis.FindSpan(deg, knots, u);
			int s = Check.KnotMultiplicity(knots, k);
			int L;
			
			if (s == deg)
			{
				//return;
				throw new Exception("s == deg");
			}
			
			if ((r + s) > deg)
			{
				r = deg - s;
			}

			// Insert new knots between span and (span + 1)
			new_knots = new List<double>(knots.Count + r);
			new_knots.AddRange(knots.Count + r, 0);
			
			for (int i = 0; i < k + 1; ++i)
			{
				new_knots[i] = knots[i];
			}
			for (int i = 1; i < r + 1; ++i)
			{
				new_knots[k + i] = u;
			}
			for (int i = k + 1; i < knots.Count; ++i)
			{
				new_knots[i + r] = knots[i];
			}

			// Copy unaffected control points
			new_cp = new List<Vector>(cp.Count + r);
			new_cp.AddRange(cp.Count + r, null);
			
			for (int i = 0; i < k - deg + 1; ++i)
			{
				new_cp[i] = cp[i];
			}
			for (int i = k - s; i < cp.Count; ++i)
			{
				new_cp[i + r] = cp[i];
			}

			// Copy affected control points
			var tmp = new List<Vector>(deg - s + 1);

			for (int i = 0; i < deg - s + 1; ++i)
			{
				tmp.Add( cp[k - deg + i] );
			}

			// Modify affected control points
			for (int j = 1; j <= r; ++j)
			{
				L = k - deg + j;
				for (int i = 0; i < deg - j - s + 1; ++i)
				{
					var a = (u - knots[L + i]) / (knots[i + k + 1] - knots[L + i]);
					tmp[i] = tmp[i] * (1 - a) + tmp[i + 1] * a;
				}
				new_cp[L] = tmp[0];
				new_cp[k + r - j - s] = tmp[deg - j - s];
			}

			L = k - deg + r;
			for (int i = L + 1; i < k - s; ++i)
			{
				new_cp[i] = tmp[i - L];
			}
		}

		/**
		 * Insert knots in the surface along one direction
		 * @param[in] degree Degree of the surface along which to insert knot
		 * @param[in] knots Knot vector
		 * @param[in] cp 2D array of control points
		 * @param[in] knot Knot value to insert
		 * @param[in] r Number of times to insert
		 * @param[in] along_u Whether inserting along u-direction
		 * @param[out] new_knots Updated knot vector
		 * @param[out] new_cp Updated control points
		 */
		internal static void SurfaceKnotInsert(int degree, List<double> knots,
			Vector[,] cp, double knot, int r, bool along_u,
			out List<double> new_knots, out Vector[,] new_cp)
		{
			int span = Basis.FindSpan(degree, knots, knot);
			int s = Check.KnotMultiplicity(knots, span);
			int L;
			
			if (s == degree)
			{
				//return;
				throw new Exception("s == degree");
			}
			
			if ((r + s) > degree)
			{
				r = degree - s;
			}

			// Create a new knot vector
			new_knots = new List<double>(knots.Count + r);
			new_knots.AddRange(knots.Count + r, 0);
			
			for (int i = 0; i <= span; ++i)
			{
				new_knots[i] = knots[i];
			}
			for (int i = 1; i <= r; ++i)
			{
				new_knots[span + i] = knot;
			}
			for (int i = span + 1; i < knots.Count; ++i)
			{
				new_knots[i + r] = knots[i];
			}

			// Compute alpha
			var alpha = new double[degree - s, r + 1];
			
			for (int j = 1; j <= r; ++j)
			{
				L = span - degree + j;
				for (int i = 0; i <= degree - j - s; ++i)
				{
					alpha[i, j] = (knot - knots[L + i]) / (knots[i + span + 1] - knots[L + i]);
				}
			}

			// Create a temporary container for affected control points per row/column
			var tmp = new List<Vector>(degree + 1);

			if (along_u)
			{
				int width = cp.GetLength(0);
				int height = cp.GetLength(1);
				
				// Create new control points with additional rows
				new_cp = new Vector[width + r, height];

				// Update control points
				// Each row is a u-isocurve, each col is a v-isocurve
				for (int col = 0; col < height; ++col)
				{
					// Copy unaffected control points
					for (int i = 0; i <= span - degree; ++i)
					{
						new_cp[i, col] = cp[i, col];
					}
					for (int i = span - s; i < width; ++i)
					{
						new_cp[i + r, col] = cp[i, col];
					}

					// Copy affected control points to temp array
					for (int i = 0; i < degree - s + 1; ++i)
					{
						tmp[i] = cp[span - degree + i, col];
					}

					// Insert knot
					for (int j = 1; j <= r; ++j)
					{
						L = span - degree + j;
						for (int i = 0; i <= degree - j - s; ++i)
						{
							var a = alpha[i, j];
							tmp[i] = tmp[i] * (1 - a) + tmp[i + 1] * a;
						}
						new_cp[L, col] = tmp[0];
						new_cp[span + r - j - s, col] = tmp[degree - j - s];
					}

					L = span - degree + r;
					for (int i = L + 1; i < span - s; ++i)
					{
						new_cp[i, col] = tmp[i - L];
					}
				}
			}
			else
			{
				int width = cp.GetLength(0);
				int height = cp.GetLength(1);
				
				// Create new control points with additional columns
				new_cp = new Vector[width, height + r];

				// Update control points
				// Each row is a u-isocurve, each col is a v-isocurve
				for (int row = 0; row < width; ++row)
				{
					// Copy unaffected control points
					for (int i = 0; i <= span - degree; ++i)
					{
						new_cp[row, i] = cp[row, i];
					}
					for (int i = span - s; i < height; ++i)
					{
						new_cp[row, i + r] = cp[row, i];
					}

					// Copy affected control points to temp array
					for (int i = 0; i < degree - s + 1; ++i)
					{
						tmp[i] = cp[row, span - degree + i];
					}

					// Insert knot
					for (int j = 1; j <= r; ++j)
					{
						L = span - degree + j;
						for (int i = 0; i <= degree - j - s; ++i)
						{
							var a = alpha[i, j];
							tmp[i] = tmp[i] * (1 - a) + tmp[i + 1] * a;
						}
						new_cp[row, L] = tmp[0];
						new_cp[row, span + r - j - s] = tmp[degree - j - s];
					}

					L = span - degree + r;
					for (int i = L + 1; i < span - s; ++i)
					{
						new_cp[row, i] = tmp[i - L];
					}
				}
			}
		}

		/**
		 * Split the curve into two
		 * @param[in] degree Degree of curve
		 * @param[in] knots Knot vector
		 * @param[in] control_points Array of control points
		 * @param[in] u Parameter to split curve
		 * @param[out] left_knots Knots of the left part of the curve
		 * @param[out] left_control_points Control points of the left part of the curve
		 * @param[out] right_knots Knots of the right part of the curve
		 * @param[out] right_control_points Control points of the right part of the curve
		 */
		static void CurveSplit(int degree, List<double> knots, List<Vector> control_points, double u,
			out List<double> left_knots, out List<Vector> left_control_points,
			out List<double> right_knots, out List<Vector> right_control_points)
		{
			List<double> tmp_knots;
			List<Vector> tmp_cp;

			int span = Basis.FindSpan(degree, knots, u);
			int r = degree - Check.KnotMultiplicity(knots, span);

			CurveKnotInsert(degree, knots, control_points, u, r, out tmp_knots, out tmp_cp);

			left_knots = new List<double>();
			right_knots = new List<double>();
			left_control_points = new List<Vector>();
			right_control_points = new List<Vector>();

			int span_l = Basis.FindSpan(degree, tmp_knots, u) + 1;
			for (int i = 0; i < span_l; ++i)
			{
				left_knots.Add(tmp_knots[i]);
			}
			left_knots.Add(u);

			for (int i = 0; i < degree + 1; ++i)
			{
				right_knots.Add(u);
			}
			for (int i = span_l; i < tmp_knots.Count; ++i)
			{
				right_knots.Add(tmp_knots[i]);
			}

			int ks = span - degree + 1;
			for (int i = 0; i < ks + r; ++i)
			{
				left_control_points.Add(tmp_cp[i]);
			}
			for (int i = ks + r - 1; i < tmp_cp.Count; ++i)
			{
				right_control_points.Add(tmp_cp[i]);
			}
		}

		/**
		 * Split the surface into two along given parameter direction
		 * @param[in] degree Degree of surface along given direction
		 * @param[in] knots Knot vector of surface along given direction
		 * @param[in] control_points Array of control points
		 * @param[in] param Parameter to split curve
		 * @param[in] along_u Whether the direction to split along is the u-direction
		 * @param[out] left_knots Knots of the left part of the curve
		 * @param[out] left_control_points Control points of the left part of the curve
		 * @param[out] right_knots Knots of the right part of the curve
		 * @param[out] right_control_points Control points of the right part of the curve
		 */
		internal static void SurfaceSplit(int degree, List<double> knots, Vector[,] control_points, double param, bool along_u,
			out List<double> left_knots, out Vector[,] left_control_points,
			out List<double> right_knots, out Vector[,] right_control_points)
		{
			List<double> tmp_knots;
			Vector[,] tmp_cp;

			int span = Basis.FindSpan(degree, knots, param);
			int r = degree - Check.KnotMultiplicity(knots, span);
			
			SurfaceKnotInsert(degree, knots, control_points, param, r, along_u, out tmp_knots, out tmp_cp);

			left_knots = new List<double>();
			right_knots = new List<double>();

			int span_l = Basis.FindSpan(degree, tmp_knots, param) + 1;
			for (int i = 0; i < span_l; ++i)
			{
				left_knots.Add(tmp_knots[i]);
			}
			left_knots.Add(param);

			for (int i = 0; i < degree + 1; ++i)
			{
				right_knots.Add(param);
			}
			for (int i = span_l; i < tmp_knots.Count; ++i)
			{
				right_knots.Add(tmp_knots[i]);
			}

			int width = tmp_cp.GetLength(0);
			int height = tmp_cp.GetLength(1);
			int ks = span - degree + 1;
			
			if (along_u)
			{				
				int ii = 0;
				left_control_points = new Vector[ks + r, height];
				
				for (int i = 0; i < ks + r; ++i)
				{
					for (int j = 0; j < height; ++j)
					{
						left_control_points[ii % width, ii / width] = tmp_cp[i, j];
						ii++;
					}
				}
				
				ii = 0;
				right_control_points = new Vector[width - ks - r + 1, height];
				
				for (int i = ks + r - 1; i < width; ++i)
				{
					for (int j = 0; j < height; ++j)
					{
						right_control_points[ii % width, ii / width] = tmp_cp[i, j];
						ii++;
					}
				}
			}
			else
			{
				int ii = 0;
				left_control_points = new Vector[width, ks + r];
				
				for (int i = 0; i < width; ++i)
				{
					for (int j = 0; j < ks + r; ++j)
					{
						left_control_points[ii % width, ii / width] = tmp_cp[i, j];
						ii++;
					}
				}
				
				ii = 0;
				right_control_points = new Vector[width, height - ks - r + 1];
				
				for (int i = 0; i < width; ++i)
				{
					for (int j = ks + r - 1; j < height; ++j)
					{
						right_control_points[ii % width, ii / width] = tmp_cp[i, j];
						ii++;
					}
				}
			}
		}

	}

}

