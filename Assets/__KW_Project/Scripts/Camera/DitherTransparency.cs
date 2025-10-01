using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace KW
{
    public class DitherTransparency : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask occluderLayer;
        [SerializeField, Range(0f, 1f)] private float transparentAlpha = 0.2f;
        [SerializeField] private float fadeSpeed = 5f;

        private Camera mainCamera;
        private Dictionary<Renderer, float> managedRenderers = new Dictionary<Renderer, float>();

        private void Awake()
        {
            mainCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (player == null || mainCamera == null) return;

            // 1. 현재 프레임에서 플레이어를 가리는 오브젝트들을 찾습니다.
            Vector3 direction = (player.position - mainCamera.transform.position).normalized;
            float distance = Vector3.Distance(mainCamera.transform.position, player.position);
            RaycastHit[] hits = Physics.RaycastAll(mainCamera.transform.position, direction, distance, occluderLayer);

            HashSet<Renderer> currentOccluders = new HashSet<Renderer>();
            foreach (var hit in hits)
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend != null)
                {
                    currentOccluders.Add(rend);
                }
            }

            // --- 여기가 수정된 부분입니다 ---
            // 2. 관리 목록에 없는 새로운 Occluder들을 먼저 추가합니다.
            foreach (var rend in currentOccluders)
            {
                if (!managedRenderers.ContainsKey(rend))
                {
                    // 새로 감지된 오브젝트는 완전히 불투명한 상태(알파 1)로 추가합니다.
                    managedRenderers.Add(rend, 1f);
                }
            }

            // 3. 관리 목록에 있는 모든 Renderer의 알파 값을 갱신합니다.
            List<Renderer> toRemove = new List<Renderer>();
            // 딕셔너리를 직접 순회하면 수정 시 에러가 날 수 있으므로 키 목록을 복사해서 사용합니다.
            List<Renderer> rendererKeys = new List<Renderer>(managedRenderers.Keys);

            foreach (var rend in rendererKeys)
            {
                float currentAlpha = managedRenderers[rend];
                float targetAlpha = currentOccluders.Contains(rend) ? transparentAlpha : 1f;

                float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);

                Material[] materials = rend.materials;
                foreach (Material mat in materials)
                {
                    if (mat.HasProperty("_BaseColor"))
                    {
                        Color color = mat.GetColor("_BaseColor");
                        color.a = newAlpha;
                        mat.SetColor("_BaseColor", color);
                    }
                }

                managedRenderers[rend] = newAlpha;

                if (Mathf.Approximately(newAlpha, 1f) && !currentOccluders.Contains(rend))
                {
                    toRemove.Add(rend);
                }
            }

            // 4. 완전히 불투명해진 오브젝트는 관리 목록에서 제거합니다.
            foreach (var rend in toRemove)
            {
                managedRenderers.Remove(rend);
            }
        }
    }
}