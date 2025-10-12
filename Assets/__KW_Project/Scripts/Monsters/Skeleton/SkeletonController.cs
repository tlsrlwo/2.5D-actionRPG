using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonController : MonoBehaviour
    {
        private MonsterBaseState<SkeletonController> currentState;

        #region 스켈레톤 fsm

        public readonly SkeletonIdleState idleState = new SkeletonIdleState();
        public readonly SkeletonPatrolState patrolState = new SkeletonPatrolState();
        public readonly SkeletonAttackState attackState = new SkeletonAttackState();
        public readonly SkeletonChaseState chaseState = new SkeletonChaseState();
        public readonly SkeletonCoolDownState coolDownState = new SkeletonCoolDownState();

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
        public float minSuspiciousTime = 2f;                        // 두리번거리는 시간
        public float maxSuspiciousTime = 6f;
        public float suspiciousTime = 3f;
        public float timeSinceLastSawPlayer;

        public LayerMask playerLayer;
        public Transform wayPoints;                                 // 목적지
        public int       wayPointIndex;                             // 목적지 인덱스 ( 증가시킬거임 ) 

        public Transform target;

        [Header("공격")]
        public float damage = 25;
        public float lungeForce = 2f;                              // 공격 후 반동
        public float lungeDuration = 0.2f;
        public bool isDoingLunge = false;
        public float coolDownDuration = 1f;
        [HideInInspector] public Vector3 lastAttackDirection;
        public Vector2 lastDirection;                               // coolDownState 에서 방향을 기억하기 위한 변수

        [Header("컴포넌트")]
        [HideInInspector] public Animator anim;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public SpriteRenderer sr;
        #endregion

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            sr = GetComponentInChildren<SpriteRenderer>();

            // 스클레톤마다 랜덤한 시간 부여
            suspiciousTime = Random.Range(minSuspiciousTime, maxSuspiciousTime);            
        }

        private void Start()
        {
            currentHp = maxHp;
            agent.updateRotation = false;
            rb.isKinematic = true;
            SwitchState(idleState);
        }

        private void Update()
        {
            currentState.UpdateState(this);            
        }

        public void SwitchState(MonsterBaseState<SkeletonController> monsterState)
        {
            currentState?.ExitState(this);

            currentState = monsterState;
            currentState.EnterState(this);            
        }

        public void PerformAttackLunge()
        {
            StopAllCoroutines();
            StartCoroutine(LungeRoutine());
        }

        private IEnumerator LungeRoutine()
        {
            isDoingLunge = true;

            // navAgent 잠시 비활성화
            agent.enabled = false;

            // rigidbody의 kinematic 잠시 비활성화
            rb.isKinematic = false;

            // AttackState 에서 지정한 방향으로 공격 시 조금 이동(반동효과)
            rb.AddForce(lastAttackDirection * lungeForce, ForceMode.Impulse);

            yield return new WaitForSeconds(lungeDuration);

            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            agent.enabled = true;

            isDoingLunge = false;
        }

        public void StartCoolDown()
        {
            SwitchState(coolDownState);

            StartCoroutine(CoolDownRoutine());
        }

        private IEnumerator CoolDownRoutine()
        {
            Debug.Log("스켈레톤 : 쿨다운 시작");

            anim.SetBool("isCoolDown", true);

            yield return new WaitForSeconds(coolDownDuration);

            Debug.Log("스켈레톤 : 쿨다운 종료!");

            anim.SetBool("isCoolDown", false);

            SwitchState(chaseState);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
