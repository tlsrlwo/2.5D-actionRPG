using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public int stack;

        public InventorySlot(Item item, int stack)
        {
            this.item = item;
            this.stack = stack;
        }
    }
}
