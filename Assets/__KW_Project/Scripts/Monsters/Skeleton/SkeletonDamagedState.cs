using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class SkeletonDamagedState : MonsterBaseState<SkeletonController>
    {
        [Header("변수")]        
        private float _stunDuration = 0.4f;              // 경직 시간
        private float _stunTimer;

        public float stunTimer => _stunTimer;

        private float _knockBackForce = 4f;              // 넉백 힘
        private Color originalColor;                    // 원래 색상 저장용

        public override void EnterState(SkeletonController controller)
        {
            Debug.Log("스켈레톤 상태 진입 : Damaged");

            // 초기화 및 정지
            _stunTimer = _stunDuration;
            controller.agent.isStopped = true;
            controller.isDoingLunge = false;

            // 붉고 반투명하게 변경
            originalColor = controller.sr.color;
            controller.sr.color = new Color(1f, 0.5f, 0.5f, 0.7f);      // (R:1, G:0.5, B:0.5 -> 붉은끼, Alpha:0.7 -> 반투명)

            controller.anim.SetFloat("HitX", controller.lastDamagedDirection.x);
            controller.anim.SetFloat("HitZ", controller.lastDamagedDirection.z);

            // 피격 트리거 발동
            controller.anim.SetTrigger("isHit");

            // 물리 힘 가하기
            // NavMeshAgent와 물리 충돌이 싸우지 않도록 kinematic 잠시 해제
            controller.rb.isKinematic = false;

            // Controller에 저장된 '피격 방향(밀려날 방향)'으로 힘을 가함
            controller.rb.AddForce(controller.lastDamagedDirection * _knockBackForce, ForceMode.Impulse);
        }

        public override void UpdateState(SkeletonController controller)
        {
            // 시간 감소
            _stunTimer -= Time.deltaTime;

            // 경직 시간이 끝나면 상태 전환
            if (_stunTimer <= 0)
            {
                // 타겟(플레이어)이 여전히 있다면 추격, 놓쳤다면 Idle
                if (controller.target != null)
                {
                    controller.SwitchState(controller.chaseState);
                }
                else
                {
                    controller.SwitchState(controller.idleState);
                }
            }
        }

        public override void ExitState(SkeletonController controller)
        {
            controller.anim.ResetTrigger("isAttack");

            // [시각 효과] 색상 원상복구
            controller.sr.color = originalColor;

            // 물리력 초기화 (미끄러짐 방지)
            controller.rb.velocity = Vector3.zero;
            controller.rb.isKinematic = true;

            // 이동 재개 허용
            controller.agent.isStopped = false;
        }
    }
}
