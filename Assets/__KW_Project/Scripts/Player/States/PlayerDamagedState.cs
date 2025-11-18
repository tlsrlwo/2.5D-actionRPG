using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerDamagedState : MovementBaseState
    {
        // 변수
        private float _stunDuration = 0.5f;
        private float _stunTime;
        public float stunTime => _stunTime;

        private float knockBackForce = 5f;


        public override void EnterState(PlayerMovement movement)
        {
            _stunTime = _stunDuration;


        }

        public override void ExitState(PlayerMovement movement)
        {

        }

        public override void UpdateState(PlayerMovement movement)
        {

        }
    }
}
