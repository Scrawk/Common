using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{

    /// <summary>
    /// A naive implementation of a point collection
    /// where all operations are O(n2).
    /// </summary>
    public class PointCollection2f<T> : IPointCollection2f<T>
        where T : IPoint2f
    {

        private List<T> m_points;

        public PointCollection2f()
        {
            m_points = new List<T>();
        }

        public PointCollection2f(int size)
        {
            m_points = new List<T>(size);
        }

        public PointCollection2f(IEnumerable<T> points)
        {
            m_points = new List<T>(points);
        }

        /// <summary>
        /// The number of points in the collection.
        /// </summary>
        public int Count
        {
            get { return m_points.Count; }
        }

        /// <summary>
        /// The capacity of the collection.
        /// </summary>
        public int Capacity
        {
            get { return m_points.Capacity; }
            set { m_points.Capacity = value; }
        }

        /// <summary>
        /// Get or Set the point at index i.
        /// </summary>
        public T this[int i]
        {
            get { return m_points[i]; }
            set { m_points[i] = value; }
        }

        public override string ToString()
        {
            return string.Format("[PointCollection2f: Count={0}]", Count);
        }

        /// <summary>
        /// Clear the collection.
        /// </summary>
        public void Clear()
        {
            m_points.Clear();
        }

        /// <summary>
        /// Add all the points in the enumerable to the collection.
        /// </summary>
        public bool Add(IEnumerable<T> points)
        {
            m_points.AddRange(points);
            return true;
        }

        /// <summary>
        /// Add a point to the collection.
        /// </summary>
        public bool Add(T point)
        {
            m_points.Add(point);
            return true;
        }

        /// <summary>
        /// Remove point from collection.
        /// </summary>
        public bool Remove(T point)
        {
           return m_points.Remove(point);
        }

        /// <summary>
        /// Fill the points list with all points in the 
        /// collection contained within the region.
        /// </summary>
        public void Search(Circle2f region, List<T> points)
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                if (PointOps2f<T>.Contains(region, m_points[i]))
                    points.Add(m_points[i]);
            }
        }

        /// <summary>
        /// Return the closest point in collect to this point.
        /// </summary>
        public T Closest(T point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find nearest point if collection is empty.");

            T nearest = default(T);
            var dist2 = float.PositiveInfinity;

            int count = Count;
            for (int i = 0; i < count; i++)
            {
                float d2 = PointOps2f<T>.SqrDistance(m_points[i], point);
                if(d2 < dist2)
                {
                    nearest = m_points[i];
                    dist2 = d2;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Create a list from all points in the collection.
        /// </summary>
        public List<T> ToList()
        {
            return new List<T>(m_points);
        }

        /// <summary>
        /// Enumerate all points in the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return m_points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
