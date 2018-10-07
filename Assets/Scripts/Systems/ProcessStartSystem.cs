using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class ProcessStartSystem : ComponentSystem
    {
        const float GlobalTimeEffortMultiplier = 5f;

        public struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            [ReadOnly]
            public ComponentArray<EnabledRecipesComponent> EnabledRecipes;
            public ComponentArray<ItemBufferInComponent> ItemBufferIn;
            public ComponentArray<ProcessComponent> Process;
            public SubtractiveComponent<ProcessActive> ProcessActive;
            public SubtractiveComponent<ProcessComplete> ProcessComplete;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                foreach (Recipe.ID recipeId in data.EnabledRecipes[i].Recipes)
                {
                    Recipe.Data recipe = Recipe.Get(recipeId);

                    bool removed = false;
                    data.ItemBufferIn[i].ItemBuffer = Inventory.RemoveIfAllAvailable(data.ItemBufferIn[i].ItemBuffer, recipe.Inputs, out removed);
                    if (!removed)
                    {
                        continue;
                    }

                    // Recipe found and all items were removed from the input buffer, start the process
                    Item.Stack[] processorBuffer = new Item.Stack[recipe.Inputs.Length];
                    Array.Copy(recipe.Inputs, processorBuffer, processorBuffer.Length);

                    ProcessComponent process = data.Process[i];
                    process.CompleteTime = GlobalTimeEffortMultiplier * recipe.Effort + Time.time;
                    process.ItemBuffer = processorBuffer;
                    process.Recipe = recipeId;

                    PostUpdateCommands.AddComponent(data.Entity[i], new ProcessActive());
                }
            }
        }
    }
}
