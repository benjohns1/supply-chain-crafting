using Unity.Entities;
using UnityEngine;

namespace SupplyChain
{
    public class FocusSystem : ComponentSystem
    {
        struct InteractorData
        {
            public readonly int Length;
            public readonly ComponentArray<Transform> Transform;
            public ComponentDataArray<Interactor> Interactor;
        }

        struct InteractableData
        {
            public readonly int Length;
            public readonly GameObjectArray GameObject;
            public ComponentDataArray<Interactable> Interactable;
        }

        [Inject] private InteractorData Interactors;
        [Inject] private InteractableData Interactables;

        protected override void OnUpdate()
        {
            for (int i = 0; i < Interactors.Length; i++)
            {
                InteractionRaycast(Interactors.Transform[i]);
            }
        }

        protected void InteractionRaycast(Transform interactorTransform)
        {
            Ray ray = new Ray(interactorTransform.position, interactorTransform.forward);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            int? hitInstanceID = hit.collider?.gameObject?.GetInstanceID();

            for (int j = 0; j < Interactables.Length; j++)
            {
                int focus = (hitInstanceID != null && hitInstanceID == Interactables.GameObject[j].GetInstanceID()) ? 1 : 0;
                if (Interactables.Interactable[j].Focus == focus)
                {
                    continue;
                }
                Interactables.Interactable[j] = new Interactable
                {
                    Focus = focus
                };
            }
        }
    }
}