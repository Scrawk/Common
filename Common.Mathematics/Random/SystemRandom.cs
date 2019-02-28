using System;
using System.Collections.Generic;

namespace Common.Mathematics.Random
{
    public class SystemRandom : RandomGenerator
    {

        public override double Value { get { return Rnd.NextDouble(); } }

        private System.Random Rnd { get; set; }

        public SystemRandom(int seed) : base(seed)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[SystemRandom: Seed={0}]", Seed);
        }

        public override void UpdateSeed(int seed)
        {
            Seed = seed;
            Rnd = new System.Random(seed);
        }
        
    }
}
