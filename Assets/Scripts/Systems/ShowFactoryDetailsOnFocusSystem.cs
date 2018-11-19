using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class ShowFactoryDetailsOnFocusSystem : ComponentSystem
    {
        struct FactoryData
        {
            public readonly int Length;
            public ComponentDataArray<Interactable> Interactable;
            public ComponentArray<ProcessComponent> Process;
        }

        [Inject] private FactoryData Factories;

        protected override void OnUpdate()
        {
            for (int i = 0; i < Factories.Length; i++)
            {
                if (Factories.Interactable[i].Focus != 1)
                {
                    continue;
                }

                Debug.Log(i + ": " + Factories.Process[i].Recipe.ToString() + " " + Factories.Process[i].State.ToString());
            }
        }
    }
}