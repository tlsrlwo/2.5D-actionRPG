using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armour = 2,
        Consumable = 3,
        Etc = 4
    }

    public abstract class Item : ScriptableObject
    {
       
         
        [Header("공통 정보")]
        public string itemId;                       // 데이터파싱 csv 를 위해
        public string itemName;

        public ItemType itemType;
        
        public Sprite itemSprite;

        [TextArea(3, 5)]
        public string description;                  // 아이템 설명        
        [Min(1)]
        public int maxStack = 1;
    }
}
