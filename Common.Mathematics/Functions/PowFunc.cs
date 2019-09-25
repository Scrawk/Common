using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

    public class PowFunc : Function
    {

        public readonly double n;

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

        public override string ToString(bool outerBrackects)
        {
            string A = VaribleToString(a);

            if (a == 1 && n == 1)
                return string.Format("x");
            else if (a == 1)
                return string.Format("x^{0}", n);
            else if (n == 1)
                return string.Format("{0}x", A);
            else
                return string.Format("{0}x^{1}", A, n);
        }

        public override Function Copy()
        {
            return new PowFunc(a, n);
        }

        public override bool IsUndefined(double x)
        {
            if (x == 0 && n < 0) return true;
            if (x < 0 && Math.Floor(n) != n) return true;
            if (!DMath.IsFinite(x)) return true;

            return false;
        }

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a * Math.Pow(x, n);
        }

        public override Function Derivative()
        {
            if (n == 1)
                return new ConstFunc(a);
            else if(n == 2)
                return new LinearFunc(a * n);
            else
                return new PowFunc(a * n, n - 1);
        }

        public override Function AntiDerivative()
        {
            if (n == -1)
                return new LogFunc();
            else
                return new PowFunc(1.0 / (n + 1), n + 1);
        }

    }

}