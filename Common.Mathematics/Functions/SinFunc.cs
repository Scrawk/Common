using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{
    /// <summary>
    /// Function in the form a*sin(bx).
    /// </summary>
	public class SinFunc : Function
	{

		public readonly double b;


        /// <summary>
        /// Constructors.
        /// </summary>
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

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
        public override string ToString(string varibleName)
		{
            string A = ConstantToString(a);
            string B = ConstantToString(b);

            if (Math.Abs(a) == 1 && Math.Abs(b) == 1)
                return string.Format("{0}sin({1}{2})", SignToString(a), SignToString(b), RemoveOuterBrackets(varibleName));
            else if (Math.Abs(a) == 1)
                return string.Format("{0}sin({1}{2})", SignToString(a), B, varibleName);
            else if (Math.Abs(b) == 1)
                return string.Format("{0}sin({1}{2})", A, SignToString(b), RemoveOuterBrackets(varibleName));
            else
                return string.Format("{0}sin({1}{2})", A, B, varibleName);
        }

        /// <summary>
        /// Copy the function.
        /// </summary>
        public override Function Copy()
		{
			return new SinFunc(a, b);
		}

        /// <summary>
        /// Is the function undefined for the value x.
        /// </summary>
        public override bool IsUndefined(double x)
        {
            return !DMath.IsFinite(x);
        }

        /// <summary>
        /// Evalulate for the value x.
        /// </summary>
        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Sin(b * x);
		}

        /// <summary>
        /// Create the derivative function.
        /// </summary>
        public override Function Derivative()
		{
			return new CosFunc(a*b, b);
		}

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public override Function AntiDerivative()
		{
            return new CosFunc(-a/b, b);
		}

	}

}