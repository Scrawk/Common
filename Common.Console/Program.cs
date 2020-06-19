using System;
using System.Collections.Generic;

using Common.Console.Collections;
using Common.Console.Geometry;

using CONSOLE = System.Console;

namespace Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //PriorityQueuePerformanceTest.Run();

            ShapeCollectionPerformanceTest.Run();

            CONSOLE.ReadLine();
        }
    }
}
