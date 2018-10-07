using System;
using System.Collections.Generic;

namespace SupplyChain
{
    public static class Inventory
    {
        [Serializable]
        public class StackSettings
        {
            public Item.ID Id;
            public int Amount;

            public Item.Stack GetStack()
            {
                return new Item.Stack(Id, Amount);
            }

            public static Item.Stack[] GetStacks(StackSettings[] stackSettings)
            {
                Item.Stack[] stacks = new Item.Stack[stackSettings.Length];
                for (int i = 0; i < stackSettings.Length; i++)
                {
                    stacks[i] = stackSettings[i].GetStack();
                }
                return stacks;
            }
        }

        public static Item.Stack[] AddAll(Item.Stack[] addTo, Item.Stack[] itemsToAdd, out bool allAdded)
        {
            List<Item.Stack> newItems = new List<Item.Stack>(addTo);
            foreach (Item.Stack stack in itemsToAdd)
            {
                int index = newItems.FindIndex(i => i.Id == stack.Id);
                if (index < 0)
                {
                    newItems.Add(new Item.Stack(stack.Id, stack.Amount));
                    continue;
                }
                newItems[index] = newItems[index].AddTo(stack.Amount);
            }

            allAdded = true;
            return newItems.ToArray();
        }

        public static Item.Stack[] RemoveIfAllAvailable(Item.Stack[] removeFrom, Item.Stack[] stacksToRemove, out bool removed)
        {
            Item.Stack[] newItemBuffer = removeFrom;
            foreach (Item.Stack stackToRemove in stacksToRemove)
            {
                bool removedItem = false;
                newItemBuffer = RemoveIfAllAvailable(newItemBuffer, stackToRemove, out removedItem);
                if (!removedItem)
                {
                    removed = false;
                    return removeFrom;
                }
            }
            removed = true;
            return newItemBuffer;
        }

        public static Item.Stack[] RemoveIfAllAvailable(Item.Stack[] removeFrom, Item.Stack stackToRemove, out bool removed)
        {
            List<Item.Stack> newItemBuffer = new List<Item.Stack>();
            int remainingAmountToRemove = stackToRemove.Amount;
            foreach (Item.Stack stack in removeFrom)
            {
                if (stack.Id != stackToRemove.Id)
                {
                    newItemBuffer.Add(new Item.Stack(stack));
                    continue;
                }

                if (stack.Amount > remainingAmountToRemove)
                {
                    newItemBuffer.Add(new Item.Stack(stack.Id, stack.Amount - remainingAmountToRemove));
                    remainingAmountToRemove = 0;
                    continue;
                }

                remainingAmountToRemove -= stack.Amount;
            }

            if (remainingAmountToRemove > 0)
            {
                removed = false;
                return removeFrom;
            }

            removed = true;
            return newItemBuffer.ToArray();
        }
    }
}
