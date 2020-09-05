﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;
using BOX2 = Common.Geometry.Shapes.Box2d;
using SEGMENT2 = Common.Geometry.Shapes.Segment2d;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// 
    /// </summary>
    public class Polyline2d : Polyobject2d
    {

        public Polyline2d(REAL width, int count) : base(count)
        {
            Width = width;
        }

        public Polyline2d(REAL width, IList<VECTOR2> positions) : base(positions)
        {
            Width = width;
        }

        public REAL Width { get; set; }

        public REAL Radius => Width * 0.5;

        public override string ToString()
        {
            return string.Format("[Polyline2d: Count={0}, Length={1}]", Count, Length);
        }

        /// <summary>
        /// Get the position with a clamped index.
        /// </summary>
        public VECTOR2 GetPosition(int i)
        {
            return Positions.GetClamped(i);
        }

        /// <summary>
        /// Get the param with a clamped index.
        /// </summary>
        public REAL GetParam(int i)
        {
            if (Params == null)
                throw new InvalidOperationException("Polyline does not have any params.");

            return Params.GetClamped(i);
        }

        /// <summary>
        /// Get the length with a clamped index.
        /// </summary>
        public REAL GetLength(int i)
        {
            if (Lengths == null)
                throw new InvalidOperationException("Polyline does not have any lengths.");

            return Lengths.GetClamped(i);
        }

        /// <summary>
        /// Create the index array.
        /// </summary>
        public override void CreateIndices()
        {
            CreateIndices((Count - 1) * 2);
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
            if (Lengths != null) Array.Reverse(Lengths);
        }

        /// <summary>
        /// Copy the polyline.
        /// No need to recalculate the copy.
        /// </summary>
        public Polyline2d Copy()
        {
            var copy = new Polyline2d(Width, Positions);
            copy.Length = Length;
            copy.Bounds = Bounds;

            if (Params != null)
                copy.SetParams(Params);

            if (Lengths != null)
                copy.SetLengths(Lengths);

            return copy;
        }

        /// <summary>
        /// Update the polylines properties.
        /// Should be called when polyline
        /// created or changes. 
        /// </summary>
        public void Calculate()
        {
            CalculateLengths();
            CalculateBounds();
        }

        /// <summary>
        /// Calculate the total length of the line
        /// and the length of each segment in line.
        /// Lengths represents the length of line 
        /// up to that point.
        /// </summary>
        public override void CalculateLengths()
        {
            Length = 0;

            int size = Count;
            if (size == 0) return;

            CreateLengths(size);
            Lengths[0] = 0;

            for (int i = 1; i < size; i++)
            {
                var p0 = GetPosition(i - 1);
                var p1 = GetPosition(i);

                Lengths[i] = Length + VECTOR2.Distance(p0, p1);
                Length = Lengths[i];
            }
        }

        /// <summary>
        /// Does the line contain the point.
        /// The line has some thickness from its width.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override bool Contains(VECTOR2 point)
        {
            if (Count == 0) return false;
            if (!Bounds.Contains(point)) return false;

            REAL radius = Width * 0.5;

            for (int i = 0; i < Count - 1; i++)
            {
                var a = Positions[i];
                var b = Positions[i + 1];
                var seg = new SEGMENT2(a, b);
                var sdf = seg.SignedDistance(point) - radius;

                if (sdf < 0) return true;
            }

            return false;
        }

        /// <summary>
        /// The signed distance from the line to the point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override REAL SignedDistance(VECTOR2 point)
        {
            if (Count == 0)
                return REAL.PositiveInfinity;

            REAL sdf = REAL.PositiveInfinity;
            REAL radius = Width * 0.5;

            for (int i = 0; i < Count - 1; i++)
            {
                var a = Positions[i];
                var b = Positions[i + 1];
                var seg = new SEGMENT2(a, b);

                sdf = Math.Min(sdf, seg.SignedDistance(point));
            }

            return sdf - radius;
        }

    }
}



