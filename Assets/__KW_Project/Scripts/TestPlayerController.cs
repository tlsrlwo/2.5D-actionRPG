using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private CharacterController cc;

    float xInput, zInput;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

    }

    private void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector2 move = cc.



    }
}
