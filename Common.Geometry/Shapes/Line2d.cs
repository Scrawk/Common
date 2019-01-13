using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

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

        public double A, B, C;

        public Line2d(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Line2d(Vector2d p1, Vector2d p2)
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
        public double Angle
        {
            get
            {
                if (IsVertical) return Math.PI / 2.0;

                double atan = Math.Atan(-A / B);
                if (atan < 0) atan += Math.PI;

                return atan;
            }
        }

        public static implicit operator Line2d(Line2f line)
        {
            return new Line2d(line.A, line.B, line.C);
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
        public double XforY(double y)
        {
            return (-C - B * y) / A;
        }

        /// <summary>
        /// Calculates the Y coordinate of a point on the line by its X coordinate.
        /// </summary>
        public double YforX(double x)
        {
            return (-C - A * x) / B;
        }

        /// <summary>
        /// Determines whether the point lies on the line.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>if the point lies on the line</returns>
        public bool PointOnLine(Vector2d p)
        {
            return A * p.x + B * p.y + C == 0;
        }

        /// <summary>
        /// Calculates the perpendicular line that
        /// passes through the given point.
        /// </summary>
        public Line2d PerpendicularLine(Vector2d p)
        {
            return new Line2d(B, -A, -B * p.x + A * p.y);
        }

        /// <summary>
        /// Determines whether the point lies
        /// on the left side of the line.
        /// </summary>
        public bool IsLeftPoint(Vector2d p)
        {
            return p.x < XforY(p.y);
        }

        /// <summary>
        /// Determines whether the point lies
        /// on the right side of the line.
        /// </summary>
        public bool IsRightPoint(Vector2d p)
        {
            return p.x > XforY(p.y);
        }

        /// <summary>
        /// Calculates the intersection of two lines.
        /// </summary>
        /// <param name="line">the other line</param>
        /// <param name="p">intersection point</param>
        /// <returns>if lines intersect</returns>
        public bool Intersects(Line2d line, out Vector2d p)
        {

            if (B != 0)
            {
                double f = line.A - line.B * A / B;
                if (f == 0)
                {
                    p = Vector2d.Zero;
                    return false;
                }
                else
                {
                    double x = (-line.C + line.B * C / B) / f;
                    double y = (-C - A * x) / B;

                    p = new Vector2d(x, y);
                    return true;
                }
            }
            else
            {
                if (A == 0)
                {
                    p = Vector2d.Zero;
                    return false;
                }
                else
                {
                    double f = line.B - line.A * B / A;
                    if (f == 0)
                    {
                        p = Vector2d.Zero;
                        return false;
                    }
                    else
                    {
                        double y = (-line.C + line.A * C / A) / f;
                        double x = (-C - B * y) / A;

                        p = new Vector2d(x, y);
                        return true;
                    }
                }
            }
        }


    }

}
