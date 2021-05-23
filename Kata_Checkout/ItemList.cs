using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Checkout
{

    //a class to manage inventory and price of all items
    public class ItemList
    {

        //a dictionary to store item/value pairs and a list to keep track of items that are valued by weight
        Dictionary<string, double> itemPrice = new Dictionary<string, double>();
        List<string> weightedItems = new List<string>();

        //add an item to the dictionary, do not accept null, empty, or negative values
        public void AddItem(string name, double value, bool weighted = false)
        {
            name = name.Trim();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"Empty values are invalid for item names.\nPlease reenter a valid argument.");
            }
            else if (value <= 0)
            {
                throw new ArgumentException($"{value} is an invalid price\nPlease reenter a valid arguement.");
            }
            else if (itemPrice.ContainsKey(name))
            {
                throw new ArgumentException($"{name} is already present in the item list.\nPlease reenter a valid argument.");
            }
            else
            {
                itemPrice.Add(name, value);
            }

            if (weighted)
                weightedItems.Add(name);

        }

        //remove an item from the dictionary; throw an exception if it's not in the dictionary
        public void RemoveItem(string name)
        {
            if (!itemPrice.ContainsKey(name))
            {
                throw new ArgumentException($"{name} was not found in inventory.");
            }
            else
            {
                itemPrice.Remove(name);

                if (weightedItems.Contains(name))
                    weightedItems.Remove(name);
            }

        }

        //check if an item is valued by weight
        public bool IsWeighted(string name)
        {
            if (weightedItems.Contains(name))
                return true;
            else
                return false;
        }

        //print out all items present in dictionary
        public void PrintOut()
        {
            foreach (KeyValuePair<string, double> pair in itemPrice)
                    Console.WriteLine($"{pair.Key}, {pair.Value}");
        }

        //extension of dictionary.ContainsKey()
        public bool Contains(string name)
        {
            if (itemPrice.ContainsKey(name))
                return true;
            else
                return false;
        }

        //return the item value given the item name. throw an exception is a weighted item IS NOT given a weight or if a non-weighted item IS given a weight
        public decimal GetValue(string name, double weight = 0, double discount = -1)
        {
            if (weight < 0)
                throw new ArgumentException($"Negative weight invalid.");

            if (itemPrice.ContainsKey(name))
            {

                if (weightedItems.Contains(name))
                {
                    if (weight > 0)
                        if (discount >= 0)
                        {
                            return Convert.ToDecimal(Math.Round(itemPrice[name]  * discount, 2) * weight);
                        }
                        else
                            return Convert.ToDecimal(itemPrice[name] * weight);
                    else
                        throw new ArgumentException($"Weight not given for {name}.");
                }
                else
                {
                    if (weight > 0)
                        throw new ArgumentException($"{name} is not valued by weight.");
                    else
                    {
                        if (discount >= 0)
                        {
                            return Convert.ToDecimal(Math.Round(itemPrice[name] * discount, 2));
                        }
                        else
                            return Convert.ToDecimal(itemPrice[name]);
                    }    
                }
            }
            else
                throw new ArgumentException($"{name} was not found in inventory.");
        }
    }
}
