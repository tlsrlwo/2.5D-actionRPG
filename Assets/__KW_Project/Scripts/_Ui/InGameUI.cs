using UnityEngine;

namespace KW
{
    public class InGameUI : MonoBehaviour
    {
        public static InGameUI Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }



            if (UiManager.Instance != null)
            {
                UiManager.Instance.inGameUIScript = this;
            }
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ingameUi = this;
            }
               if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.ingameUi = this;
            }
            
        }
    }
}
