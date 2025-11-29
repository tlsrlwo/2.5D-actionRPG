using UnityEngine;

namespace KW
{
    public enum QuestType { Collect, Kill }                 // 퀘스트 종류


    [CreateAssetMenu(fileName = "NewQuest", menuName = "DuskBorn/NpcData/Quest")]
    public class QuestSO : ScriptableObject
    {
        [Header("기본 정보")]
        public string questTitle;                           // 예 : 001_Quest
        public string questNameForGame;                     // 예 : 스켈레톤 토벌
        [TextArea] public string description;               // 예 : 스켈레톤 5마리를 토벌하라

        [Header("퀘스트 설정")]
        public QuestType type;

        [Header("목표")]
        public string targetName;                           // 예 : Skeleton
        public int targetCount;                             // 에 : 5(마리)

        [Header("토벌 퀘스트 전용")]
        public string targetRegion;                         // 예 : Forest_Zone_1
       
        [Header("수집 퀘스트 전용")]
        public Item requiredItem;                             
        public int itemRewardCount;        
    }
}
