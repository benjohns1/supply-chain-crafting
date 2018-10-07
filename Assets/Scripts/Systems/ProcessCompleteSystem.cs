using System;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class ProcessCompleteSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public EntityArray Entity;
            public GameObjectArray GameObject;
            public ComponentArray<ProcessComponent> Process;
            public ComponentArray<ItemBufferOutComponent> ItemBufferOut;
            public ComponentDataArray<ProcessComplete> ProcessComplete;
            public SubtractiveComponent<ProcessActive> ProcessActive;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                bool allAdded = false;
                Item.Stack[] itemsOut = Inventory.AddAll(data.ItemBufferOut[i].ItemBuffer, data.Process[i].ItemBuffer, out allAdded);
                if (!allAdded)
                {
                    continue; // error adding process output to output item buffer
                }

                data.Process[i].ItemBuffer = new Item.Stack[data.Process[i].ItemBuffer.Length];
                data.ItemBufferOut[i].ItemBuffer = itemsOut;

                PostUpdateCommands.RemoveComponent<ProcessComplete>(data.Entity[i]);
            }
        }
    }
}
