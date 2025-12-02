using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KW
{
    public class CutSceneSkipBtn : MonoBehaviour
    {
        public string nextSceneName;

        public void SkipCutSceneBtn()
        {
            SceneManager.LoadScene(nextSceneName);
        }


    }
}
