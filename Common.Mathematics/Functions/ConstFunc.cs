using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{
    /// <summary>
    /// Function of the form a.
    /// </summary>
    public class ConstFunc : Function
    {

        /// <summary>
        /// Constructors.
        /// </summary>
        public ConstFunc(double a) : base(a)
        {

        }

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
        public override string ToString(string varibleName)
        {
            string A = ConstantToString(a);
            return string.Format("{0}", A);
        }

        /// <summary>
        /// Does this function always evalulate to 1.
        /// </summary>
        public override bool IsOne()
        {
            return a == 1;
        }

        /// <summary>
        /// Copy the function.
        /// </summary>
        public override Function Copy()
        {
            return new ConstFunc(a);
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

            return a;
        }

        /// <summary>
        /// Create the derivative function.
        /// </summary>
        public override Function Derivative()
        {
            return new ConstFunc(0);
        }

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public override Function AntiDerivative()
        {
            return new LinearFunc(a);
        }

    }

}