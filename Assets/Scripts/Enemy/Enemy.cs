using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System.Threading.Tasks;
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
    [SerializeField] protected Vector2 damagedKnockBackVelocity;
    [SerializeField] protected float damagedKnockBackDuration;
    [SerializeField] private bool isknocked;
    #endregion

    #region Stuned Info
    [Header("Stuned Info")]
    [SerializeField] protected Vector2 stunnedKnockBackVelocity;
    [SerializeField] protected float stunnedKnockBackDuration;
    [SerializeField] public float stunnedDuration;
    [SerializeField] public float stunnedTimer;
    [SerializeField] public bool canBeStunned;

    #endregion

    #region Freezed Info
    [Header("Freezed Info")]
    [SerializeField] public bool isFreezed;

    #endregion
    public virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        fx = GetComponent<EntityFX>();

        groundCheck = transform.Find("GroundCheck");
        wallCheck = transform.Find("WallCheck");
        playerCheck = transform.Find("PlayerCheck");
        attackCheck = transform.Find("AttackCheck");
    }
    public virtual void Start()
    {
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

    public virtual IEnumerator FreezedTime(float freezedSeconds) {
        _FreezeTime(true);
        yield return new WaitForSeconds(freezedSeconds);
        _FreezeTime(false);
    }

    public virtual void _FreezeTime(bool isFreezing){
        isFreezed = isFreezing;
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

    public virtual void OpenAttackCounterWindow() 
    { 
        canBeStunned = true;
    }

    public virtual void CloseAttackCounterWindow()
    {
        canBeStunned = false;
    }

    #region UnderAttack
    // ‹ª˜≈–∂®
    public virtual void UnderAttack(string state,Direction.Dir attackDirection,bool knockback)
    {
        if (state == "stunned")
        {
            TurnStunned(attackDirection, knockback);
        }
        else if (state == "damaged") {
            Damaged(attackDirection, knockback);
        }
        
    }

    protected virtual bool TurnStunned(Direction.Dir attackDirection, bool knockback)
    {
        if (canBeStunned)
        {
            if (knockback)
            {
                StartCoroutine(HitKnockback(attackDirection, stunnedKnockBackVelocity, stunnedKnockBackDuration));
            }
            //Stun
            return true;
        }
        return false;
    }

    protected virtual void Damaged(Direction.Dir attackDirection,bool knockback) {
        fx.StartCoroutine("FlashFX");
        if (knockback) {
            StartCoroutine(HitKnockback(attackDirection,damagedKnockBackVelocity,damagedKnockBackDuration));
        }
    }
    protected virtual IEnumerator HitKnockback(Direction.Dir attackDirection,Vector2 knockBackVelocity, float KnockBackDuration)
    {
        SetVelocity(knockBackVelocity.x * (int)attackDirection, knockBackVelocity.y);
        isknocked = true;
        yield return new WaitForSeconds(KnockBackDuration);
        isknocked = false;
        SetVelocity(0, 0);
    }
    #endregion
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layerMask_Ground);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, layerMask_Ground);
    public RaycastHit2D GetPLayerDetected() => Physics2D.Raycast(playerCheck.position,Vector2.right,playerCheckDistance, layerMask_Player);
    public bool IsPLayerDetected() => Physics2D.Raycast(playerCheck.position,Vector2.right,playerCheckDistance, layerMask_Player);
    public bool IsAttackDetected() => Physics2D.OverlapCircle(attackCheck.transform.position,attackCheckRadius,layerMask_Player);
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
