using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// Represents a tree of a given graph.
    /// The tree only holds the indices of
    /// the vertices from the graph it was 
    /// created from. A tree may not contain 
    /// all the vertices in the graph if the
    /// graph is not completely connected.
    /// </summary>
    public class GraphTree
    {

        /// <summary>
        /// Create a new tree. Must be the same
        /// size graph and include space for all
        /// vertices of the graph even if they are 
        /// not in the tree.
        /// </summary>
        /// <param name="root">The trees root.</param>
        /// <param name="size">The size of the tree and graph</param>
        public GraphTree(AdjacencyGraph graph, int root, int size)
        {
            Graph = graph;
            Root = root;
            Parent = new int[size];
            Children = null;

            for (int i = 0; i < size; i++)
                Parent[i] = -1;

            //The root is its own parent.
            Parent[root] = root;
        }

        public AdjacencyGraph Graph { get; private set; }

        /// <summary>
        /// The number of verices in the graph.
        /// </summary>
        private int Count => Parent.Length;

        /// <summary>
        /// The root vertex.
        /// </summary>
        public int Root { get; private set; }

        /// <summary>
        /// The vertices parents. A vertex that is 
        /// not in the tree has -1 as its parent.
        /// </summary>
        public int[] Parent { get; private set; }

        /// <summary>
        /// The children of each vertex.
        /// </summary>
        public List<int>[] Children { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[GraphTree: Root={0}]", Root);
        }

        /// <summary>
        /// Tag all the vertices in the tree.
        /// </summary>
        public void TagAll(int tag)
        {
            for (int i = 0; i < Count; i++)
            {
                if (InTree(i))
                    Graph.Vertices[i].Tag = tag;
            }
        }

        /// <summary>
        /// Get a path from the vertex to the root.
        /// The path is made up of the vertices index 
        /// in the graph the tree was created from.
        /// </summary>
        public List<int> GetPathToRoot(int vert)
        {
            var path = new List<int>();
            GetPathToRoot(vert, path);
            return path;
        }

        /// <summary>
        /// Get a path from the vertex to the root.
        /// The path is made up of the vertices index 
        /// in the graph the tree was created from.
        /// </summary>
        public void GetPathToRoot(int vert, List<int> path)
        {
            if (vert == Root) return;

            while (true)
            {
                path.Add(vert);
                vert = Parent[vert];

                //Found the root. 
                //Add it to path and return.
                if(vert == Root)
                {
                    path.Add(vert);
                    return;
                }

                //There is no path to the root.
                //Clear path and return.
                if (vert == -1)
                {
                    path.Clear();
                    return;
                }
            }
        }

        /// <summary>
        /// Set the parent of a vertex.
        /// </summary>
        public void SetParent(int i, int parent)
        {
            Parent[i] = parent;
        }

        /// <summary>
        /// Set a child of a vertex.
        /// </summary>
        public void SetChild(int i, int child)
        {
            if (Children[i] == null)
                Children[i] = new List<int>();

            Children[i].Add(child);
        }

        /// <summary>
        /// Removes the vertex i from the tree 
        /// and all other vertices decended
        /// from that vertex.
        /// </summary>
        public void RemoveBranch(int i)
        {
            if (i == Root)
                throw new InvalidOperationException("Can not remove the root branch.");

            //Need to remove i from its 
            //parents children list;
            var p = Parent[i];
            if (p != -1)
            {
                Children[p].Remove(i);
                Parent[i] = -1;
            }

            if (Children[i] != null)
            {
                foreach (var child in Children[i])
                    RemoveBranchRecursive(child);

                Children[i] = null;
            }
        }

        private void RemoveBranchRecursive(int i)
        {
            if (Children[i] != null)
            {
                foreach (var child in Children[i])
                    RemoveBranchRecursive(child);

                Children[i] = null;
            }

            Parent[i] = -1;
        }

        /// <summary>
        /// Is this vert included in the tree.
        /// </summary>
        public bool InTree(int i)
        {
            return Parent[i] != -1;
        }

        /// <summary>
        /// Is this vertex a leaf.
        /// Leaf vertices have no children.
        /// </summary>
        public bool IsLeaf(int i)
        {
            return GetDegree(i) == 0;
        }

        /// <summary>
        /// The number of vertices in the graph  
        /// that are included in the tree.
        /// </summary>
        /// <returns></returns>
        public int TreeSize()
        {
            int size = 0;
            for (int i = 0; i < Count; i++)
            {
                if (!InTree(i)) continue;
                size++;
            }

            return size;
        }

        /// <summary>
        /// Get the degree of this vertex.
        /// The degree represents the number 
        /// of children a vertex has.
        /// </summary>
        public int GetDegree(int i)
        {
            if (Children[i] == null)
                return 0;
            else
                return Children[i].Count;
        }

        /// <summary>
        /// Presuming each vert has its parent set 
        /// then find the children of each vertex.
        /// </summary>
        public void CreateChildren()
        {
            if(Children == null)
                Children = new List<int>[Count];

            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] != null)
                    Children[i].Clear();
            }

            for (int i = 0; i < Parent.Length; i++)
            {
                int p = Parent[i];
                if(p != -1 && p != i)
                {
                    if (Children[p] == null)
                        Children[p] = new List<int>();

                    Children[p].Add(i);
                }
            }
        }

        /// <summary>
        /// Returns the vertices of the tree in depth first order.
        /// </summary>
        public List<int> DepthFirstOrder()
        {
            int count = Parent.Length;
            var queue = new Stack<int>(count);
            queue.Push(Root);

            TagAll(0);
            Graph.Vertices[Root].Tag = 1;

            var ordering = new List<int>(count);

            while (queue.Count != 0)
            {
                int u = queue.Pop();
                ordering.Add(u);

                var edges = Children[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i];

                    if (Graph.Vertices[to].Tag == 1) continue;

                    queue.Push(to);
                    Graph.Vertices[to].Tag = 1;
                }
            }

            return ordering;
        }

        /// <summary>
        /// Returns the vertices of the tree in breadth first order.
        /// </summary>
        public List<int> BreadthFirstOrder()
        {
            int count = Parent.Length;
            var queue = new Queue<int>(count);
            queue.Enqueue(Root);

            TagAll(0);
            Graph.Vertices[Root].Tag = 1;

            var ordering = new List<int>(count);

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
                ordering.Add(u);

                var edges = Children[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i];

                    if (Graph.Vertices[to].Tag == 1) continue;

                    queue.Enqueue(to);
                    Graph.Vertices[to].Tag = 1;
                }
            }

            return ordering;
        }

        /// <summary>
        /// Get the index of all leaf vertices.
        /// </summary>
        public void GetLeaves(List<int> leaves)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!InTree(i)) continue;
                if (!IsLeaf(i)) continue;

                leaves.Add(i);
            }
        }

        /// <summary>
        /// Get the data of all leaf vertices.
        /// </summary>
        public void GetLeavesData<T>(List<T> data)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!InTree(i)) continue;
                if (!IsLeaf(i)) continue;

                data.Add(Graph.GetVertexData<T>(i));
            }
        }

        /// <summary>
        /// Get the data of all vertices.
        /// </summary>
        public void GetData<T>(List<T> data)
        {
            for (int i = 0; i < Count; i++)
            {
                if (!InTree(i)) continue;
                data.Add(Graph.GetVertexData<T>(i));
            }
        }

        /// <summary>
        /// Get a flattened list of all edges in the tree.
        /// </summary>
        public void GetAllEdges(List<GraphEdge> edges)
        {
            for (int i = 0; i < Count; i++)
            {
                var children = Children[i];
                if (children == null) continue;

                for (int j = 0; j < children.Count; j++)
                {
                    var c = children[j];
                    var edge = Graph.GetEdge(i, c);
                    if (edge == null) continue;
                    edges.Add(edge);
                }
            }
        }

        /// <summary>
        /// Find the sum of the weights from this tree.
        /// </summary>
        public float FindWeightSum()
        {
            float sum = 0;
            for (int i = 0; i < Count; i++)
            {
                var children = Children[i];
                if (children == null) continue;

                for (int j = 0; j < children.Count; j++)
                {
                    var c = children[j];
                    var edge = Graph.GetEdge(i, c);

                    sum += edge.Weight;
                }
            }

            return sum;
        }

        /// <summary>
        /// Find the sum of the weights from this path.
        /// </summary>
        public float FindWeightSum(IList<int> path)
        {
            float sum = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                int i0 = path[i + 0];
                int i1 = path[i + 1];
                sum += Graph.GetEdge(i0, i1).Weight;
            }

            return sum;
        }

    }
}
