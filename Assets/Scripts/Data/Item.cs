using System;
using System.Collections.Generic;

namespace SupplyChain
{
    public static class Item
    {
        public enum ID { None, IronOre, CopperOre, Coal, IronIngot, CopperIngot, SteelIngot };

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
            { ID.None, new Data(name: "None", description: "No item") },
            { ID.IronOre, new Data(name: "Iron Ore", description: "Iron ore") },
            { ID.CopperOre, new Data(name: "Copper Ore", description: "Copper ore") },
            { ID.Coal, new Data(name: "Coal", description: "Coal") },
            { ID.IronIngot, new Data(name: "Iron Ingot", description: "Iron ingot") },
            { ID.CopperIngot, new Data(name: "Copper Ingot", description: "Copper ingot") },
            { ID.SteelIngot, new Data(name: "Steel Ingot", description: "Steel ingot") },
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
