using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Kata_Checkout;

namespace Kata_Checkout_Tests
{
    class CheckoutCounterTest
    {
        [TestCase("apple")]
        public void AddItemValue_ThrowException(string name)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<KeyNotFoundException>(() => testCounter.AddItemValue(name, testList));
        }

        [TestCase("apple", 1.00, "orange", 1.50, 2.50)]
        [TestCase("apple", 0.01, "orange", 0.01, 0.02)]
        [TestCase("apple", 0.70, "orange", 21.30, 22.00)]
        [TestCase("apple", 100.67, "orange", 406.01, 506.68)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 3010)]
        [TestCase("apple", 1.00, "orange", 1.50, 6.50, true, 5)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 7036.64, true, 5)]
        [TestCase("apple", 1.00, "orange", 1.50, 12.50, true, 5, true, 5)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 15050, true, 5, true, 5)]
        public void AddItemValue_Success(string name1, double val1, string name2, double val2, double sum, bool weighted1 = false, 
            double weight1 = 0, bool weighted2 = false, double weight2 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testList.AddItem(name1, val1, weighted1);
            testList.AddItem(name2, val2, weighted2);
            testCounter.AddItemValue(name1, testList, weight1);
            testCounter.AddItemValue(name2, testList, weight2);

            Assert.AreEqual(sum, testCounter.CustomerTotal);
        }

        [TestCase("apple")]
        public void SubtractItemValue_ThrowException(string name)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<KeyNotFoundException>(() => testCounter.SubtractItemValue(name, testList));
        }

        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 4.00)]
        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 4.00, true, 3)]
        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 8.00, true, 3, true, 2, true, 2)]
        public void SubtractItemValue_Success(string name1, double val1, string name2, double val2, string name3, double val3, double sum, 
            bool weighted1 = false, double weight1 = 0, bool weighted2 = false, double weight2 = 0, bool weighted3 = false, double weight3 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testList.AddItem(name1, val1, weighted1);
            testList.AddItem(name2, val2, weighted2);
            testList.AddItem(name3, val3, weighted3);
            testCounter.AddItemValue(name1, testList, weight1);
            testCounter.AddItemValue(name2, testList, weight2);
            testCounter.AddItemValue(name3, testList, weight3);
            testCounter.SubtractItemValue(name1, testList, weight1);

            Assert.AreEqual(sum, testCounter.CustomerTotal);
        }

        [TestCase("", 25)]
        [TestCase("apple", -1)]
        public void AddSpecialMarkDown_Exception(string name1, double markDown1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<ArgumentException>(() => testCounter.AddMarkDown(name1, markDown1));
        }
    }
}
