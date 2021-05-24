using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Checkout
{
    public class NforX
    {
        string name = "";
        double getCount = 0;
        double getPrice = 0;
        double buyLimit = 0;
        double usedCount = 0;

        public NforX(string name1, double getCount1, double getPrice1, double buyLimit1 = 0)
        {
            name1 = name1.Trim();
            if (String.IsNullOrEmpty(name1))
                throw new ArgumentException($"Name can not be a null or empty value.");
            else
                name = name1;

            if (getCount1 <= 0)
                throw new ArgumentException($"Number of items in special must be greater than zero.");
            else
                getCount = getCount1;

            if (getPrice1 <= 0)
                throw new ArgumentException($"Price must be greater than zero.");
            else
                getPrice = getPrice1;

            if (buyLimit1 > 0)
                buyLimit = buyLimit1;
            else if (buyLimit1 < 0)
                throw new ArgumentException($"Buy limit can not be negative.");
        }

        public void Inc(double i)
        {
                usedCount += i;
        }

        public void Dec(double i)
        {
            if (usedCount == 0)
                throw new InvalidOperationException("Number of items in NforX can not be less than zero.");
            usedCount -= i;
        }

        public string Name
        {
            get { return name; }
        }

        public double GetCount
        {
            get { return getCount; }
        }

        public double GetPrice
        {
            get { return getPrice; }
        }
        public double UsedCount
        {
            get { return usedCount; }
        }
        public double BuyLimit
        {
            get { return buyLimit; }
        }

    }
}
