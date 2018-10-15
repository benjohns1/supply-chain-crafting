using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class ProcessSystem : ComponentSystem
    {
        const float GlobalTimeEffortMultiplier = 5f;

        public struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            [ReadOnly]
            public ComponentArray<EnabledRecipesComponent> EnabledRecipes;
            public ComponentArray<ProcessComponent> Process;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                switch (data.Process[i].State)
                {
                    case ProcessComponent.ProcessState.Idle:
                    case ProcessComponent.ProcessState.Complete:
                    default:
                        StartProcess(i);
                        break;
                    case ProcessComponent.ProcessState.Active:
                    case ProcessComponent.ProcessState.Blocked:
                        RunProcess(i);
                        break;
                }
            }
        }

        private void RunProcess(int i)
        {
            ProcessComponent process = data.Process[i];
            if (process.CompleteTime > Time.time)
            {
                return; // process hasn't completed yet
            }

            Recipe.Data recipe = Recipe.Get(process.Recipe);
            bool allAdded = false;
            process.ItemBufferOut = Inventory.AddAll(process.ItemBufferOut, recipe.Outputs, out allAdded);
            if (!allAdded)
            {
                process.State = ProcessComponent.ProcessState.Blocked;
                return;
            }
            process.ItemBufferActive = new Item.Stack[process.ItemBufferActive.Length];
            process.State = ProcessComponent.ProcessState.Complete;
        }

        private void StartProcess(int i)
        {
            foreach (Recipe.ID recipeId in data.EnabledRecipes[i].Recipes)
            {
                Recipe.Data recipe = Recipe.Get(recipeId);

                bool removed = false;
                ProcessComponent process = data.Process[i];
                process.ItemBufferIn = Inventory.RemoveIfAllAvailable(process.ItemBufferIn, recipe.Inputs, out removed);
                if (!removed)
                {
                    continue;
                }

                // Recipe found and all items were removed from the input buffer, start the process
                process.ItemBufferActive = new Item.Stack[recipe.Inputs.Length];
                Array.Copy(recipe.Inputs, process.ItemBufferActive, recipe.Inputs.Length);

                process.CompleteTime = GlobalTimeEffortMultiplier * recipe.Effort + Time.time;
                process.Recipe = recipeId;
                process.State = ProcessComponent.ProcessState.Active;
                return;
            }

        }
    }
}
