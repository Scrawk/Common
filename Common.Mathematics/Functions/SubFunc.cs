using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{
    /// <summary>
    /// Function of the form f(x) = g(x) - h(x)...
    /// </summary>
    public class SubFunc : CompositeFunc
    {

        /// <summary>
        /// Constructors.
        /// </summary>
        public SubFunc(Function g, Function h)
            : this(new Function[] { g, h })
        {

        }

        public SubFunc(Function g, Function h, Function i)
            : this(new Function[] { g, h, i })
        {

        }

        public SubFunc(Function g, Function h, Function i, Function j)
            : this(new Function[] { g, h, i, j })
        {

        }

        public SubFunc(IList<Function> functions)
        {
            Functions = new List<Function>();

            for (int i = 0; i < functions.Count; i++)
            {
                var func = functions[i];
                //Dont include any functions that evalulate to 0
                //unless its the first.
                if (i == 0 || !func.IsZero()) Functions.Add(func);
            }
                
        }

        /// <summary>
        /// Convert to string where the varible name is x.
        /// </summary>
        public override string ToString(string varibleName)
        {
            if (Count == 0) return "0";

            string str = "(";

            for (int i = 0; i < Count; i++)
            {
                if (i == 0)
                    str += RemoveOuterBrackets(Functions[i].ToString(varibleName));
                else
                {
                    var name = RemoveOuterBrackets(Functions[i].ToString(varibleName));

                    if (name.Length > 1 && name[0] == '-')
                        str += " + " + name.Substring(1);
                    else
                        str += " - " + name;
                }

            }

            return str + ")";
        }

        /// <summary>
        /// Copy the function.
        /// </summary>
        public override Function Copy()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
                functions.Add(Functions[i].Copy());

            return new SubFunc(functions);
        }

        /// <summary>
        /// Evalulate for the value x.
        /// </summary>
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

        /// <summary>
        /// Create the derivative function.
        /// </summary>
        public override Function Derivative()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
            {
                var func = Functions[i].Derivative();
                //Dont include any functions that evalulate to 0
                //unless its the first.
                if (i == 0 || !func.IsZero())
                    functions.Add(func);
            }

            if (functions.Count == 0)
                return new ConstFunc(0);
            else if (functions.Count == 1)
                return functions[0];
            else
                return new SubFunc(functions);
        }

        /// <summary>
        /// Create the anti-derivative function.
        /// </summary>
        public override Function AntiDerivative()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
            {
                var func = Functions[i].AntiDerivative();
                //Dont include any functions that evalulate to 0 
                //unless its the first.
                if (i == 0 || !func.IsZero())
                    functions.Add(func);
            }

            if (functions.Count == 0)
                return new ConstFunc(0);
            else if (functions.Count == 1)
                return functions[0];
            else
                return new SubFunc(functions);
        }

    }

}














