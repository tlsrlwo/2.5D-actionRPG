using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KW
{
    public class SaveManager : MonoBehaviour
    {
       public void SaveBtn()
        {
            Debug.Log("Save Button is Pressed");
        }

        public void LoadBtn()
        {
            Debug.Log("Load Button is Pressed");
        }
        public void DisplayBtn()
        {
            Debug.Log("Display Button is Pressed");
        }
       public void SoundBtn()
        {
            Debug.Log("Sound Button is Pressed");
        }
        public void ExitBtn()
        {
            Debug.Log("Exit Button is Pressed");

            // --- 게임 종료 로직 ---

            // 1. 실제 빌드된 게임(PC, Mac 등)에서 종료할 때 사용
            Application.Quit();

            // 2. 유니티 에디터에서 플레이 모드를 중지할 때 사용
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}
