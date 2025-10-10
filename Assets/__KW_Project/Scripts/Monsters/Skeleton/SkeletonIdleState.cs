using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonIdleState : MonsterBaseState<SkeletonMovement>
    {
        private float idleTimer;

        public override void EnterState(SkeletonMovement controller)
        {
            Debug.Log("스켈레톤 상태 진입 : Idle");
            
            controller.anim.SetBool("isWalking", false);                // 다른 애니메이션 bool(false)로 설정
            //controller.anim.SetBool("isRunning", false);
            controller.anim.SetBool("isIdle", true);
           
            controller.agent.isStopped = true;                          // navMesh의 이동을 멈춰줌

            // //// 여기 부분을 patrol 에서 지정해주기 때문에 불필요한 거 아닌가
            idleTimer = controller.idleWaitTime;                        // 컨트롤러에 설정된 대기 시간으로 타이머 초기화
        }        

        public override void UpdateState(SkeletonMovement controller)
        {          
            // 플레이어 지정해주기
            if (controller.target == null)
            {
                Collider[] hits = Physics.OverlapSphere(controller.transform.position, controller.detectRange, controller.playerLayer);

                // 플레이어를 발견한다면 (hit에 감지된 값이 있을 시)
                if (hits.Length > 0)
                {
                    controller.target = hits[0].transform;
                    controller.SwitchState(controller.chaseState);
                    return;
                }
            }

            if (idleTimer > 0 )                                          // 대기 시간을 매 초 감소시킴
            {
                idleTimer -= Time.deltaTime;    
            }
            else                                                        // 대기 시간이 끝나면 순찰 상태로 전환
            {
                controller.SwitchState(controller.patrolState);
            }
        }

        public override void ExitState(SkeletonMovement controller)
        {
            // 애니메이션 idle 상태 false
            controller.anim.SetBool("isIdle", false);

            // 이동할 수 있도록 isStopped 해제
            controller.agent.isStopped = false;

        }
    }
}
