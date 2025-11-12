using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    // 상호작용을 위한
    public interface IInteractable
    {
        // 플레이어와 상호작용
        void Interact(GameObject player);
    }
}
