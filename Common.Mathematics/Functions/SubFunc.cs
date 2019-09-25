using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public class SubFunc : CompositeFunc
    {

        public SubFunc(Function g, Function h)
        {
            Functions = new List<Function>(2);
            Functions.Add(g, h);
        }

        public SubFunc(Function g, Function h, Function i)
        {
            Functions = new List<Function>(3);
            Functions.Add(g, h, i);
        }

        public SubFunc(IList<Function> functions)
        {
            if (functions.Count < 2)
                throw new ArgumentException("Sub function must be made from at less 2 functions.");

            Functions = new List<Function>(functions);
        }

        public override string ToString(string varibleName)
        {
            string str = "(";

            for (int i = 0; i < Count; i++)
            {
                if (i == 0)
                    str += RemoveOuterBrackets(Functions[i].ToString(varibleName));
                else
                    str += " - " + RemoveOuterBrackets(Functions[i].ToString(varibleName));
            }

            return str + ")";
        }

        public override Function Copy()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
                functions.Add(Functions[i].Copy());

            return new SubFunc(functions);
        }

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            double result = 0;

            for (int i = 0; i < Count; i++)
            {
                if(i == 0)
                    result = Functions[i].Evalulate(x);
                else
                    result -= Functions[i].Evalulate(x);
            }

            return result;
        }

        public override Function Derivative()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
            {
                var func = Functions[i].Derivative();
                if (func.a != 0)
                    functions.Add(func);
            }

            if (functions.Count == 0)
                return new ConstFunc(0);
            else if (functions.Count == 1)
                return functions[0];
            else
                return new SubFunc(functions);
        }

        public override Function AntiDerivative()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
                functions.Add(Functions[i].AntiDerivative());

            return new SubFunc(functions);
        }

    }

}














