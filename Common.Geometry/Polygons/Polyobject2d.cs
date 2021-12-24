using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using REAL = System.Double;
using POINT2 = Common.Core.Numerics.Point2d;
using BOX2 = Common.Geometry.Shapes.Box2d;
using MATRIX2 = Common.Core.Numerics.Matrix2x2d;
using MATRIX4 = Common.Core.Numerics.Matrix4x4d;

namespace Common.Geometry.Polygons
{

    /// <summary>
    /// Base class for polygon and polyline.
    /// </summary>
    public abstract class Polyobject2d
    {

        public Polyobject2d(int count)
        {
            CreatePositions(count);
        }

        public Polyobject2d(IList<POINT2> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public BOX2 Bounds { get; protected set; }

        public REAL Length { get; protected set; }

        public POINT2[] Positions { get; private set; }

        public REAL[] Params { get; private set; }

        public REAL[] Lengths { get; private set; }

        public int[] Indices { get; private set; }

        /// <summary>
        /// Get the position with a interpolated index.
        /// </summary>
        public POINT2 GetPosition(REAL t)
        {
            return Interpolate(t, Positions);
        }

        /// <summary>
        /// Get the length with a interpolated index.
        /// </summary>
        public REAL GetLength(REAL t)
        {
            if (Lengths == null)
                throw new InvalidOperationException("Polyobject does not have any lengths.");

            return Interpolate(t, Lengths);
        }

        /// <summary>
        /// Get the param with a interpolated index.
        /// </summary>
        public REAL GetParam(REAL t)
        {
            if (Params == null)
                throw new InvalidOperationException("Polyobject does not have any params.");

            return Interpolate(t, Params);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void CreatePositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new POINT2[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="positions">Array to copy from.</param>
        public void SetPositions(IList<POINT2> positions)
        {
            CreatePositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        /// <summary>
        /// Create the param array.
        /// </summary>
        public void CreateParams()
        {
            if (Params == null || Params.Length != Count)
                Params = new REAL[Count];
        }

        /// <summary>
        /// Create the param array.
        /// </summary>
        public void SetParams(IList<REAL> _params)
        {
            CreateParams();
            _params.CopyTo(Params, 0);
        }

        /// <summary>
        /// Create the length array.
        /// </summary>
        protected void CreateLengths(int size)
        {
            if (Lengths == null || Lengths.Length != size)
                Lengths = new REAL[size];
        }

        /// <summary>
        /// Create the lengths array.
        /// </summary>
        /// <param name="lengths">Array to copy from.</param>
        protected void SetLengths(IList<REAL> lengths)
        {
            CreateLengths(lengths.Count);
            lengths.CopyTo(Lengths, 0);
        }

        /// <summary>
        /// Create the indices array.
        /// </summary>
        protected void CreateIndices(int size)
        {
            if (Indices == null || Indices.Length != size)
                Indices = new int[size];
        }

        /// <summary>
        /// Find the shapes bounding box.
        /// </summary>
        public void CalculateBounds()
        {
            Bounds = new BOX2();
            if (Count == 0) return;

            var min = POINT2.PositiveInfinity;
            var max = POINT2.NegativeInfinity;

            for (int i = 0; i < Count; i++)
            {
                var p = Positions[i];

                if (p.x < min.x) min.x = p.x;
                if (p.x > max.x) max.x = p.x;
                if (p.y < min.y) min.y = p.y;
                if (p.y > max.y) max.y = p.y;
            }

            Bounds = new BOX2(min, max);
        }

        /// <summary>
        /// Create the lengths array.
        /// </summary>
        public abstract void CalculateLengths();

        /// <summary>
        /// Will reverse the polyshape.
        /// </summary>
        public abstract void Reverse();

        /// <summary>
        /// Create the index array.
        /// </summary>
        public abstract void CreateIndices();

        /// <summary>
        /// Does the shape contain the points.
        /// </summary>
        public abstract bool Contains(POINT2 point);

        /// <summary>
        /// The signed distance to the point.
        /// </summary>
        public abstract REAL SignedDistance(POINT2 point);

        /// <summary>
        /// Given the number normalized interpolate 
        /// along the object to find the value in the array.
        /// </summary>
        public POINT2 Interpolate(REAL t, IList<POINT2> array)
        {
            FindInterpolationPoint(t, out int idx, out REAL s);
            return POINT2.Lerp(array[idx], array.GetCircular(idx + 1), s);
        }

        /// <summary>
        /// Given the number normalized interpolate 
        /// along the object to find the value in the array.
        /// </summary>
        public REAL Interpolate(REAL t, IList<REAL> array)
        {
            FindInterpolationPoint(t, out int idx, out REAL s);
            return MathUtil.Lerp(array[idx], array.GetCircular(idx + 1), s);
        }

        /// <summary>
        /// Translate the positions.
        /// </summary>
        public void Translate(POINT2 translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        /// <summary>
        /// Scale the positions.
        /// </summary>
        public void Scale(POINT2 scale)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= scale;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(MATRIX4 m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = (m * Positions[i].xy01).xy;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(MATRIX2 m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }

        /// <summary>
        /// Given the normalized number find this length on the 
        /// object and return the index before this point and the 
        /// distance from this point to the next.
        /// </summary>
        /// <param name="t">The normalized param representing length between first and last point.</param>
        /// <param name="idx">The index of the point.</param>
        /// <param name="s">The normalized param representing length between idx and idx+1.</param>
        private void FindInterpolationPoint(REAL t, out int idx, out REAL s)
        {
            t = MathUtil.Clamp01(t) * Length;
            int size = Lengths.Length;

            if (t == 0)
            {
                s = 0;
                idx = 0;
            }
            else if (t == Length)
            {
                s = 1;
                idx = size - 2;
            }
            else
            {
                s = 0;
                idx = 0;

                for (int i = 0; i < size - 1; i++)
                {
                    var len0 = Lengths[i + 0];
                    var len1 = Lengths.GetClamped(i + 1);

                    if (t >= len0 && t < len1)
                    {
                        var len = len1 - len0;

                        if (len <= 0)
                            s = 0;
                        else
                            s = (t - len0) / len;

                        idx = i;
                    }
                }
            }
        }

    }
}