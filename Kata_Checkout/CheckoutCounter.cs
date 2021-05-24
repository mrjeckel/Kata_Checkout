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
        Dictionary<string, SpecialMarkDown> specialMarkDown = new Dictionary<string, SpecialMarkDown>();
        Dictionary<string, BOGO> specialBOGO = new Dictionary<string, BOGO>();
        Dictionary<string, NforX> specialNforX = new Dictionary<string, NforX>();

        public void AddItemValue(string name, ItemList inputList, double weight = 0)
        {
            if (inputList.Contains(name))
            {
                if (specialMarkDown.ContainsKey(name))
                {
                    if ((specialMarkDown[name].UsedCount < specialMarkDown[name].BuyLimit) || (specialMarkDown[name].BuyLimit == 0))
                    {
                        double discount = 1 - (specialMarkDown[name].MarkDown / 100);

                        //if resultant usedCount will be greater than the buy limit, we have to calculate the split between eligibile and non-eligible weight
                        if (((weight + specialMarkDown[name].UsedCount) >= specialMarkDown[name].BuyLimit) && (specialMarkDown[name].BuyLimit > 0))
                        {
                            //buyLimit - usedCount yields the remainder that can be discounted
                            customerTotal += inputList.GetValue(name, (specialMarkDown[name].BuyLimit - specialMarkDown[name].UsedCount), discount);

                            //weight + usedCount - buyLimit yields remaining weight that needs to be calculated without any discount
                            if ((weight + specialMarkDown[name].UsedCount - specialMarkDown[name].BuyLimit) > 0)
                                customerTotal += inputList.GetValue(name, (weight + specialMarkDown[name].UsedCount - specialMarkDown[name].BuyLimit));
                        }
                        else
                            customerTotal += inputList.GetValue(name, weight, discount);

                    }
                    else
                        customerTotal += inputList.GetValue(name, weight);

                    if (weight == 0)
                        specialMarkDown[name].Inc(1);
                    else
                        specialMarkDown[name].Inc(weight);

                }
                else if (specialBOGO.ContainsKey(name))
                {
                    //Console.WriteLine($"used:{specialBOGO[name].UsedCount}, buy:{specialBOGO[name].BuyCount}, get:{specialBOGO[name].GetCount}, spec:{specialBOGO[name].SpecialCount}");

                    //Compare the special count against floored (used/buy)*get. This tells you how many marked down items you should have at any given point.
                    if ((specialBOGO[name].SpecialCount < (Math.Floor((specialBOGO[name].UsedCount/specialBOGO[name].BuyCount))*specialBOGO[name].GetCount)) &&
                          ((specialBOGO[name].UsedCount <= specialBOGO[name].BuyLimit) || (specialBOGO[name].BuyLimit == 0)))
                    {
                        double discount = 1 - (specialBOGO[name].MarkDown / 100);
                        customerTotal += inputList.GetValue(name, weight, discount);

                        specialBOGO[name].SpecInc(1);
                    }
                    else
                    {
                        customerTotal += inputList.GetValue(name, weight);
                        specialBOGO[name].Inc(1);
                    }
                }
                else if (specialNforX.ContainsKey(name))
                {
                    specialNforX[name].Inc(1);

                    //check if the current count is eligible for discount. <= is used because we preincrement here
                    if (((specialNforX[name].UsedCount % specialNforX[name].GetCount) == 0) && ((specialNforX[name].UsedCount <= specialNforX[name].BuyLimit) ||
                            (specialNforX[name].BuyLimit == 0)))
                    {
                        for (int i = 0; i < specialNforX[name].GetCount - 1; i++)
                            customerTotal -= inputList.GetValue(name, weight);

                        customerTotal += Convert.ToDecimal(specialNforX[name].GetPrice);
                    }
                    else
                        customerTotal += inputList.GetValue(name, weight); 
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
                    double discount = 1 - (specialMarkDown[name].MarkDown / 100);

                    if ((specialMarkDown[name].UsedCount > specialMarkDown[name].BuyLimit) && (specialMarkDown[name].BuyLimit != 0))
                    {
                        //if resultant usedCount will be less than the buy limit, we have to calculate the split between eligibile and non-eligible weight
                        if (((specialMarkDown[name].UsedCount - weight) <= specialMarkDown[name].BuyLimit) && (specialMarkDown[name].BuyLimit > 0))
                        {
                            //usedCount - buyCount yields the remainder that should not be discounted
                            customerTotal -= inputList.GetValue(name, (specialMarkDown[name].UsedCount - specialMarkDown[name].BuyLimit));

                            //weight - usedCount - buyLimit yields remaining weight that needs to be calculated with discount
                            if ((specialMarkDown[name].UsedCount - weight - specialMarkDown[name].BuyLimit) > 0)
                                customerTotal -= inputList.GetValue(name, (weight - (specialMarkDown[name].UsedCount - specialMarkDown[name].BuyLimit)), discount);
                        }
                        else
                            customerTotal -= inputList.GetValue(name, weight);

                    }
                    else
                        customerTotal -= inputList.GetValue(name, weight, discount);

                    if (weight == 0)
                        specialMarkDown[name].Dec(1);
                    else
                        specialMarkDown[name].Dec(weight);

                }
                else if (specialBOGO.ContainsKey(name))
                {
                    //Console.WriteLine($"used:{specialBOGO[name].UsedCount}, buy:{specialBOGO[name].BuyCount}, get:{specialBOGO[name].GetCount}, special:{specialBOGO[name].SpecialCount}");

                    //floor the usedCount/buyCount; the -1 shows us where we're trying to go. So, if we're greater than that, we subtract special item
                    if ((specialBOGO[name].SpecialCount > (Math.Floor((specialBOGO[name].UsedCount - 1) / specialBOGO[name].BuyCount) * specialBOGO[name].GetCount)) &&
                            (((specialBOGO[name].UsedCount <= specialBOGO[name].BuyLimit)) || (specialBOGO[name].BuyLimit == 0)))
                    {
                        specialBOGO[name].SpecDec(1);

                        double discount = 1 - (specialBOGO[name].MarkDown / 100);
                        customerTotal -= inputList.GetValue(name, weight, discount);
                    }
                    else
                    {
                        specialBOGO[name].Dec(1);

                        customerTotal -= inputList.GetValue(name, weight);
                    }
                }
                else if (specialNforX.ContainsKey(name))
                {
                    specialNforX[name].Dec(1);

                    //Console.WriteLine($"count: {specialNforX[name].UsedCount}");

                    //adding 1 to usedcount checks if the previous value was applicable for a special discount. coming down from that means we have to take it away
                    if ((((specialNforX[name].UsedCount + 1) % specialNforX[name].GetCount) == 0) && ((specialNforX[name].UsedCount <= specialNforX[name].BuyLimit) ||
                            (specialNforX[name].BuyLimit == 0)))
                    {
                        customerTotal -= Convert.ToDecimal(specialNforX[name].GetPrice);

                        for (int i = 0; i < specialNforX[name].GetCount - 1; i++)
                            customerTotal += inputList.GetValue(name, weight);
                    }
                    else
                    {
                        customerTotal -= inputList.GetValue(name, weight);
                    }
                        
                }
                else
                    customerTotal -= inputList.GetValue(name, weight);
            }
            else
                throw new KeyNotFoundException($"{name} was not found in inventory.");
        }

        public void AddMarkDown(string nameIn, double markDownIn, double buyLimitIn = 0)
        {
            if (specialMarkDown.ContainsKey(nameIn))
                throw new ArgumentException($"A markdown for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialBOGO.ContainsKey(nameIn))
                throw new ArgumentException($"A BOGO for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialMarkDown.ContainsKey(nameIn))
                throw new ArgumentException($"A mark-down for {nameIn} already exists. Remove this entry before creating a new one.");
            else if (specialNforX.ContainsKey(nameIn))
                throw new ArgumentException($"An N for X special for {nameIn} already exists. Remove this entry before creating a new one.");
            else
                specialMarkDown.Add(nameIn, new SpecialMarkDown(nameIn, markDownIn, buyLimitIn));
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
