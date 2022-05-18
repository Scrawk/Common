using System;
using System.Collections.Generic;

namespace Common.GraphTheory.MatrixGraphs
{
    public partial class MatrixGraph
    {
        //const int NODES = 6;
        const int INFINITE = 10000000;

        private static void Push(int[,] C, int[,] F, int[] excess, int u, int v)
        {
            int send = Math.Min(excess[u], C[u, v] - F[u, v]);
            F[u, v] += send;
            F[v, u] -= send;
            excess[u] -= send;
            excess[v] += send;
        }

        private static void Relabel(int[,] C, int[,] F, int[] height, int u)
        {
            int NODES = C.GetLength(0);
            int min_height = INFINITE;

            for (int v = 0; v < NODES; v++)
            {
                if (C[u, v] - F[u, v] > 0)
                {
                    min_height = Math.Min(min_height, height[v]);
                    height[u] = min_height + 1;
                }
            }
        }

        private static void Discharge(int[,] C, int[,] F, int[] excess, int[] height, int[] seen, int u)
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

        private static void MoveToFront(int i, int[] A)
        {
            int temp = A[i];

            for (int n = i; n > 0; n--)
            {
                A[n] = A[n - 1];
            }
            A[0] = temp;
        }

        public static int PushRelabel(int[,] C, int[,] F, int source, int sink)
        {
            int NODES = C.GetLength(0);

            int[] excess = new int[NODES];
            int[] height = new int[NODES];
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
                int old_height = height[u];
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

            int maxflow = 0;
            for (int i = 0; i < NODES; i++)
                maxflow += F[source, i];

            return maxflow;
        }
    }
}
