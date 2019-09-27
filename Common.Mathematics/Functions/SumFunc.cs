using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

	public class SumFunc : CompositeFunc
	{

        public SumFunc(Function g, Function h)
            : this(new Function[] { g, h })
        {

        }

        public SumFunc(Function g, Function h, Function i)
            : this(new Function[] { g, h, i })
        {

        }

        public SumFunc(Function g, Function h, Function i, Function j)
            : this(new Function[] { g, h, i, j })
        {

        }

        public SumFunc(IList<Function> functions)
        {
            Functions = new List<Function>();

            foreach (var func in functions)
                if (!func.IsZero()) Functions.Add(func);
        }

        public override string ToString(string varibleName)
		{
            if (Count == 0) return "0";

			string str = "(";

            for (int i = 0; i < Count; i++)
			{
				if(i == 0)
					str += RemoveOuterBrackets(Functions[i].ToString(varibleName));
				else
                {
                    var name = RemoveOuterBrackets(Functions[i].ToString(varibleName));

                    if(name.Length > 1 && name[0] == '-')
                        str += " - " + name.Substring(1);
                    else
                        str += " + " + name;
                }
					
			}

            return str + ")";
		}

		public override Function Copy()
		{
			var functions = new List<Function>(Count);
			for(int i = 0; i < Count; i++)
				functions.Add(Functions[i].Copy());

			return new SumFunc(functions);
		}

        public override double Evalulate(double x)
		{
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            double result = 0;

			for(int i = 0; i < Count; i++)
				result += Functions[i].Evalulate(x);
	
			return result;
		}

		public override Function Derivative()
		{
			var functions = new List<Function>(Count);
			for(int i = 0; i < Count; i++)
            {
                var func = Functions[i].Derivative();
                if(!func.IsZero())
                    functions.Add(func);
            }

            if (functions.Count == 0)
                return new ConstFunc(0);
            else if (functions.Count == 1)
                return functions[0];
            else
                return new SumFunc(functions);
		}

		public override Function AntiDerivative()
		{
			var functions = new List<Function>(Count);
			for(int i = 0; i < Count; i++)
				functions.Add(Functions[i].AntiDerivative());

			return new SumFunc(functions);
		}

	}

}














