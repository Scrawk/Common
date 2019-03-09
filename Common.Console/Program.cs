using System;
using System.Collections.Generic;

using CONSOLE = System.Console;

using Common.Mathematics.Random;

namespace Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new SubtractiveGenerator(0);

            for (int i = 0; i < 100; i++)
            {
                CONSOLE.WriteLine(rnd.Next());
            }
        }
    }
}
