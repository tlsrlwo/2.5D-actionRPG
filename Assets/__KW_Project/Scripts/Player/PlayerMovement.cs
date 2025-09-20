using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace KW
{    
    public class PlayerMovement : MonoBehaviour
    {
        [Header("������")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeeed = 5f;
        [SerializeField] private float airSpeed = 1.5f;
        [SerializeField] private float xInput, zInput;

        private Vector3 dir;

        CharacterController cController;
        SpriteRenderer sr;

        [Header("����")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float groundYOffset;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float sphereRadius = 0.05f;

        public virtual bool isGrounded(bool value) => IsGrounded();

        Vector3 spherePos;

        [Header("�߷�")]
        [SerializeField] private float gravity = -9.81f;
        private Vector3 velocity;


        [Header("Anim")]
        Animator anim;

        #region "�÷��̾� FSM"
        public MovementBaseState previousState;
        public MovementBaseState currentState;

        public PlayerIdleState playerIdle = new PlayerIdleState();
        public PlayerWalkState playerWalk = new PlayerWalkState();
        public PlayerRunState playerRun = new PlayerRunState();
        public PlayerAttackState playerAttack = new PlayerAttackState();


        #endregion

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            cController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            PlayerMove();
            Gravity();

            if (xInput > 0)
            {
                sr.flipX = false;
                anim.SetBool("isWalk", true);
            }
            else if (xInput < 0)
            {
                sr.flipX = true;
                anim.SetBool("isWalk", true);
            }

            if (xInput == 0 && zInput == 0)
            {
                anim.SetBool("isWalk", false);
            }
        }

        private void PlayerMove()
        {
            xInput = Input.GetAxis("Horizontal");
            zInput = Input.GetAxis("Vertical");
            Vector3 airDir = Vector3.zero;

            // ���߿� ���� �� 
            if (!IsGrounded()) airDir = transform.forward * zInput + transform.right * xInput;
            // �ٴڿ� ���� �� 
            dir = transform.forward * zInput + transform.right * xInput;

            cController.Move((dir.normalized * walkSpeed + airDir * airSpeed) * Time.deltaTime);
        }

        private bool IsGrounded()
        {
            // �÷��̾��� �ٴ� ����
            spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
            if (Physics.CheckSphere(spherePos, cController.radius - sphereRadius, groundLayer))
            {
                return true;
            }

            return false;
        }


        private void Gravity()
        {
            // ���� ���� �� gravity ��ŭ �� ������ ����
            if (!IsGrounded())
            {
                velocity.y += gravity * Time.deltaTime;
            }
            // ���� ���� & �ٴڿ� �ٿ�����
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