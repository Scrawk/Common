using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{
    /// <summary>
    /// Abstract function.
    /// </summary>
    public abstract class Function
    {
        /// <summary>
        /// The factor, ie ax.
        /// </summary>
        public readonly double a;

        /// <summary>
        /// If the derivative is needed keep a copy to reuse.
        /// </summary>
        private Function m_derivative, m_antiderivative;

        public Function(double a)
        {
            if (!MathUtil.IsFinite(a))
                throw new ArgumentException("a must be finite.");

            this.a = a;
        }

        /// <summary>
        /// If the function always evalulates to 0.
        /// </summary>
        public virtual bool IsZero()
        {
            return a == 0;
        }

        /// <summary>
        /// If the function always evalulates to 1.
        /// </summary>
        public virtual bool IsOne()
        {
            return false;
        }

        /// <summary>
        /// Represent know constants with their symbol.
        /// </summary>
        protected string ConstantToString(double x)
        {
            if (x == Math.PI)
                return "PI";
            else if (x == Math.E)
                return "E";
            else
                return x.ToString();
        }

        /// <summary>
        /// If varible is negative return its sign.
        /// </summary>
        protected string SignToString(double x)
        {
            if (x < 0)
                return "-";
            else
                return "";
        }

        /// <summary>
        /// Remove the outer brackets from this string if present.
        /// </summary>
        protected string RemoveOuterBrackets(string name)
        {
            int count = name.Length;
            if (name.Length < 2) return name;

            if (name[0] == '(' && name[count - 1] == ')')
                return name.Substring(1, count - 2);
            else
                return name;
        }

        /// <summary>
        /// Return as string.
        /// </summary>
        public override string ToString()
        {
            return RemoveOuterBrackets(ToString("x"));
        }

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
        public abstract string ToString(string varibleName);

        /// <summary>
        /// Copy the function.
        /// </summary>
        public abstract Function Copy();

        /// <summary>
        /// Is the function undefined for the value x.
        /// </summary>
        public abstract bool IsUndefined(double x);

        /// <summary>
        /// Evalulate for the value x.
        /// </summary>
        public abstract double Evalulate(double x);

        /// <summary>
        /// Create the derivative function of order n.
        /// </summary>
        public Function Derivative(int n)
        {
            Function func = Copy();
            for (int i = 0; i < n; i++)
                func = func.Derivative();

            return func;
        }

        /// <summary>
        /// Create the derivative function.
        /// </summary>
        public abstract Function Derivative();

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public abstract Function AntiDerivative();

        /// <summary>
        /// Create the tangent function.
        /// </summary>
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

        /// <summary>
        /// Create the normal function.
        /// </summary>
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

        /// <summary>
        /// Integrate a definite integral from a to b.
        /// </summary>
        public double Integrate(double a, double b)
        {
            if (m_antiderivative == null)
                m_antiderivative = AntiDerivative();

            if (m_antiderivative.IsUndefined(a))
                throw new ArgumentException("Antiderivative is undefined for a = " + a);

            if (m_antiderivative.IsUndefined(b))
                throw new ArgumentException("Antiderivative is undefined for b = " + b);

            return m_antiderivative.Evalulate(b) - m_antiderivative.Evalulate(a);
        }

    }

}