﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Geometry.Shapes
{

    /// <summary>
    /// Represents a halfplane by means of the boundary line
    /// ax + by = c, where one accepts that
    /// for the halfplane ax + by + c <= 0 holds.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct HalfPlane2d : IEquatable<HalfPlane2d>
    {

        public Line2d Line;

        public bool RightSide;

        /// <summary>
        /// Initializes a halfplane by its boundary line
        /// and by the side where the halfplane lies.
        /// </summary>
        /// <param name="line">The boundary line</param>
        /// <param name="rightSide">rightSide true, if the 
        /// halfplanes lies on the right side of the line, 
        /// false otherwise</param>
        public HalfPlane2d(Line2d line, bool rightSide)
        {
            Line = line;
            RightSide = rightSide;
        }

        /// <summary>
        /// Determines whether the boundary line
        /// lies on the left side of the halfplane.
        /// </summary>
        /// <return>if the boundary line is on the left, flase otherwise</return>
        public bool IsLeftBoundary
        {
            get
            {
                return RightSide;
            }
        }

        /// <summary>
        /// Determines whether the boundary line
        /// lies on the right side of the halfplane.
        /// </summary>
        /// <return>if the boundary line is on the right, flase otherwise</return>
        public bool IsRightBoundary
        {
            get
            {
                return !RightSide;
            }
        }

        public static implicit operator HalfPlane2d(HalfPlane2f hp)
        {
            return new HalfPlane2d(hp.Line, hp.RightSide);
        }

        public static bool operator ==(HalfPlane2d h1, HalfPlane2d h2)
        {
            return h1.RightSide == h2.RightSide && h1.Line == h2.Line;
        }

        public static bool operator !=(HalfPlane2d h1, HalfPlane2d h2)
        {
            return h1.RightSide != h2.RightSide || h1.Line != h2.Line;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is HalfPlane2d)) return false;
            HalfPlane2d line = (HalfPlane2d)obj;
            return this == line;
        }

        public bool Equals(HalfPlane2d line)
        {
            return this == line;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ RightSide.GetHashCode();
                hash = (hash * 16777619) ^ Line.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[HalfSpace2d: Line={0}, RightSide={1}]", Line, RightSide);
        }

        /// <summary>
        /// Determines whether the point lies in the halfplane.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>if the point lies in the left halfplane, false otherwise</returns>
        public bool Contains(Vector2d p)
        {
            if (IsLeftBoundary)
            {
                if (Line.IsHorizontal)
                    return p.y < Line.YforX(0);
                else
                    return p.x > Line.XforY(p.y);
            }
            else
            {
                if (Line.IsHorizontal)
                    return p.y > Line.YforX(0);
                else
                    return p.x < Line.XforY(p.y);
            }
        }


    }

}
