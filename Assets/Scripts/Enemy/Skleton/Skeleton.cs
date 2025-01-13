using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Skeleton : Enemy
{
    #region Skeleton State
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonReactState reactState { get; private set; }
    public SkeletonTraceState traceState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonHurtState hurtState { get; private set;}
    public SkeletonStunnedState stunnedState { get; private set;}
    #endregion

    [Header("Patrol Info")]
    #region Patrol Info
    //Ѳ��״̬�����ã�Ѳ�߰�������״̬ ���ú��ƶ�
    //����״̬���л��ļ�ʱ��������2��ʱΪ����״̬��С��2��ʱΪ�ƶ�״̬
    public float patrolTime;
    public float patrolTimer;
    #endregion

    [Header("Guard Info")]
    #region Guard Info
    //����״̬������׷����ҵ��ٶ�,���������ĳ���ʱ��
    public float traceSpeed;
    public float defaultTraceSpeed;
    public float GuardTime;
    public float GuardTimer;
    public float GuardDistance;
    #endregion

    [Header("Attack Info")]
    #region Attack Info
    //���ý��빥��״̬�µ�����Ϊ����Ҽ�����С��Xm
    public float attackDistance;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle");
        moveState = new SkeletonMoveState(this, stateMachine, "Move");
        reactState = new SkeletonReactState(this, stateMachine, "React");
        traceState = new SkeletonTraceState(this, stateMachine, "Trace");
        attackState = new SkeletonAttackState(this, stateMachine, "Attack");
        hurtState = new SkeletonHurtState(this, stateMachine, "Hurt");
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned");
    }

    protected override void Start() {
        base.Start();
        stateMachine.Initialize(idleState);
        defaultTraceSpeed = traceSpeed;
    }

    protected override void Update()
    {
        base.Update();
    }
    public override void FlipController()
    {
        base.FlipController();

        if ((!IsGroundDetected() || IsWallDetected()) && (rb.velocity.y < 0.001 && rb.velocity.y > -0.001))
        {
            Flip();
        }
    }



    public override IEnumerator FreezedTime(float freezedSeconds)
    {
        return base.FreezedTime(freezedSeconds);
    }

    public override void _FreezeTime(bool isFreezing)
    {
        if (isFreezing)
        {
            animator.speed = 0;
            moveSpeed = 0;
            traceSpeed = 0;
            SetVelocity(0, 0);
        }
        else
        {
            animator.speed = 1;
            moveSpeed = deafultMoveSpeed;
            traceSpeed = defaultTraceSpeed;
        }
    }

    public override void UnderAttack(string state, Direction.Dir attackDirection,bool knockback)
    {
        if (state == "stunned")
        {
            TurnStunned(attackDirection,knockback);
        }
        else if (state == "damaged")
        {
            Damaged(attackDirection,knockback);
        }
    }

    public override void Damaged(Direction.Dir attackDirection,bool knockback)
    {
        if (stunnedTimer > 0)
        {
            base.Damaged(attackDirection,knockback);
        }
        else
        {
            stateMachine.ChangeState(hurtState);
            if (knockback)
            {
                StartCoroutine(HitKnockback(attackDirection, damagedKnockBackVelocity,damagedKnockBackDuration));
            }

        }
    }

    public override bool TurnStunned(Direction.Dir attackDirection, bool knockback)
    {
        if (canBeStunned) {
            stateMachine.ChangeState(stunnedState);
            if (knockback) {
                StartCoroutine(HitKnockback(attackDirection, stunnedKnockBackVelocity,stunnedKnockBackDuration));
            }
            return true;
        }
        return false;
        
    }
}
