using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;
using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{

    /// <summary>
    ///  Represents a line by means of three coefficients
    ///  a, b and c, where ax + by + c = 0 holds.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Line2f : IEquatable<Line2f>
    {

        public float A, B, C;

        public Line2f(float a, float b, float c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Line2f(Vector2f p1, Vector2f p2)
        {
            A = p1.y - p2.y;
            B = p2.x - p1.x;
            C = p1.x * p2.y - p2.x * p1.y;
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
                return (B == 0 || (-A / B) >= 0);
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
                return B == 0 && A != 0;
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
                return (B == 0 || (-A / B) < 0);
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
                return (A == 0 && B != 0);
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
                return (A == 0 && B == 0 && C == 0);
            }
        }

        /// <summary>
        /// Calculates the angle that the line makes
        /// with the positive direction of the X axis.
        /// </summary>
        /// <returns></returns>
        public float Angle
        {
            get
            {
                if (IsVertical) return (float)(Math.PI / 2.0);

                double atan = Math.Atan(-A / B);
                if (atan < 0) atan += Math.PI;

                return (float)atan;
            }
        }

        public static explicit operator Line2f(Line2d line)
        {
            return new Line2f((float)line.A, (float)line.B, (float)line.C);
        }

        public static bool operator ==(Line2f i1, Line2f i2)
        {
            return i1.A == i2.A && i1.B == i2.B && i1.C == i2.C;
        }

        public static bool operator !=(Line2f i1, Line2f i2)
        {
            return i1.A != i2.A || i1.B != i2.B || i1.C != i2.C;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Line2f)) return false;
            Line2f line = (Line2f)obj;
            return this == line;
        }

        public bool Equals(Line2f line)
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
            return string.Format("[Line2f: A={0}, B={1}, C={2}]", A, B, C);
        }

        /// <summary>
        /// Calculates the X coordinate of a point on the line by its Y coordinate.
        /// </summary>
        public float XforY(float y)
        {
            return (-C - B * y) / A;
        }

        /// <summary>
        /// Calculates the Y coordinate of a point on the line by its X coordinate.
        /// </summary>
        public float YforX(float x)
        {
            return (-C - A * x) / B;
        }

        /// <summary>
        /// Determines whether the point lies on the line.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>if the point lies on the line</returns>
        public bool PointOnLine(Vector2f p)
        {
            return A * p.x + B * p.y + C == 0;
        }

        /// <summary>
        /// Calculates the perpendicular line that
        /// passes through the given point.
        /// </summary>
        public Line2f PerpendicularLine(Vector2f p)
        {
            return new Line2f(B, -A, -B * p.x + A * p.y);
        }

        /// <summary>
        /// Determines whether the point lies
        /// on the left side of the line.
        /// </summary>
        public bool IsLeftPoint(Vector2f p)
        {
            return p.x < XforY(p.y);
        }

        /// <summary>
        /// Determines whether the point lies
        /// on the right side of the line.
        /// </summary>
        public bool IsRightPoint(Vector2f p)
        {
            return p.x > XforY(p.y);
        }

        /// <summary>
        /// Calculates the intersection of two lines.
        /// </summary>
        /// <param name="line">the other line</param>
        /// <param name="p">intersection point</param>
        /// <returns>if lines intersect</returns>
        public bool Intersects(Line2f line, out Vector2f p)
        {
            
            if (B != 0)
            {
                float f = line.A - line.B * A / B;
                if (f == 0)
                {
                    p = Vector2f.Zero;
                    return false;
                }
                else
                {
                    float x = (-line.C + line.B * C / B) / f;
                    float y = (-C - A * x) / B;

                    p = new Vector2f(x, y);
                    return true;
                }
            }
            else
            {
                if (A == 0)
                {
                    p = Vector2f.Zero;
                    return false;
                }
                else
                {
                    float f = line.B - line.A * B / A;
                    if (f == 0)
                    {
                        p = Vector2f.Zero;
                        return false;
                    }
                    else
                    {
                        float y = (-line.C + line.A * C / A) / f;
                        float x = (-C - B * y) / A;

                        p = new Vector2f(x, y);
                        return true;
                    }
                }
            }
        }


    }

}
