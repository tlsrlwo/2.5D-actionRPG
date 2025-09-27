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
        public float walkSpeed = 3f;
        public float runSpeeed = 5f;
        public float airSpeed = 4f;
        public float xInput, zInput;
        public float lastMoveX, lastMoveZ;

        [SerializeField]private bool canMove = true;

        public float currentSpeed;

        public Vector3 dir;


        [Header("점프")]
        [SerializeField] private float groundYOffset;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float sphereRadius = 0.05f;

        public virtual bool isGrounded(bool value) => IsGrounded();

        Vector3 spherePos;

        [Header("중력")]
        [SerializeField] private float gravity = -9.81f;
        private Vector3 velocity;


        [Header("컴포넌트 참조")]
        [HideInInspector] public Animator anim;
        [HideInInspector] public SpriteRenderer sr;
        private CharacterController cController;

        #region "플레이어 FSM"
        public MovementBaseState previousState;
        public MovementBaseState currentState;

        public PlayerIdleState playerIdle = new PlayerIdleState();
        public PlayerWalkState playerWalk = new PlayerWalkState();
        public PlayerRunState playerRun = new PlayerRunState();
        public PlayerAttackState playerAttack = new PlayerAttackState();

        #endregion       
               
        void OnEnable()
        {
            UiManager.OnMapStateChanged += HandleMapStateChanged;
            UiManager.OnInventoryStateChanged += HandleInventoryStateChanged;
            UiManager.OnSettingsStateChanged += HandleSettingsStateChanged;
            UiManager.OnQuestStateChanged += HandleQuestStateChanged;
        }

        void OnDisable()
        {
            UiManager.OnMapStateChanged -= HandleMapStateChanged;
            UiManager.OnInventoryStateChanged -= HandleInventoryStateChanged;
            UiManager.OnSettingsStateChanged -= HandleSettingsStateChanged;
            UiManager.OnQuestStateChanged -= HandleQuestStateChanged;
        }

        private void HandleMapStateChanged(bool isMapOpen)
        {
            canMove = !isMapOpen;
        }
        private void HandleInventoryStateChanged(bool isInventoryOpen)
        {
            canMove = !isInventoryOpen;
        }
        private void HandleSettingsStateChanged(bool isSettingsOpen)
        {
            canMove = !isSettingsOpen;
        }
        private void HandleQuestStateChanged(bool isQuestOpen)
        {
            canMove = !isQuestOpen;
        }



        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            cController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            // 씬 시작 시 상태 설정
            SwitchState(playerIdle);
        }

        private void Update()
        {
            if(!canMove)
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
            if(dir.magnitude > 0.01f)
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