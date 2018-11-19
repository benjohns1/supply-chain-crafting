using UnityEngine;

namespace SupplyChain
{
    public class ProcessComponent : MonoBehaviour
    {
        public enum ProcessState { Idle, Active, Complete, Blocked }
        public float CompleteTime;
        public ProcessState State;
        public Recipe.ID Recipe;
        public Item.Stack[] ItemBufferIn = new Item.Stack[1];
        public Item.Stack[] ItemBufferActive = new Item.Stack[1];
        public Item.Stack[] ItemBufferOut = new Item.Stack[1];
    }
}
