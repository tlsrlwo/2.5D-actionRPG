using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    public class QuestRow_UI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;                     // 퀘스트 이름
        [SerializeField] private TextMeshProUGUI progressText;                  // 퀘스트 진행도
        [SerializeField] private TextMeshProUGUI descriptionText;               // 퀘스트 설명
        [SerializeField] private Button trackBtn;                               // 추적 버튼
        [SerializeField] private TextMeshProUGUI trackBtnText;                  // 추적 버튼 텍스트

        private Quest _quest;                                                   // 내가 담당하는 퀘스트 데이터

        public void Setup(Quest quest)
        {
            _quest = quest;

            // 텍스트 설정
            titleText.text = quest.data.questTitle;

            if (quest.isCompleted)
            {
                progressText.text = "<color=green>완료!</color>";
            }
            else
            {
                progressText.text = $"{quest.currentCount} / {quest.data.targetCount}";
            }

            if (descriptionText != null)
            {
                descriptionText.text = quest.data.description;
            }

            UpdateTrackButtonState();

            trackBtn.onClick.RemoveAllListeners();
            trackBtn.onClick.AddListener(OnTrackButtonClicked);
        }

        public void OnButtonClicked()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.ToggleQuestTracking(_quest);

                // 버튼 상태 즉시 갱신
                UpdateTrackButtonState();
            }
        }

        public void OnTrackButtonClicked()
        {
            if(QuestManager.Instance != null)
            {
                QuestManager.Instance.ToggleQuestTracking(_quest);

                Debug.Log("[QuestRow_UI] 버튼 눌림");
                // 버튼 상태 즉시 갱신
                UpdateTrackButtonState();
            }
        }

        private void UpdateTrackButtonState()
        {
            if (QuestManager.Instance == null) return;

            bool isTracking = QuestManager.Instance.trackedQuests.Contains(_quest);

            if (isTracking)
            {
                trackBtnText.text = "현재 추적중";
            }
            else
            {
                trackBtnText.text = "추적하기";
            }
        }
    }
}
