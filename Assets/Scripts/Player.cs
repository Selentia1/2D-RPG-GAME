using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using UnityEngine;
using UnityEngine.Accessibility;

public class Player : Entity
{
    //�ж������Ծ������ʵ�ֶ�����
    private int jumpTimes;

    //���ڴ洢�ƶ���Ծʱ��õ�˲ʱ�ٶ�
    private float velocityMove;
    private float velocityDash;
    private float velocityAattck;

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

    [Header("Attack Info")]
    //����״̬
    [SerializeField] private bool isAttack;
    //��������
    [SerializeField] private int attackComboo;
    //���������ü�ʱ��
    [SerializeField] private float combooResetTimer;
    //��������������
    [SerializeField] private float combooResetDuration;
    //�������
    [SerializeField] private float acttckDuration;
    //���������ʱ��
    [SerializeField] private float attackTimer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        direction = FaceDirection.RIGHT;
        
        isGround = true;
        rb.velocity = Vector2.zero;
        moveSpeed = 5;
        jumpForce = 20;
        jumpTimes = 0;
        dashSpeed = 15;
        dashDuration = 0.3f;
        dashCoolDown = 1;
        combooResetDuration = 0.6f;
        acttckDuration = 0.4f;
        attackComboo = -1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInPut();
        Movement();
        ActionTimers();

        AnimatorControllers();
        base.Update();
        FlipController();
    }

    //������صļ�ʱ��
    private void ActionTimers()
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

        //���CD��ʱ��
        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }

        //������ʱ��
        if (combooResetTimer > 0) {
            combooResetTimer -= Time.deltaTime;
        }

        //�������������ʱ��
        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
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

        if (Input.GetKeyDown(KeyCode.X)) {
            Attack();
        }
    }

    //������ͨ����
    protected override void Attack()
    {
        //������������ļ���ʱ�䣬������Comboo
        if (combooResetTimer < 0) {
            attackComboo = -1;
        }

        //���û���ڻ���״̬��û���ڹ�������ڣ���ִ�й�������
        if (!isDash && attackTimer <= 0) {
            isAttack = true;
            
            combooResetTimer = combooResetDuration;
            attackTimer = acttckDuration;

            //���attackComboo>3��ָ�Ϊ0
            attackComboo++;
            attackComboo = attackComboo % 3;
        }
    }

    //�������������ص��������һ֡
    public void AttackOver() {
        isAttack = false;
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
    protected override void Movement()
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
        else if (isAttack && isMoving && !isGround )
        {
            rb.velocity = new Vector2(velocityMove * 0.3f, rb.velocity.y);
        }
        else if (isAttack && isMoving)
        { 
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if(!isAttack)
        {
            rb.velocity = new Vector2(velocityMove, rb.velocity.y);
        }
        
    }

    //����������
    protected override void AnimatorControllers() {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGround", isGround);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isDash", isDash);
        animator.SetBool("isAttack", isAttack);
        animator.SetInteger("attackComboo", attackComboo);
    }

    //������Ծ����
    private void FixedJumpTimes() {
        if (isGround) {
            jumpTimes = 0;
        }
    }

    //��Ծ
    protected override void Jump() {
        //������ʵ��
        if (isGround || jumpTimes<2) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimes++;
        }
        
    }

    //��ת������
    protected override void FlipController()
    {
        if (rb.velocity.x > 0 && direction != FaceDirection.RIGHT)
        {
            Filp();
        }
        else if (rb.velocity.x < 0 && direction != FaceDirection.LEFT)
        {
            Filp();
        }
    }
}
