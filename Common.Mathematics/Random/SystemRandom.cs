using System;
using System.Collections.Generic;

namespace Common.Mathematics.Random
{
    public class SystemRandom : RandomGenerator
    {

        private System.Random Rnd { get; set; }

        public SystemRandom(uint seed) : base(seed)
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
        public override void UpdateSeed(uint seed)
        {
            Seed = seed;
            Rnd = new System.Random((int)seed);
        }
        
    }
}
