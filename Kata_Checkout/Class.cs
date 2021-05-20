using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata_Checkout
{
    class ItemList
    {
        Dictionary<string, decimal> itemPrice = new Dictionary<string, decimal>();
        List<bool> weightedItems = new List<bool>();

        public void AddItem(string name, decimal value)
        {
            if (name == "")
            {
                throw new ArgumentException($"Null values are invalid for item names.\nPlease reenter a valid argument.");
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
        }

        public void RemoveItem(string name)
        {
            if (name == "")
            {
                throw new ArgumentException($"Null values are invalid for item names.\nPlease reenter a valid argument.");
            }
            else if (!itemPrice.ContainsKey(name))
            {
                throw new ArgumentException($"{name} was not found in the current item list.");
            }
            else
            {
                itemPrice.Remove(name);
            }

        }

        public void PrintOut()
        {
            foreach (KeyValuePair<string, decimal> pair in itemPrice)
                    Console.WriteLine($"{pair.Key}, {pair.Value}");
        }
    }
}
