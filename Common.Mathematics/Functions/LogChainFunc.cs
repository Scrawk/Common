using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public class LogChainFunc : Function
    {

        private Function m_func;

        public LogChainFunc(ProductFunc product) : base(1)
        {
            m_func = Simplify(product);
        }

        public LogChainFunc(QuotientFunc quotient) : base(1)
        {
            m_func = Simplify(quotient);
        } 

        public LogChainFunc(PowFunc pow) : base(1)
        {
            m_func = Simplify(pow);
        }

        private Function Simplify(Function func)
        {
            if(func is PowFunc)
            {
                var pow = func as PowFunc;
                var g = new ConstFunc(pow.n);
                var h = new ChainFunc(new LogFunc(), new LinearFunc());

                if (pow.a == 1)
                    return new ProductFunc(g, h);
                else
                {
                    var chain = new ChainFunc(new LogFunc(), new ConstFunc(pow.a));
                    return new SumFunc(chain, new ProductFunc(g, h));
                }
            }
            else if(func is QuotientFunc)
            {
                var quotient = func as QuotientFunc;
                var g = Simplify(quotient.G);
                var h = Simplify(quotient.H);

                return new SubFunc(g, h);
            }
            else if (func is ProductFunc)
            {
                var prod = func as ProductFunc;
                var functions = prod.ToList();
                var chain = new List<Function>();

                foreach (var h in functions)
                    chain.Add(Simplify(h.Copy()));

                return new SumFunc(chain);
            }
            else
            {
                return new ChainFunc(new LogFunc(), func.Copy());
            }
        }

        public override string ToString(string varibleName)
        {
            return m_func.ToString(varibleName);
        }

        public override Function Copy()
        {
            return m_func.Copy();
        }

        public override bool IsUndefined(double x)
        {
            if (m_func.IsUndefined(x)) return true;

            return false;
        }

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return m_func.Evalulate(x);
        }

        public override Function Derivative()
        {
            return m_func.Derivative();
        }

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }
    }

}














