using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A non-uniform rational basis spline curve in 2D space with control points in 3D homogeneous space.
    /// </summary>
    public class NurbsCurveData2f
    {
        
        public NurbsCurveData2f(int degree, IList<Vector2d> control, IList<double> knots, IList<double> weights = null)
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
            NormalizeKnots();

            NumberOfBasisFunctions = Knots.Length - Degree - 2;
        }

        public NurbsCurveData2f(int degree, IList<Vector3d> control, IList<double> knots)
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
            NormalizeKnots();

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
        /// The control points.
        /// </summary>
        public Vector3d[] Control { get; private set; }

        /// <summary>
        /// The knot vector.
        /// </summary>
        public double[] Knots { get; private set; }

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
        public NurbsCurveData2f Copy()
        {
            return new NurbsCurveData2f(Degree, Control, Knots);
        }

        /// <summary>
        /// 
        /// </summary>
        private void NormalizeKnots()
        {
            double min = Knots.First();
            double max = Knots.Last();
            for (int i = 0; i < Knots.Length; i++)
                Knots[i] = Math.Round(DMath.Normalize(Knots[i], min, max), 4);
        }

    }
}
