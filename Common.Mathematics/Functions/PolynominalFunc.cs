using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

	public class PolynominalFunc : CompositeFunc
	{

		public PolynominalFunc(double a, double b, double c)
		{
            CreateFromConstants(new double[] { a, b, c });
        }

        public PolynominalFunc(double a, double b, double c, double d)
        {
            CreateFromConstants(new double[] { a, b, c, d});
        }

        public PolynominalFunc(double a, double b, double c, double d, double e)
        {
            CreateFromConstants(new double[] { a, b, c, d, e });
        }

        public PolynominalFunc(IList<double> constants)
        {
            CreateFromConstants(constants);
		}

        private void CreateFromConstants(IList<double> constants)
        {
            int degree = constants.Count - 1;

            Functions = new List<Function>(degree);
            for (int i = 0; i <= degree; i++)
            {
                if (i == degree)
                    Functions.Add(new ConstFunc(constants[i]));
                else
                    Functions.Add(new PowFunc(constants[i], degree - i));
            }
        }

        public int Degree => Functions.Count-1;

		public override string ToString(string varibleName)
		{
			string str = "(";
            bool first = true;

            for (int i = 0; i <= Degree; i++)
            {
                if (Functions[i].IsZero()) continue;

                if (first)
                {
                    str += Functions[i].ToString(varibleName);
                    first = false;
                }
                else
                {
                    var name = Functions[i].ToString(varibleName);

                    if (name.Length > 1 && name[0] == '-')
                        str += " - " + name.Substring(1);
                    else
                        str += " + " + name;
                }
            }

            return str + ")";
		}

		public override Function Copy()
		{
            var constants = new List<double>(Degree + 1);
            for (int i = 0; i <= Degree; i++)
                constants.Add(Functions[i].a);

            return new PolynominalFunc(constants);
		}

        public override double Evalulate(double x)
        {
            if (IsUndefined(x))
                throw new ArgumentException("Evalulate is undefined for x = " + x);

            double result = 0;

            for (int i = 0; i < Count; i++)
                result += Functions[i].Evalulate(x);

            return result;
        }

        public override Function Derivative()
		{
			if(Degree == 0)
			{
				return new ConstFunc(0);
			}
			else
			{
                var constants = new List<double>(Degree);
                for (int i = 0; i < Degree; i++)
                {
                    int n = Degree - i;
                    double a = Functions[i].a;
                    constants.Add(a * n);
                }

                return new PolynominalFunc(constants);
			}
		}

        public override Function AntiDerivative()
        {
            throw new NotImplementedException();
        }

    }

}














