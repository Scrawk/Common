using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

    public abstract class Function
    {
        public readonly double a;

        private Function m_derivative;

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

        public virtual bool IsOne()
        {
            return false;
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

        protected string SignToString(double x)
        {
            if (x < 0)
                return "-";
            else
                return "";
        }

        protected string RemoveOuterBrackets(string name)
        {
            int count = name.Length;
            if (name.Length < 2) return name;

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

        public Function Derivative(int order)
        {
            Function func = Copy();
            for (int i = 0; i < order; i++)
                func = func.Derivative();

            return func;
        }

        public abstract Function Derivative();

        public abstract Function AntiDerivative();

        public Function Tangent(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            if (m_derivative == null)
                m_derivative = Derivative();

            var a = m_derivative.Evalulate(x);
            var b = Evalulate(x);

            var sub = new SubFunc(new LinearFunc(), new ConstFunc(x));
            var prod = new ProductFunc(new ConstFunc(a), sub);
            return new SumFunc(prod, new ConstFunc(b));
        }

        public Function Normal(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            if (m_derivative == null)
                m_derivative = Derivative();

            var a = m_derivative.Evalulate(x);
            var b = Evalulate(x);

            var sub = new SubFunc(new LinearFunc(), new ConstFunc(x));
            var quot = new QuotientFunc(new ConstFunc(-1), new ConstFunc(a));
            var prod = new ProductFunc(quot, sub);
            return new SumFunc(prod, new ConstFunc(b));
        }

    }

}