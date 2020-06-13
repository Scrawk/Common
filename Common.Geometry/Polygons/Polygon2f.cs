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
    public class Polygon2f : Polyobject2f
    {

        public Polygon2f(int count) : base(count)
        {
        }

        public Polygon2f(IList<Vector2f> positions) : base(positions)
        {

        }

        public float SignedArea { get; private set; }

        public float Area => Math.Abs(SignedArea);

        public Vector2f ConvexCentroid { get; private set; }

        public bool IsCW => SignedArea < 0.0;

        public bool IsCCW => SignedArea > 0.0;

        public bool IsDegenerate => SignedArea == 0.0;

        public override string ToString()
        {
            return string.Format("[Polygon2f: Count={0}, Length={1}, Area={2}, IsCCW={3}]",
                Count, Length, Area, IsCCW);
        }

        /// <summary>
        /// Get the position with a circular index.
        /// </summary>
        public Vector2f GetPosition(int i)
        {
            return Positions.GetCircular(i);
        }

        /// <summary>
        /// Get the param with a circular index.
        /// </summary>
        public float GetParam(int i)
        {
            if (Params == null)
                throw new InvalidOperationException("Polygon does not have any params.");

            return Params.GetCircular(i);
        }

        /// <summary>
        /// Get the length with a circular index.
        /// </summary>
        public float GetLength(int i)
        {
            if (Lengths == null)
                throw new InvalidOperationException("Polygon does not have any lengths.");

            return Lengths.GetCircular(i);
        }

        /// <summary>
        /// Create the index array.
        /// </summary>
        public override void CreateIndices()
        {
            CreateIndices(Count * 2);
            for (int i = 0; i < Count; i++)
            {
                Indices[i * 2 + 0] = i;
                Indices[i * 2 + 1] = MathUtil.Wrap(i + 1, Count);
            }
        }

        /// <summary>
        /// Will reverse the polygon.
        /// A CCW polygon will be come a CW polygon.
        /// No need to recalculate.
        /// </summary>
        public override void Reverse()
        {
            Array.Reverse(Positions);
            if (Params != null) Array.Reverse(Params);
            if (Lengths != null) Array.Reverse(Lengths);
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
            copy.ConvexCentroid = ConvexCentroid;
            copy.Bounds = Bounds;

            if (Params != null)
                copy.SetParams(Params);

            if (Lengths != null)
                copy.SetLengths(Lengths);

            return copy;
        }

        /// <summary>
        /// Update the polygons properties.
        /// Should be called when polygon
        /// created or changes. 
        /// </summary>
        public void Calculate()
        {
            CalculateConvexCentroid();
            CalculateBounds();
            CalculateArea();
            CalculateLengths();
        }

        /// <summary>
        /// The centroid of the polygon presuming its convex.
        /// </summary>
        public void CalculateConvexCentroid()
        {
            float det = 0;
            ConvexCentroid = Vector2f.Zero;

            for (int i = 0; i < Count; i++)
            {
                var a = GetPosition(i);
                var b = GetPosition(i + 1);

                // compute the determinant
                float tempDet = a.x * b.y - b.x * a.y;
                det += tempDet;

                ConvexCentroid += (a + b) * tempDet;
            }

            ConvexCentroid /= (3 * det);
        }

        /// <summary>
        /// The polygons area.
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        public void CalculateArea()
        {
            SignedArea = 0;
            if (Count < 3) return;

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

        /// <summary>
        /// Create the lengths array.
        /// </summary>
        public override void CalculateLengths()
        {
            Length = 0;

            int size = Count;
            if (size < 3) return;

            //polygons need a extra length to wrap
            //back to start position.
            size++;

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
        /// Does the polygon contain the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(Vector2f point)
        {
            if (Count < 3) return false;
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
        /// The sigined distance from the polygon to the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override float SignedDistance(Vector2f point)
        {
            if (Count < 3) 
                return float.PositiveInfinity;

            var v0 = Positions[0];

            float d = Vector2f.Dot(point - v0, point - v0);
            float s = 1.0f;

            for (int i = 0; i < Count; i++)
            {
                Vector2f vj = GetPosition(i - 1);
                Vector2f vi = GetPosition(i);

                Vector2f e = vj - vi;
                Vector2f w = point - vi;

                float we = Vector2f.Dot(w, e);
                float ee = Vector2f.Dot(e, e);
                Vector2f b = w - e * MathUtil.Clamp(MathUtil.SafeDiv(we, ee), 0.0f, 1.0f);

                d = Math.Min(d, Vector2f.Dot(b, b));

                bool b0 = point.y >= vi.y;
                bool b1 = point.y < vj.y;
                bool b2 = e.x * w.y > e.y * w.x;

                if ((b0 && b1 && b2) || (!b0 && !b1 && !b2))
                    s *= -1.0f;
            }

            return s * MathUtil.Sqrt(d);
        }

        public static Polygon2f FromTriangle(Vector2f a, Vector2f b, Vector2f c)
        {
            var polygon = new Polygon2f(3);

            polygon.Positions[0] = a;
            polygon.Positions[1] = b;
            polygon.Positions[2] = c;

            return polygon;
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

            float pi = MathUtil.PI;
            float fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = 2.0f * pi * i / fseg;

                float x = -radius * MathUtil.Cos(theta);
                float y = -radius * MathUtil.Sin(theta);

                polygon.Positions[i] = center + new Vector2f(x, y);
            }

            return polygon;
        }
    }
}