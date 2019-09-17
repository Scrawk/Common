using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class LogFunc : Function
	{

		public readonly double b;

		public LogFunc() : this(1, Math.E)
        {
		}

		public LogFunc(double a) : this(a, Math.E)
		{

		}

		public LogFunc(double a, double b) : base(a)
		{
            if (!DMath.IsFinite(b))
                throw new ArgumentException("b must be finite.");

            if (b <= 1)
                throw new ArgumentException("b must be > 1.");

			this.b = b;
		}

		public override string ToString()
		{
            string B = b == Math.E ? "E" : b.ToString();

            if(a == 1)
			    return string.Format("log{0}(x)", B);
            else
                return string.Format("{0}log{1}(x)", a, B);
        }

		public override Function Copy()
		{
			return new LogFunc(a, b);
		}

        public override bool IsUndefined(double x)
        {
            if (x <= 0) return true;
            return !DMath.IsFinite(x);
        }

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Log(x, b);
		}

		public override Function Derivative()
		{
            var con = new ConstFunc(a);
			var lin	= new LinearFunc(Math.Log(b));
			
			return new QuotientFunc(con, lin);
		}

		public override Function AntiDerivative()
		{
            throw new NotImplementedException();
		}

	}

}