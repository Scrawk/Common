using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public class LogChainFunc : Function
    {

        private Function m_func;

        public LogChainFunc(ProductFunc product) : base(1)
        {
            var functions = new List<Function>();
            Simplify(product, functions, 1);
            m_func = new SumFunc(functions);
        }

        public LogChainFunc(QuotientFunc quotient) : base(1)
        {
            var functions = new List<Function>();
            Simplify(quotient, functions, 1);
            m_func = new SumFunc(functions);
        } 

        public LogChainFunc(PowFunc pow) : base(1)
        {
            var functions = new List<Function>();
            Simplify(pow, functions, 1);
            m_func = new SumFunc(functions);
        }

        private void Simplify(Function func, List<Function> functions, int sign)
        {
            if (func is ProductFunc)
            {
                var prod = func as ProductFunc;
                foreach (var h in prod.ToList())
                    Simplify(h, functions, sign);
            }
            else if (func is QuotientFunc)
            {
                var quotient = func as QuotientFunc;
                Simplify(quotient.G, functions, sign);
                Simplify(quotient.H, functions, sign * -1);
            }
            else if(func is PowFunc)
            {
                var pow = func as PowFunc;

                if (pow.a != 1)
                    Simplify(new ConstFunc(pow.a), functions, 1);

                var chain = new ChainFunc(new LogFunc(), new LinearFunc());
                functions.Add(new ProductFunc(new ConstFunc(pow.n), chain));
            }
            else if(func is ChainFunc)
            {
                var chain = func as ChainFunc;
                var g = chain.G;
                var h = chain.H;

                if(g is PowFunc)
                {
                    var pow = g as PowFunc;

                    if (pow.a != 1)
                        Simplify(new ConstFunc(pow.a), functions, 1);

                    var chain2 = new ChainFunc(new LogFunc(), h);
                    functions.Add(new ProductFunc(new ConstFunc(pow.n), chain2));
                }
            }
            else
            {
                functions.Add(new ChainFunc(new LogFunc(sign), func));
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














