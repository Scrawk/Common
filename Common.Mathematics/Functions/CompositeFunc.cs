using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public abstract class CompositeFunc : Function
    {

        protected List<Function> Functions;

        public CompositeFunc() : base(1)
        {
            Functions = new List<Function>();
        }

        public CompositeFunc(Function g, Function h) : base(1)
        {
            Functions = new List<Function>(2);
            Functions.Add(g, h);
        }

        public CompositeFunc(Function g, Function h, Function i) : base(1)
        {
            Functions = new List<Function>(3);
            Functions.Add(g, h, i);
        }

        public CompositeFunc(IList<Function> functions) : base(1)
        {
            if (functions.Count < 2)
                throw new ArgumentException("Composite function must be made from at less 2 functions.");

            Functions = new List<Function>(functions);
        }

        public int Count => Functions.Count;

        public override bool IsZero()
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
                if (Functions[i].IsZero()) count++;

            return Count == count;
        }

        public override bool IsUndefined(double x)
        {
            for (int i = 0; i < Count; i++)
                if (Functions[i].IsUndefined(x)) return true;

            return false;
        }

        public List<Function> ToList()
        {
            var list = new List<Function>();

            foreach (var func in Functions)
                list.Add(func.Copy());

            return list;
        }

    }

}














