using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    #region Move and Jump Info
    [Header("Move and Jump Info")]
    public float jumpForce;
    public float moveSpeed;
    public int jumpTimes;
    public FaceDirection faceDirection;
    #endregion

    #region Dash Info
    [Header("Dash Info")]
    //³å´Ì¼ÆÊ±Æ÷
    public float dashTimer;
    //³å´Ì³ÖÐøÊ±¼ä
    public float dashDuration;
    //³å´ÌCD¼ÆÊ±Æ÷
    public float dashCDTimer;
    //³å´ÌÀäÈ´Ê±¼ä
    public float dashCoolDown;
    //³å´ÌËÙ¶È
    public float dashSpeed;
    #endregion

    #region Wallslide Info
    [Header("Wallslide Info")]
    public float WallSlideFallSpeed;
    #endregion

    #region Componets
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Collider2D collider2D { get; private set; }
    #endregion

    #region CollidingCheck
    [Header("CollidingCheck")]
    //¼ì²âÍ¼²ã
    [SerializeField] protected LayerMask layerMask_Ground;

    //µØÃæÅö×²¼ì²â
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    //Ç½ÃæÅö×²ÉäÏß¼ì²â
    [SerializeField] public Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    #endregion

    #region Attack Info
    [Header("Attack Info")]
    public int attackComboo;
    #endregion

    #region PlayerStates
    public PlayerState currentState;
    public PlayerStateMachine stateMachine { get; private set; }
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }
    public RiseState riseState { get; private set; }
    public FallState fallState { get; private set; }
    public DashState dashState { get; private set; }
    public WallSlideDownState wallSlideDownState { get; private set; }
    public Attack_01_State attack_01_State { get; private set; }
    public Attack_02_State attack_02_State { get; private set; }
    public Attack_03_State attack_03_State { get; private set; }

    #endregion


    public enum FaceDirection { 
        Left = -1,
        Right = 1, 
    }
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new IdleState(this,stateMachine,"Idle");
        moveState = new MoveState(this, stateMachine, "Move");
        riseState = new RiseState(this, stateMachine, "IsAir");
        fallState = new FallState(this, stateMachine, "IsAir");
        dashState = new DashState(this, stateMachine, "IsDash");
        attack_01_State = new Attack_01_State(this, stateMachine, "IsAttack");
        attack_02_State = new Attack_02_State(this, stateMachine, "IsAttack");
        attack_03_State = new Attack_03_State(this, stateMachine, "IsAttack");
        wallSlideDownState = new WallSlideDownState(this, stateMachine, "IsWallSlide");
    }
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        stateMachine.Initialize(idleState);
        groundCheck = transform.Find("GroundCheck");
        wallCheck = transform.Find("WallCheck");
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        currentState = stateMachine.GetCurrentState();
    }
    public void SetVelocity(float xVelocity,float yVelocity) { 
        rb.velocity = new Vector2 (xVelocity,yVelocity);
        FlipController(xVelocity);
    }

    public void Filp()
    {
        faceDirection = (faceDirection == FaceDirection.Left) ? FaceDirection.Right:FaceDirection.Left;
        transform.Rotate(0, 180, 0);
        wallCheckDistance = -wallCheckDistance;
    }

    //·­×ª¿ØÖÆÆ÷
    public void FlipController(float xVelocity)
    {
        if (xVelocity > 0 && faceDirection != FaceDirection.Right)
        {
            Filp();
        }
        else if (xVelocity < 0 && faceDirection != FaceDirection.Left)
        {
            Filp();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layerMask_Ground);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, layerMask_Ground);
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}

