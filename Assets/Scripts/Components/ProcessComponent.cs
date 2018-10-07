using UnityEngine;

namespace SupplyChain
{
    public class ProcessComponent : MonoBehaviour
    {
        public float CompleteTime;
        public Recipe.ID Recipe;
        public Item.Stack[] ItemBuffer;
    }
}
