using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
	public static class Util
	{

		public static Vector[] ToVectors(IList<Vector2f> points)
		{
			var copy = new Vector[points.Count];
			for (int i = 0; i < points.Count; i++)
				copy[i] = new Vector(points[i].x, points[i].y);

			return copy;
		}

		public static Vector[] ToVectors(IList<Vector3f> points)
		{
			var copy = new Vector[points.Count];
			for (int i = 0; i < points.Count; i++)
				copy[i] = new Vector(points[i].x, points[i].y, points[i].z);

			return copy;
		}

		public static Vector[] ToVectors(IList<Vector2d> points)
		{
			var copy = new Vector[points.Count];
			for (int i = 0; i < points.Count; i++)
				copy[i] = new Vector(points[i].x, points[i].y);

			return copy;
		}

		public static Vector[] ToVectors(IList<Vector3d> points)
		{
			var copy = new Vector[points.Count];
			for (int i = 0; i < points.Count; i++)
				copy[i] = new Vector(points[i].x, points[i].y, points[i].z);

			return copy;
		}

		public static Vector2d[] ToVectors2d(IList<Vector> points)
		{
			var copy = new Vector2d[points.Count];
			for (int i = 0; i < points.Count; i++)
				copy[i] = new Vector2d(points[i].x, points[i].y);

			return copy;
		}

		public static Vector[] ToArray(IList<Vector> list)
		{
			var copy = new Vector[list.Count];
			for (int i = 0; i < list.Count; i++)
				copy[i] = list[i].Copy();

			return copy;
		}

		public static Vector[,] ToArray(Vector[,] array)
		{
			int width = array.GetLength(0);
			int height = array.GetLength(1);

			var copy = new Vector[width, height];

			for (int x = 0; x < width; x++)
				for (int y = 0; y < height; y++)
					copy[x, y] = array[x, y].Copy();

			return copy;
		}

		public static List<Vector> ToList(IList<Vector> list)
		{
			var copy = new List<Vector>(list.Count);
			for(int i = 0; i < list.Count; i++)
				copy.Add(list[i].Copy());

			return copy;
		}

		/**
		 * Convert an nd point in homogenous coordinates to an (n-1)d point in cartesian
		 * coordinates by perspective division
		 * @param[in] pt Point in homogenous coordinates
		 * @return Point in cartesian coordinates
		 */
		internal static Vector HomogenousToCartesian(Vector pt)
		{
			int nd = pt.Dimension;
			return new Vector(nd - 1, pt / pt[nd - 1]);
		}

		/**
		 * Convert a list of nd points in homogenous coordinates to a list of (n-1)d points in cartesian
		 * coordinates by perspective division
		 * @param[in] ptsws Points in homogenous coordinates
		 * @param[out] pts Points in cartesian coordinates
		 * @param[out] ws Homogenous weights
		 */
		internal static void HomogenousToCartesian(List<Vector> ptsws, out List<Vector> pts, out List<double> ws)
		{
			pts = new List<Vector>(ptsws.Count);
			ws = new List<double>(ptsws.Count);

			for (int i = 0; i < ptsws.Count; ++i)
			{
				Vector ptw_i = ptsws[i];
				int nd = ptw_i.Dimension;

				pts.Add(new Vector(nd - 1, ptw_i / ptw_i[nd - 1]));
				ws.Add(ptw_i[nd - 1]);
			}
		}

		/**
		* Convert a list of nd points in homogenous coordinates to a list of (n-1)d points in cartesian
		* coordinates by perspective division
		* @param[in] ptsws Points in homogenous coordinates
		* @param[out] pts Points in cartesian coordinates
		*/
		internal static List<Vector> HomogenousToCartesian(List<Vector> ptsws)
		{
			var pts = new List<Vector>(ptsws.Count);

			for (int i = 0; i < ptsws.Count; ++i)
			{
				Vector ptw_i = ptsws[i];
				int nd = ptw_i.Dimension;

				pts.Add(new Vector(nd - 1, ptw_i / ptw_i[nd - 1]));
			}

			return pts;
		}

		/**
		 * Convert a 2D list of nd points in homogenous coordinates to cartesian
		 * coordinates by perspective division
		 * @param[in] ptsws Points in homogenous coordinates
		 * @param[out] pts Points in cartesian coordinates
		 * @param[out] ws Homogenous weights
		 */
		internal static void HomogenousToCartesian(Vector[,] ptsws, out Vector[,] pts, out double[,] ws)
		{
			int width = ptsws.GetLength(0);
			int height = ptsws.GetLength(1);
			pts = new Vector[width, height];
			ws = new double[width, height];

			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					Vector ptw_ij = ptsws[i, j];
					int nd = ptw_ij.Dimension;

					double w_ij = ptw_ij[nd - 1];
					pts[i, j] = new Vector(nd - 1, ptw_ij / w_ij);
					ws[i, j] = w_ij;
				}
			}
		}

		/**
		 * Convert an nd point in cartesian coordinates to an (n+1)d point in homogenous
		 * coordinates
		 * @param[in] pt Point in cartesian coordinates
		 * @param[in] w Weight
		 * @return Input point in homogenous coordinates
		 */
		internal static Vector CartesianToHomogenous(Vector pt, double w)
		{
			int nd = pt.Dimension;
			return new Vector(nd + 1, pt * w, w);
		}

		/**
		 * Convert list of points in cartesian coordinates to homogenous coordinates
		 * @param[in] pts Points in cartesian coordinates
		 * @param[in] ws Weights
		 * @return Points in homogenous coordinates
		 */
		internal static List<Vector> CartesianToHomogenous(List<Vector> pts, List<double> ws)
		{
			var Cw = new List<Vector>(pts.Count);
	
			for (int i = 0; i < pts.Count; ++i)
			{
				Cw.Add(CartesianToHomogenous(pts[i], ws[i]));
			}

			return Cw;
		}

		/**
		 * Convert 2D list of points in cartesian coordinates to homogenous coordinates
		 * @param[in] pts Points in cartesian coordinates
		 * @param[in] ws Weights
		 * @return Points in homogenous coordinates
		 */
		internal static Vector[,] CartesianToHomogenous(Vector[,] pts, double[,] ws)
		{
			int width = pts.GetLength(0);
			int height = pts.GetLength(1);

			var Cw = new Vector[width, height];

			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					Cw[i, j] = CartesianToHomogenous(pts[i, j], ws[i, j]);
				}
			}

			return Cw;
		}

		/**
		 * Convert an (n+1)d point to an nd point without perspective division
		 * by truncating the last dimension
		 * @param[in] pt Point in homogenous coordinates
		 * @return Input point in cartesian coordinates
		 */
		internal static Vector TruncateHomogenous(Vector pt)
		{
			int nd = pt.Dimension;
			return new Vector(nd-1, pt);
		}

		/**
		 * Compute the binomial coefficient (nCk) using the formula
		 * \product_{i=0}^k (n + 1 - i) / i
		 */
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

		/**
		 * Check if two numbers are close enough within eps
		 * @param[in] a First number
		 * @param[in] b Second number
		 * @param[in] eps Tolerance for checking closeness
		 * @return Whether the numbers are close w.r.t. the tolerance
		 */
		internal static bool Close(double a, double b, double eps = DMath.EPS)
		{
			return (Math.Abs(a - b) < eps) ? true : false;
		}

		/**
		 * Map numbers from one interval to another
		 * @param[in] val Number to map to another range
		 * @param[in] old_min Minimum value of original range
		 * @param[in] old_max Maximum value of original range
		 * @param[in] new_min Minimum value of new range
		 * @param[in] new_max Maximum value of new range
		 * @return Number mapped to new range
		 */
		internal static double MapToRange(double val, double old_min, double old_max, double new_min, double new_max)
		{
			double old_range = old_max - old_min;
			double new_range = new_max - new_min;
			return (((val - old_min) * new_range) / old_range) + new_min;
		}

	}

}

