using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{

    /// <summary>
    /// A naive implementation of a shape collection
    /// where all operations are O(n2).
    /// </summary>
    public class ShapeCollection3f : IShapeCollection3f
    {

        private List<IShape3f> m_shapes;

        public ShapeCollection3f()
        {
            m_shapes = new List<IShape3f>();
        }

        public ShapeCollection3f(int size)
        {
            m_shapes = new List<IShape3f>(size);
        }

        public ShapeCollection3f(IEnumerable<IShape3f> shapes)
        {
            m_shapes = new List<IShape3f>(shapes);
        }

        /// <summary>
        /// The number of shapes in the collection.
        /// </summary>
        public int Count
        {
            get { return m_shapes.Count; }
        }

        /// <summary>
        /// The capacity of the shape collection.
        /// </summary>
        public int Capacity
        {
            get { return m_shapes.Capacity; }
            set { m_shapes.Capacity = value; }
        }

        /// <summary>
        /// Get or Set the shape at index i.
        /// </summary>
        public IShape3f this[int i]
        {
            get { return m_shapes[i]; }
            set { m_shapes[i] = value; }
        }

        public override string ToString()
        {
            return string.Format("[ShapeCollection3f: Count={0}]", Count);
        }

        /// <summary>
        /// Clear the collection.
        /// </summary>
        public void Clear()
        {
            m_shapes.Clear();
        }

        /// <summary>
        /// Add all the shapes in the enumerable to the collection.
        /// </summary>
        public void Add(IEnumerable<IShape3f> shapes)
        {
            m_shapes.AddRange(shapes);
        }

        /// <summary>
        /// Add a shape to the collection.
        /// </summary>
        public void Add(IShape3f shape)
        {
            m_shapes.Add(shape);
        }

        /// <summary>
        /// Remove a shape from the collection.
        /// </summary>
        public bool Remove(IShape3f shape)
        {
            return m_shapes.Remove(shape);
        }

        /// <summary>
        /// Return the signed distance field from 
        /// the union of all shapes in the collection.
        /// </summary>
        public float SignedDistance(Vector3f point)
        {
            var dist = float.PositiveInfinity;

            int count = Count;
            for (int i = 0; i < count; i++)
            {
                var shape = m_shapes[i];
                var d = shape.SignedDistance(point);

                if (d < dist)
                    dist = d;
            }

            return dist;
        }

        /// <summary>
        /// Does the collection have a shape that contains the point.
        /// </summary>
        public bool Contains(Vector3f point)
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                var shape = m_shapes[i];
                if (shape.Contains(point)) return true;
            }

            return false;
        }

        /// <summary>
        /// Create a list of all the shapes in the collection.
        /// </summary>
        public List<IShape3f> ToList()
        {
            return new List<IShape3f>(m_shapes);
        }

        /// <summary>
        /// Enumerate all shapes in the collection.
        /// </summary>
        public IEnumerator<IShape3f> GetEnumerator()
        {
            return m_shapes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
