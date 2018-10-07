using System;
using System.Collections.Generic;

namespace SupplyChain
{
    public static class Recipe
    {
        public enum ID { Recipe1 };

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
            { ID.Recipe1, new Data(name: "Recipe1", description: "Recipe1 description", effort: 1f, inputs: new Item.Stack[] { new Item.Stack(Item.ID.Item1, 2) }, outputs: new Item.Stack[] { new Item.Stack(Item.ID.Item2, 1) }) }
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
