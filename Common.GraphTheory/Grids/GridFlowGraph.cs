using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Directions;

namespace Common.GraphTheory.Grids
{
    public class GridFlowGraph
    {
        public const byte UNLABELED = 0;

        public const byte SOURCE = 1;

        public const byte SINK = 2;

        public float MaxFlow { get; private set; }

        public int VertexCount { get { return Width * Height; } }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public float[,,] Capacity { get; private set; }

        public float[,,] Flow { get; private set; }

        public byte[,] Label { get; private set; }

        public GridFlowGraph(int width, int height)
        {
            Width = width;
            Height = height;

            Capacity = new float[width, height, 8];
            Flow = new float[width, height, 8];
            Label = new byte[width, height];
        }

        public override string ToString()
        {
            return string.Format("[GridFlowGraph: VertexCount={0}, MaxFlow={1}, Width={2}, Height={3}]",
                VertexCount, MaxFlow, Width, Height);
        }

        public void Clear()
        {
            Array.Clear(Capacity, 0, Capacity.Length);
            Array.Clear(Flow, 0, Flow.Length);
            Array.Clear(Label, 0, Label.Length);
        }

        public void FordFulkersonMaxFlow()
        {
            MaxFlow = Searches.FordFulkersonGrid.MaxFlow(this);
            CalculateMinCut();
        }

        private void CalculateMinCut()
        {
            Queue<Vector2i> queue = new Queue<Vector2i>(VertexCount * 8);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] == SOURCE)
                        queue.Enqueue(new Vector2i(x, y));
                }
            }

            while (queue.Count > 0)
            {
                Vector2i u = queue.Dequeue();

                for (int i = 0; i < 8; i++)
                {
                    float residual = Capacity[u.x, u.y, i] - Flow[u.x, u.y, i];
                    if (residual <= 0) continue;

                    int xi = u.x + D8.OFFSETS[i, 0];
                    int yi = u.y + D8.OFFSETS[i, 1];

                    if (xi < 0 || xi >= Width) continue;
                    if (yi < 0 || yi >= Height) continue;
                    if (Label[xi, yi] == SOURCE) continue;

                    Label[xi, yi] = SOURCE;

                    queue.Enqueue(new Vector2i(xi, yi));
                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (Label[x, y] != SOURCE)
                        Label[x, y] = SINK;

                    for (int i = 0; i < 8; i++)
                        if (Flow[x, y, i] < 0) Flow[x, y, i] = 0;
                }
            }

        }
    }
}
