using System;
using System.Collections.Generic;

namespace SupplyChain
{
    public static class Item
    {
        public enum ID { Item1, Item2 };

        [Serializable]
        public struct Data
        {
            public readonly string Name;
            public readonly string Description;

            public Data(string name, string description)
            {
                Name = name;
                Description = description;
            }
        };

        [Serializable]
        public struct Stack
        {
            public readonly ID Id;
            public readonly int Amount;

            public Stack(ID id, int value)
            {
                Id = id;
                Amount = value;
            }

            public Stack(Stack stack) : this(stack.Id, stack.Amount) { }

            public Stack AddTo(int amount)
            {
                return new Stack(Id, Amount + amount);
            }
        };

        private static readonly Dictionary<ID, Data> items = new Dictionary<ID, Data>
        {
            { ID.Item1, new Data(name: "Item1", description: "Item1 description") },
            { ID.Item2, new Data(name: "Item2", description: "Item2 description") },
        };

        public static Data Get(ID itemId)
        {
            Data itemData;
            if (!items.TryGetValue(itemId, out itemData))
            {
                throw new Exception("Could not find data for itemId " + itemId.ToString());
            }
            return itemData;
        }
    }
}
