using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class MonsterHealth : MonoBehaviour
    {
        [TextArea(2, 3)]
        [SerializeField] private string scriptPurpose;

        protected float _currentHp;
        protected float _maxHp;

        // 읽기 전용
        public float currentHp => _currentHp;
        public float maxHp => _maxHp;

        public event Action<Vector3> OnHit;                         // 피격 시 플레이어 위치를 전달받기 위함
        public event Action OnDeath;

        public virtual void InitializeHealth(float parsedMaxHp)
        {
            _maxHp = parsedMaxHp;
            _currentHp = _maxHp;
        }

        public virtual void TakeDamage(float damage, Transform attacker)
        {
            if (_currentHp <= 0) return;

            // 데미지 만큼 체력 감소
            _currentHp -= damage;
            Debug.Log($"몬스터 체력 감소 : {damage} 만큼 감소됨");

            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

            if (_currentHp <= 0)
            {
                OnDeath?.Invoke();
            }
            else
            {
                Vector3 onHitDirection = (transform.position - attacker.position).normalized;
                
                // 방향을 실어서 방송
                OnHit?.Invoke(onHitDirection);
            }
        }
    }
}
