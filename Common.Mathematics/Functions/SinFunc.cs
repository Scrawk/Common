using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class SinFunc : Function
	{

		public readonly double b;

        public SinFunc() : this(1, 1)
        {

        }

        public SinFunc(double b) : this(1, b)
		{

		}

		public SinFunc(double a, double b) : base(a)
		{
            if (!DMath.IsFinite(b))
                throw new ArgumentException("b must be finite.");

            if (b == 0)
                throw new ArgumentException("b must not be 0.");

			this.b = b;
		}

		public override string ToString(string varibleName, bool addBrackets)
		{
            string A = ConstantToString(a);
            string B = ConstantToString(b);

            if (a == 1 && b == 1)
                return string.Format("sin({0})", varibleName);
            else if (a == 1)
                return string.Format("sin({0}{1})", B, varibleName);
            else if (b == 1)
                return string.Format("{0}sin({1})", A, varibleName);
            else
                return string.Format("{0}sin({1}{2})", A, B, varibleName);
        }

		public override Function Copy()
		{
			return new SinFunc(a, b);
		}

        public override bool IsUndefined(double x)
        {
            return !DMath.IsFinite(x);
        }

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Sin(b * x);
		}

		public override Function Derivative()
		{
			return new CosFunc(a*b, b);
		}

		public override Function AntiDerivative()
		{
			return new CosFunc(-a * (1.0-b), b);
		}

	}

}