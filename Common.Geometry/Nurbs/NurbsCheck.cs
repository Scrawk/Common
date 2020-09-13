using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	internal static class NurbsCheck
	{

		/// <summary>
		/// Returns the mulitplicity of the knot at index
		/// </summary>
		/// <param name="knots">Knot vector</param>
		/// <param name="index">index Index of knot of interest</param>
		/// <returns>Multiplicity (>= 1)</returns>
		internal static int KnotMultiplicity(IList<double> knots, int index)
		{
			double u = knots[index];

			int mult = 0;
			for (int i = index; i < knots.Count; ++i)
			{
				int idx = index + 1;
				if (MathUtil.IsZero(u - knots[idx]))
				{
					++mult;
				}
			}
			return mult;
		}

		/// <summary>
		/// Returns whether the curve is valid
		/// </summary>
		/// <param name="crv">Curve object</param>
		/// <returns>Whether valid</returns>
		internal static bool CurveIsValid(NurbsCurve3d crv)
		{
			if (crv.IsRational)
				return RationalCurveIsValid(crv as RationalNurbsCurve3d);
			else
				return CurveIsValid(crv.Degree, crv.Knots, crv.ControlPoints);
		}

		/// <summary>
		/// Returns whether the curve is valid
		/// </summary>
		/// <param name="crv">Rational Curve object</param>
		/// <returns>Whether valid</returns>
		internal static bool RationalCurveIsValid(RationalNurbsCurve3d crv)
		{
			return CurveIsValid(crv.Degree, crv.Knots, crv.ControlPoints, crv.Weights);
		}

		/// <summary>
		/// Returns whether the surface is valid
		/// </summary>
		/// <param name="srf">Surface object</param>
		/// <returns>Whether valid</returns>
		internal static bool SurfaceIsValid(NurbsSurface3d srf)
		{
			if (srf.IsRational)
				return RationalSurfaceIsValid(srf as RationalNurbsSurface3d);
			else
				return SurfaceIsValid(srf.DegreeU, srf.DegreeV, srf.KnotsU, srf.KnotsV, srf.ControlPoints);
		}

		/// <summary>
		/// Returns whether the surface is valid
		/// </summary>
		/// <param name="srf">Rational Surface object</param>
		/// <returns>Whether valid</returns>
		internal static bool RationalSurfaceIsValid(RationalNurbsSurface3d srf)
		{
			return SurfaceIsValid(srf.DegreeU, srf.DegreeV, srf.KnotsU, srf.KnotsV, srf.ControlPoints, srf.Weights);
		}

		/// <summary>
		/// Checks whether the curve is closed
		/// </summary>
		/// <param name="crv">Curve object</param>
		/// <returns>Whether closed</returns>
		internal static bool CurveIsClosed(NurbsCurve3d crv)
		{
			if (crv.IsRational)
				return RationalCurveIsClosed(crv as RationalNurbsCurve3d);
			else
				return IsArray1Closed(crv.Degree, crv.ControlPoints) && 
					IsKnotVectorClosed(crv.Degree, crv.Knots);
		}

		/// <summary>
		/// Checks whether the curve is closed
		/// </summary>
		/// <param name="crv">Curve object</param>
		/// <returns>Whether closed</returns>
		internal static bool RationalCurveIsClosed(RationalNurbsCurve3d crv)
		{
			return IsArray1Closed(crv.Degree, crv.ControlPoints) &&
				IsArray1Closed(crv.Degree, crv.Weights) &&
				IsKnotVectorClosed(crv.Degree, crv.Knots);
		}

		/// <summary>
		/// Checks whether the surface is closed along u-direction
		/// </summary>
		/// <param name="srf">Surface object</param>
		/// <returns>Whether closed along u-direction</returns>
		internal static bool SurfaceIsClosedU(NurbsSurface3d srf)
		{
			if (srf.IsRational)
				return RationalSurfaceIsClosedU(srf as RationalNurbsSurface3d);
			else
				return IsArray2ClosedU(srf.DegreeU, srf.ControlPoints) && 
					IsKnotVectorClosed(srf.DegreeU, srf.KnotsU);
		}

		/// <summary>
		/// Checks whether the surface is closed along v-direction
		/// </summary>
		/// <param name="srf">Surface object</param>
		/// <returns>Whether closed along u-direction</returns>
		internal static bool SurfaceIsClosedV(NurbsSurface3d srf)
		{
			if (srf.IsRational)
				return RationalSurfaceIsClosedV(srf as RationalNurbsSurface3d);
			else
				return IsArray2ClosedV(srf.DegreeV, srf.ControlPoints) &&
					IsKnotVectorClosed(srf.DegreeV, srf.KnotsV);
		}

		/// <summary>
		/// Checks whether the rational surface is closed along u-direction
		/// </summary>
		/// <param name="srf">RationalSurface object</param>
		/// <returns>Whether closed along u-direction</returns>
		internal static bool RationalSurfaceIsClosedU(RationalNurbsSurface3d srf)
		{
			return IsArray2ClosedU(srf.DegreeU, srf.ControlPoints) &&
				IsKnotVectorClosed(srf.DegreeU, srf.KnotsU) &&
				IsArray2ClosedU(srf.DegreeU, srf.Weights);
		}

		/// <summary>
		/// Checks whether the rational surface is closed along v-direction
		/// </summary>
		/// <param name="srf">RationalSurface object</param>
		/// <returns>Whether closed along v-direction</returns>
		internal static bool RationalSurfaceIsClosedV(RationalNurbsSurface3d srf)
		{
			return IsArray2ClosedV(srf.DegreeV, srf.ControlPoints) &&
				IsKnotVectorClosed(srf.DegreeV, srf.KnotsV) &&
				IsArray2ClosedV(srf.DegreeV, srf.Weights);
		}

		/// <summary>
		/// Checks if the relation between degree, number of knots, and
		/// number of control points is valid
		/// </summary>
		/// <param name="degree">The degree</param>
		/// <param name="num_knots">Number of knot values</param>
		/// <param name="num_ctrl_pts">Number of control points</param>
		/// <returns>Whether the relationship is valid</returns>
		private static bool IsValidRelation(int degree, int num_knots, int num_ctrl_pts)
		{
			return (num_knots - degree - 1) == num_ctrl_pts;
		}

		/// <summary>
		/// Whether the knots are in ascending order
		/// </summary>
		/// <param name="knots">Knot vector</param>
		/// <returns>Whether monotonic</returns>
		private static bool IsKnotVectorMonotonic(IList<double> knots)
		{
			if (knots.Count <= 1) return true;

			for (int i = 1; i < knots.Count; i++)
			{
				if (knots[i - 1] > knots[i]) return false;
			}

			return true;
		}

		/// <summary>
		/// Returns whether the curve is valid
		/// </summary>
		/// <param name="degree">Degree of curve</param>
		/// <param name="knots">Knot vector of curve</param>
		/// <param name="control_point">Control points of curve</param>
		/// <returns>Whether valid</returns>
		private static bool CurveIsValid(int degree, IList<double> knots, IList<Vector3d> control_points)
		{
			if (degree < 1 || degree > 9)
				return false;

			if (!IsValidRelation(degree, knots.Count, control_points.Count))
				return false;

			if (!IsKnotVectorMonotonic(knots))
				return false;

			return true;
		}


		/// <summary>
		/// Returns whether the curve is valid
		/// </summary>
		/// <param name="degree">Degree of curve</param>
		/// <param name="knots">Knot vector of curve</param>
		/// <param name="control_point">Control points of curve</param>
		/// <param name="weights">Weights of curve</param>
		/// <returns>Whether valid</returns>
		private static bool CurveIsValid(int degree, IList<double> knots, IList<Vector3d> control_points, IList<double> weights)
		{
			if (!IsValidRelation(degree, knots.Count, control_points.Count))
				return false;

			if (weights.Count != control_points.Count)
				return false;

			return true;
		}

		/// <summary>
		/// Returns whether the surface is valid
		/// </summary>
		/// <param name="degree_u">Degree of surface along u-direction</param>
		/// <param name="degree_v">Degree of surface along v-direction</param>
		/// <param name="knots_u">Knot vector of surface along u-direction</param>
		/// <param name="knots_v">Knot vector of surface along v-direction</param>
		/// <param name="control_points">Control points grid of surface</param>
		/// <returns>Whether valid</returns>
		private static bool SurfaceIsValid(int degree_u, int degree_v, IList<double> knots_u, IList<double> knots_v, Vector3d[,] control_points)
		{
			if (degree_u < 1 || degree_u > 9 || degree_v < 1 || degree_v > 9)
				return false;

			if (!IsValidRelation(degree_u, knots_u.Count, control_points.GetLength(0)) ||
				!IsValidRelation(degree_v, knots_v.Count, control_points.GetLength(1)))
				return false;

			if (!IsKnotVectorMonotonic(knots_u) || !IsKnotVectorMonotonic(knots_v))
				return false;

			return true;
		}

		/// <summary>
		/// Returns whether the surface is valid
		/// </summary>
		/// <param name="degree_u">Degree of surface along u-direction</param>
		/// <param name="degree_v">Degree of surface along v-direction</param>
		/// <param name="knots_u">Knot vector of surface along u-direction</param>
		/// <param name="knots_v">Knot vector of surface along v-direction</param>
		/// <param name="control_points">Control points grid of surface</param>
		/// <param name="weights">Weights corresponding to control point grid of surface</param>
		/// <returns>Whether valid</returns>
		private static bool SurfaceIsValid(int degree_u, int degree_v, IList<double> knots_u, IList<double> knots_v, Vector3d[,] control_points, double[,] weights)
		{
			if (!SurfaceIsValid(degree_u, degree_v, knots_u, knots_v, control_points))
				return false;

			if (control_points.GetLength(0) != weights.GetLength(0) || control_points.GetLength(1) != weights.GetLength(1))
				return false;

			return true;
		}


		/// <summary>
		/// Returns whether the given knot vector is closed by checking the
		/// periodicity of knot vectors near the start and end
		/// </summary>
		/// <param name="degree">Degree of curve/surface</param>
		/// <param name="knots">Knot vector of curve/surface</param>
		/// <returns>Whether knot vector is closed</returns>
		private static bool IsKnotVectorClosed(int degree, IList<double> knots)
		{
			for (int i = 0; i < degree - 1; ++i)
			{
				int j = knots.Count - degree + i;
				if (!MathUtil.IsZero((knots[i + 1] - knots[i]) - (knots[j + 1] - knots[j])))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Returns whether the given knot vector is closed by checking the
		/// periodicity of knot vectors near the start and end
		/// </summary>
		/// <param name="degree">Degree of curve/surface</param>
		/// <param name="vec">Array of any weights</param>
		/// <returns>Whether knot vector is closed</returns>
		private static bool IsArray1Closed(int degree, IList<double> vec)
		{
			for (int i = 0; i < degree; ++i)
			{
				int j = vec.Count - degree + i;
				if (!MathUtil.IsZero(vec[i] - vec[j]))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Returns whether the given knot vector is closed by checking the
		/// periodicity of knot vectors near the start and end
		/// </summary>
		/// <param name="degree">Degree of curve/surface</param>
		/// <param name="vec">Array of any control points</param>
		/// <returns>Whether knot vector is closed</returns>
		private static bool IsArray1Closed(int degree, IList<Vector3d> vec)
		{
			for (int i = 0; i < degree; ++i)
			{
				int j = vec.Count - degree + i;
				if (!MathUtil.IsZero((vec[i] - vec[j]).Magnitude))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Returns whether the 2D array is closed along the u-direction
		/// i.e., along rows.
		/// </summary>
		/// <param name="degree_u">Degree along u-direction</param>
		/// <param name="arr">2D array of control points / weights</param>
		/// <returns>Whether closed along u-direction</returns>
		private static bool IsArray2ClosedU(int degree_u, double[,] arr)
		{
			for (int i = 0; i < degree_u; ++i)
			{
				for (int j = 0; j < arr.GetLength(1); ++j)
				{
					int k = arr.GetLength(1) - degree_u + i;
					if (!MathUtil.IsZero(arr[i, j] - arr[k, j]))
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns whether the 2D array is closed along the v-direction
		/// i.e., along columns.
		/// </summary>
		/// <param name="degree_v">Degree along v-direction</param>
		/// <param name="arr">2D array of control points / weights</param>
		/// <returns>Whether closed along v-direction</returns>
		private static bool IsArray2ClosedV(int degree_v, double[,] arr)
		{
			for (int i = 0; i < arr.GetLength(0); ++i)
			{
				for (int j = 0; j < degree_v; j++)
				{
					int k = arr.GetLength(0) - degree_v + i;
					if (!MathUtil.IsZero(arr[i, j] - arr[i, k]))
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns whether the 2D array is closed along the u-direction
		/// i.e., along rows.
		/// </summary>
		/// <param name="degree_u">Degree along u-direction</param>
		/// <param name="arr">2D array of control points / weights</param>
		/// <returns>Whether closed along u-direction</returns>
		private static bool IsArray2ClosedU(int degree_u, Vector3d[,] arr)
		{
			for (int i = 0; i < degree_u; ++i)
			{
				for (int j = 0; j < arr.GetLength(1); ++j)
				{
					int k = arr.GetLength(1) - degree_u + i;
					if (!MathUtil.IsZero((arr[i, j] - arr[k, j]).Magnitude))
						return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Returns whether the 2D array is closed along the v-direction
		/// i.e., along columns.
		/// </summary>
		/// <param name="degree_v">Degree along v-direction</param>
		/// <param name="arr">2D array of control points / weights</param>
		/// <returns>Whether closed along v-direction</returns>
		private static bool IsArray2ClosedV(int degree_v, Vector3d[,] arr)
		{
			for (int i = 0; i < arr.GetLength(0); ++i)
			{
				for (int j = 0; j < degree_v; j++)
				{
					int k = arr.GetLength(0) - degree_v + i;
					if (!MathUtil.IsZero((arr[i, j] - arr[i, k]).Magnitude))
						return false;
				}
			}
			return true;
		}

	}

}

