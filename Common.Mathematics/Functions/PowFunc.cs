using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{
    /// <summary>
    /// A function of the form ax^n.
    /// </summary>
    public class PowFunc : Function
    {
        /// <summary>
        /// The power of the function.
        /// </summary>
        public readonly double n;

        /// <summary>
        /// Constructors.
        /// </summary>
        public PowFunc(double n) : this(1, n)
        {

        }

        public PowFunc(double a, double n) : base(a)
        {
            if (!DMath.IsFinite(n))
                throw new ArgumentException("n must be finite.");

            if (n == 0)
                throw new ArgumentException("n must not be 0.");

            this.n = n;
        }

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
        public override string ToString(string varibleName)
        {
            string A = ConstantToString(a);

            if (Math.Abs(a) == 1 && n == 1)
                return string.Format("{0}{1}", SignToString(a), varibleName);
            else if (Math.Abs(a) == 1)
                return string.Format("{0}{2}^{1}", SignToString(a), n, varibleName);
            else if (n == 1)
                return string.Format("{0}{1}", A, varibleName);
            else
                return string.Format("{0}{2}^{1}", A, n, varibleName);
        }

        /// <summary>
        /// Copy the function.
        /// </summary>
        public override Function Copy()
        {
            return new PowFunc(a, n);
        }

        /// <summary>
        /// Is the function undefined for the value x.
        /// </summary>
        public override bool IsUndefined(double x)
        {
            if (x == 0 && n < 0) return true;
            if (x < 0 && Math.Floor(n) != n) return true;
            if (!DMath.IsFinite(x)) return true;

            return false;
        }

        /// <summary>
        /// Evalulate for the value x.
        /// </summary>
        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Pow(x, n);
        }

        /// <summary>
        /// Create the derivative function.
        /// </summary>
        public override Function Derivative()
        {
            if (n == 1)
                return new ConstFunc(a);
            else if(n == 2)
                return new LinearFunc(a * n);
            else
                return new PowFunc(a * n, n - 1);
        }

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public override Function AntiDerivative()
        {
            if (n == -1)
                return new LogFunc(a);
            else
                return new PowFunc(1.0 / (n + 1) * a, n + 1);
        }

    }

}