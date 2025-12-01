using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    [CreateAssetMenu(fileName = "NewPotion", menuName = "DuskBorn/Item/Potion")]
    public class Potion : Item
    {
        [Header("포션 설정")]
        public int healAmount = 30; // 회복량
       
    }
}
