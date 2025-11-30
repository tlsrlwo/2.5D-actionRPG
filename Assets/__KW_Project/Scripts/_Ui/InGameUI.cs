using UnityEngine;

namespace KW
{
    public class InGameUI : MonoBehaviour
    {
        private void Start()
        {
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
