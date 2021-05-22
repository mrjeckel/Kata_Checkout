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
        Dictionary<string, BOGO> specialBOGO = new Dictionary<string, BOGO>();
        Dictionary<string, NforX> specialNforX = new Dictionary<string, NforX>();

        public void AddItemValue(string name, ItemList inputList, double weight = 0)
        {
            if (inputList.Contains(name))
            {
                if (specialMarkDown.ContainsKey(name))
                {
                    double discount = 1 - (specialMarkDown[name] / 100);
                    customerTotal += inputList.GetValue(name, weight, discount);
                }
                else if (specialBOGO.ContainsKey(name))
                {
                    if (((specialBOGO[name].UsedCount % specialBOGO[name].BuyCount) < specialBOGO[name].GetCount)
                        && (specialBOGO[name].UsedCount >= specialBOGO[name].BuyCount))
                    {
                        double discount = 1 - (specialBOGO[name].MarkDown / 100);
                        customerTotal += inputList.GetValue(name, weight, discount);

                        specialBOGO[name].Inc();
                    }
                    else
                    {
                        customerTotal += inputList.GetValue(name, weight);
                        specialBOGO[name].Inc();
                    }
                }
                else
                    customerTotal += inputList.GetValue(name, weight);
            }
                
            else
                throw new KeyNotFoundException($"{name} was not found in inventory.");
                
        }

        public void SubtractItemValue(string name, ItemList inputList, double weight = 0)
        {
            if (inputList.Contains(name))
            {
                if (specialMarkDown.ContainsKey(name))
                {
                    double discount = 1 - (specialMarkDown[name] / 100);
                    customerTotal -= inputList.GetValue(name, weight, discount);
                }
                else
                    customerTotal -= inputList.GetValue(name, weight);
            }
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
            else if (specialMarkDown.ContainsKey(nameIn))
                throw new ArgumentException($"A markdown for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialBOGO.ContainsKey(nameIn))
                throw new ArgumentException($"A BOGO for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialMarkDown.ContainsKey(nameIn))
                throw new ArgumentException($"A mark-down for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialNforX.ContainsKey(nameIn))
                throw new ArgumentException($"An N for X special for {nameIn} already exists. Remove this entry before creating a new one.");
            else
                specialMarkDown.Add(nameIn, markDownIn);
        }

        public void AddBOGO(string nameIn, double buyCountIn, double getCountIn, double markDownIn, double buyLimitIn = 0)
        {
                if (specialBOGO.ContainsKey(nameIn))
                    throw new ArgumentException($"A BOGO for {nameIn} already exists. Remove this entry before creating a new one.");
                else if (specialMarkDown.ContainsKey(nameIn))
                    throw new ArgumentException($"A mark-down for {nameIn} already exists. Remove this entry before creating a new one.");
                else if (specialNforX.ContainsKey(nameIn))
                    throw new ArgumentException($"An N for X special for {nameIn} already exists. Remove this entry before creating a new one.");
                else
                    specialBOGO.Add(nameIn, new BOGO(nameIn, buyCountIn, getCountIn, markDownIn, buyLimitIn));
        }

        public void AddNforX(string nameIn, double getCountIn, double getPriceIn, double buyLimitIn = 0)
        {
            if (specialBOGO.ContainsKey(nameIn))
                throw new ArgumentException($"A BOGO for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialMarkDown.ContainsKey(nameIn))
                throw new ArgumentException($"A mark-down for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialNforX.ContainsKey(nameIn))
                throw new ArgumentException($"An N for X special for {nameIn} already exists. Remove this entry before creating a new one.");
            else
                specialNforX.Add(nameIn, new NforX(nameIn, getCountIn, getPriceIn, buyLimitIn));
        }

        public void RemoveMarkDown(string nameIn)
        {
            if (specialMarkDown.ContainsKey(nameIn))
                specialMarkDown.Remove(nameIn);
            else
                throw new ArgumentException($"No mark-down was found for {nameIn}.");
        }

        public void RemoveBOGO(string nameIn)
        {
            if (specialBOGO.ContainsKey(nameIn))
                    specialBOGO.Remove(nameIn);
            else
                throw new ArgumentException($"No BOGO was found for {nameIn}.");
        }

        public void RemoveNforX(string nameIn)
        {
            if (specialNforX.ContainsKey(nameIn))
                    specialNforX.Remove(nameIn);
            else
                throw new ArgumentException($"No NforX was found for {nameIn}.");
        }

        public decimal CustomerTotal
        {
            get { return customerTotal;  }
        }
    }
}
