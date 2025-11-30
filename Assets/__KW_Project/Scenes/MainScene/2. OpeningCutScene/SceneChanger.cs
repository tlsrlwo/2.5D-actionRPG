using UnityEngine;
using UnityEngine.Video; // 비디오 기능을 위해 필요
using UnityEngine.SceneManagement; // 씬 전환을 위해 필요

public class SceneChanger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 인스펙터에서 할당할 비디오 플레이어
    public string nextSceneName;    // 이동할 씬의 이름

    void Start()
    {
        // 비디오 플레이어가 할당되지 않았다면 같은 오브젝트에서 찾음
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // 비디오 끝에 도달했을 때 실행될 함수를 등록 (이벤트 구독)
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // 비디오가 끝났을 때 호출되는 함수
    void OnVideoEnd(VideoPlayer vp)
    {
        // 다음 씬 로드
        SceneManager.LoadScene(nextSceneName);
    }
}