using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace KW
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public static string nextSceneName;

        [Header("UI")]
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI loadingText;

        void Start()
        {
            StartCoroutine(LoadSceneCoroutine());
        }

        private IEnumerator LoadSceneCoroutine()
        {
            // 비동기 로드 시작
            AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
            op.allowSceneActivation = false;                                    // 로딩 끝나도 바로 넘어가지 않게 대기

            float timer = 0f;

            // 로딩 진행
            while (!op.isDone)
            {
                yield return null;
                timer += Time.deltaTime;

                // 진행률 0.9 에서 멈춤
                if (op.progress > 0.9f)
                {
                    if (progressBar != null) progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                }
                else
                {
                    if (progressBar != null) progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);

                    // 로딩이 다 됐고, 최소 1초는 지났다면
                    if(progressBar.value >= 0.99f && timer > 1.0f)
                    {
                        op.allowSceneActivation = true;         // 씬 전환 허용
                    }
                }
            }            
        }
    }
}
