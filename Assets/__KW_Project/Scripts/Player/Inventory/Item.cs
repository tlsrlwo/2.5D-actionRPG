using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class Item : ScriptableObject
    {
        [Header("공통 정보")]
        public string itemName;
        public Sprite itemSprite;
        [TextArea(3, 5)]
        public string description;                  // 아이템 설명        
        [Min(1)]
        public int maxStack = 1;
    }
}
