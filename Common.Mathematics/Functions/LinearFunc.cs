using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class LinearFunc : Function
	{

        public LinearFunc() : base(1)
        {

        }

        public LinearFunc(double a) : base(a)
		{

		}

		public override string ToString(string varibleName)
		{
            string A = ConstantToString(a);

            if (a == 1)
			    return string.Format("{0}", varibleName);
            else
                return string.Format("{0}{1}", A, varibleName);
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