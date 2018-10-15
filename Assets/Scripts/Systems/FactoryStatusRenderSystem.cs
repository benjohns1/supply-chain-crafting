using System;
using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    [UpdateBefore(typeof(ConnectorTransferOutSystem))]
    public class FactoryStatusIdleRenderSystem : ComponentSystem
    {
        public struct Data
        {
            public readonly int Length;
            public ComponentArray<MeshRenderer> MeshRenderer;
            public ComponentArray<FactorySettings> Settings;
            public ComponentArray<ProcessComponent> Process;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data.MeshRenderer[i].material.color = GetColor(data.Process[i].State, data.Settings[i]);
            }
        }

        private Color GetColor(ProcessComponent.ProcessState state, FactorySettings settings)
        {
            switch (state)
            {
                case ProcessComponent.ProcessState.Active:
                    return settings.ActiveTint;
                case ProcessComponent.ProcessState.Complete:
                    return settings.CompleteTint;
                case ProcessComponent.ProcessState.Blocked:
                    return settings.BlockedTint;
                case ProcessComponent.ProcessState.Idle:
                default:
                    return settings.IdleTint;
            }
        }
    }
}
