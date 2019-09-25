using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

    public class ConstFunc : Function
    {

        public ConstFunc(double a) : base(a)
        {

        }

        public override string ToString(bool outerBrackects)
        {
            string A = VaribleToString(a);

            return string.Format("{0}", A);
        }
        public override Function Copy()
        {
            return new ConstFunc(a);
        }

        public override bool IsUndefined(double x)
        {
            return !DMath.IsFinite(x);
        }

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return a;
        }

        public override Function Derivative()
        {
            return new ConstFunc(0);
        }

        public override Function AntiDerivative()
        {
            return new ConstFunc(0);
        }

    }

}