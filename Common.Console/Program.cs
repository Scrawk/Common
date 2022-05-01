using System;
using System.Collections.Generic;
using System.Threading;

using Common.Core.Numerics;

using Common.Core.Threading;

using CONSOLE = System.Console;

namespace Common.Console
{
    class Program
    {

        static void Main(string[] args)
        {


            var token = new ThreadingToken(100);
            token.TimePeriodFormat = Core.Time.TIME_PERIOD.SECONDS;
            token.StartTimer();

            for(int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);

                token.IncrementProgess();

                CONSOLE.WriteLine(token.EstimatedCompletionTime() + token.TimePeriodUnit);
            }



        }
    }
}
