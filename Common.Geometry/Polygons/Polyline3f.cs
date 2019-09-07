using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Polygons
{
    /// <summary>
    /// 
    /// </summary>
    public class Polyline3f
    {

        public Polyline3f(int count)
        {
            SetPositions(count);
        }

        public Polyline3f(IList<Vector3f> positions)
        {
            SetPositions(positions);
        }

        public int Count => Positions.Length;

        public Vector3f[] Positions { get; private set; }

        public float[] Params { get; private set; }

        public int[] Indices { get; private set; }

        public float Length { get; private set; }

        public Box3f Bounds { get; private set; }


        public override string ToString()
        {
            return string.Format("[Polyline3f: Count={0}, Length={1}]", Count, Length);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new Vector3f[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="positions">Array to copy from.</param>
        public void SetPositions(IList<Vector3f> positions)
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
        public Polyline3f Copy()
        {
            var copy = new Polyline3f(Positions);
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
                Length += Vector3f.Distance(Positions[i], Positions[i + 1]);
        }

        public void CalculateBounds()
        {
            Bounds = new Box3f();
            if (Count == 0) return;

            var min = Vector3f.PositiveInfinity;
            var max = Vector3f.NegativeInfinity;

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

            Bounds = new Box3f(min, max);
        }

        /// <summary>
        /// Translate the positions.
        /// </summary>
        public void Translate(Vector3f translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        /// <summary>
        /// Rotate allpositions.
        /// </summary>
        public void Rotate(Vector3f rotate)
        {
            var q = Quaternion3f.FromEuler(rotate);
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= q;
        }

        /// <summary>
        /// Scale the positions.
        /// </summary>
        public void Scale(Vector3f scale)
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
                Positions[i] = (m * Positions[i].xyz1).xyz;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(Matrix3x3f m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }

    }
}




