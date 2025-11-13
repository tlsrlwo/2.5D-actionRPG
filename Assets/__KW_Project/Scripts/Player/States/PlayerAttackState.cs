using Unity.VisualScripting;
using UnityEngine;

namespace KW
{
    public class PlayerAttackState : MovementBaseState
    {

        public override void EnterState(PlayerMovement movement)
        {
            // isAttacking' true로 설정
            movement.isAttacking = true;

            // 공격 방향 결정
            float attackDirX;
            float attackDirZ;

            if (movement.dir.magnitude > 0.1f)
            {
                attackDirX = movement.xInput;
                attackDirZ = movement.zInput;
            }
            else
            {
                attackDirX = movement.lastMoveX;
                attackDirZ = movement.lastMoveZ;
            }

            movement.anim.SetFloat("AttackX", attackDirX);
            movement.anim.SetFloat("AttackZ", attackDirZ);

            // Attack" 애니메이션 트리거 발동
            movement.anim.SetTrigger("isAttack");
        }


        public override void UpdateState(PlayerMovement movement)
        {
           
        }
        public override void ExitState(PlayerMovement movement)
        {
            movement.isAttacking = false;

            // (안전장치) 히트박스가 혹시 켜져있다면 강제로 끔
            movement.AnimationEvent_DisableHitBox();
        }  
    }
}