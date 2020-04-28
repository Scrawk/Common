using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	internal static class Check
	{
		/**
		 * Returns the mulitplicity of the knot at index
		 * @tparam Type of knot values
		 * @param[in] knots Knot vector
		 * @param[in] index Index of knot of interest
		 * @return Multiplicity (>= 1)
		 */
		internal static int KnotMultiplicity(List<double> knots, int index)
		{
			double u = knots[index];

			int mult = 0;
			for (int i = index; i < knots.Count; ++i)
			{
				int idx = index + 1;
				if (Math.Abs(u - knots[idx]) < DMath.EPS)
				{
					++mult;
				}
			}
			return mult;
		}

		/**
		 * Returns whether the curve is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param[in] crv Curve object
		 * @return Whether valid
		 */
		internal static bool CurveIsValid(Curve crv)
		{
			return CurveIsValid(crv.Degree, crv.Knots, crv.ControlPoints);
		}

		/**
		 * Returns whether the curve is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param[in] crv RationalCurve object
		 * @return Whether valid
		 */
		internal static bool CurveIsValid(RationalCurve crv)
		{
			return CurveIsValid(crv.Degree, crv.Knots, crv.ControlPoints, crv.Weights);
		}

		/**
		 * Returns whether the surface is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param srf Surface object
		 * @return Whether valid
		 */
		internal static bool SurfaceIsValid(Surface srf)
		{
			return SurfaceIsValid(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v,
				srf.control_points);
		}

		/**
		 * Returns whether the rational surface is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param[in] srf RationalSurface object
		 * @return Whether valid
		 */
		internal static bool SurfaceIsValid(RationalSurface srf)
		{
			return SurfaceIsValid(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v,
				srf.control_points, srf.weights);
		}

		/**
		 * Checks whether the curve is closed
		 * @param[in] crv Curve object
		 * @return  Whether closed
		 */
		internal static bool CurveIsClosed(Curve crv)
		{
			return IsArray1Closed(crv.Degree, crv.ControlPoints) &&
				IsKnotVectorClosed(crv.Degree, crv.Knots);
		}

		/**
		 * Checks whether the rational curve is closed
		 * @param[in] crv RationalCurve object
		 * @return  Whether closed
		 */
		internal static bool CurveIsClosed(RationalCurve crv)
		{
			return IsArray1Closed(crv.Degree, crv.ControlPoints) &&
				IsArray1Closed(crv.Degree, crv.Weights) &&
				IsKnotVectorClosed(crv.Degree, crv.Knots);
		}

		/**
		 * Checks whether the surface is closed along u-direction
		 * @param[in] srf Surface object
		 * @return  Whether closed along u-direction
		 */
		internal static bool SurfaceIsClosedU(Surface srf)
		{
			return IsArray2ClosedU(srf.degree_u, srf.control_points) &&
				IsKnotVectorClosed(srf.degree_u, srf.knots_u);
		}

		/**
		 * Checks whether the surface is closed along v-direction
		 * @param[in] srf Surface object
		 * @return  Whether closed along v-direction
		 */
		internal static bool SurfaceIsClosedV(Surface srf)
		{
			return IsArray2ClosedV(srf.degree_v, srf.control_points) &&
				IsKnotVectorClosed(srf.degree_v, srf.knots_v);
		}

		/**
		 * Checks whether the rational surface is closed along u-direction
		 * @param[in] srf RationalSurface object
		 * @return  Whether closed along u-direction
		 */
		internal static bool SurfaceIsClosedU(RationalSurface srf)
		{
			return IsArray2ClosedU(srf.degree_u, srf.control_points) &&
				IsKnotVectorClosed(srf.degree_u, srf.knots_u) &&
				IsArray2ClosedU(srf.degree_u, srf.weights);
		}

		/**
		 * Checks whether the rational surface is closed along v-direction
		 * @param[in] srf RationalSurface object
		 * @return  Whether closed along v-direction
		 */
		internal static bool SurfaceIsClosedV(RationalSurface srf)
		{
			return IsArray2ClosedV(srf.degree_v, srf.control_points) &&
				IsKnotVectorClosed(srf.degree_v, srf.knots_v) &&
				IsArray2ClosedV(srf.degree_v, srf.weights);
		}

		/**
		 * Checks if the relation between degree, number of knots, and
		 * number of control points is valid
		 * @param[in] degree Degree
		 * @param[in] num_knots Number of knot values
		 * @param[in] num_ctrl_pts Number of control points
		 * @return Whether the relationship is valid
		 */
		private static bool IsValidRelation(int degree, int num_knots, int num_ctrl_pts)
		{
			return (num_knots - degree - 1) == num_ctrl_pts;
		}

		/**
		 * isKnotVectoMonotonic returns whether the knots are in ascending order
		 * @tparam Type of knot values
		 * @param[in] knots Knot vector
		 * @return Whether monotonic
		 */
		private static bool IsKnotVectorMonotonic(List<double> knots)
		{
			if (knots.Count <= 1) return true;

			for (int i = 1; i < knots.Count; i++)
			{
				if (knots[i - 1] > knots[i]) return false;
			}

			return true;
		}

		/**
		 * Returns whether the curve is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param[in] degree Degree of curve
		 * @param[in] knots Knot vector of curve
		 * @param[in] control_points Control points of curve
		 * @return Whether valid
		 */
		private static bool CurveIsValid(int degree, List<double> knots, List<Vector> control_points)
		{
			if (degree < 1 || degree > 9)
			{
				return false;
			}
			if (!IsValidRelation(degree, knots.Count, control_points.Count))
			{
				return false;
			}
			if (!IsKnotVectorMonotonic(knots))
			{
				return false;
			}
			return true;
		}

		/**
		 * Returns whether the curve is valid
		 * @tparam T Type of control point coordinates, knot values and weights
		 * @param[in] degree Degree of curve
		 * @param[in] knots Knot vector of curve
		 * @param[in] control_points Control points of curve
		 * @return Whether valid
		 */
		private static bool CurveIsValid(int degree, List<double> knots, List<Vector> control_points, IList<double> weights)
		{
			if (!IsValidRelation(degree, knots.Count, control_points.Count))
			{
				return false;
			}
			if (weights.Count != control_points.Count)
			{
				return false;
			}
			return true;
		}

		/**
		 * Returns whether the surface is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param[in] degree_u Degree of surface along u-direction
		 * @param[in] degree_v Degree of surface along v-direction
		 * @param[in] knots_u Knot vector of surface along u-direction
		 * @param[in] knots_v Knot vector of surface along v-direction
		 * @param[in] control_points Control points grid of surface
		 * @return Whether valid
		 */
		private static bool SurfaceIsValid(int degree_u, int degree_v, List<double> knots_u, List<double> knots_v, Vector[,] control_points)
		{
			if (degree_u < 1 || degree_u > 9 || degree_v < 1 || degree_v > 9)
			{
				return false;
			}
			if (!IsValidRelation(degree_u, knots_u.Count, control_points.GetLength(0)) ||
				!IsValidRelation(degree_v, knots_v.Count, control_points.GetLength(1)))
			{
				return false;
			}
			if (!IsKnotVectorMonotonic(knots_u) || !IsKnotVectorMonotonic(knots_v))
			{
				return false;
			}
			return true;
		}

		/**
		 * Returns whether the rational surface is valid
		 * @tparam T Type of control point coordinates, knot values
		 * @param[in] degree_u Degree of surface along u-direction
		 * @param[in] degree_v Degree of surface along v-direction
		 * @param[in] knots_u Knot vector of surface along u-direction
		 * @param[in] knots_v Knot vector of surface along v-direction
		 * @param[in] control_points Control points grid of surface
		 * @param[in] weights Weights corresponding to control point grid of surface
		 * @return Whether valid
		 */
		private static bool SurfaceIsValid(int degree_u, int degree_v, List<double> knots_u,
			List<double> knots_v, Vector[,] control_points, double[,] weights)
		{
			if (!Check.SurfaceIsValid(degree_u, degree_v, knots_u, knots_v, control_points))
			{
				return false;
			}
			if (control_points.GetLength(0) != weights.GetLength(0) || control_points.GetLength(1) != weights.GetLength(1))
			{
				return false;
			}
			return true;
		}


		/**
		 * Returns whether the given knot vector is closed by checking the
		 * periodicity of knot vectors near the start and end
		 * @param[in] degree Degree of curve/surface
		 * @param[in] knots Knot vector of curve/surface
		 * @return Whether knot vector is closed
		 */
		private static bool IsKnotVectorClosed(int degree, List<double> knots)
		{
			for (int i = 0; i < degree - 1; ++i)
			{
				int j = knots.Count - degree + i;

				if (Math.Abs((knots[i + 1] - knots[i]) - (knots[j + 1] - knots[j])) > DMath.EPS)
				{
					return false;
				}
			}
			return true;
		}

		/**
		 * Returns whether the given knot vector is closed by checking the
		 * periodicity of knot vectors near the start and end
		 * @param[in] degree Degree of curve/surface
		 * @param[in] vec Array of any weights
		 * @return Whether knot vector is closed
		 */
		private static bool IsArray1Closed(int degree, List<double> vec)
		{
			for (int i = 0; i < degree; ++i)
			{
				int j = vec.Count - degree + i;
				if ((vec[i] - vec[j]) > DMath.EPS)
				{
					return false;
				}
			}
			return true;
		}

		/**
		* Returns whether the given knot vector is closed by checking the
		* periodicity of knot vectors near the start and end
		* @param[in] degree Degree of curve/surface
		* @param[in] vec Array of any control points
		* @return Whether knot vector is closed
		*/
		private static bool IsArray1Closed(int degree, List<Vector> vec)
		{
			for (int i = 0; i < degree; ++i)
			{
				int j = vec.Count - degree + i;
				if ((vec[i] - vec[j]).Magnitude > DMath.EPS)
				{
					return false;
				}
			}
			return true;
		}

		/**
		 * Returns whether the 2D array is closed along the u-direction
		 * i.e., along rows.
		 * @param[in] degree_u Degree along u-direction
		 * @param[in] arr 2D array of control points / weights
		 * @return Whether closed along u-direction
		 */
		private static bool IsArray2ClosedU(int degree_u, double[,] arr)
		{
			for (int i = 0; i < degree_u; ++i)
			{
				for (int j = 0; j < arr.GetLength(1); ++j)
				{
					int k = arr.GetLength(1) - degree_u + i;
					if ((arr[i, j] - arr[k, j]) > DMath.EPS)
					{
						return false;
					}
				}
			}
			return true;
		}

		/**
		 * Returns whether the 2D array is closed along the v-direction
		 * i.e., along columns.
		 * @param[in] degree_v Degree along v-direction
		 * @param[in] arr 2D array of control points / weights
		 * @return Whether closed along v-direction
		 */
		private static bool IsArray2ClosedV(int degree_v, double[,] arr)
		{
			for (int i = 0; i < arr.GetLength(0); ++i)
			{
				for (int j = 0; j < degree_v; j++)
				{
					int k = arr.GetLength(0) - degree_v + i;
					if ((arr[i, j] - arr[i, k]) > DMath.EPS)
					{
						return false;
					}
				}
			}
			return true;
		}

		/**
		* Returns whether the 2D array is closed along the u-direction
		* i.e., along rows.
		* @param[in] degree_u Degree along u-direction
		* @param[in] arr 2D array of control points / weights
		* @return Whether closed along u-direction
		*/
		private static bool IsArray2ClosedU(int degree_u, Vector[,] arr)
		{
			for (int i = 0; i < degree_u; ++i)
			{
				for (int j = 0; j < arr.GetLength(1); ++j)
				{
					int k = arr.GetLength(1) - degree_u + i;
					if ((arr[i, j] - arr[k, j]).Magnitude > DMath.EPS)
					{
						return false;
					}
				}
			}
			return true;
		}

		/**
		 * Returns whether the 2D array is closed along the v-direction
		 * i.e., along columns.
		 * @param[in] degree_v Degree along v-direction
		 * @param[in] arr 2D array of control points / weights
		 * @return Whether closed along v-direction
		 */
		private static bool IsArray2ClosedV(int degree_v, Vector[,] arr)
		{
			for (int i = 0; i < arr.GetLength(0); ++i)
			{
				for (int j = 0; j < degree_v; j++)
				{
					int k = arr.GetLength(0) - degree_v + i;
					if ((arr[i, j] - arr[i, k]).Magnitude > DMath.EPS)
					{
						return false;
					}
				}
			}
			return true;
		}

	}

}

