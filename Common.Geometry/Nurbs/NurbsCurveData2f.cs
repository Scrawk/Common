using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A non-uniform rational basis spline curve in 2D space with control points in 3D homogeneous space.
    /// </summary>
    public class NurbsCurveData2f
    {
        
        public NurbsCurveData2f(int degree, IList<Vector2f> control, IList<float> knots, IList<float> weights = null)
        {
            if (!NurbsFunctions.AreValidRelations(degree, control.Count, knots.Count))
                throw new ArgumentException("Not a valid curve.");

            Degree = degree;

            int count = control.Count;
            Control = new Vector3f[count];

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = control[i];
                var w = (weights != null) ? weights[i] : 1;
                Control[i] = new Vector3f(c * w, w);
            }

            count = knots.Count;
            Knots = new float[count];
            knots.CopyTo(Knots, 0);

            NumberOfBasisFunctions = Knots.Length - Degree - 2;
        }

        public NurbsCurveData2f(int degree, IList<Vector3f> control, IList<float> knots)
        {
            if (!NurbsFunctions.AreValidRelations(degree, control.Count, knots.Count))
                throw new ArgumentException("Not a valid curve.");

            Degree = degree;

            int count = control.Count;
            Control = new Vector3f[count];

            //homogenise control points.
            for (int i = 0; i < count; i++)
            {
                var c = control[i].xy;
                var w = control[i].z;
                Control[i] = new Vector3f(c * w, w);
            }

            count = knots.Count;
            Knots = new float[count];
            knots.CopyTo(Knots, 0);

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
        public Vector3f[] Control { get; private set; }

        /// <summary>
        /// The knot vector.
        /// </summary>
        public float[] Knots { get; private set; }

    }
}
