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

        [SerializeField] private GameObject miniMapUI;
        [SerializeField] private GameObject fullMapUI;
               
        private bool isMapFull = false;             // 맵의 상태 체크

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                isMapFull = !isMapFull;

                miniMapUI.SetActive(!isMapFull);
                fullMapUI.SetActive(isMapFull);

                OnMapStateChanged?.Invoke(isMapFull);
            }
        }
    }
}
