using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KW
{
    [System.Serializable]
        public class Quest
        {
            public QuestSO data;            // 원본 데이터
            public int currentCount;        // 현재 잡은 수 
            public bool isCompleted;        // 완료 여부

            public Quest(QuestSO questData)
            {
                this.data = questData;
                this.currentCount = 0;
                this.isCompleted = false;
            }
        }
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance {get; private set;  }

        public List<Quest> activeQuests = new List<Quest>();            // 모든 퀘스트를 담을 리스트

        // 플레이어가 현재 수행 중인 퀘스트 목록
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
        }

        // 퀘스트 수락 함수 (NPC에서 호출)
        public void AcceptQuest(QuestSO questData)
        {
            // 중복방지
            foreach(var quest in activeQuests)
            {
                if(quest.data == questData)
                {
                    Debug.LogWarning("[QuestManager] 이미 수행중인 퀘스트입니다");

                    // pop-up 알림 등 효과 추가

                    return;
                }
            }

            Quest newQuest = new Quest(questData);
            activeQuests.Add(newQuest);

            Debug.Log($"[QuestManager]퀘스트 수락됨 : {questData.questTitle}");

            // 퀘스트 UI 업데이트
        }

    }
}
