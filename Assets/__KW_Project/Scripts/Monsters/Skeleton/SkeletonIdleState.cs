using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonIdleState : MonsterBaseState<SkeletonController>
    {
        private float _idleTimer;

        public float idleTimer => _idleTimer;

        public override void EnterState(SkeletonController controller)
        {
            // Debug.Log("스켈레톤 상태 진입 : Idle");
                       
            controller.anim.SetBool("isIdle", true);
           
            controller.agent.isStopped = true;                          // navMesh의 이동을 멈춰줌
                        
            _idleTimer = controller.idleWaitTime;                        // 컨트롤러에 설정된 대기 시간으로 타이머 초기화
        }        

        public override void UpdateState(SkeletonController controller)
        {
            if (controller.isDoingLunge)
            {
                return;
            }
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

            if (_idleTimer > 0 )                                          // 대기 시간을 매 초 감소시킴
            {
                _idleTimer -= Time.deltaTime;    
            }
            else                                                        // 대기 시간이 끝나면 순찰 상태로 전환
            {
                controller.SwitchState(controller.patrolState);
            }
        }

        public override void ExitState(SkeletonController controller)
        {
            // 애니메이션 idle 상태 false
            controller.anim.SetBool("isIdle", false);

            // 이동할 수 있도록 isStopped 해제
            controller.agent.isStopped = false;
        }
    }
}
