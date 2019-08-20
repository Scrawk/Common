using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphTree
    {

        public GraphTree(int root, int size)
        {
            Root = root;
            Parent = new int[size];
            Children = new List<int>[size];

            for (int i = 0; i < size; i++)
                Parent[i] = -1;

            Parent[root] = root;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => Parent.Length;

        /// <summary>
        /// 
        /// </summary>
        public int Root { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] Parent { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int>[] Children { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[GraphTree: Root={0}]", Root);
        }

        public List<int> GetPathToRoot(int start)
        {
            var path = new List<int>();
            GetPathToRoot(start, path);
            return path;
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public void SetParent(int i, int parent)
        {
            Parent[i] = parent;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetChild(int i, int child)
        {
            if (Children[i] == null)
                Children[i] = new List<int>();

            Children[i].Add(child);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveBranch(int i)
        {
            Parent[i] = -1;
            
            if(Children[i] != null)
            {
                foreach (var child in Children[i])
                    RemoveBranch(child);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool InTree(int i)
        {
            return Parent[i] != -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLeaf(int i)
        {
            return GetDegree(i) == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public int GetDegree(int i)
        {
            if (Children[i] == null)
                return 0;
            else
                return Children[i].Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateChildren()
        {
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
        /// 
        /// </summary>
        public GraphOrdering DepthFirstOrder()
        {
            int count = Parent.Length;
            var queue = new Stack<int>(count);
            queue.Push(Root);

            var isVisited = new bool[count];
            isVisited[Root] = true;

            var ordering = new GraphOrdering(count);

            while (queue.Count != 0)
            {
                int u = queue.Pop();
                ordering.Vertices.Add(u);

                var edges = Children[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i];

                    if (isVisited[to]) continue;

                    queue.Push(to);
                    isVisited[to] = true;
                }
            }

            return ordering;
        }

        /// <summary>
        /// 
        /// </summary>
        public GraphOrdering BreadthFirstOrder()
        {
            int count = Parent.Length;
            var queue = new Queue<int>(count);
            queue.Enqueue(Root);

            var isVisited = new bool[count];
            isVisited[Root] = true;

            var ordering = new GraphOrdering(count);

            while (queue.Count != 0)
            {
                int u = queue.Dequeue();
                ordering.Vertices.Add(u);

                var edges = Children[u];
                if (edges == null) continue;

                for (int i = 0; i < edges.Count; i++)
                {
                    int to = edges[i];

                    if (isVisited[to]) continue;

                    queue.Enqueue(to);
                    isVisited[to] = true;
                }
            }

            return ordering;
        }

    }
}
