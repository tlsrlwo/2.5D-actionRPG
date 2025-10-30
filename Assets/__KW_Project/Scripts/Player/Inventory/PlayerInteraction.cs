using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactionDistance = 1f;
        public KeyCode interactionKey = KeyCode.E;

        private Color rayColor = Color.blue;

        private void Start()
        {
            Debug.Log($"상호작용 ray 색 : {rayColor}");
        }

        private void Update()
        {
            // E 키를 누르는 '순간'
            if (Input.GetKeyDown(interactionKey))
            {
                RaycastHit hit;
                Vector3 startPos = transform.position;
                Vector3 direction = transform.forward; // 플레이어의 정면

                // 1. Raycast 발사
                if (Physics.Raycast(startPos, direction, out hit, interactionDistance))
                {
                    // 맞은 물체에서 <IInteractable> 찾기
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if (interactable != null)
                    {
                        interactable.Interact(this.gameObject);
                    }

                    // (성공!) 1초 동안 '녹색' 광선을 '맞은 곳까지만' 그림
                    Debug.DrawRay(startPos, direction * hit.distance, rayColor, 1.0f);
                }
                // 2. Raycast가 아무것도 맞히지 못했을 때
                else
                {
                    // (실패!) 1초 동안 '빨간색' 광선을 '최대 거리까지' 그림
                    Debug.DrawRay(startPos, direction * interactionDistance, rayColor, 1.0f);
                }
            }
        }
    }
}
