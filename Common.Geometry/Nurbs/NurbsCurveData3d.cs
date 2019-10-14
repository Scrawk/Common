using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A non-uniform rational basis spline curve in 3D space with control points in 4D homogeneous space.
    /// </summary>
    public class NurbsCurveData3d
    {
        
        public NurbsCurveData3d(int degree, IList<Vector3d> control, IList<double> knots, IList<double> weights = null)
        {
            NurbsFunctions.IsValidNurbsCurveData(degree, control, knots);

            Degree = degree;

            int count = control.Count;
            Control = new Vector4d[count];

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = control[i];
                var w = (weights != null) ? weights[i] : 1;
                Control[i] = new Vector4d(c * w, w);
            }

            count = knots.Count;
            Knots = new double[count];
            knots.CopyTo(Knots, 0);

            float min = (float)Knots.First();
            float max = (float)Knots.Last();
            Domain = new Interval1f(min, max);

            NumberOfBasisFunctions = Knots.Length - Degree - 2;
        }

        public NurbsCurveData3d(int degree, IList<Vector4d> control, IList<double> knots)
        {
            NurbsFunctions.IsValidNurbsCurveData(degree, control, knots);

            Degree = degree;

            int count = control.Count;
            Control = new Vector4d[count];

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = control[i].xyz;
                var w = control[i].w;
                Control[i] = new Vector4d(c * w, w);
            }

            count = knots.Count;
            Knots = new double[count];
            knots.CopyTo(Knots, 0);

            float min = (float)Knots.First();
            float max = (float)Knots.Last();
            Domain = new Interval1f(min, max);

            NumberOfBasisFunctions = Knots.Length - Degree - 2;
        }

        /// <summary>
        /// The curves degree.
        /// </summary>
        public readonly int Degree;

        /// <summary>
        /// The number of basis functions in curve, ie n.
        /// </summary>
        public readonly int NumberOfBasisFunctions;

        /// <summary>
        /// The domain of the curve parameter.
        /// </summary>
        public readonly Interval1f Domain;

        /// <summary>
        /// The control points.
        /// </summary>
        internal Vector4d[] Control { get; private set; }

        /// <summary>
        /// The knot vector.
        /// </summary>
        internal double[] Knots { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrintControl()
        {
            string str = "[";
            for (int i = 0; i < Control.Length; i++)
            {
                if (i == 0)
                    str += DehomogenisedControl(i);
                else
                    str += ", " + DehomogenisedControl(i);
            }

            return str + "]";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrintKnots()
        {
            string str = "[";
            for (int i = 0; i < Knots.Length; i++)
            {
                if (i == 0)
                    str += Knots[i];
                else
                    str += ", " + Knots[i];
            }

            return str + "]";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PrintWeights()
        {
            string str = "[";
            for (int i = 0; i < Control.Length; i++)
            {
                if (i == 0)
                    str += Weight(i);
                else
                    str += ", " + Weight(i);
            }

            return str + "]";
        }

        /// <summary>
        /// The control points from homogenise space to world space.
        /// </summary>
        public List<Vector3d> DehomogenisedControl()
        {
            var points = new List<Vector3d>();
            for (int i = 0; i < Control.Length; i++)
                points.Add(DehomogenisedControl(i));

            return points;
        }

        /// <summary>
        /// The control point from homogenise space to world space.
        /// </summary>
        public Vector3d DehomogenisedControl(int i)
        {
            return Control[i].xyz / Control[i].w;
        }

        /// <summary>
        /// The control point weights.
        /// </summary>
        public List<double> Weights()
        {
            var weights = new List<double>();
            for (int i = 0; i < Control.Length; i++)
                weights.Add(Control[i].w);

            return weights;
        }

        /// <summary>
        /// The control point weights.
        /// </summary>
        public double Weight(int i)
        {
            return Control[i].w;
        }

        /// <summary>
        /// Copy data.
        /// </summary>
        public NurbsCurveData3d Copy()
        {
            return new NurbsCurveData3d(Degree, Control, Knots);
        }

        /// <summary>
        /// Reverse the data.
        /// </summary>
        public NurbsCurveData3d Reverse()
        {
            var control = new List<Vector4d>(Control);
            control.Reverse();
            return new NurbsCurveData3d(Degree, control, KnotsReverse());
        }

        /// <summary>
        /// Reverse a knot vector
        /// </summary>
        /// <param name="knots">An array of knots</param>
        /// <returns>The reversed array of knots</returns>
        private List<double> KnotsReverse()
        {
            var min = Knots[0];
            var len = Knots.Length;

            var list = new List<double>(len);
            list.Add(min);

            for (int i = 1; i < len; i++)
                list.Add(list[i - 1] + (Knots[len - i] - Knots[len - i - 1]));

            return list;
        }

    }
}
