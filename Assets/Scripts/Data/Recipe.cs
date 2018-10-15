using System;
using System.Collections.Generic;

namespace SupplyChain
{
    public static class Recipe
    {
        public enum ID { MineIron, MineCopper, MineCoal, SmeltIron, SmeltCopper, SmeltSteel };

        [Serializable]
        public struct Data
        {
            public readonly string Name;
            public readonly string Description;
            public readonly float Effort;
            public readonly Item.Stack[] Inputs;
            public readonly Item.Stack[] Outputs;

            public Data(string name, string description, float effort, Item.Stack[] inputs, Item.Stack[] outputs)
            {
                Name = name;
                Description = description;
                Effort = effort;
                Inputs = inputs;
                Outputs = outputs;
            }
        };

        private static readonly Dictionary<ID, Data> items = new Dictionary<ID, Data>
        {
            { ID.MineIron, new Data(name: "Mine Iron", description: "Mine iron ore", effort: 1f, inputs: new Item.Stack[] {}, outputs: new Item.Stack[] { new Item.Stack(Item.ID.IronOre, 1) }) },
            { ID.MineCopper, new Data(name: "Mine Copper", description: "Mine copper ore", effort: 1f, inputs: new Item.Stack[] {}, outputs: new Item.Stack[] { new Item.Stack(Item.ID.CopperOre, 1) }) },
            { ID.MineCoal, new Data(name: "Mine Coal", description: "Mine coal", effort: 1f, inputs: new Item.Stack[] {}, outputs: new Item.Stack[] { new Item.Stack(Item.ID.Coal, 1) }) },
            { ID.SmeltIron, new Data(name: "Smelt Iron", description: "Smelt iron ore into iron ingots", effort: 1.5f, inputs: new Item.Stack[] { new Item.Stack(Item.ID.IronOre, 2) }, outputs: new Item.Stack[] { new Item.Stack(Item.ID.IronIngot, 1) }) },
            { ID.SmeltCopper, new Data(name: "Smelt Copper", description: "Smelt copper ore into copper ingots", effort: 1f, inputs: new Item.Stack[] { new Item.Stack(Item.ID.CopperOre, 2) }, outputs: new Item.Stack[] { new Item.Stack(Item.ID.CopperIngot, 1) }) },
            { ID.SmeltSteel, new Data(name: "Smelt Steel", description: "Smelt iron ore and coal into steel ingots", effort: 2f, inputs: new Item.Stack[] { new Item.Stack(Item.ID.IronOre, 2), new Item.Stack(Item.ID.Coal, 1) }, outputs: new Item.Stack[] { new Item.Stack(Item.ID.SteelIngot, 1) }) },
        };

        public static Data Get(ID recipeId)
        {
            Data recipeData;
            if (!items.TryGetValue(recipeId, out recipeData))
            {
                throw new Exception("Could not find data for recipeId " + recipeId.ToString());
            }
            return recipeData;
        }
    }
}
