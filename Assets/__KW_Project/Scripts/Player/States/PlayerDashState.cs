using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerDashState : MovementBaseState
    {
        public override void EnterState(PlayerMovement movement)
        {
            movement.isDashing = true;

            //movement.sr.flipX = false;

            movement.anim.SetTrigger("isDashing");
            movement.anim.SetFloat("lastMoveX", movement.lastMoveX);
            movement.anim.SetFloat("lastMoveZ", movement.lastMoveZ);

            movement.StartCoroutine(DashCoroutine(movement));
        }


        public override void UpdateState(PlayerMovement movement)
        {
            
        }

        public override void ExitState(PlayerMovement movement)
        {
            movement.isDashing = false;
            movement.anim.ResetTrigger("isDashing");    
        }

        private IEnumerator DashCoroutine(PlayerMovement movement)
        {
            float startTime = Time.time;
            Vector3 dashDir;

            // 따로 입력되는 값이 없으면 이전에 움직였던 방향으로 대쉬
            if(movement.dir.magnitude > 0.1f)
            {
                dashDir = movement.dir;
            }
            else
            {
                dashDir = new Vector3(movement.lastMoveX, 0, movement.lastMoveZ);
                // 만약 게임 시작 직후라 lastMove 방향이 없으면 정면으로 나가게 예외처리
                if (dashDir.magnitude < 0.1f) dashDir = movement.transform.forward;
            }

            while (Time.time < startTime + movement.dashDuration)
            {
                movement.cController.Move(movement.dashSpeed * dashDir.normalized * Time.deltaTime);

                //movement.sr.flipX = false;

                yield return null;
            }

            movement.isDashing = false;

            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            if(Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
            {
                movement.SwitchState(movement.playerWalk);

            }
            else
            {
                movement.SwitchState(movement.playerIdle);
            }

            //movement.SwitchState(movement.previousState);
        }
    }
}
