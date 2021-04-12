using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Common.Core.Random;

namespace Common.Console.Core
{
    public static class RandomnessTest
    {

        public static void Run()
        {

            Run(new SystemRandom(), 10000000);
            Run(new LCHGenerator(), 10000000);
            Run(new MersenneTwister(), 10000000);
        }

        private static void Run(IRandomGenerator generator, int range, int count)
        {
            string name = generator.GetType().Name;

            string[] lines = new string[count];

            for (int i = 0; i < count; i++)
                lines[i] = generator.Next(range + 1).ToString();

            string filename = "C:/Users/Justin/Desktop/" + name + ".txt";
            File.WriteAllLines(filename, lines);
        }

        private static void Run(IRandomGenerator generator, int count)
        {
            string name = generator.GetType().Name;

            string[] lines = new string[count];

            for (int i = 0; i < count; i++)
                lines[i] = generator.Value.ToString();

            string filename = "C:/Users/Justin/Desktop/" + name + ".txt";
            File.WriteAllLines(filename, lines);
        }

    }
}
