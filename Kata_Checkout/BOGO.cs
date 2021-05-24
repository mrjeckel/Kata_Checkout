using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Checkout
{
    public class BOGO
    {
        string name = "";
        double buyCount = 0;
        double getCount = 0;
        double markDown = 0;
        double buyLimit = 0;
        double usedCount = 0;
        double specialCount = 0;

        public BOGO(string name1, double buyCount1, double getCount1, double markDown1, double buyLimit1 = 0)
        {
            name1 = name1.Trim();
            if (String.IsNullOrEmpty(name1))
                throw new ArgumentException($"Name can not be a null or empty value.");
            else
                name = name1;

            if (buyCount1 <= 0)
                throw new ArgumentException($"Number of bought items in special must be greater than zero.");
            else
                buyCount = buyCount1;

            if (getCount1 <= 0)
                throw new ArgumentException($"Number of marked-down items in special must be greater than zero.");
            else
                getCount = getCount1;

            if (markDown1 <= 0)
                throw new ArgumentException($"Markdown value must be greater than zero.");
            else
                markDown = markDown1;

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
                throw new InvalidOperationException("Number of items in BOGO can not be less than zero.");
            usedCount -= i;
        }

        public void SpecInc(double i)
        {
            specialCount += i;
        }

        public void SpecDec(double i)
        {
            if (specialCount == 0)
                throw new InvalidOperationException("Number of marked down items in BOGO can not be less than zero.");
            specialCount -= i;
        }

        public string Name
        {
            get { return name; }
        }
        public double BuyCount
        {
            get { return buyCount;  }
        }

        public double GetCount
        {
            get { return getCount; }
        }

        public double MarkDown
        {
            get { return markDown; }
        }

        public double BuyLimit
        {
            get { return buyLimit; }
        }
        public double UsedCount
        {
            get { return usedCount; }
        }
        public double SpecialCount
        {
            get { return specialCount; }
        }

    }
}
