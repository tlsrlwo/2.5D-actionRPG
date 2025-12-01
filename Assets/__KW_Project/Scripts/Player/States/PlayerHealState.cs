using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class PlayerHealState : MovementBaseState
    {
        private Potion potionToUse;

        public void SetPotion(Potion potion)
        {
            potionToUse = potion;
        }

        public override void EnterState(PlayerMovement movement)
        {
            movement.cController.Move(Vector3.zero);
            movement.canMove = false;

            movement.anim.SetTrigger("isHeal");

            Debug.Log("플레이어가 회복함");

            movement.StartCoroutine(HealRoutine(movement));

        }

        public override void ExitState(PlayerMovement movement)
        {
            movement.canMove = true;
            
            potionToUse = null; // 초기화
        }

        public override void UpdateState(PlayerMovement movement)
        {
        }

        private IEnumerator HealRoutine(PlayerMovement movement)
        {
            yield return new WaitForSeconds(1.0f);

            if (movement.playerHealth != null && potionToUse != null)
            {
                movement.playerHealth.Heal(potionToUse.healAmount);
                Debug.Log($"체력 {potionToUse.healAmount} 회복");

                if (movement.GetComponent<Inventory>() != null)
                {
                   movement.ConsumePotionFromQuickSlot();
                }
            }
            movement.SwitchState(movement.playerIdle);
        }
    }
}
