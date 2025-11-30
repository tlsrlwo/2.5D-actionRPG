using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KW
{
    public class DuskbornSceneManager : MonoBehaviour
    {
        public static DuskbornSceneManager Instance { get; private set; }

        public int nextSpawnPointID = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);

                // 씬이 로드될 때마다 실행될 함수 등록
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // 씬 로딩이 끝나면 자동 호출됨
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            
        }

    }
}
