using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Kata_Checkout;

namespace Kata_Checkout_Tests
{
    class BOGOTest
    {
        [TestCase("", 1, 1, 25)]
        [TestCase("meat", 0, 1, 25)]
        [TestCase("meat", 1, 0, 25)]
        [TestCase("meat", 1, 1, 0)]
        [TestCase("meat", 1, 1, 1, -1)]
        public void CreateBOGO_ThrowException(string name, double buyCount, double getCount, double markDown, double buyLimit = 0)
        {
            Assert.Throws<ArgumentException>(() => new BOGO(name, buyCount, getCount, markDown, buyLimit));
        }
    }
}
