using System;
using System.Collections.Generic;

using Common.Core.Time;
using Common.Collections.Lists;

using CONSOLE = System.Console;

namespace Common.Console.Collections
{
    public static class DynamicListPerformanceTest
    {

        public static void Run()
        {
            Run(new DynamicList<object>());
        }

        private static void Run(IDynamicList<object> list)
        {
            var objects = CreateTestList(100000);

            double addTime = RunAddTest(list, objects);

            double accessTime = RunAccessTest(list);

            objects.Shuffle(0);
            double removeTime = RunRemoveTest(list, objects);

            CONSOLE.WriteLine(list.ToString());
            CONSOLE.WriteLine("Add = " + addTime + "s");
            CONSOLE.WriteLine("Access = " + accessTime + "s");
            CONSOLE.WriteLine("Remove = " + removeTime + "s");

            CONSOLE.WriteLine("");
        }

        private static double RunAddTest(IDynamicList<object> list, List<object> objects)
        {
            var timer = new Timer();
            timer.Start();

            list.AddRange(objects);

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunAccessTest(IDynamicList<object> list)
        {
            var timer = new Timer();
            timer.Start();

            for(int i = 0; i < list.Count; i++)
            {
                var obj = list[i];
            }
               
            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunRemoveTest(IDynamicList<object> list, List<object> objects)
        {
            var timer = new Timer();
            timer.Start();

            foreach(var obj in objects)
                list.Remove(obj);

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static List<object> CreateTestList(int count)
        {
            var list = new List<object>(count);

            for (int i = 0; i < count; i++)
                list.Add(new object());

            return list;
        }

    }
}
