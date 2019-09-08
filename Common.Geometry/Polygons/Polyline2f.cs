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

        public float Length { get; private set; }

        public override string ToString()
        {
            return string.Format("[Polyline2f: Count={0}, Length={1}]", Count, Length);
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

            return copy;
        }

        /// <summary>
        /// Update the polylines properties.
        /// Should be called when polyline
        /// created or changes. 
        /// </summary>
        public void Calculate()
        {
            CalculateLength();
            CalculateBounds();
        }

        public void CalculateLength()
        {
            Length = 0;
            if (Count == 0) return;

            for (int i = 0; i < Count - 1; i++)
                Length += Vector2f.Distance(Positions[i], Positions[i + 1]);
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

    }
}




