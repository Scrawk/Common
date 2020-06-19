using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{

    /// <summary>
    /// A BVH tree using based on the implementation found here.
    /// http://allenchou.net/2014/02/game-physics-broadphase-dynamic-aabb-tree/
    /// </summary>
    public class BVHTree2f : IShapeCollection2f
    {

        public BVHTree2f()
        {

        }

        public BVHTree2f(float border)
        {
            Border = border;
        }

        /// <summary>
        /// Added border around the nodes aabb.
        /// </summary>
        public float Border { get; private set; }

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
        public BVHTreeNode2f Root { get; private set; }

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
        public bool Contains(IShape2f shape)
        {
            var bounds = shape.Bounds;
            bounds.Min -= Border;
            bounds.Max += Border;

            return FindNode(Root, shape, bounds) != null;
        }

        /// <summary>
        /// Find the node that contains this shape.
        /// </summary>
        public BVHTreeNode2f FindNode(IShape2f shape)
        {
            var bounds = shape.Bounds;
            bounds.Min -= Border;
            bounds.Max += Border;

            return FindNode(Root, shape, bounds);
        }

        /// <summary>
        /// Add a shapes to the tree.
        /// </summary>
        public void Add(IEnumerable<IShape2f> shapes)
        {
            foreach (var shape in shapes)
                Add(shape);
        }

        /// <summary>
        /// Add a shape to the tree.
        /// </summary>
        public void Add(IShape2f shape)
        {
            var bounds = shape.Bounds;
            bounds.Min -= Border;
            bounds.Max += Border;

            if (Root != null)
            {
                var node = new BVHTreeNode2f(shape, bounds);
                Add(Root, node);
            }
            else
                Root = new BVHTreeNode2f(shape, bounds);
        }

        /// <summary>
        /// Remove the node containing this shape.
        /// </summary>
        public bool Remove(IShape2f shape)
        {
            var bounds = shape.Bounds;
            bounds.Min -= Border;
            bounds.Max += Border;

            var node = FindNode(Root, shape, bounds);
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
            return NodeContains(Root, point, false);
        }

        /// <summary>
        /// Does a shapes bounds contain the point.
        /// </summary>
        public bool BoundsContains(Vector2f point)
        {
            return NodeContains(Root, point, true);
        }

        /// <summary>
        /// Does a shape intersects the box.
        /// </summary>
        public bool Intersects(Box2f box)
        {
            return NodeIntersects(Root, box, false);
        }

        /// <summary>
        /// Does a shapes bounds intersects the box.
        /// </summary>
        public bool BoundsIntersects(Box2f box)
        {
            return NodeIntersects(Root, box, true);
        }

        /// <summary>
        /// Return the signed distance field from 
        /// the union of all shapes in tree.
        /// Any point not contained in a leaf nodes aabb has undefined
        /// signed distance.
        /// </summary>
        public float SignedDistance(Vector2f point)
        {
            float sd = float.PositiveInfinity;
            return NodeSignedDistance(Root, point, sd);
        }

        /// <summary>
        /// Create a list containing all the
        /// shapes in the tree.
        /// </summary>
        /// <returns></returns>
        public List<IShape2f> ToList()
        {
            var list = new List<IShape2f>(Count);
            list.AddRange(this);
            return list;
        }

        /// <summary>
        /// Enumerate all leaf nodes.
        /// </summary>
        public IEnumerator<IShape2f> GetEnumerator()
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
        private void Add(BVHTreeNode2f parent, BVHTreeNode2f node)
        {
            if (parent.IsLeaf)
            {
                var bounds = parent.Shape.Bounds;
                bounds.Min -= Border;
                bounds.Max += Border;

                parent.Left = node;
                parent.Right = new BVHTreeNode2f(parent.Shape, bounds);
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
        private void Remove(BVHTreeNode2f node)
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
        private BVHTreeNode2f FindNode(BVHTreeNode2f node, IShape2f shape, Box2f aabb)
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
        /// 
        /// </summary>
        private bool NodeContains(BVHTreeNode2f node, Vector2f point, bool boundsOnly)
        {
            if (node != null && node.Bounds.Contains(point))
            {
                if (node.IsLeaf)
                {
                    if (boundsOnly)
                        return true;
                    else
                        return node.Shape.Contains(point);
                }
                else
                {
                    if (NodeContains(node.Left, point, boundsOnly)) return true;
                    if (NodeContains(node.Right, point, boundsOnly)) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool NodeIntersects(BVHTreeNode2f node, Box2f box, bool boundsOnly)
        {
            if (node != null && node.Bounds.Intersects(box))
            {
                if (node.IsLeaf)
                {
                    if (boundsOnly)
                        return true;
                    else
                        return node.Shape.Intersects(box);
                }
                else
                {
                    if (NodeIntersects(node.Left, box, boundsOnly)) return true;
                    if (NodeIntersects(node.Right, box, boundsOnly)) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private float NodeSignedDistance(BVHTreeNode2f node, Vector2f point, float sd)
        {
            if (node == null) return sd;

            if (node.Bounds.Contains(point))
            {
                if (node.IsLeaf)
                    return Math.Min(sd, node.Shape.SignedDistance(point));
                else
                {
                    float left = NodeSignedDistance(node.Left, point, sd);
                    float right = NodeSignedDistance(node.Right, point, sd);
                    return Math.Min(left, right);
                }
            }

            return sd;
        }

        private int MaxDepth(BVHTreeNode2f node, int depth)
        {
            if (node == null || node.IsLeaf) return depth;
            return Math.Max(MaxDepth(node.Left, depth + 1), MaxDepth(node.Right, depth + 1));
        }

        private int CountLeaves(BVHTreeNode2f node, int count)
        {
            if (node == null || node.IsLeaf) return count + 1;
            return CountLeaves(node.Left, count) + CountLeaves(node.Right, count);
        }


    }
}
