using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerIdleState : MovementBaseState
    {
        public override void EnterState(PlayerMovement movement)
        {
            movement.anim.SetBool("isWalking", false);
            movement.anim.SetBool("isRunning", false);
        }

       
        public override void UpdateState(PlayerMovement movement)
        {
            if (movement.dir.magnitude > 0.1f)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift)) movement.SwitchState(movement.playerRun);
                else movement.SwitchState(movement.playerWalk);
            }
        }

        public override void ExitState(PlayerMovement movement)
        {
        }

    }
}
