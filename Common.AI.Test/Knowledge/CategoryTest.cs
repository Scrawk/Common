using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.AI.Knowledge;

namespace Common.AI.Test
{
    [TestClass]
    public class AI_Knowledge_CategoryTest
    {
        [TestMethod]
        public void IsA()
        {

            Category food = new Category("Food");
            Category meat = new Category("Meat");
            Category fruit = new Category("Fruit");

            meat.Add(food);
            fruit.Add(food);

            Category apple = new Category("Apple");
            Category pear = new Category("Pear");
            Category bacon = new Category("Bacon");

            apple.Add(fruit);
            pear.Add(fruit);
            bacon.Add(meat);

            Assert.IsTrue(apple.IsA(food.Name));
            Assert.IsTrue(apple.IsA(fruit.Name));
            Assert.IsFalse(apple.IsA(meat.Name));
            Assert.IsTrue(apple.IsA(apple.Name));
            Assert.IsFalse(apple.IsA(pear.Name));

        }

    }
}
