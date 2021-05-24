using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Checkout
{
    public class SpecialMarkDown
    {
        string name = "";
        double markDown = 0;
        double buyLimit = 0;
        double usedCount = 0;

        public SpecialMarkDown(string name1, double markDown1, double buyLimit1 = 0)
        {
            name1 = name1.Trim();
            if (String.IsNullOrEmpty(name1))
                throw new ArgumentException($"Name can not be a null or empty value.");
            else
                name = name1;

            if (markDown1 <= 0)
                throw new ArgumentException($"Mark-down can not be negative.");
            else
                markDown = markDown1; ;

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
                throw new InvalidOperationException("Number of items in mark-down can not be less than zero.");
            usedCount -= i;
        }

        public string Name
        {
            get { return name; }
        }

        public double UsedCount
        {
            get { return usedCount; }
        }
        public double BuyLimit
        {
            get { return buyLimit; }
        }
        public double MarkDown
        {
            get { return markDown; }
        }


    }
}
