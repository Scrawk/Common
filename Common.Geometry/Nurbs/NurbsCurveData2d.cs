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
    public class NurbsCurveData2d
    {
        
        public NurbsCurveData2d(int degree, IList<Vector2d> control, IList<double> knots, IList<double> weights = null)
        {
            NurbsFunctions.IsValidNurbsCurveData(degree, control, knots);

            Degree = degree;

            int count = control.Count;
            Control = new Vector3d[count];

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = control[i];
                var w = (weights != null) ? weights[i] : 1;
                Control[i] = new Vector3d(c * w, w);
            }

            count = knots.Count;
            Knots = new double[count];
            knots.CopyTo(Knots, 0);

            float min = (float)Knots.First();
            float max = (float)Knots.Last();
            Domain = new Interval1f(min, max);

            NumberOfBasisFunctions = Knots.Length - Degree - 2;
        }

        public NurbsCurveData2d(int degree, IList<Vector3d> control, IList<double> knots)
        {
            NurbsFunctions.IsValidNurbsCurveData(degree, control, knots);

            Degree = degree;

            int count = control.Count;
            Control = new Vector3d[count];

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = control[i].xy;
                var w = control[i].z;
                Control[i] = new Vector3d(c * w, w);
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
        internal Vector3d[] Control { get; private set; }

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
                    str += Control[i].xy / Control[i].z;
                else
                    str += ", " + Control[i].xy / Control[i].z;
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
                    str += Control[i].z;
                else
                    str += ", " + Control[i].z;
            }

            return str + "]";
        }

        /// <summary>
        /// The control points from homogenise space to world space.
        /// </summary>
        public List<Vector2d> DehomogenisedControl()
        {
            var points = new List<Vector2d>();
            for (int i = 0; i < Control.Length; i++)
                points.Add(Control[i].xy / Control[i].z);

            return points;
        }

        /// <summary>
        /// The control point weights.
        /// </summary>
        public List<double> Weights()
        {
            var weights = new List<double>();
            for (int i = 0; i < Control.Length; i++)
                weights.Add(Control[i].z);

            return weights;
        }

        /// <summary>
        /// Copy data.
        /// </summary>
        public NurbsCurveData2d Copy()
        {
            return new NurbsCurveData2d(Degree, Control, Knots);
        }

        /// <summary>
        /// Reverse the data.
        /// </summary>
        public NurbsCurveData2d Reverse()
        {
            var control = new List<Vector3d>(Control);
            control.Reverse();
            return new NurbsCurveData2d(Degree, control, KnotsReverse());
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
