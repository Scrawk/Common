using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Directions;
using Common.Core.Shapes;
using Common.Core.Extensions;

namespace Common.GraphTheory.GridGraphs
{
    public partial class GridFlowGraph
    {
        //const int NODES = 6;
        const int INFINITE = 10000000;

        private void Push(float[,] C, float[,] F, float[] excess, int u, int v)
        {
            float send = Math.Min(excess[u], C[u, v] - F[u, v]);
            F[u, v] += send;
            F[v, u] -= send;
            excess[u] -= send;
            excess[v] += send;
        }

        private void Push(float[,,] excess, Point2i u, Point2i v)
        {
            int i = GetEdgeDirection(u, v);
            var flow = GetFlow(u.x, u.y, i);
            var capacity = GetFlow(u.x, u.y, i);

            float send = Math.Min(excess[u.x, u.y, i], capacity - flow);

            flow += send;
            flow -= send;
            SetFlow(u.x, u.y, i, flow);

            excess[u.x, u.y, i] -= send;
            excess[u.x, u.y, i] += send;
        }

        private void Relabel(float[,] C, float[,] F, float[] height, int u)
        {
            int NODES = C.GetLength(0);
            float min_height = INFINITE;

            for (int v = 0; v < NODES; v++)
            {
                if (C[u, v] - F[u, v] > 0)
                {
                    min_height = Math.Min(min_height, height[v]);
                    height[u] = min_height + 1;
                }
            }
        }

        private void Discharge(float[,] C, float[,] F, float[] excess, float[] height, int[] seen, int u)
        {
            int NODES = C.GetLength(0);

            while (excess[u] > 0)
            {
                if (seen[u] < NODES)
                {
                    int v = seen[u];
                    if ((C[u, v] - F[u, v] > 0) && (height[u] > height[v]))
                    {
                        Push(C, F, excess, u, v);
                    }
                    else
                    {
                        seen[u] += 1;
                    }
                }
                else
                {
                    Relabel(C, F, height, u);
                    seen[u] = 0;
                }
            }
        }

        private static void MoveToFront(int i, float[] A)
        {
            float temp = A[i];

            for (int n = i; n > 0; n--)
            {
                A[n] = A[n - 1];
            }

            A[0] = temp;
        }

        private static void MoveToFront(int i, int[] A)
        {
            int temp = A[i];

            for (int n = i; n > 0; n--)
            {
                A[n] = A[n - 1];
            }

            A[0] = temp;
        }

        public float PushRelabel(float[,] C, float[,] F, int source, int sink)
        {
            int NODES = C.GetLength(0);

            float[] excess = new float[NODES];
            float[] height = new float[NODES];
            int[] list = new int[NODES - 2];
            int[] seen = new int[NODES];

            for (int i = 0, j = 0; i < NODES; i++)
            {
                if ((i != source) && (i != sink))
                {
                    list[j] = i;
                    j++;
                }
            }

            height[source] = NODES;
            excess[source] = INFINITE;
            for (int i = 0; i < NODES; i++)
                Push(C, F, excess, source, i);

            int p = 0;
            while (p < NODES - 2)
            {
                int u = list[p];
                float old_height = height[u];
                Discharge(C, F, excess, height, seen, u);
                if (height[u] > old_height)
                {
                    MoveToFront(p, list);
                    p = 0;
                }
                else
                {
                    p += 1;
                }
            }

            float maxflow = 0;
            for (int i = 0; i < NODES; i++)
                maxflow += F[source, i];

            return maxflow;
        }
    }
}
