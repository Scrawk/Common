using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Collections.DCEL
{

    public class InvalidDCELException : Exception
    {

        public InvalidDCELException() { }

        public InvalidDCELException(string msg) : base(msg) { }

    }

    public class EdgeNotClosedException : Exception
    {

        public EdgeNotClosedException() { }

        public EdgeNotClosedException(string msg) : base(msg) { }

    }

    public class BetweenEdgeNotFoundException : Exception
    {
        public BetweenEdgeNotFoundException() { }

        public BetweenEdgeNotFoundException(string msg) : base(msg) { }
    }
}
