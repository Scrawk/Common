using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// 
    /// </summary>
    public class Polyline3d
    {

        public Polyline3d(int count)
        {
            SetPositions(count);
        }

        public Polyline3d(IList<Vector3d> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public Vector3d[] Positions { get; private set; }

        public double[] Params { get; private set; }

        public int[] Indices { get; private set; }

        public double Length { get; private set; }

        public Box3d Bounds { get; private set; }


        public override string ToString()
        {
            return string.Format("[Polyline3d: Count={0}, Length={1}]", Count, Length);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new Vector3d[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="positions">Array to copy from.</param>
        public void SetPositions(IList<Vector3d> positions)
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
        public Polyline3d Copy()
        {
            var copy = new Polyline3d(Positions);
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
                Length += Vector3d.Distance(Positions[i], Positions[i + 1]);
        }

        public void CalculateBounds()
        {
            Bounds = new Box3d();
            if (Count == 0) return;

            var min = Vector3d.PositiveInfinity;
            var max = Vector3d.NegativeInfinity;

            for (int i = 0; i < Count; i++)
            {
                var p = Positions[i];

                if (p.x < min.x) min.x = p.x;
                if (p.x > max.x) max.x = p.x;
                if (p.y < min.y) min.y = p.y;
                if (p.y > max.y) max.y = p.y;
                if (p.z < min.z) min.z = p.z;
                if (p.z > max.z) max.z = p.z;
            }

            Bounds = new Box3d(min, max);
        }

    }
}




