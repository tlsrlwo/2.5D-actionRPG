using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KW
{
    public class PlayerRunState : MovementBaseState
    {
        public override void EnterState(PlayerMovement movement)
        {
            movement.anim.SetBool("isRunning", true);
        }

        public override void UpdateState(PlayerMovement movement)
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                movement.SwitchState(movement.playerWalk);
                return;
            }
            else if (movement.dir.magnitude < 0.1f)
            {
                movement.SwitchState(movement.playerIdle);
            }

            movement.currentSpeed = movement.runSpeed;
        }
        public override void ExitState(PlayerMovement movement)
        {
            movement.anim.SetBool("isRunning", false);
           
        }
    }
}
