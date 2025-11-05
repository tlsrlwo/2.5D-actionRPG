using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    [System.Serializable]
    public class ChestSlot
    {
        public Item item;                       // 상자 안에 있는 아이템

        [Min(1)]
        public int quantity = 1;                // 아이템의 수량
    }
}
