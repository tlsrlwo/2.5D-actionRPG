using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KW
{
    public class QuestTrackerRow_UI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI progressText;

        public void Setup(Quest quest)
        {
            titleText.text = quest.data.questTitle;

            if (quest.isCompleted)
            {
                progressText.text = "<color=green>완료!</color>";

            }
            else
            {
                progressText.text = $"{quest.currentCount} / {quest.data.targetCount}";
            }
        }
    }
}
