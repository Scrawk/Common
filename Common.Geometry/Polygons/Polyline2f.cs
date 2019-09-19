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
            Indices = new int[(Count - 1) * 2];
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
            Lengths = null;
            if (Count == 0) return;

            Lengths = new float[Count];
            Lengths[0] = 0;

            for (int i = 1; i < Count; i++)
            {
                Lengths[i] = Length + Vector2f.Distance(Positions[i-1], Positions[i]);
                Length = Lengths[i];
            }
        }

        public override bool ContainsPoint(Vector2f point)
        {
            if (Count == 0) return false;
            if (!Bounds.Contains(point)) return false;

            float w2 = Width * Width;

            for (int i = 0; i < Count - 1; i++)
            {
                var a = Positions[i];
                var b = Positions[i + 1];
                var seg = new Segment2f(a, b);
                var c = seg.Closest(point);

                if (Vector2f.SqrDistance(c, point) < w2) return true;
            }

            return false;
        }

        /// <summary>
        /// Given the number (0 >= t <= 1) find this length on the 
        /// line and return the index before this point and the 
        /// distance (0 >= s <= 1) from this point to the next.
        /// </summary>
        protected override void FindInterpolationPoint(float t, out int idx, out float s)
        {
            t = FMath.Clamp01(t) * Length;

            if (t == 0)
            {
                s = 0;
                idx = 0;
            }
            else if (t == Length)
            {
                s = 1;
                idx = Count - 2;
            }
            else
            {
                s = 0;
                idx = -1;

                for (int i = 0; i < Count - 1; i++)
                {
                    float len0 = Lengths[i + 0];
                    float len1 = Lengths[i + 1];

                    if (t >= len0 && t < len1)
                    {
                        float len = len1 - len0;

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




