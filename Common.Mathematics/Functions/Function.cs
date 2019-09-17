using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

    public abstract class Function
    {
        public readonly double a;

        public Function(double a)
        {
            if (!DMath.IsFinite(a))
                throw new ArgumentException("a must be finite.");

            this.a = a;
        }

        public virtual bool IsZero()
        {
            return a == 0;
        }

        public abstract Function Copy();

        public abstract bool IsUndefined(double x);

        public abstract double Evalulate(double x);

        public abstract Function Derivative();

        public abstract Function AntiDerivative();

    }

}