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
    //角色的移动速度和跳跃力
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
   
    //是否在移动
    [SerializeField] private bool isMoving;

    //判断玩家跳跃次数来实现二段跳
    [SerializeField] private int jumpTimes;

    //用于存储移动跳跃时获得的瞬时速度
    private float velocityMove;
    private float velocityDash;

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

    [Header("Physical Component")]
    //获取组件
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;
    private Animator animator;

    [Header("Collider Check")]
    //图层遮罩用于检测是否接触地面，将需要检测的物体设置到Ground图层
    [SerializeField] private LayerMask layerMask_Ground;
    //检测射线最大距离
    [SerializeField] private float GroundCheckDistance;
    //判断玩家是否在地面
    [SerializeField] private bool isGround;
    

    //角色朝向
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

    //动作相关的计时器
    private void ActionTimer()
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

        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
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
    private void Movement()
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
        else {
            rb.velocity = new Vector2(velocityMove, rb.velocity.y);
        }
        
    }

    //动画控制器
    private void AnimatorControllers() {
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isGround", isGround);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetBool("isDash", isDash);
    }

    //修正跳跃次数
    private void FixedJumpTimes() {
        if (isGround) {
            jumpTimes = 0;
        }
    }



    //跳跃
    private void Jump() {
        //二段跳实现
        if (isGround || jumpTimes<2) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimes++;
        }
        
    }
    
    //翻转人物
    private void Filp(){
        direction = (direction == FaceDirection.LEFT ? FaceDirection.RIGHT: FaceDirection.LEFT);
        transform.Rotate(0, 180, 0);
    }

    //翻转控制器
    private void FlipController() {
        if (rb.velocity.x > 0 && direction != FaceDirection.RIGHT) {
            Filp();
        } else if (rb.velocity.x < 0 && direction != FaceDirection.LEFT) {
            Filp();
        }
    }

    //碰撞检测
    private void ColliderController() {
        isGround = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, layerMask_Ground);
    }

    //画出检测射线
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x, transform.position.y - GroundCheckDistance));
    }
}
