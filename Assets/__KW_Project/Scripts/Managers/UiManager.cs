using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    [System.Serializable]
    public class UiPanel
    {
        public string name;
        public KeyCode key;
        public GameObject panelObject;
        public bool isOpen = false;
    }

    public class UiManager : MonoBehaviour
    {
        public static event Action<bool> OnAnyUiStateChanged;           // 패널이 열고 닫을 때 호출 되는 이벤트 (PlayerMovement에서구독함)

        [SerializeField] private GameObject ingameUi;
        [SerializeField] private GameObject systemCanvas;

        [SerializeField] private List<UiPanel> uiPanels;                // 각 (인벤/맵/설정/퀘스트) 패널을 담을 리스트

        private UiPanel currentOpenPanel = null;

        private void Start()
        {
            foreach (var panel in uiPanels)
            {
                panel.panelObject.SetActive(false);                     // 패널 클래스를 가진 패널 각각 setActive(false)
                panel.isOpen = false;
            }
            ingameUi.SetActive(true);                                   // 게임 실행 시 인게임 캔버스 on
            systemCanvas.SetActive(false);                              // 게임 실행 시 시스템 캔버스 off
        }

        private void Update()
        {
            foreach (var panel in uiPanels)
            {
                if (Input.GetKeyDown(panel.key))
                {
                    TogglePanel(panel);
                    return;                                             // 한 프레임에 하나의 입력 처리
                }
            }
            if (currentOpenPanel != null && Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePanel(currentOpenPanel);
            }
        }

        private void TogglePanel(UiPanel panelToToggle)
        {
            if (currentOpenPanel == panelToToggle)                       // 현재 열려있는 게 panelToToggle이면 -> 끄기
            {
                currentOpenPanel = null;
            }
            else                                                        // 열려있는게 없으면                  -> 열기
            {
                currentOpenPanel = panelToToggle;
            }
            UpdateAllPanelViews();
            OnAnyUiStateChanged?.Invoke(currentOpenPanel != null);      // currentOpenPanel 이 있으면 이벤트 호출
        }

        private void UpdateAllPanelViews()
        {
            bool isAnyPanelOpen = currentOpenPanel != null;             // currenOpenPanel 이 있으면 true

            ingameUi.SetActive(!isAnyPanelOpen);
            systemCanvas.SetActive(isAnyPanelOpen);

            foreach (var panel in uiPanels)
            {
                panel.isOpen = (panel == currentOpenPanel);             // 해당 panel 이 현재 열려있는 패널이면 isOpen
                panel.panelObject.SetActive(panel.isOpen);              // isOpen 이 true 면 해당 패널 오브젝트 SetActive
            }
        }
    }  
}
