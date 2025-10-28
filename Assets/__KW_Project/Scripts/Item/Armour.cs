using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public enum ArmourType
    {
        Helmet,
        Armour,
        Leg
    }
    public enum ArmourGrade
    {
        Normal,
        Traveler,
        Knight,
        Legendary
    }

    [CreateAssetMenu(fileName = "NewArmourData", menuName = "DuskBorn/ItemData/Armour")]
    public class Armour : Item
    {
        [Header("갑옷 정보")]
        public ArmourType armourType;
        public ArmourGrade armourGrade;

        public float defenceRate;
    }
}
