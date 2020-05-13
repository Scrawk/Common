using System;
using System.Collections.Generic;

using Common.Collections.Queues;
using Common.Collections.Trees;
using Common.Console.Collections;

using CONSOLE = System.Console;


namespace Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            PriorityQueuePerformanceTest.Run();

            CONSOLE.ReadLine();
        }
    }
}
