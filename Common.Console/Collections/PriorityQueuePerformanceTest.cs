using System;
using System.Collections.Generic;

using Common.Core.Time;
using Common.Collections.Queues;
using Common.Collections.Trees;

using CONSOLE = System.Console;

namespace Common.Console.Collections
{
    public class TestComparer : IComparable<TestComparer>
    {
        public int Value;

        public TestComparer(int value)
        {
            Value = value;
        }

        public int CompareTo(TestComparer other)
        {
            return Value.CompareTo(other.Value);
        }
    }

    public static class PriorityQueuePerformanceTest
    {

        public static void Run()
        {
            Run(new PriorityList<TestComparer>());

            Run(new PriorityLinkedList<TestComparer>());

            Run(new BinaryHeap<TestComparer>());

            Run(new BinaryTree<TestComparer>());

            Run(new AVLTree<TestComparer>());
        }

        private static void Run(IPriorityQueue<TestComparer> queue)
        {
            double addTime = RunAddTest(queue, 1000000);
            queue.Clear();

            double removeTime = RunRemoveTest(queue, 10000);
            queue.Clear();

            double removeFirstTime = RunRemoveFirstTest(queue, 10000);
            queue.Clear();

            double findPredecessorTime = RunFindPredecessorTest(queue, 10000);
            queue.Clear();

            CONSOLE.WriteLine(queue.ToString());
            CONSOLE.WriteLine("Add = " + addTime + "s");
            CONSOLE.WriteLine("Remove = " + removeTime + "s");
            CONSOLE.WriteLine("RemoveFirst = " + removeFirstTime + "s");
            CONSOLE.WriteLine("FindPredecessor = " + findPredecessorTime + "s");
            CONSOLE.WriteLine("");
        }

        private static double RunAddTest(IPriorityQueue<TestComparer> queue, int count)
        {
            var list = CreateUniqueTestList(count);

            var timer = new Timer();
            timer.Start();

            queue.Add(list);
            queue.Peek();

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunRemoveTest(IPriorityQueue<TestComparer> queue, int count)
        {
            var list = CreateUniqueTestList(count);
            queue.Add(list);

            var timer = new Timer();
            timer.Start();

            for (int i = 0; i < count; i++)
                queue.RemoveValue(new TestComparer(i));

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunRemoveFirstTest(IPriorityQueue<TestComparer> queue, int count)
        {
            var list = CreateUniqueTestList(count);
            queue.Add(list);

            var timer = new Timer();
            timer.Start();

            for (int i = 0; i < count; i++)
                queue.RemoveFirst();

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static double RunFindPredecessorTest(IPriorityQueue<TestComparer> queue, int count)
        {
            var list = CreateUniqueTestList(count);
            queue.Add(list);

            var timer = new Timer();
            timer.Start();

            for (int i = 0; i < count; i++)
                queue.FindPredecessor(new TestComparer(i), out _);

            timer.Stop();
            return timer.ElapsedSeconds;
        }

        private static List<TestComparer> CreateUniqueTestList(int count)
        {
            var list = new List<TestComparer>();

            for(int i = 0; i < count; i++)
            {
                list.Add(new TestComparer(i));
            }

            list.Shuffle(0);

            return list;
        }


    }
}
