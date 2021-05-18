using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Kata_Checkout;

namespace Kata_Checkout_Tests
{
    class NforXTest
    {
        [TestCase("", 1, 1)]
        [TestCase("meat", 0, 1)]
        [TestCase("meat", 1, 0)]
        [TestCase("meat", 1, 1, -1)]
        public void CreateBOGO_ThrowException(string name, double getCount, double getPrice, double buyLimit = 0)
        {
            Assert.Throws<ArgumentException>(() => new NforX(name, getCount, getPrice, buyLimit));
        }
    }
}
