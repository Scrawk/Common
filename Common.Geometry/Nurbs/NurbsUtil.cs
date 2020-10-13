using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
	internal static class NurbsUtil
	{

		/// <summary>
		/// Convert an nd point in homogenous coordinates to an (n-1)d point in cartesian
		/// coordinates by perspective division
		/// </summary>
		/// <param name="pt">Point in homogenous coordinates</param>
		/// <returns>Point in cartesian coordinates</returns>
		internal static Vector2d HomogenousToCartesian(Vector3d pt)
		{
			return pt.xy / pt.z;
		}

		/// <summary>
		/// Convert an nd point in homogenous coordinates to an (n-1)d point in cartesian
		/// coordinates by perspective division
		/// </summary>
		/// <param name="pt">Point in homogenous coordinates</param>
		/// <returns>Point in cartesian coordinates</returns>
		internal static Vector3d HomogenousToCartesian(Vector4d pt)
		{
			return pt.xyz / pt.w;
		}

		/// <summary>
		/// Convert a list of nd points in homogenous coordinates to a list of (n-1)d points in cartesian
		/// coordinates by perspective division
		/// </summary>
		/// <param name="ptsws">Points in homogenous coordinates</param>
		/// <param name="pts">Points in cartesian coordinates</param>
		/// <param name="ws">Homogenous weights</param>
		internal static void HomogenousToCartesian(IList<Vector3d> ptsws, out List<Vector2d> pts, out List<double> ws)
		{
			pts = new List<Vector2d>(ptsws.Count);
			ws = new List<double>(ptsws.Count);

			for (int i = 0; i < ptsws.Count; ++i)
			{
				Vector3d ptw_i = ptsws[i];

				pts.Add(ptw_i.xy / ptw_i.z);
				ws.Add(ptw_i.z);
			}
		}

		/// <summary>
		/// Convert a list of nd points in homogenous coordinates to a list of (n-1)d points in cartesian
		/// coordinates by perspective division
		/// </summary>
		/// <param name="ptsws">Points in homogenous coordinates</param>
		/// <param name="pts">Points in cartesian coordinates</param>
		/// <param name="ws">Homogenous weights</param>
		internal static void HomogenousToCartesian(IList<Vector4d> ptsws, out List<Vector3d> pts, out List<double> ws)
		{
			pts = new List<Vector3d>(ptsws.Count);
			ws = new List<double>(ptsws.Count);

			for (int i = 0; i < ptsws.Count; ++i)
			{
				Vector4d ptw_i = ptsws[i];

				pts.Add(ptw_i.xyz / ptw_i.w);
				ws.Add(ptw_i.w);
			}
		}

		/// <summary>
		/// Convert a list of nd points in homogenous coordinates to a list of (n-1)d points in cartesian
		/// coordinates by perspective division
		/// </summary>
		/// <param name="ptsws">Points in homogenous coordinates</param>
		/// <returns>Points in cartesian coordinates</returns>
		internal static List<Vector3d> HomogenousToCartesian(IList<Vector4d> ptsws)
		{
			var pts = new List<Vector3d>(ptsws.Count);

			for (int i = 0; i < ptsws.Count; ++i)
				pts.Add(HomogenousToCartesian(ptsws[i]));

			return pts;
		}

		/// <summary>
		/// Convert a 2D list of nd points in homogenous coordinates to cartesian
		/// coordinates by perspective division
		/// </summary>
		/// <param name="ptsws">Points in homogenous coordinates</param>
		/// <param name="pts">Points in cartesian coordinates</param>
		/// <param name="ws">Homogenous weights</param>
		internal static void HomogenousToCartesian(Vector4d[,] ptsws, out Vector3d[,] pts, out double[,] ws)
		{
			int width = ptsws.GetLength(0);
			int height = ptsws.GetLength(1);
			pts = new Vector3d[width, height];
			ws = new double[width, height];

			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					Vector4d ptw_ij = ptsws[i, j];
					double w_ij = ptw_ij.w;

					pts[i, j] = ptw_ij.xyz / w_ij;
					ws[i, j] = w_ij;
				}
			}
		}

		/// <summary>
		/// Convert an nd point in cartesian coordinates to an (n+1)d point in homogenous coordinates
		/// </summary>
		/// <param name="pt">Point in cartesian coordinates</param>
		/// <param name="w">Weight</param>
		/// <returns>point in homogenous coordinates</returns>
		internal static Vector4d CartesianToHomogenous(Vector3d pt, double w)
		{
			return new Vector4d(pt * w, w);
		}

		/// <summary>
		/// Convert list of points in cartesian coordinates to homogenous coordinates
		/// </summary>
		/// <param name="pts">Points in cartesian coordinates</param>
		/// <param name="ws">Weights</param>
		/// <returns>Points in homogenous coordinates</returns>
		internal static List<Vector4d> CartesianToHomogenous(IList<Vector3d> pts, IList<double> ws)
		{
			var Cw = new List<Vector4d>(pts.Count);

			for (int i = 0; i < pts.Count; ++i)
				Cw.Add(CartesianToHomogenous(pts[i], ws[i]));

			return Cw;
		}

		/// <summary>
		/// Convert 2D list of points in cartesian coordinates to homogenous coordinates
		/// </summary>
		/// <param name="pts">Points in cartesian coordinates</param>
		/// <param name="ws">Weights</param>
		/// <returns>Points in homogenous coordinates</returns>
		internal static Vector4d[,] CartesianToHomogenous(Vector3d[,] pts, double[,] ws)
		{
			int width = pts.GetLength(0);
			int height = pts.GetLength(1);

			var Cw = new Vector4d[width, height];

			for (int i = 0; i < width; ++i)
				for (int j = 0; j < height; ++j)
					Cw[i, j] = CartesianToHomogenous(pts[i, j], ws[i, j]);

			return Cw;
		}

		/// <summary>
		/// Convert an (n+1)d point to an nd point without perspective division
		/// by truncating the last dimension
		/// </summary>
		/// <param name="pt">Point in homogenous coordinates</param>
		/// <returns>Input point in cartesian coordinates</returns>
		internal static Vector3d TruncateHomogenous(Vector4d pt)
		{
			return pt.xyz;
		}

		/// <summary>
		/// Compute the binomial coefficient
		/// </summary>
		internal static uint Binomial(int n, int k)
		{
			uint result = 1;
			if (k > n)
			{
				return 0;
			}
			for (uint i = 1; i <= k; ++i)
			{
				result *= ((uint)n + 1 - i);
				result /= i;
			}

			return result;
		}

		/// <summary>
		/// Map numbers from one interval to another
		/// </summary>
		internal static double MapToRange(double val, double old_min, double old_max, double new_min, double new_max)
		{
			double old_range = old_max - old_min;
			double new_range = new_max - new_min;
			return (((val - old_min) * new_range) / old_range) + new_min;
		}

	}

}

