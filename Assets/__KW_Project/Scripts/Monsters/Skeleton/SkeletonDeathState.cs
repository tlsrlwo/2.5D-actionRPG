using System.Collections;
using UnityEngine;

namespace KW
{
    public class SkeletonDeathState : MonsterBaseState<SkeletonController>
    {        
        private Color originalColor;
        
        public override void EnterState(SkeletonController controller)
        {
            // Debug.Log("스켈레톤 상태 진입 : Dead");

            controller.anim.SetTrigger("isDead");
            controller.anim.ResetTrigger("isAttack");

            controller.agent.isStopped = true;
            controller.agent.enabled = false;

            controller.rb.velocity = Vector3.zero;
            controller.rb.isKinematic = true;         

            Collider col = controller.GetComponent<Collider>();
            if(col != null)
            {
                col.enabled = false;    
            }

            if(controller.hitBox != null)
            {
                controller.hitBox.SetActive(false);
            }

            originalColor = controller.sr.color;
            controller.sr.color = new Color(1f, 0.5f, 0.5f, 0.7f);   

            // 보상 있을 시 드랍
            // controller.DropLootItem();

            GameObject.Destroy(controller.gameObject, 20f);
            // controller.healthCanvas.SetActive(false);
            controller.StartCoroutine(AfterDeathEffect(controller));
            
        }

        public override void UpdateState(SkeletonController controller)
        {
        }

        public override void ExitState(SkeletonController controller)
        {
        }

        private IEnumerator AfterDeathEffect(SkeletonController controller)
        {
            yield return new WaitForSeconds(0.2f);
            controller.sr.color = originalColor;

            yield return new WaitForSeconds(3f);

            controller.healthCanvas.SetActive(false);
        }       
    }
}
