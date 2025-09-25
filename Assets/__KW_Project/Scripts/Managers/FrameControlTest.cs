using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class FrameControlTest : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
