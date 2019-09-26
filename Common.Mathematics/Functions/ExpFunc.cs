using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class ExpFunc : Function
	{

		public readonly double c;

        public ExpFunc() : this(1, 1)
        {

        }

        public ExpFunc(double c) : this(1, c)
		{

		}

		public ExpFunc(double a, double c) : base(a)
		{
            if (!DMath.IsFinite(c))
                throw new ArgumentException("c must be finite.");

            if (c == 0)
                throw new ArgumentException("c must not be 0.");

			this.c = c;
		}

		public override string ToString(string varibleName)
		{
            string A = ConstantToString(a);
            string C = ConstantToString(c);

            if (Math.Abs(a) == 1 && Math.Abs(c) == 1)
                return string.Format("{0}e^{1}{2}", SignToString(a), SignToString(c), varibleName);
            else if (Math.Abs(a) == 1)
                return string.Format("{0}e^{1}{2}", SignToString(a), C, varibleName);
            else if (Math.Abs(c) == 1)
                return string.Format("{0}e^{1}{2}", A, SignToString(c), varibleName);
            else
                return string.Format("{0}e^{1}{2}", A, C, varibleName);
        }

		public override Function Copy()
		{
			return new ExpFunc(a, c);
		}

        public override bool IsUndefined(double x)
        {
            return !DMath.IsFinite(x);
        }

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Exp(c*x);
		}

		public override Function Derivative()
		{
			return new ExpFunc(a * c, c);
		}

		public override Function AntiDerivative()
		{
			return new ExpFunc(a * (1.0/c), c);
		}

		public static ExpFunc FromBase(double a, double b)
		{
			return new ExpFunc(a, Math.Log(b));
		}

	}

}