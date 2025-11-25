using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonChaseState : MonsterBaseState<SkeletonController>
    {
        public override void EnterState(SkeletonController controller)
        {
            // Debug.Log("스켈레톤 상태 진입 : Chasing");

            controller.agent.isStopped = false;

            controller.anim.SetBool("isChase", true);
           
            controller.agent.speed = controller.chaseSpeed;

            controller.agent.stoppingDistance = 1;                                          // 플레이어한테 너무 붙지 않게끔 1로 설정(patrol에서 다시 0 으로 해줌)
        }   

        public override void UpdateState(SkeletonController controller)
        {
            if (controller.isDoingLunge)
            {
                return;
            }

            // 타겟이 없으면 patrol로 다시 돌아감
            if(controller.target == null)
            {
                controller.SwitchState(controller.patrolState);
                return;
            }

            controller.agent.SetDestination(controller.target.position);

            // 플레이어와의 공격범위 거리 체크
            float distanceToTarget = Vector3.Distance(controller.transform.position, controller.target.position);


            // 타겟을 목적지로 삼기
            if (distanceToTarget <= controller.attackRange)
            {
                controller.SwitchState(controller.attackState);
                return;
            }

            // 타겟을 놓쳤을 때
            if(distanceToTarget > controller.detectRange)
            {
                controller.target = null;
                controller.SwitchState(controller.patrolState);
                return;
            }

            if (controller.agent.velocity.sqrMagnitude < 0.01f) return;

            Vector3 normalizedVelocity = controller.agent.velocity.normalized;         // 스켈레톤의 이도방향과 속도를 가져옴
            controller.lastDirection = new Vector2(normalizedVelocity.x, normalizedVelocity.z);

            if (controller.agent.velocity.x > 0.1f)                                    // 방향에 맞게끔 sprite를 뒤집어줌
            {
                controller.sr.flipX = false;
            }
            else if (controller.agent.velocity.x < -0.1f)
            {
                controller.sr.flipX = true;
            }           

            float blendTreeMoveX = Mathf.Abs(normalizedVelocity.x);

            controller.anim.SetFloat("xInput", blendTreeMoveX);
            controller.anim.SetFloat("zInput", normalizedVelocity.z);

        }

        public override void ExitState(SkeletonController controller)
        {
            controller.anim.SetBool("isChase", false);
            controller.anim.ResetTrigger("isAttack");
        }
    }
}
