using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class SkeletonHealth : MonsterHealth
    {
       /* [Header("체력")]
        [SerializeField] private float _currentHp;
        [SerializeField] private float _maxHp;
        [HideInInspector] public virtual float currentHp => _currentHp;
        [HideInInspector] public virtual float maxHp => _maxHp;

        // SkeletonController 와 SkeletonDeathState에서 사용
        public event Action OnSkeletonHit;
        public event Action OnSkeletonDead;

        [HideInInspector] public SkeletonController skeletoncontroller;

        public void InitializeHealth(float parsedMaxHp)
        {
            _maxHp = parsedMaxHp;
            _currentHp = _maxHp;
        }

        public void TakeDamage(float damage)
        {
            if (_currentHp <= 0) return;

            _currentHp -= damage;
            _currentHp = Mathf.Clamp(_currentHp, 0, _maxHp);

            if (_currentHp <= 0)
            {
                OnSkeletonDead?.Invoke();
            }
            else
            {
                OnSkeletonHit?.Invoke();
            }
        }*/
    }
}
