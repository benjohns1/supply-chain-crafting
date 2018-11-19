using UnityEngine;

namespace SupplyChain
{
    public class FactorySettings : MonoBehaviour
    {
        public Recipe.ID[] EnabledRecipes;
        public Inventory.StackSettings[] StartingInputItems;
        public Color IdleTint = Color.white;
        public Color ActiveTint = Color.green;
        public Color CompleteTint = Color.blue;
        public Color BlockedTint = Color.red;
        public GameObject OutputConnected;
    }
}
