using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public enum WeaponType
    {
        Sword,
        Speark,
        Bow
    }

    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "MyGame/ItemData/Weapon")]
    public class Weapon : Item
    {
        // 코드 적기 전에 한글폰트 보이게 다른이름으로저장 -> 인코딩 하기 !!
        [Header("무기 정보")]
        public WeaponType weaponType;
        public int damage;
    }
}
