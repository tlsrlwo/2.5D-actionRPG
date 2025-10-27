using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public enum WeaponType
    {
        Sword,
        Spear,
        Bow
    }
    
    public enum WeaponGrade
    {
       Normal,
       Traveler,
       Knight,
       Legendary
    }

    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "DuskBorn/ItemData/Weapon")]
    public class Weapon : Item
    {     
        [Header("무기 정보")]
        public WeaponType weaponType;
        public WeaponGrade weaponGrade;
        public int damage;
    }
}
