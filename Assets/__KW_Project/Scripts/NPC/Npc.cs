using System.Collections.Generic; // ◀◀ 퀘스트/보상 리스트를 위해
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
        [SerializeField] private DialogueSO questCompletionDialogue;            // 퀘스트 완료 대사         
        [SerializeField] private DialogueSO afterQuestDialogue;                 // 퀘스트 완료 후 방문 시 대사


        /* [Header("퀘스트 데이터")]
        [Tooltip("퀘스트 요구 사항")]
        public List<ChestSlot> questRequirements;

        [Tooltip("퀘스트 완료 시 지급할 보상")]
        public List<ChestSlot> questRewards;*/

        public QuestSO questData; 

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
            // 플레이어 인벤토리 참조 (최초 1회)
            if (_playerInventory == null)
            {
                _playerInventory = player.GetComponent<Inventory>();
            }

            // NPC 현재 상태에 따른 대화 시작
            switch (currentQuestState)
            {
                case QuestState.NotOffered:                     // 아직 퀘스트를 주지 않은 상태
                    if (hasMetPlayer == false)
                    {
                        hasMetPlayer = true;
                        _dialogueManager.StartDialogue(firstMeetingDialogue, this);
                    }
                    else
                    {
                        _dialogueManager.StartDialogue(questOfferDialogue, this);
                    }
                    break;

                case QuestState.Declined:                       // 퀘스트를 알려줬지만 거절당한 상태
                    _dialogueManager.StartDialogue(declinedLoopDialogue, this);
                    break;

                case QuestState.Accepted:                       // 퀘스트가 수락된 상태
                    // QuestManager 에서 조건 만족 여부 확인
                    if (QuestManager.Instance.IsQuestConditionMet(questData))
                    {
                        // 조건만족 -> 완료(보상) 대화 시작
                        _dialogueManager.StartDialogue(questCompletionDialogue, this);
                    }
                    else
                    {
                        // 아직 달성 못 했으면 수락 후 재방문 대사 
                        _dialogueManager.StartDialogue(acceptedLoopDialogue, this);
                    }
                    break;
                case QuestState.Completed:                      // 퀘스트 완료된 상태
                    _dialogueManager.StartDialogue(afterQuestDialogue, this);
                    break;

            }

            /*  // NPC의 '현재 상태'에 따라 다른 대화를 시작
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
                 // 요구 아이템을 다 모았는지 '먼저 확인'
                 if (CheckQuestRequirements())
                 {
                     // [If Yes] "다 모아왔군!" 대화 시작 (보상받기 버튼)
                     _dialogueManager.StartDialogue(afterQuestDialogue, this);
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
             } */
        } 
        
        public void OnChoiceMade(DialogueChoice choice)
        {
            // 보상 지급
            if(choice.rewardItem != null)
            {
                if (_playerInventory != null)
                {
                    _playerInventory.AddItem(choice.rewardItem, choice.rewardItemCount);
                    Debug.Log($"보상 지급 완료 : {choice.rewardItem}, {choice.rewardItemCount}개 추가됨");

                    // 팝업 알림 로직 여기에도 추가

                }
            }

            // 다음 대화 확인
            DialogueSO nextDialogue = choice.nextDialogue;

            // 다음 대화 선택지를 확인하고
            if (nextDialogue == acceptedDialogue)
            {
                // 현재 상태 변경
                currentQuestState = QuestState.Accepted;

                if (questData != null)
                {
                    QuestManager.Instance.AcceptQuest(questData);
                }
            }
            else if (nextDialogue == declinedDialogue)
            {
                currentQuestState = QuestState.Declined;
            }
            else if (nextDialogue == afterQuestDialogue)
            {
                // 요구 아이템 가져가기
                QuestManager.Instance.SubmitQuestItems(questData);

                QuestManager.Instance.FinishQuest(questData);

                currentQuestState = QuestState.Completed;
            }
            if (nextDialogue != null)
            {
                _dialogueManager.StartDialogue(nextDialogue, this);
            }
            else
            {
                
            }
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