using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleSceneUIController : MonoBehaviour
{
    // === 목표 알파 값 설정 (0 ~ 255) ===
    // 0에서 150까지 투명도를 증가시키도록 설정합니다.
    private const int TARGET_ALPHA_INT = 150;
    private readonly float TARGET_ALPHA_FLOAT = TARGET_ALPHA_INT / 255f;
    // ===================================

    // 페이드 효과를 적용할 버튼 GameObject (상위 오브젝트)
    [Header("UI GameObjects for Fading")]
    [SerializeField] private GameObject gameStartButtonObject;
    [SerializeField] private GameObject loadGameButtonObject;
    [SerializeField] private GameObject quitGameButtonObject;

    [Header("Appearance Timing (Seconds)")]
    // Game Start 버튼 등장 시작 지연 시간 (5.0초)
    [SerializeField] private float gameStartDelay = 5.0f;

    // Quit Game 버튼 등장 시작 지연 시간 (5.2초)
    [SerializeField] private float loadGameDelay = 5.2f;
    [SerializeField] private float quitGameDleay = 5.4f;

    [Header("Fade Settings")]
    // 페이드 인에 걸리는 시간 (0.5초 동안 투명도 0 -> TARGET_ALPHA_FLOAT)
    [SerializeField] private float fadeInDuration = 0.5f;

    void Start()
    {
        // 씬 시작 시 버튼들의 투명도를 0으로 초기화
        SetAlphaRecursive(gameStartButtonObject, 0f);
        SetAlphaRecursive(loadGameButtonObject, 0f);
        SetAlphaRecursive(quitGameButtonObject, 0f);

        // 버튼 등장 코루틴 시작
        StartCoroutine(ShowButtonsSequentially());
    }

    // 버튼을 순차적으로 지연 후 페이드 인 시키는 코루틴
    private IEnumerator ShowButtonsSequentially()
    {
        // Game Start 등장 시작 지점까지 대기 (5.0초)
        yield return new WaitForSeconds(gameStartDelay);

        Debug.Log($"Game Start 버튼 페이드 인 시작 (5.0초, 목표 Alpha: {TARGET_ALPHA_INT}/255)");
        // 페이드 인 코루틴 실행 및 완료까지 대기
        yield return StartCoroutine(FadeGameObject(gameStartButtonObject, TARGET_ALPHA_FLOAT, fadeInDuration));

        // Game Start 버튼 등장 완료 후, Quit Game 버튼 시작 지점까지 남은 시간 대기
        float delayBetweenFades = loadGameDelay - gameStartDelay;
        if (delayBetweenFades > 0)
        {
            yield return new WaitForSeconds(delayBetweenFades);
        }

        Debug.Log($"Quit Game 버튼 페이드 인 시작 (5.2초, 목표 Alpha: {TARGET_ALPHA_INT}/255)");
        // 페이드 인 코루틴 실행 및 완료까지 대기
        yield return StartCoroutine(FadeGameObject(loadGameButtonObject, TARGET_ALPHA_FLOAT, fadeInDuration));

        if (delayBetweenFades > 0)
        {
            yield return new WaitForSeconds(delayBetweenFades);
        }
        yield return StartCoroutine(FadeGameObject(quitGameButtonObject, TARGET_ALPHA_FLOAT, fadeInDuration));
    }

    // GameObject와 그 모든 자식 요소의 Graphic 컴포넌트 투명도를 목표 값으로 변경
    private IEnumerator FadeGameObject(GameObject rootObject, float targetAlpha, float duration)
    {
        Graphic[] graphics = rootObject.GetComponentsInChildren<Graphic>(true);

        if (graphics.Length == 0) yield break;

        List<Color> startColors = new List<Color>();
        foreach (var graphic in graphics)
        {
            startColors.Add(graphic.color);
        }

        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;

            for (int i = 0; i < graphics.Length; i++)
            {
                Color startColor = startColors[i];
                // 시작 알파 값(0)에서 목표 알파 값(TARGET_ALPHA_FLOAT)으로 부드럽게 보간
                float currentAlpha = Mathf.Lerp(startColor.a, targetAlpha, t);

                Color newColor = new Color(startColor.r, startColor.g, startColor.b, currentAlpha);
                graphics[i].color = newColor;
            }

            yield return null;
        }

        // 최종적으로 목표 알파 값으로 설정
        for (int i = 0; i < graphics.Length; i++)
        {
            Color startColor = startColors[i];
            Color finalColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
            graphics[i].color = finalColor;
        }
    }

    // 초기 설정 시 투명도를 일괄적으로 설정하는 헬퍼 함수
    private void SetAlphaRecursive(GameObject rootObject, float alpha)
    {
        Graphic[] graphics = rootObject.GetComponentsInChildren<Graphic>(true);
        foreach (var graphic in graphics)
        {
            Color color = graphic.color;
            graphic.color = new Color(color.r, color.g, color.b, alpha);
        }
    }
}