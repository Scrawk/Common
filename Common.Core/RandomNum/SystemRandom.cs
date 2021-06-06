using System;
using System.Collections.Generic;

namespace Common.Core.RandomNum
{
    public class SystemRandom : RandomGenerator
    {

        private System.Random Rnd { get; set; }

        public SystemRandom()
        {

        }

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

        /// <summary>
        /// A random int between 0 - MaxInt.
        /// </summary>
        public override int Next()
        {
            return Rnd.Next();
        }

        /// <summary>
        /// Update seed.
        /// Called when seed changes.
        /// </summary>
        public override void UpdateSeed(int seed)
        {
            Seed = seed;
            Rnd = new System.Random(seed);
        }
        
    }
}
