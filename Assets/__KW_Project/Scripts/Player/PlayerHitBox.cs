using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerHitBox : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        private float _attackDamage;

        private HashSet<Collider> alreadyHitTargets = new HashSet<Collider>();

        private void Awake()
        {
            playerHealth = transform.root.GetComponent<PlayerHealth>();

            if (playerHealth == null)
            {
                Debug.LogError("PlayerHitBox : PlyaerHealth.cs 를 찾을 수 없음");
            }

            alreadyHitTargets.Clear();
        }

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                _attackDamage = playerHealth.TotalDamage;
            }
            else
            {
                _attackDamage = 10f;
                Debug.LogError("PlayerHitBox : PlayerHealth를 찾지 못해 공격 수치 10 으로 고정");
            }

            alreadyHitTargets.Clear();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 부딪힌 대상이 적인지 확인 (태그 확인)
            if (other.CompareTag("Monster"))
            {
                if (alreadyHitTargets.Contains(other))
                {
                    return;
                }

                // 적에게서 'MonsterHealth' 컴포넌트 가져오기
                MonsterHealth monsterHealth = other.gameObject.GetComponent<MonsterHealth>();

                // 적의 체력 스크립트가 존재한다면
                if (monsterHealth != null)
                {
                    // 적에게 데미지를 주고, 공격자(나)의 위치를 전달
                    monsterHealth.TakeDamage(_attackDamage, transform.root);
                    Debug.Log("플레이어가 공격함");

                    alreadyHitTargets.Add(other);

                    // 공격했을 때 효과 등 로직 

                }
            }
        }
    }
}
