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
            if (Input.GetKey(KeyCode.LeftShift)) ExitState(movement, movement.playerRun);
            else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.playerIdle);

            movement.currentSpeed = movement.walkSpeed;
        }

        private void ExitState(PlayerMovement movement, MovementBaseState state)
        {
            movement.anim.SetBool("isWalking", false);
            movement.SwitchState(state);
        }
    }
}
