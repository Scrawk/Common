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

        protected string ConstantToString(double x)
        {
            if (x == Math.PI)
                return "PI";
            else if (x == Math.E)
                return "E";
            else
                return x.ToString();
        }

        protected string RemoveOuterBrackets(string name)
        {
            int count = name.Length;
            if (name.Length < count) return name;

            if (name[0] == '(' && name[count - 1] == ')')
                return name.Substring(1, count - 2);
            else
                return name;
        }

        public override string ToString()
        {
            return RemoveOuterBrackets(ToString("x"));
        }

        public abstract string ToString(string varibleName);

        public abstract Function Copy();

        public abstract bool IsUndefined(double x);

        public abstract double Evalulate(double x);

        public abstract Function Derivative();

        public abstract Function AntiDerivative();

    }

}