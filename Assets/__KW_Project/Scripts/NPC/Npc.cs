using System.Collections.Generic; // ◀◀ 퀘스트/보상 리스트를 위해
using UnityEngine;

namespace KW
{
    [RequireComponent(typeof(IInteractable))] // IInteractable이 필수임을 명시
    public class Npc : MonoBehaviour, IInteractable
    {
        [Header("대화 SO")]        
        [SerializeField] private DialogueSO firstMeetingDialogue;               // 최초 첫 만남 시 대사        
        [SerializeField] private DialogueSO questOfferDialogue;                 // 퀘스트 제안 대사       
        [SerializeField] private DialogueSO acceptedDialogue;                   // 퀘스트 수락 시 대사        
        [SerializeField] private DialogueSO declinedDialogue;                   // 퀘스트 거절했을 때 대사       
        [SerializeField] private DialogueSO acceptedLoopDialogue;               // 퀘스트 진행 중 대사 (미션 미완)      
        [SerializeField] private DialogueSO declinedLoopDialogue;               // 퀘스트 거절 후 재방문 대사        
        [SerializeField] private DialogueSO questHandInDialogue;                // 퀘스트 완료 대사         
        [SerializeField] private DialogueSO afterQuestDialogue;                 // 퀘스트 완료 후 방문 시 대사


        [Header("퀘스트 데이터")]
        [Tooltip("퀘스트 요구 사항")]
        public List<ChestSlot> questRequirements;

        [Tooltip("퀘스트 완료 시 지급할 보상")]
        public List<ChestSlot> questRewards;

        // NPC의 현재 상태 저장 
        private bool hasMetPlayer = false;
        [SerializeField] private QuestState currentQuestState = QuestState.NotOffered;

        [Header("내부 변수")]
        private DialogueManager _dialogueManager;
        private Inventory _playerInventory;

        private void Start()
        {
            _dialogueManager = DialogueManager.Instance;
        }

        // --- 메인 상호작용 함수 ---
        public void Interact(GameObject player)
        {
            // 1. 플레이어 인벤토리 참조 (최초 1회)
            if (_playerInventory == null)
            {
                _playerInventory = player.GetComponent<Inventory>();
            }

            // 2. [핵심] NPC의 '현재 상태'에 따라 다른 대화를 시작

            if (hasMetPlayer == false)
            {
                hasMetPlayer = true;
                _dialogueManager.StartDialogue(firstMeetingDialogue, this);
            }
            else if (currentQuestState == QuestState.NotOffered)
            {
                _dialogueManager.StartDialogue(questOfferDialogue, this);
            }
            else if (currentQuestState == QuestState.Declined)
            {
                _dialogueManager.StartDialogue(declinedLoopDialogue, this);
            }
            else if (currentQuestState == QuestState.Accepted) // 퀘스트를 수락한 상태일 때
            {
                // [튜토리얼 로직 적용!] 요구 아이템을 다 모았는지 '먼저 확인'
                if (CheckQuestRequirements())
                {
                    // [If Yes] "다 모아왔군!" 대화 시작 (보상받기 버튼)
                    _dialogueManager.StartDialogue(questHandInDialogue, this);
                }
                else
                {
                    // [If No] "아직인가?" 대화 시작
                    _dialogueManager.StartDialogue(acceptedLoopDialogue, this);
                }
            }
            else if (currentQuestState == QuestState.Completed)
            {
                _dialogueManager.StartDialogue(afterQuestDialogue, this);
            }
        }

        // [새 함수] 플레이어 인벤토리를 확인하는 함수
        private bool CheckQuestRequirements()
        {
            if (_playerInventory == null) return false;

            // (Inventory.cs에 HasItem 함수가 필요합니다. 지금은 임시로 true를 반환시켜 테스트합니다.)
            //
            // foreach (ChestSlot req in questRequirements)
            // {
            //    // [필요한 함수] if (!playerInventory.HasItem(req.item, req.quantity))
            //    // {
            //    //     return false; // 하나라도 부족하면 false
            //    // }
            // }
            // return true; // 모두 다 있음

            Debug.LogWarning("CheckQuestRequirements()가 아직 구현되지 않아 임시로 'true'를 반환합니다.");
            return false; // ◀◀ [임시] 테스트를 위해 항상 true 반환
        }

        // [핵심 수정] DialogueManager가 선택지를 눌렀을 때 호출할 함수
        public void OnChoiceMade(DialogueSO nextDialogue)
        {
            // 1. 퀘스트 수락 시
            if (nextDialogue == acceptedDialogue)
            {
                currentQuestState = QuestState.Accepted;
                // [삭제!] 보상 지급 로직 삭제
            }
            // 2. 퀘스트 거절 시
            else if (nextDialogue == declinedDialogue)
            {
                currentQuestState = QuestState.Declined;
            }
            // 3. [추가!] '보상 받기' 버튼을 눌렀을 때
            // (questHandInDialogue의 "[보상받기]" 선택지에 nextDialogue를 'null'로 연결)
            else if (nextDialogue == null)
            {
                // [추가!] 요구 아이템을 '제출'받습니다.
                SubmitItems();

                // [이동!] 여기서 '보상'을 줍니다.
                GiveRewards();

                // [추가!] 상태를 '완료'로 변경
                currentQuestState = QuestState.Completed;

                // [추가!] 완료 후 대화로 넘어감
                _dialogueManager.StartDialogue(afterQuestDialogue, this);

                return; // 여기서 함수 종료
            }

            // 4. (보상 받기가 아닌) 다음 대화로 이어서 시작
            _dialogueManager.StartDialogue(nextDialogue, this);
        }

        // 보상 지급 함수 (Chest.cs 로직 재활용)
        private void GiveRewards()
        {
            if (_playerInventory != null && questRewards.Count > 0)
            {
                Debug.Log("퀘스트 보상을 지급합니다...");
                List<ChestSlot> givenRewards = new List<ChestSlot>();
                foreach (ChestSlot reward in questRewards)
                {
                    if (_playerInventory.AddItem(reward.item, reward.quantity))
                    {
                        givenRewards.Add(reward);
                    }
                    else
                    {
                        Debug.LogWarning("인벤토리 공간 부족으로 보상 지급 실패.");
                    }
                }
                foreach (ChestSlot reward in givenRewards)
                {
                    questRewards.Remove(reward);
                }
            }
        }

        // [새 함수] (선택 사항) 요구 아이템을 인벤토리에서 제거
        private void SubmitItems()
        {
            if (_playerInventory == null) return;

            Debug.Log("퀘스트 아이템을 제출받습니다...");
            // (Inventory.cs에 RemoveItem 함수가 필요합니다)
            // foreach (ChestSlot req in questRequirements)
            // {
            //    playerInventory.RemoveItem(req.item, req.quantity);
            // }
        }
    }

    // NPC의 퀘스트 상태를 관리할 'Enum'
    public enum QuestState
    {
        NotOffered, // 퀘스트 제안 전
        Declined,   // 플레이어가 퀘스트를 거절함
        Accepted,   // 플레이어가 퀘스트를 수락함 (진행 중)
        Completed   // 플레이어가 퀘스트를 완료함
    }
}