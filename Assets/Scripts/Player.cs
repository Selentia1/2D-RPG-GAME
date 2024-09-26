using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using UnityEngine;
using UnityEngine.Accessibility;

public class Player : MonoBehaviour
{
    [Header("Move And Jump")]
    //��ɫ���ƶ��ٶȺ���Ծ��
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
   
    //�Ƿ����ƶ�
    [SerializeField] private bool isMoving;

    //�ж������Ծ������ʵ�ֶ�����
    [SerializeField] private int jumpTimes;

    //���ڴ洢�ƶ���Ծʱ��õ�˲ʱ�ٶ�
    private float velocityMove;
    private float velocityDash;

    [Header("Dash Info")]
    //��̼�ʱ��
    [SerializeField] private float dashTimer;
    //��̳���ʱ��
    [SerializeField] private float dashDuration;
    //���CD��ʱ��
    [SerializeField] private float dashCDTimer;
    //�����ȴʱ��
    [SerializeField] private float dashCoolDown;
    //����ٶ�
    [SerializeField] private float dashSpeed;
    //���״̬
    [SerializeField] private bool isDash;

    [Header("Physical Component")]
    //��ȡ���
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;
    private Animator animator;

    [Header("Collider Check")]
    //ͼ���������ڼ���Ƿ�Ӵ����棬����Ҫ�����������õ�Groundͼ��
    [SerializeField] private LayerMask layerMask_Ground;
    //�������������
    [SerializeField] private float GroundCheckDistance;
    //�ж�����Ƿ��ڵ���
    [SerializeField] private bool isGround;
    

    //��ɫ����
    private enum FaceDirection {
        RIGHT = -1,
        LEFT = 1
    }
    private FaceDirection direction;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        
        rb.velocity = Vector2.zero;
        moveSpeed = 5;
        jumpForce = 15;
        direction = FaceDirection.RIGHT;
        GroundCheckDistance = 1.02f;
        isGround = true;
        jumpTimes = 0;
        dashSpeed = 15;
        dashDuration = 0.3f;
        dashCoolDown = 1;                                    
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        GetInPut();
        ActionTimer();

        ColliderController();
        AnimatorControllers();
        FlipController();

    }

    //������صļ�ʱ��
    private void ActionTimer()
    {
        //��̼�ʱ��
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else if (dashTimer < 0)
        {
            isDash = false;
        }

        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }
    }

    //�������
    private void GetInPut()
    {
        velocityMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        
        //���¿ո�������Ծ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FixedJumpTimes();
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Dash();
        }
    }

    //������
    private void Dash()
    {
        if (dashCDTimer <= 0) 
        {
            isDash = true;
            dashTimer = dashDuration;
            dashCDTimer = dashCoolDown;

            if (direction == FaceDirection.LEFT)
            {
                velocityDash = -dashSpeed;
            }
            else if (direction == FaceDirection.RIGHT)
            {
                velocityDash = dashSpeed;
            }
        }
    }

    //�����ƶ�
    private void Movement()
    {
        //�����ƶ�״̬����
        //isMoving = (rb.velocity.x != 0);
        if (rb.velocity.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //������ʱ��ö�����ٶ�
        if (dashTimer > 0)
        {
            rb.velocity = new Vector2(velocityDash, 0);
        }
        else {
            rb.velocity = new Vector2(velocityMove, rb.velocity.y);
        }
        
    }

    //����������
    private void AnimatorControllers() {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGround", isGround);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isDash", isDash);
    }

    //������Ծ����
    private void FixedJumpTimes() {
        if (isGround) {
            jumpTimes = 0;
        }
    }



    //��Ծ
    private void Jump() {
        //������ʵ��
        if (isGround || jumpTimes<2) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimes++;
        }
        
    }
    
    //��ת����
    private void Filp(){
        direction = (direction == FaceDirection.LEFT ? FaceDirection.RIGHT: FaceDirection.LEFT);
        transform.Rotate(0, 180, 0);
    }

    //��ת������
    private void FlipController() {
        if (rb.velocity.x > 0 && direction != FaceDirection.RIGHT) {
            Filp();
        } else if (rb.velocity.x < 0 && direction != FaceDirection.LEFT) {
            Filp();
        }
    }

    //��ײ���
    private void ColliderController() {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, layerMask_Ground);
    }

    //�����������
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x, transform.position.y - GroundCheckDistance));
    }
}
