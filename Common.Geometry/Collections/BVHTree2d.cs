﻿using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Shapes;

using REAL = System.Double;
using POINT2 = Common.Core.Numerics.Point2d;
using BOX2 = Common.Core.Shapes.Box2d;

namespace Common.Geometry.Collections
{

    /// <summary>
    /// A BVH tree using based on the implementation found here.
    /// http://allenchou.net/2014/02/game-physics-broadphase-dynamic-aabb-tree/
    /// </summary>
    public class BVHTree2d<T> : IShapeCollection2d<T>
        where T : class, IShape2d
    {

        public BVHTree2d()
        {

        }

        /// <summary>
        /// The number of shapes in the tree.
        /// </summary>
        public int Count
        {
            get
            {
                if (Root == null) return 0;
                return CountLeaves(Root, 0);
            }
        }

        /// <summary>
        /// The max depth of the tree.
        /// </summary>
        public int Depth => MaxDepth(Root, 0);

        /// <summary>
        /// The root node of the tree.
        /// </summary>
        public BVHTreeNode2d<T> Root { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            var bounds = (Root != null) ? Root.Bounds : new BOX2();
            return string.Format("[BVHTree2d: Count={0}, Depth={1}, Bounds={2}]", Count, Depth, bounds);
        }

        /// <summary>
        /// Clear the tree.
        /// </summary>
        public void Clear()
        {
            Root = null;
        }

        /// <summary>
        /// Does the tree contain this shape.
        /// </summary>
        public bool Contains(T shape)
        {
            return FindNode(Root, shape, shape.Bounds) != null;
        }

        /// <summary>
        /// Find the node that contains this shape.
        /// </summary>
        public BVHTreeNode2d<T> FindNode(T shape)
        {
            return FindNode(Root, shape, shape.Bounds);
        }

        /// <summary>
        /// Add a shapes to the tree.
        /// </summary>
        public void Add(IEnumerable<T> shapes)
        {
            foreach (var shape in shapes)
                Add(shape);
        }

        /// <summary>
        /// Add a shape to the tree.
        /// </summary>
        public void Add(T shape)
        {
            if (Root != null)
            {
                var node = new BVHTreeNode2d<T>(shape);
                Add(Root, node);
            }
            else
                Root = new BVHTreeNode2d<T>(shape);
        }

        /// <summary>
        /// Remove the node containing this shape.
        /// </summary>
        public bool Remove(T shape)
        {
            var node = FindNode(Root, shape, shape.Bounds);
            if (node == null) return false;

            if (node == Root)
                Root = null;
            else
                Remove(node);

            return true;
        }

        /// <summary>
        /// Does a shape contain the point.
        /// </summary>
        public T Contains(POINT2 point)
        {
            return NodeContains(Root, point)?.Shape;
        }

        /// <summary>
        /// Find all the shapes that contain the point and 
        /// add them to the list.
        /// </summary>
        public void Containing(POINT2 point, List<T> shapes)
        {
            NodeContaining(Root, point, shapes);
        }

        /// <summary>
        /// Does a shape intersect the box.
        /// </summary>
        public T Intersects(BOX2 box)
        {
            return NodeIntersects(Root, box)?.Shape;
        }

        /// <summary>
        /// Find all the shapes that intersect the box and 
        /// add them to the list.
        /// </summary>
        public void Intersecting(BOX2 box, List<T> shapes)
        {
            NodeIntersecting(Root, box, shapes);
        }

        /// <summary>
        /// The signed distance to the closest shape.
        /// </summary>
        public REAL SignedDistance(POINT2 point)
        {
            REAL sd = REAL.PositiveInfinity;

            var middle = NodeCenterMost(Root, true);
            if (middle == null) return sd;

            sd = middle.Shape.SignedDistance(point);
            return NodeSignedDistance(Root, point, sd);
        }

        /// <summary>
        /// Create a list containing all the
        /// shapes in the tree.
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            var list = new List<T>(Count);
            list.AddRange(this);
            return list;
        }

