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
            {
                if (specialMarkDown.ContainsKey(name))
                {
                    decimal temp = Convert.ToDecimal((specialMarkDown[name] / 100));
                    customerTotal += Convert.ToDecimal(inputList.GetValue(name, weight) * temp);
                }
                else
                    customerTotal += Convert.ToDecimal(inputList.GetValue(name, weight));
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
                    decimal temp = Convert.ToDecimal((specialMarkDown[name] / 100));
                    customerTotal -= Convert.ToDecimal(inputList.GetValue(name, weight) * temp);
                }
                else
                    customerTotal -= Convert.ToDecimal(inputList.GetValue(name, weight));
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
                throw new ArgumentException($"A markdown for {nameIn} already exists. Delete this entry before creating a new one.");

            specialMarkDown.Add(nameIn, markDownIn);
        }

        public void AddBOGO(string nameIn, double buyCountIn, double getCountIn, double markDownIn, double buyLimitIn = 0)
        {
            nameIn.Trim();

            if (String.IsNullOrEmpty(nameIn))
                throw new ArgumentException($"Name can not be a null or empty value.");
            else if (buyCountIn <= 0)
                throw new ArgumentException($"Number of bought items in special must be greater than zero.");
            else if (getCountIn <= 0)
                throw new ArgumentException($"Number of marked-down items in special must be greater than zero.");
            else if (markDownIn <= 0)
                throw new ArgumentException($"Markdown value must be greater than zero.");      
            else if (buyLimitIn < 0)
                throw new ArgumentException($"Buy limit can not be negative.");
            foreach (BOGO x in specialBOGO)
            {
                if ((x.Name) == nameIn)
                    throw new ArgumentException($"A BOGO for {nameIn} already exists. Delete this entry before creating a new one.");

            }

            specialBOGO.Add(new BOGO(nameIn, buyCountIn, getCountIn, markDownIn, buyLimitIn));
        }

        public void AddNforX(string nameIn, double getCountIn, double getPriceIn, double buyLimitIn = 0)
        {
            nameIn.Trim();

            if (String.IsNullOrEmpty(nameIn))
                throw new ArgumentException($"Name can not be a null or empty value.");
            else if (getCountIn <= 0)
                throw new ArgumentException($"Number of marked-down items in special must be greater than zero.");
            else if (getPriceIn <= 0)
                throw new ArgumentException($"Price must be greater than zero.");
            else if (buyLimitIn < 0)
                throw new ArgumentException($"Buy limit can not be negative.");
            foreach (NforX x in specialNforX)
            {
                if ((x.Name) == nameIn)
                    throw new ArgumentException($"A BOGO for {nameIn} already exists. Delete this entry before creating a new one.");
            }

            specialNforX.Add(new NforX(nameIn, getCountIn, getPriceIn, buyLimitIn));
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
            for (int i  = 0; i < specialBOGO.Count; i++)
            {
                if (nameIn == specialBOGO[i].Name)
                {
                    specialBOGO.RemoveAt(i);
                    return;
                }
            }

            throw new ArgumentException($"No BOGO was found for {nameIn}.");
        }

        public void RemoveNforX(string nameIn)
        {
            for (int i = 0; i < specialNforX.Count; i++)
            {
                if (nameIn == specialNforX[i].Name)
                {
                    specialNforX.RemoveAt(i);
                    return;
                }
            }

            throw new ArgumentException($"No NforX was found for {nameIn}.");
        }

        public decimal CustomerTotal
        {
            get { return customerTotal;  }
        }
    }
}
