using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    public class UiManager : MonoBehaviour
    {
        public static event Action<bool> OnMapStateChanged;
        public static event Action<bool> OnInventoryStateChanged;
        public static event Action<bool> OnSettingsStateChanged;
        public static event Action<bool> OnQuestStateChanged;

        [SerializeField] private GameObject ingameUI;

        [SerializeField] private GameObject systemCanvas;                   // 전체 캔버스

        [SerializeField] private GameObject inventoryCanvas;                // 인벤토리
        [SerializeField] private GameObject mapCanvas;                      // 맵
        [SerializeField] private GameObject settingsCanvas;                 // 설정
        [SerializeField] private GameObject questCanvas;                    // 퀘스트

        private bool isMapFull = false;                                     // 맵 화면 상태 체크
        private bool isInventoryFull = false;                               // 인벤토리 화면 상태 체크
        private bool isSettingsFull = false;                                // 설정화면 상태 체크
        private bool isQuestFull = false;                                   // 퀘스트 화면 상태 체크

        private void Awake()
        {
            ingameUI.SetActive(true);
            systemCanvas.SetActive(false);
        }

        private void Update()
        {
            ActivateMapScreen();
            ActivateInventoryScreen();
            ActivateSettingScreen();
            ActivateQuestScreen();
        }

        #region 지도 UI 
        private void ActivateMapScreen()
        {
            if (isInventoryFull || isSettingsFull || isQuestFull)
                return;

            if (Input.GetKeyDown(KeyCode.M))
            {
                isMapFull = !isMapFull;

                UpdateMapUI();

                OnMapStateChanged?.Invoke(isMapFull);
            }

            // 맵이 켜져있을 때 M 혹은 Esc 로도 종료 가능
            if (isMapFull && Input.GetKeyDown(KeyCode.Escape))
            {
                isMapFull = false;

                UpdateMapUI();

                OnMapStateChanged?.Invoke(isMapFull);
            }
        }

        private void UpdateMapUI()
        {
            ingameUI.SetActive(!isMapFull);
            systemCanvas.SetActive(isMapFull);
            inventoryCanvas.SetActive(!isMapFull);
            mapCanvas.SetActive(isMapFull);
            settingsCanvas.SetActive(!isMapFull);
            questCanvas.SetActive(!isMapFull);
        }
        #endregion

        #region 인벤토리 UI
        private void ActivateInventoryScreen()
        {
            if (isMapFull || isSettingsFull || isQuestFull)
                return;

            if (Input.GetKeyDown(KeyCode.I))
            {
                isInventoryFull = !isInventoryFull;

                UpdateInventoryUI();

                OnInventoryStateChanged?.Invoke(isInventoryFull);
            }

            if (isInventoryFull && Input.GetKeyDown(KeyCode.Escape))
            {
                isInventoryFull = false;

                UpdateInventoryUI();

                OnInventoryStateChanged?.Invoke(isInventoryFull);
            }
        }

        private void UpdateInventoryUI()
        {
            ingameUI.SetActive(!isInventoryFull);
            systemCanvas.SetActive(isInventoryFull);
            inventoryCanvas.SetActive(isInventoryFull);
            mapCanvas.SetActive(!isInventoryFull);
            settingsCanvas.SetActive(!isInventoryFull);
            questCanvas.SetActive(!isInventoryFull);
        }
        #endregion

        #region 설정 UI
        private void ActivateSettingScreen()
        {
            if (isMapFull || isInventoryFull || isQuestFull)
                return;

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab))              // 에디터
            {
                isSettingsFull = !isSettingsFull;

                UpdateSettingUI();

                OnSettingsStateChanged?.Invoke(isSettingsFull);
            }
#else
    if (Input.GetKeyDown(KeyCode.Escape))                   // 빌드 시
            {
                isSettingsFull = !isSettingsFull;

                UpdateSettingUI();

                OnSettingsStateChanged?.Invoke(isSettingsFull);
            }
#endif
            else if (isSettingsFull && Input.GetKeyDown(KeyCode.Escape))
            {
                isSettingsFull = false;

                UpdateSettingUI();

                OnSettingsStateChanged?.Invoke(isSettingsFull);
            }
        }

        private void UpdateSettingUI()
        {
            ingameUI.SetActive(!isSettingsFull);
            systemCanvas.SetActive(isSettingsFull);
            settingsCanvas.SetActive(isSettingsFull);

            inventoryCanvas.SetActive(!isSettingsFull);
            mapCanvas.SetActive(!isSettingsFull);
            questCanvas.SetActive(!isSettingsFull);
        }

        #endregion

        #region 퀘스트 UI
        private void ActivateQuestScreen()
        {
            if (isMapFull || isSettingsFull || isInventoryFull)
                return;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                isQuestFull = !isQuestFull;

                UpdateQeustUI();

                OnQuestStateChanged?.Invoke(isQuestFull);
            }

            if (isQuestFull && Input.GetKeyDown(KeyCode.Escape))
            {
                isQuestFull = false;

                UpdateQeustUI();

                OnQuestStateChanged?.Invoke(isQuestFull);
            }
        }
        private void UpdateQeustUI()
        {
            ingameUI.SetActive(!isQuestFull);
            systemCanvas.SetActive(isQuestFull);
            questCanvas.SetActive(isQuestFull);

            inventoryCanvas.SetActive(!isQuestFull);
            mapCanvas.SetActive(!isQuestFull);
            settingsCanvas.SetActive(!isQuestFull);
        }

        #endregion

    }
}
