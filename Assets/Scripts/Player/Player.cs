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
    //���ͼ��
    [SerializeField] public LayerMask layerMask_Ground;
    [SerializeField] public LayerMask layerMask_Enemy;
    //������ײ���
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    //ǽ����ײ���߼��
    [SerializeField] public Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
   
    //��ͨ������ⷶΧ
    [SerializeField] public Transform attack_01_Check;
    [SerializeField] public float attack_01_Check_Radius;

    [SerializeField] public Transform attack_02_Check;
    [SerializeField] public float attack_02_Check_Radius;

    [SerializeField] public Transform attack_03_Check;
    [SerializeField] public float attack_03_Check_Radius;

    //������ⷶΧ
    [SerializeField] public Transform counterAttackCheck;
    [SerializeField] public Vector2 counterAttackBoxSize;  

    //��¼��⵽��ʵ����Ϣ
    public RaycastHit2D hitInfo;
    #endregion

    #region Attack Info
    [Header("Attack Info")]
    //����������
    public int attackComboo;
    //���������ü�ʱ��
    public float combooResetTimer;
    //��������������
    public float combooResetDuration;
    #endregion

    #region PlayerStates
    public PlayerState currentState;
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerRiseState riseState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideDownState wallSlideDownState { get; private set; }
    public PlayerAttack_01_State attack_01_State { get; private set; }
    public PlayerAttack_02_State attack_02_State { get; private set; }
    public PlayerAttack_03_State attack_03_State { get; private set; }
    public PlayerDefenceState defenceState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerUseSkillState_Clone useSkill_Clone { get; private set; }
    public PlayerCatchSowrdState catchSwordState { get; private set; }
    public PlayerThrowSwordState throwSwordState { get; private set; } 
    public PlayerPrepareUseSkillState prepareUseSkillState { get; private set; }
    public PlayerUseSkillState_BlackHole useSkillState_BlackHole { get; private set; }
    public PlayerUseSkillState_Crystal useSkillState_Crystal { get; private set; }
    public PlayerDeadState playerDeathState { get; private set; }
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


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move"); 
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        riseState = new PlayerRiseState(this, stateMachine, "IsAir");
        fallState = new PlayerFallState(this, stateMachine, "IsAir");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        attack_01_State = new PlayerAttack_01_State(this, stateMachine, "Attack");
        attack_02_State = new PlayerAttack_02_State(this, stateMachine, "Attack");
        attack_03_State = new PlayerAttack_03_State(this, stateMachine, "Attack");
        wallSlideDownState = new PlayerWallSlideDownState(this, stateMachine, "WallSlide");
        defenceState = new PlayerDefenceState(this, stateMachine, "Defence");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        useSkill_Clone = new PlayerUseSkillState_Clone(this, stateMachine, "UseCloneSkill");
        catchSwordState = new PlayerCatchSowrdState(this, stateMachine, "CatchSword");
        throwSwordState = new PlayerThrowSwordState(this, stateMachine, "ThrowSword");
        useSkillState_BlackHole = new PlayerUseSkillState_BlackHole(this, stateMachine, "UseBlackHoleSkill");
        prepareUseSkillState = new PlayerPrepareUseSkillState(this, stateMachine, "PrepareUseSkill");
        useSkillState_Crystal = new PlayerUseSkillState_Crystal(this, stateMachine, "LuanchCrystal");
        playerDeathState = new PlayerDeadState(this, stateMachine, "Die");
    }
    protected override void Start()
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

    protected override void Update()
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

    //��ת������
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
    public override IEnumerator HitKnockback(Direction.Dir attackDirection, Vector2 knockBackVelocity, float KnockBackDuration) {
        //���������ٶȵ��ǲ���ı���ҵķ���
        rb.velocity = new Vector2(knockBackVelocity.x * (int)attackDirection, knockBackVelocity.y);
        isknocked = true;
        yield return new WaitForSeconds(KnockBackDuration);
        isknocked = false;
        SetVelocity(0, 0);
    }

    #region UnderAttack
    //�ܻ��ж�
    public override void UnderAttack(string state, Direction.Dir attackDirection, bool knockback)
    {
        base.UnderAttack(state, attackDirection, knockback);
    }

    public override bool TurnStunned(Direction.Dir attackDirection, bool knockback)
    {
        //����ѣ��״̬
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

    public override void Damaged(Direction.Dir attackDirection, bool knockback)
    {
        if (isDenfend && faceDirection != attackDirection)
        {
            fx.StartCoroutine("DefenceFX");
        }
        else
        {
            fx.StartCoroutine("FlashFX");
            StartCoroutine(HitKnockback(attackDirection,damagedknockBackVelocity,damagedKnockBackDuration));
        }
    }

    public override void Die()
    {
        stateMachine.ChangeState(playerDeathState);
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

