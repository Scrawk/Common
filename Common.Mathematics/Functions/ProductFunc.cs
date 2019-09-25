using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Functions
{

	public class ProductFunc : CompositeFunc
	{


		public ProductFunc(Function g, Function h) : base(g, h)
		{

		}

        public ProductFunc(Function g, Function h, Function i) : base(g, h, i)
        {

        }

        public ProductFunc(IList<Function> functions) : base(functions)
        {

		}

		public override string ToString(string varibleName)
		{
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














