using UnityEngine;
using System;

namespace KW
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("체력 설정")]
        [SerializeField] private float _maxHp;       
        [SerializeField] private float _currentHp;

        public virtual float currentHp => _currentHp; 
        public virtual float maxHp => _maxHp;

        public event Action<float, float> OnHealthChanged;

        // 나중에 PlayerMovement가 사용
        public event Action OnPlayerDied; // 사망 이벤트
        public event Action OnPlayerHit;  // 피격 이벤트

        private void Start()
        {
            _currentHp = _maxHp;

            OnHealthChanged?.Invoke(_currentHp, _maxHp);
        }


        private void Update()
        {
            // 테스트
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TakeDamage(10f);
            }
        }

        public void InitializeHealth(float parsedMaxHp)
        {
            _maxHp = parsedMaxHp;
            _currentHp = _maxHp;

            OnHealthChanged?.Invoke(_currentHp, parsedMaxHp);
        }

        // 몬스터가 타격 시 호출
        public void TakeDamage(float damage)
        {
            if (_currentHp <= 0) return;

            // 2. 체력 깎기
            _currentHp -= damage;
            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

            // 3. UI에 변경된 체력 '방송' (가장 중요!)
            OnHealthChanged?.Invoke(_currentHp, _maxHp);

            // 4. (나중에 구현할 것) 체력이 0 이하가 되면 '사망' 이벤트 방송
            if (_currentHp <= 0)
            {
                OnPlayerDied?.Invoke();
            }
            else
            {
                OnPlayerHit?.Invoke();
            }
        }

        // --- (선택 사항) 힐링 포션용 함수 ---
        public void Heal(float amount)
        {
            // 1. 체력 회복 (maxHp를 넘지 않게)
            _currentHp = Mathf.Clamp(_currentHp + amount, 0, _maxHp);

            // 2. UI에 체력 '방송' (OnHealthChanged?.Invoke)
            OnHealthChanged?.Invoke(_currentHp, _maxHp);
        }
    }
}