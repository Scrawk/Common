using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    /// <summary>
    /// A 3f KdTree using T as the point.
    /// Does not support points with the same coordinates.
    /// </summary>
    public class KdTree3f<T> : IStaticPointCollection3f<T>
        where T : IPoint3f
    {

        private Box3f m_bounds;

        public KdTree3f()
        {

        }

        public KdTree3f(IEnumerable<T> points)
        {
            Build(points);
        }

        /// <summary>
        /// The number of points in the tree.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The max depth of the tree.
        /// </summary>
        public int Depth
        {
            get { return MaxDepth(Root); }
        }

        /// <summary>
        /// The bounding box of the trees points.
        /// </summary>
        public Box3f Bounds { get { return m_bounds; } }

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        public KdTreeNode3f<T> Root { get; private set; }

        public override string ToString()
        {
            return string.Format("[KdTree3f: Count={0}, Depth={1}, Bounds={2}]", Count, Depth, Bounds);
        }

        /// <summary>
        /// Clear the tree.
        /// </summary>
        public void Clear()
        {
            Root = null;
            Count = 0;
            m_bounds = new Box3f();
        }

        /// <summary>
        /// Build the tree from a set of points.
        /// </summary>
        public void Build(IEnumerable<T> points)
        {
            var list = new List<T>(points);

            int count = list.Count;
            if (count == 0)
            {
                Clear();
            }
            else if (count == 1)
            {
                var p = list[0];
                m_bounds = new Box3f(p.x, p.x, p.x, p.y, p.y, p.y);
                Root = new KdTreeNode3f<T>(p, 0);
                Count = 1;
            }
            else
            {
                var p = list[0];
                m_bounds = new Box3f(p.x, p.x, p.x, p.y, p.y, p.y);
                Root = Build(list, 0);
            }
        }

        /// <summary>
        /// Return all points contained in the search region.
        /// </summary>
        public void Search(Sphere3f region, List<T> points)
        {
            if (!region.Intersects(Bounds)) return;
            Search(Root, region, Bounds, points);
        }

        /// <summary>
        /// Find the nearest point to input point.
        /// </summary>
        public T Closest(T point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find closest point if collection is empty.");

            T closest = default(T);
            float dist = float.PositiveInfinity;

            Closest(Root, point, ref closest, ref dist);
            return closest;
        }

        /// <summary>
        /// Create a indexed list of the segments that make up the 
        /// bounds of the lines of the tree.
        /// </summary>
        /// <param name="addBounds">should the bounding box be added</param>
        public void CalculateSegments(List<Segment3f> segments, bool addBounds = true)
        {
            if (addBounds)
            {
                var CUBE_INDICES = new int[]
                {
                    0, 1, 1, 2, 2, 3, 3, 0,
                    4, 5, 5, 6, 6, 7, 7, 4,
                    0, 4, 1, 5, 2, 6, 3, 7
                };

                var corners = new Vector3f[8];
                m_bounds.GetCorners(corners);

                for (int i = 0; i < CUBE_INDICES.Length / 2; i++)
                {
                    var a = corners[CUBE_INDICES[i * 2 + 0]];
                    var b = corners[CUBE_INDICES[i * 2 + 1]];
                    segments.Add(new Segment3f(a, b));
                }
            }

            CalculateSegments(null, Root, Bounds, segments);
        }

        public void CalculateBoxes(List<Box3f> boxes)
        {
            CalculateBoxes(Root, Bounds, boxes);
        }

        /// <summary>
        /// Copy the tree into a list.
        /// </summary>
        public List<T> ToList()
        {
            var list = new List<T>(Count);
            CopyTo(Root, list);
            return list;
        }

        /// <summary>
        /// Gets an enumerator for the tree.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (Root != null)
            {
                foreach (var n in Root)
                    yield return n.Point;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Iteratively builds the tree from a set of points.
        /// </summary>
        private KdTreeNode3f<T> Build(List<T> points, int depth)
        {
            int count = points.Count;
            if (count == 0)
                return null;

            if (count == 1)
            {
                var p = points[0];
                Count++;
                m_bounds = Box3f.Enlarge(m_bounds, new Vector3f(p.x, p.y, p.z));
                return new KdTreeNode3f<T>(p, depth);
            }
            else
            {
                int median = count / 2;
                List<T> left, right;
                T p;

                if (depth % 3 == 0)
                {
                    points.Sort(m_compareX);
                    p = points[median];
                }
                else if (depth % 3 == 1)
                {
                    points.Sort(m_compareY);
                    p = points[median];
                }
                else
                {
                    points.Sort(m_compareZ);
                    p = points[median];
                }

                left = points.GetRange(0, median);
                right = points.GetRange(median + 1, count - median - 1);

                Count++;
                m_bounds = Box3f.Enlarge(m_bounds, new Vector3f(p.x, p.y, p.z));
                var node = new KdTreeNode3f<T>(p, depth);

                node.Left = Build(left, depth + 1);
                node.Right = Build(right, depth + 1);

                return node;
            }
        }

        /// <summary>
        /// Iteratively searchs the tree for points contained
        /// in the search region.
        /// </summary>
        private void Search(KdTreeNode3f<T> node, Sphere3f region, Box3f bounds, List<T> points)
        {
            if (node == null) return;

            if (PointOps3f<T>.Contains(region, node.Point))
                points.Add(node.Point);

            var left = bounds;
            var right = bounds;

            if (node.Depth % 3 == 0)
            {
                left.Max.x = node.Point.x;
                right.Min.x = node.Point.x;
            }
            else if (node.Depth % 3 == 1)
            {
                left.Max.y = node.Point.y;
                right.Min.y = node.Point.y;
            }
            else
            {
                left.Max.z = node.Point.z;
                right.Min.z = node.Point.z;
            }

            if (region.Contains(left))
                CopyTo(node.Left, points);
            else if (region.Intersects(left))
                Search(node.Left, region, left, points);

            if (region.Contains(right))
                CopyTo(node.Right, points);
            else if (region.Intersects(right))
                Search(node.Right, region, right, points);
        }

        /// <summary>
        /// Iteratively searchs the tree for the nearest point to input.
        /// </summary>
        private void Closest(KdTreeNode3f<T> node, T point, ref T closest, ref float dist)
        {
            if (node == null) return;

            float d = PointOps3f<T>.Distance(node.Point, point);
            if (d < dist)
            {
                dist = d;
                closest = node.Point;
            }

            if (node.Depth % 3 == 0)
            {
                if (point.x - dist <= node.Point.x)
                    Closest(node.Left, point, ref closest, ref dist);

                if (point.x + dist >= node.Point.x)
                    Closest(node.Right, point, ref closest, ref dist);
            }
            else if (node.Depth % 3 == 1)
            {
                if (point.y - dist <= node.Point.y)
                    Closest(node.Left, point, ref closest, ref dist);

                if (point.y + dist >= node.Point.y)
                    Closest(node.Right, point, ref closest, ref dist);
            }
            else
            {
                if (point.z - dist <= node.Point.z)
                    Closest(node.Left, point, ref closest, ref dist);

                if (point.z + dist >= node.Point.z)
                    Closest(node.Right, point, ref closest, ref dist);
            }
        }

        /// <summary>
        /// Iteratively adds the points into the list.
        /// </summary>
        private void CopyTo(KdTreeNode3f<T> node, List<T> points)
        {
            if (node == null) return;
            foreach (var n in node)
                points.Add(n.Point);
        }

        /// <summary>
        /// Iteratively calculates the segements made from 
        /// the intersecting lines.
        /// </summary>
        private void CalculateSegments(KdTreeNode3f<T> parent, KdTreeNode3f<T> node, Box3f bounds, List<Segment3f> segments)
        {
            if (node == null) return;

            Vector3f p0, p1;
            if (node.Depth % 3 == 0)
            {
                p0 = new Vector3f(node.Point.x, bounds.Min.y, bounds.Min.z);
                p1 = new Vector3f(node.Point.x, bounds.Max.y, bounds.Max.z);

                if (parent != null)
                {
                    if (node == parent.Left)
                    {
                        p1.y = parent.Point.y;
                        p1.z = parent.Point.z;
                        bounds.Max.y = p1.y;
                        bounds.Max.z = p1.z;
                    }
                    else
                    {
                        p0.y = parent.Point.y;
                        p0.z = parent.Point.z;
                        bounds.Min.y = p0.y;
                        bounds.Min.z = p0.z;
                    }
                }
            }
            else if (node.Depth % 3 == 1)
            {
                p0 = new Vector3f(bounds.Min.x, node.Point.y, bounds.Min.z);
                p1 = new Vector3f(bounds.Max.x, node.Point.y, bounds.Max.z);

                if (parent != null)
                {
                    if (node == parent.Left)
                    {
                        p1.x = parent.Point.x;
                        p1.z = parent.Point.z;
                        bounds.Max.x = p1.x;
                        bounds.Max.z = p1.z;
                    }
                    else
                    {
                        p0.x = parent.Point.x;
                        p0.z = parent.Point.z;
                        bounds.Min.x = p0.x;
                        bounds.Min.z = p0.z;
                    }
                }
            }
            else
            {
                p0 = new Vector3f(bounds.Min.x, bounds.Min.y, node.Point.z);
                p1 = new Vector3f(bounds.Max.x, bounds.Max.y, node.Point.z);

                if (parent != null)
                {
                    if (node == parent.Left)
                    {
                        p1.x = parent.Point.x;
                        p1.y = parent.Point.y;
                        bounds.Max.x = p1.x;
                        bounds.Max.y = p1.y;
                    }
                    else
                    {
                        p0.x = parent.Point.x;
                        p0.y = parent.Point.y;
                        bounds.Min.x = p0.x;
                        bounds.Min.y = p0.y;
                    }
                }
            }

            segments.Add(new Segment3f(p0, p1));

            CalculateSegments(node, node.Left, bounds, segments);
            CalculateSegments(node, node.Right, bounds, segments);
        }

        private void CalculateBoxes(KdTreeNode3f<T> node, Box3f bounds, List<Box3f> boxes)
        {
            if (node == null) return;

            var left = bounds;
            var right = bounds;

            if (node.Depth % 3 == 0)
            {
                left.Max.x = node.Point.x;
                right.Min.x = node.Point.x;
            }
            else if (node.Depth % 3 == 1)
            {
                left.Max.y = node.Point.y;
                right.Min.y = node.Point.y;
            }
            else
            {
                left.Max.z = node.Point.z;
                right.Min.z = node.Point.z;
            }

            boxes.Add(left, right);

            CalculateBoxes(node.Left, left, boxes);
            CalculateBoxes(node.Right, right, boxes);
        }

        /// <summary>
        /// Find the max depth of tree.
        /// </summary>
        public int MaxDepth(KdTreeNode3f<T> node)
        {
            if (node == null) return 0;
            if (node.IsLeaf) return node.Depth;
            return Math.Max(MaxDepth(node.Left), MaxDepth(node.Right));
        }

        /// <summary>
        /// Comparer points on the x axis.
        /// </summary>
        private static readonly CompareX m_compareX = new CompareX();

        /// <summary>
        /// Comparer points on the y axis.
        /// </summary>
        private static readonly CompareY m_compareY = new CompareY();

        /// <summary>
        /// Comparer points on the y axis.
        /// </summary>
        private static readonly CompareZ m_compareZ = new CompareZ();

        private class CompareX : IComparer<T>
        {
            public int Compare(T v0, T v1)
            {
                return v0.x.CompareTo(v1.x);
            }
        }

        private class CompareY : IComparer<T>
        {
            public int Compare(T v0, T v1)
            {
                return v0.y.CompareTo(v1.y);
            }
        }

        private class CompareZ : IComparer<T>
        {
            public int Compare(T v0, T v1)
            {
                return v0.z.CompareTo(v1.z);
            }
        }

    }
}
