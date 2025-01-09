using System.Collections;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;
using System.Threading.Tasks;
public class Player : Entity
{
    #region Move and Jump Info
    [Header("Move and Jump Info")]
    public float jumpForce;
    public float moveSpeed;
    public int jumpTimes;
    public Direction.Dir faceDirection;
    public float defaultGravityScale;

    #endregion

    #region Wallslide Info
    [Header("Wallslide Info")]
    public float WallSlideFallSpeed;
    #endregion

    #region Componets
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public new Collider2D collider2D { get; private set; }
    #endregion

    #region CollidingCheck
    [Header("CollidingCheck")]
    //检测图层
    [SerializeField] public LayerMask layerMask_Ground;
    [SerializeField] public LayerMask layerMask_Enemy;
    //地面碰撞检测
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    //墙面碰撞射线检测
    [SerializeField] public Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
   
    //普通攻击检测范围
    [SerializeField] public Transform attack_01_Check;
    [SerializeField] public float attack_01_Check_Radius;

    [SerializeField] public Transform attack_02_Check;
    [SerializeField] public float attack_02_Check_Radius;

    [SerializeField] public Transform attack_03_Check;
    [SerializeField] public float attack_03_Check_Radius;

    //反击检测范围
    [SerializeField] public Transform counterAttackCheck;
    [SerializeField] public Vector2 counterAttackBoxSize;  

    //记录检测到的实体信息
    public RaycastHit2D hitInfo;
    #endregion

    #region Attack Info
    [Header("Attack Info")]
    //攻击连击数
    public int attackComboo;
    //连击数重置计时器
    public float combooResetTimer;
    //连击数重置周期
    public float combooResetDuration;
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
    public DefenceState defenceState { get; private set; }
    public CounterAttackState counterAttackState { get; private set; }
    public UseSkillState_Clone useSkill_Clone { get; private set; }
    public CatchSowrdState catchSwordState { get; private set; }
    public ThrowSwordState throwSwordState { get; private set; } 
    public PrepareUseSkillState prepareUseSkillState { get; private set; }
    public UseSkillState_BlackHole useSkillState_BlackHole { get; private set; }
    #endregion

    #region Effect
    public EntityFX fx { get; private set; }
    #endregion

    #region was Hurt
    [Header("Hurt Info")]
    [SerializeField] protected Vector2 damagedknockBackVelocity;
    [SerializeField] protected float damagedKnockBackDuration;
    private bool isknocked;
    #endregion

    #region was Stunned
    [Header("Hurt Info")]
    [SerializeField] protected Vector2 stunnedknockBackVelocity;
    [SerializeField] protected float stunnedKnockBackDuration;
    private bool canBeStunned;
    #endregion
    #region Defence
    [Header("Defence Info")]
    public bool isDenfend;
    [SerializeField] public float defenceCDTimer;
    [SerializeField] public float defenceCDDuration;

    #endregion

    #region Counter Attack Info
    #endregion

