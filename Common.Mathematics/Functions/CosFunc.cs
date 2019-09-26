using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class CosFunc : Function
	{

        public readonly double b;

        public CosFunc() : this(1, 1)
        {

        }

        public CosFunc(double b) : this(1, b)
        {

        }

        public CosFunc(double a, double b) : base(a)
        {
            if (!DMath.IsFinite(b))
                throw new ArgumentException("b must be finite.");

            if (b == 0)
                throw new ArgumentException("b must not be 0.");

            this.b = b;
        }

        public override string ToString(string varibleName)
        {
            string A = ConstantToString(a);
            string B = ConstantToString(b);

            if (Math.Abs(a) == 1 && Math.Abs(b) == 1)
                return string.Format("{0}cos({1}{2})", SignToString(a), SignToString(b), RemoveOuterBrackets(varibleName));
            else if (Math.Abs(a) == 1)
                return string.Format("{0}cos({1}{2})", SignToString(a), B, varibleName);
            else if (Math.Abs(b) == 1)
                return string.Format("{0}cos({1}{2})", A, SignToString(b), RemoveOuterBrackets(varibleName));
            else
                return string.Format("{0}cos({1}{2})", A, B, varibleName);
        }

        public override Function Copy()
        {
            return new CosFunc(a, b);
        }

        public override bool IsUndefined(double x)
        {
            return !DMath.IsFinite(x);
        }

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Cos(b * x);
        }

        public override Function Derivative()
		{
			return new SinFunc(-a*b, b);
		}

		public override Function AntiDerivative()
		{
			return new SinFunc(a * (1.0-b), b);
		}

	}

}