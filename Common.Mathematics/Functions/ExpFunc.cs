using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

    /// <summary>
    /// A function of the form ae^cx.
    /// </summary>
	public class ExpFunc : Function
	{

		public readonly double c;

        /// <summary>
        /// Find b if function was in base form.
        /// </summary>
        public double b => Math.Pow(Math.E, c);

        /// <summary>
        /// Constructors.
        /// </summary>
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

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
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

        /// <summary>
        /// Copy the function.
        /// </summary>
		public override Function Copy()
		{
			return new ExpFunc(a, c);
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

            return a * Math.Exp(c*x);
		}

        /// <summary>
        /// Create the derivative function.
        /// </summary>
		public override Function Derivative()
		{
			return new ExpFunc(a * c, c);
		}

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public override Function AntiDerivative()
		{
			return new ExpFunc(a * (1.0/c), c);
		}

        /// <summary>
        /// Convert from the base form, ie ab^x to ae^cx
        /// </summary>
		public static ExpFunc FromBase(double a, double b)
		{
			return new ExpFunc(a, Math.Log(b));
		}

        /// <summary>
        /// After time d what amount is left where 
        /// c > 0 represents growth and c < 0 decay.
        /// </summary>
        public double Life(double d)
        {
            if(c < 0)
                return -Math.Log(d) / c;
            else
                return Math.Log(d) / c;
        }

	}

}