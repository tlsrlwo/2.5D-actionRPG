using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonAttackState : MonsterBaseState<SkeletonController>
    {
        // 평상시의 이동을 navMesh 로 하지만 공격할때는 rigidbody를 사용

        private bool hasSwitchedState;              // attack 이 실행되는 동안 update 문이 다시 실행되지 않게 하기 위함
        public override void EnterState(SkeletonController controller)
        {
            // Debug.Log("스켈레톤 상태 진입 : Attack");
            hasSwitchedState = false;

            // navAgent 는 멈춰줌
            controller.agent.isStopped = true;

            if (controller.target != null)
            {
                // 플레이어의 방향을 계산
                Vector3 dirToPlayer = controller.target.position - controller.transform.position;
                dirToPlayer.y = 0;

                // LungeRoutine 을 위해 방향값 저장
                controller.lastAttackDirection = dirToPlayer.normalized;

                // 스프라이트 & blend tree 파라미터 설정            
                // controller.sr.flipX = (dirToPlayer.x < 0);                                   // 스프라이트 반전
                controller.sr.flipX = false;
                
                float blendTreeMoveX = dirToPlayer.x;                                           // 애니메이터에 전달할 값
                float blendTreeMoveZ = dirToPlayer.z;
               
                controller.anim.SetFloat("xInput", blendTreeMoveX);                             // 애니메이터 설정
                controller.anim.SetFloat("zInput", blendTreeMoveZ);
            }
            // trigger 애니메이션
            controller.anim.SetTrigger("isAttack");
        }


        public override void UpdateState(SkeletonController controller)
        {
            if (controller.isDoingLunge)
            {
                return;
            }
            if (hasSwitchedState) return;

            // 타겟이 사라졌으면 순찰로 복귀
            if (controller.target == null)
            {
                controller.SwitchState(controller.patrolState);
                hasSwitchedState = true;
                return;
            }

              /*// 플레이어가 공격범위를 벗어났는지 확인
            float distanceToTarget = Vector3.Distance(controller.transform.position, controller.target.position);
            if (distanceToTarget > controller.attackRange)
            {
                controller.SwitchState(controller.chaseState);
                hasSwitchedState = true;
                return;
            }*/

            // 현재 애니메이션이 'Attack' 이고, 재생이 끝났는지 확인
            AnimatorStateInfo animStateInfo = controller.anim.GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsName("Attack") && animStateInfo.normalizedTime >= 1.0f)
            {
                // 애니메이션이 끝나면, 잠시 딜레이를 주기 위해 coolDownState 로 전환
                controller.SwitchState(controller.coolDownState);
                hasSwitchedState = true;
            }
        }


        public override void ExitState(SkeletonController controller)
        {
            controller.anim.ResetTrigger("isAttack");
        }
    }
}
