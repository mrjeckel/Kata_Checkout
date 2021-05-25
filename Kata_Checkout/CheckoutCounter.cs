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

                    double tempWeight = weight;
                    double getCount = specialBOGO[name].GetCount;
                    double buyCount = specialBOGO[name].BuyCount;
                    double buyLimit = specialBOGO[name].BuyLimit;
                    double discount = 1 - (specialBOGO[name].MarkDown / 100);

                    if (weight == 0)
                        tempWeight = 1;

                    while (tempWeight > 0)
                    {

                        double usedCount = specialBOGO[name].UsedCount;
                        double specialCount = specialBOGO[name].SpecialCount;

                        //compare groups of usedCount against groups of specialCount. If equal, then we another group of normal items
                        if ((Math.Floor(usedCount / buyCount) == Math.Floor(specialCount / getCount)) || ((usedCount > buyLimit) && (buyLimit != 0)))
                        {
                            //if we're in between making another full set of normal items, add the difference
                            if (((usedCount % buyCount) != 0) && (tempWeight >= (buyCount - (usedCount % buyCount))))
                            {
                                customerTotal += inputList.GetValue(name, buyCount - (usedCount % buyCount));
                                specialBOGO[name].Inc(buyCount - (usedCount % buyCount));
                                tempWeight -= buyCount - (usedCount % buyCount);
                            }
                            //if we're at a clean break with our normal item groups, then we just add one group if we have the weight
                            else if (tempWeight >= buyCount)
                            {
                                customerTotal += inputList.GetValue(name, buyCount);
                                specialBOGO[name].Inc(buyCount);
                                tempWeight -= buyCount;
                            }
                            //we don't have enough weight to make a full group, so just add what we have
                             else
                            {
                                customerTotal += inputList.GetValue(name, tempWeight);
                                specialBOGO[name].Inc(tempWeight);
                                tempWeight = 0;
                            }
                            //Console.WriteLine($"normal: used:{specialBOGO[name].UsedCount} spec:{specialBOGO[name].SpecialCount} temp:{tempWeight} total:{customerTotal}");
                        }

                        //break if we ran out of items
                        if (tempWeight == 0)
                            break;

                        //if we make it here, we've added enough to make another normal group and are eligibile for a special
                        if ((usedCount <= buyLimit) || (buyLimit == 0))
                        {
                            //if we're in between groups of specials, add the remainder to make a full group
                            if (((specialCount % getCount) != 0) && (tempWeight >= (getCount - (specialCount % getCount))))
                            {
                                customerTotal += inputList.GetValue(name, getCount - (specialCount % getCount), discount);
                                specialBOGO[name].SpecInc(getCount - (specialCount % getCount));
                                tempWeight -= getCount - (specialCount % getCount);
                            }
                            //if we have a full group, add another full group
                            else if (tempWeight >= getCount)
                            {
                                customerTotal += inputList.GetValue(name, getCount, discount);
                                specialBOGO[name].SpecInc(getCount);
                                tempWeight -= getCount;
                            }
                            //we don't have enough to make a full group, so just add what we have
                            else
                            {
                                customerTotal += inputList.GetValue(name, tempWeight, discount);
                                specialBOGO[name].SpecInc(tempWeight);
                                tempWeight = 0;
                            }
                           //Console.WriteLine($"special: used:{specialBOGO[name].UsedCount} spec:{specialBOGO[name].SpecialCount} temp:{tempWeight} total:{customerTotal}");
                        }
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

                    if ((specialMarkDown[name].UsedCount > specialMarkDown[name].BuyLimit) || (specialMarkDown[name].BuyLimit == 0))
                    {
                        //if resultant usedCount will be less than the buy limit, we have to calculate the split between eligibile and non-eligible weight
                        if (((specialMarkDown[name].UsedCount - weight) <= specialMarkDown[name].BuyLimit) && (specialMarkDown[name].BuyLimit > 0))
                        {
                            //buyLimit - usedCount yields the remainder that can be discounted
                            customerTotal -= inputList.GetValue(name, (specialMarkDown[name].UsedCount - specialMarkDown[name].BuyLimit));

                            //weight - usedCount - buyLimit yields remaining weight that needs to be calculated with any discount
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
                    //Console.WriteLine($"used:{specialBOGO[name].UsedCount}, buy:{specialBOGO[name].BuyCount}, get:{specialBOGO[name].GetCount}, spec:{specialBOGO[name].SpecialCount}");

                    double tempWeight = weight;
                    double getCount = specialBOGO[name].GetCount;
                    double buyCount = specialBOGO[name].BuyCount;
                    double buyLimit = specialBOGO[name].BuyLimit;
                    double discount = 1 - (specialBOGO[name].MarkDown / 100);

                    if (weight == 0)
                        tempWeight = 1;

                    while (tempWeight > 0)
                    {

                        double usedCount = specialBOGO[name].UsedCount;
                        double specialCount = specialBOGO[name].SpecialCount;

                        //compare groups of usedCount against groups of specialCount. If equal, then we another group of normal items
                        if ((Math.Floor(usedCount / buyCount) == Math.Floor(specialCount / getCount)) && ((usedCount <= buyLimit) || (buyLimit == 0)))
                        {
                            //if we're in between groups of specials, add the remainder to make a full group
                            if (((specialCount % getCount) != 0) && (tempWeight >= (getCount - (specialCount % getCount))))
                            {
                                customerTotal -= inputList.GetValue(name, getCount - (specialCount % getCount), discount);
                                specialBOGO[name].SpecDec(getCount - (specialCount % getCount));
                                tempWeight -= getCount - (specialCount % getCount);
                            }
                            //if we have a full group, add another full group
                            else if (tempWeight >= getCount)
                            {
                                customerTotal -= inputList.GetValue(name, getCount, discount);
                                specialBOGO[name].SpecDec(getCount);
                                tempWeight -= getCount;
                            }
                            //we don't have enough to make a full group, so just add what we have
                            else
                            {
                                customerTotal -= inputList.GetValue(name, tempWeight, discount);
                                specialBOGO[name].SpecDec(tempWeight);
                                tempWeight = 0;
                            }
                            Console.WriteLine($"special: used:{specialBOGO[name].UsedCount} spec:{specialBOGO[name].SpecialCount} temp:{tempWeight} total:{customerTotal}");
                        }

                        //break if we ran out of items
                        if (tempWeight == 0)
                            break;

                            //if we're in between making another full set of normal items, add the difference
                            if (((usedCount % buyCount) != 0) && (tempWeight >= (buyCount - (usedCount % buyCount))))
                            {
                                customerTotal -= inputList.GetValue(name, buyCount - (usedCount % buyCount));
                                specialBOGO[name].Dec(buyCount - (usedCount % buyCount));
                                tempWeight -= buyCount - (usedCount % buyCount);
                            }
                            //if we're at a clean break with our normal item groups, then we just add one group if we have the weight
                            else if (tempWeight >= buyCount)
                            {
                                customerTotal -= inputList.GetValue(name, buyCount);
                                specialBOGO[name].Dec(buyCount);
                                tempWeight -= buyCount;
                            }
                            //we don't have enough weight to make a full group, so just add what we have
                            else
                            {
                                customerTotal -= inputList.GetValue(name, tempWeight);
                                specialBOGO[name].Dec(tempWeight);
                                tempWeight = 0;
                            }
                        Console.WriteLine($"normal: used:{specialBOGO[name].UsedCount} spec:{specialBOGO[name].SpecialCount} temp:{tempWeight} total:{customerTotal}");
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
