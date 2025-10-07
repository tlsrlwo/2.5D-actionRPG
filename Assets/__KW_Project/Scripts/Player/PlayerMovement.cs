using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KW
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("움직임")]
        public float walkSpeed = 3f;                        // 걷는 속도                    
        public float runSpeed = 5f;                         // 달리는 속도
        public float airSpeed = 4f;                         // 공중에서의 속도 (필요없음)
        public float xInput, zInput;
        public float lastMoveX, lastMoveZ;

        [SerializeField] private bool canMove = true;       // 플레이어 움직임 허용
        public float currentSpeed;                          // 현재 속도
        public Vector3 dir;


        [Header("점프")]
        [SerializeField] private float groundYOffset;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float sphereRadius = 0.05f;

        public virtual bool isGrounded(bool value) => IsGrounded();
        Vector3 spherePos;                                  // 플레이어 지면 확인용 구체

        [Header("중력")]
        [SerializeField] private float gravity = -9.81f;    // 중력값
        private Vector3 velocity;


        [Header("컴포넌트 참조")]
        [HideInInspector] public Animator anim;
        [HideInInspector] public SpriteRenderer sr;
        private CharacterController cController;


        [Header("전투")]
        private float maxHp = 100;                          // 최대 체력
        private float baseDamage = 10;                      // 기본 데미지
        private float weaponDamage = 0;                     // 무기 데미지
        private float defencePercentage = 0;                // 방어율

        public float TotalDamage { get { return baseDamage + weaponDamage; } }


        #region 플레이어 FSM
        public MovementBaseState previousState;
        public MovementBaseState currentState;

        public PlayerIdleState playerIdle = new PlayerIdleState();
        public PlayerWalkState playerWalk = new PlayerWalkState();
        public PlayerRunState playerRun = new PlayerRunState();
        public PlayerAttackState playerAttack = new PlayerAttackState();

        #endregion

        #region Ui 에 따른 상태
        void OnEnable()
        {
            UiManager.OnAnyUiStateChanged += HandleUiStateChanged;
        }

        void OnDisable()
        {
            UiManager.OnAnyUiStateChanged -= HandleUiStateChanged;
        }

        private void HandleUiStateChanged(bool isAnyUiOpen)
        {
            canMove = !isAnyUiOpen;
        }
        #endregion

        private void LoadStatsFromJson()
        {
            // json 파일을 text 로서 읽어옴
            TextAsset playerStatFile = Resources.Load<TextAsset>("playerStats");

            if(playerStatFile != null)
            {
                // 읽어온 json의 내용을 PlayerStats 에 적용시키고, 현재 스크립트에도 적용
                PlayerStats stats = JsonUtility.FromJson<PlayerStats>(playerStatFile.text);

                this.walkSpeed = stats.walkSpeed;
                this.runSpeed = stats.runSpeed;
                this.baseDamage = stats.baseDamage;
                this.maxHp = stats.maxHp;

                Debug.Log("플레이어 기본 스탯 로드 완료 : maxHP(" + maxHp + ")" + " , baseDamage(" + baseDamage + ")");
            }
            else
            {
                Debug.Log("플레이어 데이터를 담은 json 파일을 찾을 수 없습니다");
            }
        }


        private void Awake()
        {
            LoadStatsFromJson();

            sr = GetComponent<SpriteRenderer>();
            cController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            // 씬 시작 시 상태(state) 설정
            SwitchState(playerIdle);
        }

        private void Update()
        {
            if (!canMove)
            {
                return;
            }

            PlayerMove();
            Gravity();

            // 스프라이트가 오른쪽만 있어서 반대로 뒤집어줌
            /*
            if (xInput < 0)
            {
                sr.flipX = true;
            }
            if (xInput > 0)
            {
                sr.flipX = false;
            }
            */
            if (xInput != 0)
            {
                sr.flipX = (xInput < 0);
            }

            anim.SetFloat("lastMoveX", lastMoveX);
            anim.SetFloat("lastMoveZ", lastMoveZ);

            currentState.UpdateState(this);
        }

        private void PlayerMove()
        {
            // GetAxisRaw 로 입력값 1,0 으로 고정
            xInput = Input.GetAxisRaw("Horizontal");
            zInput = Input.GetAxisRaw("Vertical");
            //xInput = Input.GetAxis("Horizontal");
            //zInput = Input.GetAxis("Vertical");

            //Vector3 airDir = Vector3.zero;            

            dir = transform.forward * zInput + transform.right * xInput;            // vector3 dir

            // 애니메이터에서 0 값을 받지 않도록 (달리다가 IDLE 로 전환될 때 오류 수정)
            if (dir.magnitude > 0.01f)
            {
                lastMoveX = xInput;
                lastMoveZ = zInput;

                anim.SetFloat("xInput", xInput);
                anim.SetFloat("zInput", zInput);

            }
            // 변수 초기화
            Vector3 finalVelocity = dir.normalized * currentSpeed;

            cController.Move(finalVelocity * Time.deltaTime);
        }

        public void SwitchState(MovementBaseState state)
        {
            currentState = state;
            currentState.EnterState(this);
            currentState.UpdateState(this);
        }
        private bool IsGrounded()
        {
            // 플레이어의 바닥 판정
            spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
            if (Physics.CheckSphere(spherePos, cController.radius - sphereRadius, groundLayer))
            {
                return true;
            }

            return false;
        }


        private void Gravity()
        {
            // 점프 했을 때 gravity 만큼 더 빨르게 낙하
            if (!IsGrounded())
            {
                velocity.y += gravity * Time.deltaTime;
            }
            // 오류 방지 & 바닥에 붙여놓기
            else if (velocity.y < 0)
            {
                velocity.y = -2;
            }
            cController.Move(velocity * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spherePos, cController.radius - sphereRadius);
        }
    }
}