using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class QuestMenu_UI : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private GameObject questRowPrefab;                     // Ui 에 표시될 퀘스트 내용 prefab
        [SerializeField] private Transform contentParent;                       // 퀘스트prefab을 담을 곳


        private void OnEnable()
        {
            UpdateQuestList();

            if(QuestManager.Instance != null)
            {
                QuestManager.Instance.OnQuestListUpdated += UpdateQuestList;
            }
        }

        private void OnDisable()
        {
             if(QuestManager.Instance != null)
            {
                QuestManager.Instance.OnQuestListUpdated -= UpdateQuestList;
            }
        }
        
        public void UpdateQuestList()
        {
            // 기준 목록 초기화
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            if (QuestManager.Instance == null) return;

            // 현재 활성화된 퀘스트 목록 가져오기
            List<Quest> activeQuests = QuestManager.Instance.activeQuests;

            // 하나씩 생성
            foreach (Quest quest in activeQuests)
            {
                GameObject go = Instantiate(questRowPrefab, contentParent);

                QuestRow_UI row = go.GetComponent<QuestRow_UI>();

                // 데이터 주입
                row.Setup(quest);
            }

        }
    }
}
