using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;
using Common.Core.Directions;
using Common.Core.Time;

using Common.GraphTheory.GridGraphs;

using CONSOLE = System.Console;

namespace Common.Console
{
    class Program
    {

        static void Main(string[] args)
        {
            var graph = new FlowGridGraph(128, 128);
            graph.IsOrthogonal = true;

            var rnd = new Random(0);

            graph.Iterate((x, y) =>
            {
                int capacity = rnd.Next(1, 255);
                graph.SetCapacity(x, y, capacity);
            });

            int width = 2;
            graph.SetLabelAndCapacityInPerimeter(width, FLOW_GRAPH_LABEL.SOURCE, 255);

            int offset = 16;
            var bounds = new Box2i(offset, offset, graph.Width - 1 - offset, graph.Height - 1 - offset);
            graph.SetLabelAndCapacityInBounds(bounds, FLOW_GRAPH_LABEL.SINK, 255);

            var timer = new Timer();
            timer.Start();

            float maxflow = graph.Calculate();

            timer.Stop();

            WriteLine("Max flow " + maxflow);
            WriteLine("Time " + timer.ElapsedMilliseconds);

        }

        static void WriteLine(object obj)
        {
            CONSOLE.WriteLine(obj?.ToString());
        }
    }
}
