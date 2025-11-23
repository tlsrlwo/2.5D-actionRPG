using UnityEngine;

namespace KW 
{
    // [힌트] 
    // "DuskBorn/Dialogue" 메뉴에서 이 에셋을 생성할 수 있게 하는
    // 어트리뷰트가 필요합니다. (예: [CreateAssetMenu(...)])
    [CreateAssetMenu(fileName = "npcDialogue", menuName = "DuskBorn/NpcData/NpcDialogue")]
    public class DialogueSO : ScriptableObject
    {
        [Header("대화 내용")]

        [TextArea(3, 10)]
        public string[] dialogueLines;

        [Header("선택지 (없으면 대화 종료)")]    
        public DialogueChoice[] dialogueChoices;
    }

    [System.Serializable]
    public class DialogueChoice
    {        
        // 거절한다, 수락한다와 같은 대화 선택지
        public string choiceText;

        // 다음으로 지정할 다이엍SO
        public DialogueSO nextDialogue;

        // 보상이 있는 대화에서만 사용
        public Item rewardItem;
        public int rewardItemCount = 1;
    }
}