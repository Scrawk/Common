using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Arrays;

namespace Common.Collections.Test.Arrays
{
    [TestClass]
    public class FloatArray1Test
    {
        
        [TestMethod]
        public void GetClamped()
        {

            var array = new FloatArray1(10);
            array.Fill(x => x);

            //Console.WriteLine("Sum = " + array.Sum());

            int samples = 20;
            float sum = 0;

            for(int i = 0; i < samples; i++)
            {
                float u = i / (samples-1.0f);

                float x = array.GetClamped(u);
                sum += x;

                //Console.WriteLine("i = " + i + " u = " + u + " x = " + x);
            }

            //Console.WriteLine("Sum2 = " + sum);

        }
        
    }
}
