using UnityEngine;

namespace SupplyChain
{
    public class FactorySettings : MonoBehaviour
    {
        public Recipe.ID[] EnabledRecipes;
        public Inventory.StackSettings[] StartingInputItems;
        public Color IdleTint = Color.white;
        public Color RunningTint = Color.green;
    }
}
