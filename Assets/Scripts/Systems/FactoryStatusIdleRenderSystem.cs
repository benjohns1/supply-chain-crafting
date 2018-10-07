using System;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class FactoryStatusIdleRenderSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public ComponentArray<MeshRenderer> MeshRenderer;
            public ComponentArray<FactorySettings> Settings;
            public ComponentDataArray<ProcessComplete> ProcessComplete;
            public SubtractiveComponent<ProcessActive> ProcessActive;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data.MeshRenderer[i].material.color = data.Settings[i].IdleTint;
            }
        }
    }
}
