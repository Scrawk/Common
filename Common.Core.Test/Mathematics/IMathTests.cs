﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Mathematics;

namespace Common.Core.Test.Mathematics
{
    [TestClass]
    public class Core_Mathematics_IMathTests
    {
        [TestMethod]
        public void Permutations()
        {
            Assert.AreEqual(1, IMath.Permutations(1));
            Assert.AreEqual(2, IMath.Permutations(2));
            Assert.AreEqual(6, IMath.Permutations(3));
            Assert.AreEqual(24, IMath.Permutations(4));
            Assert.AreEqual(120, IMath.Permutations(5));
        }

        [TestMethod]
        public void PermutationsOrderedWithRepeats()
        {
            //If you roll a six-sided dice twice, how many possible out comes are there?
            Assert.AreEqual(36, IMath.PermutationsOrderedWithRepeats(2, 6));

            //If you flip a coin 4 times, how many possible out comes are there?
            Assert.AreEqual(16, IMath.PermutationsOrderedWithRepeats(4, 2));
        }

        [TestMethod]
        public void PermutationsOrderedWithoutRepeats()
        {
            //How many different ordered pairs of people can be chosen from a group of 5 people.
            Assert.AreEqual(20, IMath.PermutationsOrderedWithoutRepeats(2,5));
        }

        [TestMethod]
        public void PermutationsUnorderedWithRepeats()
        {
            //Put 3 letters in a hat. If you draw out 4, putting each drawn letter 
            //back into the hat after a draw, how many unordered combinations could there be?
            Assert.AreEqual(15, IMath.PermutationsUnorderedWithRepeats(4, 3));
        }

        [TestMethod]
        public void PermutationsUnorderedWithoutRepeats()
        {
            //How many different unordered pairs of people can be chosen from a group of 5 people.
            Assert.AreEqual(10, IMath.PermutationsUnorderedWithoutRepeats(2, 5));

            //How many possible poker hands are possible in a deck of 52 cards?
            Assert.AreEqual(2598960, IMath.PermutationsUnorderedWithoutRepeats(5, 52));
        }

    }
}
