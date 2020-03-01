﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Nurbs
{
    /// <summary>
    /// A Bezier curve is a common spline curve
    /// </summary>
    public class BezierCurve3d : NurbsCurve3d
    {

        /// <summary>
        /// Create a bezier curve
        /// </summary>
        /// <param name="points">Array of control points</param>
        /// <param name="weights">Array of control point weights (optional)</param>
        public BezierCurve3d(IList<Vector3d> points, IList<double> weights = null)
            : base(NurbsFunctions.RationalBezierCurve(points, weights))
        {

        }
    }
}