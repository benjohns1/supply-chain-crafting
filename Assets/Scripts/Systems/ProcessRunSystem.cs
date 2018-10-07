using System;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class ProcessRunSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public ComponentArray<ProcessComponent> Process;
            public ComponentDataArray<ProcessActive> ProcessActive;
            public SubtractiveComponent<ProcessComplete> ProcessComplete;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                ProcessComponent process = data.Process[i];
                if (process.CompleteTime > Time.time)
                {
                    continue; // process hasn't completed yet
                }

                Recipe.Data recipe = Recipe.Get(process.Recipe);
                process.ItemBuffer = new Item.Stack[recipe.Outputs.Length];
                Array.Copy(recipe.Outputs, process.ItemBuffer, recipe.Outputs.Length);

                PostUpdateCommands.RemoveComponent<ProcessActive>(data.Entity[i]);
                PostUpdateCommands.AddComponent(data.Entity[i], new ProcessComplete());
            }
        }
    }
}
