using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Checkout
{
    public class CheckoutCounter
    {
        decimal customerTotal = 0;
        Dictionary<string, double> specialMarkDown = new Dictionary<string, double>();
        List<BOGO> specialBOGO = new List<BOGO>();
        List<NforX> specialNforX = new List<NforX>();

        public void AddItemValue(string name, ItemList inputList, double weight = 0)
        {
            if (inputList.Contains(name))
                customerTotal += Convert.ToDecimal(inputList.GetValue(name, weight));
            else
                throw new KeyNotFoundException($"{name} was not found in inventory.");
                
        }

        public void SubtractItemValue(string name, ItemList inputList, double weight = 0)
        {
            if (inputList.Contains(name))
                customerTotal -= Convert.ToDecimal(inputList.GetValue(name, weight));
            else
                throw new KeyNotFoundException($"{name} was not found in inventory.");
        }

        public void AddMarkDown(string nameIn, double markDownIn)
        {
            nameIn.Trim();

            if (String.IsNullOrEmpty(nameIn))
                throw new ArgumentException($"Name can not be a null or empty value.");
            else if (markDownIn <= 0)
                throw new ArgumentException($"Markdown value must be greater than zero.");

            specialMarkDown.Add(nameIn, markDownIn);
        }

        public void AddBOGO(string name1, double buyCountIn, double getCountIn, double markDownIn, double buyLimitIn = 0)
        {

        }

        public void AddXforN(string nameIn, double getCountIn, double getPriceIn, double buyLimitIn = 0)
        {

        }

        public decimal CustomerTotal
        {
            get { return customerTotal;  }
        }
    }
}
