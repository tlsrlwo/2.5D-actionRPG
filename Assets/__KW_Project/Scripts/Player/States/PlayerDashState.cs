using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerDashState : MovementBaseState
    {
        public override void EnterState(PlayerMovement movement)
        {
          
        }

        public override void ExitState(PlayerMovement movement)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState(PlayerMovement movement)
        {

        }

        private void ExitState(PlayerMovement movement, MovementBaseState state)
        {
            movement.SwitchState(state);
        }
    }
}
