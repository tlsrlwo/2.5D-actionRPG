using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class SkeletonHitBox : MonoBehaviour
    {
        private float _damage;
        public MonsterHealth health;
        public SkeletonController skeleton;

        private HashSet<Collider> alreadyHitTargets = new HashSet<Collider>();

        private void Awake()
        {
            health = GetComponentInParent<MonsterHealth>();
            skeleton = GetComponentInParent<SkeletonController>();

            alreadyHitTargets.Clear();
        }

        private void OnEnable()
        {
            if (skeleton != null)
            {
                _damage = skeleton.damage;
            }

            alreadyHitTargets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 플레이어인지 확인 
            if (other.CompareTag("Player"))
            {
                if (alreadyHitTargets.Contains(other))
                {
                    return;
                }

                // 플레이어의 체력 스크립트 찾기
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                // 플레이어에게 데미지 주기
                if (playerHealth != null)
                {
                    Debug.Log($"SkeletonHitBox : 플레이어에게 {_damage} 피해");
                    playerHealth.TakeDamage(_damage);

                    alreadyHitTargets.Add(other);
                }
            }
        }
    }
}
