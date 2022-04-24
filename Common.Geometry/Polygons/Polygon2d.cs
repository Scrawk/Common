using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;
using Common.Core.Extensions;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;
using POINT2 = Common.Core.Numerics.Point2d;
using BOX2 = Common.Core.Shapes.Box2d;
using SEGMENT2 = Common.Core.Shapes.Segment2d;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// A simple polygon with no holes.
    /// Maybe CCW or CW.
    /// </summary>
    public class Polygon2d : Polyobject2d
    {

        /// <summary>
        /// Create a polygon.
        /// </summary>
        /// <param name="count">The number of points in polygon.</param>
        public Polygon2d(int count) : base(count)
        {
        }

        /// <summary>
        /// Create a polygon.
        /// </summary>
        /// <param name="positions">The polygons positions.</param>
        public Polygon2d(IList<POINT2> positions) : base(positions)
        {

        }

        /// <summary>
        /// The signed area of the polygon.
        /// CCW polygons have a positive area.
        /// </summary>
        public REAL SignedArea { get; private set; }

        /// <summary>
        /// The polygons area.
        /// </summary>
        public REAL Area => Math.Abs(SignedArea);

        /// <summary>
        /// The polygons centroid.
        /// </summary>
        public POINT2 Centroid { get; private set; }

        /// <summary>
        /// Is the polygon points ordered clockwise.
        /// </summary>
        public bool IsCW => SignedArea < 0;

        /// <summary>
        /// Is the polygon points ordered counter clockwise.
        /// </summary>
        public bool IsCCW => SignedArea > 0;

        /// <summary>
        /// Are the polygon points degenerate.
        /// </summary>
        public bool IsDegenerate => SignedArea == 0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Polygon2d: Count={0}, Length={1}, Area={2}, IsCCW={3}]",
                Count, Length, Area, IsCCW);
        }

        /// <summary>
        /// Get the position with a circular index.
        /// </summary>
        public POINT2 GetPosition(int i)
        {
            return Positions.GetWrapped(i);
        }

        /// <summary>
        /// Get the param with a circular index.
        /// </summary>
        public REAL GetParam(int i)
        {
            if (Params == null)
                throw new InvalidOperationException("Polygon does not have any params.");

            return Params.GetWrapped(i);
        }

        /// <summary>
        /// Get the length with a circular index.
        /// </summary>
        public REAL GetLength(int i)
        {
            if (Lengths == null)
                throw new InvalidOperationException("Polygon does not have any lengths.");

            return Lengths.GetWrapped(i);
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
        public Polygon2d Copy()
        {
            var copy = new Polygon2d(Positions);
            copy.SignedArea = SignedArea;
            copy.Centroid = Centroid;
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
            CalculateCentroid();
            CalculateBounds();
            CalculateArea();
            CalculateLengths();
        }

        /// <summary>
        /// The centroid of the polygon.
        /// </summary>
        public void CalculateCentroid()
        {
            REAL det = 0;
            Centroid = POINT2.Zero;

            for (int i = 0; i < Count; i++)
            {
                var a = GetPosition(i);
                var b = GetPosition(i + 1);

                // compute the determinant
                REAL tempDet = a.x * b.y - b.x * a.y;
                det += tempDet;

                Centroid += (a + b) * tempDet;
            }

            Centroid /= (3 * det);
        }

        /// <summary>
        /// The polygons area.
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        public void CalculateArea()
        {
            SignedArea = 0;
            if (Count < 3) return;

            REAL firstProducts = 0.0;
            REAL secondProducts = 0.0;

            for (int i = 0; i < Count; i++)
            {
                var p0 = GetPosition(i);
                var p1 = GetPosition(i + 1);

                firstProducts += p0.x * p1.y;
                secondProducts += p0.y * p1.x;
            }

            SignedArea = (firstProducts - secondProducts) / 2.0;
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

                Lengths[i] = Length + POINT2.Distance(p0, p1);
                Length = Lengths[i];
            }
        }

        /// <summary>
        /// Does the polygon contain the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(POINT2 point, bool includeBorder = true)
        {
            if (Count < 3) return false;
            if (!Bounds.Contains(point, true)) return false;

            var ab = new SEGMENT2();
            ab.A = new POINT2(Bounds.Min.x - Bounds.Width, point.y);
            ab.B = point;

            int windingNumber = 0;
            for (int i = 0; i < Count; i++)
            {
                var c = GetPosition(i);
                var d = GetPosition(i + 1);
                var cd = new SEGMENT2(c, d);

                if (ab.Intersects(cd)) windingNumber++;
            }

            return (windingNumber % 2 != 0);
        }

        /// <summary>
        /// The sigined distance from the polygon to the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override REAL SignedDistance(POINT2 point)
        {
            if (Count < 3)
                return REAL.PositiveInfinity;

            var v0 = Positions[0];

            REAL d = VECTOR2.Dot(point - v0, point - v0);
            REAL s = 1.0f;

            for (int i = 0; i < Count; i++)
            {
                POINT2 vj = GetPosition(i - 1);
                POINT2 vi = GetPosition(i);

                VECTOR2 e = vj - vi;
                VECTOR2 w = point - vi;

                REAL we = VECTOR2.Dot(w, e);
                REAL ee = VECTOR2.Dot(e, e);
                VECTOR2 b = w - e * MathUtil.Clamp(MathUtil.SafeDiv(we, ee), 0.0, 1.0);

                d = Math.Min(d, VECTOR2.Dot(b, b));

                bool b0 = point.y >= vi.y;
                bool b1 = point.y < vj.y;
                bool b2 = e.x * w.y > e.y * w.x;

                if ((b0 && b1 && b2) || (!b0 && !b1 && !b2))
                    s *= -1.0f;
            }

            return s * MathUtil.Sqrt(d);
        }

        /// <summary>
        /// Create a triangle polygon from the 3 points.
        /// </summary>
        public static Polygon2d FromTriangle(POINT2 a, POINT2 b, POINT2 c)
        {
            var polygon = new Polygon2d(3);

            polygon.Positions[0] = a;
            polygon.Positions[1] = b;
            polygon.Positions[2] = c;

            return polygon;
        }

        /// <summary>
        /// Create a box polygon from the 4 points.
        /// </summary>
        public static Polygon2d FromBox(POINT2 min, POINT2 max)
        {
            var polygon = new Polygon2d(4);

            polygon.Positions[0] = min;
            polygon.Positions[1] = new POINT2(max.x, min.y);
            polygon.Positions[2] = max;
            polygon.Positions[3] = new POINT2(min.x, max.y);

            return polygon;
        }

        /// <summary>
        /// Create a circle polygon.
        /// </summary>
        public static Polygon2d FromCircle(POINT2 center, REAL radius, int segments)
        {
            var polygon = new Polygon2d(segments);

            REAL pi = MathUtil.PI_64;
            REAL fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                REAL theta = 2.0 * pi * i / fseg;

                REAL x = -radius * MathUtil.Cos(theta);
                REAL y = -radius * MathUtil.Sin(theta);

                polygon.Positions[i] = center + new POINT2(x, y);
            }

            return polygon;
        }
    }
}