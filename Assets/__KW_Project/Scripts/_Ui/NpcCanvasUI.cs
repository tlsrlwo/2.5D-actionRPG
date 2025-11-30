using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    public class NpcCanvasUI : MonoBehaviour
    {
        [Header("대화 UI 요소들")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI dialogueText;
        public Button nextBtn;
        public Transform choiceBtnHolder;
        public GameObject choiceBtnPrefab;

      
        private void Start()
        {
            Debug.Log("[NpcCanvasUI] 나 존재함");

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.npcCanvas = this;
            }
            dialoguePanel.SetActive(false);
        }
    }
}
