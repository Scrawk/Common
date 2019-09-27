using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

	public class ChainFunc : Function
	{

		private Function g, h;

		public ChainFunc(Function g, Function h) : base(1)
		{
			this.g = g;
			this.h = h;
		}

		public override string ToString(string varibleName)
		{
            var x = RemoveOuterBrackets(h.ToString("x"));
            if (x != "x") x = "(" + x + ")";

            return string.Format("{0}", g.ToString(x));
		}

        public Function G => g.Copy();

        public Function H => h.Copy();

        public override bool IsZero()
        {
            return g.IsZero();
        }

        public override bool IsOne()
        {
            return g.IsOne();
        }

        public override Function Copy()
		{
			return new ChainFunc(g.Copy(), h.Copy());
		}

        public override bool IsUndefined(double x)
        {
            if (h.IsUndefined(x)) return true;
            if (g.IsUndefined(h.Evalulate(x))) return true;

            return false;
        }

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return g.Evalulate(h.Evalulate(x));
		}

		public override Function Derivative()
		{
            var func1 = h.Derivative();
            var func2 = new ChainFunc(g.Derivative(), h.Copy());

            if (func1.IsOne())
                return func2;
            else
			    return new ProductFunc(func1, func2);
		}

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }
    }

}














