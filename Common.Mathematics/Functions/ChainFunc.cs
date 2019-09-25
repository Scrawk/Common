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

		public override string ToString(bool outerBrackets)
		{
			return string.Format("g({0}, h({1}))", g.ToString(true), h.ToString(false));
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
			var chain = new ChainFunc(g.Derivative(), h.Copy());
			return new ProductFunc(h.Derivative(), chain);
		}

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }
    }

}














