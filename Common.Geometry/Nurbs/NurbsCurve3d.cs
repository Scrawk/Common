using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Nurbs
{
	public class NurbsCurve3d
    {

        protected NurbsCurveData3d m_data;
		
		public NurbsCurve3d()
        {
		}
		
		public NurbsCurve3d(NurbsCurveData3d data)
        {
			m_data = Check.IsValidNurbsCurveData(data);
        }
	
		public static NurbsCurve3d ByKnotsControlPointsWeights(int degree, IList<double> knots, IList<Vector3d> controlPoints, IList<double> weights)
        {
			return new NurbsCurve3d(new NurbsCurveData3d(degree, knots, Eval.Homogenize1d(controlPoints, weights)));
		}
		
        /*
		public static NurbsCurve3d ByPoints(int degree, IList<Vector3d> points)
        {
		    return new NurbsCurve3d(Make.RationalInterpCurve(degree, points));
		}
        */

        public virtual int Degree => m_data.Degree;

        public double[] Knots => m_data.Knots;

        public Vector3d[] ControlPoints => Eval.Dehomogenize1d(m_data.ControlPoints);

        public double[] Weights => Eval.Weight1d(m_data.ControlPoints);

        public NurbsCurveData3d AsNurbs()
        {
			return new NurbsCurveData3d(Degree, Knots, Eval.Homogenize1d(ControlPoints, Weights));
		}
		
		public NurbsCurve3d Copy()
        {
			return new NurbsCurve3d(m_data);
		}
		
		public Interval1d Domain => new Interval1d(Knots.First(), Knots.Last());

        /*
        public NurbsCurve3d Transform(Matrix4x4d mat)
        {
			return new NurbsCurve3d(Modify.RationalCurveTransform(m_data, mat));
		}

        public Vector3d Point(double u)
        {
            return Eval.RationalCurvePoint(m_data, u);
        }
		
		public Vector3d Tangent(double u)
        {
			return Eval.RationalCurveTangent(m_data, u);
		}
		
		public Vector3d[] Derivatives(double u, int numDerivs = 1)
        {
	        return Eval.rationalCurveDerivatives(m_data, u);
		}
		
		public Vector3d ClosestPoint(Vector3d pt)
        {
			return Analyze.RationalCurveClosestPoint(m_data, pt);
		}
		
		public double ClosestParam(Vector3d pt)
        {
			return Analyze.rationalCurveClosestParam(m_data, pt);
		}
		
		public double Length()
        {
			return Analyze.RationalCurveArcLength(m_data);
		}
		
		public double LengthAtParam(double u)
        {
			return Analyze.RationalCurveArcLength(m_data);
		}
		
		
		public double ParamAtLength(double len, double tolerance = DMath.EPS)
        {
			return Analyze.RationalCurveParamAtArcLength(m_data, len, tolerance, null, null);
		}
		
		public CurveLengthSample[] DivideByEqualArcLength(int divisions)
        {
			return Divide.rationalCurveByEqualArcLength(m_data, divisions);
		}
		
		public CurveLengthSample[] DivideByArcLength(double arcLength)
        {
			return Divide.rationalCurveByArcLength(m_data, arcLength);
		}
		
		public NurbsCurve3d[] Split(double u)
        {
			return Divide.CurveSplit(m_data, u);
		}
		
		public NurbsCurve3d Reverse()
        {
			return new NurbsCurve3d(Modify.CurveReverse(m_data));
		}
		
		public Vector3d[] Tessellate(double tolerance = DMath.EPS)
        {
			return Tess.RationalCurveAdaptiveSample(m_data, tolerance, false);
		}
        */
	}
}


