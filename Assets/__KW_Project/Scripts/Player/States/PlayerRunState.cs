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
            if (Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement, movement.playerWalk);
            else if (movement.dir.magnitude < 0.1f) ExitState(movement, movement.playerIdle);

            movement.currentSpeed = movement.runSpeeed;
        }
        private void ExitState(PlayerMovement movement, MovementBaseState state)
        {
            movement.anim.SetBool("isRunning", false);
            movement.SwitchState(state);
        }
    }
}
