using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class ProductFunc : CompositeFunc
	{

        public ProductFunc(Function g, Function h)
            : this(new Function[] { g, h })
        {

        }

        public ProductFunc(Function g, Function h, Function i)
            : this(new Function[] { g, h, i })
        {

        }

        public ProductFunc(Function g, Function h, Function i, Function j)
            : this(new Function[] { g, h, i, j })
        {

        }

        public ProductFunc(IList<Function> functions)
        {
            Functions = new List<Function>();

            bool isZero = false;

            foreach (var func in functions)
            {
                if(func.IsZero())
                {
                    isZero = true;
                    break;
                }
            }

            if(!isZero && functions.Count != 0)
            {
                foreach (var func in functions)
                    if (!func.IsOne()) Functions.Add(func);

                if (Count == 0)
                    Functions.Add(new ConstFunc(1));
            }
        }

        public override string ToString(string varibleName)
		{
            if (Count == 0) return "0";

            string str = "(";

			for(int i = 0; i < Count; i++)
			{
				if(i == 0)
					str += Functions[i].ToString(varibleName);
				else
					str += " * " + Functions[i].ToString(varibleName);
			}

			return str + ")";
		}

        public override Function Copy()
        {
            var functions = new List<Function>(Count);
            for (int i = 0; i < Count; i++)
                functions.Add(Functions[i].Copy());

            return new ProductFunc(functions);
        }

        public override double Evalulate(double x)
		{
            if (Count == 0) return 0;

			double result = 1;

			for(int i = 0; i < Count; i++)
				result *= Functions[i].Evalulate(x);
	
			return result;
		}

		public override Function Derivative()
		{
			var functions1 = new List<Function>(Count);
			for(int i = 0; i < Count; i++)
			{
                bool isZero = false;
				var functions2 = new List<Function>(Count);
				for(int j = 0; j < Count; j++)
				{
                    Function func = null;

					if(i == j)
                        func = Functions[j].Derivative();
					else
						func = Functions[j].Copy();

                    if (func.IsZero())
                    {
                        isZero = true;
                        break;
                    }
                    else
                    {
                        functions2.Add(func);
                    }
                }

                if(!isZero)
				    functions1.Add(new ProductFunc(functions2));
			}

            if (functions1.Count == 0)
                return new ConstFunc(0);
            else if (functions1.Count == 1)
                return functions1[0];
            else
                return new SumFunc(functions1);
        }

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }

    }

}














