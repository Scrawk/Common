using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A non-uniform rational basis spline curve in 2D space with control points in 3D homogeneous space.
    /// </summary>
    public class NurbsCurve2d
    {
        private NurbsCurveData2d m_data;

        public NurbsCurve2d(int degree, IList<Vector2d> control, IList<double> knots, IList<double> weights = null)
        {
            m_data = new NurbsCurveData2d(degree, control, knots, weights);
        }

        public NurbsCurve2d(int degree, IList<Vector3d> control, IList<double> knots)
        {
            m_data = new NurbsCurveData2d(degree, control, knots);
        }

        internal NurbsCurve2d(NurbsCurveData2d data)
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
        public List<Vector3d> Control => new List<Vector3d>(m_data.Control);

        /// <summary>
        /// The knot vector.
        /// </summary>
        public List<double> Knots => new List<double>(m_data.Knots);

        /// <summary>
        /// The control points from homogenise space to world space.
        /// </summary>
        public List<Vector2d> DehomogenisedControl => m_data.DehomogenisedControl();

        /// <summary>
        /// The control point weights.
        /// </summary>
        public List<double> Weights => m_data.Weights();

        /// <summary>
        /// The domain of the curve parameter.
        /// </summary>
        public Interval1f Domain => m_data.Domain;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrintControl()
        {
            return m_data.PrintControl();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrintKnots()
        {
            return m_data.PrintKnots();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrintWeights()
        {
            return m_data.PrintWeights();
        }

        /// <summary>
        /// Copy curve.
        /// </summary>
        public NurbsCurve2d Copy()
        {
            return new NurbsCurve2d(m_data.Copy());
        }

        /// <summary>
        /// Reverse curve.
        /// </summary>
        public NurbsCurve2d Reverse()
        {
            return new NurbsCurve2d(m_data.Reverse());
        }

        /// <summary>
        /// Determine the arc length of the curve at the given parameter.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        public double Length(double u)
        {
            u = DMath.Clamp01(u);
            u = Domain.Min + u * Domain.Length;

            return NurbsFunctions.RationalArcLength(m_data, u);
        }

        /// <summary>
        /// Determine the parameter of the curve at the given arc length.
        /// </summary>
        /// <param name="len">The arc length at which to determine the parameter</param>
        public double ParamAtLength(double len)
        {
            return NurbsFunctions.RationalParamAtArcLength(m_data, len);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        public Vector2d Position(double u)
        {
            u = DMath.Clamp01(u);
            u = Domain.Min + u * Domain.Length;

            return NurbsFunctions.RationalPoint(m_data, u);
        }

        /// <summary>
        /// The tangent on the curve at u.
        /// </summary>
        /// <param name="u">Number between 0 and 1.</param>
        public Vector2d Tangent(double u)
        {
            Vector2d d = Derivatives(u, 1)[1];
            return d.Normalized;
        }

        /// <summary>
        /// The normal on the curve at u.
        /// </summary>
        /// <param name="u">Number between 0 and 1.</param>
        public Vector2d Normal(double u)
        {
            Vector2d d = Derivatives(u, 1)[1];
            return d.Normalized.PerpendicularCW;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        /// <param name="numDerivs">The number of derivatives to compute.</param>
    	public Vector2d[] Derivatives(double u, int numDerivs)
        {
            u = DMath.Clamp01(u);
            u = Domain.Min + u * Domain.Length;

            return NurbsFunctions.RationalDerivatives(m_data, u, numDerivs);
        }

        /// <summary>
        /// Create a curve that passes through the provided points.
        /// </summary>
        /// <param name="degree">The degree of the curve.</param>
        /// <param name="points">The points the curve must pass through.</param>
        /// <returns></returns>
        public static NurbsCurve2d FromPoints(int degree, IList<Vector2d> points)
        {
            var data = NurbsFunctions.RationalInterpolate(degree, points);
            return new NurbsCurve2d(data);
        }

        /// <summary>
        /// Split the curve.
        /// </summary>
        /// <param name="u">Parameter 0 <= u <= 1</param>
        /// <returns>Two new curves.</returns>
        public NurbsCurve2d[] Split(double u)
        {
            u = DMath.Clamp01(u);
            u = Domain.Min + u * Domain.Length;

            var data = NurbsFunctions.Split(m_data, u);

            return new NurbsCurve2d[]
            {
                new NurbsCurve2d(data[0]),
                new NurbsCurve2d(data[1])
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

        /// <summary>
        /// Tessellate a curve.
        /// </summary>
        /// <param name="numSamples">integer number of samples</param>
        /// <returns></returns>
        public List<Vector2d> Tessellate(int numSamples)
        {
            return NurbsFunctions.RationalRegularSampleRange(m_data, numSamples);
        }

        /// <summary>
        /// Transform the curve.
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public NurbsCurve2d Transform(Matrix4x4d mat)
        {
            int count = m_data.Control.Length;
            var control = new List<Vector2d>(count);

            for (int i = 0; i < count; i++)
            {
                var p = (Control[i].xy / Control[i].z).xy01;
                p = mat * p;
                control.Add(p.xy);
            }

            return new NurbsCurve2d(Degree, control, Knots, Weights);
        }
    }
}
