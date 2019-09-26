using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public class LogChainFunc : Function
    {
        private Function m_func;

        private SumFunc m_logChain;

        public LogChainFunc(ProductFunc product) 
            : this(product as Function)
        {

        }

        public LogChainFunc(QuotientFunc quotient) 
            : this(quotient as Function)
        {

        }

        public LogChainFunc(PowFunc pow)
            : this(pow as Function)
        {

        }

        private LogChainFunc(Function func) : base(1)
        {
            m_func = func;

            var functions = new List<Function>();
            Simplify(m_func, functions, 1);
            m_logChain = new SumFunc(functions);
        }

        private LogChainFunc(Function func, SumFunc logChain) : base(1)
        {
            m_func = func;
            m_logChain = logChain;
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
                Simplify(quotient.H, functions, -1);
            }
            else if(func is PowFunc)
            {
                var pow = func as PowFunc;

                if (pow.a != 1)
                    Simplify(new ConstFunc(pow.a), functions, sign);

                var chain = new ChainFunc(new LogFunc(), new LinearFunc());
                functions.Add(new ProductFunc(new ConstFunc(pow.n * sign), chain));
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
                        Simplify(new ConstFunc(pow.a), functions, sign);

                    var chain2 = new ChainFunc(new LogFunc(), h);
                    functions.Add(new ProductFunc(new ConstFunc(pow.n * sign), chain2));

                }
            }
            else if(func is ExpFunc)
            {
                var exp = func as ExpFunc;
                functions.Add(new LinearFunc(exp.a * exp.c));
            }
            else
            {
                functions.Add(new ChainFunc(new LogFunc(sign), func));
            }
        }

        public override string ToString(string varibleName)
        {
            return m_logChain.ToString(varibleName);
        }

        public override Function Copy()
        {
            return new LogChainFunc(m_func.Copy(), m_logChain.Copy() as SumFunc);
        }

        public override bool IsUndefined(double x)
        {
            if (m_logChain.IsUndefined(x)) return true;

            return false;
        }

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            return m_logChain.Evalulate(x);
        }

        public override Function Derivative()
        {
            var functions = new List<Function>();
            foreach (var func in m_logChain.ToList())
                functions.Add(func.Derivative());

            return new ProductFunc(m_func.Copy(), new SumFunc(functions));
        }

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }
    }

}














