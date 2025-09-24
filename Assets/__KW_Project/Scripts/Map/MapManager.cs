using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    public class MapManager : MonoBehaviour
    {
        public static event Action<bool> OnMapStateChanged;

        [SerializeField] private GameObject mapIcon;
        [SerializeField] private GameObject fullMapUI;

        private bool isMapFull = false;             // 맵의 상태 체크

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                isMapFull = !isMapFull;

                UpdateMapUI();

                OnMapStateChanged?.Invoke(isMapFull);
            }

            // 맵이 켜져있을 때 M 혹은 Esc 로도 종료 가능
            if(isMapFull && Input.GetKeyDown(KeyCode.Escape))
            {
                isMapFull = false;

                UpdateMapUI();

                OnMapStateChanged?.Invoke(isMapFull);
            }
        }

        private void UpdateMapUI()
        {
            mapIcon.SetActive(!isMapFull);
            fullMapUI.SetActive(isMapFull);
        }
    }
}
