using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// A simple polygon with no holes.
    /// Maybe CCW or CW.
    /// </summary>
    public class Polygon2d
    {

        public Polygon2d(int count)
        {
            SetPositions(count);
        }

        public Polygon2d(IList<Vector2d> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public Vector2d[] Positions { get; private set; }

        public double[] Params { get; private set; }

        public int[] Indices { get; private set; }

        public double SignedArea { get; private set; }

        public double Area => Math.Abs(SignedArea);

        public Vector2d Centroid { get; private set; }

        public Box2d Bounds { get; private set; }

        public bool IsCW => SignedArea < 0.0;

        public bool IsCCW => SignedArea > 0.0;

        public bool IsDegenerate => SignedArea == 0.0;

        public override string ToString()
        {
            return string.Format("[Polygon2d: Count={0}, Area={1}, IsCCW={2}]", Count, Area, IsCCW);
        }

        public Vector2d GetPosition(int i)
        {
            return Positions[IMath.Wrap(i + 1, Count)];
        }

        public double GetParam(int i)
        {
            if (Params == null)
                throw new InvalidOperationException("Polygon does not have any params.");

            return Params[IMath.Wrap(i + 1, Count)];
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
            Indices = new int[Count * 2];
            for (int i = 0; i < Count; i++)
            {
                Indices[i * 2 + 0] = i;
                Indices[i * 2 + 1] = IMath.Wrap(i + 1, Count);
            }
        }

        /// <summary>
        /// Will reverse the polygon.
        /// A CCW polygon will be come a CW polygon.
        /// No need to recalculate.
        /// </summary>
        public void Reverse()
        {
            Array.Reverse(Positions);
            if (Params != null) Array.Reverse(Params);
            SignedArea *= -1;
        }

        /// <summary>
        /// Copy the polygon.
        /// No need to recalculate the copy.
        /// </summary>
        public Polygon2d Copy()
        {
            var copy = new Polygon2d(Positions);
            copy.SignedArea = SignedArea;
            copy.Centroid = Centroid;
            copy.Bounds = Bounds;

            return copy;
        }

        /// <summary>
        /// Update the polygons properties.
        /// Should be called when polygon
        /// created or changes. 
        /// </summary>
        public void Calculate()
        {
            CalculateCentroid();
            CalculateBounds();
            CalculateArea();
        }

        public void CalculateCentroid()
        {
            Centroid = Vector2d.Zero;
            if (Count == 0) return;

            for (int i = 0; i < Count; i++)
                Centroid += Positions[i];

            Centroid /= Count;
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

        /// <summary>
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        public void CalculateArea()
        {
            SignedArea = 0;
            if (Count == 0) return;

            double firstProducts = 0.0;
            double secondProducts = 0.0;

            for (int i = 0; i < Count; i++)
            {
                var p0 = GetPosition(i);
                var p1 = GetPosition(i + 1);

                firstProducts += p0.x * p1.y;
                secondProducts += p0.y * p1.x;
            }

            SignedArea = (firstProducts - secondProducts) / 2.0;
        }

        public bool ContainsPoint(Vector2d point)
        {
            if (Count == 0) return false;
            if (!Bounds.Contains(point)) return false;

            var ab = new Segment2d();
            ab.A = new Vector2d(Bounds.Min.x - Bounds.Width, point.y);
            ab.B = point;

            int windingNumber = 0;
            for (int i = 0; i < Count; i++)
            {
                var c = GetPosition(i);
                var d = GetPosition(i + 1);
                var cd = new Segment2d(c, d);

                if (ab.Intersects(cd)) windingNumber++;
            }

            return (windingNumber % 2 != 0);
        }

        public static Polygon2d FromBox(Vector2d min, Vector2d max)
        {
            var polygon = new Polygon2d(4);

            polygon.Positions[0] = min;
            polygon.Positions[1] = new Vector2d(max.x, min.y);
            polygon.Positions[2] = max;
            polygon.Positions[3] = new Vector2d(min.x, max.y);

            return polygon;
        }

        public static Polygon2d FromCircle(Vector2d center, double radius, int segments)
        {
            var polygon = new Polygon2d(segments);

            double pi = Math.PI;
            double fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                double theta = 2.0f * pi * i / fseg;

                double x = -radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);

                polygon.Positions[i] = center + new Vector2d(x, y);
            }

            return polygon;
        }
    }
}