        /// <summary>
        /// Enumerate all leaf nodes.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (Root != null)
            {
                foreach (var node in Root)
                    yield return node.Shape;
            }
        }

        /// <summary>
        /// Enumerate all leaf nodes.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Add a node to the tree.
        /// </summary>
        private void Add(BVHTreeNode2d<T> parent, BVHTreeNode2d<T> node)
        {
            if (parent.IsLeaf)
            {
                parent.Left = node;
                parent.Right = new BVHTreeNode2d<T>(parent.Shape);
                parent.Shape = null;

                parent.Left.Parent = parent;
                parent.Right.Parent = parent;
            }
            else
            {
                var aabb0 = parent.Left.Bounds;
                var aabb1 = parent.Right.Bounds;

                REAL diff0 = BOX2.Enlarge(aabb0, node.Bounds).Area - aabb0.Area;
                REAL diff1 = BOX2.Enlarge(aabb1, node.Bounds).Area - aabb1.Area;

                if (diff0 < diff1)
                    Add(parent.Left, node);
                else
                    Add(parent.Right, node);
            }

            {
                var aabb0 = parent.Left.Bounds;
                var aabb1 = parent.Right.Bounds;
                parent.Bounds = BOX2.Enlarge(aabb0, aabb1);
                parent.Shape = null;
            }
        }

        /// <summary>
        /// Remove a node from the tree.
        /// All its children will be remove as well.
        /// </summary>
        private void Remove(BVHTreeNode2d<T> node)
        {
            var parent = node.Parent;
            var sibling = node.Sibling;

            if (parent.Parent != null)
            {
                sibling.Parent = parent.Parent;

                if (parent == parent.Parent.Left)
                    parent.Parent.Left = sibling;
                else
                    parent.Parent.Right = sibling;
            }
            else
            {
                Root = sibling;
                sibling.Parent = null;
            }
        }

        /// <summary>
        /// Find the node that holds this shape by iterating
        /// throught the nodes if the shapes aabb is contains
        /// in the nodes bounds.
        /// </summary>
        private BVHTreeNode2d<T> FindNode(BVHTreeNode2d<T> node, IShape2d shape, BOX2 aabb)
        {
            if (node == null) return null;
            if (shape == null) return null;

            if (node.Bounds.Contains(aabb, true))
            {
                if (node.Shape == shape)
                    return node;
                else
                {
                    var left = FindNode(node.Left, shape, aabb);
                    if (left != null) return left;

                    var right = FindNode(node.Right, shape, aabb);
                    if (right != null) return right;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the leaf node that has a shape containing
        /// this point.
        /// </summary>
        private BVHTreeNode2d<T> NodeContains(BVHTreeNode2d<T> node, POINT2 point)
        {
            if (node != null && node.Bounds.Contains(point, true))
            {
                if (node.IsLeaf)
                {
                    if (node.Shape.Contains(point))
                        return node;
                }
                else
                {
                    var left = NodeContains(node.Left, point);
                    if (left != null) return left;

                    var right = NodeContains(node.Right, point);
                    if (right != null) return right;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all the shapes that contain the point and 
        /// add them to the list.
        /// </summary>
        private void NodeContaining(BVHTreeNode2d<T> node, POINT2 point, List<T> shapes)
        {
            if (node != null && node.Bounds.Contains(point, true))
            {
                if (node.IsLeaf)
                {
                    if (node.Shape.Contains(point))
                        shapes.Add(node.Shape);
                }
                else
                {
                    NodeContaining(node.Left, point, shapes);
                    NodeContaining(node.Right, point, shapes);
                }
            }
        }

        /// <summary>
        /// Find the leaf node that has a shape intersecting
        /// this box.
        /// </summary>
        private BVHTreeNode2d<T> NodeIntersects(BVHTreeNode2d<T> node, BOX2 box)
        {
            if (node != null && node.Bounds.Intersects(box, true))
            {
                if (node.IsLeaf)
                {
                    if (node.Shape.Intersects(box))
                        return node;
                }
                else
                {
                    var left = NodeIntersects(node.Left, box);
                    if (left != null) return left;

                    var right = NodeIntersects(node.Right, box);
                    if (right != null) return right;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all the shapes that intersect the box and 
        /// add them to the list.
        /// </summary>
        private bool NodeIntersecting(BVHTreeNode2d<T> node, BOX2 box, List<T> shapes)
        {
            if (node != null && node.Bounds.Intersects(box, true))
            {
                if (node.IsLeaf)
                {
                    if (node.Shape.Intersects(box))
                        shapes.Add(node.Shape);
                }
                else
                {
                    NodeIntersecting(node.Left, box, shapes);
                    NodeIntersecting(node.Right, box, shapes);
                }
            }

            return false;
        }

        /// <summary>
        /// Find the smallest signed distance to the point.
        /// </summary>
        private REAL NodeSignedDistance(BVHTreeNode2d<T> node, POINT2 point, REAL sd)
        {
            if (node != null)
            {
                if (node.IsLeaf)
                {
                    sd = Math.Min(sd, node.Shape.SignedDistance(point));
                }
                else
                {
                    REAL leftSD = REAL.PositiveInfinity;
                    REAL rightSD = REAL.PositiveInfinity;

                    if (node.Left != null)
                        leftSD = node.Left.Bounds.SignedDistance(point);

                    if (node.Right != null)
                        rightSD = node.Right.Bounds.SignedDistance(point);

                    if (leftSD <= sd)
                        sd = NodeSignedDistance(node.Left, point, sd);

                    if (rightSD <= sd)
                        sd = NodeSignedDistance(node.Right, point, sd);
                }
            }

            return sd;
        }

        /// <summary>
        /// Find the node that is center most in the tree.
        /// </summary>
        private BVHTreeNode2d<T> NodeCenterMost(BVHTreeNode2d<T> node, bool left)
        {
            if (node != null)
            {
                if (node.IsLeaf)
                {
                    return node;
                }
                else
                {
                    if (left || node.Right == null)
                    {
                        var n = NodeCenterMost(node.Left, !left);
                        if (n != null) return n;
                    }

                    if (!left || node.Left == null)
                    {
                        var n = NodeCenterMost(node.Right, !left);
                        if (n != null) return n;
                    }
                }
            }

            return null;
        }

        private int MaxDepth(BVHTreeNode2d<T> node, int depth)
        {
            if (node == null || node.IsLeaf) return depth;
            return Math.Max(MaxDepth(node.Left, depth + 1), MaxDepth(node.Right, depth + 1));
        }

        private int CountLeaves(BVHTreeNode2d<T> node, int count)
        {
            if (node == null || node.IsLeaf) return count + 1;
            return CountLeaves(node.Left, count) + CountLeaves(node.Right, count);
        }


    }
}
