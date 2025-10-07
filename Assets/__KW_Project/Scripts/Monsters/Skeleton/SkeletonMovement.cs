using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonMovement : MonoBehaviour
    {
        private MonsterBaseState<SkeletonMovement> currentState;

        #region ½ºÄÌ·¹Åæ fsm

        public readonly SkeletonIdleState idleState = new SkeletonIdleState();
        public readonly SkeletonPatrolState patrolState = new SkeletonPatrolState();
        public readonly SkeletonAttackState attackState = new SkeletonAttackState();


        #endregion

        [Header("ÀÌµ¿")]
        private float patrolSpeed = 3;
        private float chaseSpeed = 3;

        [Header("Ã¼·Â")]
        private float currentHp =0;
        private float maxHp = 80;

        [Header("°ø°Ý")]
        private float damage = 25;

        [Header("ÄÄÆ÷³ÍÆ®")]
        private Animator anim;
        private NavMeshAgent nav;
        private Rigidbody rb;

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            rb = GetComponentInChildren<Rigidbody>();
            nav = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            currentHp = maxHp;

            SwitchState(idleState);
        }

        private void Update()
        {
            
        }

        private void SwitchState(MonsterBaseState<SkeletonMovement> monsterState)
        {
            currentState = monsterState;
            currentState.EnterState(this);
            currentState.UpdateState(this);
        }


    }
}
