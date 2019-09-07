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
    public class Polygon2f
    {

        public Polygon2f(int count)
        {
            SetPositions(count);
        }

        public Polygon2f(IList<Vector2f> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public Vector2f[] Positions { get; private set; }

        public float[] Params { get; private set; }

        public int[] Indices { get; private set; }

        public float SignedArea { get; private set; }

        public float Area => Math.Abs(SignedArea);

        public Vector2f Centroid { get; private set; }

        public Box2f Bounds { get; private set; }

        public bool IsCW => SignedArea < 0.0;

        public bool IsCCW => SignedArea > 0.0;

        public bool IsDegenerate => SignedArea == 0.0;

        public override string ToString()
        {
            return string.Format("[Polygon2f: Count={0}, Area={1}, IsCCW={2}]", Count, Area, IsCCW);
        }

        public Vector2f GetPosition(int i)
        {
            return Positions[IMath.Wrap(i + 1, Count)];
        }

        public float GetParam(int i)
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
        public Polygon2f Copy()
        {
            var copy = new Polygon2f(Positions);
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
            Centroid = Vector2f.Zero;
            if (Count == 0) return;

            for (int i = 0; i < Count; i++)
                Centroid += Positions[i];

            Centroid /= Count;
        }

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
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        public void CalculateArea()
        {
            SignedArea = 0;
            if (Count == 0) return;

            float firstProducts = 0.0f;
            float secondProducts = 0.0f;

            for (int i = 0; i < Count; i++)
            {
                var p0 = GetPosition(i);
                var p1 = GetPosition(i + 1);

                firstProducts += p0.x * p1.y;
                secondProducts += p0.y * p1.x;
            }

            SignedArea = (firstProducts - secondProducts) / 2.0f;
        }

        public bool ContainsPoint(Vector2f point)
        {
            if (Count == 0) return false;
            if (!Bounds.Contains(point)) return false;

            var ab = new Segment2f();
            ab.A = new Vector2f(Bounds.Min.x - Bounds.Width, point.y);
            ab.B = point;

            int windingNumber = 0;
            for (int i = 0; i < Count; i++)
            {
                var c = GetPosition(i);
                var d = GetPosition(i + 1);
                var cd = new Segment2f(c, d);

                if (ab.Intersects(cd)) windingNumber++;
            }

            return (windingNumber % 2 != 0);
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

        public static Polygon2f FromBox(Vector2f min, Vector2f max)
        {
            var polygon = new Polygon2f(4);

            polygon.Positions[0] = min;
            polygon.Positions[1] = new Vector2f(max.x, min.y);
            polygon.Positions[2] = max;
            polygon.Positions[3] = new Vector2f(min.x, max.y);

            return polygon;
        }

        public static Polygon2f FromCircle(Vector2f center, float radius, int segments)
        {
            var polygon = new Polygon2f(segments);

            float pi = FMath.PI;
            float fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = 2.0f * pi * i / fseg;

                float x = -radius * FMath.Cos(theta);
                float y = -radius * FMath.Sin(theta);

                polygon.Positions[i] = center + new Vector2f(x, y);
            }

            return polygon;
        }
    }
}