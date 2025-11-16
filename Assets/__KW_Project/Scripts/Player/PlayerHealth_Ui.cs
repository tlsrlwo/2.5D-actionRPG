using TMPro;
using UnityEngine;
using UnityEngine.UI; 

namespace KW
{
    public class PlayerHealth_UI : MonoBehaviour
    {
        [Header("참조")]
        // [힌트] 1. Player 오브젝트에 붙어있는 'PlayerHealth' 스크립트       
        [SerializeField] private PlayerHealth playerHealth;

        // [힌트] 2. 채웠다 비웠다 할 '붉은색 HP 바' Image 컴포넌트
        [SerializeField] private Image healthBarImage;

        // [힌트] 3. (선택 사항) "80/100" 텍스트
        [SerializeField] private TextMeshProUGUI healthText;

        // --- 이벤트 구독 ---
        private void OnEnable()
        {
            if (playerHealth == null)
            {
                // 플레이어에서 PlayerHealth 를 자동으로 할당하게 해줌 (씬이 변경되도 찾을 수 있게끔)
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerHealth = player.GetComponent<PlayerHealth>();
                }
            }

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged += UpdateHealthBar;

                UpdateHealthBar(playerHealth.currentHp, playerHealth.maxHp);
            }
            else
            {
                Debug.LogError("PlayerHealth_UI: 씬에서 'Player' 태그를 가진 PlayerHealth를 찾을 수 없습니다!");
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged -= UpdateHealthBar;
            }
        }

        private void UpdateHealthBar(float currentHp, float maxHp)
        {
            // 플레이어 현재체력/최대체력의 비율
            float ratio = 0;
            
            if(maxHp > 0)
            {
                ratio = currentHp / maxHp;
            }
                     
            // 체력 바의 채움 정도를 설정
            if (healthBarImage != null)
            {                
                healthBarImage.fillAmount = ratio;
            }

            // 4. (선택 사항) 텍스트 업데이트
             if (healthText != null)
             {
                // ceilToInt 는 반올림해서 정수로 만들어줌
                healthText.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
             }
        }
    }
}