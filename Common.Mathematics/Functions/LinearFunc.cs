using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{
    /// <summary>
    /// A function of the form ax.
    /// </summary>
	public class LinearFunc : Function
	{

        /// <summary>
        /// Constructors.
        /// </summary>
        public LinearFunc() : base(1)
        {

        }

        public LinearFunc(double a) : base(a)
		{

		}

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
        public override string ToString(string varibleName)
		{
            string A = ConstantToString(a);

            if (Math.Abs(a ) == 1)
			    return string.Format("{0}{1}", SignToString(a), varibleName);
            else
                return string.Format("{0}{1}", A, varibleName);
        }

        /// <summary>
        /// Copy the function.
        /// </summary>
        public override Function Copy()
		{
			return new LinearFunc(a);
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

            return a * x;
		}

        /// <summary>
        /// Create the derivative function.
        /// </summary>
        public override Function Derivative()
		{
			return new ConstFunc(a);
		}

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public override Function AntiDerivative()
		{
			return new PowFunc(0.5 * a, 2);
		}

	}

}