using System;
using System.Collections;
using System.Collections.Generic;
using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{

    /// <summary>
    /// A BVH tree using based on the implementation found here.
    /// http://allenchou.net/2014/02/game-physics-broadphase-dynamic-aabb-tree/
    /// </summary>
    public class BVHTree2f<T> : IShapeCollection2f<T>
        where T : class, IShape2f
    {

        public BVHTree2f()
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
        public BVHTreeNode2f<T> Root { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            var bounds = (Root != null) ? Root.Bounds : new Box2f();
            return string.Format("[BVHTree2f: Count={0}, Depth={1}, Bounds={2}]", Count, Depth, bounds);
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
        public BVHTreeNode2f<T> FindNode(T shape)
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
                var node = new BVHTreeNode2f<T>(shape);
                Add(Root, node);
            }
            else
                Root = new BVHTreeNode2f<T>(shape);
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
        public bool Contains(Vector2f point)
        {
            return NodeContains(Root, point);
        }

        /// <summary>
        /// Find all the shapes that contain the point and 
        /// add them to the list.
        /// </summary>
        public void Containing(Vector2f point, List<T> shapes)
        {
            NodeContaining(Root, point, shapes);
        }

        /// <summary>
        /// Does a shape intersects the box.
        /// </summary>
        public bool Intersects(Box2f box)
        {
            return NodeIntersects(Root, box);
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
        private void Add(BVHTreeNode2f<T> parent, BVHTreeNode2f<T> node)
        {
            if (parent.IsLeaf)
            {
                parent.Left = node;
                parent.Right = new BVHTreeNode2f<T>(parent.Shape);
                parent.Shape = null;

                parent.Left.Parent = parent;
                parent.Right.Parent = parent;
            }
            else
            {
                var aabb0 = parent.Left.Bounds;
                var aabb1 = parent.Right.Bounds;

                float diff0 = Box2f.Enlarge(aabb0, node.Bounds).Area - aabb0.Area;
                float diff1 = Box2f.Enlarge(aabb1, node.Bounds).Area - aabb1.Area;

                if (diff0 < diff1)
                    Add(parent.Left, node);
                else
                    Add(parent.Right, node);
            }

            {
                var aabb0 = parent.Left.Bounds;
                var aabb1 = parent.Right.Bounds;
                parent.Bounds = Box2f.Enlarge(aabb0, aabb1);
                parent.Shape = null;
            }
        }

        /// <summary>
        /// Remove a node from the tree.
        /// All its children will be remove as well.
        /// </summary>
        private void Remove(BVHTreeNode2f<T> node)
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
        private BVHTreeNode2f<T> FindNode(BVHTreeNode2f<T> node, IShape2f shape, Box2f aabb)
        {
            if (node == null) return null;
            if (shape == null) return null;

            if (node.Bounds.Contains(aabb))
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
        private bool NodeContains(BVHTreeNode2f<T> node, Vector2f point)
        {
            if (node != null && node.Bounds.Contains(point))
            {
                if (node.IsLeaf)
                {
                    return node.Shape.Contains(point);
                }
                else
                {
                    if (NodeContains(node.Left, point)) return true;
                    if (NodeContains(node.Right, point)) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Find all the shapes that contain the point and 
        /// add them to the list.
        /// </summary>
        private void NodeContaining(BVHTreeNode2f<T> node, Vector2f point, List<T> shapes)
        {
            if (node != null && node.Bounds.Contains(point))
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
        private bool NodeIntersects(BVHTreeNode2f<T> node, Box2f box)
        {
            if (node != null && node.Bounds.Intersects(box))
            {
                if (node.IsLeaf)
                {
                    return node.Shape.Intersects(box);
                }
                else
                {
                    if (NodeIntersects(node.Left, box)) return true;
                    if (NodeIntersects(node.Right, box)) return true;
                }
            }

            return false;
        }

        private int MaxDepth(BVHTreeNode2f<T> node, int depth)
        {
            if (node == null || node.IsLeaf) return depth;
            return Math.Max(MaxDepth(node.Left, depth + 1), MaxDepth(node.Right, depth + 1));
        }

        private int CountLeaves(BVHTreeNode2f<T> node, int count)
        {
            if (node == null || node.IsLeaf) return count + 1;
            return CountLeaves(node.Left, count) + CountLeaves(node.Right, count);
        }


    }
}
