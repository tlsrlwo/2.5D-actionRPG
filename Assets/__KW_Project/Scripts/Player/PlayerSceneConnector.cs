using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KW
{
    public class PlayerSceneConnector : MonoBehaviour
    {
     /*    private void Onable()
        {
            // 씬 로드 이벤트 구독
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        } */

        private void Start()
        {
            // 게임 처음 시작할 때도 연결
            ConnectToSceneComponents();            
        }

      /*   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "LoadingScene") return;

            // ConnectToSceneComponents();
        } */

        public void ConnectToSceneComponents()
        {
            // Cinemachine 연결
            CinemachineVirtualCamera vCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vCam != null)
            {
                vCam.Follow = this.transform;
                vCam.LookAt = this.transform;

                // 씬 전환 이후 카메라가 튀지 않게 워프 처린
                vCam.OnTargetObjectWarped(transform, transform.position - vCam.transform.position);

                Debug.Log("[PlayerSceneConnector] VC 연결 완료.");
            }
            else
            {
                Debug.LogError("[PlayerSceneConnector] VC 못 찾음.");

            }

            // Camera - DitherTranparency 연결
            if (Camera.main != null)
            {
                DitherTransparency dither = Camera.main.GetComponent<DitherTransparency>();

                dither.player = this.transform;

                Debug.Log("[PlayerSceneConnector] Dither 연결 완료.");
            }
            else
            {
                Debug.LogError("[PlayerSceneConnector] Dither 못 찾음.");

            }
        }
    }
}
