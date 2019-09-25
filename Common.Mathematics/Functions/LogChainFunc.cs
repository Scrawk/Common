using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public class LogChainFunc : Function
    {

        private Function m_func;

        public LogChainFunc(ProductFunc product) : base(1)
        {
            var functions = product.ToList();
            var chain = new List<Function>();

            foreach (var h in functions)
                chain.Add(new ChainFunc(new LogFunc(), h.Copy()));

            m_func = new SumFunc(chain);
        }

        public override string ToString(string varibleName, bool addBrackets)
        {
            return m_func.ToString(varibleName, addBrackets);
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














