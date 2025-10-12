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
            Debug.Log("스켈레톤 상태 진입 : CoolDown");
                        
            controller.agent.isStopped = true;

            controller.anim.SetBool("isCoolDown", true);
            controller.anim.SetBool("isChase", false);

            controller.sr.flipX = (controller.lastDirection.x < 0);
            controller.anim.SetFloat("xInput", controller.lastDirection.x);
            controller.anim.SetFloat("zInput", controller.lastDirection.y);

            coolDownTimer = controller.coolDownDuration;
        }       

        public override void UpdateState(SkeletonController controller)
        {
            coolDownTimer -= Time.deltaTime;

            if(coolDownTimer <= 0)
            {
                controller.SwitchState(controller.chaseState);
            }
        }

        public override void ExitState(SkeletonController controller)
        {
            controller.anim.SetBool("isCoolDown", false);
        }
    }
}
