using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class LinearFunc : Function
	{

		public LinearFunc(double a) : base(a)
		{

		}

		public override string ToString()
		{
            if(a == 1)
			    return string.Format("x");
            else
                return string.Format("{0}x", a);
        }

		public override Function Copy()
		{
			return new LinearFunc(a);
		}

        public override bool IsUndefined(double x)
        {
            return !DMath.IsFinite(x);
        }

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * x;
		}

		public override Function Derivative()
		{
			return new ConstFunc(a);
		}

		public override Function AntiDerivative()
		{
			return new PowFunc(0.5, 2);
		}

	}

}