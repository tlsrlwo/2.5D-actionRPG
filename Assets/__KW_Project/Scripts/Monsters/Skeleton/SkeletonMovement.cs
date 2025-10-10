using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonMovement : MonoBehaviour
    {
        private MonsterBaseState<SkeletonMovement> currentState;

        #region 스켈레톤 fsm

        public readonly SkeletonIdleState idleState = new SkeletonIdleState();
        public readonly SkeletonPatrolState patrolState = new SkeletonPatrolState();
        public readonly SkeletonAttackState attackState = new SkeletonAttackState();
        public readonly SkeletonChaseState chaseState = new SkeletonChaseState();


        #endregion

        #region 컴포넌트
        [Header("이동")]
        public float patrolSpeed = 1;
        public float chaseSpeed = 3;

        [Header("체력")]
        public float currentHp =0;
        public float maxHp = 80;

        [Header("추격,공격 범위")]
        public float detectRange = 5f;
        public float attackRange = 1f;
        public float idleWaitTime = 3f;
        public float suspiciousTime = 3f;                           // 두리번거리는 시간
        public float timeSinceLastSawPlayer;

        public LayerMask playerLayer;
        public Transform wayPoints;                                 // 목적지
        public int       wayPointIndex;                             // 목적지 인덱스 ( 증가시킬거임 ) 

        public Transform target;

        [Header("공격")]
        public float damage = 25;

        [Header("컴포넌트")]
        [HideInInspector] public Animator anim;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public SpriteRenderer sr;
        #endregion

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            rb = GetComponentInChildren<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            currentHp = maxHp;
            agent.updateRotation = false;
            SwitchState(idleState);
        }

        private void Update()
        {
            currentState.UpdateState(this);
            
        }

        public void SwitchState(MonsterBaseState<SkeletonMovement> monsterState)
        {
            currentState = monsterState;
            currentState.EnterState(this);            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }

    }
}
