using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using UnityEngine;
using UnityEngine.Accessibility;

public class Player : Entity
{
    //判断玩家跳跃次数来实现二段跳
    private int jumpTimes;

    //用于存储移动跳跃时获得的瞬时速度
    private float velocityMove;
    private float velocityDash;
    private float velocityAattck;

    [Header("Dash Info")]
    //冲刺计时器
    [SerializeField] private float dashTimer;
    //冲刺持续时间
    [SerializeField] private float dashDuration;
    //冲刺CD计时器
    [SerializeField] private float dashCDTimer;
    //冲刺冷却时间
    [SerializeField] private float dashCoolDown;
    //冲刺速度
    [SerializeField] private float dashSpeed;
    //冲刺状态
    [SerializeField] private bool isDash;

    [Header("Attack Info")]
    //攻击状态
    [SerializeField] private bool isAttack;
    //连击段数
    [SerializeField] private int attackComboo;
    //连击数重置计时器
    [SerializeField] private float combooResetTimer;
    //连击数重置周期
    [SerializeField] private float combooResetDuration;
    //攻击间隔
    [SerializeField] private float acttckDuration;
    //攻击间隔计时器
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

    //动作相关的计时器
    private void ActionTimers()
    {
        //冲刺计时器
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else if (dashTimer < 0)
        {
            isDash = false;
        }

        //冲刺CD计时器
        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }

        //攻击计时器
        if (combooResetTimer > 0) {
            combooResetTimer -= Time.deltaTime;
        }

        //连续攻击间隔计时器
        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        }
    }

    //获得输入
    private void GetInPut()
    {
        velocityMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        
        //按下空格人物跳跃
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

    //人物普通攻击
    protected override void Attack()
    {
        //如果超出连击的计数时间，则重置Comboo
        if (combooResetTimer < 0) {
            attackComboo = -1;
        }

        //如果没有在滑行状态且没有在攻击间隔内，则执行攻击动作
        if (!isDash && attackTimer <= 0) {
            isAttack = true;
            
            combooResetTimer = combooResetDuration;
            attackTimer = acttckDuration;

            //如果attackComboo>3则恢复为0
            attackComboo++;
            attackComboo = attackComboo % 3;
        }
    }

    //攻击结束，挂载到动画最后一帧
    public void AttackOver() {
        isAttack = false;
    }

    //人物冲刺
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

    //人物移动
    protected override void Movement()
    {
        //人物移动状态控制
        //isMoving = (rb.velocity.x != 0);
        if (rb.velocity.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //人物冲刺时获得额外的速度
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

    //动画控制器
    protected override void AnimatorControllers() {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGround", isGround);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isDash", isDash);
        animator.SetBool("isAttack", isAttack);
        animator.SetInteger("attackComboo", attackComboo);
    }

    //修正跳跃次数
    private void FixedJumpTimes() {
        if (isGround) {
            jumpTimes = 0;
        }
    }

    //跳跃
    protected override void Jump() {
        //二段跳实现
        if (isGround || jumpTimes<2) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimes++;
        }
        
    }

    //翻转控制器
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
