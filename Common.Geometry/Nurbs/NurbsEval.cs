using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{

	internal static class NurbsEval
	{

		/**
		Evaluate point on a nonrational NURBS curve
		@param[in] crv Curve object
		@param[in] u Parameter to evaluate the curve at.
		@return point Resulting point on the curve at parameter u.
		*/
		internal static Vector CurvePoint(NurbsCurve crv, double u)
		{
			return CurvePoint(crv.Degree, crv.Knots, crv.ControlPoints, u);
		}

		/**
		 * Evaluate point on a rational NURBS curve
		 * @param[in] crv RationalCurve object
		 * @param[in] u Parameter to evaluate the curve at.
		 * @return point Resulting point on the curve.
		 */
		internal static Vector CurvePoint(RationalNurbsCurve crv, double u)
		{
			// Compute homogenous coordinates of control points
			var Cw = new List<Vector>(crv.ControlPoints.Count);

			for (int i = 0; i < crv.ControlPoints.Count; i++)
			{
				Cw.Add(NurbsUtil.CartesianToHomogenous(crv.ControlPoints[i], crv.Weights[i]));
			}

			// Compute point using homogenous coordinates
			Vector pointw = CurvePoint(crv.Degree, crv.Knots, Cw, u);

			// Convert back to cartesian coordinates
			return NurbsUtil.HomogenousToCartesian(pointw);
		}

		/**
		 * Evaluate derivatives of a non-rational NURBS curve
		 * @param[in] crv Curve object
		 * @param[in] num_ders Number of times to derivate.
		 * @param[in] u Parameter to evaluate the derivatives at.
		 * @return curve_ders Derivatives of the curve at u.
		 * E.g. curve_ders[n] is the nth derivative at u, where 0 <= n <= num_ders.
		 */
		internal static Vector[] CurveDerivatives(NurbsCurve crv, int num_ders, double u)
		{
			return CurveDerivatives(crv.Degree, crv.Knots, crv.ControlPoints, num_ders, u);
		}

		/**
		 * Evaluate derivatives of a rational NURBS curve
		 * @param[in] u Parameter to evaluate the derivatives at.
		 * @param[in] knots Knot vector of the curve.
		 * @param[in] control_points Control points of the curve.
		 * @param[in] weights Weights corresponding to each control point.
		 * @param[in] num_ders Number of times to differentiate.
		 * @param[inout] curve_ders Derivatives of the curve at u.
		 * E.g. curve_ders[n] is the nth derivative at u, where n is between 0 and
		 * num_ders-1.
		 */
		internal static List<Vector> CurveDerivatives(RationalNurbsCurve crv, int num_ders, double u)
		{
			var curve_ders = new List<Vector>(num_ders + 1);

			// Compute homogenous coordinates of control points
			var Cw = new List<Vector>(crv.ControlPoints.Count);

			for (int i = 0; i < crv.ControlPoints.Count; i++)
			{
				Cw.Add(NurbsUtil.CartesianToHomogenous(crv.ControlPoints[i], crv.Weights[i]));
			}

			// Derivatives of Cw
			var Cwders = CurveDerivatives(crv.Degree, crv.Knots, Cw, num_ders, u);

			// Split Cwders into coordinates and weights
			var Aders = new List<Vector>();
			var wders = new List<double>();

			foreach (var val in Cwders)
			{
				Aders.Add(NurbsUtil.TruncateHomogenous(val));
				wders.Add(val.Last);
			}

			// Compute rational derivatives
			for (int k = 0; k <= num_ders; k++)
			{
				var v = Aders[k];
				for (int i = 1; i <= k; i++)
				{
					v -= curve_ders[k - i] * NurbsUtil.Binomial(k, i) * wders[i];
				}

				curve_ders.Add(v / wders[0]);
			}


			return curve_ders;
		}

		/**
		 * Evaluate the tangent of a B-spline curve
		 * @param[in] crv Curve object
		 * @return Unit tangent of the curve at u.
		 */
		internal static Vector CurveTangent(NurbsCurve crv, double u)
		{
			var ders = CurveDerivatives(crv, 1, u);
			var du = ders[1];
			var du_len = du.Magnitude;

			if (!MathUtil.IsZero(du_len))
			{
				du /= du_len;
			}

			return du;
		}

		/**
		 * Evaluate the tangent of a rational B-spline curve
		 * @param[in] crv RationalCurve object
		 * @return Unit tangent of the curve at u.
		 */
		internal static Vector CurveTangent(RationalNurbsCurve crv, double u)
		{
			var ders = CurveDerivatives(crv, 1, u);
			var du = ders[1];
			var du_len = du.Magnitude;

			if (!MathUtil.IsZero(du_len))
			{
				du /= du_len;
			}

			return du;
		}

		/**
		 * Evaluate point on a nonrational NURBS surface
		 * @param[in] srf Surface object
		 * @param[in] u Parameter to evaluate the surface at.
		 * @param[in] v Parameter to evaluate the surface at.
		 * @return Resulting point on the surface at (u, v).
		 */
		internal static Vector SurfacePoint(NurbsSurface srf, double u, double v)
		{
			return SurfacePoint(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v, srf.control_points, u, v);
		}

		/**
		 * Evaluate point on a non-rational NURBS surface
		 * @param[in] srf RationalSurface object
		 * @param[in] u Parameter to evaluate the surface at.
		 * @param[in] v Parameter to evaluate the surface at.
		 * @return Resulting point on the surface at (u, v).
		 */
		internal static Vector SurfacePoint(RationalNurbsSurface srf, double u, double v)
		{
			int width = srf.control_points.GetLength(0);
			int height = srf.control_points.GetLength(1);

			// Compute homogenous coordinates of control points
			var Cw = new Vector[width, height];

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Cw[i, j] = NurbsUtil.CartesianToHomogenous(srf.control_points[i, j], srf.weights[i, j]);
				}
			}

			// Compute point using homogenous coordinates
			var pointw = SurfacePoint(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v, Cw, u, v);

			// Convert back to cartesian coordinates
			return NurbsUtil.HomogenousToCartesian(pointw);
		}

		/**
		 * Evaluate derivatives on a non-rational NURBS surface
		 * @param[in] degree_u Degree of the given surface in u-direction.
		 * @param[in] degree_v Degree of the given surface in v-direction.
		 * @param[in] knots_u Knot vector of the surface in u-direction.
		 * @param[in] knots_v Knot vector of the surface in v-direction.
		 * @param[in] control_points Control points of the surface in a 2D array.
		 * @param[in] num_ders Number of times to differentiate
		 * @param[in] u Parameter to evaluate the surface at.
		 * @param[in] v Parameter to evaluate the surface at.
		 * @return surf_ders Derivatives of the surface at (u, v).
		 */
		internal static Vector[,] SurfaceDerivatives(NurbsSurface srf, int num_ders, double u, double v)
		{
			return SurfaceDerivatives(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v,
				srf.control_points, num_ders, u, v);
		}

		/**
		 * Evaluate derivatives on a rational NURBS surface
		 * @param[in] srf RationalSurface object
		 * @param[in] u Parameter to evaluate the surface at.
		 * @param[in] v Parameter to evaluate the surface at.
		 * @param[in] num_ders Number of times to differentiate
		 * @return Derivatives on the surface at parameter (u, v).
		 */
		internal static Vector[,] SurfaceDerivatives(RationalNurbsSurface srf, int num_ders, double u, double v)
		{
			int width = srf.control_points.GetLength(0);
			int height = srf.control_points.GetLength(1);
			var homo_cp = new Vector[width, height];

			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					homo_cp[i, j] = NurbsUtil.CartesianToHomogenous(srf.control_points[i, j], srf.weights[i, j]);
				}
			}

			var homo_ders = SurfaceDerivatives(srf.degree_u, srf.degree_v, srf.knots_u, srf.knots_v, homo_cp, num_ders, u, v);

			var Aders = new Vector[num_ders + 1, num_ders + 1];

			for (int i = 0; i < homo_ders.GetLength(0); ++i)
			{
				for (int j = 0; j < homo_ders.GetLength(1); ++j)
				{
					Aders[i, j] = NurbsUtil.TruncateHomogenous(homo_ders[i, j]);
				}
			}

			var surf_ders = new Vector[num_ders + 1, num_ders + 1];

			for (int k = 0; k < num_ders + 1; ++k)
			{
				for (int l = 0; l < num_ders - k + 1; ++l)
				{
					var der = Aders[k, l];

					for (int j = 1; j < l + 1; ++j)
					{
						der -= surf_ders[k, l - j] * (homo_ders[0, j].Last * NurbsUtil.Binomial(l, j));
					}

					for (int i = 1; i < k + 1; ++i)
					{
						der -= surf_ders[k - i, l] * (homo_ders[i, 0].Last * NurbsUtil.Binomial(k, i));

						Vector tmp = new Vector(der.Dimension);
						for (int j = 1; j < l + 1; ++j)
						{
							tmp -= surf_ders[k - 1, l - j] * (homo_ders[i, j].Last * NurbsUtil.Binomial(l, j));
						}

						der -= tmp * NurbsUtil.Binomial(k, i);
					}

					der *= 1 / homo_ders[0, 0].Last;
					surf_ders[k, l] = der;
				}
			}
			return surf_ders;
		}

		/**
		 * Evaluate the two orthogonal tangents of a non-rational surface at the given
		 * parameters
		 * @param[in] srf Surface object
		 * @param u Parameter in the u-direction
		 * @param v Parameter in the v-direction
		 * @return Tuple with unit tangents along u- and v-directions
		 */
		internal static (Vector, Vector) SurfaceTangent(NurbsSurface srf, double u, double v)
		{
			var ptder = SurfaceDerivatives(srf, 1, u, v);
			var du = ptder[1, 0];
			var dv = ptder[0, 1];

			var du_len = du.Magnitude;
			var dv_len = dv.Magnitude;

			if (!MathUtil.IsZero(du_len))
				du /= du_len;

			if (!MathUtil.IsZero(dv_len))
				dv /= dv_len;

			return (du, dv);
		}

		/**
		 * Evaluate the two orthogonal tangents of a rational surface at the given
		 * parameters
		 * @param[in] srf Rational Surface object
		 * @param u Parameter in the u-direction
		 * @param v Parameter in the v-direction
		 * @return Tuple with unit tangents along u- and v-directions
		 */
		internal static (Vector, Vector) SurfaceTangent(RationalNurbsSurface srf, double u, double v)
		{
			var ptder = SurfaceDerivatives(srf, 1, u, v);
			var du = ptder[1, 0];
			var dv = ptder[0, 1];

			var du_len = du.Magnitude;
			var dv_len = dv.Magnitude;

			if (!MathUtil.IsZero(du_len))
				du /= du_len;
			
			if (!MathUtil.IsZero(dv_len))
				dv /= dv_len;

			return (du, dv);
		}

		/**
		 * Evaluate the normal a non-rational surface at the given parameters
		 * @param[in] srf Surface object
		 * @param u Parameter in the u-direction
		 * @param v Parameter in the v-direction
		 * @param[inout] normal Unit normal at of the surface at (u, v)
		 */
		internal static Vector SurfaceNormal(NurbsSurface srf, double u, double v)
		{
			var ptder = SurfaceDerivatives(srf, 1, u, v);

			//TODO - fix me
			var n = Vector.Cross3(ptder[0, 1], ptder[1, 0]);

			var n_len = n.Magnitude;
			if (!MathUtil.IsZero(n_len))
				n /= n_len;

			return n;
		}

		/**
		 * Evaluate the normal of a rational surface at the given parameters
		 * @param[in] srf Rational Surface object
		 * @param u Parameter in the u-direction
		 * @param v Parameter in the v-direction
		 * @return Unit normal at of the surface at (u, v)
		 */
		internal static Vector SurfaceNormal(RationalNurbsSurface srf, double u, double v)
		{
			var ptder = SurfaceDerivatives(srf, 1, u, v);

			//TODO - fix me
			var n = Vector.Cross3(ptder[0, 1], ptder[1, 0]);

			var n_len = n.Magnitude;
			if (!MathUtil.IsZero(n_len))
				n /= n_len;
			
			return n;
		}

		/**
		 * Evaluate point on a nonrational NURBS curve
		 * @param[in] degree Degree of the given curve.
		 * @param[in] knots Knot vector of the curve.
		 * @param[in] control_points Control points of the curve.
		 * @param[in] u Parameter to evaluate the curve at.
		 * @return point Resulting point on the curve at parameter u.
		 */
		private static Vector CurvePoint(int degree, List<double> knots, List<Vector> control_points, double u)
		{
			int nd = control_points[0].Dimension;
			var point = new Vector(nd);

			// Find span and corresponding non-zero basis functions
			int span = NurbsBasis.FindSpan(degree, knots, u);
			var N = NurbsBasis.BSplineBasis(degree, span, knots, u);

			// Compute point
			for (int j = 0; j <= degree; j++)
			{
				point += control_points[span - degree + j] * N[j];
			}

			return point;
		}

		/**
		 * Evaluate derivatives of a non-rational NURBS curve
		 * @param[in] degree Degree of the curve
		 * @param[in] knots Knot vector of the curve.
		 * @param[in] control_points Control points of the curve.
		 * @param[in] num_ders Number of times to derivate.
		 * @param[in] u Parameter to evaluate the derivatives at.
		 * @return curve_ders Derivatives of the curve at u.
		 * E.g. curve_ders[n] is the nth derivative at u, where 0 <= n <= num_ders.
		 */
		private static Vector[] CurveDerivatives(int degree, List<double> knots, List<Vector> control_points, int num_ders, double u)
		{
			int nd = control_points[0].Dimension;
			var curve_ders = new Vector[num_ders + 1];

			// Assign higher order derivatives to zero
			for (int k = degree + 1; k <= num_ders; k++)
			{
				curve_ders[k] = new Vector(nd);
			}

			// Find the span and corresponding non-zero basis functions & derivatives
			int span = NurbsBasis.FindSpan(degree, knots, u);
			var ders = NurbsBasis.BSplineDerBasis(degree, span, knots, u, num_ders);

			// Compute first num_ders derivatives
			int du = num_ders < degree ? num_ders : degree;
			for (int k = 0; k <= du; k++)
			{
				curve_ders[k] = new Vector(nd);
				for (int j = 0; j <= degree; j++)
				{
					curve_ders[k] += control_points[span - degree + j] * ders[k, j];
				}
			}

			return curve_ders;
		}

		/**
		 * Evaluate point on a nonrational NURBS surface
		 * @param[in] degree_u Degree of the given surface in u-direction.
		 * @param[in] degree_v Degree of the given surface in v-direction.
		 * @param[in] knots_u Knot vector of the surface in u-direction.
		 * @param[in] knots_v Knot vector of the surface in v-direction.
		 * @param[in] control_points Control points of the surface in a 2d array.
		 * @param[in] u Parameter to evaluate the surface at.
		 * @param[in] v Parameter to evaluate the surface at.
		 * @return point Resulting point on the surface at (u, v).
		 */
		private static Vector SurfacePoint(int degree_u, int degree_v, List<double> knots_u, List<double> knots_v,
			Vector[,] control_points, double u, double v)
		{
			int nd = control_points[0, 0].Dimension;
			// Initialize result to 0s
			var point = new Vector(nd);

			// Find span and non-zero basis functions
			int span_u = NurbsBasis.FindSpan(degree_u, knots_u, u);
			int span_v = NurbsBasis.FindSpan(degree_v, knots_v, v);
			var Nu = NurbsBasis.BSplineBasis(degree_u, span_u, knots_u, u);
			var Nv = NurbsBasis.BSplineBasis(degree_v, span_v, knots_v, v);

			for (int l = 0; l <= degree_v; l++)
			{
				var temp = new Vector(nd);
				for (int k = 0; k <= degree_u; k++)
				{
					temp += control_points[span_u - degree_u + k, span_v - degree_v + l] * Nu[k];
				}

				point += temp * Nv[l];
			}

			return point;
		}

		/**
		 * Evaluate derivatives on a non-rational NURBS surface
		 * @param[in] degree_u Degree of the given surface in u-direction.
		 * @param[in] degree_v Degree of the given surface in v-direction.
		 * @param[in] knots_u Knot vector of the surface in u-direction.
		 * @param[in] knots_v Knot vector of the surface in v-direction.
		 * @param[in] control_points Control points of the surface in a 2D array.
		 * @param[in] num_ders Number of times to differentiate
		 * @param[in] u Parameter to evaluate the surface at.
		 * @param[in] v Parameter to evaluate the surface at.
		 * @param[out] surf_ders Derivatives of the surface at (u, v).
		 */
		private static Vector[,] SurfaceDerivatives(int degree_u, int degree_v, List<double> knots_u,
					List<double> knots_v, Vector[,] control_points, int num_ders, double u, double v)
		{
			int nd = control_points[0, 0].Dimension;

			var surf_ders = new Vector[num_ders + 1, num_ders + 1];

			// Set higher order derivatives to 0
			for (int k = degree_u + 1; k <= num_ders; k++)
			{
				for (int l = degree_v + 1; l <= num_ders; l++)
				{
					surf_ders[k, l] = new Vector(nd);
				}
			}

			// Find span and basis function derivatives
			int span_u = NurbsBasis.FindSpan(degree_u, knots_u, u);
			int span_v = NurbsBasis.FindSpan(degree_v, knots_v, v);
			var ders_u = NurbsBasis.BSplineDerBasis(degree_u, span_u, knots_u, u, num_ders);
			var ders_v = NurbsBasis.BSplineDerBasis(degree_v, span_v, knots_v, v, num_ders);

			// Number of non-zero derivatives is <= degree
			int du = Math.Min(num_ders, degree_u);
			int dv = Math.Min(num_ders, degree_v);

			var temp = new Vector[degree_v + 1];
			// Compute derivatives
			for (int k = 0; k <= du; k++)
			{
				for (int s = 0; s <= degree_v; s++)
				{
					temp[s] = new Vector(nd);
					for (int r = 0; r <= degree_u; r++)
					{
						temp[s] += control_points[span_u - degree_u + r, span_v - degree_v + s] * ders_u[k, r];
					}
				}

				int dd = Math.Min(num_ders - k, dv);

				for (int l = 0; l <= dd; l++)
				{
					for (int s = 0; s <= degree_v; s++)
					{
						surf_ders[k, l] += temp[s] * ders_v[l, s];
					}
				}
			}

			return surf_ders;
		}

	}

}

