using UnityEngine;

namespace KW
{
    public class SystemCanvasUI : MonoBehaviour
    {
        public static SystemCanvasUI Instance { get; private set;   }
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
                UiManager.Instance.systemCanvasUIScript = this;
            }

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.systemCanvasUi = this;
            }
        }
    }
}
