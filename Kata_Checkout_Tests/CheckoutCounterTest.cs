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

        [TestCase("apple")]
        [TestCase("apple", "BOGO")]
        [TestCase("apple", "NforX")]
        public void SpecialMarkDown_DuplicateException(string name1, string type = "")
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            if (type == "BOGO")
                testCounter.AddBOGO(name1, 2, 1, 1);
            else if (type == "NforX")
                testCounter.AddNforX(name1, 2, 1);
            else
                testCounter.AddMarkDown(name1, 1);

            Assert.Throws<ArgumentException>(() => testCounter.AddMarkDown(name1, 1));
        }

        [TestCase("", 1, 1, 100)]
        [TestCase("apple", -1, 1, 1)]
        [TestCase("apple", 1, -1, 1)]
        [TestCase("apple", 1, 1, -1)]
        [TestCase("apple", 1, 1, -1, 3)]
        public void AddBOGO_Exception(string name1, double buyCount1, double getCount1, double markDown1, double buyLimit1 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<ArgumentException>(() => testCounter.AddBOGO(name1, buyCount1, getCount1, markDown1, buyLimit1));
        }

        [TestCase("apple", "MarkDown")]
        [TestCase("apple", "NforX")]
        [TestCase("apple")]
        public void AddBOGO_DuplicateException(string name1, string type = "")
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            if (type == "MarkDown")
                testCounter.AddMarkDown(name1, 1);
            else if (type == "NforX")
                testCounter.AddNforX(name1, 2, 1);
            else
                testCounter.AddBOGO(name1, 1, 1, 1);

            Assert.Throws<ArgumentException>(() => testCounter.AddBOGO(name1, 1, 1, 1));
        }

        [TestCase("", 1, 1)]
        [TestCase("apple", -1, 1)]
        [TestCase("apple", 1, -1)]
        [TestCase("apple", 1, 1, -1)]
        public void AddNforX_Exception(string name1, double getCount1, double getPrice1, double buyLimit1 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<ArgumentException>(() => testCounter.AddNforX(name1, getCount1, getPrice1, buyLimit1));
        }

        [TestCase("apple")]
        [TestCase("apple", "MarkDown")]
        [TestCase("apple", "BOGO")]
        public void AddNforX_DuplicateException(string name1, string type = "")
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            if (type == "MarkDown")
                testCounter.AddMarkDown(name1, 1);
            else if (type == "BOGO")
                testCounter.AddBOGO(name1, 2, 1, 1);
            else
                 testCounter.AddNforX(name1, 1, 1);

            Assert.Throws<ArgumentException>(() => testCounter.AddNforX(name1, 1, 1));
        }

        [TestCase("apple", 50)]
        public void RemoveMarkDown_Success(string name1, double markDown1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddMarkDown(name1, markDown1);
            testCounter.RemoveMarkDown(name1);
        }

        [TestCase("apple")]
        public void RemoveMarkDown_Exception(string name1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<ArgumentException>(() => testCounter.RemoveMarkDown(name1));
        }

        [TestCase("apple", 2, 1, 50)]
        public void RemoveBOGO_Success(string name1, double buyCount1, double getCount1, double markDown1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddBOGO(name1, buyCount1, getCount1, markDown1);
            testCounter.RemoveBOGO(name1);
        }

        [TestCase("apple")]
        public void RemoveBOGO_Exception(string name1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<ArgumentException>(() => testCounter.RemoveBOGO(name1));
        }

        [TestCase("apple", 2, 1)]
        public void RemoveNforX_Success(string name1, double getCount1, double getPrice1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddNforX(name1, getCount1, getPrice1);
            testCounter.RemoveNforX(name1);
        }

        [TestCase("apple")]
        public void RemoveNforX_Exception(string name1)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            Assert.Throws<ArgumentException>(() => testCounter.RemoveNforX(name1));
        }

        [TestCase("apple", 1.00, "orange", 1.50, 2.00)]
        [TestCase("apple", 0.01, "orange", 0.01, 0.01)]
        [TestCase("apple", 0.70, "orange", 21.30, 21.66)]
        [TestCase("apple", 100.67, "orange", 406.01, 456.35)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 2506.66)]
        [TestCase("apple", 1.00, "orange", 1.50, 4.00, true, 5)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 4519.94, true, 5)]
        [TestCase("apple", 1.00, "orange", 1.50, 10.00, true, 5, true, 5)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 12533.30, true, 5, true, 5)]
        public void AddItemValue_MarkDown_Success(string name1, double val1, string name2, double val2, double sum,
            bool weighted1 = false, double weight1 = 0, bool weighted2 = false, double weight2 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddMarkDown(name1, 75);

            testList.AddItem(name1, val1, weighted1);
            testList.AddItem(name2, val2, weighted2);
            testCounter.AddItemValue(name1, testList, weight1);
            testCounter.AddItemValue(name1, testList, weight1);
            testCounter.AddItemValue(name2, testList, weight2);

            Assert.AreEqual(sum, testCounter.CustomerTotal);
        }

        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 4.25)]
        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 4.75, true, 3)]
        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 8.75, true, 3, true, 2, true, 2)]
        public void SubtractItemValue_MarkDown_Success(string name1, double val1, string name2, double val2, string name3, double val3, double sum,
            bool weighted1 = false, double weight1 = 0, bool weighted2 = false, double weight2 = 0, bool weighted3 = false, double weight3 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddMarkDown(name1, 75);

            testList.AddItem(name1, val1, weighted1);
            testList.AddItem(name2, val2, weighted2);
            testList.AddItem(name3, val3, weighted3);
            testCounter.AddItemValue(name1, testList, weight1);
            testCounter.AddItemValue(name1, testList, weight1);
            testCounter.AddItemValue(name2, testList, weight2);
            testCounter.AddItemValue(name3, testList, weight3);
            testCounter.SubtractItemValue(name1, testList, weight1);

            Assert.AreEqual(sum, testCounter.CustomerTotal);
        }

        [TestCase("apple", 1.00, "orange", 1.50, 3.75, 1, 2, 75)]
        [TestCase("apple", 1.00, "orange", 1.50, 6.00, 2, 4, 75)]
        [TestCase("apple", 1.00, "orange", 1.50, 15.00, 2, 4, 75, true, 3)]
        [TestCase("apple", 1.00, "orange", 1.50, 21.00, 2, 4, 75, true, 3, true, 5)]
        [TestCase("apple", 100.67, "orange", 406.01, 1030.17, 3, 5, 60)]
        [TestCase("apple", 100.67, "orange", 406.01, 2278.49, 3, 5, 60, true, 3)]
        [TestCase("apple", 100.67, "orange", 406.01, 3902.53, 3, 5, 60, true, 3, true, 5)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 5224.66, 2, 2, 40)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 8445.98, 2, 2, 40, true, 2)]
        [TestCase("apple", 1006.66, "orange", 2003.34, 12452.66, 2, 2, 40, true, 2, true, 3)]
        public void AddItemValue_BOGO_Success(string name1, double val1, string name2, double val2, double sum, double getCountIn, double buyCountIn,
            double markDownIn, bool weighted1 = false, double weight1 = 0, bool weighted2 = false, double weight2 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddBOGO(name1, buyCountIn, getCountIn, markDownIn);

            testList.AddItem(name1, val1, weighted1);
            testList.AddItem(name2, val2, weighted2);

            for (int i = 0; i < getCountIn+buyCountIn; i++)
            {
                testCounter.AddItemValue(name1, testList, weight1);
            }
            
            testCounter.AddItemValue(name2, testList, weight2);

            Assert.AreEqual(sum, testCounter.CustomerTotal);
        }

        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 6.00, 1, 2, 75)]
        [TestCase("apple", 1.00, "orange", 1.50, "banana", 2.50, 8.25, 2, 4, 75, 3)]
        [TestCase("apple", 1006.66, "orange", 2003.34, "banana", 2.50, 7240.48, 2, 2, 40, 3, true, 2)]
        [TestCase("apple", 1006.66, "orange", 2003.34, "banana", 2.50, 11247.16, 2, 2, 40, 3, true, 2, true, 3)]
        public void SubtractItemValue_BOGO_Success(string name1, double val1, string name2, double val2, string name3, double val3, double sum,
                double getCountIn, double buyCountIn, double markDownIn, int iterations = 1, bool weighted1 = false, double weight1 = 0, bool weighted2 = false, 
                double weight2 = 0, bool weighted3 = false, double weight3 = 0)
        {
            ItemList testList = new ItemList();
            CheckoutCounter testCounter = new CheckoutCounter();

            testCounter.AddBOGO(name1, buyCountIn, getCountIn, markDownIn);

            testList.AddItem(name1, val1, weighted1);
            testList.AddItem(name2, val2, weighted2);
            testList.AddItem(name3, val3, weighted3);

            //add items until we have (iterations) full specials in our basket
            for (int i = 0; i < (getCountIn + buyCountIn) * iterations; i++)
            {
                testCounter.AddItemValue(name1, testList, weight1);
            }

            //remove items until total added for special is 1 shy of buyCount + getCount (1 full special - 1 item)
            for (double i = (getCountIn + buyCountIn) * iterations; i > (getCountIn + buyCountIn) - 1; i--)
            {
                testCounter.SubtractItemValue(name1, testList, weight1);
            }

            testCounter.AddItemValue(name2, testList, weight2);
            testCounter.AddItemValue(name3, testList, weight3);


            Assert.AreEqual(sum, testCounter.CustomerTotal);

        }

    }
}
