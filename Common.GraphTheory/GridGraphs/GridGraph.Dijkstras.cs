using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridGraph
    {

        public GridSearch DijkstrasShortestPathTree(Point2i root)
        {
            var search = new GridSearch(Width, Height);
            DijkstrasShortestPathTree(search, root.x, root.y);
            return search;
        }

        public GridSearch DijkstrasShortestPathTree(int x, int y)
        {
            var search = new GridSearch(Width, Height);
            DijkstrasShortestPathTree(search, x, y);
            return search;
        }

        public void DijkstrasShortestPathTree(GridSearch search, int x, int y)
        {
            int width = Width;
            int height = Height;

            var queue = new List<GridVertex>(width * height);
            GetAllVertices(queue);
            var vertexGrid = new GridVertex[width, height];

            for (int i = 0; i < queue.Count; i++)
            {
                var v = queue[i];
                int xi = v.Index.x;
                int yi = v.Index.y;
                v.Cost = float.PositiveInfinity;

                vertexGrid[xi, yi] = v;
            }

            search.SetIsVisited(x, y, true);
            search.SetParent(x, y, new Point2i(x, y));
            vertexGrid[x, y].Cost = 0;

            while (queue.Count != 0)
            {
                queue.Sort();

                var vertex = queue[0];
                queue.RemoveAt(0);
                var u = vertex.Index;

                search.AddOrder(u);
                search.SetIsVisited(u,  true);

                int edge = Edges[u.x, u.y];
                if (edge != 0)
                {
                    float cost = vertexGrid[u.x, u.y].Cost;

                    for (int j = 0; j < Directions.Count; j++)
                    {
                        int i = Directions[j];
                        int xi = u.x + D8.OFFSETS[i, 0];
                        int yi = u.y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > width - 1) continue;
                        if (yi < 0 || yi > height - 1) continue;

                        if ((edge & 1 << i) == 0) continue;
                        if (search.GetIsVisited(xi, yi)) continue;

                        var v = vertexGrid[xi, yi];
                        if (v == null)
                        {
                            v = new GridVertex(xi, yi, 0);
                            vertexGrid[xi, yi] = v;
                        }

                        float alt = cost + (float)Point2i.Distance(u, v.Index);

                        if (alt < v.Cost)
                        {
                            if (!MathUtil.IsFinite(alt))
                                throw new ArithmeticException("Cost is not finite.");

                            v.Cost = alt;
                            search.SetParent(xi, yi, u);
                        }
                    }
                }
            }
        }
    }
}
