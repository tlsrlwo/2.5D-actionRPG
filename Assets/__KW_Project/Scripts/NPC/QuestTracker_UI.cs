using UnityEngine;

namespace KW
{
    public class QuestTracker_UI : MonoBehaviour
    {
        [SerializeField] private GameObject trackerRowPrefab;
        [SerializeField] private Transform contentParent;

        private void Start()
        {
            if (QuestManager.Instance != null)
            {
                // 추적 목록이 변할 때 
                // 퀘스트 진행도가 변할 때
                QuestManager.Instance.OnTrackListUpdated += UpdateTracker;
            }

            UpdateTracker();
        }

        private void OnDestroy()
        {
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnTrackListUpdated -= UpdateTracker;
            }
        }
        
        public void UpdateTracker()
        {
            if (QuestManager.Instance == null) return;

            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            // 추적 중인 퀘스트가 없으면 패널 자체를 끔
            if (QuestManager.Instance.trackedQuests.Count == 0)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            foreach(Quest quest in QuestManager.Instance.trackedQuests)
            {
                // 완료된 건 트래커에서 뺌
                if (quest.isCompleted) continue;

                GameObject go = Instantiate(trackerRowPrefab, contentParent);
                QuestTrackerRow_UI row = go.GetComponent<QuestTrackerRow_UI>();

                row.Setup(quest);
            }
        }
    }
}
