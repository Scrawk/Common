using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    /// <summary>
    /// A 3f KdTree using Vector3f as the point.
    /// Does not support points with the same coordinates.
    /// </summary>
    public class KdTree3f : IStaticPointCollection3f, ISignedDistanceFunction3f
    {

        private Box3f m_bounds;

        public KdTree3f()
        {

        }

        public KdTree3f(IEnumerable<Vector3f> points)
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
        public void Build(IEnumerable<Vector3f> points)
        {
            var list = new List<Vector3f>(points);

            int count = list.Count;
            if (count == 0)
            {
                Clear();
            }
            else if (count == 1)
            {
                var p = list[0];
                m_bounds = new Box3f(p, p);
                Root = new KdTreeNode3f(p, 0, 0);
                Count = 1;
            }
            else
            {
                var p = list[0];
                m_bounds = new Box3f(p, p);
                Root = Build(list, 0);
            }
        }

        /// <summary>
        /// Return all points contained in the search region.
        /// </summary>
        public void Search(Sphere3f region, List<Vector3f> points)
        {
            if (!region.Intersects(Bounds)) return;
            Search(Root, region, Bounds, points);
        }

        /// <summary>
        /// Return all points indices contained in the search region.
        /// </summary>
        public void Search(Sphere3f region, List<int> indices)
        {
            if (!region.Intersects(Bounds)) return;
            Search(Root, region, Bounds, indices);
        }

        /// <summary>
        /// Find the nsigned distance to input point.
        /// </summary>
        public float SignedDistance(Vector3f point)
        {
            if (Count == 0)
                return float.PositiveInfinity;

            return Vector3f.Distance(point, Closest(point));
        }

        /// <summary>
        /// Find the nearest point to input point.
        /// </summary>
        public Vector3f Closest(Vector3f point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find closest point if collection is empty.");

            Vector3f closest = new Vector3f();
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

                for(int i = 0; i < CUBE_INDICES.Length/2; i++)
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
        public List<Vector3f> ToList()
        {
            var list = new List<Vector3f>(Count);
            CopyTo(Root, list);
            return list;
        }

        /// <summary>
        /// Gets an enumerator for the tree.
        /// </summary>
        public IEnumerator<Vector3f> GetEnumerator()
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
        private KdTreeNode3f Build(List<Vector3f> points, int depth)
        {
            int count = points.Count;
            if (count == 0)
                return null;

            if (count == 1)
            {
                var p = points[0];
                Count++;
                m_bounds = Box3f.Enlarge(m_bounds, p);
                return new KdTreeNode3f(p, depth, Count-1);
            }
            else
            {
                int median = count / 2;
                List<Vector3f> left, right;
                Vector3f p;

                if (depth % 3 == 0)
                {
                    points.Sort(m_compareX);
                    p = points[median];
                }
                else if(depth % 3 == 1)
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
                m_bounds = Box3f.Enlarge(m_bounds, p);
                var node = new KdTreeNode3f(p, depth, Count-1);

                node.Left = Build(left, depth + 1);
                node.Right = Build(right, depth + 1);

                return node;
            }
        }

        /// <summary>
        /// Iteratively searchs the tree for points contained
        /// in the search region.
        /// </summary>
        private void Search(KdTreeNode3f node, Sphere3f region, Box3f bounds, List<Vector3f> points)
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
            else if(node.Depth % 3 == 1)
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
        /// Iteratively searchs the tree for points contained
        /// in the search region.
        /// </summary>
        private void Search(KdTreeNode3f node, Sphere3f region, Box3f bounds, List<int> indices)
        {
            if (node == null) return;

            if (region.Contains(node.Point))
                indices.Add(node.Index);

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
                CopyTo(node.Left, indices);
            else if (region.Intersects(left))
                Search(node.Left, region, left, indices);

            if (region.Contains(right))
                CopyTo(node.Right, indices);
            else if (region.Intersects(right))
                Search(node.Right, region, right, indices);
        }

        /// <summary>
        /// Iteratively searchs the tree for the nearest point to input.
        /// </summary>
        private void Closest(KdTreeNode3f node, Vector3f point, ref Vector3f closest, ref float dist)
        {
            if (node == null) return;

            float d = Vector3f.Distance(node.Point, point);
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
        private void CopyTo(KdTreeNode3f node, List<Vector3f> points)
        {
            if (node == null) return;
            foreach (var n in node)
                points.Add(n.Point);
        }

        /// <summary>
        /// Iteratively adds the point indices into the list.
        /// </summary>
        private void CopyTo(KdTreeNode3f node, List<int> indices)
        {
            if (node == null) return;
            foreach (var n in node)
                indices.Add(n.Index);
        }

        /// <summary>
        /// Iteratively calculates the segements made from 
        /// the intersecting lines.
        /// </summary>
        private void CalculateSegments(KdTreeNode3f parent, KdTreeNode3f node, Box3f bounds, List<Segment3f> segments)
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

        private class CompareX : IComparer<Vector3f>
        {
            public int Compare(Vector3f v0, Vector3f v1)
            {
                return v0.x.CompareTo(v1.x);
            }
        }

        private class CompareY : IComparer<Vector3f>
        {
            public int Compare(Vector3f v0, Vector3f v1)
            {
                return v0.y.CompareTo(v1.y);
            }
        }

        private class CompareZ : IComparer<Vector3f>
        {
            public int Compare(Vector3f v0, Vector3f v1)
            {
                return v0.z.CompareTo(v1.z);
            }
        }

    }
}
