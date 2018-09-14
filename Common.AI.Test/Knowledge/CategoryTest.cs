using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.AI.Knowledge;

namespace Common.AI.Test
{
    [TestClass]
    public class AI_Knowledge_CategoryTest
    {
        [TestMethod]
        public void Contains()
        {
            Category food = new Category("Food");

            food.Add("Apple");
            food.Add("Pear");
            food.Add("Bacon");

            Assert.IsTrue(food.Contains("Apple"));
            Assert.IsTrue(food.Contains("Bacon"));
            Assert.IsTrue(food.Contains("Pear"));
            Assert.IsFalse(food.Contains("Oil"));
        }

        [TestMethod]
        public void Subset()
        {
            SubCategory food = new SubCategory("Food");
            Category meat = new Category("Meat");
            Category fruit = new Category("Fruit");

            fruit.Add("Apple");
            fruit.Add("Pear");
            meat.Add("Bacon");

            food.Add(fruit);
            food.Add(meat);

            Assert.IsTrue(food.Contains("Apple"));
            Assert.IsTrue(food.Contains("Bacon"));
            Assert.IsTrue(food.Contains("Pear"));
            Assert.IsFalse(food.Contains("Oil"));
        }

    }
}
