using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerInteraction : MonoBehaviour
    {
        public float interactionDistance = 2f;
        public KeyCode interactionKey = KeyCode.E;

        private void Start()
        {
            Debug.Log("상호작용 ray 색 : ");
        }

        private void Update()
        {
            if(Input.GetKeyDown(interactionKey))
            {
                RaycastHit hit;

                if(Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
                {
                    // 맞은 물체에서 <IInteractable> 찾기
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if (interactable != null)
                    {
                        interactable.Interact(this.gameObject);
                    }
                }
            }
        }
    }
}
