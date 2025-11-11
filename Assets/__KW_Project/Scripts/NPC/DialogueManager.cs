using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

namespace KW
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; set; }

        [Header("UI 요소")]
        [SerializeField] private GameObject dialoguePanel;                       // 대화창 
        [SerializeField] private TextMeshProUGUI dialogueText;                   // 대화가 표시될 텍스트
        [SerializeField] private Button nextBtn;                                 // 다음 버튼

        [SerializeField] private Transform choiceBtnHolder;
        [SerializeField] private GameObject choiceBtnPrefab;

        // Queue 는 순차적으로 정보를 보여주는 형식의 List와 비슷한 것
        private Queue<string> _dialogueQueue;
        private DialogueSO _currentDialogueData;

        private Npc currentNpc;

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _dialogueQueue = new Queue<string>();
        }

        private void Start()
        {
            // 시작 때 대화창 숨기기
            dialoguePanel.SetActive(false);

            // 다음 버튼에 DisplayNextLine 을 미리 연결
            // nextBtn.onClick.AddListener(=> );
        }

        public void StartDialogue(DialogueSO dialogueData, Npc npc)
        {
            // 대화창 패널 활성화

            // 게임 시간 정지 / 플레이어 정지

            // 현재 대화 중인 NPC 저장

            // 현재 대화 데이터 저장 (선택지를 위해)

            // lineQueue 비우기 (clear)

            // SO 안의 모든 대사를 lineQueue 에 추가 (foreach, Enqueue)

            // 첫 번째 대사 표시
            //DisplayNextLine();
        }

        public void DisplayNextLine()
        {
            // 다음 버튼을 일단 보이게 함

            // lineQueue 에 남은 대사가 (count) 0 보다 큰가?

            // 남은 대사가 없다면 (else)
        }

        private void DisplayChoices()
        {
            // _currentDialogueData.choices 에 선택지가 0보다 많은가?

            // 기존에 있던 버튼들 삭제 후 선택지 버튼 생성

            // currentDialogueData.choices 배열을 순회 (foreach)

                // choiceBtnPrefab 을  choiceBtnHolder 자식으로 생성 (Instantiate)
                // 생성된 버튼의 텍스트를 지정
                // 생성된 버튼의 onClick 리스너에 새 기능 추가

                // (리스너 내부)선택한 다음대화 로 바로 넘어가기

            // else 선택지가 없다면 (대화 끝)
            // EndDialogue();
        }

        private void OnChoiceSelected(DialogueSO nextDialogue)
        {
            // 모든  선택지 버튼 삭제

            // 다음 대화가 있는지 확인 (null 체크)
                // 다음 대화로 이어서 시작

            // else 다음 대화가 없다면 (대화 끝)
            // EndDialogue();
        }

        private void EndDialogue()
        {
            // 대화창 패널 비활성화

            // 시간 되돌리기

            // NPC 참조 비우기

        }
    }
}