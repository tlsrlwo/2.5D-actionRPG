using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonPatrolState : MonsterBaseState<SkeletonMovement>
    {
        public override void EnterState(SkeletonMovement controller)
        {
            Debug.Log("스켈레톤 patrol State 진입");
            controller.agent.isStopped = false;

            controller.anim.SetBool("isIdle", false);
            controller.anim.SetBool("isWalking", true);                         // 걷는 애니메이션 활성화
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

        public override void UpdateState(SkeletonMovement controller)
        {
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

        }

        public override void ExitState(SkeletonMovement controller)
        {
            
        }

       
    }
}
