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

        /// <summary>
        /// 
        /// </summary>
        public void GetPathToRoot(int start, List<int> path)
        {
            while (start != Parent[start] && Parent[start] != -1)
            {
                path.Add(start);
                start = Parent[start];
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

    }
}
