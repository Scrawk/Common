using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A non-uniform rational basis spline curve in 2D space with control points in 3D homogeneous space.
    /// </summary>
    public class NurbsCurve2f
    {
        private NurbsCurveData2f m_data;

        public NurbsCurve2f(int degree, IList<Vector2f> control, IList<float> knots, IList<float> weights = null)
        {
            m_data = new NurbsCurveData2f(degree, control, knots, weights);
        }

        public NurbsCurve2f(int degree, IList<Vector3f> control, IList<float> knots)
        {
            m_data = new NurbsCurveData2f(degree, control, knots);
        }

        private NurbsCurve2f(NurbsCurveData2f data)
        {
            m_data = data;
        }

        /// <summary>
        /// The curves degree.
        /// </summary>
        public int Degree => m_data.Degree;

        /// <summary>
        /// The number of basis functions in curve, ie n.
        /// </summary>
        public int NumberOfBasisFunctions => m_data.NumberOfBasisFunctions;

        /// <summary>
        /// The control points.
        /// </summary>
        public Vector3f[] Control => m_data.Control;

        /// <summary>
        /// The knot vector.
        /// </summary>
        public float[] Knots => m_data.Knots;

        /// <summary>
        /// The control points from homogenise space to world space.
        /// </summary>
        public List<Vector2f> DehomogenisedControl => m_data.DehomogenisedControl();

        /// <summary>
        /// The control point weights.
        /// </summary>
        public List<float> Weights => m_data.Weights();

        /// <summary>
        /// Copy curve.
        /// </summary>
        public NurbsCurve2f Copy()
        {
            return new NurbsCurve2f(m_data.Copy());
        }

        /// <summary>
        /// Determine the arc length of the curve at the given parameter.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        public float Length(float u)
        {
            u = FMath.Clamp01(u);
            return NurbsFunctions.RationalArcLength(m_data, u);
        }

        /// <summary>
        /// Determine the parameter of the curve at the given arc length.
        /// </summary>
        /// <param name="len">The arc length at which to determine the parameter</param>
        public float ParamAtLength(float len)
        {
            return NurbsFunctions.RationalParamAtArcLength(m_data, len);
        }

        /// <summary>
        /// Compute a point on a non-uniform, rational b-spline curve.
        /// Corresponds to algorithm 4.1 from The NURBS book, Piegl & Tiller 2nd edition
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        public Vector2f Position(float u)
        {
            u = FMath.Clamp01(u);

            int n = NumberOfBasisFunctions;
            var span = NurbsFunctions.FindSpan(u, Degree, n, Knots);
            var basis = NurbsFunctions.BasisFunctions(u, Degree, span, Knots);

            Vector3f p = new Vector3f();
            for (int i = 0; i <= Degree; i++)
                p = p + basis[i] * Control[span - Degree + i];

            return p.xy / p.z;
        }

        /// <summary>
        /// The tangent on the curve at u.
        /// </summary>
        /// <param name="u">Number between 0 and 1.</param>
        public Vector2f Tangent(float u)
        {
            Vector2f d = Derivatives(u, 1)[1];
            return d.Normalized;
        }

        /// <summary>
        /// The normal on the curve at u.
        /// </summary>
        /// <param name="u">Number between 0 and 1.</param>
        public Vector2f Normal(float u)
        {
            Vector2f d = Derivatives(u, 1)[1];
            return d.Normalized.PerpendicularCW;
        }

        /// <summary>
        /// Determine the derivatives of a non-uniform, rational B-spline curve at a given parameter.
        /// Corresponds to algorithm 4.2 from The NURBS book, Piegl & Tiller 2nd edition.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        /// <param name="numDerivs">The number of derivatives to compute.</param>
    	public Vector2f[] Derivatives(float u, int numDerivs)
        {
            u = FMath.Clamp01(u);

            var ders = NurbsFunctions.RationalDerivatives(m_data, u, numDerivs);

            var CK = new Vector2f[numDerivs + 1];

            for (int k = 0; k <= numDerivs; k++)
            {
                var v = ders[k].xy;

                for (int i = 1; i <= k; i++)
                    v = v - (float)IMath.Binomial(k, i) * ders[i].z * CK[k - i];

                CK[k] = v / ders[0].z;
            }

            return CK;
        }

        /// <summary>
        /// Create a curve that passes through the provided points.
        /// </summary>
        /// <param name="degree">The degree of the curve.</param>
        /// <param name="points">The points the curve must pass through.</param>
        /// <returns></returns>
        public static NurbsCurve2f FromPoints(int degree, IList<Vector2f> points)
        {
            var data = NurbsFunctions.RationalInterpolate(degree, points);
            return new NurbsCurve2f(data);
        }

        /// <summary>
        /// Split the curve.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        /// <returns>Two new curves.</returns>
        public NurbsCurve2f[] Split(float u)
        {
            u = FMath.Clamp01(u);

            var data = NurbsFunctions.Split(m_data, u);

            return new NurbsCurve2f[]
            {
                new NurbsCurve2f(data[0]),
                new NurbsCurve2f(data[1])
            };
        }

        /// <summary>
        /// Determine the parameters necessary to divide the curve into equal arc length segments.
        /// </summary>
        /// <param name="divisions">Number of divisions of the curve</param>
        /// <returns>A collection of parameters</returns>
        public List<CurveLengthSample> DivideByEqualArcLength(int divisions)
        {
            return NurbsFunctions.RationalByEqualArcLength(m_data, divisions);
        }

    }
}
