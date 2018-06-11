using System;

namespace Common.GraphTheory
{
    public class CyclicGraphException : Exception
    {

        public CyclicGraphException(string msg) : base(msg)
        {

        }

    }
}
