using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // 이동할 씬 이름을 "OpeningCutScene"으로 변경
    public string nextSceneName = "OpeningCutScene";

    // 인스펙터에서 할당할 효과음
    public AudioClip clickSound;

    // 오디오 소스 컴포넌트
    private AudioSource audioSource;

    void Awake() // Start() 대신 Awake()를 사용해 초기화 보장
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // TitleManager 오브젝트에 AudioSource가 없으면 자동으로 추가
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource 컴포넌트가 자동으로 추가되었습니다.");
        }
    }

    // 1. 게임 시작 버튼 기능
    public void StartGame()
    {
        // 1. 효과음 재생
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // 2. 씬 전환 (클릭음은 짧으므로 즉시 로드)
        // 주의: OpeningCutScene 씬이 Build Settings에 추가되어 있어야 합니다.
        SceneManager.LoadScene(nextSceneName);
    }

    // 2. 게임 종료 버튼 기능
    public void QuitGame()
    {
        // (선택 사항) 종료 효과음이 있다면 여기서 재생

        // 즉시 게임 종료
#if UNITY_EDITOR
        // 에디터에서 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 빌드된 게임 종료
            Application.Quit();
#endif
        Debug.Log("게임 종료 요청");
    }
}