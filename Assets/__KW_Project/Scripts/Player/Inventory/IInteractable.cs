using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    // 상호작용을 위한
    public interface IInteractable
    {
        // 코드 적기 전에 한글폰트 보이게 다른이름으로저장 -> 인코딩 하기 !!
        // 플레이어와 상호작용
        void Interact(GameObject plyaer);
    }
}
