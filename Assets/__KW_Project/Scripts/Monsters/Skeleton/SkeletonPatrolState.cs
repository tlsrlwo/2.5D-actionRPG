using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonPatrolState : MonsterBaseState<SkeletonController>
    {
        public override void EnterState(SkeletonController controller)
        {
            Debug.Log("스켈레톤 상태 진입 : Patrol");
            controller.agent.isStopped = false;                                     // navemshAgent 움직임 다시 활성화

            controller.anim.SetBool("isPatrol", true);                              // 걷는 애니메이션 활성화
                                                                                   
            controller.agent.speed = controller.patrolSpeed;                        

            // 목적지가 있는지 확인
            if (controller.wayPoints != null && controller.wayPoints.childCount > 0)
            {
                // navAgent 의 목적지 설정
                controller.agent.SetDestination(controller.wayPoints.GetChild(controller.wayPointIndex).position);
            }
            else
            {
                Debug.LogWarning("순찰 지점(WayPoints) 이 설정되지 않아 Idle 상태로 전환합니다");
                controller.SwitchState(controller.idleState);
            }
        }

        public override void UpdateState(SkeletonController controller)
        {
            if (controller.isDoingLunge)
            {
                return;
            }
            // 플레이어 감지
            if (controller.target == null)
            {
                Collider[] hits = Physics.OverlapSphere(controller.transform.position, controller.detectRange, controller.playerLayer);

                // 플레이어를 발견한다면
                if(hits.Length > 0)
                {
                    controller.target = hits[0].transform;
                    controller.SwitchState(controller.chaseState);
                    return;
                }
            }

            if (!controller.agent.pathPending && controller.agent.remainingDistance <= 0.3f)
            {
                controller.wayPointIndex++;                                         // 다음 목적지로 목적지 설정

                // wayPoint 순찰 완료 시 처음 wayPoint 로 초기화
                if(controller.wayPointIndex >= controller.wayPoints.childCount)
                {
                    controller.wayPointIndex = 0;
                }                

                controller.idleWaitTime = controller.suspiciousTime;                // 두리번시간 초기화
                controller.SwitchState(controller.idleState);

                return;
            }

            Vector3 skeletonVelocity = controller.agent.velocity;                   // 스켈레톤의 이도방향과 속도를 가져옴

            if (controller.agent.velocity.x > 0.1f)                                 // 방향에 맞게끔 sprite를 뒤집어줌
            {
                controller.sr.flipX = false;
            }
            else if (controller.agent.velocity.x < -0.1f)
            {
                controller.sr.flipX = true;
            }

            if (controller.agent.velocity.sqrMagnitude < 0.01f) return;

            Vector3 normalizedVelocity = controller.agent.velocity.normalized;
            controller.lastDirection = new Vector2(normalizedVelocity.x, normalizedVelocity.z);

            float blendTreeMoveX = Mathf.Abs(normalizedVelocity.x);

            controller.anim.SetFloat("xInput", blendTreeMoveX);
            controller.anim.SetFloat("zInput", normalizedVelocity.z);


        }

        public override void ExitState(SkeletonController controller)
        {
            controller.anim.SetBool("isPatrol", false);
        }

       
    }
}
