using Unity.Entities;
using Unity.Mathematics;

namespace SupplyChain
{
    public struct Interactor : IComponentData {}

    public struct Interactable : IComponentData
    {
        public int Focus;
    }
}