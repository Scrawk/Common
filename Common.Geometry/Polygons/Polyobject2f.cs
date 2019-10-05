using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Polygons
{

    /// <summary>
    /// Base class for polygon and polyline.
    /// </summary>
    public abstract class Polyobject2f
    {

        public Polyobject2f(int count)
        {
            SetPositions(count);
        }

        public Polyobject2f(IList<Vector2f> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public Box2f Bounds { get; protected set; }

        public float Length { get; protected set; }

        public Vector2f[] Positions { get; private set; }

        public float[] Params { get; private set; }

        public float[] Lengths { get; protected set; }

        public int[] Indices { get; protected set; }

        /// <summary>
        /// Get the position with a interpolated index.
        /// </summary>
        public Vector2f GetPosition(float t)
        {
            return Interpolate(t, Positions);
        }

        /// <summary>
        /// Get the length with a interpolated index.
        /// </summary>
        public float GetLength(float t)
        {
            if (Lengths == null)
                throw new InvalidOperationException("Polyobject does not have any lengths.");

            return Interpolate(t, Lengths);
        }

        /// <summary>
        /// Get the param with a interpolated index.
        /// </summary>
        public float GetParam(float t)
        {
            if (Params == null)
                throw new InvalidOperationException("Polyobject does not have any params.");

            return Interpolate(t, Params);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new Vector2f[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="positions">Array to copy from.</param>
        public void SetPositions(IList<Vector2f> positions)
        {
            SetPositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        /// <summary>
        /// Create the param array.
        /// </summary>
        public void CreateParams()
        {
            Params = new float[Count];
        }

        /// <summary>
        /// Create the param array.
        /// </summary>
        public void SetParams(IList<float> _params)
        {
            if (Params == null) Params = new float[Count];
            _params.CopyTo(Params, 0);
        }

        /// <summary>
        /// Create the length array.
        /// </summary>
        protected void SetLengths(IList<float> lengths)
        {
            if (Lengths == null) Lengths = new float[Count];
            lengths.CopyTo(Lengths, 0);
        }

        /// <summary>
        /// Find the shapes bounding box.
        /// </summary>
        public void CalculateBounds()
        {
            Bounds = new Box2f();
            if (Count == 0) return;

            var min = Vector2f.PositiveInfinity;
            var max = Vector2f.NegativeInfinity;

            for (int i = 0; i < Count; i++)
            {
                var p = Positions[i];

                if (p.x < min.x) min.x = p.x;
                if (p.x > max.x) max.x = p.x;
                if (p.y < min.y) min.y = p.y;
                if (p.y > max.y) max.y = p.y;
            }

            Bounds = new Box2f(min, max);
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
        public abstract bool ContainsPoint(Vector2f point);

        /// <summary>
        /// Given the number (0 >= t <= 1) interpolate 
        /// along the object to find the value in the array.
        /// </summary>
        public Vector2f Interpolate(float t, IList<Vector2f> array)
        {
            FindInterpolationPoint(t, out int idx, out float s);
            return Vector2f.Lerp(array[idx], array.GetCircular(idx + 1), s);
        }

        /// <summary>
        /// Given the number (0 >= t <= 1) interpolate 
        /// along the object to find the value in the array.
        /// </summary>
        public float Interpolate(float t, IList<float> array)
        {
            FindInterpolationPoint(t, out int idx, out float s);
            return FMath.Lerp(array[idx], array.GetCircular(idx + 1), s);
        }

        /// <summary>
        /// Translate the positions.
        /// </summary>
        public void Translate(Vector2f translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        /// <summary>
        /// Scale the positions.
        /// </summary>
        public void Scale(Vector2f scale)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= scale;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(Matrix4x4f m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = (m * Positions[i].xy01).xy;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(Matrix2x2f m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }

        /// <summary>
        /// Given the number (0 >= t <= 1) find this length on the 
        /// object and return the index before this point and the 
        /// distance (0 >= s <= 1) from this point to the next.
        /// </summary>
        protected abstract void FindInterpolationPoint(float t, out int idx, out float s);
    }
}