    #region Black Hole Info
    [Header("Black Hole Info")]
    [SerializeField] public float blackHoleFlySpeed;
    [SerializeField] public float blackHoleDropSpeed;
    [SerializeField] public float blackHoleFlyDuration;
    [SerializeField] public float blackHoleFlyTimer;
    #endregion


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new IdleState(this,stateMachine,"Idle");
        moveState = new MoveState(this, stateMachine, "Move"); 
        moveState = new MoveState(this, stateMachine, "Move");
        riseState = new RiseState(this, stateMachine, "IsAir");
        fallState = new FallState(this, stateMachine, "IsAir");
        dashState = new DashState(this, stateMachine, "Dash");
        attack_01_State = new Attack_01_State(this, stateMachine, "Attack");
        attack_02_State = new Attack_02_State(this, stateMachine, "Attack");
        attack_03_State = new Attack_03_State(this, stateMachine, "Attack");
        wallSlideDownState = new WallSlideDownState(this, stateMachine, "WallSlide");
        defenceState = new DefenceState(this, stateMachine, "Defence");
        counterAttackState = new CounterAttackState(this, stateMachine, "CounterAttack");
        useSkill_Clone = new UseSkillState_Clone(this, stateMachine, "UseCloneSkill");
        catchSwordState = new CatchSowrdState(this, stateMachine, "CatchSword");
        throwSwordState = new ThrowSwordState(this, stateMachine, "ThrowSword");
        useSkillState_BlackHole = new UseSkillState_BlackHole(this, stateMachine, "UseBlackHoleSkill");
        prepareUseSkillState = new PrepareUseSkillState(this, stateMachine, "PrepareUseSkill");
    }
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        fx = GetComponent<EntityFX>();

        stateMachine.Initialize(idleState);
        groundCheck = transform.Find("GroundCheck");
        wallCheck = transform.Find("WallCheck");
        attack_01_Check = transform.Find("Attack01Check");
        attack_02_Check = transform.Find("Attack02Check");
        attack_03_Check = transform.Find("Attack03Check");
        counterAttackCheck = transform.Find("CounterAttackCheck");
        defaultGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        animator.SetFloat("YVlocity", rb.velocity.y);
        animator.SetInteger("AttackComboo", attackComboo);
        stateMachine.currentState.Update();
        currentState = stateMachine.GetCurrentState();
    }
    public void SetVelocity(float xVelocity,float yVelocity) {
        if (!isknocked)
        {
            rb.velocity = new Vector2(xVelocity, yVelocity);
            FlipController(xVelocity);
        }
    }

    public void Flip()
    {
        faceDirection = (faceDirection == Direction.Dir.Left) ? Direction.Dir.Right : Direction.Dir.Left;
        transform.Rotate(0, 180, 0);
        wallCheckDistance = -wallCheckDistance;
        attack_01_Check.transform.Rotate(0, 180, 0);
        attack_02_Check.transform.Rotate(0, 180, 0);
        attack_03_Check.transform.Rotate(0, 180, 0);
    }

    //翻转控制器
    public void FlipController(float xVelocity)
    {
        if (xVelocity > 0.001 && faceDirection != Direction.Dir.Right)
        {
            Flip();
        }
        else if (xVelocity < -0.001 && faceDirection != Direction.Dir.Left)
        {
            Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attack_01_Check.position, attack_01_Check_Radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attack_02_Check.position, attack_02_Check_Radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attack_03_Check.position, attack_03_Check_Radius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(counterAttackCheck.position, counterAttackBoxSize);
    }
    protected virtual IEnumerator HitKnockback(Direction.Dir attackDirection, Vector2 knockBackVelocity, float KnockBackDuration) {
        //这里设置速度但是不想改变玩家的方向
        rb.velocity = new Vector2(knockBackVelocity.x * (int)attackDirection, knockBackVelocity.y);

        isknocked = true;
        yield return new WaitForSeconds(KnockBackDuration);
        isknocked = false;
        SetVelocity(0, 0);
    }

    #region UnderAttack
    //受击判定
    public virtual void UnderAttack(string state, Direction.Dir attackDirection, bool knockback)
    {
        if (state == "stunned")
        {
            TurnStunned(attackDirection,knockback);
        }
        else if (state == "damaged")
        {
            Damaged(attackDirection, knockback);
        }
    }

    public virtual bool TurnStunned(Direction.Dir attackDirection, bool knockback)
    {
        //进入眩晕状态
        if (canBeStunned)
        {
            if (knockback)
            {
                StartCoroutine(HitKnockback(attackDirection, stunnedknockBackVelocity, stunnedKnockBackDuration));
            }
            //Stun
            return true;
        }
        return false;
    }

    public virtual void Damaged(Direction.Dir attackDirection, bool knockback)
    {
        if (isDenfend && faceDirection != attackDirection)
        {
            fx.StartCoroutine("DefenceFX");
        }
        else
        {
            Debug.Log(gameObject.name + " was damgaed!");
            fx.StartCoroutine("FlashFX");
            StartCoroutine(HitKnockback(attackDirection,damagedknockBackVelocity,damagedKnockBackDuration));
        }
    }

    #endregion

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, layerMask_Ground);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, layerMask_Ground);
    public bool IsAttack_01_Detected() => Physics2D.OverlapCircle(attack_01_Check.transform.position, attack_01_Check_Radius,layerMask_Enemy);
    public bool IsAttack_02_Detected() => Physics2D.OverlapCircle(attack_02_Check.transform.position, attack_02_Check_Radius,layerMask_Enemy);
    public bool IsAttack_03_Detected() => Physics2D.OverlapCircle(attack_03_Check.transform.position, attack_03_Check_Radius,layerMask_Enemy);
    public bool IsCounterAttackDetected() => Physics2D.OverlapBox(counterAttackCheck.transform.position,counterAttackBoxSize,layerMask_Enemy);
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}

