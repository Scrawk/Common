﻿using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    /// <summary>
    /// A 2f KdTree using T as the point.
    /// Does not support points with the same coordinates.
    /// </summary>
    public class KdTree2f<T> : IStaticPointCollection2f<T>
        where T : IPoint2f
    {

        private Box2f m_bounds;

        public KdTree2f()
        {

        }

        public KdTree2f(IEnumerable<T> points)
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
        public Box2f Bounds { get { return m_bounds; } }

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        public KdTreeNode2f<T> Root { get; private set; }

        public override string ToString()
        {
            return string.Format("[KdTree2f: Count={0}, Depth={1}, Bounds={2}]", Count, Depth, Bounds);
        }

        /// <summary>
        /// Clear the tree.
        /// </summary>
        public void Clear()
        {
            Root = null;
            Count = 0;
            m_bounds = new Box2f();
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
                m_bounds = new Box2f(p.x, p.x, p.y, p.y);
                Root = new KdTreeNode2f<T>(p, 0);
                Count = 1;
            }
            else
            {
                var p = list[0];
                m_bounds = new Box2f(p.x, p.x, p.y, p.y);
                Root = Build(list, 0);
            }
        }

        /// <summary>
        /// Return all points contained in the search region.
        /// </summary>
        public void Search(Circle2f region, List<T> points)
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
        public void CalculateSegments(List<Segment2f> segments, bool addBounds = true)
        {
            if (addBounds)
            {
                segments.Add(new Segment2f(m_bounds.Corner00, m_bounds.Corner10));
                segments.Add(new Segment2f(m_bounds.Corner10, m_bounds.Corner11));
                segments.Add(new Segment2f(m_bounds.Corner11, m_bounds.Corner01));
                segments.Add(new Segment2f(m_bounds.Corner01, m_bounds.Corner00));
            }

            CalculateSegments(null, Root, Bounds, segments);
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
        private KdTreeNode2f<T> Build(List<T> points, int depth)
        {
            int count = points.Count;
            if (count == 0)
                return null;

            if (count == 1)
            {
                var p = points[0];
                Count++;
                m_bounds = Box2f.Enlarge(m_bounds, new Vector2f(p.x, p.y));
                return new KdTreeNode2f<T>(p, depth);
            }
            else
            {
                int median = count / 2;
                List<T> left, right;
                T p;

                if (depth % 2 == 0)
                {
                    points.Sort(m_compareX);
                    p = points[median];
                }
                else
                {
                    points.Sort(m_compareY);
                    p = points[median];
                }

                left = points.GetRange(0, median);
                right = points.GetRange(median + 1, count - median-1);

                Count++;
                m_bounds = Box2f.Enlarge(m_bounds, new Vector2f(p.x, p.y));
                var node = new KdTreeNode2f<T>(p, depth);

                node.Left = Build(left, depth + 1);
                node.Right = Build(right, depth + 1);

                return node;
            }
        }

        /// <summary>
        /// Iteratively searchs the tree for points 
        /// contained in the search region.
        /// </summary>
        private void Search(KdTreeNode2f<T> node, Circle2f region, Box2f bounds, List<T> points)
        {
            if (node == null) return;

            if (PointOps2f<T>.Contains(region, node.Point))
                points.Add(node.Point);

            var left = bounds;
            var right = bounds;

            if (node.IsVertical)
            {
                left.Max.x = node.Point.x;
                right.Min.x = node.Point.x;
            }
            else
            {
                left.Max.y = node.Point.y;
                right.Min.y = node.Point.y;
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
        private void Closest(KdTreeNode2f<T> node, T point, ref T closest, ref float dist)
        {
            if (node == null) return;

            float d = PointOps2f<T>.Distance(node.Point, point);
            if (d < dist)
            {
                dist = d;
                closest = node.Point;
            }

            if (node.IsVertical)
            {
                if (point.x - dist <= node.Point.x)
                    Closest(node.Left, point, ref closest, ref dist);

                if (point.x + dist >= node.Point.x)
                    Closest(node.Right, point, ref closest, ref dist);
            }
            else
            {
                if (point.y - dist <= node.Point.y)
                    Closest(node.Left, point, ref closest, ref dist);

                if (point.y + dist >= node.Point.y)
                    Closest(node.Right, point, ref closest, ref dist);
            }
        }

        /// <summary>
        /// Iteratively adds the points into the list.
        /// </summary>
        private void CopyTo(KdTreeNode2f<T> node, List<T> points)
        {
            if (node == null) return;
            foreach (var n in node)
                points.Add(n.Point);
        }

        /// <summary>
        /// Iteratively calculates the segements made from 
        /// the intersecting lines.
        /// </summary>
        private void CalculateSegments(KdTreeNode2f<T> parent, KdTreeNode2f<T> node, Box2f bounds, List<Segment2f> segments)
        {
            if (node == null) return;

            Vector2f p0, p1;
            if (node.IsVertical)
            {
                p0 = new Vector2f(node.Point.x, bounds.Min.y);
                p1 = new Vector2f(node.Point.x, bounds.Max.y);

                if (parent != null)
                {
                    if (node == parent.Left)
                    {
                        p1.y = parent.Point.y;
                        bounds.Max.y = p1.y;
                    }
                    else
                    {
                        p0.y = parent.Point.y;
                        bounds.Min.y = p0.y;
                    }
                }
            }
            else
            {
                p0 = new Vector2f(bounds.Min.x, node.Point.y);
                p1 = new Vector2f(bounds.Max.x, node.Point.y);
               
                if (parent != null)
                {
                    if (node == parent.Left)
                    {
                        p1.x = parent.Point.x;
                        bounds.Max.x = p1.x;
                    }
                    else
                    {
                        p0.x = parent.Point.x;
                        bounds.Min.x = p0.x;
                    }
                }
            }

            segments.Add(new Segment2f(p0, p1));

            CalculateSegments(node, node.Left, bounds, segments);
            CalculateSegments(node, node.Right, bounds, segments);
        }

        /// <summary>
        /// Find the max depth of tree.
        /// </summary>
        public int MaxDepth(KdTreeNode2f<T> node)
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

    }
}
