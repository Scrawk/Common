using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;

namespace Common.Geometry.Shapes
{

    /// <summary>
    ///  Represents a line by means of three coefficients
    ///  a, b and c, where ax + by + c = 0 holds.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Line2d : IEquatable<Line2d>
    {

        public REAL A, B, C;

        public Line2d(REAL a, REAL b, REAL c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Line2d(VECTOR2 p1, VECTOR2 p2)
        {
            A = p1.y - p2.y;
            B = p2.x - p1.x;
            C = p1.x * p2.y - p2.x * p1.y;
        }

        /// <summary>
        /// Find the slope of the line.
        /// </summary>
        public REAL Slope
        {
            get
            {
                if (DMath.IsZero(B)) return 0;
                return -A / B;
            }
        }

        /// <summary>
        /// Determines whether the line is ascending
        /// (that is, makes an angle with the positive
        /// direction of the X axis that lies in (0, pi/2).
        /// </summary>
        public bool IsAscending
        {
            get
            {
                return (DMath.IsZero(B) || (-A / B) >= 0);
            }
        }

        /// <summary>
        /// Determines whether the line is vertical
        /// (that is, makes an angle with the positive
        /// direction of the X axis that is equal to pi/2.
        /// </summary>
        public bool IsVertical
        {
            get
            {
                return DMath.IsZero(B) && !DMath.IsZero(A);
            }
        }

        /// <summary>
        /// Determines whether the line is descending
        /// (that is, makes an angle with the positive
        /// direction of the X axis that lies in (pi/2, pi).
        /// </summary>
        public bool IsDescending
        {
            get
            {
                return (DMath.IsZero(B) || (-A / B) < 0);
            }
        }

        /// <summary>
        /// Determines whether the line is horizontal
        /// (that is, makes an angle with the positive
        /// direction of the X axis that is equal to pi.
        /// </summary>
        public bool IsHorizontal
        {
            get
            {
                return (DMath.IsZero(A) && !DMath.IsZero(B));
            }
        }

        /// <summary>
        /// Determines whether the line is undefined
        /// (e.g.two equal points were passed to the constructor).
        /// </summary>
        public bool IsUndefined
        {
            get
            {
                return (DMath.IsZero(A) && DMath.IsZero(B) && DMath.IsZero(C));
            }
        }

        /// <summary>
        /// Calculates the angle that the line makes
        /// with the positive direction of the X axis.
        /// </summary>
        /// <returns></returns>
        public REAL Angle
        {
            get
            {
                if (IsVertical) return DMath.PI / 2.0;

                REAL atan = Math.Atan(-A / B);
                if (atan < 0) atan += DMath.PI;

                return atan;
            }
        }

        public static bool operator ==(Line2d i1, Line2d i2)
        {
            return i1.A == i2.A && i1.B == i2.B && i1.C == i2.C;
        }

        public static bool operator !=(Line2d i1, Line2d i2)
        {
            return i1.A != i2.A || i1.B != i2.B || i1.C != i2.C;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Line2d)) return false;
            Line2d line = (Line2d)obj;
            return this == line;
        }

        public bool Equals(Line2d line)
        {
            return this == line;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                hash = (hash * 16777619) ^ C.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Line2d: A={0}, B={1}, C={2}]", A, B, C);
        }

        /// <summary>
        /// Calculates the X coordinate of a point on the line by its Y coordinate.
        /// </summary>
        public REAL XforY(REAL y)
        {
            if (DMath.IsZero(A)) return 0;
            return (-C - B * y) / A;
        }

        /// <summary>
        /// Calculates the Y coordinate of a point on the line by its X coordinate.
        /// </summary>
        public REAL YforX(REAL x)
        {
            if (DMath.IsZero(B)) return 0;
            return (-C - A * x) / B;
        }

        /// <summary>
        /// Determines whether the point lies on the line.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>if the point lies on the line</returns>
        public bool PointOnLine(VECTOR2 p)
        {
            return Math.Abs(A * p.x + B * p.y + C) < DMath.EPS;
        }

        /// <summary>
        /// Calculates the perpendicular line that
        /// passes through the given point.
        /// </summary>
        public Line2d PerpendicularLine(VECTOR2 p)
        {
            return new Line2d(B, -A, -B * p.x + A * p.y);
        }

        /// <summary>
        /// Determines whether the point lies
        /// on the left side of the line.
        /// </summary>
        public bool IsLeftPoint(VECTOR2 p)
        {
            return p.x < XforY(p.y);
        }

        /// <summary>
        /// Determines whether the point lies
        /// on the right side of the line.
        /// </summary>
        public bool IsRightPoint(VECTOR2 p)
        {
            return p.x > XforY(p.y);
        }

        /// <summary>
        /// Determine if the two lines are parallel.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsParallel(Line2d line)
        {
            if (!DMath.AlmostEqual(C, line.C)) return false;

            REAL slope = Slope;
            if (DMath.IsZero(slope))
                return IsVertical && line.IsVertical;
            else
                return DMath.AlmostEqual(slope, line.Slope);
        }

        /// <summary>
        /// Calculates the intersection of two lines.
        /// </summary>
        /// <param name="line">the other line</param>
        /// <param name="p">intersection point</param>
        /// <returns>if lines intersect</returns>
        public bool Intersects(Line2d line, out VECTOR2 p)
        {

            if (!DMath.IsZero(B))
            {
                REAL f = line.A - line.B * A / B;
                if (DMath.IsZero(f))
                {
                    p = VECTOR2.Zero;
                    return false;
                }
                else
                {
                    REAL x = (-line.C + line.B * C / B) / f;
                    REAL y = (-C - A * x) / B;

                    p = new VECTOR2(x, y);
                    return true;
                }
            }
            else
            {
                if (DMath.IsZero(A))
                {
                    p = VECTOR2.Zero;
                    return false;
                }
                else
                {
                    REAL f = line.B - line.A * B / A;
                    if (DMath.IsZero(f))
                    {
                        p = VECTOR2.Zero;
                        return false;
                    }
                    else
                    {
                        REAL y = (-line.C + line.A * C / A) / f;
                        REAL x = (-C - B * y) / A;

                        p = new VECTOR2(x, y);
                        return true;
                    }
                }
            }
        }


    }

}
