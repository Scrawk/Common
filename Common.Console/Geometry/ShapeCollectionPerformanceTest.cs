using System;
using System.Collections.Generic;

using Common.Core.Time;
using Common.Geometry.Collections;
using Common.Geometry.Shapes;
using CONSOLE = System.Console;

namespace Common.Console.Geometry
{
    public static class ShapeCollectionPerformanceTest
    {
        private static int Range = 100;

        public static void Run()
        {
            Run(new ShapeCollection2f());
            Run(new BVHTree2f());
        }

        private static void Run(IShapeCollection2f collection)
        {
            double addTime = RunAddTest(collection, 10000);
            double containsTime = RunContainsTest(collection, 10000);
            double removeTime = RunRemoveTest(collection);

            CONSOLE.WriteLine(collection.ToString());
            CONSOLE.WriteLine("Add = " + addTime + "s");
            CONSOLE.WriteLine("Contains = " + containsTime + "s");
            CONSOLE.WriteLine("Remove = " + removeTime + "s");
            CONSOLE.WriteLine("");
        }

        private static double RunAddTest(IShapeCollection2f collection, int count)
        {

            var timer = new Timer();
            timer.Start();

            CreateRandomSegments(collection, count);

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunContainsTest(IShapeCollection2f collection, int count)
        {
            var rnd = new System.Random(0);

            var timer = new Timer();
            timer.Start();

            for (int i = 0; i < count; i++)
            {
                var p = rnd.NextVector2f(-Range, Range);

                collection.Contains(p);
            }

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunRemoveTest(IShapeCollection2f collection)
        {
            var list = collection.ToList();
            list.Shuffle(0);

            var timer = new Timer();
            timer.Start();

            foreach (var shape in list)
                collection.Remove(shape);

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static void CreateRandomSegments(IShapeCollection2f collection, int count)
        {
            var rnd = new System.Random(0);
       
            for (int i = 0; i < count; i++)
            {
                var a = rnd.NextVector2f(-Range, Range);
                var b = a + rnd.NextVector2f(-10, 10);

                var seg = new Segment2f(a, b);
                if (seg.Length < 0.1f) continue;

                collection.Add(seg);
            }
        }
    }
}
