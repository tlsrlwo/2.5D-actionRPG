using UnityEngine;

namespace KW
{
    public class PlayerAttackState : MovementBaseState
    {
        public override void EnterState(PlayerMovement movement)
        {           
            movement.isAttacking = true;
            movement.anim.SetBool("isAttacking", true);            

            // "공격 방향" 결정
            float attackDirX;
            float attackDirZ;

            if (movement.dir.magnitude > 0.1f)
            {
                // 이동 키를 '누르고 있는 중' -> 현재 입력 사용
                attackDirX = movement.xInput;
                attackDirZ = movement.zInput;
            }
            else
            {
                // [If No] '가만히 서있는' 상태 -> "마지막" 입력 사용
                attackDirX = movement.lastMoveX;
                attackDirZ = movement.lastMoveZ;

                // [예외 처리] 마지막 방향도 없으면 (게임 시작 직후)
                if (Mathf.Abs(attackDirX) < 0.1f && Mathf.Abs(attackDirZ) < 0.1f)
                {
                    attackDirX = 0;
                    attackDirZ = 1f; // "정면" (Blend Tree의 Up 방향)
                }
            }

            // Blend Tree가 사용할 '방향' 파라미터 설정            
            movement.anim.SetFloat("AttackX", attackDirX);
            movement.anim.SetFloat("AttackZ", attackDirZ);
            

            // "Attack" 트리거 발동 (Any State -> Attack Blend Tree 상태로 이동)
            movement.anim.SetTrigger("isAttack");
            movement.sr.flipX = false;

        }

        public override void ExitState(PlayerMovement movement)
        {
            // "공격 끝" 플래그 해제
            movement.isAttacking = false;

            movement.anim.SetBool("isAttacking", false);

            // (안전장치) 히트박스가 혹시 켜져있다면 강제로 끔
            movement.AnimationEvent_DisableHitBox();
        }

        public override void UpdateState(PlayerMovement movement)
        {
          
        }
    }
}