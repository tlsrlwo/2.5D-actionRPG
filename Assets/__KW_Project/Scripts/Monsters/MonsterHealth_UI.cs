using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    public class MonsterHealth_UI : MonoBehaviour
    {
        [SerializeField] private MonsterHealth monsterHealth;

        [SerializeField] private Image healthBar;

        private void OnEnable()
        {
            monsterHealth = GetComponent<MonsterHealth>();

            if(monsterHealth != null)
            {
                monsterHealth.OnHealthChanged += UpdateHealthBar;

                UpdateHealthBar(monsterHealth.currentHp, monsterHealth.maxHp);           
                
                Debug.Log($"현재 몬스터의 체력 : {monsterHealth.currentHp}, {monsterHealth.maxHp}");
            }
               else
            {
                Debug.LogError("MonsterHealth_UI: MonsterHealth를 찾을 수 없습니다!");
            }
        }

        private void OnDisable()
        {
              if(monsterHealth != null)
            {
                monsterHealth.OnHealthChanged -= UpdateHealthBar;     
            }
        }

        private void UpdateHealthBar(float currentHp, float maxHp)
        {
            float ratio = 0f;

            if(maxHp > 0)
            {
                ratio = currentHp / maxHp;
            }

            if(healthBar != null)
            {
                healthBar.fillAmount = ratio;
            }
        }
    }
}
