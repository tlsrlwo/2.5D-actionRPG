using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KW
{
    public class MapChangePortal : MonoBehaviour
    {
        [Header("이동할 씬 이름")]
        [SerializeField] private string sceneToLoad;

        [SerializeField] private int spawnPointID;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // DuskbornSceneManager.Instance.nextSpawnPointID = spawnPointID;

                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
