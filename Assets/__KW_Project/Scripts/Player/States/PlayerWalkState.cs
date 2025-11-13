using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerWalkState : MovementBaseState
    {
        public override void EnterState(PlayerMovement movement)
        {
            movement.anim.SetBool("isWalking", true);
        }

        public override void UpdateState(PlayerMovement movement)
        {
            movement.currentSpeed = movement.walkSpeed;          

            if (Input.GetKey(KeyCode.LeftShift)) movement.SwitchState(movement.playerRun);
            else if (movement.dir.magnitude < 0.1f) movement.SwitchState(movement.playerIdle);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                movement.previousState = this;
                movement.SwitchState(movement.dashState);
            }
        }

        public override void ExitState(PlayerMovement movement)
        {
            movement.anim.SetBool("isWalking", false);            
        }
    }
}
