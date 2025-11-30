using UnityEngine;

namespace KW
{
    public class SystemCanvasUI : MonoBehaviour
    {
        private void Start()
        {
            if (UiManager.Instance != null)
            {
                UiManager.Instance.systemCanvasUIScript = this;
            }

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.systemCanvasUi = this;
            }
        }
    }
}
