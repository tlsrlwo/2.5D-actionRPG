using System;
using System.Collections.Generic;
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

        [SerializeField] private UiPanel currentOpenPanel = null;

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
            if (DialogueManager.isDialogueActive)
            {
                return;
            }

            if (currentOpenPanel != null && Input.GetKeyDown(KeyCode.Escape))   // 열려있는 게 있으면 우선적으로 닫음
            {
                TogglePanel(currentOpenPanel);
                return;
            }
            foreach (var panel in uiPanels)                                     // 열려있는 게 없으면 선택된 패널을 연다
            {
                if (Input.GetKeyDown(panel.key))
                {
                    TogglePanel(panel);
                    return;                                             // 한 프레임에 하나의 입력 처리
                }
            }
        }

        private void TogglePanel(UiPanel panelToToggle)
        {
            if (currentOpenPanel == panelToToggle)                      // 현재 열려있는 게 panelToToggle이면 -> 끄기
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
            // currenOpenPanel 이 있거나, 외부 팝업창이 열려있으면 true
            bool isAnyUiOpen = (currentOpenPanel != null);         

            if (isAnyUiOpen)
            {
                Time.timeScale = 0.0001f;
            }
            else
            {
                Time.timeScale = 1f;
            }

            ingameUi.SetActive(!isAnyUiOpen);
            systemCanvas.SetActive(isAnyUiOpen);

            foreach (var panel in uiPanels)
            {
                panel.isOpen = (panel == currentOpenPanel);             // 해당 panel 이 현재 열려있는 패널이면 isOpen
                panel.panelObject.SetActive(panel.isOpen);              // isOpen 이 true 면 해당 패널 오브젝트 SetActive
            }
            
            // OnAnyUiStateChanged?.Invoke(isAnyUiOpen);
        }

        // 패널의 버튼을 위한 함수
        public void OpenPanelByName(string panelName)
        {
            // 'IsPanelMatch'라는 이름의 함수를 조건으로 사용해 패널을 찾습니다.
            UiPanel panelToOpen = FindPanelUsingFunction(panelName);

            if (currentOpenPanel == panelToOpen)                         // 버튼으로 패널을 열 때, 현재 열려있는게 열어야 될 패널이면 버튼으로 꺼지지 않게끔 반환
            {
                return;
            }

            if (panelToOpen != null)                                    // 열어야 될 패널이 있으면(들어온 값이 있다), toggleOpen실행
            {
                TogglePanel(panelToOpen);
            }
        }

        private UiPanel FindPanelUsingFunction(string nameToFind)
        {
            // 리스트의 모든 패널을 처음부터 끝까지 확인합니다.
            foreach (var panel in uiPanels)
            {
                // 만약 현재 확인 중인 패널의 이름이 우리가 찾으려는 이름과 같다면
                if (panel.name == nameToFind)
                {
                    // 바로 그 패널을 반환하고 함수를 종료합니다.
                    return panel;
                }
            }

            // 루프가 끝날 때까지 찾지 못했다면 null을 반환합니다.
            return null;
        }
    }
}
