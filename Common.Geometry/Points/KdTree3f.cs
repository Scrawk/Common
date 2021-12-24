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
    public class KdTree3f: IStaticPointCollection3f
    {

        private Box3f m_bounds;

        public KdTree3f()
        {

        }

        public KdTree3f(IEnumerable<Point3f> points)
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
        public KdTreeNode3f Root { get; private set; }

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
        public void Build(IEnumerable<Point3f> points)
        {
            var list = new List<Point3f>(points);

            int count = list.Count;
            if (count == 0)
            {
                Clear();
            }
            else if (count == 1)
            {
                var p = list[0];
                m_bounds = new Box3f(p.x, p.x, p.y, p.y, p.z, p.z);
                Root = new KdTreeNode3f(p, 0);
                Count = 1;
            }
            else
            {
                var p = list[0];
                m_bounds = new Box3f(p.x, p.x, p.y, p.y, p.z, p.z);
                Root = Build(list, 0);
            }
        }

        /// <summary>
        /// Return all points contained in the search region.
        /// </summary>
        public void Search(Sphere3f region, List<Point3f> points)
        {
            if (!region.Intersects(Bounds)) return;
            Search(Root, region, Bounds, points);
        }

        /// <summary>
        /// Find the nearest point to input point.
        /// </summary>
        public Point3f Closest(Point3f point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find closest point if collection is empty.");

            Point3f closest = new Point3f();
            float dist = float.PositiveInfinity;

            Closest(Root, point, ref closest, ref dist);
            return closest;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boxes"></param>
        public void CalculateBoxes(List<Box3f> boxes)
        {
            CalculateBoxes(Root, Bounds, boxes);
        }

        /// <summary>
        /// Copy the tree into a list.
        /// </summary>
        public List<Point3f> ToList()
        {
            var list = new List<Point3f>(Count);
            CopyTo(Root, list);
            return list;
        }

        /// <summary>
        /// Gets an enumerator for the tree.
        /// </summary>
        public IEnumerator<Point3f> GetEnumerator()
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
        private KdTreeNode3f Build(List<Point3f> points, int depth)
        {
            int count = points.Count;
            if (count == 0)
                return null;

            if (count == 1)
            {
                var p = points[0];
                Count++;
                m_bounds = Box3f.Enlarge(m_bounds, new Point3f(p.x, p.y, p.z));
                return new KdTreeNode3f(p, depth);
            }
            else
            {
                int median = count / 2;
                List<Point3f> left, right;
                Point3f p;

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
                m_bounds = Box3f.Enlarge(m_bounds, new Point3f(p.x, p.y, p.z));
                var node = new KdTreeNode3f(p, depth);

                node.Left = Build(left, depth + 1);
                node.Right = Build(right, depth + 1);

                return node;
            }
        }

        /// <summary>
        /// Iteratively searchs the tree for points contained
        /// in the search region.
        /// </summary>
        private void Search(KdTreeNode3f node, Sphere3f region, Box3f bounds, List<Point3f> points)
        {
            if (node == null) return;

            if (region.Contains(node.Point))
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
        private void Closest(KdTreeNode3f node, Point3f point, ref Point3f closest, ref float dist)
        {
            if (node == null) return;

            float d = Point3f.Distance(node.Point, point);
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
        private void CopyTo(KdTreeNode3f node, List<Point3f> points)
        {
            if (node == null) return;
            foreach (var n in node)
                points.Add(n.Point);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalculateBoxes(KdTreeNode3f node, Box3f bounds, List<Box3f> boxes)
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
        public int MaxDepth(KdTreeNode3f node)
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

        private class CompareX : IComparer<Point3f>
        {
            public int Compare(Point3f v0, Point3f v1)
            {
                if (v0.x != v1.x)
                    return v0.x < v1.x ? -1 : 1;
                else if (v0.y != v1.y)
                    return v0.y < v1.y ? -1 : 1;
                else if (v0.z != v1.z)
                    return v0.z < v1.z ? -1 : 1;

                return 0;
            }
        }

        private class CompareY : IComparer<Point3f>
        {
            public int Compare(Point3f v0, Point3f v1)
            {
                if (v0.y != v1.y)
                    return v0.y < v1.y ? -1 : 1;
                else if (v0.z != v1.z)
                    return v0.z < v1.z ? -1 : 1;
                else if (v0.x != v1.x)
                    return v0.x < v1.x ? -1 : 1;

                return 0;
            }
        }

        private class CompareZ : IComparer<Point3f>
        {
            public int Compare(Point3f v0, Point3f v1)
            {
                if (v0.z != v1.z)
                    return v0.z < v1.z ? -1 : 1;
                else if (v0.x != v1.x)
                    return v0.x < v1.x ? -1 : 1;
                else if (v0.y != v1.y)
                    return v0.y < v1.y ? -1 : 1;

                return 0;
            }
        }

    }
}
