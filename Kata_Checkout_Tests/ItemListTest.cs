using System;
using NUnit.Framework;
using Kata_Checkout;

namespace Kata_Checkout_Tests
{
    class ItemListTest
    {
        [TestCase("",1.00)]
        [TestCase("apple", -1)]
        [TestCase(" ", 1.00)]
        [TestCase("apple", 0)]
        public void AddEmptyItem_ThrowException(string name1, double val1)
        {
            ItemList testList = new ItemList();

            Assert.Throws<ArgumentException>(() => testList.AddItem(name1, val1));
        }

        [TestCase("apple", 1.00)]
        public void AddDuplicateItem_ThrowException(string name1, double val1)
        {
            ItemList testList = new ItemList();

            testList.AddItem(name1, val1);

            Assert.Throws<ArgumentException>(() => testList.AddItem(name1, val1));
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("apple")]
        public void RemoveItem_ThrowException(string name)
        {
            ItemList testList = new ItemList();

            Assert.Throws<ArgumentException>(() => testList.RemoveItem(name));
        }

        [TestCase("meat", 1.00, true)]
        [TestCase("orange", 1.25)]
        public void AddRemoveItem_Success(string name, double val, bool weighted = false)
        {
            ItemList testList = new ItemList();

            try
            {
                testList.AddItem(name, val);
                testList.RemoveItem(name);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestCase("meat", 1.00, true)]
        public void CheckWeighted(string name, double val, bool weighted = false)
        {
            ItemList testList = new ItemList();

            testList.AddItem(name, val, weighted);

            if (!(testList.IsWeighted(name)))
                Assert.Fail();
        }

        [TestCase("apple", 1.00)]
        public void CheckContains(string name, double val)
        {
            ItemList testList = new ItemList();

            testList.AddItem(name, val);

            if (!(testList.Contains(name)))
                Assert.Fail();
        }

        [TestCase("apple", 1.00)]
        [TestCase("orange", 1.50)]
        public void CheckGetValue(string name, double val)
        {
            ItemList testList = new ItemList();

            testList.AddItem(name, val);

            Assert.AreEqual(val, testList.GetValue(name));
        }

        [TestCase("meat", 1.50, 2, 3.00)]
        [TestCase("meat", 1.66, 5, 8.30)]
        public void CheckGetValue_Weight(string name, double val, double weight, double sum)
        {
            ItemList testList = new ItemList();

            testList.AddItem(name, val, true);

            Assert.AreEqual(sum, testList.GetValue(name, weight));
        }

        [TestCase("meat", 1.50, 0, true)]
        [TestCase("meat", 1.50, 5, false)]
        [TestCase("meat", 1.50, -1, true)]
        [TestCase("meat", 1.50, -1, false)]
        public void CheckGetValue_ThrowException(string name, double val, double weight = 0, bool weighted = false)
        {
            ItemList testList = new ItemList();

            testList.AddItem(name, val, weighted);

            Assert.Throws<ArgumentException>(() => testList.GetValue(name, weight));
        }
    }
}
