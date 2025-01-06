using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static Player;

public class Enemy : Entity
{
    #region Move and Jump Info
    [Header("Move and Jump Info")]
    public float jumpForce;
    public float moveSpeed;
    public float deafultMoveSpeed;
    public int jumpTimes;
    public Direction.Dir faceDirection;
    #endregion

    #region Componets
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public new Collider2D collider2D { get; private set; }
    #endregion

    #region CollidingCheck
    [Header("CollidingCheck")]
    //…‰œﬂºÏ≤‚Õº≤„
    [SerializeField] protected LayerMask layerMask_Ground;
    [SerializeField] protected LayerMask layerMask_Player;

    //µÿ√Ê≈ˆ◊≤ºÏ≤‚
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    //«Ω√Ê≈ˆ◊≤…‰œﬂºÏ≤‚
    [SerializeField] public Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    //ÕÊº“ºÏ≤‚…‰œﬂ
    [SerializeField] public Transform playerCheck;
    [SerializeField] protected float playerCheckDistance;

    //π•ª˜ºÏ≤‚∑∂Œß
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;

    //º«¬ººÏ≤‚µΩµƒÕÊº“–≈œ¢
    public RaycastHit2D hitInfo;
    #endregion

    #region EnemyStates
    public EnemyState currentState;
    public EnemyStateMachine stateMachine { get; private set; }
    #endregion

    #region Effect
    public EntityFX fx;
    #endregion

    #region was Damaged
    [Header("was Damaged")]
    [SerializeField] protected Vector2 knockBackVelocity;
    [SerializeField] protected float KnockBackDuration;
    [SerializeField] private bool isknocked;
    #endregion

    #region Stuned Info
    [Header("Stuned Info")]
    [SerializeField] public float stunnedDuration;
    [SerializeField] public float stunnedTimer;
    [SerializeField] public bool canBeStunned;

    #endregion

    public virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }
    public virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        fx = GetComponent<EntityFX>();

        groundCheck = transform.Find("GroundCheck");
        wallCheck = transform.Find("WallCheck");
        playerCheck = transform.Find("PlayerCheck");
        attackCheck = transform.Find("AttackCheck");

        deafultMoveSpeed = moveSpeed;
    }

    public virtual void Update()
    {
        stateMachine.currentState.Update();
        currentState = stateMachine.GetCurrentState();
        FlipController();
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (!isknocked) {
            rb.velocity = new Vector2(xVelocity, yVelocity);
        }
    }

    public IEnumerator FreezedTime(float freezedSeconds) {
        _FreezeTime(true);
        yield return new WaitForSeconds(freezedSeconds);
        _FreezeTime(false);
    }

    private void _FreezeTime(bool isFreezing){
        if (isFreezing)
        {
            animator.speed = 0;
            moveSpeed = 0;
        }
        else {
            animator.speed = 1;
            moveSpeed = deafultMoveSpeed;
        }
    }

    public void Flip()
    {
        faceDirection = (faceDirection == Direction.Dir.Left) ? Direction.Dir.Right : Direction.Dir.Left;
        transform.Rotate(0, 180, 0);
        wallCheckDistance = -wallCheckDistance;
        playerCheckDistance = -playerCheckDistance;
        attackCheck.transform.Rotate(0, 180, 0);
        SetVelocity(-rb.velocity.x, rb.velocity.y);
    }

    public virtual void Damaged(Direction.Dir attackDirection) {
        Debug.Log(gameObject.name + " was damgaed!");
        fx.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockback(attackDirection));

    }

    //∑≠◊™øÿ÷∆∆˜
    public virtual void FlipController()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance, playerCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    protected virtual IEnumerator HitKnockback(Direction.Dir attackDirection)
    {
        SetVelocity(knockBackVelocity.x * (int)attackDirection, knockBackVelocity.y);
        isknocked = true;
        yield return new WaitForSeconds(KnockBackDuration);
        isknocked = false;
        SetVelocity(0,0);
    }

    public virtual void OpenAttackCounterWindow() 
    { 
        canBeStunned = true;
    }

    public virtual void CloseAttackCounterWindow()
    {
        canBeStunned = false;
    }

    public virtual bool CheckAndTurnStunned()
    {
        if (canBeStunned) {
            TurnStunned();
            return true;
        }
        return false;
    }

    public virtual void TurnStunned()
    {
        //Ω¯»Î—£‘Œ◊¥Ã¨
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layerMask_Ground);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, layerMask_Ground);
    public RaycastHit2D GetPLayerDetected() => Physics2D.Raycast(playerCheck.position,Vector2.right,playerCheckDistance, layerMask_Player);
    public bool IsPLayerDetected() => Physics2D.Raycast(playerCheck.position,Vector2.right,playerCheckDistance, layerMask_Player);
    public bool IsAttackDetected() => Physics2D.OverlapCircle(attackCheck.transform.position,attackCheckRadius,layerMask_Player);
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
