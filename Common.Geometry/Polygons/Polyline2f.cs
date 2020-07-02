using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// 
    /// </summary>
    public class Polyline2f : Polyobject2f
    {

        public Polyline2f(float width, int count) : base(count)
        {
            Width = width;
        }

        public Polyline2f(float width, IList<Vector2f> positions) : base(positions)
        {
            Width = width;
        }

        public float Width { get; set; }

        public float Radius => Width * 0.5f;

        public override string ToString()
        {
            return string.Format("[Polyline2f: Count={0}, Length={1}]", Count, Length);
        }

        /// <summary>
        /// Get the position with a clamped index.
        /// </summary>
        public Vector2f GetPosition(int i)
        {
            return Positions.GetClamped(i);
        }

        /// <summary>
        /// Get the param with a clamped index.
        /// </summary>
        public float GetParam(int i)
        {
            if (Params == null)
                throw new InvalidOperationException("Polyline does not have any params.");

            return Params.GetClamped(i);
        }

        /// <summary>
        /// Get the length with a clamped index.
        /// </summary>
        public float GetLength(int i)
        {
            if (Lengths == null)
                throw new InvalidOperationException("Polyline does not have any lengths.");

            return Lengths.GetClamped(i);
        }

        /// <summary>
        /// Create the index array.
        /// </summary>
        public override void CreateIndices()
        {
            CreateIndices((Count - 1) * 2);
            for (int i = 0; i < Count - 1; i++)
            {
                Indices[i * 2 + 0] = i;
                Indices[i * 2 + 1] = i + 1;
            }
        }

        /// <summary>
        /// Will reverse the polyline.
        /// </summary>
        public override void Reverse()
        {
            Array.Reverse(Positions);
            if (Params != null) Array.Reverse(Params);
            if (Lengths != null) Array.Reverse(Lengths);
        }

        /// <summary>
        /// Copy the polyline.
        /// No need to recalculate the copy.
        /// </summary>
        public Polyline2f Copy()
        {
            var copy = new Polyline2f(Width, Positions);
            copy.Length = Length;
            copy.Bounds = Bounds;

            if (Params != null)
                copy.SetParams(Params);

            if (Lengths != null)
                copy.SetLengths(Lengths);

            return copy;
        }

        /// <summary>
        /// Update the polylines properties.
        /// Should be called when polyline
        /// created or changes. 
        /// </summary>
        public void Calculate()
        {
            CalculateLengths();
            CalculateBounds();
        }

        /// <summary>
        /// Calculate the total length of the line
        /// and the length of each segment in line.
        /// Lengths represents the length of line 
        /// up to that point.
        /// </summary>
        public override void CalculateLengths()
        {
            Length = 0;

            int size = Count;
            if (size == 0) return;

            CreateLengths(size);
            Lengths[0] = 0;

            for (int i = 1; i < size; i++)
            {
                var p0 = GetPosition(i - 1);
                var p1 = GetPosition(i);

                Lengths[i] = Length + Vector2f.Distance(p0, p1);
                Length = Lengths[i];
            }
        }

        /// <summary>
        /// Does the line contain the point.
        /// The line has some thickness from its width.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(Vector2f point)
        {
            if (Count == 0) return false;
            if (!Bounds.Contains(point)) return false;

            float radius = Width * 0.5f;

            for (int i = 0; i < Count - 1; i++)
            {
                var a = Positions[i];
                var b = Positions[i + 1];
                var seg = new Segment2f(a, b);
                var sdf = seg.SignedDistance(point) - radius;

                if (sdf < 0.0f) return true;
            }

            return false;
        }

        /// <summary>
        /// The signed distance from the line to the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override float SignedDistance(Vector2f point)
        {
            if (Count == 0) 
                return float.PositiveInfinity;

            float sdf = float.PositiveInfinity;
            float radius = Width * 0.5f;

            for (int i = 0; i < Count - 1; i++)
            {
                var a = Positions[i];
                var b = Positions[i + 1];
                var seg = new Segment2f(a, b);

                sdf = Math.Min(sdf, seg.SignedDistance(point));
            }

            return sdf - radius;
        }

    }
}




