using System;
using System.Collections.Generic;


namespace Common.Mathematics.Functions
{

    public abstract class CompositeFunc : Function
    {

        protected List<Function> Functions;

        public CompositeFunc() : base(1)
        {

        }

        public int Count => Functions.Count;

        public override bool IsZero()
        {
            int count = 0;
            for (int i = 0; i < Count; i++)
                if (Functions[i].IsZero()) count++;

            return Count == count;
        }

        public override bool IsOne()
        {
            if (Count == 0) return false;

            for (int i = 0; i < Count; i++)
                if (!Functions[i].IsOne()) return false;

            return true;
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














