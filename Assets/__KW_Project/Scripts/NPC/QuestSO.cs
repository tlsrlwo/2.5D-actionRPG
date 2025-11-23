using UnityEngine;

namespace KW
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "DuskBorn/NpcData/Quest")]
    public class QuestSO : ScriptableObject
    {
        [Header("기본 정보")]
        public string questTitle;                           // 예 : 마을 구하기
        [TextArea] public string description;               // 예 : 스켈레톤 5마리를 토벌하라

        [Header("목표")]
        public string targetName;                           // 예 : Skeleton
        public int targetCount;                             // 에 : 5(마리)

        [Header("보상")]
        public Item itemReward;                             
        public int itemRewardCount;        
    }
}
