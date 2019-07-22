using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// 
    /// </summary>
    public class Polyline2d
    {

        public Polyline2d(int count)
        {
            SetPositions(count);
        }

        public Polyline2d(IList<Vector2d> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public Vector2d[] Positions { get; private set; }

        public double[] Params { get; private set; }

        public int[] Indices { get; private set; }

        public double Length { get; private set; }

        public Box2d Bounds { get; private set; }


        public override string ToString()
        {
            return string.Format("[Polyline2d: Count={0}, Length={1}]", Count, Length);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new Vector2d[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="positions">Array to copy from.</param>
        public void SetPositions(IList<Vector2d> positions)
        {
            SetPositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        /// <summary>
        /// Create the param array.
        /// </summary>
        public void CreateParams()
        {
            Params = new double[Count];
        }

        /// <summary>
        /// Create the param array.
        /// </summary>
        public void SetParams(IList<double> _params)
        {
            if (Params == null) Params = new double[Count];
	        _params.CopyTo(Params, 0);
        }

        /// <summary>
        /// Create the index array.
        /// </summary>
        public void CreateIndices()
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
        public void Reverse()
        {
            Array.Reverse(Positions);
            if (Params != null) Array.Reverse(Params);
        }

        /// <summary>
        /// Copy the polyline.
        /// No need to recalculate the copy.
        /// </summary>
        public Polyline2d Copy()
        {
            var copy = new Polyline2d(Positions);
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
                Length += Vector2d.Distance(Positions[i], Positions[i + 1]);
        }

        public void CalculateBounds()
        {
            Bounds = new Box2d();
            if (Count == 0) return;

            var min = Vector2d.PositiveInfinity;
            var max = Vector2d.NegativeInfinity;

            for (int i = 0; i < Count; i++)
            {
                var p = Positions[i];

                if (p.x < min.x) min.x = p.x;
                if (p.x > max.x) max.x = p.x;
                if (p.y < min.y) min.y = p.y;
                if (p.y > max.y) max.y = p.y;
            }

            Bounds = new Box2d(min, max);
        }

        public bool ContainsPoint(Vector2d point, double width)
        {
            if (Count == 0) return false;
            if (!Bounds.Contains(point)) return false;

            double w2 = width * width;

            for (int i = 0; i < Count - 1; i++)
            {
                var a = Positions[i];
                var b = Positions[i + 1];
                var seg = new Segment2d(a, b);
                var c = seg.Closest(point);

                if (Vector2d.SqrDistance(c, point) < w2) return true;
            }

            return false;
        }

    }
}



