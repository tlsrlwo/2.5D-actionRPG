using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class SkeletonCoolDownState : MonsterBaseState<SkeletonController>
    {
        private float coolDownTimer;

        public override void EnterState(SkeletonController controller)
        {
            // Debug.Log("스켈레톤 상태 진입 : CoolDown");
                        
            controller.agent.isStopped = true;

            controller.anim.SetBool("isCoolDown", true);
            controller.anim.SetBool("isChase", false);

            controller.sr.flipX = false;

            controller.anim.SetFloat("xInput", controller.lastDirection.x);
            controller.anim.SetFloat("zInput", controller.lastDirection.y);

            coolDownTimer = controller.coolDownDuration;
        }       

        public override void UpdateState(SkeletonController controller)
        {
            coolDownTimer -= Time.deltaTime;

            // 플레이어와의 공격범위 거리 체크
            float distanceToTarget = Vector3.Distance(controller.transform.position, controller.target.position);

            if (coolDownTimer <= 0)
            {
                // 플레이어가 아직 detectRange 안에 있다면
                if (controller.target != null && (distanceToTarget <= controller.detectRange))
                {
                    // 추격(Chase) 상태로 복귀
                    controller.SwitchState(controller.chaseState);
                }
                else
                {
                    // 범위를 벗어났거나 타겟이 없다면 순찰 상태로 복귀
                    controller.target = null; 
                    controller.SwitchState(controller.patrolState);
                }
            }
        }

        public override void ExitState(SkeletonController controller)
        {
            controller.anim.SetBool("isCoolDown", false);
        }
    }
}
