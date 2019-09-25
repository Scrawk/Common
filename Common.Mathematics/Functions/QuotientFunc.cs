using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class QuotientFunc : Function
	{

		private Function g, h;

		public QuotientFunc(Function g, Function h) : base(1)
		{
			this.g = g;
			this.h = h;
		}

		public override string ToString(string varibleName, bool addBrackets)
		{
            if(addBrackets)
			    return string.Format("({0} / {1})", g.ToString(varibleName, true), h.ToString(varibleName, true));
            else
                return string.Format("{0} / {1}", g.ToString(varibleName, true), h.ToString(varibleName, true));
        }

		public override Function Copy()
		{
			return new QuotientFunc(g.Copy(), h.Copy());
		}

        public override bool IsUndefined(double x)
        {
            if (g.IsUndefined(x)) return true;
            if (h.IsUndefined(x)) return true;
            if (h.Evalulate(x) == 0) return true;

            return false;
        }

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            double a = g.Evalulate(x);
			double b = h.Evalulate(x);

			return a / b;
		}

		public override Function Derivative()
		{
			var a = new ProductFunc(g.Derivative(), h.Copy());
			var b = new ProductFunc(g.Copy(), h.Derivative());

			var com = new SubFunc(a, b);
			var prod = new ProductFunc(h.Copy(), h.Copy());

            if (prod.IsZero())
                throw new DivideByZeroException("denominator is zero");

            return new QuotientFunc(com, prod);
		}

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }
    }

}














